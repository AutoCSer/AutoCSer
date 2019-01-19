using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace AutoCSer.Log
{
    /// <summary>
    /// 日志信息
    /// </summary>
    internal sealed partial class DebugInfo : AutoCSer.Threading.Link<DebugInfo>
    {
        /// <summary>
        /// 字符串转换流
        /// </summary>
        private static readonly CharStream toStringStream = new CharStream();
        /// <summary>
        /// 字符串转换流访问锁
        /// </summary>
        private static readonly object toStringStreamLock = new object();
        /// <summary>
        /// 调用堆栈
        /// </summary>
        public StackTrace StackTrace;
        /// <summary>
        /// 调用堆栈帧
        /// </summary>
        public StackFrame StackFrame;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message;
        /// <summary>
        /// 错误异常
        /// </summary>
        public Exception Exception;
        /// <summary>
        /// 日志同步标志
        /// </summary>
        public int WaitId;
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType Type;
        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType CacheType;
        /// <summary>
        /// 字符串
        /// </summary>
        public string toString;
    }
}
