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
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;

namespace OCRLibrary
{
    public class ImageProcFunc
    {
        public static Dictionary<string, string> lstHandleFun = new Dictionary<string, string>() {
            { "不进行处理" , "ImgFunc_NoDeal" },
            { "OTSU二值化处理" , "ImgFunc_OTSU" },
            { "连通分量二值化处理", "ImgFunc_CC" },
            { "连通分量二值化处理（反转）", "ImgFunc_CC_Reversed" }
        };

        public static Dictionary<string, string> lstOCRLang = new Dictionary<string, string>() {
            { "英语" , "eng" },
            { "日语" , "jpn" },
        };

        /// <summary>
        /// 根据方法名自动处理
        /// </summary>
        /// <param name="b"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Bitmap Auto_Thresholding(Bitmap b, string func) {
            switch (func) {
                case "ImgFunc_NoDeal":
                    return b;
                case "ImgFunc_OTSU":
                    return OtsuThreshold(b);
                case "ImgFunc_CC":
                    return ConnectedComponentThreshold(b, false);
                case "ImgFunc_CC_Reversed":
                    return ConnectedComponentThreshold(b, true);
            }
            return b;
        }


        public static byte[] Image2Bytes(Image img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }


        /// <summary>
        /// 得到图片的Base64编码
        /// </summary>
        /// <param name="img">欲处理的图片</param>
        /// <returns></returns>
        public static string GetFileBase64(Image img)
        {
            return Convert.ToBase64String(Image2Bytes(img));
        }
        
        /// <summary>
        /// 按固定阈值进行二值化处理
        /// </summary>
        /// <param name="b">图片</param>
        /// <param name="thresh">阈值 0-255</param>
        /// <returns></returns>
        public static Bitmap Thresholding(Bitmap b, byte thresh)
        {
            byte threshold = thresh;
            int width = b.Width;
            int height = b.Height;
            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* p = (byte*)data.Scan0;
                int offset = data.Stride - width * 4;
                byte R, G, B, gray;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        R = p[2];
                        G = p[1];
                        B = p[0];
                        gray = (byte)((R * 19595 + G * 38469 + B * 7472) >> 16);

                        if (gray >= threshold)
                        {
                            p[0] = p[1] = p[2] = 255;
                        }
                        else
                        {
                            p[0] = p[1] = p[2] = 0;
                        }
                        p += 4;
                    }
                    p += offset;
                }
                b.UnlockBits(data);
                return b;
            }
        }
        
        /// <summary>
        /// 自动二值化处理-OTSU
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Bitmap OtsuThreshold(Bitmap b)
        {
            // 图像灰度化   
            // b = Gray(b);   
            int width = b.Width;
            int height = b.Height;
            byte threshold = 0;
            int[] hist = new int[256];
            
            int AllPixelNumber = 0, PixelNumberSmall = 0, PixelNumberBig = 0;
            double MaxValue, AllSum = 0, SumSmall = 0, SumBig, ProbabilitySmall, ProbabilityBig, Probability;
            
            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* p = (byte*)data.Scan0;
                int offset = data.Stride - width * 4;
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        hist[p[0]]++;
                        p += 4;
                    }
                    p += offset;
                }
                b.UnlockBits(data);
            }

            //计算灰度为I的像素出现的概率   
            for (int i = 0; i < 256; i++)
            {
                AllSum += i * hist[i];     //   质量矩   
                AllPixelNumber += hist[i];  //  质量    
            }
            
            MaxValue = -1.0;
            for (int i = 0; i < 256; i++)
            {
                PixelNumberSmall += hist[i];
                PixelNumberBig = AllPixelNumber - PixelNumberSmall;
                if (PixelNumberBig == 0)
                {
                    break;
                }
                SumSmall += i * hist[i];
                SumBig = AllSum - SumSmall;
                ProbabilitySmall = SumSmall / PixelNumberSmall;
                ProbabilityBig = SumBig / PixelNumberBig;
                Probability = PixelNumberSmall * ProbabilitySmall * ProbabilitySmall + PixelNumberBig * ProbabilityBig * ProbabilityBig;

                if (Probability > MaxValue)
                {
                    MaxValue = Probability;
                    threshold = (byte)i;
                }
            }

            return Thresholding(b, threshold);
        }

        /// <summary>
        /// 自动二值化处理-连通分量法
        /// </summary>
        /// <param name="b"></param>
        /// <param name="reversed"></param>
        /// <returns></returns>
        public static Bitmap ConnectedComponentThreshold(Bitmap b, bool reversed)
        {
            return ConnectComponentImageProcImpl.Process(b, reversed);
        }

            /// <summary>
            /// 按固定颜色进行二值化处理,即阈值颜色变白，非阈值颜色变黑
            /// </summary>
            /// <param name="b"></param>
            /// <param name="thresh"></param>
            /// <returns></returns>
            public static Bitmap ColorThreshold(Bitmap b, Color thresh)
        {
            int width = b.Width;
            int height = b.Height;
            BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* p = (byte*)data.Scan0;
                int offset = data.Stride - width * 4;
                byte R, G, B, gray;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        R = p[2];
                        G = p[1];
                        B = p[0];
                        
                        if (R == thresh.R && G == thresh.G && B == thresh.B)
                        {
                            p[0] = p[1] = p[2] = 255;
                        }
                        else
                        {
                            p[0] = p[1] = p[2] = 0;
                        }
                        p += 4;
                    }
                    p += offset;
                }
                b.UnlockBits(data);
                return b;
            }
        }

        /// <summary>
        /// 将System.Drawing.Image转换成System.Windows.Media.Imaging.BitmapImage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapImage ImageToBitmapImage(Image bitmap)
        {
            if (bitmap == null) {
                return null;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png); //格式选Bmp时，不带透明度

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }

        }

        /// <summary>
        /// 获取文件图标
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static unsafe Bitmap GetAppIcon(string filepath)
        {
            //选中文件中的图标总数
            var iconTotalCount = PInvoke.PrivateExtractIcons(filepath, 0, 0, 0, null, null, 0, 0);

            //用于接收获取到的图标指针
            Span<HICON> hIcons = stackalloc HICON[(int)iconTotalCount];

            uint result = 0xFFFFFFFF;
            fixed (HICON* p = hIcons)
            {
                //成功获取到的图标个数
                result = PInvoke.PrivateExtractIcons(filepath, 0, 256, 256, p, null, iconTotalCount, (uint)(IMAGE_FLAGS.LR_DEFAULTCOLOR));
            }

            Bitmap myIcon = null;
            if (result > 0 && result != 0xFFFFFFFF)
            {
                using Icon ico = Icon.FromHandle(hIcons[0]);
                myIcon = ico.ToBitmap();
            }

            //遍历并保存图标
            for (var i = 0; i < result; i++)
            {
                //指针为空，跳过
                if (hIcons[i] == HICON.Null) continue;
                //内存回收
                PInvoke.DestroyIcon(hIcons[i]);
            }

            return myIcon;
        }

        /// <summary>
        /// 灰度化，32-8转换
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap ColorToGrayscale(Bitmap bmp)
        {
            int w = bmp.Width,
            h = bmp.Height,
            r, ic, oc, bmpStride, outputStride, bytesPerPixel;
            PixelFormat pfIn = bmp.PixelFormat;
            ColorPalette palette;
            Bitmap output;
            BitmapData bmpData, outputData;

            //Create the new bitmap
            output = new Bitmap(w, h, PixelFormat.Format8bppIndexed);

            //Build a grayscale color Palette
            palette = output.Palette;
            for (int i = 0; i < 256; i++)
            {
                Color tmp = Color.FromArgb(255, i, i, i);
                palette.Entries[i] = Color.FromArgb(255, i, i, i);
            }
            output.Palette = palette;

            //No need to convert formats if already in 8 bit
            if (pfIn == PixelFormat.Format8bppIndexed)
            {
                output = (Bitmap)bmp.Clone();

                //Make sure the palette is a grayscale palette and not some other
                //8-bit indexed palette
                output.Palette = palette;

                return output;
            }

            //Get the number of bytes per pixel
            switch (pfIn)
            {
                case PixelFormat.Format24bppRgb: bytesPerPixel = 3; break;
                case PixelFormat.Format32bppArgb: bytesPerPixel = 4; break;
                case PixelFormat.Format32bppRgb: bytesPerPixel = 4; break;
                default: throw new InvalidOperationException("Image format not supported");
            }

            //Lock the images
            bmpData = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly,
            pfIn);
            outputData = output.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly,
            PixelFormat.Format8bppIndexed);
            bmpStride = bmpData.Stride;
            outputStride = outputData.Stride;

            //Traverse each pixel of the image
            unsafe
            {
                byte* bmpPtr = (byte*)bmpData.Scan0.ToPointer(),
                outputPtr = (byte*)outputData.Scan0.ToPointer();

                if (bytesPerPixel == 3)
                {
                    //Convert the pixel to it's luminance using the formula:
                    // L = .299*R + .587*G + .114*B
                    //Note that ic is the input column and oc is the output column
                    for (r = 0; r < h; r++)
                        for (ic = oc = 0; oc < w; ic += 3, ++oc)
                            outputPtr[r * outputStride + oc] = (byte)(int)
                            (0.299f * bmpPtr[r * bmpStride + ic] +
                            0.587f * bmpPtr[r * bmpStride + ic + 1] +
                            0.114f * bmpPtr[r * bmpStride + ic + 2]);
                }
                else //bytesPerPixel == 4
                {
                    //Convert the pixel to it's luminance using the formula:
                    // L = alpha * (.299*R + .587*G + .114*B)
                    //Note that ic is the input column and oc is the output column
                    for (r = 0; r < h; r++)
                        for (ic = oc = 0; oc < w; ic += 4, ++oc)
                            outputPtr[r * outputStride + oc] = (byte)(int)
                            ((bmpPtr[r * bmpStride + ic] / 255.0f) *
                            (0.299f * bmpPtr[r * bmpStride + ic + 1] +
                            0.587f * bmpPtr[r * bmpStride + ic + 2] +
                            0.114f * bmpPtr[r * bmpStride + ic + 3]));
                }
            }

            //Unlock the images
            bmp.UnlockBits(bmpData);
            output.UnlockBits(outputData);

            return output;
        }
    }
}
