using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 日志处理
    /// </summary>
    public partial interface ILog
    {
        /// <summary>
        /// 添加警告日志
        /// </summary>
        /// <param name="message">警告日志内容</param>
        /// <param name="level">日志级别</param>
        void Warn(string message, LogLevel level = LogLevel.Warn);
        /// <summary>
        /// 添加普通日志
        /// </summary>
        /// <param name="message">普通日志内容</param>
        /// <param name="level">日志级别</param>
        void Info(string message, LogLevel level = LogLevel.Info);
        /// <summary>
        /// 添加调试日志
        /// </summary>
        /// <param name="message">调试日志内容</param>
        /// <param name="level">日志级别</param>
        void Debug(string message, LogLevel level = LogLevel.Debug);
        /// <summary>
        /// 添加一般错误日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        void Error(string message, LogLevel level = LogLevel.Error);
        /// <summary>
        /// 添加致命错误日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        void Fatal(string message, LogLevel level = LogLevel.Fatal);
    }
}
