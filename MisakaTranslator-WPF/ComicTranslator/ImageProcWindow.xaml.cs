using AppEnvironmentLibrary;
using OCRLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MisakaTranslator_WPF.ComicTranslator
{
    /// <summary>
    /// ImageProcWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ImageProcWindow : Window
    {
        System.Drawing.Bitmap bmp;
        Image img;

        Stack<StrokeCollection> tempList;//操作栈，用于撤销

        DrawingAttributes da;

        public ImageProcWindow(System.Drawing.Bitmap bitmap)
        {
            bmp = bitmap;
            img = new Image();
            da = new DrawingAttributes();

            InitializeComponent();

            tempList = new Stack<StrokeCollection>();
            ink.Strokes.StrokesChanged += Strokes_StrokesChanged;

            img.Width = bitmap.Width;
            img.Height = bitmap.Height;
            ink.Children.Add(img);
            ink.Height = bitmap.Height;
            ink.Width = bitmap.Width;
            ink.EditingMode = InkCanvasEditingMode.None;


            da.Color = Color.FromRgb(255, 255, 255);
            da.Width = 10;
            da.Height = 10;
            da.StylusTip = StylusTip.Ellipse;
        }

        private void EraseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (EraseBtn.IsChecked == true)
            {

                ink.DefaultDrawingAttributes = da;
                ink.EditingMode = InkCanvasEditingMode.Ink;
                ink.UseCustomCursor = true;
            }
            else {
                ink.EditingMode = InkCanvasEditingMode.None;
            }
        }

        private void Ctrl_Z(object sender, RoutedEventArgs e)
        {
            if (tempList.Count > 0)
            {
                ink.Strokes.Remove(tempList.Pop());
            }
        }

        private void Strokes_StrokesChanged(object sender, System.Windows.Ink.StrokeCollectionChangedEventArgs e)
        {
            if (e.Added.Count > 0)
            {
                tempList.Push(e.Added);
            }
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            ImageSave(ink, AppEnvironment.TemporaryFolder + "\\comicTemp.png");
            this.Close();
        }

        private void ImageSave(InkCanvas inkCanvas, string _imageFile)
        {
            double width = inkCanvas.ActualWidth;
            double height = inkCanvas.ActualHeight;
            RenderTargetBitmap bmpCopied = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(inkCanvas);
                dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), new System.Windows.Size(width, height)));
            }
            bmpCopied.Render(dv);
            using (FileStream file = new FileStream(_imageFile, FileMode.Create, FileAccess.Write))
            {
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmpCopied));
                encoder.Save(file);
            }
        }

        private void ThresholdBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            System.Drawing.Bitmap bm = (System.Drawing.Bitmap)bmp.Clone();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            ImageProcFunc.Thresholding(bm, (byte)(int)ThresholdBar.Value).Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            bm.Dispose();
            ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
            img.Source = (ImageSource)imageSourceConverter.ConvertFrom(stream);
        }

        private void InkBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            da.Width = InkBar.Value;
            da.Height = InkBar.Value;
            ink.DefaultDrawingAttributes = da;
        }
    }
}
