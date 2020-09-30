using AspectCore.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace SDT.BaseTool
{
    public static partial class LoggerExtensions
    {
        #region Task
        /// <summary>
        /// Task 出现错误记录日志,并返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static T ResultEx<T>(Task<T> task, string errorMsg = "") => task.CatchEx(errorMsg).Result;

        /// <summary>
        ///  Task 出现错误记录日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static Task<T> CatchEx<T>(this Task<T> task, string errorMsg = "") => task.ContinueWith(c =>
        {
            if (!c.IsFaulted)
            {
                return c.Result;
            }

            LogUtility.Error(errorMsg, c.Exception);
            return default;
        });

        /// <summary>
        ///  Task 出现错误记录日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="errorMsg"></param>
        public static void CatchEx<T>(this Task task, string errorMsg = "") => task.ContinueWith(c =>
        {
            if (c.IsFaulted)
            {
                LogUtility.Error(errorMsg, c.Exception);
            }
        });
        #endregion

        #region LoggerLevel
        public static void Trace(this ILogger logger, Exception ex, string message, params object[] args) => logger.Write(LogLevel.Trace, ex, message, args);

        public static void Trace(this ILogger logger, Exception ex) => logger.Trace(ex, ex?.Message, null);

        public static void Trace(this ILogger logger, string message, params object[] args) => logger.Trace(null, message, args);

        public static void Debug(this ILogger logger, Exception ex, string message, params object[] args) => logger.Write(LogLevel.Debug, ex, message, args);

        public static void Debug(this ILogger logger, Exception ex) => logger.Debug(ex, ex?.Message, null);

        public static void Debug(this ILogger logger, string message, params object[] args) => logger.Debug(null, message, args);

        public static void Info(this ILogger logger, Exception ex, string message, params object[] args) => logger.Write(LogLevel.Info, ex, message, args);

        public static void Info(this ILogger logger, Exception ex) => logger.Info(ex, ex?.Message, null);

        public static void Info(this ILogger logger, string message, params object[] args) => logger.Info(null, message, args);

        public static void Warn(this ILogger logger, Exception ex, string message, params object[] args) => logger.Write(LogLevel.Warn, ex, message, args);

        public static void Warn(this ILogger logger, Exception ex) => logger.Warn(ex, ex?.Message, null);

        public static void Warn(this ILogger logger, string message, params object[] args) => logger.Warn(null, message, args);

        public static void Error(this ILogger logger, Exception ex, string message, params object[] args) => logger.Write(LogLevel.Error, ex, message, args);

        public static void Error(this ILogger logger, Exception ex) => logger.Error(ex, ex?.Message, null);

        public static void Error(this ILogger logger, string message, params object[] args) => logger.Error(null, message, args);

        public static void Fatal(this ILogger logger, Exception ex, string message, params object[] args) => logger.Write(LogLevel.Fatal, ex, message, args);

        public static void Fatal(this ILogger logger, Exception ex) => logger.Fatal(ex, ex?.Message, null);

        public static void Fatal(this ILogger logger, string message, params object[] args) => logger.Fatal(null, message, args);
        #endregion

        #region Ioc
        public static IServiceContext AddLogger(this IServiceContext services, ILogger logger)
        {
            services.RemoveAll<ILogger>();
            services.AddInstance(logger);
            return services;
        }

        public static IServiceContext AddNLogger(this IServiceContext services, string name = "")
        {
            var logger = new NLogger(name);
            services.AddLogger(logger);
            return services;
        }
        #endregion
    }
}
