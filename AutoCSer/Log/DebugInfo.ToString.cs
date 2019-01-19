using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace AutoCSer.Log
{
    /// <summary>
    /// 日志信息
    /// </summary>
    internal sealed partial class DebugInfo
    {
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            if (toString == null)
            {
                DebugString debugString = new DebugString { DebugStream = toStringStream, Type = Type.ToString() };
                if (StackFrame != null) debugString.Set(StackFrame);
                if (StackTrace != null) debugString.StackTrace = StackTrace.ToString();
                if (Exception != null) debugString.Exception = Exception.ToString();
                Monitor.Enter(toStringStreamLock);
                try
                {
                    debugString.Write(Message);
                    debugString.Write();
                    toString = toStringStream.ToString();
                }
                finally { Monitor.Exit(toStringStreamLock); }
            }
            return toString;
        }
    }
}
