using Pithline.FMS.Framework.Web.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.Framework.Web.Exceptions
{
    /// <summary>
    /// The event log based exception handler
    /// </summary>
    [ExceptionHandler(Type = "EventLog")]
    public class EventLogExceptionHandler : IExceptionHandler
    {
        private string _source;
        private string _log;

        public EventLogExceptionHandler()
        {
            this._source = MefHelper.Helper.App.ApplicationName;
            this._log = "Application"; 
        }

        public void Handle(Exception ex)
        {
            try
            {
                if (!EventLog.SourceExists(this._source))
                    EventLog.CreateEventSource(this._source, this._log);

                EventLog.WriteEntry(this._source, ex.ToString(), EventLogEntryType.Error, 619);
            }
            catch (Exception)
            { }
        }
    }
}
