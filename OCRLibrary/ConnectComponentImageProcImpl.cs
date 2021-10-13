using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace OCRLibrary
{
    class ConnectComponentImageProcImpl
    {
        private static int[,] ds = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        /// <summary>
        /// 将颜色相近的像素连接成连通分量，根据每个联通分量的性质判断是否是文字
        /// 默认假设浅色文字周围有深色的边框，否则需要反转
        /// </summary>
        /// <param name="b"></param>
        /// <param name="reversed">false:浅色文字，深色边框/背景，true:深色文字，浅色边框/背景</param>
        /// <returns></returns>
        public static Bitmap Process(Bitmap b, bool reversed)
        {
            int width = b.Width;
            int height = b.Height;
            float[,,] pixel = new float[width, height, 3];
            int[,] selected = new int[width, height];
            //把像素值转换成0-1之间的浮点数，存入pixel
            NormalizeBitmap(b, pixel, reversed);
            //将颜色相近的像素合并成色块，并去掉和图片边缘相邻的所有色块（假设文字在图片内部）
            //注：此时还不知道文字是什么颜色的
            //将那些最有可能是文字的色块所包含的像素坐标存入textPixels
            int[,] components = new int[width, height];
            List<(int, int)> textPixels = new List<(int, int)>();
            List<int> validLevels = new List<int>();
            int cur = 1;
            //先遍历边缘，再遍历中间
            for (int x = 0; x < width; x++)
            {
                if (components[x, 0] == 0)
                    VisitComponent(x, 0, cur, components, pixel, textPixels, validLevels, true);
                if (components[x, height - 1] == 0)
                    VisitComponent(x, height - 1, cur, components, pixel, textPixels, validLevels, true);
            }
            for (int y = 1; y < height - 1; y++)
            {
                if (components[0, y] == 0)
                    VisitComponent(0, y, cur, components, pixel, textPixels, validLevels, true);
                if (components[width - 1, y] == 0)
                    VisitComponent(width - 1, y, cur, components, pixel, textPixels, validLevels, true);
            }
            ++cur;
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (components[x, y] == 0)
                    {
                        VisitComponent(x, y, cur, components, pixel, textPixels, validLevels, false);
                        ++cur;
                    }
                }
            }

            //判断文字宽度的范围
            double meanLevel = validLevels.Average(), levelDistSum = 0;
            foreach (var lv in validLevels)
            {
                levelDistSum += (lv - meanLevel) * (lv - meanLevel);
            }
            double levelStddev = Math.Sqrt(levelDistSum / Math.Max(1, validLevels.Count));
            int minLevel = 2, maxLevel = 3, levelThresh = (int)Math.Round(meanLevel + 2.5 * levelStddev);
            foreach (var lv in validLevels)
            {
                if (lv <= levelThresh)
                {
                    maxLevel = Math.Max(maxLevel, lv + 2);
                }
            }
            int minSize = 5;

            //假设上一步找到的像素中大部分都属于文字
            //由于第一轮寻找的条件相对苛刻，很难保证找到所有文字像素，所以我们需要第二轮寻找
            //找出这些像素颜色的近似（加权）几何中位数，以及标准差
            //我们猜测这个几何中位数就是文字的颜色
            float stddiv;
            ColorTuple median = GeometricMedian(textPixels, pixel, out stddiv);
            float radius = Math.Min(1.1f * stddiv, 0.3f);
            radius = Math.Max(radius, 0.1f);
            //把颜色接近几何中位数的所有像素标记为疑似文字像素（标为1），去除其他像素（标为0）
            //标为1的像素之间互相是联通的
            Relabel(pixel, components, median, radius);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //遍历所有可能是文字的色块
                    if (components[x, y] == 1)
                    {
                        List<(int, int)> visited = new List<(int, int)>();
                        bool isBorder = false;
                        List<(int, int)> boundary = ComputeBoundary(median, x, y, 2,
                            pixel, components, visited, out isBorder);
                        //如果当前色块与边界接壤，则舍弃
                        if (!isBorder)
                        {
                            int level = ComputeLevelWithBoundary(boundary, components[x, y], maxLevel, components);
                            //如果当前色块的宽度/大小符合要求，则记录，否则舍弃
                            if ((visited.Count >= minSize || level >= minLevel) && level <= maxLevel)
                            {
                                foreach (var t in visited)
                                {
                                    selected[t.Item1, t.Item2] = 1;
                                }
                            }
                        }
                    }
                }
            }
            //将记录下来的色块标为白色，其他标为黑色
            return SelectedThreshold(b, selected);
        }

        /// <summary>
        /// 把图片中的值（0-255）存为0-1之间的浮点数
        /// </summary>
        /// <param name="b"></param>
        /// <param name="pixel"></param>
        /// <param name="reversed"></param>
        private static void NormalizeBitmap(Bitmap b, float[,,] pixel, bool reversed)
        {
            int width = pixel.GetLength(0);
            int height = pixel.GetLength(1);
            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* p = (byte*)data.Scan0;
                int offset = data.Stride - width * 4;
                if (reversed)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            pixel[x, y, 0] = 1.0f - p[2] / 255.0f;
                            pixel[x, y, 1] = 1.0f - p[1] / 255.0f;
                            pixel[x, y, 2] = 1.0f - p[0] / 255.0f;
                            p += 4;
                        }
                        p += offset;
                    }
                }
                else
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            pixel[x, y, 0] = p[2] / 255.0f;
                            pixel[x, y, 1] = p[1] / 255.0f;
                            pixel[x, y, 2] = p[0] / 255.0f;
                            p += 4;
                        }
                        p += offset;
                    }
                }
                b.UnlockBits(data);
            }
        }

        /// <summary>
        /// 从当前坐标出发，寻找颜色相似的像素，形成色块
        /// 如果shipCheck==false则计算当前色块的宽度，
        /// 并且如果宽度/大小符合要求则把当前色块的像素坐标记录在textPixels，把宽度记录在validLevels中
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cur"></param>
        /// <param name="components"></param>
        /// <param name="pixel"></param>
        /// <param name="textPixels"></param>
        /// <param name="validLevels"></param>
        /// <param name="skipCheck"></param>
        /// <returns></returns>
        private static bool VisitComponent(int x, int y, int cur, int[,] components, float[,,] pixel,
            List<(int, int)> textPixels, List<int> validLevels, bool skipCheck)
        {
            //颜色相似：颜色距离初始像素thresh或距离最近的色块内像素contThresh以内
            float thresh = 0.25f, thresh2 = thresh * thresh;
            float contThresh = 0.15f, c2 = contThresh * contThresh;
            //定义有效宽度的范围，超出范围的则不认为是文字
            int minLevel = 2, maxLevel = 10;
            int minSize = 16;
            Queue<(int, int)> q = new Queue<(int, int)>();
            List<(int, int)> boundary = new List<(int, int)>();
            List<(int, int)> visited = new List<(int, int)>();
            int width = components.GetLength(0);
            int height = components.GetLength(1);
            components[x, y] = cur;       // 假设components[x, y] == 0
            q.Enqueue((x, y));
            //lightEdgeCount记录当前像素为色块边界且当前像素比边界外的像素亮的次数
            int lightEdgeCount = 0, edgeCount = 0, componentSize = 1;

            while (q.Count > 0)
            {
                var t = q.Dequeue();
                if (!skipCheck)
                {
                    visited.Add(t);
                }
                int i = t.Item1, j = t.Item2;
                bool isBoundary = false;
                //遍历上下左右四个方向
                for (int d = 0; d < 4; d++)
                {
                    int nextX = i + ds[d, 0], nextY = j + ds[d, 1];
                    bool inrange = nextX >= 0 && nextX < width && nextY >= 0 && nextY < height;
                    if (inrange && components[nextX, nextY] == 0)
                    {
                        float r = pixel[nextX, nextY, 0], g = pixel[nextX, nextY, 1], b = pixel[nextX, nextY, 2];
                        if (ColorDist2(pixel[x, y, 0], pixel[x, y, 1], pixel[x, y, 2], r, g, b) < thresh2
                           && ColorDist2(pixel[i, j, 0], pixel[i, j, 1], pixel[i, j, 2], r, g, b) < c2)
                        {
                            components[nextX, nextY] = cur;
                            componentSize++;
                            q.Enqueue((nextX, nextY));
                        }
                        else
                        {
                            if (CalcGrey(pixel[i, j, 0], pixel[i, j, 1], pixel[i, j, 2]) > CalcGrey(r, g, b) * 1.1f)
                            {
                                lightEdgeCount++;
                            }
                            edgeCount++;
                        }
                    }
                    if (!inrange || components[nextX, nextY] != cur)
                    {
                        isBoundary = true;
                    }
                }
                if (!skipCheck && isBoundary)
                {
                    boundary.Add((i, j));
                }
            }
            if (!skipCheck && (double)lightEdgeCount / edgeCount > 0.8)
            {
                int level = ComputeLevelWithBoundary(boundary, cur, maxLevel, components);
                if (level > 0 && (level >= minLevel || componentSize >= minSize) && level <= maxLevel)
                {
                    validLevels.Add(level);
                    textPixels.AddRange(visited);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 计算当前坐标所在的色块的边缘，并把当前色块的所有像素的色块标号改为newComp
        /// 如果色块边缘有颜色相近的像素，会适当地延伸色块来包含这些像素
        /// 把当前色块的所有坐标记录在visited中
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="newComp"></param>
        /// <param name="components"></param>
        /// <param name="visited"></param>
        /// <param name="isBorder">是否与图片边缘接壤</param>
        /// <returns></returns>
        private static List<(int, int)> ComputeBoundary(ColorTuple m, int x, int y, int newComp, float[,,] pixel,
            int[,] components, List<(int, int)> visited, out bool isBorder)
        {
            //第二轮的相似条件比第一轮宽松，因为我们已经找到了文字的颜色和大致的位置
            float thresh = 0.35f, thresh2 = thresh * thresh;
            float contThresh = 0.15f, c2 = contThresh * contThresh;
            List<(int, int)> boundary = new List<(int, int)>();
            Queue<(int, int)> q = new Queue<(int, int)>();
            int width = components.GetLength(0);
            int height = components.GetLength(1);
            q.Enqueue((x, y));
            visited.Add((x, y));
            int cur = components[x, y];
            components[x, y] = newComp;
            isBorder = false;
            while (q.Count > 0)
            {
                var t = q.Dequeue();
                int xx = t.Item1, yy = t.Item2;
                if (xx == 0 || xx == width - 1 || yy == 0 || yy == height - 1)
                {
                    isBorder = true;
                }
                bool isBoundary = false;
                for (int d = 0; d < 4; d++)
                {
                    int nextX = xx + ds[d, 0], nextY = yy + ds[d, 1];
                    bool inrange = nextX >= 0 && nextX < width && nextY >= 0 && nextY < height;
                    if (inrange && components[nextX, nextY] != cur && components[nextX, nextY] != newComp
                        && ColorDist2(m.r, m.g, m.b,
                        pixel[nextX, nextY, 0], pixel[nextX, nextY, 1], pixel[nextX, nextY, 2]) < thresh2
                        && ColorDist2(pixel[xx, yy, 0], pixel[xx, yy, 1], pixel[xx, yy, 2],
                        pixel[nextX, nextY, 0], pixel[nextX, nextY, 1], pixel[nextX, nextY, 2]) < c2)
                    {
                        components[nextX, nextY] = cur; //适当延伸当前色块
                    }
                    if (!inrange || (components[nextX, nextY] != cur && components[nextX, nextY] != newComp))
                    {
                        isBoundary = true; //标记边界
                    }
                    var nextXY = (nextX, nextY);
                    if (inrange && components[nextX, nextY] == cur)
                    {
                        visited.Add(nextXY);
                        components[nextX, nextY] = newComp;
                        q.Enqueue(nextXY);
                    }
                }
                if (!isBorder && isBoundary)
                {
                    boundary.Add((xx, yy));
                }
            }
            return boundary;
        }

        /// <summary>
        /// 假设一个色块（S）的边缘的坐标已经在boundary中，计算当前色块的宽度L（S）
        /// 如果L（S）> maxLevel, 则返回maxLevel+1
        /// 如果把像素的坐标表示为x
        /// 定义D（x)：从当前像素x出发，走到到其所在的色块之外的最短步数
        /// 定义宽度L（S）：max({ D(x_i) | x_i 属于 S})
        /// </summary>
        /// <param name="boundary">边缘坐标</param>
        /// <param name="cur">当前色块的标号</param>
        /// <param name="maxLevel">最大宽度</param>
        /// <param name="components">像素到色块的映射</param>
        /// <returns></returns>
        private static int ComputeLevelWithBoundary(List<(int, int)> boundary, int cur, int maxLevel, int[,] components)
        {
            List<(int, int)> nextBoundary = new List<(int, int)>();
            int width = components.GetLength(0);
            int height = components.GetLength(1);
            int level = 0;
            for (int i = 0; i < boundary.Count; i++)
            {
                int xx = boundary[i].Item1, yy = boundary[i].Item2;
                components[xx, yy] = -cur; //标记为已访问
            }
            while (boundary.Count > 0 && level <= maxLevel)
            {
                level++;
                for (int i = 0; i < boundary.Count; i++)
                {
                    int xx = boundary[i].Item1, yy = boundary[i].Item2;
                    for (int d = 0; d < 4; d++)
                    {
                        int nextX = xx + ds[d, 0], nextY = yy + ds[d, 1];
                        if (nextX >= 0 && nextX < width && nextY >= 0 && nextY < height
                            && components[nextX, nextY] == cur)
                        {
                            components[nextX, nextY] = -cur; //标记为已访问
                            nextBoundary.Add((nextX, nextY)); //把下一层入队
                        }
                    }
                }
                boundary.Clear();
                //交换boundary和nextBoundary
                var temp = boundary;
                boundary = nextBoundary;
                nextBoundary = temp;
            }
            return level;
        }
        /// <summary>
        /// 计算textPixels中像素颜色的加权几何中心，权值与到中心的距离成反比
        /// </summary>
        /// <param name="textPixels"></param>
        /// <param name="pixel"></param>
        /// <param name="stddev"></param>
        /// <returns></returns>
        private static ColorTuple GeometricMedian(List<(int, int)> textPixels, float[,,] pixel, out float stddev)
        {
            ColorTuple v = new ColorTuple(0.0f, 0.0f, 0.0f);
            float ep = 0.0001f;
            List<ColorTuple> xs = new List<ColorTuple>();
            foreach (var t in textPixels)
            {
                float r = pixel[t.Item1, t.Item2, 0];
                float g = pixel[t.Item1, t.Item2, 1];
                float b = pixel[t.Item1, t.Item2, 2];
                v.r += r;
                v.g += g;
                v.b += b;
                xs.Add(new ColorTuple(r, g, b));
            }
            int n = Math.Max(1, xs.Count);
            v.r /= n;
            v.g /= n;
            v.b /= n;
            for (int iter = 0; iter < 10; iter++)
            {
                ColorTuple v1 = new ColorTuple(0.0f, 0.0f, 0.0f);
                float sumW = 0.0f;
                for (int i = 0; i < xs.Count; i++)
                {
                    float w = 1.0f / (float)(ep + Math.Sqrt(ColorDist2(v, xs[i])));
                    sumW += w;
                    v1.r += xs[i].r * w;
                    v1.g += xs[i].g * w;
                    v1.b += xs[i].b * w;
                }
                v1.r /= sumW;
                v1.g /= sumW;
                v1.b /= sumW;
                if (v.r == v1.r && v.g == v1.g && v.b == v1.b)
                {
                    break;
                }
                v = v1;
            }
            float sumDist = 0;
            foreach (var x in xs)
            {
                sumDist += ColorDist2(x, v);
            }
            stddev = (float)Math.Sqrt(sumDist / n);
            return v;
        }

        /// <summary>
        /// 把与clr的距离小于radius的所有像素的components值标为1，否则标为0
        /// </summary>
        /// <param name="pixel"></param>
        /// <param name="components"></param>
        /// <param name="clr"></param>
        /// <param name="radius"></param>
        private static void Relabel(float[,,] pixel, int[,] components, ColorTuple clr, float radius)
        {
            float r2 = radius * radius;
            for (int x = 0; x < components.GetLength(0); x++)
            {
                for (int y = 0; y < components.GetLength(1); y++)
                {
                    if (ColorDist2(clr.r, clr.g, clr.b, pixel[x, y, 0], pixel[x, y, 1], pixel[x, y, 2]) < r2)
                    {
                        components[x, y] = 1;
                    }
                    else
                    {
                        components[x, y] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 计算两个颜色之间的距离
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        private static float ColorDist2(ColorTuple c1, ColorTuple c2)
        {
            return ColorDist2(c1.r, c1.g, c1.b, c2.r, c2.g, c2.b);
        }

        /// <summary>
        /// 计算两个颜色之间的距离
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="g1"></param>
        /// <param name="b1"></param>
        /// <param name="r2"></param>
        /// <param name="g2"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private static float ColorDist2(float r1, float g1, float b1, float r2, float g2, float b2)
        {
            float dr = r1 - r2;
            float dg = g1 - g2;
            float db = b1 - b2;
            return dr * dr + dg * dg + db * db;
        }

        /// <summary>
        /// 计算颜色的灰度
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static float CalcGrey(float r, float g, float b)
        {
            return r * 0.299f + g * 0.587f + b * 0.114f;
        }

        /// <summary>
        /// 表示一个颜色
        /// </summary>
        private struct ColorTuple
        {
            public float r;
            public float g;
            public float b;
            public ColorTuple(float r, float g, float b)
            {
                this.r = r;
                this.g = g;
                this.b = b;
            }
        }

        /// <summary>
        /// 按selected进行二值化处理，selected[x,y]==1则颜色变白，非selected颜色变黑
        /// </summary>
        /// <param name="b"></param>
        /// <param name="cluster"></param>
        /// <param name="threshCluster"></param>
        private static Bitmap SelectedThreshold(Bitmap b, int[,] selected)
        {
            int width = b.Width;
            int height = b.Height;
            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* p = (byte*)data.Scan0;
                int offset = data.Stride - width * 4;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        p[0] = p[1] = p[2] = (byte)((selected[x, y] == 1) ? 255 : 0);
                        p += 4;
                    }
                    p += offset;
                }
                b.UnlockBits(data);
            }
            return b;
        }
    }
}
