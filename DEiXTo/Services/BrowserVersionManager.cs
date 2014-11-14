using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;

namespace DEiXTo.Services
{
    public class BrowserVersionManager : IBrowserVersionManager
    {
        public void UpdateBrowserVersion()
        {
            // FeatureControl settings are per-process
            var fileName = Path.GetFileName(Process.GetCurrentProcess().
                MainModule.FileName);

            SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION",
                fileName, GetBrowserEmulationMode());
        }

        public void ResetBrowserVersion()
        {
            var fileName = Path.GetFileName(Process.GetCurrentProcess().
                MainModule.FileName);
            UInt32 defaultVersion = 7000;
            SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION",
                fileName, defaultVersion);
        }

        private static void SetBrowserFeatureControlKey(string feature,
            string appName, uint value)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(
                String.Concat(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\", feature),
                RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                key.SetValue(appName, (UInt32)value, RegistryValueKind.DWord);
            }
        }

        private static UInt32 GetBrowserEmulationMode()
        {
            int browserVersion = 7;
            using (var ieKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer",
                RegistryKeyPermissionCheck.ReadSubTree,
                RegistryRights.QueryValues))
            {
                var version = ieKey.GetValue("svcVersion");
                if (null == version)
                {
                    version = ieKey.GetValue("Version");
                    if (null == version)
                        throw new ApplicationException("Microsoft Internet Explorer is required!");
                }
                int.TryParse(version.ToString().Split('.')[0], out browserVersion);
            }
            UInt32 mode = 10000;

            return mode;
        }
    }
}
