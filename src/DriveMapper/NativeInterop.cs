using System;
using System.Runtime.InteropServices;

namespace DriveMapper
{
    public static class NativeInterop
    {
        [DllImport("mpr.dll")]
        public static extern int WNetAddConnection2A(ref NETRESOURCE lpNetResource, string lpPassword, string lpUsername, int dwFlags);
        [DllImport("mpr.dll")]
        public static extern int WNetCancelConnection2A(string lpName, int dwFlags, bool fForce);
        [DllImport("mpr.dll")]
        public static extern int WNetRestoreConnectionW(IntPtr hwndParent, string lpDevice, bool fUseUI);

        public enum ResourceScope
        {
            RESOURCE_CONNECTED = 0x00000001,
            RESOURCE_GLOBALNET = 0x00000002,
            RESOURCE_REMEMBERED = 0x00000003,
            RESOURCE_RECENT = 0x00000004,
            RESOURCE_CONTEXT = 0x00000005
        }

        public enum ResourceType
        {
            RESOURCETYPE_ANY = 0,
            RESOURCETYPE_DISK = 1,
            RESOURCETYPE_PRINT = 2,
            RESOURCETYPE_RESERVED = 8,
            RESOURCETYPE_UNKNOWN = -1,
        }

        public enum ResourceUsage
        {
            RESOURCEUSAGE_CONNECTABLE = 0x00000001,
            RESOURCEUSAGE_CONTAINER = 0x00000002,
            RESOURCEUSAGE_NOLOCALDEVICE = 0x00000004,
            RESOURCEUSAGE_SIBLING = 0x00000008,
            RESOURCEUSAGE_ATTACHED = 0x00000010,
            RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE | RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED),
        }

        public enum ResourceDisplayType
        {
            RESOURCEDISPLAYTYPE_GENERIC = 0x00000000,
            RESOURCEDISPLAYTYPE_DOMAIN = 0x00000001,
            RESOURCEDISPLAYTYPE_SERVER = 0x00000002,
            RESOURCEDISPLAYTYPE_SHARE = 0x00000003,
            RESOURCEDISPLAYTYPE_FILE = 0x00000004,
            RESOURCEDISPLAYTYPE_GROUP = 0x00000005,
            RESOURCEDISPLAYTYPE_NETWORK = 0x00000006,
            RESOURCEDISPLAYTYPE_ROOT = 0x00000007,
            RESOURCEDISPLAYTYPE_SHAREADMIN = 0x00000008,
            RESOURCEDISPLAYTYPE_DIRECTORY = 0x00000009,
            RESOURCEDISPLAYTYPE_TREE = 0x0000000A,
            RESOURCEDISPLAYTYPE_NDSCONTAINER = 0x0000000B
        }

        [Flags]
        public enum Flags
        {
            CONNECT_UPDATE_PROFILE = 0x00000001,
            CONNECT_UPDATE_RECENT = 0x00000002,
            CONNECT_TEMPORARY = 0x00000004,
            CONNECT_INTERACTIVE = 0x00000008,
            CONNECT_PROMPT = 0x00000010,
            CONNECT_REDIRECT = 0x00000080,
            CONNECT_CURRENT_MEDIA = 0x00000200,
            CONNECT_COMMANDLINE = 0x00000800,
            CONNECT_CMD_SAVECRED = 0x00001000,
            CONNECT_CRED_RESET = 0x00002000
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NETRESOURCE
        {
            public ResourceScope dwScope;
            public ResourceType dwType;
            public ResourceDisplayType dwDisplayType;
            public ResourceUsage dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }
    }
}
