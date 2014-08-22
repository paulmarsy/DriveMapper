using System;

namespace DriveMapper
{
    public class NetworkDrive
    {
        public bool SaveCredentials { get; set; }

        public bool Persistent { get; set; }

        public bool Force { get; set; }

        public string LocalDrive { get; set; }

        public string ShareName { get; set; }

        public void MapDrive()
        {
            MapDrive(null, null);
        }

        public void MapDrive(string username, string password)
        {
            var netResource = new NativeInterop.NETRESOURCE
            {
                dwScope = NativeInterop.ResourceScope.RESOURCE_GLOBALNET,
                dwType = NativeInterop.ResourceType.RESOURCETYPE_DISK,
                dwDisplayType = NativeInterop.ResourceDisplayType.RESOURCEDISPLAYTYPE_SHARE,
                dwUsage = NativeInterop.ResourceUsage.RESOURCEUSAGE_CONNECTABLE,
                lpRemoteName = ShareName,
                lpLocalName = LocalDrive + ":"
            };

            NativeInterop.Flags dwFlags = 0;
            if (SaveCredentials) { dwFlags = dwFlags | NativeInterop.Flags.CONNECT_CMD_SAVECRED; }
            if (Persistent) { dwFlags = dwFlags | NativeInterop.Flags.CONNECT_UPDATE_PROFILE; }
            
            if (Force) { try { UnMapDrive(true); } catch { } }
            
            var result = NativeInterop.WNetAddConnection2A(ref netResource, password, username, (int)dwFlags);
            if (result > 0) { throw new System.ComponentModel.Win32Exception(result); };
        }

        public void UnMapDrive()
        {
            UnMapDrive(Force);
        }

        private void UnMapDrive(bool force)
        {
            NativeInterop.Flags dwFlags = 0;
            if (Persistent) { dwFlags = dwFlags | NativeInterop.Flags.CONNECT_UPDATE_PROFILE; }

            var result = NativeInterop.WNetCancelConnection2A(LocalDrive + ":", (int)dwFlags, force);
            if (result > 0) { throw new System.ComponentModel.Win32Exception(result); }
        }

        public void RestoreAllDrives()
        {
            RestoreDrive(null);
        }

        public void RestoreDrive()
        {
            RestoreDrive(LocalDrive + ":");
        }
        private void RestoreDrive(string drive)
        {
            var result = NativeInterop.WNetRestoreConnectionW(IntPtr.Zero, drive, false);
            if (result > 0) { throw new System.ComponentModel.Win32Exception(result); }
        }
    }
}
