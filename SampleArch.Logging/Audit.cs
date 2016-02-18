using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using log4net;
using log4net.Config;
using log4net.Core;

namespace SampleArch.Logging
{
    /// <summary>
    ///     Audit
    /// </summary>
    public class Audit : LogImpl
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Audit" /> class.
        /// </summary>
        /// <param name="logger">The logger to wrap.</param>
        /// <remarks>
        ///     Construct a new wrapper for the specified logger.
        /// </remarks>
        public Audit(ILogger logger)
            : base(logger)
        {
        }

        #region Logging

        //

        /// <summary>
        ///     Logs a message object with the <c>INFO</c> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <remarks>
        ///     <para>
        ///         This method first checks if this logger is <c>INFO</c>
        ///         enabled by comparing the level of this logger with the
        ///         <c>INFO</c> level. If this logger is
        ///         <c>INFO</c> enabled, then it converts the message object
        ///         (passed as parameter) to a string by invoking the appropriate
        ///         <see cref="T:log4net.ObjectRenderer.IObjectRenderer" />. It then
        ///         proceeds to call all the registered appenders in this logger
        ///         and also higher in the hierarchy depending on the value of
        ///         the additivity flag.
        ///     </para>
        ///     <para>
        ///         <b>WARNING</b> Note that passing an <see cref="T:System.Exception" />
        ///         to this method will print the name of the <see cref="T:System.Exception" />
        ///         but no stack trace. To print a stack trace use the
        ///         <see cref="M:Info(object,Exception)" /> form instead.
        ///     </para>
        /// </remarks>
        public override void Info(object message)
        {
            base.Info(CommonFormat(message));
        }

        /// <summary>
        ///     Logs a message object with the <c>INFO</c> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        ///     <para>
        ///         Logs a message object with the <c>INFO</c> level including
        ///         the stack trace of the <see cref="T:System.Exception" /><paramref name="exception" />
        ///         passed as a parameter.
        ///     </para>
        ///     <para>
        ///         See the <see cref="M:Info(object)" /> form for more detailed information.
        ///     </para>
        /// </remarks>
        /// <seealso cref="M:Info(object)" />
        public override void Info(object message, Exception exception)
        {
            base.Info(CommonFormat(message), exception);
        }

        /// <summary>
        ///     Informations the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public override void InfoFormat(string format, params object[] args)
        {
            base.InfoFormat(CommonFormat(format), args);
        }

        /// <summary>
        ///     Logs a message object with the <c>DEBUG</c> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <remarks>
        ///     <para>
        ///         This method first checks if this logger is <c>DEBUG</c>
        ///         enabled by comparing the level of this logger with the
        ///         <c>DEBUG</c> level. If this logger is
        ///         <c>DEBUG</c> enabled, then it converts the message object
        ///         (passed as parameter) to a string by invoking the appropriate
        ///         <see cref="T:log4net.ObjectRenderer.IObjectRenderer" />. It then
        ///         proceeds to call all the registered appenders in this logger
        ///         and also higher in the hierarchy depending on the value of the
        ///         additivity flag.
        ///     </para>
        ///     <para>
        ///         <b>WARNING</b> Note that passing an <see cref="T:System.Exception" />
        ///         to this method will print the name of the <see cref="T:System.Exception" />
        ///         but no stack trace. To print a stack trace use the
        ///         <see cref="M:Debug(object,Exception)" /> form instead.
        ///     </para>
        /// </remarks>
        public override void Debug(object message)
        {
            base.Debug(CommonFormat(message));
        }

        /// <summary>
        ///     Debugs the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public override void DebugFormat(string format, params object[] args)
        {
            base.DebugFormat(CommonFormat(format), args);
        }

        /// <summary>
        ///     Logs a message object with the <c>DEBUG</c> level
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        ///     <para>
        ///         Logs a message object with the <c>DEBUG</c> level including
        ///         the stack trace of the <see cref="T:System.Exception" /><paramref name="exception" /> passed
        ///         as a parameter.
        ///     </para>
        ///     <para>
        ///         See the <see cref="M:Debug(object)" /> form for more detailed information.
        ///     </para>
        /// </remarks>
        /// <seealso cref="M:Debug(object)" />
        public override void Debug(object message, Exception exception)
        {
            base.Debug(CommonFormat(message), exception);
        }

        /// <summary>
        ///     Logs a message object with the <c>ERROR</c> level
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        ///     <para>
        ///         Logs a message object with the <c>ERROR</c> level including
        ///         the stack trace of the <see cref="T:System.Exception" /><paramref name="exception" />
        ///         passed as a parameter.
        ///     </para>
        ///     <para>
        ///         See the <see cref="M:Error(object)" /> form for more detailed information.
        ///     </para>
        /// </remarks>
        /// <seealso cref="M:Error(object)" />
        public override void Error(object message, Exception exception)
        {
            base.Error(CommonFormat(message), exception);
        }

        /// <summary>
        ///     Logs a message object with the <c>ERROR</c> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <remarks>
        ///     <para>
        ///         This method first checks if this logger is <c>ERROR</c>
        ///         enabled by comparing the level of this logger with the
        ///         <c>ERROR</c> level. If this logger is
        ///         <c>ERROR</c> enabled, then it converts the message object
        ///         (passed as parameter) to a string by invoking the appropriate
        ///         <see cref="T:log4net.ObjectRenderer.IObjectRenderer" />. It then
        ///         proceeds to call all the registered appenders in this logger and
        ///         also higher in the hierarchy depending on the value of the
        ///         additivity flag.
        ///     </para>
        ///     <para>
        ///         <b>WARNING</b> Note that passing an <see cref="T:System.Exception" /> to this
        ///         method will print the name of the <see cref="T:System.Exception" /> but no
        ///         stack trace. To print a stack trace use the
        ///         <see cref="M:Error(object,Exception)" /> form instead.
        ///     </para>
        /// </remarks>
        public override void Error(object message)
        {
            base.Error(CommonFormat(message));
        }

        /// <summary>
        ///     Errors the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public override void ErrorFormat(string format, params object[] args)
        {
            base.ErrorFormat(CommonFormat(format), args);
        }

        /// <summary>
        ///     Logs a message object with the <c>FATAL</c> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <remarks>
        ///     <para>
        ///         This method first checks if this logger is <c>FATAL</c>
        ///         enabled by comparing the level of this logger with the
        ///         <c>FATAL</c> level. If this logger is
        ///         <c>FATAL</c> enabled, then it converts the message object
        ///         (passed as parameter) to a string by invoking the appropriate
        ///         <see cref="T:log4net.ObjectRenderer.IObjectRenderer" />. It then
        ///         proceeds to call all the registered appenders in this logger and
        ///         also higher in the hierarchy depending on the value of the
        ///         additivity flag.
        ///     </para>
        ///     <para>
        ///         <b>WARNING</b> Note that passing an <see cref="T:System.Exception" /> to this
        ///         method will print the name of the <see cref="T:System.Exception" /> but no
        ///         stack trace. To print a stack trace use the
        ///         <see cref="M:Fatal(object,Exception)" /> form instead.
        ///     </para>
        /// </remarks>
        public override void Fatal(object message)
        {
            base.Fatal(CommonFormat(message));
        }

        /// <summary>
        ///     Fatals the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public override void FatalFormat(string format, params object[] args)
        {
            base.FatalFormat(CommonFormat(format), args);
        }

        /// <summary>
        ///     Logs a message object with the <c>FATAL</c> level
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        ///     <para>
        ///         Logs a message object with the <c>FATAL</c> level including
        ///         the stack trace of the <see cref="T:System.Exception" /><paramref name="exception" />
        ///         passed as a parameter.
        ///     </para>
        ///     <para>
        ///         See the <see cref="M:Fatal(object)" /> form for more detailed information.
        ///     </para>
        /// </remarks>
        /// <seealso cref="M:Fatal(object)" />
        public override void Fatal(object message, Exception exception)
        {
            base.Fatal(CommonFormat(message), exception);
        }

        /// <summary>
        ///     The _log
        /// </summary>
        private static ILog _log;

        /// <summary>
        ///     Gets the log.
        /// </summary>
        /// <value>
        ///     The log.
        /// </value>
        public static ILog Log
        {
            get
            {
                if (_log != null) return _log;

                XmlConfigurator.Configure();
                _log = LogManager.GetLogger("SortingHat.Logging");

                return _log;
            }
        }

        /// <summary>
        ///     Commons the format.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns></returns>
        private string CommonFormat(object message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Trace.WriteLine("message: " + message);
            Trace.WriteLine("member name: " + memberName);
            Trace.WriteLine("source file path: " + sourceFilePath);
            Trace.WriteLine("source line number: " + sourceLineNumber);

            return string.Format("[{0}] {1} :: {2} - src: {3} - Line#: {4}", GetType(), memberName, message,
                sourceFilePath, sourceLineNumber);
        }

        //

        #endregion Logging
    }
}