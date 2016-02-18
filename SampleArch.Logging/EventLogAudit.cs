using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Security;

namespace SampleArch.Logging
{
    /// <summary>
    ///     EventLogService
    /// </summary>
    public class EventLogAudit
    {
        /// <summary>
        ///     The EVT_LOG_NAME
        /// </summary>
        private const string EvtLogName = "SortingHat";

        /// <summary>
        ///     The EVT_APP_SOURCE
        /// </summary>
        private const string EvtAppSource = "SortingHat";

        /// <summary>
        ///     The _audit log
        /// </summary>
        private EventLog _auditLog;

        /// <summary>
        ///     The _log detail level
        /// </summary>
        private LogEntryInfoDetailLevel _logDetailLevel;

        /// <summary>
        ///     Initializes the specified log detail level.
        /// </summary>
        /// <param name="logDetailLevel">The log detail level.</param>
        /// <exception cref="System.Diagnostics.Eventing.Reader.EventLogNotFoundException"></exception>
        /// <exception cref="System.Security.SecurityException">EventLog SecurityException: {0}</exception>
        public void Initialize(LogEntryInfoDetailLevel logDetailLevel)
        {
            //=======================================================================
            // We depend on the installer to create the application event source.
            // FOR DEV: powershell: New-EventLog -LogName SortingHat -Source SortingHat
            //=======================================================================
            try
            {
                if (!EventLog.SourceExists(EvtAppSource))
                {
                    throw new EventLogNotFoundException(
                        string.Format("Event log source '{0}' with log name '{1}' was not found. Please create it.",
                            EvtAppSource, EvtLogName));
                }
            }
            catch (SecurityException securityException)
            {
                throw new SecurityException("EventLog SecurityException: {0}", securityException);
            }

            _auditLog = new EventLog(EvtLogName) {Source = EvtAppSource};
            _logDetailLevel = logDetailLevel;
        }

        /// <summary>
        ///     Audits the specified success.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void Audit(bool success, string format, object[] args = null)
        {
            if (args != null)
            {
                format = string.Format(format, args);
            }
            WriteToEventLog(format, success ? EventLogEntryType.SuccessAudit : EventLogEntryType.FailureAudit);
        }

        /// <summary>
        ///     Logs the specified entry type.
        /// </summary>
        /// <param name="entryType">Type of the entry.</param>
        /// <param name="message">The message.</param>
        /// <param name="detailLevel">The detail level.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException">null</exception>
        public void Log(LogEntryType entryType, string message,
            LogEntryInfoDetailLevel detailLevel = LogEntryInfoDetailLevel.AlwaysLog)
        {
            if (_logDetailLevel < detailLevel && entryType == LogEntryType.Information)
                return;

            var eventLogEntryType = EventLogEntryType.Information;

            switch (entryType)
            {
                case LogEntryType.Error:
                    eventLogEntryType = EventLogEntryType.Error;
                    break;
                case LogEntryType.Warning:
                    eventLogEntryType = EventLogEntryType.Warning;
                    break;
                case LogEntryType.Information:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(entryType.ToString());
            }

            WriteToEventLog(message, eventLogEntryType);
        }

        /// <summary>
        ///     Writes to event log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logEntryType">Type of the log entry.</param>
        private void WriteToEventLog(string message, EventLogEntryType logEntryType)
        {
            _auditLog.WriteEntry(message, logEntryType);
        }
    }

    /// <summary>
    ///     LogEntryInfoDetailLevel enumeration
    /// </summary>
    public enum LogEntryInfoDetailLevel
    {
        /// <summary>
        ///     The low
        /// </summary>
        Low = 0,

        /// <summary>
        ///     The normal
        /// </summary>
        Normal = 5000,

        /// <summary>
        ///     The high
        /// </summary>
        High = 10000,

        /// <summary>
        ///     The always log
        /// </summary>
        AlwaysLog = int.MinValue
    }

    /// <summary>
    ///     LogEntryType enumeration
    /// </summary>
    public enum LogEntryType
    {
        /// <summary>
        ///     The information
        /// </summary>
        Information = 0,

        /// <summary>
        ///     The warning
        /// </summary>
        Warning = 5000,

        /// <summary>
        ///     The error
        /// </summary>
        Error = 10000
    }
}