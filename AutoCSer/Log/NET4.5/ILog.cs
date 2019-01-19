using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoCSer.Log
{
    /// <summary>
    /// 日志处理
    /// </summary>
    public partial interface ILog
    {
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="cache">缓存类型</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        void Add(LogType type, string message, CacheType cache, [CallerMemberName] string callerMemberName = null, [CallerFilePath]string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0);
        /// <summary>
        /// 同步添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="cache">缓存类型</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        void Wait(LogType type, string message, CacheType cache, [CallerMemberName] string callerMemberName = null, [CallerFilePath]string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0);
    }
}
