using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AppEnvironmentLibrary
{
    public static class AppEnvironment
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetCurrentPackageFamilyName(ref int packageFamilyNameLength, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder packageFamilyName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetCurrentPackagePath(ref int pathLength, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder path);

        private static readonly string packageFamilyName;

        public static bool RunWithId { get; private set; }
        public static string LocalFolder { get; private set; }
        public static string TemporaryFolder { get; private set; }
        public static string PackageFolder { get; private set; }

        static AppEnvironment()
        {
            (RunWithId, packageFamilyName) = GetPackageInfo();
            LocalFolder = GetLocalFolder();
            TemporaryFolder = GetTemporaryFolder();
            PackageFolder = GetPacakgeFolder();
        }

        private static (bool, string) GetPackageInfo()
        {
            if (Environment.OSVersion.Version.Build < 9200)
            {
                return (false, string.Empty);
            }
            else
            {
                int length = 0;
                StringBuilder @string = new StringBuilder();
                if (GetCurrentPackageFamilyName(ref length, @string) != 15700)
                {
                    @string.Capacity = length;
                    _ = GetCurrentPackageFamilyName(ref length, @string);
                    return (true, @string.ToString());
                }
                else
                {
                    return (false, string.Empty);
                }
            }
        }

        private static string GetLocalFolder()
        {
            if (RunWithId)
            {
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return $@"{localAppData}\Packages\{packageFamilyName}\LocalState";
            }
            else
            {
                return Environment.CurrentDirectory;
            }
        }

        private static string GetTemporaryFolder()
        {
            if (RunWithId)
            {
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return $@"{localAppData}\Packages\{packageFamilyName}\TempState";
            }
            else
            {
                return Environment.CurrentDirectory;
            }
        }

        private static string GetPacakgeFolder()
        {
            if (RunWithId)
            {
                int length = 0;
                StringBuilder @string = new StringBuilder();
                _ = GetCurrentPackagePath(ref length, @string);
                @string.Capacity = length;
                _ = GetCurrentPackagePath(ref length, @string);
                return @string.ToString();
            }
            else
            {
                return Environment.CurrentDirectory;
            }
        }
    }
}
