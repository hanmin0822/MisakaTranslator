using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Security;

namespace MisakaTranslator_WPF
{
    static internal class Program
    {
        static unsafe bool IsElevated()
        {
            HANDLE hToken = HANDLE.Null;
            TOKEN_ELEVATION tokenType = new()
            {
                TokenIsElevated = 0
            };
            if (PInvoke.OpenProcessToken((HANDLE)Process.GetCurrentProcess().Handle, TOKEN_ACCESS_MASK.TOKEN_ALL_ACCESS, &hToken))
            {
                PInvoke.GetTokenInformation(hToken, TOKEN_INFORMATION_CLASS.TokenElevation, &tokenType, (uint)Marshal.SizeOf(tokenType), out var _);
                PInvoke.CloseHandle(hToken);
            }
            return (BOOL)(int)tokenType.TokenIsElevated;
        }

        static unsafe bool CanElevate()
        {
            HANDLE hToken = HANDLE.Null;
            TOKEN_ELEVATION_TYPE tokenType = 0;
            if (PInvoke.OpenProcessToken((HANDLE)Process.GetCurrentProcess().Handle, TOKEN_ACCESS_MASK.TOKEN_ALL_ACCESS, &hToken))
            {
                PInvoke.GetTokenInformation(hToken, TOKEN_INFORMATION_CLASS.TokenElevationType, &tokenType, sizeof(TOKEN_ELEVATION_TYPE), out var _);
                PInvoke.CloseHandle(hToken);
            }
            return tokenType == TOKEN_ELEVATION_TYPE.TokenElevationTypeLimited;
        }

        static bool RunElevate()
        {
            ProcessStartInfo elevationInfo = new()
            {
                FileName = Process.GetCurrentProcess().ProcessName,
                UseShellExecute = true,
                Verb = "runas",
                WorkingDirectory = Environment.CurrentDirectory
            };
            try
            {
                Process.Start(elevationInfo);
                Environment.Exit(0);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [STAThread]
        static void Main()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            if (CanElevate())
            {
                RunElevate();
            }
            App.Main();
        }
    }
}
