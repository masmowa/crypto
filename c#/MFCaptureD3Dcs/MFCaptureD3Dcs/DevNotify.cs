using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using NativeAdapter.DeviceNotification.Interop;
using NativeAdapter.DeviceNotification.Interop.Constants;
using NativeAdapter.DeviceNotification.Interop.Enums;
using NativeAdapter.DeviceNotification.Interop.Structures;
using NativeAdapter.DeviceNotification.Interop.DNExtern;

namespace MFCaptureD3Dcs
{
    public class DevNotify
    {

        public DevNotify()
        {

        }

        public IntPtr NotifyHandle { get; set; }
        public Guid InterfaceGuid { get; set;  }
        private const int DbtDevtypDeviceinterface = 5;

        /// <summary>
        /// Registers a window to receive notifications when USB devices are plugged or unplugged.
        /// </summary>
        /// <param name="windowHandle">Handle to the window receiving notifications.</param>
        public void RegisterDeviceNotification(IntPtr windowHandle)
        {
            try
            {
                DEV_BROADCAST_DEVICEINTERFACE dbi = new DEV_BROADCAST_DEVICEINTERFACE()
                {
                    DeviceType = (uint)DBT_DEVTYP.DEVICEINTERFACE,
                    ClassGuid = DIUSBGuid.DIUSBDevice,
                    Name = string.Empty
                };

                dbi.Size = Marshal.SizeOf(dbi);
                IntPtr buffer = Marshal.AllocHGlobal(dbi.Size);
                Marshal.StructureToPtr(dbi, buffer, false);

                NotifyHandle = DN.RegisterDeviceNotification(windowHandle, buffer, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("RegisterDeviceNotification() {0}", ex.Message);
            }
        }

        /// <summary>
        /// Unregisters the window for USB device notifications
        /// </summary>
        public void UnregisterUsbDeviceNotification(/*object odn*/)
        {
            //DeviceNotification dn = (DeviceNotification)odn;
            DN.UnregisterDeviceNotification(NotifyHandle);
        }

    }
}
