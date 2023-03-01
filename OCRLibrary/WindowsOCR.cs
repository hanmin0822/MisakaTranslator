using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.System;

namespace OCRLibrary
{
    public class WindowsOCR : OCREngine
    {
        public string srcLangCode;
        private OcrEngine rtOcr;

        public override async Task<string> OCRProcessAsync(Bitmap img)
        {
            try
            {
                using var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream();
                img.Save(stream.AsStream(), ImageFormat.Bmp);
                var decoder = await BitmapDecoder.CreateAsync(stream);
                var bitmap = await decoder.GetSoftwareBitmapAsync();
                var res = await rtOcr.RecognizeAsync(bitmap);
                return res.Text;
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return string.Empty;
            }

        }

        public override bool OCR_Init(string param1 = "", string param2 = "")
        {
            try
            {
                Language lang = new(srcLangCode);
                rtOcr = OcrEngine.TryCreateFromLanguage(lang);
                if (rtOcr == null)
                {
                    if (MessageBox.Show($"请在Windows语言设置中添加目标语言并等待OCR组件安装完成。{Environment.NewLine}Please add the target language in Windows Settings and wait for OCR component to be installed.", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error) == MessageBoxResult.OK)
                    {
                        _ = Launcher.LaunchUriAsync(new Uri("ms-settings:regionlanguage-adddisplaylanguage"));
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                return false;
            }
        }

        public override void SetOCRSourceLang(string lang)
        {
            srcLangCode = lang switch
            {
                "jpn" => "ja",
                "eng" => "en",
                _ => lang
            };
        }

        public IReadOnlyList<Language> GetSupportLang()
        {
            return OcrEngine.AvailableRecognizerLanguages;
        }
    }
}
