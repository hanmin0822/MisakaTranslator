using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MisakaTranslator_WPF
{
    /// <summary>
    /// ScreenCaptureWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenCaptureWindow : Window
    {
        BitmapImage img;
        
        Point iniP;
        private ViewModel viewModel;
        Rect selectRect;

        public static System.Drawing.Rectangle OCRArea;



        public ScreenCaptureWindow(BitmapImage i)
        {
            img = i;
            InitializeComponent();

            imgMeasure.Source = img;

            DrawingAttributes drawingAttributes = new DrawingAttributes
            {
                Color = Colors.Red,
                Width = 2,
                Height = 2,
                StylusTip = StylusTip.Rectangle,
                //FitToCurve = true,
                IsHighlighter = false,
                IgnorePressure = true,

            };
            inkCanvasMeasure.DefaultDrawingAttributes = drawingAttributes;

            viewModel = new ViewModel
            {
                MeaInfo = "按住鼠标左键并拖动鼠标绘制出要识别的区域，确认完成后单击右键退出",
                InkStrokes = new StrokeCollection(),
            };

            DataContext = viewModel;


        }

        private void InkCanvasMeasure_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                iniP = e.GetPosition(inkCanvasMeasure);
            }

            if (e.ChangedButton == MouseButton.Right) {
                this.Close();
            }
        }

        private void InkCanvasMeasure_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Draw square
                System.Windows.Point endP = e.GetPosition(inkCanvasMeasure);
                List<System.Windows.Point> pointList = new List<System.Windows.Point>
                {
                    new System.Windows.Point(iniP.X, iniP.Y),
                    new System.Windows.Point(iniP.X, endP.Y),
                    new System.Windows.Point(endP.X, endP.Y),
                    new System.Windows.Point(endP.X, iniP.Y),
                    new System.Windows.Point(iniP.X, iniP.Y),
                };
                StylusPointCollection point = new StylusPointCollection(pointList);
                Stroke stroke = new Stroke(point)
                {
                    DrawingAttributes = inkCanvasMeasure.DefaultDrawingAttributes.Clone()
                };
                viewModel.InkStrokes.Clear();
                viewModel.InkStrokes.Add(stroke);

                selectRect = new Rect(iniP,endP);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            OCRArea = new System.Drawing.Rectangle((int)selectRect.Location.X, (int)selectRect.Location.Y, (int)selectRect.Size.Width, (int)selectRect.Size.Height);
        }
    }

    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string meaInfo;
        public string MeaInfo
        {
            get => meaInfo;
            set
            {
                meaInfo = value;
                OnPropertyChanged("MeaInfo");
            }
        }

        private StrokeCollection inkStrokes;
        public StrokeCollection InkStrokes
        {
            get { return inkStrokes; }
            set
            {
                inkStrokes = value;
                OnPropertyChanged("InkStrokes");
            }
        }
    }
    
}
