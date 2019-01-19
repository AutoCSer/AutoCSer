using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.WebView.Poll
{
    /// <summary>
    /// 会话超时
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct SessionTimeout
    {
        /// <summary>
        /// 超时
        /// </summary>
        internal DateTime Timeout;
        /// <summary>
        /// 会话标识
        /// </summary>
        internal AutoCSer.Net.HttpDomainServer.SessionId SessionId;
        /// <summary>
        /// 创建会话标识
        /// </summary>
        /// <param name="timeoutTicks">超时时钟周期</param>
        /// <returns>会话标识</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.HttpDomainServer.SessionId New(long timeoutTicks)
        {
            SessionId.New();
            Timeout = Date.NowTime.Now.AddTicks(timeoutTicks);
            return SessionId;
        }
        /// <summary>
        /// 获取会话标识
        /// </summary>
        /// <param name="timeoutTicks">超时时钟周期</param>
        /// <returns>会话标识</returns>
        internal AutoCSer.Net.HttpDomainServer.SessionId Get(long timeoutTicks)
        {
            if (Timeout <= Date.NowTime.Now)
            {
                SessionId.New();
                Timeout = Date.NowTime.Now.AddTicks(timeoutTicks);
            }
            return SessionId;
        }
        /// <summary>
        /// 会话标识验证
        /// </summary>
        /// <param name="verify"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Check(string verify)
        {
            return Timeout > Date.NowTime.Now && SessionId.CheckHex(verify);
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Clear()
        {
            Timeout = DateTime.MinValue;
            SessionId.Null();
        }
    }
}
