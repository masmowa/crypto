using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.ComponentModel;
using Native.Adapter;

namespace UsbDeviceList.Native
{
    class Win32Adapter : Win32
    {
        protected Win32 win32;
        public Win32Adapter()
        {
            Console.WriteLine(">>{0}.{1}()", ClassName, Method.Name);
            win32 = new Win32();
        }
        protected new MethodBase Method
        {
            get { return System.Reflection.MethodBase.GetCurrentMethod(); }
        }
        protected new string ClassName
        {
            get { return Method.ReflectedType.Name; }
        }
        // Console.WriteLine("{0}.{1}()", ClassName, Method.Name);

        private IntPtr GetRootIntPtr(Guid GuidToWorkWith)
        {
            MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            string className = method.ReflectedType.Name; Console.WriteLine("{0}.{1}()", className, methodName);
            IntPtr hDevInfoSet = SetupDiGetClassDevs(ref GuidToWorkWith
                                                        , 0
                                                        , IntPtr.Zero
                                                        , (int)(DIGCF.DIGCF_PRESENT | DIGCF.DIGCF_DEVICEINTERFACE));


            if (hDevInfoSet == INVALID_HANDLE_VALUE)
            {
                //throw (new ApplicationException("[CCore::GetRootIntPtr] Can not get handle to root devices list"));
                int lastErr = Marshal.GetLastWin32Error();
                string msg = NativeAdapterException.Win32ErrorMessageWithContext("[CCore::GetRootIntPtr] Can not get handle to root devices list", lastErr);

                throw (new NativeAdapterException(msg, new Win32Exception (lastErr)));
            }
            return hDevInfoSet;
        }

        private void SampleGetDeviceInterfaceData()
        {
            Guid DiskGUID = GUID_DEVINTERFACE.GUID_DEVINTERFACE_DISK; // new Guid(GUID_DEVINTERFACE_DISK);
            Console.WriteLine(">> {0}.{1}()", ClassName, Method.Name);

            // We start at the "root" of the device tree and look for all
            // devices that match the interface GUID of a disk
            IntPtr h = SetupDiGetClassDevs(ref DiskGUID, 0, IntPtr.Zero,
                (int)(DIGCF.DIGCF_PRESENT | DIGCF.DIGCF_DEVICEINTERFACE)
                );
            if (h != INVALID_HANDLE_VALUE)
            {
                bool Success = true;
                uint i = 0;            
                                      
                while (Success)
                {
                    // create a Device Interface Data structure
                    SP_DEVICE_INTERFACE_DATA dia = new SP_DEVICE_INTERFACE_DATA();
                    dia.cbSize = Marshal.SizeOf(dia);

                    // start the enumeration 
                    Success = SetupDiEnumDeviceInterfaces(h, IntPtr.Zero, ref DiskGUID, i, ref dia);
                    if (Success)
                    {
                        // build a DevInfo Data structure
                        SP_DEVINFO_DATA da = new SP_DEVINFO_DATA();
                        da.Init();

                        // build a Device Interface Detail Data structure
                        SP_DEVICE_INTERFACE_DETAIL_DATA didd = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                        // note: this is for vista32
                        // didd.cbSize = 4 + Marshal.SystemDefaultCharSize; // trust me :)
                        didd.Init();
                        // now we can get some more detailed information
                        uint nRequiredSize = 0;
                        uint nBytes = BUFFER_SIZE;
                        if (SetupDiGetDeviceInterfaceDetail(h, ref dia, ref didd, nBytes, out nRequiredSize, ref da))
                        {
                            // current InstanceID is at the "USBSTOR" level, so we
                            // need up "move up" one level to get to the "USB" level
                            IntPtr ptrPrevious;
                            CM_Get_Parent(out ptrPrevious, da.devInst, 0);

                            // Now we get the InstanceID of the USB level device
                            IntPtr ptrInstanceBuf = Marshal.AllocHGlobal((int)nBytes);
                            CM_Get_Device_ID(ptrPrevious, ptrInstanceBuf, (int)nBytes, 0);
                            string InstanceID = Marshal.PtrToStringAuto(ptrInstanceBuf);

                            Marshal.FreeHGlobal(ptrInstanceBuf);
                        }
                        i++;
                    }
                }
            }
            SetupDiDestroyDeviceInfoList(h);

        }

    }
}
