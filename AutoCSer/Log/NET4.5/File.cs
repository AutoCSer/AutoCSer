using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Log
{
    /// <summary>
    /// 文件日志处理(默认日志)
    /// </summary>
    public sealed partial class File
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
        /// <param name="isWait">是否同步</param>
        /// <returns>日志信息</returns>
        private DebugInfo add(LogType type, string message, CacheType cache, string callerMemberName, string callerFilePath,  int callerLineNumber, bool isWait)
        {
            if ((Type & type) != 0)
            {
                DebugInfo value = new DebugInfo
                {
                    Type = type,
                    Message = message,
                    CallerMemberName = callerMemberName,
                    CallerFilePath = callerFilePath,
                    CallerLineNumber = callerLineNumber,
                    CacheType = cache
                };
                add(value, isWait);
                return value;
            }
            return null;
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="cache">缓存类型</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        public void Add(LogType type, string message, CacheType cache, [CallerMemberName] string callerMemberName = null, [CallerFilePath]string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            add(type, message, cache, callerMemberName, callerFilePath, callerLineNumber, false);
        }
        /// <summary>
        /// 同步添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="message">提示信息</param>
        /// <param name="cache">缓存类型</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        public void Wait(LogType type, string message, CacheType cache, [CallerMemberName] string callerMemberName = null, [CallerFilePath]string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            DebugInfo value = add(type, message, cache, callerMemberName, callerFilePath, callerLineNumber, true);
            if (value != null) wait(value);
        }
    }
}
