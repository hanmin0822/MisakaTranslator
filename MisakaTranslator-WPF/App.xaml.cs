using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MisakaTranslator_WPF
{
    public partial class App
    {
        public App()
        {
            //注册开始和退出事件
            this.Startup += new StartupEventHandler(App_Startup);
            this.Exit += new ExitEventHandler(App_Exit);
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            //UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        void App_Exit(object sender, ExitEventArgs e)
        {
            //程序退出时需要处理的业务
        }

        /// <summary>
        /// UI线程未捕获异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string fn = System.DateTime.Now.ToString("g");
            PrintErrorMessageToFile(fn,e.Exception,0);
            MessageBox.Show(App.Current.Resources["App_Global_ErrorHint_left"].ToString() + fn + App.Current.Resources["App_Global_ErrorHint_right"].ToString()
                , App.Current.Resources["MessageBox_Error"].ToString());
        }

        /// <summary>
        /// 非UI线程未捕获异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string fn = System.DateTime.Now.ToString("g");
            if (e.ExceptionObject is Exception)
            {
                PrintErrorMessageToFile(fn,(Exception)e.ExceptionObject, 1);
            }
            else
            {
                PrintErrorMessageToFile(fn,null, 1, e.ExceptionObject.ToString());
            }

            MessageBox.Show(App.Current.Resources["App_Global_ErrorHint_left"].ToString() + fn + App.Current.Resources["App_Global_ErrorHint_right"].ToString()
                , App.Current.Resources["MessageBox_Error"].ToString());
        }

        /// <summary>
        /// Task线程内未捕获异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            string fn = System.DateTime.Now.ToString("g");
            PrintErrorMessageToFile(fn,e.Exception, 2);
            MessageBox.Show(App.Current.Resources["App_Global_ErrorHint_left"].ToString() + fn + App.Current.Resources["App_Global_ErrorHint_right"].ToString()
                , App.Current.Resources["MessageBox_Error"].ToString());
        }

        /// <summary>
        /// 打印错误信息到文本文件
        /// </summary>
        /// <param name="e"></param>
        private void PrintErrorMessageToFile(string fileName,Exception e,int exceptionThread,string ErrorMessage = null) {
            if (Directory.Exists(Environment.CurrentDirectory + "\\logs") == false)
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\logs");
            }
            FileStream fs = new FileStream(Environment.CurrentDirectory + "\\logs\\" + fileName + ".txt", FileMode.Create);
            
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine("==============System Info================");
            sw.WriteLine("System:" + Environment.OSVersion.ToString());
            sw.WriteLine("CurrentTime:" + System.DateTime.Now.ToString("g"));
            sw.WriteLine("dotNetVersion:" + Environment.Version.ToString());

            if (ErrorMessage != null)
            {
                sw.WriteLine("==============Exception Info================");
                sw.WriteLine("ExceptionType:" + "Non UI Thread But not Exception");
                sw.WriteLine("ErrorMessage:" + ErrorMessage);
            }
            else
            {
                sw.WriteLine("==============Exception Info================");
                if (exceptionThread == 0)
                {
                    sw.WriteLine("ExceptionType:" + "UI Thread");
                }
                else if (exceptionThread == 1)
                {
                    sw.WriteLine("ExceptionType:" + "Non UI Thread");
                }
                else if (exceptionThread == 2)
                {
                    sw.WriteLine("ExceptionType:" + "Task Thread");
                }

                sw.WriteLine("ExceptionSource:" + e.Source);
                sw.WriteLine("ExceptionMessage:" + e.Message);
                sw.WriteLine("ExceptionStackTrace:" + e.StackTrace);
            }

            
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        
    }
}
