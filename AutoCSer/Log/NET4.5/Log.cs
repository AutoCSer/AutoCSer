using AutoCSer.Log;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static partial class Log
    {
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="isCache">是否缓存</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        public static void Add(this ILog log, LogType type, string message, bool isCache, [CallerMemberName] string callerMemberName = null, [CallerFilePath]string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            log.Add(type, message, isCache ? AutoCSer.Log.CacheType.Queue : AutoCSer.Log.CacheType.None, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 同步添加日志
        /// </summary>
        /// <param name="log">日志处理</param>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="isCache">是否缓存</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        public static void Wait(this ILog log, LogType type, string message, bool isCache, [CallerMemberName] string callerMemberName = null, [CallerFilePath]string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            log.Wait(type, message, isCache ? AutoCSer.Log.CacheType.Queue : AutoCSer.Log.CacheType.None, callerMemberName, callerFilePath, callerLineNumber);
        }
    }
}
