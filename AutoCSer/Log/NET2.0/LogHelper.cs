using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 公共日志配置
    /// </summary>
    public static partial class LogHelper
    {
        /// <summary>
        /// 添加警告日志
        /// </summary>
        /// <param name="message">警告日志内容</param>
        /// <param name="level">日志级别</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Warn(string message, LogLevel level = LogLevel.Warn)
        {
            Default.Warn(message, level);
        }
        /// <summary>
        /// 添加普通日志
        /// </summary>
        /// <param name="message">普通日志内容</param>
        /// <param name="level">日志级别</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Info(string message, LogLevel level = LogLevel.Info)
        {
            Default.Info(message, level);
        }
        /// <summary>
        /// 添加调试日志
        /// </summary>
        /// <param name="message">调试日志内容</param>
        /// <param name="level">日志级别</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Debug(string message, LogLevel level = LogLevel.Debug)
        {
            Default.Debug(message, level);
        }
        /// <summary>
        /// 添加一般错误日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Error(string message, LogLevel level = LogLevel.Error)
        {
            Default.Error(message, level);
        }
        /// <summary>
        /// 添加致命错误日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Fatal(string message, LogLevel level = LogLevel.Fatal)
        {
            Default.Fatal(message, level);
        }
    }
}
