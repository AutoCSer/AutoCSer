using System;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.WebView.Poll
{
    /// <summary>
    /// 会话超时集合
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct Session
    {
        /// <summary>
        /// 长连接轮询验证集合
        /// </summary>
        private SessionTimeout[] sessions;
        /// <summary>
        /// 长连接轮询验证访问锁
        /// </summary>
        private readonly object sessionLock;
        /// <summary>
        /// 超时时钟周期
        /// </summary>
        private long timeoutTicks;
        /// <summary>
        /// 会话超时集合
        /// </summary>
        /// <param name="timeoutTicks">超时时钟周期</param>
        internal Session(long timeoutTicks)
        {
            sessions = NullValue<SessionTimeout>.Array;
            sessionLock = new object();
            this.timeoutTicks = timeoutTicks;
        }
        /// <summary>
        /// 获取用户长连接轮询验证
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <param name="sessionId">长连接轮询验证,0表示失败</param>
        internal void Get(int userId, out AutoCSer.Net.HttpDomainServer.SessionId sessionId)
        {
            int index = userId >> 8;
            if ((uint)index < (uint)sessions.Length)
            {
                Monitor.Enter(sessionLock);
                sessionId = sessions[index].Get(timeoutTicks);
                Monitor.Exit(sessionLock);
            }
            else sessionId = default(AutoCSer.Net.HttpDomainServer.SessionId);
        }
        /// <summary>
        /// 添加用户长连接轮询验证
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal string Add(int userId)
        {
            AutoCSer.Net.HttpDomainServer.SessionId sessionId;
            int index = userId >> 8;
            Monitor.Enter(sessionLock);
            if ((uint)index < (uint)sessions.Length)
            {
                sessionId = sessions[index].Get(timeoutTicks);
                Monitor.Exit(sessionLock);
            }
            else
            {
                try
                {
                    sessions = sessions.copyNew(Math.Max(sessions.Length << 1, index + 256));
                    sessionId = sessions[index].New(timeoutTicks);
                }
                finally { Monitor.Exit(sessionLock); }
            }
            return sessionId.ToHex();
        }
        /// <summary>
        /// 删除用户长连接轮询验证
        /// </summary>
        /// <param name="userId"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Remove(int userId)
        {
            int index = userId >> 8;
            if ((uint)index < (uint)sessions.Length)
            {
                Monitor.Enter(sessionLock);
                sessions[index].Clear();
                Monitor.Exit(sessionLock);
            }
        }
        /// <summary>
        /// 轮询验证检测
        /// </summary>
        /// <param name="verify">轮询验证输入参数信息</param>
        /// <returns></returns>
        internal bool Verify(ref VerifyInfo verify)
        {
            if (verify.Verify != null)
            {
                int index = verify.UserId >> 8;
                return (uint)index < (uint)sessions.Length && sessions[index].Check(verify.Verify);
            }
            return false;
        }
    }
}
