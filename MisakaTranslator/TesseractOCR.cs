/*
 *Namespace         MisakaTranslator
 *Class             TesseractOCR
 *Description       TesseractOCR处理类，使用OpenCV+Tesseract实现
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace MisakaTranslator
{
    class TesseractOCR
    {
        public static int thresh;//阈值:0-255
        public static string srcLangCode;//OCR识别语言 jpn=日语 eng=英语
        private static TesseractEngine OCR;
        
        public static void TesseractOCR_Init()
        {
            OCR = new TesseractEngine(Environment.CurrentDirectory + "\\tessdata", srcLangCode, EngineMode.Default);
        }
        
        /// <summary>
        /// TesseractOCR处理，得到结果
        /// </summary>
        /// <param name="img">欲OCR的图片</param>
        /// <returns></returns>
        public static string OCRProcess(Bitmap img)
        {
            var page = OCR.Process(img);
            string res = page.GetText();
            page.Dispose();
            return res;
        }

        /// <summary>
        /// 图片二值化处理，以thresh作为阈值
        /// </summary>
        /// <param name="img">输入原图</param>
        /// <returns>得到二值化以后的图片</returns>
        public static Bitmap Thresholding(Bitmap b)
        {
            byte threshold = (byte)thresh;
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
    }
}
