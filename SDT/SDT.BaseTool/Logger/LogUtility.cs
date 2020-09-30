using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
{
    public class LogUtility
    {
        public static void Write(LogLevel level, Exception ex, string message, params object[] args) => LoggerFactory.Logger?.Write(level, ex, message, args);

        public static void Trace(string message, params object[] args) => LoggerFactory.Logger?.Trace(message, args);

        public static void Trace(Exception ex) => LoggerFactory.Logger?.Trace(ex);

        public static void Trace(Exception ex, string message, params object[] args) => LoggerFactory.Logger?.Trace(ex, message, args);

        public static void Debug(string message, params object[] args) => LoggerFactory.Logger?.Debug(message, args);

        public static void Debug(Exception ex) => LoggerFactory.Logger?.Debug(ex);

        public static void Debug(Exception ex, string message, params object[] args) => LoggerFactory.Logger?.Debug(ex, message, args);

        public static void Info(string message, params object[] args) => LoggerFactory.Logger?.Info(message, args);

        public static void Info(Exception ex) => LoggerFactory.Logger?.Info(ex);

        public static void Info(Exception ex, string message, params object[] args) => LoggerFactory.Logger?.Info(ex, message, args);

        public static void Warn(string message, params object[] args) => LoggerFactory.Logger?.Warn(message, args);

        public static void Warn(Exception ex) => LoggerFactory.Logger?.Warn(ex);

        public static void Warn(Exception ex, string message, params object[] args) => LoggerFactory.Logger?.Warn(ex, message, args);

        public static void Error(string message, params object[] args) => LoggerFactory.Logger?.Error(message, args);

        public static void Error(Exception ex) => LoggerFactory.Logger?.Error(ex);

        public static void Error(Exception ex, string message, params object[] args) => LoggerFactory.Logger?.Error(ex, message, args);

        public static void Fatal(string message, params object[] args) => LoggerFactory.Logger?.Fatal(message, args);

        public static void Fatal(Exception ex) => LoggerFactory.Logger?.Fatal(ex);

        public static void Fatal(Exception ex, string message, params object[] args) => LoggerFactory.Logger?.Fatal(ex, message, args);
    }
}
