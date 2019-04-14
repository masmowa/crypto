using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
/// <summary>
/// declerations of Device Broadcast Type and related information
/// used when a program wants to intercept specific device broadcast messages (WM_DEV?) from Window 
/// </summary>
/// <remarks>
/// <a href="https://social.msdn.microsoft.com/Forums/vstudio/en-US/5e1e78ea-964b-425a-8063-422bcb7fc307/how-to-retrieve-dbccname-from-devbroadcastdeviceinterface?forum=csharpgeneral">How to retrieve dbcname from DEV_BROADCAST_INTERFACE structure</a>
/// <a href="http://stackoverflow.com/questions/994774/acessing-dbcc-name-from-dev-broadcast-deviceinterface-win32-programming"> acessing dbcc_name from DEV_BROADCAST_DEVICEINTERFACE win32 programming</a>
/// <a href="http://stackoverflow.com/questions/2208722/how-to-get-friendly-device-name-from-dev-broadcast-deviceinterface-and-device-ins">How to get friendly device name from DEV_BROADCAST_DEVICEINTERFACE and Device Instance ID</a>
/// <a href="http://stackoverflow.com/questions/30981181/c-sharp-usb-driver-from-c-setupdigetdeviceinterfacedetail">C# USB driver from C++: SetupDiGetDeviceInterfaceDetail</a>
/// <a href="http://stackoverflow.com/questions/9245595/win32api-usb-setupdigetdeviceinterfacedetail-fail">Win32Api USB SetupDiGetDeviceInterfaceDetail fail</a>
/// <a href="http://stackoverflow.com/questions/13974047/first-call-to-setupdigetdeviceinterfacedetail-will-always-fail-with-error-invali?noredirect=1&lq=1">First call to SetupDiGetDeviceInterfaceDetail will always fail with ERROR_INVALID_USER_BUFFER</a>
/// <a href="http://pinvoke.net/default.aspx/setupapi.SetupDiGetDeviceInterfaceDetail">(setupapi) SetupDiGetDeviceInterfaceDetail</a>
/// <a href="http://pinvoke.net/default.aspx/Structures/SP_DEVICE_INTERFACE_DETAIL_DATA.html">SP_DEVICE_INTERFACE_DETAIL_DATA</a> 
/// <a href="https://social.msdn.microsoft.com/Forums/en-US/f3e702d5-56cb-46b3-8dee-c9aa1558f617/get-the-battery-information-design-capacity-and-full-charge-capacity-using-windows-api?forum=winembplatdev">Get the Battery Information (Design Capacity and Full Charge Capacity) using Windows API</a>
/// <a href="http://geekswithblogs.net/BruceEitman/archive/2010/03/03/windows-xp-overview-of-battery-monitor-series.aspx">Windows XP: Overview of Battery Monitor Series</a>
/// <a href="https://social.msdn.microsoft.com/Forums/vstudio/en-US/36bfee43-270b-4ce9-97f9-b2be8770c7da/setupdienumdeviceinterfaces-returns-error-87?forum=csharpgeneral">SetupDiEnumDeviceInterfaces returns error 87</a>
/// </remarks>
namespace Native.Win32.Interop
{
    namespace DBT
    {
        namespace Consts
        {
            public static class CLSID
            {
                public const string GUID_DEVINTERFACE_USB_DEVICE = "A5DCBF10-6530-11D2-901F-00C04FB951ED";
            }
        }
        namespace Enums
        {
            /*
             * Broadcast message and receipient flags.
             *
             * Note that there is a third "flag". If the wParam has:
             *
             * bit 15 on:   lparam is a pointer and bit 14 is meaningfull.
             * bit 15 off:  lparam is just a UNLONG data type.
             *
             * bit 14 on:   lparam is a pointer to an ASCIIZ string.
             * bit 14 off:  lparam is a pointer to a binary struture starting with
             *              a dword describing the length of the structure.
             */

            [Flags]
            public enum BSF : uint
            {
                QUERY = 0x00000001,
                IGNORECURRENTTASK = 0x00000002,      /* Meaningless for VxDs */
                FLUSHDISK = 0x00000004,      /* Shouldn't be used by VxDs */
                NOHANG = 0x00000008,
                POSTMESSAGE = 0x00000010,
                FORCEIFHUNG = 0x00000020,
                NOTIMEOUTIFNOTHUNG = 0x00000040,
                MSGSRV32ISOK = 0x80000000,      /* Called synchronously from PM API */
                MSGSRV32ISOK_BIT = 31,              /* Called synchronously from PM API */
            }
            [Flags]
            public enum BSM : uint
            {
                ALLCOMPONENTS = 0x00000000,
                VXDS = 0x00000001,
                NETDRIVER = 0x00000002,
                INSTALLABLEDRIVERS = 0x00000004,
                APPLICATIONS = 0x00000008,
            }

            /// <summary>
            /// DBT = Device Broadcast Type
            /// </summary>
            [Flags]
            public enum DBT : uint
            {
                /*
                 * Message = WM_DEVICECHANGE
                 * wParam  = DBT_APPYBEGIN
                 * lParam  = (not used)
                 *
                 *      'Appy-time is now available.  This message is itself sent
                 *      at 'Appy-time.
                 *
                 * Message = WM_DEVICECHANGE
                 * wParam  = DBT_APPYEND
                 * lParam  = (not used)
                 *
                 *      'Appy-time is no longer available.  This message is *NOT* sent
                 *      at 'Appy-time.  (It cannot be, because 'Appy-time is gone.)
                 *
                 * NOTE!  It is possible for DBT_APPYBEGIN and DBT_APPYEND to be sent
                 * multiple times during a single Windows session.  Each appearance of
                 * 'Appy-time is bracketed by these two messages, but 'Appy-time may
                 * momentarily become unavailable during otherwise normal Windows
                 * processing.  The current status of 'Appy-time availability can always
                 * be obtained from a call to _SHELL_QueryAppyTimeAvailable.
                 */
                APPYBEGIN = 0x0000,
                APPYEND = 0x0001,

                /*
                 * Message = WM_DEVICECHANGE
                 * wParam  = DBT_DEVNODES_CHANGED
                 * lParam  = 0
                 *
                 *      send when configmg finished a process tree batch. Some devnodes
                 *      may have been added or removed. This is used by ring3 people which
                 *      need to be refreshed whenever any devnode changed occur (like
                 *      device manager). People specific to certain devices should use
                 *      DBT_DEVICE* instead.
                 */

                DEVNODES_CHANGED = 0x0007,

                /*
                 * Message = WM_DEVICECHANGE
                 * wParam  = DBT_QUERYCHANGECONFIG
                 * lParam  = 0
                 *
                 *      sent to ask if a config change is allowed
                 */

                QUERYCHANGECONFIG = 0x0017,

                /*
                 * Message = WM_DEVICECHANGE
                 * wParam  = DBT_CONFIGCHANGED
                 * lParam  = 0
                 *
                 *      sent when a config has changed
                 */

                CONFIGCHANGED = 0x0018,

                /*
                 * Message = WM_DEVICECHANGE
                 * wParam  = DBT_CONFIGCHANGECANCELED
                 * lParam  = 0
                 *
                 *      someone cancelled the config change
                 */

                CONFIGCHANGECANCELED = 0x0019,

                /*
                 * Message = WM_DEVICECHANGE
                 * wParam  = DBT_MONITORCHANGE
                 * lParam  = new resolution to use (LOWORD=x, HIWORD=y)
                 *           if 0, use the default res for current config
                 *
                 *      this message is sent when the display monitor has changed
                 *      and the system should change the display mode to match it.
                 */

                MONITORCHANGE = 0x001B,

                /*
                 * Message = WM_DEVICECHANGE
                 * wParam  = DBT_SHELLLOGGEDON
                 * lParam  = 0
                 *
                 *      The shell has finished login on: VxD can now do Shell_EXEC.
                 */

                SHELLLOGGEDON = 0x0020,

                /*
                 * Message = WM_DEVICECHANGE
                 * wParam  = DBT_CONFIGMGAPI
                 * lParam  = CONFIGMG API Packet
                 *
                 *      CONFIGMG ring 3 call.
                 */
                CONFIGMGAPI32 = 0x0022,

                /*
                 * Message = WM_DEVICECHANGE
                 * wParam  = DBT_VXDINITCOMPLETE
                 * lParam  = 0
                 *
                 *      CONFIGMG ring 3 call.
                 */
                VXDINITCOMPLETE = 0x0023,

                DEVICEARRIVAL = 0x8000,  // system detected a new device
                DEVICEQUERYREMOVE = 0x8001,  // wants to remove, may fail
                DEVICEQUERYREMOVEFAILED = 0x8002,  // removal aborted
                DEVICEREMOVEPENDING = 0x8003,  // about to remove, still avail.
                DEVICEREMOVECOMPLETE = 0x8004,  // device is gone
                DEVICETYPESPECIFIC = 0x8005,  // type specific event
                                              /*
                                               * Message = WM_DEVICECHANGE
                                               * wParam  = DBT_VOLLOCK*
                                               * lParam  = pointer to VolLockBroadcast structure described below
                                               *
                                               *      Messages issued by IFSMGR for volume locking purposes on WM_DEVICECHANGE.
                                               *      All these messages pass a pointer to a struct which has no pointers.
                                               */

                VOLLOCKQUERYLOCK = 0x8041,
                VOLLOCKLOCKTAKEN = 0x8042,
                VOLLOCKLOCKFAILED = 0x8043,
                VOLLOCKQUERYUNLOCK = 0x8044,
                VOLLOCKLOCKRELEASED = 0x8045,
                VOLLOCKUNLOCKFAILED = 0x8046,

            }
            [Flags]
            public enum DBTF : uint
            {
                MEDIA = 0x0001,          // media comings and goings
                NET = 0x0002,          // network volume
            }

            [Flags]
            public enum DBT_DEVTYP : uint
            {
                OEM = 0x00000000,  // oem-defined device type
                DEVNODE = 0x00000001,  // devnode number
                VOLUME = 0x00000002,  // logical volume
                PORT = 0x00000003,  // serial, parallel
                NET = 0x00000004,  // network resource
                DEVICEINTERFACE = 0x00000005,  // device interface class
                HANDLE = 0x00000006,  // file system handle
            }
        }
        namespace Structures
        {
            /*
             * Device broadcast header
             */

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
            public class DEV_BROADCAST_HDR
            {     /* */
                public UInt32 Size;
                public UInt32 DeviceType;
                public UInt32 Reserved;

                public IntPtr GetPtr()
                {
                    IntPtr ip = new IntPtr();

                    int size = Marshal.SizeOf(this);

                    Marshal.ReAllocCoTaskMem(ip, size);

                    Marshal.StructureToPtr(this, ip, false);

                    return ip;
                }

                public DEV_BROADCAST_HDR()
                {
                    Size = (UInt32)Marshal.SizeOf(this);
                    Reserved = 0;

                }
                public static DEV_BROADCAST_HDR PtrToEvtHdr(IntPtr pNativeData)
                {
                    UInt32 size = (UInt32)Marshal.ReadInt32(pNativeData);
                    Enums.DBT_DEVTYP met = (Enums.DBT_DEVTYP)Marshal.ReadInt32(pNativeData, 4);

                    object managed = null;

                    switch (met)
                    {
                        case Enums.DBT_DEVTYP.OEM:
                            managed = (DEV_BROADCAST_OEM)Marshal.PtrToStructure(pNativeData, typeof(DEV_BROADCAST_OEM));
                            break;
                        case Enums.DBT_DEVTYP.DEVNODE:
                            managed = (DEV_BROADCAST_DEVNODE)Marshal.PtrToStructure(pNativeData, typeof(DEV_BROADCAST_DEVNODE));
                            break;
                        case Enums.DBT_DEVTYP.VOLUME:
                            managed = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(pNativeData, typeof(DEV_BROADCAST_VOLUME));
                            break;
                        case Enums.DBT_DEVTYP.PORT:
                            managed = (DEV_BROADCAST_PORT)Marshal.PtrToStructure(pNativeData, typeof(DEV_BROADCAST_PORT));
                            break;
                        case Enums.DBT_DEVTYP.NET:
                            managed = (DEV_BROADCAST_NET)Marshal.PtrToStructure(pNativeData, typeof(DEV_BROADCAST_NET));
                            break;
                        case Enums.DBT_DEVTYP.DEVICEINTERFACE:
                            managed = (DEV_BROADCAST_DEVICEINTERFACE)Marshal.PtrToStructure(pNativeData, typeof(DEV_BROADCAST_DEVICEINTERFACE));
                            break;
                        case Enums.DBT_DEVTYP.HANDLE:
                            managed = (DEV_BROADCAST_HANDLE)Marshal.PtrToStructure(pNativeData, typeof(DEV_BROADCAST_HANDLE));
                            break;

                        default:
                            managed = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(pNativeData, typeof(DEV_BROADCAST_HDR));
                            break;
                    }
                    return (DEV_BROADCAST_HDR)managed;

                }
                public static DEV_BROADCAST_HDR NewDHDR(IntPtr pNative)
                {
                    DEV_BROADCAST_HDR managed = new DEV_BROADCAST_HDR();
                    managed = (DEV_BROADCAST_HDR)Marshal.PtrToStructure<DEV_BROADCAST_HDR>(pNative);
                    return managed;
                }
            }
            internal class EvtHdrMarshaler : ICustomMarshaler
            {
                public void CleanUpManagedData(object ManagedObj)
                {
                    ManagedObj = null;
                }

                public void CleanUpNativeData(IntPtr pNativeData)
                {
                    // Free the unmanaged memory. Use Marshal.FreeCoTaskMem if using COM.
                    Marshal.FreeCoTaskMem(pNativeData);
                }

                public int GetNativeDataSize()
                {
                    return sizeof(UInt32) + Marshal.SizeOf<DEV_BROADCAST_HDR>();
                }

                public IntPtr MarshalManagedToNative(object ManagedObj)
                {
                    DEV_BROADCAST_HDR hdr = (DEV_BROADCAST_HDR)ManagedObj;
                    IntPtr ip = hdr.GetPtr();
                    return ip;
                }

                public object MarshalNativeToManaged(IntPtr pNativeData)
                {
                    DEV_BROADCAST_HDR obj = DEV_BROADCAST_HDR.PtrToEvtHdr(pNativeData);

                    return obj;
                    //throw new NotImplementedException();
                }
            }


            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public class DEV_BROADCAST_OEM : DEV_BROADCAST_HDR
            {     /* */
                public UInt32 identifier;
                public UInt32 suppfunc;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public class DEV_BROADCAST_DEVNODE : DEV_BROADCAST_HDR
            { /* */
                public UInt32 devnode;

                public UInt32 DevNode { get { return devnode; } }
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public class DEV_BROADCAST_VOLUME : DEV_BROADCAST_HDR
            { /* */
                public UInt32 unitmask;
                public UInt16 flags;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public class DEV_BROADCAST_PORT : DEV_BROADCAST_HDR
            {
                [MarshalAs(UnmanagedType.LPWStr, SizeConst = 256)]
                public string name;

                public string Name { get { return name; } }
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public class DEV_BROADCAST_NET : DEV_BROADCAST_HDR
            { /* */
                public UInt32 resource;
                public UInt32 flags;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public class DEV_BROADCAST_DEVICEINTERFACE : DEV_BROADCAST_HDR
            {
                public Guid ClassGuid;
                [MarshalAs(UnmanagedType.LPWStr, SizeConst = 256)]
                public string Name;
            }


            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public class DEV_BROADCAST_HANDLE : DEV_BROADCAST_HDR
            {
                public IntPtr handle;     // file handle used in call to RegisterDeviceNotification
                public IntPtr hdevnotify; // returned from RegisterDeviceNotification
                                          //
                                          // The following 3 fields are only valid if wParam is DBT_CUSTOMEVENT.
                                          //
                public Guid eventguid;
                public long nameoffset; // offset (bytes) of variable-length string buffer (-1 if none)
                public IntPtr data;    // variable-sized buffer, potentially containing binary and/or text data
            }


        }

    }
}
