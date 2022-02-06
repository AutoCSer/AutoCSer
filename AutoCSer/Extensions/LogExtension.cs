using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static class LogExtension
    {
        /// <summary>
        /// 判断是否支持任意级别
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="logLevel">日志级别</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static bool IsAnyLevel(this ILog log, AutoCSer.LogLevel logLevel)
        {
            return (log.LogLevel & logLevel) != 0;
        }
        /// <summary>
        /// 判断是否支持所有级别
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="logLevel">日志级别</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static bool IsAllLevel(this ILog log, AutoCSer.LogLevel logLevel)
        {
            return (log.LogLevel & logLevel) == logLevel;
        }
    }
}
