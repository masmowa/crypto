using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetupApi.DeviceInstallation;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using Native.Win32.Interop;
/// <summary>
/// Example from StackOverflow
/// </summary>
/// <see cref="http://stackoverflow.com/questions/4363701/how-to-reload-the-usb-disk-in-c"/>
/// <remarks>
/// a useful list of Microsoft namespace code 
/// <see cref="https://referencesource.microsoft.com/#PresentationFramework/Test/Common/FxCop/Suppression/Framework/Microsoft.Usage.cs"/>
/// </remarks>
namespace caSetupDiEnumDeviceInterfacesReturnsError87fix
{
    class GetBatteryInfo
    {
        #region Variables Declaration
        private const int DIGCF_DEFAULT = 0x00000001;  // only valid with DIGCF_DEVICEINTERFACE
        private const int DIGCF_PRESENT = 0x00000002;
        private const int DIGCF_ALLCLASSES = 0x00000004;
        private const int DIGCF_PROFILE = 0x00000008;
        private const int DIGCF_DEVICEINTERFACE = 0x00000010;
        const int GENERIC_READ = 80000000;
        const uint GENERIC_WRITE = 0x40000000;
        internal static uint OPEN_EXISTING = 3;
        internal static uint FILE_ATTRIBUTE_NORMAL = 0x80;
        internal static uint FILE_SHARE_READ = 1;
        internal static uint FILE_SHARE_WRITE = 2;
        public const uint METHOD_BUFFERED = 0;
        public const uint FILE_ANY_ACCESS = 0x0001;
        public const uint FILE_DEVICE_BATTERY = 0x294044; //0x294040; //0x00000029;
        public static readonly Guid GUID_DEVCLASS_BATTERY = new Guid(0x72631e54, 0x78a4, 0x11d0, 0xbc, 0xf7, 0x00, 0xaa, 0x00, 0xb7, 0xb3, 0x2a);
        #endregion
        #region Windows APIs
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPStr)] string lpFileName, uint dwDesiredAccess,
            uint dwShareMode, IntPtr SecurityAttributes,
            uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile(
            string fileName, [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
             [MarshalAs(UnmanagedType.U4)] FileShare fileShare, IntPtr securityAttributes,
             [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition, [MarshalAs(UnmanagedType.U4)] EFileAttributes flags,
              IntPtr template);
        [DllImport("wimgapi.dll", EntryPoint = "WIMCloseHandle", SetLastError = true)]
        public static extern int WIMCloseHandle(IntPtr hObject);
        [DllImport(@"hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void HidD_GetHidGuid(out Guid gHid);
        [DllImport("setupapi.dll")]
        internal static extern Int32 SetupDiClassNameFromGuid(
            ref Guid ClassGuid, 
            StringBuilder className, 
            Int32 ClassNameSize, 
            ref Int32 RequiredSize
            );
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(
            IntPtr hDevInfo, 
            IntPtr devInvo,
            ref Guid interfaceClassGuid, 
            Int32 memberIndex, 
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
            );
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern UInt16 SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);
        //[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        //public static extern bool DeviceIoControl(SafeHandle hDevice, uint dwIoControlCode,
        //    IntPtr lpInBuffer, uint nInBufferSize, IntPtr lpOutBuffer, uint nOutBufferSize, out int lpBytesReturned,
        //    IntPtr lpOverlapped);
        //[DllImport("Kernel32.dll", SetLastError = false, CharSet = CharSet.Auto)]
        //public static extern bool DeviceIoControl(SafeFileHandle hDevice, uint IoControlCode,
        //    [MarshalAs(UnmanagedType.AsAny)][In] BATTERY_QUERY_INFORMATION InBuffer, uint nInBufferSize,
        //[MarshalAs(UnmanagedType.AsAny)][Out] BATTERY_INFORMATION OutBuffer, out uint nOutBufferSize, ref uint pBytesReturned,
        //[In] IntPtr lpOverlapped);
        [DllImport("Kernel32.dll", SetLastError = false, CharSet = CharSet.Auto)]
        public static extern bool DeviceIoControl(SafeFileHandle hDevice, uint IoControlCode,
            ref BATTERY_QUERY_INFORMATION InBuffer, uint nInBufferSize,
        ref BATTERY_INFORMATION OutBuffer, uint nOutBufferSize, ref uint pBytesReturned,
        IntPtr lpOverlapped);
        [DllImport("Kernel32.dll", SetLastError = false, CharSet = CharSet.Auto)]
        public static extern bool DeviceIoControl(SafeFileHandle hDevice, uint IoControlCode, ref ulong InBuffer,
             uint nInBufferSize, ref ulong OutBuffer, uint nOutBufferSize, ref ulong pBytesReturned, object lpOverlapped);
        #endregion
        #region Structures and Enums
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BATTERY_QUERY_INFORMATION
        {
            public ulong BatteryTag;
            public BATTERY_QUERY_INFORMATION_LEVEL InformationLevel;
            public long AtRate;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BATTERY_QUERY_INFORMATION_LEVEL
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string BatteryDeviceName;
            public ulong BatteryEstimatedTime;
            public BATTERY_REPORTING_SCALE BatteryGranularityInformation;
            public BATTERY_INFORMATION BatteryInformation;
            public BATTERY_MANUFACTURE_DATE BatteryManufactureDate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string BatteryManufactureName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string BatterySerialNumber;
            public ulong BatteryTemperature;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string BatteryUniqueID;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BATTERY_REPORTING_SCALE
        {
            public ulong Granularity;
            public ulong Capacity;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BATTERY_MANUFACTURE_DATE
        {
            public char Day;
            public char Month;
            public ushort Year;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BATTERY_INFORMATION
        {
            public ulong Capabilities;
            public char Technology;
            public char[] Reserved;
            public char[] Chemistry;
            public ulong DesignCapacity;
            public ulong FullChargeCapacity;
            public ulong DefaultAlert1;
            public ulong DefaultAlert2;
            public ulong CriticalBias;
            public ulong CycleCount;
        }
        [Flags]
        public enum EFileAttributes : uint
        {
            Readonly = 0x00000001,
            Hidden = 0x00000002,
            System = 0x00000004,
            Directory = 0x00000010,
            Archive = 0x00000020,
            Device = 0x00000040,
            Normal = 0x00000080,
            Temporary = 0x00000100,
            SparseFile = 0x00000200,
            ReparsePoint = 0x00000400,
            Compressed = 0x00000800,
            Offline = 0x00001000,
            NotContentIndexed = 0x00002000,
            Encrypted = 0x00004000,
            Write_Through = 0x80000000,
            Overlapped = 0x40000000,
            NoBuffering = 0x20000000,
            RandomAccess = 0x10000000,
            SequentialScan = 0x08000000,
            DeleteOnClose = 0x04000000,
            BackupSemantics = 0x02000000,
            PosixSemantics = 0x01000000,
            OpenReparsePoint = 0x00200000,
            OpenNoRecall = 0x00100000,
            FirstPipeInstance = 0x00080000
        }
        #endregion
        #region Methods
        public void ConnectWithDevice()
        {
            int index = 0;
            Guid guid = GUID_DEVCLASS_BATTERY;
            // get the GUID of the HID class
            //HidD_GetHidGuid(out guid);
            // get a handle to all devices that are part of the HID class
            // Fun fact:  DIGCF_PRESENT worked on my machine just fine.  I reinstalled Vista, and now it no longer finds the Wiimote with that parameter enabled...
            IntPtr hDevInfo = Win32.SetupDiGetClassDevs(
                ref guid, 
                null, 
                IntPtr.Zero, 
                DIGCF_DEVICEINTERFACE   // | HIDImports.DIGCF_PRESENT);
                );
            // create a new interface data struct and initialize its size
            SP_DEVICE_INTERFACE_DATA diData = new SP_DEVICE_INTERFACE_DATA();
            diData.cbSize = (uint)Marshal.SizeOf(diData);
            // get a device interface to a single device (enumerate all devices)
            while (SetupDiEnumDeviceInterfaces(hDevInfo, IntPtr.Zero, ref guid, index, ref diData))
            {
                UInt32 size;
                // Get Device Name
                string deviceName = Win32.GetClassNameFromGuid(diData.InterfaceClassGuid);
                // get the buffer size for this device detail instance (returned in the size parameter)
                Win32.SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, IntPtr.Zero, 0, out size, IntPtr.Zero);
                // create a detail struct and set its size
                SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                diDetail.cbSize = (uint)(IntPtr.Size == 8 ? 8 : 5);
                // actually get the detail struct
                if (Win32.SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, ref diDetail, size, out size, IntPtr.Zero))
                {
                    MessageBox.Show("Battery " + (index + 1) + " Device Path: " + diDetail.DevicePath);
                    // open a read/write handle to our device using the DevicePath returned
                    // TODO Convert to Microsoft.Win32.SafeHandle.SafeHandleZeroOrMinusOneIsInvalid
                    SafeFileHandle mHandle = CreateFile(
                        diDetail.DevicePath, 
                        FileAccess.ReadWrite, 
                        FileShare.ReadWrite, 
                        IntPtr.Zero, 
                        FileMode.Open, 
                        EFileAttributes.Overlapped, 
                        IntPtr.Zero
                        );
                    if (!mHandle.IsInvalid)
                    {
                        bool result = false;
                        try
                        {
                            ulong time = 0, tag = 0, bytes = 0;
                            result = DeviceIoControl(
                                mHandle, 
                                CTL_CODE(FILE_DEVICE_BATTERY, 0x10, METHOD_BUFFERED, FILE_ANY_ACCESS),
                                ref time, 
                                (uint)Marshal.SizeOf(time),
                                ref tag, 
                                (uint)Marshal.SizeOf(tag), 
                                ref bytes, 
                                null
                                );
                            if (result)
                            {
                                BATTERY_QUERY_INFORMATION bqi = new BATTERY_QUERY_INFORMATION();
                                bqi.AtRate = 0;
                                bqi.BatteryTag = tag;
                                //bqi.InformationLevel = 
                                MessageBox.Show("Tag: " + tag.ToString() + " and bytes: " + bytes);
                                uint len = 0;
                                uint nInBufferSize = (uint)Marshal.SizeOf(bqi);
                                BATTERY_INFORMATION bi = new BATTERY_INFORMATION();
                                uint nOutBufferSize = (uint)Marshal.SizeOf(bqi);
                                result = false;
                                result = DeviceIoControl(
                                    mHandle, 
                                    CTL_CODE(FILE_DEVICE_BATTERY, 0x11, METHOD_BUFFERED, FILE_ANY_ACCESS),
                                   ref bqi, 
                                   nInBufferSize, 
                                   ref bi, 
                                   nOutBufferSize, 
                                   ref len, 
                                   IntPtr.Zero
                                   );
                                if (result)
                                {
                                    MessageBox.Show("Bytes Return: " + len.ToString());
                                    MessageBox.Show("Design Capacity: " + bi.DesignCapacity);
                                    MessageBox.Show("Full Charge Capacity: " + bi.FullChargeCapacity);
                                }
                                else
                                {
                                    MessageBox.Show("No Result");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Battery Query Information Not processed");
                            }
                        }
                        finally
                        {
                            //Marshal.FreeHGlobal(pInBuffer);
                            //Marshal.FreeHGlobal(pOutBuffer);
                        }
                    }
                    else
                    {
                        MessageBox.Show("In-valid File Handle");
                    }
                    mHandle.Close();
                    int errCode = Marshal.GetLastWin32Error();
                    MessageBox.Show("Win32 Error: " + errCode.ToString());
                }
                else
                {
                    // failed to get the detail struct
                    throw new Exception("SetupDiGetDeviceInterfaceDetail failed on index " + index);
                }
                // move to the next device
                index++;
            }
            // clean up our list
            SetupDiDestroyDeviceInfoList(hDevInfo);
            // if we didn't find a Wiimote, throw an exception
            //if (!found)
            //    throw new Exception("Wiimote not found in HID device list.");
        }
        private static uint CTL_CODE(uint DeviceType, uint Function, uint Method, uint Access)
        {
            return (uint)(((DeviceType) << 16) | ((Access) << 14) | ((Function) << 2) | (Method));
        }
        #endregion
    }
    }
