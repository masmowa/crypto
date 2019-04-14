using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetupApi.DeviceInstallation;
using System.Runtime.InteropServices;
using Native.Win32.Interop;

/// <summary>
/// example SetupDiEnumDeviceInterfaces returns error 87
/// </summary>
/// <see cref="https://social.msdn.microsoft.com/Forums/vstudio/en-US/36bfee43-270b-4ce9-97f9-b2be8770c7da/setupdienumdeviceinterfaces-returns-error-87?forum=csharpgeneral"/>
/// <remarks></remarks>
namespace caSetupDiEnumDeviceInterfacesReturnsError87fix
{
    class Program
    {
        //        Question
        //Sign in to vote
        //0
        //Sign in to vote
        //The answer to your specific question is that you are not 
        // passing a correctly filled out SP_DEVINFO_DATA structure to SetupDiEnumDeviceInterfaces.
        // Typically you get one of those from calling SetupDiEnumDeviceInfo. 
        // I was messing around with this and came up with an interesting wrapper:

        static Guid MY_GUID = Guid.Empty;
        /// <summary>
        /// The GUID_DEVINTERFACE_IMAGE device interface class is defined for WIA devices and Still Image (STI) devices, including digital cameras and scanners.
        /// </summary>
        /// <remarks>
        /// WIA: Windows Image Acquisition Drivers.
        /// <see cref="https://msdn.microsoft.com/windows/hardware/drivers/image/windows-image-acquisition-drivers"/> 
        /// IRP : I/O Request Packet : 
        /// </remarks>
        static Guid GUID_DEVINTERFACE_IMAGE = new Guid("6BDD1FC6-810F-11D0-BEC7-08002BE2092F");
        static void Main()
        {
            try
            {
                using (DeviceDriverList ddList = new DeviceDriverList(Digcf.AllClasses | Digcf.Present))
                {
                    foreach (DeviceInfoData deviceInfoData in ddList.EnumDeviceInfo())
                    {
                        Console.WriteLine(deviceInfoData.SetupClass);

                        string className = Win32.GetClassDescriptionFromGuid(deviceInfoData.SetupClass);
                        Console.WriteLine(" > {0} ", className);
                        string classDescription = Win32.GetClassDescriptionFromGuid(deviceInfoData.SetupClass);
                        Console.WriteLine(" > {0} ", classDescription);

                        foreach (DeviceInterfaceData deviceInterfaceData in ddList.EnumDeviceInterfaces(deviceInfoData, MY_GUID))
                        {
                            Console.WriteLine(" >" + deviceInterfaceData.InterfaceClass);
                        }
                    }
                }
                WaitKey("");
            }
            catch (Exception ex)
            {
                Console.WriteLine("-- EXCEPTION --");
                Console.WriteLine(ex);
            }
        }
        public static void WaitKey(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine(message);
            }
            Console.Write("press any key ... ");
            ConsoleKeyInfo key = Console.ReadKey();
        }
    }
}

namespace SetupApi.DeviceInstallation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using Microsoft.Win32.SafeHandles;

    [Flags]
    public enum Digcf : uint
    {
        Default = 0x00000001, // only valid with DeviceInterface
        Present = 0x00000002,
        AllClasses = 0x00000004,
        Profile = 0x00000008,
        DeviceInterface = 0x00000010,
    }
    [StructLayout(LayoutKind.Sequential)]
    public sealed class DeviceInfoData
    {
        public DeviceInfoData()
        {
            cbSize = (UInt32)Marshal.SizeOf(this);
        }

        public Guid SetupClass { get { return classGuid; } }
        public UInt32 DeviceInstance { get { return devInst; } }

        private UInt32 cbSize;
        private Guid classGuid;
        private UInt32 devInst;
        private IntPtr reserved;
    }

    // Device interface data
    [StructLayout(LayoutKind.Sequential)]
    public sealed class DeviceInterfaceData
    {
        public DeviceInterfaceData()
        {
            cbSize = (UInt32)Marshal.SizeOf(this);
        }

        public Guid InterfaceClass { get { return interfaceClassGuid; } }
        public UInt32 Flags { get { return flags; } }

        private UInt32 cbSize;
        private Guid interfaceClassGuid;
        private UInt32 flags;
        private IntPtr reserved;
    }

    public sealed class DeviceDriverList : IDisposable
    {
        public DeviceDriverList(Digcf flags) : this(Guid.Empty, null, IntPtr.Zero, flags) { }
        public DeviceDriverList(Guid classGuid, string enumerator, IntPtr hwndParent, Digcf flags)
        {
            handle = SetupDiGetClassDevs(ref classGuid, enumerator, hwndParent, (uint)flags);
            if (handle.IsInvalid)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        ~DeviceDriverList()
        {
            Dispose();
        }

        public void Dispose()
        {
            handle.Dispose();
            GC.SuppressFinalize(this);
        }

        public IEnumerable<DeviceInfoData> EnumDeviceInfo()
        {
            for (uint memberIndex = 0; ; memberIndex++)
            {
                if (handle.IsClosed)
                    break;
                var deviceInfoData = new DeviceInfoData();
                if (!SetupDiEnumDeviceInfo(handle, memberIndex, /*out*/ deviceInfoData))
                {
                    int error = Marshal.GetLastWin32Error();
                    if (error == ERROR_NO_MORE_ITEMS)
                        break;
                    throw new Win32Exception(error);
                }
                yield return deviceInfoData;
            }
        }

        public IEnumerable<DeviceInterfaceData> EnumDeviceInterfaces(DeviceInfoData deviceInfoData, Guid interfaceClassGuid)
        {
            for (uint memberIndex = 0; ; memberIndex++)
            {
                if (handle.IsClosed)
                    break;
                var deviceInterfaceData = new DeviceInterfaceData();
                if (!SetupDiEnumDeviceInterfaces(handle, deviceInfoData, ref interfaceClassGuid, memberIndex, out deviceInterfaceData))
                {
                    int error = Marshal.GetLastWin32Error();
                    if (error == ERROR_NO_MORE_ITEMS)
                        break;
                    throw new Win32Exception(error);
                }
                yield return deviceInterfaceData;
            }
        }

        private SafeDeviceInfoListHandle handle;

        private const int ERROR_NO_MORE_ITEMS = 259;

        [DllImport("setupapi.dll", SetLastError = true)]
        extern static SafeDeviceInfoListHandle SetupDiGetClassDevs(
            ref Guid classGuid, 
            string enumerator, 
            IntPtr hwndParent, 
            uint flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        extern static bool SetupDiEnumDeviceInterfaces(
            SafeDeviceInfoListHandle deviceInfoSet, 
            DeviceInfoData deviceInfoData, 
            ref Guid interfaceClassGuid, 
            UInt32 memberIndex, 
            out DeviceInterfaceData deviceInterfaceData
            );

        [DllImport("setupapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        extern static bool SetupDiEnumDeviceInfo(
            SafeDeviceInfoListHandle deviceInfoSet, 
            uint memberIndex, 
            /*out*/ DeviceInfoData deviceInfoData
            );

        [DllImport("setupapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        extern static bool SetupDiDestroyDeviceInfoList(IntPtr handle);

        private sealed class SafeDeviceInfoListHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            public SafeDeviceInfoListHandle() : base(true) { }
            public SafeDeviceInfoListHandle(IntPtr preexistingHandle, bool ownsHandle) : base(ownsHandle)
            {
                base.SetHandle(preexistingHandle);
            }

            protected override bool ReleaseHandle()
            {
                return SetupDiDestroyDeviceInfoList(this.handle);
            }
        }
    }
}
