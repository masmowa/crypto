using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetupApi.DeviceInstallation;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Win32.SafeHandles;

namespace Native.Win32.Interop
{
    //namespace SystemDevices
    //{
    #region Structures and Enums
    [StructLayout(LayoutKind.Sequential)]
    public struct SP_BROADCAST_HANDLE {
        public int dbch_size;
        public int dbch_devicetype;
        public int dbch_reserved;
        public IntPtr dbch_handle;
        public IntPtr dbch_hdevnotify;
        public Guid dbch_eventguid;
        public long dbch_nameoffset;
        public byte dbch_data;
        public byte dbch_data1;
    }
    [StructLayout(LayoutKind.Sequential)]
    public sealed class DEV_BROADCAST_DEVICEINTERFACE
    {
        public UInt32 dbcc_size;
        public UInt32 dbcc_devicetype;
        public UInt32 dbcc_reserved;
        public Guid dbcc_classGuid;

        public DEV_BROADCAST_DEVICEINTERFACE()
        {
            dbcc_size = (UInt32)Marshal.SizeOf(this);
        }
        public UInt32 DeviceType { get { return dbcc_devicetype; } }
        //public UInt32 DeviceInstance { get { return devInst; } }
        public UInt32 Size { get { return dbcc_size; } }
    }

    [StructLayout(LayoutKind.Sequential)]
    public sealed class SP_DEVINFO_DATA
    {
        public UInt32 cbSize;
        public Guid classGuid;
        public UInt32 devInst;
        public IntPtr reserved;

        public SP_DEVINFO_DATA()
        {
            cbSize = (UInt32)Marshal.SizeOf(this);
        }
        public Guid SetupClass { get { return classGuid; } }
        public UInt32 DeviceInstance { get { return devInst; } }
        public UInt32 Size { get { return cbSize; } }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public sealed class SP_DEVICE_INTERFACE_DATA
    {
        public UInt32 cbSize;
        public Guid interfaceClassGuid;
        public UInt32 flags;
        public UIntPtr reserved;

        public SP_DEVICE_INTERFACE_DATA()
        {
            cbSize = (UInt32)Marshal.SizeOf(this);
        }
        public Guid InterfaceClass { get { return interfaceClassGuid; } }
        public UInt32 Flags { get { return flags; } }
        public UInt32 Size { get { return cbSize; } }

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SP_DEVICE_INTERFACE_DETAIL_DATA
    {
        public UInt32 cbSize;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string DevicePath;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class SP_DEVINSTALL_PARAMS
    {
        public int cbSize;
        public int Flags;
        public int FlagsEx;
        public IntPtr hwndParent;
        public IntPtr InstallMsgHandler;
        public IntPtr InstallMsgHandlerContext;
        public IntPtr FileQueue;
        public IntPtr ClassInstallReserved;
        public int Reserved;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string DriverPath;
    };

    [StructLayout(LayoutKind.Sequential)]
    public class SP_PROPCHANGE_PARAMS
    {
        public SP_CLASSINSTALL_HEADER ClassInstallHeader = new SP_CLASSINSTALL_HEADER();
        public int StateChange;
        public int Scope;
        public int HwProfile;
    };

    [StructLayout(LayoutKind.Sequential)]
    public class SP_CLASSINSTALL_HEADER
    {
        public int cbSize;
        public int InstallFunction;
    };
    #endregion
    #region Enums
    #endregion
    public static class Win32
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr RegisterDeviceNotification(
            IntPtr hRecipient, 
            DEV_BROADCAST_DEVICEINTERFACE NotificationFilter, 
            UInt32 Flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern UInt32 UnregisterDeviceNotification(IntPtr hHandle);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(
            ref Guid ClassGuid,
            [MarshalAs(UnmanagedType.LPStr)]String Enumerator, 
            IntPtr hwndParent, Int32 Flags);

        [DllImport("setupapi.dll")]
        public static extern IntPtr SetupDiGetClassDevsEx(
            ref Guid ClassGuid,
            [MarshalAs(UnmanagedType.LPStr)]String Enumerator,
            IntPtr hwndParent, Int32 Flags, IntPtr DeviceInfoSet,
            [MarshalAs(UnmanagedType.LPStr)]String MachineName,
            IntPtr Reserved);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern Int32 SetupDiDestroyDeviceInfoList(IntPtr lpInfoSet);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInfo(
            IntPtr lpInfoSet, Int32 
            dwIndex, SP_DEVINFO_DATA devInfoData);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(
            IntPtr lpDeviceInfoSet, 
            ref SP_DEVICE_INTERFACE_DATA oInterfaceData, 
            IntPtr lpDeviceInterfaceDetailData,
            UInt32 nDeviceInterfaceDetailDataSize, 
            out UInt32 nRequiredSize,
            IntPtr lpDeviceInfoData);
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(
            IntPtr lpDeviceInfoSet, 
            ref SP_DEVICE_INTERFACE_DATA oInterfaceData, 
            ref SP_DEVICE_INTERFACE_DETAIL_DATA oDetailData, 
            uint nDeviceInterfaceDetailDataSize, 
            out uint nRequiredSize, 
            IntPtr lpDeviceInfoData);
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern Boolean SetupDiGetDeviceRegistryProperty(
            IntPtr lpInfoSet, 
            SP_DEVINFO_DATA DeviceInfoData, 
            UInt32 Property,
            UInt32 PropertyRegDataType, 
            StringBuilder PropertyBuffer, 
            UInt32 PropertyBufferSize, 
            IntPtr RequiredSize);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern Boolean SetupDiSetClassInstallParams(
            IntPtr DeviceInfoSet, 
            SP_DEVINFO_DATA DeviceInfoData, 
            SP_PROPCHANGE_PARAMS 
            ClassInstallParams, 
            int ClassInstallParamsSize);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern Boolean SetupDiCallClassInstaller(
            UInt32 InstallFunction, 
            IntPtr DeviceInfoSet, 
            SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern Boolean SetupDiClassNameFromGuid(
            ref Guid ClassGuid, 
            StringBuilder className, 
            Int32 ClassNameSize, 
            ref Int32 RequiredSize);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern Boolean SetupDiGetClassDescription(
            ref Guid ClassGuid, 
            StringBuilder classDescription, 
            Int32 ClassDescriptionSize, 
            ref Int32 RequiredSize);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern Boolean SetupDiGetDeviceInstanceId(
            IntPtr DeviceInfoSet, 
            SP_DEVINFO_DATA DeviceInfoData, 
            StringBuilder DeviceInstanceId, 
            Int32 DeviceInstanceIdSize, 
            ref Int32 RequiredSize);

        public const int DIGCF_ALLCLASSES = (0x00000004);
        public const int DIGCF_PRESENT = (0x00000002);
        public const int INVALID_HANDLE_VALUE = -1;
        public const int SPDRP_DEVICEDESC = (0x00000000);
        public const int MAX_DEV_LEN = 256;

        public const int DEVICE_NOTIFY_WINDOW_HANDLE = (0x00000000);
        public const int DEVICE_NOTIFY_SERVICE_HANDLE = (0x00000001);
        public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = (0x00000004);
        public const int DBT_DEVTYP_DEVICEINTERFACE = (0x00000005);
        public const int DBT_DEVNODES_CHANGED = (0x0007);
        public const int WM_DEVICECHANGE = (0x0219);
        public const int DIF_PROPERTYCHANGE = (0x00000012);
        public const int DICS_FLAG_GLOBAL = (0x00000001);
        public const int DICS_FLAG_CONFIGSPECIFIC = (0x00000002);
        public const int DICS_ENABLE = (0x00000001);
        public const int DICS_DISABLE = (0x00000002);

        public static bool StateChange(bool Enable, int SelectedItem, IntPtr DevInfo)
        {
            bool result = false;
            SP_DEVINFO_DATA devInfoData = new SP_DEVINFO_DATA(); ;
            devInfoData.cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA));
            if (true == SetupDiEnumDeviceInfo(DevInfo, SelectedItem, devInfoData))
            {
                SP_PROPCHANGE_PARAMS pcp = new SP_PROPCHANGE_PARAMS(); ;
                pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(typeof(SP_CLASSINSTALL_HEADER));
                pcp.ClassInstallHeader.InstallFunction = DIF_PROPERTYCHANGE;
                pcp.Scope = DICS_FLAG_GLOBAL;
                pcp.StateChange = (Enable ? DICS_ENABLE : DICS_DISABLE);
                if (true == SetupDiSetClassInstallParams(DevInfo, devInfoData, pcp, Marshal.SizeOf(pcp)))
                {
                    if (true == SetupDiCallClassInstaller(DIF_PROPERTYCHANGE, DevInfo, devInfoData))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public static String GetClassNameFromGuid(Guid guid)
        {
            String result = String.Empty;
            StringBuilder className = new StringBuilder();
            Int32 iRequiredSize = 0;
            Int32 iSize = 0;
            bool b = SetupDiClassNameFromGuid(ref guid, className, iSize, ref iRequiredSize);

            className = new StringBuilder(iRequiredSize);
            iSize = iRequiredSize;

            b = SetupDiClassNameFromGuid(ref guid, className, iSize, ref iRequiredSize);
            if (true == b)
            {
                result = className.ToString();
            }
            return result;
        }
        public static String GetClassDescriptionFromGuid(Guid guid)
        {
            String result = String.Empty;
            StringBuilder classDesc = new StringBuilder(0);
            Int32 iRequiredSize = 0;
            Int32 iSize = 0;
            bool b = SetupDiGetClassDescription(ref guid, classDesc, iSize, ref iRequiredSize);

            classDesc = new StringBuilder(iRequiredSize);
            iSize = iRequiredSize;

            b = SetupDiGetClassDescription(ref guid, classDesc, iSize, ref iRequiredSize);
            if (true == b)
            {
                result = classDesc.ToString();
            }
            return result;
        }

        public static String GetDeviceInstanceId(IntPtr DeviceInfoSet, SP_DEVINFO_DATA DeviceInfoData)
        {
            String result = String.Empty;
            StringBuilder id = new StringBuilder(0);
            Int32 iRequiredSize = 0;
            Int32 iSize = 0;
            bool b = SetupDiGetDeviceInstanceId(DeviceInfoSet, DeviceInfoData, id, iSize, ref iRequiredSize);

            id = new StringBuilder(iRequiredSize);
            iSize = iRequiredSize;

            b = SetupDiGetDeviceInstanceId(DeviceInfoSet, DeviceInfoData, id, iSize, ref iRequiredSize);
            if (true == b)
            {
                result = id.ToString();
            }
            return result;
        }
    }
}
