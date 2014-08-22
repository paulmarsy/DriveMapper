using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace DriveMapper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            foreach (var mappedDriveConfig in MappedDriveConfig.GetAllConfigItems())
            {
                try
                {
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

                    SetNetworkDriveLabel(mappedDriveConfig.Share, mappedDriveConfig.Label);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error: " + e.Message);
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
