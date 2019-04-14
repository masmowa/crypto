using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Reflection;


namespace Native.Adapter
{

    #region WIN32 SetupDi constants and flags
    /// <summary>
    /// taken from http://msdn.microsoft.com/en-us/library/windows/hardware/ff553412(v=vs.85).aspx
    /// GUID class constants
    /// </summary>
    public class GUID_DEVINTERFACE
    {
        public static Guid BUS1394_CLASS_GUID = new Guid("6BDD1FC1-810F-11d0-BEC7-08002BE2092F");
        public static Guid GUID_61883_CLASS = new Guid("7EBEFBC0-3200-11d2-B4C2-00A0C9697D07");
        public static Guid GUID_DEVICE_APPLICATIONLAUNCH_BUTTON = new Guid("629758EE-986E-4D9E-8E47-DE27F8AB054D");
        public static Guid GUID_DEVICE_BATTERY = new Guid("72631E54-78A4-11D0-BCF7-00AA00B7B32A");
        public static Guid GUID_DEVICE_LID = new Guid("4AFA3D52-74A7-11d0-be5e-00A0C9062857");
        public static Guid GUID_DEVICE_MEMORY = new Guid("3FD0F03D-92E0-45FB-B75C-5ED8FFB01021");
        public static Guid GUID_DEVICE_MESSAGE_INDICATOR = new Guid("CD48A365-FA94-4CE2-A232-A1B764E5D8B4");
        public static Guid GUID_DEVICE_PROCESSOR = new Guid("97FADB10-4E33-40AE-359C-8BEF029DBDD0");
        public static Guid GUID_DEVICE_SYS_BUTTON = new Guid("4AFA3D53-74A7-11d0-be5e-00A0C9062857");
        public static Guid GUID_DEVICE_THERMAL_ZONE = new Guid("4AFA3D51-74A7-11d0-be5e-00A0C9062857");
        public static Guid GUID_BTHPORT_DEVICE_INTERFACE = new Guid("0850302A-B344-4fda-9BE9-90576B8D46F0");
        public static Guid GUID_DEVINTERFACE_BRIGHTNESS = new Guid("FDE5BBA4-B3F9-46FB-BDAA-0728CE3100B4");
        public static Guid GUID_DEVINTERFACE_DISPLAY_ADAPTER = new Guid("5B45201D-F2F2-4F3B-85BB-30FF1F953599");
        public static Guid GUID_DEVINTERFACE_I2C = new Guid("2564AA4F-DDDB-4495-B497-6AD4A84163D7");
        public static Guid GUID_DEVINTERFACE_IMAGE = new Guid("6BDD1FC6-810F-11D0-BEC7-08002BE2092F");
        public static Guid GUID_DEVINTERFACE_MONITOR = new Guid("E6F07B5F-EE97-4a90-B076-33F57BF4EAA7");
        public static Guid GUID_DEVINTERFACE_OPM = new Guid("BF4672DE-6B4E-4BE4-A325-68A91EA49C09");
        public static Guid GUID_DEVINTERFACE_VIDEO_OUTPUT_ARRIVAL = new Guid("1AD9E4F0-F88D-4360-BAB9-4C2D55E564CD");
        public static Guid GUID_DISPLAY_DEVICE_ARRIVAL = new Guid("1CA05180-A699-450A-9A0C-DE4FBE3DDD89");
        public static Guid GUID_DEVINTERFACE_HID = new Guid("4D1E55B2-F16F-11CF-88CB-001111000030");
        public static Guid GUID_DEVINTERFACE_KEYBOARD = new Guid("884b96c3-56ef-11d1-bc8c-00a0c91405dd");
        public static Guid GUID_DEVINTERFACE_MOUSE = new Guid("378DE44C-56EF-11D1-BC8C-00A0C91405DD");
        public static Guid GUID_DEVINTERFACE_MODEM = new Guid("2C7089AA-2E0E-11D1-B114-00C04FC2AAE4");
        public static Guid GUID_DEVINTERFACE_NET = new Guid("CAC88484-7515-4C03-82E6-71A87ABAC361");
        public static Guid GUID_DEVINTERFACE_SENSOR = new Guid(0XBA1BB692, 0X9B7A, 0X4833, 0X9A, 0X1E, 0X52, 0X5E, 0XD1, 0X34, 0XE7, 0XE2);
        public static Guid GUID_DEVINTERFACE_COMPORT = new Guid("86E0D1E0-8089-11D0-9CE4-08003E301F73");
        public static Guid GUID_DEVINTERFACE_PARALLEL = new Guid("97F76EF0-F883-11D0-AF1F-0000F800845C");
        public static Guid GUID_DEVINTERFACE_PARCLASS = new Guid("811FC6A5-F728-11D0-A537-0000F8753ED1");
        public static Guid GUID_DEVINTERFACE_SERENUM_BUS_ENUMERATOR = new Guid("4D36E978-E325-11CE-BFC1-08002BE10318");
        public static Guid GUID_DEVINTERFACE_CDCHANGER = new Guid("53F56312-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_CDROM = new Guid("53F56308-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_DISK = new Guid("53F56307-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_FLOPPY = new Guid("53F56311-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_MEDIUMCHANGER = new Guid("53F56310-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_PARTITION = new Guid("53F5630A-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_STORAGEPORT = new Guid("2ACCFE60-C130-11D2-B082-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_TAPE = new Guid("53F5630B-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_VOLUME = new Guid("53F5630D-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_DEVINTERFACE_WRITEONCEDISK = new Guid("53F5630C-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_IO_VOLUME_DEVICE_INTERFACE = new Guid("53F5630D-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid MOUNTDEV_MOUNTED_DEVICE_GUID = new Guid("53F5630D-B6BF-11D0-94F2-00A0C91EFB8B");
        public static Guid GUID_AVC_CLASS = new Guid("095780C3-48A1-4570-BD95-46707F78C2DC");
        public static Guid GUID_VIRTUAL_AVC_CLASS = new Guid("616EF4D0-23CE-446D-A568-C31EB01913D0");
        public static Guid KSCATEGORY_ACOUSTIC_ECHO_CANCEL = new Guid("BF963D80-C559-11D0-8A2B-00A0C9255AC1");
        public static Guid KSCATEGORY_AUDIO = new Guid("6994AD04-93EF-11D0-A3CC-00A0C9223196");
        public static Guid KSCATEGORY_AUDIO_DEVICE = new Guid("FBF6F530-07B9-11D2-A71E-0000F8004788");
        public static Guid KSCATEGORY_AUDIO_GFX = new Guid("9BAF9572-340C-11D3-ABDC-00A0C90AB16F");
        public static Guid KSCATEGORY_AUDIO_SPLITTER = new Guid("9EA331FA-B91B-45F8-9285-BD2BC77AFCDE");
        public static Guid KSCATEGORY_BDA_IP_SINK = new Guid("71985F4A-1CA1-11d3-9CC8-00C04F7971E0");
        public static Guid KSCATEGORY_BDA_NETWORK_EPG = new Guid("71985F49-1CA1-11d3-9CC8-00C04F7971E0");
        public static Guid KSCATEGORY_BDA_NETWORK_PROVIDER = new Guid("71985F4B-1CA1-11d3-9CC8-00C04F7971E0");
        public static Guid KSCATEGORY_BDA_NETWORK_TUNER = new Guid("71985F48-1CA1-11d3-9CC8-00C04F7971E0");
        public static Guid KSCATEGORY_BDA_RECEIVER_COMPONENT = new Guid("FD0A5AF4-B41D-11d2-9C95-00C04F7971E0");
        public static Guid KSCATEGORY_BDA_TRANSPORT_INFORMATION = new Guid("A2E3074F-6C3D-11d3-B653-00C04F79498E");
        public static Guid KSCATEGORY_BRIDGE = new Guid("085AFF00-62CE-11CF-A5D6-28DB04C10000");
        public static Guid KSCATEGORY_CAPTURE = new Guid("65E8773D-8F56-11D0-A3B9-00A0C9223196");
        public static Guid KSCATEGORY_CLOCK = new Guid("53172480-4791-11D0-A5D6-28DB04C10000");
        public static Guid KSCATEGORY_COMMUNICATIONSTRANSFORM = new Guid("CF1DDA2C-9743-11D0-A3EE-00A0C9223196");
        public static Guid KSCATEGORY_CROSSBAR = new Guid("A799A801-A46D-11D0-A18C-00A02401DCD4");
        public static Guid KSCATEGORY_DATACOMPRESSOR = new Guid("1E84C900-7E70-11D0-A5D6-28DB04C10000");
        public static Guid KSCATEGORY_DATADECOMPRESSOR = new Guid("2721AE20-7E70-11D0-A5D6-28DB04C10000");
        public static Guid KSCATEGORY_DATATRANSFORM = new Guid("2EB07EA0-7E70-11D0-A5D6-28DB04C10000");
        public static Guid KSCATEGORY_DRM_DESCRAMBLE = new Guid("FFBB6E3F-CCFE-4D84-90D9-421418B03A8E");
        public static Guid KSCATEGORY_ENCODER = new Guid("19689BF6-C384-48fd-AD51-90E58C79F70B");
        public static Guid KSCATEGORY_ESCALANTE_PLATFORM_DRIVER = new Guid("74F3AEA8-9768-11D1-8E07-00A0C95EC22E");
        public static Guid KSCATEGORY_FILESYSTEM = new Guid("760FED5E-9357-11D0-A3CC-00A0C9223196");
        public static Guid KSCATEGORY_INTERFACETRANSFORM = new Guid("CF1DDA2D-9743-11D0-A3EE-00A0C9223196");
        public static Guid KSCATEGORY_MEDIUMTRANSFORM = new Guid("CF1DDA2E-9743-11D0-A3EE-00A0C9223196");
        public static Guid KSCATEGORY_MICROPHONE_ARRAY_PROCESSOR = new Guid("830A44F2-A32D-476B-BE97-42845673B35A");
        public static Guid KSCATEGORY_MIXER = new Guid("AD809C00-7B88-11D0-A5D6-28DB04C10000");
        public static Guid KSCATEGORY_MULTIPLEXER = new Guid("7A5DE1D3-01A1-452c-B481-4FA2B96271E8");
        public static Guid KSCATEGORY_NETWORK = new Guid("67C9CC3C-69C4-11D2-8759-00A0C9223196");
        public static Guid KSCATEGORY_PREFERRED_MIDIOUT_DEVICE = new Guid("D6C50674-72C1-11D2-9755-0000F8004788");
        public static Guid KSCATEGORY_PREFERRED_WAVEIN_DEVICE = new Guid("D6C50671-72C1-11D2-9755-0000F8004788");
        public static Guid KSCATEGORY_PREFERRED_WAVEOUT_DEVICE = new Guid("D6C5066E-72C1-11D2-9755-0000F8004788");
        public static Guid KSCATEGORY_PROXY = new Guid("97EBAACA-95BD-11D0-A3EA-00A0C9223196");
        public static Guid KSCATEGORY_QUALITY = new Guid("97EBAACB-95BD-11D0-A3EA-00A0C9223196");
        public static Guid KSCATEGORY_REALTIME = new Guid("EB115FFC-10C8-4964-831D-6DCB02E6F23F");
        public static Guid KSCATEGORY_RENDER = new Guid("65E8773E-8F56-11D0-A3B9-00A0C9223196");
        public static Guid KSCATEGORY_SPLITTER = new Guid("0A4252A0-7E70-11D0-A5D6-28DB04C10000");
        public static Guid KSCATEGORY_SYNTHESIZER = new Guid("DFF220F3-F70F-11D0-B917-00A0C9223196");
        public static Guid KSCATEGORY_SYSAUDIO = new Guid("A7C7A5B1-5AF3-11D1-9CED-00A024BF0407");
        public static Guid KSCATEGORY_TEXT = new Guid("6994AD06-93EF-11D0-A3CC-00A0C9223196");
        public static Guid KSCATEGORY_TOPOLOGY = new Guid("DDA54A40-1E4C-11D1-A050-405705C10000");
        public static Guid KSCATEGORY_TVAUDIO = new Guid("A799A802-A46D-11D0-A18C-00A02401DCD4");
        public static Guid KSCATEGORY_TVTUNER = new Guid("A799A800-A46D-11D0-A18C-00A02401DCD4");
        public static Guid KSCATEGORY_VBICODEC = new Guid("07DAD660-22F1-11D1-A9F4-00C04FBBDE8F");
        public static Guid KSCATEGORY_VIDEO = new Guid("6994AD05-93EF-11D0-A3CC-00A0C9223196");
        public static Guid KSCATEGORY_VIRTUAL = new Guid("3503EAC4-1F26-11D1-8AB0-00A0C9223196");
        public static Guid KSCATEGORY_VPMUX = new Guid("A799A803-A46D-11D0-A18C-00A02401DCD4");
        public static Guid KSCATEGORY_WDMAUD = new Guid("3E227E76-690D-11D2-8161-0000F8775BF1");
        public static Guid KSMFT_CATEGORY_AUDIO_DECODER = new Guid("9ea73fb4-ef7a-4559-8d5d-719d8f0426c7");
        public static Guid KSMFT_CATEGORY_AUDIO_EFFECT = new Guid("11064c48-3648-4ed0-932e-05ce8ac811b7");
        public static Guid KSMFT_CATEGORY_AUDIO_ENCODER = new Guid("91c64bd0-f91e-4d8c-9276-db248279d975");
        public static Guid KSMFT_CATEGORY_DEMULTIPLEXER = new Guid("a8700a7a-939b-44c5-99d7-76226b23b3f1");
        public static Guid KSMFT_CATEGORY_MULTIPLEXER = new Guid("059c561e-05ae-4b61-b69d-55b61ee54a7b");
        public static Guid KSMFT_CATEGORY_OTHER = new Guid("90175d57-b7ea-4901-aeb3-933a8747756f");
        public static Guid KSMFT_CATEGORY_VIDEO_DECODER = new Guid("d6c02d4b-6833-45b4-971a-05a4b04bab91");
        public static Guid KSMFT_CATEGORY_VIDEO_EFFECT = new Guid("12e17c21-532c-4a6e-8a1c-40825a736397");
        public static Guid KSMFT_CATEGORY_VIDEO_ENCODER = new Guid("f79eac7d-e545-4387-bdee-d647d7bde42a");
        public static Guid KSMFT_CATEGORY_VIDEO_PROCESSOR = new Guid("302ea3fc-aa5f-47f9-9f7a-c2188bb16302");
        public static Guid GUID_DEVINTERFACE_USB_DEVICE = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");
        public static Guid GUID_DEVINTERFACE_USB_HOST_CONTROLLER = new Guid("3ABF6F2D-71C4-462A-8A92-1E6861E6AF27");
        public static Guid GUID_DEVINTERFACE_USB_HUB = new Guid("F18A0E88-C30C-11D0-8815-00A0C906BED8");
        public static Guid GUID_DEVINTERFACE_WPD = new Guid("6AC27878-A6FA-4155-BA85-F98F491D4F33");
        public static Guid GUID_DEVINTERFACE_WPD_PRIVATE = new Guid("BA0C718F-4DED-49B7-BDD3-FABE28661211");
        public static Guid GUID_DEVINTERFACE_SIDESHOW = new Guid("152E5811-FEB9-4B00-90F4-D32947AE1681");
    }

    [Flags]
    public enum DEVICE_TYPE : uint
    {
        FILE_DEVICE_BEEP = 0x01,
        FILE_DEVICE_CD_ROM = 0x02,
        FILE_DEVICE_CD_ROM_FILE_SYSTEM = 0x03,
        FILE_DEVICE_CONTROLLER = 0x04,
        FILE_DEVICE_DATALINK = 0x05,
        FILE_DEVICE_DFS = 0x06,
        FILE_DEVICE_DISK = 0x07, // IOCTL_DISK_BASE
        FILE_DEVICE_DISK_FILE_SYSTEM = 0x08,
        FILE_DEVICE_FILE_SYSTEM = 0x09,
        FILE_DEVICE_INPORT_PORT = 0x0a,
        FILE_DEVICE_KEYBOARD = 0x0b,
        FILE_DEVICE_MAILSLOT = 0x0c,
        FILE_DEVICE_MIDI_IN = 0x0d,
        FILE_DEVICE_MIDI_OUT = 0x0e,
        FILE_DEVICE_MOUSE = 0x0f,
        FILE_DEVICE_MULTI_UNC_PROVIDER = 0x10,
        FILE_DEVICE_NAMED_PIPE = 0x11,
        FILE_DEVICE_NETWORK = 0x12,
        FILE_DEVICE_NETWORK_BROWSER = 0x13,
        FILE_DEVICE_NETWORK_FILE_SYSTEM = 0x14,
        FILE_DEVICE_NULL = 0x15,
        FILE_DEVICE_PARALLEL_PORT = 0x16,
        FILE_DEVICE_PHYSICAL_NETCARD = 0x17,
        FILE_DEVICE_PRINTER = 0x18,
        FILE_DEVICE_SCANNER = 0x19,
        FILE_DEVICE_SERIAL_MOUSE_PORT = 0x1a,
        FILE_DEVICE_SERIAL_PORT = 0x1b,
        FILE_DEVICE_SCREEN = 0x1c,
        FILE_DEVICE_SOUND = 0x1d,
        FILE_DEVICE_STREAMS = 0x1e,
        FILE_DEVICE_TAPE = 0x1f,
        FILE_DEVICE_TAPE_FILE_SYSTEM = 0x20,
        FILE_DEVICE_TRANSPORT = 0x21,
        FILE_DEVICE_UNKNOWN = 0x22,
        FILE_DEVICE_VIDEO = 0x23,
        FILE_DEVICE_VIRTUAL_DISK = 0x24,
        FILE_DEVICE_WAVE_IN = 0x25,
        FILE_DEVICE_WAVE_OUT = 0x26,
        FILE_DEVICE_8042_PORT = 0x27,
        FILE_DEVICE_NETWORK_REDIRECTOR = 0x28,
        FILE_DEVICE_BATTERY = 0x29,
        FILE_DEVICE_BUS_EXTENDER = 0x2a,
        FILE_DEVICE_MODEM = 0x2b,
        FILE_DEVICE_VDM = 0x2c,
        FILE_DEVICE_MASS_STORAGE = 0x2d, // IOCTL_STORAGE_BASE
        FILE_DEVICE_SMB = 0x2e,
        FILE_DEVICE_KS = 0x2f,
        FILE_DEVICE_CHANGER = 0x30, // IOCTL_CHANGER_BASE
        FILE_DEVICE_SMARTCARD = 0x31,
        FILE_DEVICE_ACPI = 0x32,
        FILE_DEVICE_DVD = 0x33,
        FILE_DEVICE_FULLSCREEN_VIDEO = 0x34,
        FILE_DEVICE_DFS_FILE_SYSTEM = 0x35,
        FILE_DEVICE_DFS_VOLUME = 0x36,
        FILE_DEVICE_SERENUM = 0x37,
        FILE_DEVICE_TERMSRV = 0x38,
        FILE_DEVICE_KSEC = 0x39,
        FILE_DEVICE_FIPS = 0x3A,
        FILE_DEVICE_INFINIBAND = 0x3B,
        FILE_DEVICE_VMBUS = 0x3E,
        FILE_DEVICE_CRYPT_PROVIDER = 0x3F,
        FILE_DEVICE_WPD = 0x40,
        FILE_DEVICE_BLUETOOTH = 0x41,
        FILE_DEVICE_MT_COMPOSITE = 0x42,
        FILE_DEVICE_MT_TRANSPORT = 0x43,
        FILE_DEVICE_BIOMETRIC = 0x44,
        FILE_DEVICE_PMI = 0x45,
        FILE_DEVICE_EHSTOR = 0x46,
        FILE_DEVICE_DEVAPI = 0x47,
        FILE_DEVICE_GPIO = 0x48,
        FILE_DEVICE_USBEX = 0x49,
        FILE_DEVICE_CONSOLE = 0x50,
        FILE_DEVICE_NFP = 0x51,
        FILE_DEVICE_SYSENV = 0x52,
        FILE_DEVICE_VIRTUAL_BLOCK = 0x53,
        FILE_DEVICE_POINT_OF_SERVICE = 0x54,
        FILE_DEVICE_STORAGE_REPLICATION = 0x55,
        FILE_DEVICE_TRUST_ENV = 0x56 // IOCTL_VOLUME_BASE
    }

    /// <summary>
    /// Flags controlling what is included in the device information set built by SetupDiGetClassDevs
    /// </summary>
    [Flags]
    public enum DIGCF : int
    {
        DIGCF_DEFAULT = 0x00000001,    // only valid with DIGCF_DEVICEINTERFACE
        DIGCF_PRESENT = 0x00000002,
        DIGCF_ALLCLASSES = 0x00000004,
        DIGCF_PROFILE = 0x00000008,
        DIGCF_DEVICEINTERFACE = 0x00000010,
    }
    #endregion

    public class NativeAdapterException : Exception
    {
        public NativeAdapterException()
        {
        }
        //public NativeAdapterException(int win32Err) : base(win32Err) {}

        public NativeAdapterException(string mesage) : base(mesage) {}
        public NativeAdapterException(string mesage, Exception inner) : base(mesage, inner)
        {
        }

        #region HELPER METHODS
        public static string Win32ErorMessage(int win32Err)
        {
            return (new System.ComponentModel.Win32Exception(win32Err).Message);
        }
        public static string Win32ErorMessage()
        {
            return (Win32ErorMessage(Marshal.GetLastWin32Error()));
        }
        public static string Win32ErrorMessageWithContext(string contextMsg, int win32Error)
        {
            string w32Msg = Win32ErorMessage(win32Error);
            string message = string.Format("{0} : {1} [0x{2:4X}", contextMsg, w32Msg, win32Error);
            return message;
        }
        #endregion
    }
    public class Win32
    {
        public Win32()
        {
            string methodName = Method.Name;
            Console.WriteLine("{0}{1}()", ClassName, methodName);
        }

        protected MethodBase Method { get { return System.Reflection.MethodBase.GetCurrentMethod(); } }
        protected string ClassName { get { return Method.ReflectedType.Name; } }
        // Console.WriteLine("{0}.{1}()", ClassName, Method.Name);

        #region WIN32 FILE TYPES
        public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        public const uint PATH_SIZE = 256;
        public const int BUFFER_SIZE = (int)PATH_SIZE;
        #endregion

        #region WIN32 IOCTL
        public static class IOCTL_FUNCTION {
            public const int USB_GET_NODE_CONNECTION_INFORMATION = 0x22040C;
            public const int USB_GET_DESCRIPTOR_FROM_NODE_CONNECTION = 0x220410;
            public const int USB_GET_NODE_CONNECTION_NAME = 0x220414;
            public const int USB_GET_NODE_INFORMATION = 0x220408;
            public const int USB_GET_NODE_CONNECTION_INFORMATION_EX = 0x220448;
            public const int STORAGE_GET_DEVICE_NUMBER = 0x2D1080;
        }
        #endregion

        #region WIN32 IOCTL METHOD
        public const uint METHOD_BUFFERED = 0;
        public const uint METHOD_IN_DIRECT = 1;
        public const uint METHOD_OUT_DIRECT = 2;
        public const uint METHOD_NEITHER = 3;
        #endregion
        #region WIN32 FILE ACCESS FLAGS
        public const uint FILE_ANY_ACCESS = 0;
        public const uint FILE_READ_ACCESS = 1;
        public const uint FILE_WRITE_ACCESS = 2;
        public const uint FILE_DEVICE_DISK = 0x0007;
        public const uint FILE_DEVICE_PROTOCOL = 0x8000;
        public const uint FILE_DEVICE_PHYSICAL_NETCARD = 0x0017;

        #endregion

        #region
        public const int USB_DEVICE_DESCRIPTOR_TYPE = 1;
        public const int USB_STRING_DESCRIPTOR_TYPE = 3;
        #endregion
        #region WIN32 Defined Types
        [StructLayout(LayoutKind.Sequential)]
        public sealed class SP_DEVINFO_DATA
        {
            /// <summary>
            /// size of structure in bytes
            /// </summary>
            public UInt32 cbSize;
            public Guid classGuid;
            public UInt32 devInst;
            public IntPtr reserved;

            public void Init() { cbSize = (uint)Marshal.SizeOf(typeof(SP_DEVINFO_DATA)); }
            public UInt32 Size { get { return cbSize; } }
            public Guid ClassGuid { get { return classGuid; } }
            public UInt32 DevInst { get { return devInst; } }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVICE_INTERFACE_DATA
        {
            /// <summary>
            /// size of structure in bytes
            /// </summary>
            public UInt32 cbSize;
            /// <summary>
            /// GUID of the device interface class.
            /// </summary>
            public Guid interfaceClassGuid;
            public UInt32 flags;
            private UIntPtr reserved;

            public void Init() { cbSize = (UInt32)Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA)); }
            public UInt32 Size { get { return cbSize; } }
            public Guid InterfaceClassGuid { get { return interfaceClassGuid; } }
            public UInt32 Flags { get { return flags; } }
        }

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string DevicePath;

            public void Init() { cbSize = (UInt32)Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DETAIL_DATA)); }
            public void Init2()
            {
                        if (IntPtr.Size == 8) // for 64 bit operating systems
                            this.cbSize = 8; 
                        else
                            this.cbSize = 4 + Marshal.SystemDefaultCharSize; // for 32 bit systems

            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct USB_HUB_DESCRIPTOR
        {
            public byte bDescriptorLength;
            public byte bDescriptorType;
            public byte bNumberOfPorts;
            public short wHubCharacteristics;
            public byte bPowerOnToPowerGood;
            public byte bHubControlCurrent;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] bRemoveAndPowerMask;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct USB_STRING_DESCRIPTOR
        {
            public byte bLength;
            public byte bDescriptorType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string bString;
        }
        #endregion

        #region DLL import

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool DeviceIoControl(
            [MarshalAs(UnmanagedType.U4)] int hDevice,
            [MarshalAs(UnmanagedType.U4)] uint dwIoControlCode,
            IntPtr lpInBuffer,
            [MarshalAs(UnmanagedType.U4)]uint nInBufferSize,
            IntPtr lpOutBuffer,
            [MarshalAs(UnmanagedType.U4)]uint nOutBufferSize,
            [MarshalAs(UnmanagedType.U4)]out int lpBytesReturned,
            [MarshalAs(UnmanagedType.U4)]uint lpOverlapped);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFile(
           string fileName,
           [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
           [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
           IntPtr securityAttributes,
           [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
           int flags,
           IntPtr template
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(
           IntPtr hDevInfo,
            //           ref DevinfoData devInfo,
            IntPtr devInfo,
           //           uint devInfo,
           ref Guid interfaceClassGuid,
           UInt32 memberIndex,
           ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
        );

        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern bool SetupDiGetDeviceInterfaceDetail(
            IntPtr lpDeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA oInterfaceData,
               //IntPtr lpDeviceInterfaceDetailData,         //To get the nRequiredSize pass IntPtr.Zero
            ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
            [In, MarshalAs(UnmanagedType.U4)] uint nDeviceInterfaceDetailDataSize,
            [Out, MarshalAs(UnmanagedType.U4)] out uint nRequiredSize,
            ref SP_DEVINFO_DATA deviceInfoData
            );

        //[DllImport( "setupapi.dll", SetLastError = true )]
        //internal static extern bool SetupDiGetDeviceInterfaceDetail (
        //    IntPtr lpDeviceInfoSet,
        //    ref DeviceInterfaceData oInterfaceData,
        //    //ref DeviceInterfaceDetailData oDetailData,  //We have the size -> send struct
        //    IntPtr deviceInterfaceDetailData,
        //    uint nDeviceInterfaceDetailDataSize,
        //    ref uint nRequiredSize,
        //    ref DevinfoData deviceInfoData );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetupDiGetClassDevs(           // 1st form using a ClassGUID
           ref Guid ClassGuid,
           int Enumerator,
           IntPtr hwndParent,
           int Flags
        );
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiDestroyDeviceInfoList(
             [In] IntPtr DeviceInfoSet
        );
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern int CM_Get_Device_ID(
           IntPtr hDeviceInstance,
           IntPtr Buffer,
           int BufferLen,
           int ulFlags
        );
        [DllImport("setupapi.dll")]
        public static extern int CM_Get_Parent(
           [Out] out IntPtr pdnDevInst,
           [In, MarshalAs(UnmanagedType.U4)] uint dnDevInst,
           int ulFlags
        );
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetVolumeNameForVolumeMountPoint(
            string volumeName,
            StringBuilder uniqueVolumeName,
            int uniqueNameBufferCapacity
            );

        #endregion

        #region Functions

        internal uint CTL_CODE(uint DeviceType, uint Function, uint Method, uint Access)
        {
            return ((DeviceType << 16) | (Access << 14) | (Function << 2) | Method);
        }

        internal uint IOCTL_USB_GET_NODE_INFORMATION()
        {
            return CTL_CODE((uint)DEVICE_TYPE.FILE_DEVICE_UNKNOWN, IOCTL_FUNCTION.USB_GET_NODE_INFORMATION, METHOD_BUFFERED, FILE_ANY_ACCESS);
        }

        internal uint IOCTL_USB_GET_NODE_CONNECTION_INFORMATION()
        {
            return CTL_CODE((uint)DEVICE_TYPE.FILE_DEVICE_UNKNOWN, IOCTL_FUNCTION.USB_GET_NODE_CONNECTION_INFORMATION, METHOD_BUFFERED, FILE_ANY_ACCESS);
        }

        internal uint IOCTL_USB_GET_DESCRIPTOR_FROM_NODE_CONNECTION()
        {
            return CTL_CODE((uint)DEVICE_TYPE.FILE_DEVICE_USBEX, IOCTL_FUNCTION.USB_GET_DESCRIPTOR_FROM_NODE_CONNECTION, METHOD_BUFFERED, FILE_ANY_ACCESS);
        }
    }
    #endregion

}
