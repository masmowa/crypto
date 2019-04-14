using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MediaFoundation.Net;
using MediaFoundation;
using SharpDX;
using SharpDX.MediaFoundation.DirectX;
using NativeAdapter.MediaFoundation.Interop;
using NMI = NativeAdapter.MediaFoundation.Interop;


namespace MFCaptureD3Dcs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // STARUP Media Foundation
            int hr = 
            NMI.Structures.MFAdapter.MFStartup(NMI.Enums.MFStartup.LITE);
            Console.WriteLine("MediaFoundation Startup result[{0:X}]", hr);
            devNotify = new DevNotify();
            devNotify.RegisterDeviceNotification(this.Handle);
        }

        private void selectDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Selecting device");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MFAPI.MFShutdown();
            devNotify.UnregisterUsbDeviceNotification();
        }

        protected void OnChooseDevice(bool prompt)
        {

        }
        protected DevNotify devNotify;

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Form1_Load()");
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Console.WriteLine("Form1_Shown()");

        }
    }
}
