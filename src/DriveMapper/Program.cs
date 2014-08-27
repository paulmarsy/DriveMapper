using System.Text;
using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace DriveMapper
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            foreach (var mappedDriveConfig in MappedDriveConfig.GetAllConfigItems())
            {
                var log = new StringBuilder();
                try
                {
                    log.AppendFormat("Preparing to map {0} to {1}:\\", mappedDriveConfig.Share, mappedDriveConfig.DriveLetter);
                    var networkDrive = new NetworkDrive
                    {
                        Persistent = mappedDriveConfig.Persistent,
                        LocalDrive = mappedDriveConfig.DriveLetter,
                        ShareName = mappedDriveConfig.Share,
                        SaveCredentials = true,
                        Force = true
                    };
                    if (string.IsNullOrWhiteSpace(mappedDriveConfig.Username) ||
                        string.IsNullOrWhiteSpace(mappedDriveConfig.Password))
                        networkDrive.MapDrive();
                    else
                        networkDrive.MapDrive(mappedDriveConfig.Username, mappedDriveConfig.Password);
                    log.AppendLine("Call to WNetAddConnection2A succeeded, about to rename drive label");

                    SetNetworkDriveLabel(mappedDriveConfig.Share, mappedDriveConfig.Label);

                    log.AppendFormat("Successfully mapped and renamed network drive {0}:\\", mappedDriveConfig.DriveLetter);
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("Error: {0}\n{1}\nInternal log:\n{2}\nStack Trace:\n{3}",
                        e.Message,
                        e.InnerException != null ? e.InnerException.Message : "No Inner Exception",
                        log,
                        e.StackTrace
                        ), string.Format("Mapping network drive {0}:\\ failed", mappedDriveConfig.DriveLetter), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }   
            }
        }

        private static void SetNetworkDriveLabel(string shareNwme, string label)
        {
            var mountKeyName = string.Format(@"Software\Microsoft\Windows\CurrentVersion\Explorer\MountPoints2\{0}", shareNwme.Replace('\\', '#'));
            var mountRegistryKey = Registry.CurrentUser.OpenSubKey(mountKeyName, true);
            mountRegistryKey.SetValue("_LabelFromReg", label, RegistryValueKind.String);
        }
    }
}
