using System;
using System.IO;
using System.Reflection;
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
            this.Startup += App_Startup;
            this.Exit += App_Exit;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            //UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            //程序退出时检查是否断开Hook
            DoHookCheck();


        }

        /// <summary>
        /// UI线程未捕获异常处理事件
        /// </summary>
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MisakaTranslator_WPF.MainWindow.Instance.CloseNotifyIcon();
            string fn = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            PrintErrorMessageToFile(fn, e.Exception, 0);
            DoHookCheck();
            MessageBox.Show($"{Current.Resources["App_Global_ErrorHint_left"]}{fn}{Current.Resources["App_Global_ErrorHint_right"]}"
                , Current.Resources["MessageBox_Error"].ToString());
        }

        /// <summary>
        /// 非UI线程未捕获异常处理事件
        /// </summary>
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MisakaTranslator_WPF.MainWindow.Instance.CloseNotifyIcon();
            string fn = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            if (e.ExceptionObject is Exception exception)
            {
                PrintErrorMessageToFile(fn, exception, 1);
            }
            else
            {
                PrintErrorMessageToFile(fn, null, 1, e.ExceptionObject.ToString());
            }

            DoHookCheck();
            MessageBox.Show($"{Current.Resources["App_Global_ErrorHint_left"]}{fn}{Current.Resources["App_Global_ErrorHint_right"]}"
                , Current.Resources["MessageBox_Error"].ToString());
        }

        /// <summary>
        /// Task线程内未捕获异常处理事件
        /// </summary>
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            MisakaTranslator_WPF.MainWindow.Instance.CloseNotifyIcon();
            string fn = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            PrintErrorMessageToFile(fn, e.Exception, 2);

            DoHookCheck();
            MessageBox.Show($"{Current.Resources["App_Global_ErrorHint_left"]}{fn}{Current.Resources["App_Global_ErrorHint_right"]}"
                , Current.Resources["MessageBox_Error"].ToString());
        }

        /// <summary>
        /// 打印错误信息到文本文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="e">异常</param>
        /// <param name="exceptionThread">异常线程</param>
        /// <param name="errorMessage">错误消息</param>
        private static void PrintErrorMessageToFile(string fileName, Exception e, int exceptionThread, string errorMessage = null)
        {
            if (!Directory.Exists($"{Environment.CurrentDirectory}\\logs"))
            {
                Directory.CreateDirectory($"{Environment.CurrentDirectory}\\logs");
            }
            FileStream fs = new FileStream($"{Environment.CurrentDirectory}\\logs\\{fileName}.txt", FileMode.Create);

            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine("==============System Info================");
            sw.WriteLine("System:" + Environment.OSVersion);
            sw.WriteLine("CurrentTime:" + DateTime.Now.ToString("g"));
            sw.WriteLine("dotNetVersion:" + Environment.Version);
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            sw.WriteLine("MisakaTranslatorVersion:" + version.ToString());

            if (errorMessage != null)
            {
                sw.WriteLine("==============Exception Info================");
                sw.WriteLine("ExceptionType:" + "Non UI Thread But not Exception");
                sw.WriteLine("ErrorMessage:" + errorMessage);
            }
            else
            {
                sw.WriteLine("==============Exception Info================");
                switch (exceptionThread)
                {
                    case 0:
                        sw.WriteLine("ExceptionType:" + "UI Thread");
                        break;
                    case 1:
                        sw.WriteLine("ExceptionType:" + "Non UI Thread");
                        break;
                    case 2:
                        sw.WriteLine("ExceptionType:" + "Task Thread");
                        break;
                }

                sw.WriteLine("ExceptionName:" + e.GetType());
                sw.WriteLine("ExceptionSource:" + e.Source);
                sw.WriteLine("ExceptionMessage:" + e.Message);
                sw.WriteLine("ExceptionStackTrace:" + e.StackTrace);
                if(e.InnerException != null)
                    sw.WriteLine("InnerExceptionStackTrace:" + e.InnerException);
            }

            sw.Flush();
            sw.Close();
            fs.Close();
        }


        /// <summary>
        /// 执行Hook是否完全卸载的检查
        /// </summary>
        public void DoHookCheck() {
            if (Common.textHooker != null)
            {
                Common.textHooker = null;
                GC.Collect();
            }
        }
    }
}
