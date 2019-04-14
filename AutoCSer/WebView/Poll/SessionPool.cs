using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.WebView.Poll
{
    /// <summary>
    /// 长连接轮询会话超时集合
    /// </summary>
    public sealed class SessionPool
    {
        /// <summary>
        /// 长连接轮询验证集合
        /// </summary>
        private Session[] sessions;
        /// <summary>
        /// 超时秒数
        /// </summary>
        /// <param name="timeoutSeconds"></param>
        public SessionPool(int timeoutSeconds = 3600)
        {
            long timeoutTicks = TimeSpan.TicksPerSecond * Math.Max(timeoutSeconds, 60);
            sessions = new Session[256];
            for (int index = sessions.Length; index != 0; sessions[--index] = new Session(timeoutTicks)) ;
        }
        /// <summary>
        /// 获取用户长连接轮询验证
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <returns>长连接轮询验证</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private string getString(int userId)
        {
            AutoCSer.Net.HttpDomainServer.SessionId sessionId;
            get(userId, out sessionId);
            return sessionId.Low == 0 ? null : sessionId.ToHex();
        }
        /// <summary>
        /// 获取用户长连接轮询验证
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <param name="sessionId">长连接轮询验证,0表示失败</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void get(int userId, out AutoCSer.Net.HttpDomainServer.SessionId sessionId)
        {
            sessions[userId & 0xff].Get(userId, out sessionId);
        }
        /// <summary>
        /// 获取用户长连接轮询验证
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <returns>长连接轮询验证</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public VerifyInfo Get(int userId)
        {
            return new VerifyInfo { UserId = userId, Verify = userId < 0 ? null : (getString(userId) ?? add(userId)) };
        }
        /// <summary>
        /// 添加用户长连接轮询验证
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private string add(int userId)
        {
            return sessions[userId & 0xff].Add(userId);
        }
        /// <summary>
        /// 删除用户长连接轮询验证
        /// </summary>
        /// <param name="userId">用户标识</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Remove(int userId)
        {
            sessions[userId & 0xff].Remove(userId);
        }
        /// <summary>
        /// 轮询验证检测
        /// </summary>
        /// <param name="verify">轮询验证输入参数信息</param>
        /// <returns>是否通过验证</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Verify(ref VerifyInfo verify)
        {
            return sessions[verify.UserId & 0xff].Verify(ref verify);
        }
    }
}
