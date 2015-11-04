using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace jolcode
{
   public class Logclass : IDisposable
    {
        private string _LogName;
        private string _SourceName;
        private string _MessageFile;
        private int _CategoryCount;
        private long _DisplayNameMsgId;
        private EventLog _MYEventLog;
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public string LogName
        {
            get
            {
                return _LogName;
            }set
            {
                _LogName = value;
            }
        }
        public string SourceName
        {
            get
            {
                return _SourceName;
            }
            set
            {
                _SourceName = value;
            }
        }
        public string MessageFile
        {
            get
            {
                return _MessageFile;
            }set
            {
                _MessageFile = value;
            }
        }
        public int CategoryCount
        {
            get
            {
                return _CategoryCount;
            }set
            {
                _CategoryCount = value;
            }
        }
        public long DisplayNameMsgId
        {
            get
            {
                return _DisplayNameMsgId;
            }set
            {
                _DisplayNameMsgId = value;
            }
        }

        public EventLog MYEventLog
        {
            get
            {
                return _MYEventLog;
            }set
            {
                _MYEventLog = value;
            }
        }

        public  void CreateEvent()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventSourceCreationData mySourceData = new EventSourceCreationData(SourceName, LogName);

                    if (!System.IO.File.Exists(MessageFile))
                    {
                        Console.WriteLine("Input message resource file does not exist - {0}",
                            MessageFile);
                        MessageFile = "";
                    }
                    else
                    {
                        //mySourceData.MessageResourceFile = MessageFile;
                        //mySourceData.CategoryResourceFile = MessageFile;
                        //mySourceData.CategoryCount = CategoryCount;
                        //mySourceData.ParameterResourceFile = MessageFile;

                        Console.WriteLine("Event source message resource file set to {0}", MessageFile);
                    }

                    Console.WriteLine("Registering new source for event log.");
                    EventLog.CreateEventSource(mySourceData);
                }
                else
                {
                    LogName = EventLog.LogNameFromSourceName(SourceName, ".");
                }

                MYEventLog = new EventLog(LogName, ".", SourceName);

                if (MessageFile.Length > 0)
                {
                    MYEventLog.RegisterDisplayName(MessageFile, DisplayNameMsgId);
                }

               
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region Dispose



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Logclass()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources AnotherResource 

             
                //if (managedResource != null)
                //{
                //    managedResource.Dispose();
                //    managedResource = null;
                //}
            }
            // free native resources if there are any.
            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
        }

        #endregion

    }//end
}//end
