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
            if (PInvoke.OpenProcessToken(Process.GetCurrentProcess().SafeHandle, TOKEN_ACCESS_MASK.TOKEN_ALL_ACCESS, out var hToken))
            {
                TOKEN_ELEVATION tokenType = new();
                if (PInvoke.GetTokenInformation(hToken, TOKEN_INFORMATION_CLASS.TokenElevation, &tokenType, (uint)Marshal.SizeOf(tokenType), out var _))
                {
                    return (BOOL)(int)tokenType.TokenIsElevated;
                }
            }
            return false;
        }

        static unsafe bool CanElevate()
        {
            if (PInvoke.OpenProcessToken(Process.GetCurrentProcess().SafeHandle, TOKEN_ACCESS_MASK.TOKEN_ALL_ACCESS, out var hToken))
            {
                TOKEN_ELEVATION_TYPE tokenType = new();
                if (PInvoke.GetTokenInformation(hToken, TOKEN_INFORMATION_CLASS.TokenElevationType, &tokenType, sizeof(TOKEN_ELEVATION_TYPE), out var _))
                {
                    return tokenType == TOKEN_ELEVATION_TYPE.TokenElevationTypeLimited;
                }
            }
            return false;
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
