using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateWinService
{
    // used to log the information - errors, excpetion info or any other info
    // Ref - http://www.codeproject.com/Articles/140911/log-net-Tutorial
    public class Logger
    {
        //Here is the once-per-class call to initialize the log object
        //private static readonly log4net.ILog log =
        //    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static log4net.ILog log = null;

        /// <summary>
        /// Used to log the debug messages
        /// </summary>
        internal static void LogDebug(string message)
        {
            log = GetLogger();
            log.Debug(message);
        }

        /// <summary>
        /// Used to log ordinary information
        /// </summary>
        internal static void LogInfo(string message)
        {
            log = GetLogger();
            log.Info(message);
        }

        /// <summary>
        /// Used to log ordinary information
        /// </summary>
        internal static void LogWarn(string message)
        {
            log = GetLogger();
            log.Warn(message);
        }

        /// <summary>
        /// Used to log exceptions
        /// </summary>
        internal static void LogExceptions(string message, Exception exception)
        {
            log = GetLogger();
            log.Error(message, exception);
        }

        /// <summary>
        /// Used to log fatal/catastrpic errors
        /// </summary>
        internal static void LogFatalErrors(string message, Exception exception)
        {
            log = GetLogger();
            log.Fatal(message, exception);
        }

        /// <summary>
        /// Used to create and return the logger object
        /// Ref - http://stackoverflow.com/questions/3095696/how-do-i-get-the-calling-method-name-and-type-using-reflection
        /// </summary>
        private static log4net.ILog GetLogger()
        {
            Type invokerClass = IdentifyCallerClass();
            log = log4net.LogManager.GetLogger(invokerClass);
            return log;
        }

        /// <summary>
        /// Used to identify the caller/invoker class name
        /// </summary>
        private static Type IdentifyCallerClass()
        {
            Type declaringType = null;
            StackTrace stackTrace = new StackTrace();           // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

            /*
             stackFrames[0] = current method - IdentifyCallerClass()
             stackFrames[1] = caller of the current method - GetLogger()
             stackFrames[2] = caller of the GetLogger() - LogData()
             stackFrames[3] = caller of the LogData() - we need this for logging
             */
            StackFrame callingFrame = stackFrames[3];
            MethodBase method = callingFrame.GetMethod();
            //string methodName = method.Name;
            //string className = method.DeclaringType.Name;
            declaringType = method.DeclaringType;
            return declaringType;
        }

    }
}
