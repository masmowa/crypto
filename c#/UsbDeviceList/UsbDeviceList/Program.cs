using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;

namespace UsbDeviceList
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(ReportUnhandledExceptionHandler);
#if false // event log
        // Since we can't prevent the app from terminating, log this to the event log.
        if (!EventLog.SourceExists("ThreadException"))
        {
            EventLog.CreateEventSource("ThreadException", "Application");
        }


        // Create an EventLog instance and assign its source.
        EventLog myLog = new EventLog();
        myLog.Source = "ThreadException";
        myLog.WriteEntry(errorMsg + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
#endif
        }

        /// <summary>
        /// Report Unhandled exception before exiting
        /// </summary>
        /// <see cref="https://msdn.microsoft.com/en-us/library/system.appdomain.unhandledexception.aspx"/> 
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void ReportUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Console.WriteLine("ReportExceptionHandler caught : " + e.Message);
            Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
        }
    }
}
