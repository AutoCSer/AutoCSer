using System;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.HttpDomainServer
{
    /// <summary>
    /// 会话标识接口
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// 删除Session
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        void Remove(ref SessionId sessionId);
        /// <summary>
        /// 设置Session值
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        /// <param name="value">值</param>
        /// <returns>Session是否被更新</returns>
        bool Set(ref SessionId sessionId, object value);
        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        /// <param name="nullValue">失败返回值</param>
        /// <returns>Session值</returns>
        object Get(ref SessionId sessionId, object nullValue);
    }
#if !NOJIT
    /// <summary>
    /// 会话标识接口
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public interface ISession<valueType> : ISession
    {
        /// <summary>
        /// 设置Session值
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        /// <param name="value">值</param>
        /// <returns>Session是否被更新</returns>
        bool Set(ref SessionId sessionId, valueType value);
        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        /// <param name="nullValue">失败返回值</param>
        /// <returns>Session值</returns>
        valueType Get(ref SessionId sessionId, valueType nullValue);
    }
#endif
    /// <summary>
    /// 会话标识
    /// </summary>
    public abstract unsafe class Session : TcpInternalServer.TimeVerifyServer, IDisposable
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        internal const string ServerName = "HttpSession";
        /// <summary>
        /// 超时时钟周期
        /// </summary>
        protected long timeoutTicks;
        /// <summary>
        /// 超时刷新秒数
        /// </summary>
        private int refreshSeconds;
        /// <summary>
        /// 是否释放资源
        /// </summary>
        private int isDisposed;
        /// <summary>
        /// 会话标识
        /// </summary>
        /// <param name="timeoutMinutes">超时分钟数</param>
        /// <param name="refreshMinutes">超时刷新分钟数</param>
        protected Session(int timeoutMinutes, int refreshMinutes)
        {
            timeoutTicks = new TimeSpan(0, Math.Max(timeoutMinutes, 1), 0).Ticks;
            Math.Max(refreshMinutes * 60, 60);
            refreshSeconds = Math.Max(refreshMinutes, 1) * 60;
            //refreshTicks = new TimeSpan(0, refreshMinutes, 0).Ticks;
            //timerTask.Default.Add(this, thread.callType.HttpSessionRefreshTimeout, date.nowTime.Now.AddTicks(refreshTicks));
            isTimer = refreshSeconds;
            PushNotNull(this);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0) PopNotNull(this);
        }
        /// <summary>
        /// 是否已经触发定时任务
        /// </summary> 
        private int isTimer;
        /// <summary>
        /// 定时器触发日志写入
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnTimer()
        {
            if (Interlocked.Decrement(ref isTimer) == 0)
            {
                RefreshTimeout();
                isTimer = refreshSeconds;
            }
        }
        /// <summary>
        /// 超时检测
        /// </summary>
        internal virtual void RefreshTimeout() { }

        /// <summary>
        /// 下一个节点
        /// </summary>
        internal Session DoubleLinkNext;
        /// <summary>
        /// 上一个节点
        /// </summary>
        internal Session DoubleLinkPrevious;
        /// <summary>
        /// 链表尾部
        /// </summary>
        internal static Session SessionEnd;
        /// <summary>
        /// 链表访问锁
        /// </summary>
        private static int sessionLinkLock;
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void PushNotNull(Session value)
        {
            while (System.Threading.Interlocked.CompareExchange(ref sessionLinkLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkDoublePush);
            if (SessionEnd == null)
            {
                SessionEnd = value;
                System.Threading.Interlocked.Exchange(ref sessionLinkLock, 0);
                WebView.OnTime.Set(Date.NowTime.OnTimeFlag.HttpSession);
            }
            else
            {
                SessionEnd.DoubleLinkNext = value;
                value.DoubleLinkPrevious = SessionEnd;
                SessionEnd = value;
                System.Threading.Interlocked.Exchange(ref sessionLinkLock, 0);
            }
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void PopNotNull(Session value)
        {
            while (System.Threading.Interlocked.CompareExchange(ref sessionLinkLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.YieldLinkDoublePop);
            if (value == SessionEnd)
            {
                if ((SessionEnd = value.DoubleLinkPrevious) != null) SessionEnd.DoubleLinkNext = value.DoubleLinkPrevious = null;
                System.Threading.Interlocked.Exchange(ref sessionLinkLock, 0);
            }
            else
            {
                value.freeNotEnd();
                System.Threading.Interlocked.Exchange(ref sessionLinkLock, 0);
            }
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void freeNotEnd()
        {
            DoubleLinkNext.DoubleLinkPrevious = DoubleLinkPrevious;
            if (DoubleLinkPrevious != null)
            {
                DoubleLinkPrevious.DoubleLinkNext = DoubleLinkNext;
                DoubleLinkPrevious = null;
            }
            DoubleLinkNext = null;
        }
    }
    /// <summary>
    /// 会话标识
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    [AutoCSer.Net.TcpInternalServer.Server(Name = Session.ServerName, Host = "127.0.0.1", Port = (int)ServerPort.HttpSession, GenericType = typeof(Session<int>))]
    public partial class Session<valueType> : Session
#if NOJIT
        , ISession
#else
, ISession<valueType>
#endif
    {
        /// <summary>
        /// Session值
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct Value
        {
            /// <summary>
            /// 超时时间
            /// </summary>
            public DateTime Timeout;
            /// <summary>
            /// 随机数低64位
            /// </summary>
            public ulong Low;
            /// <summary>
            /// 随机数高64位
            /// </summary>
            public ulong High;
            /// <summary>
            /// Session值集合
            /// </summary>
            public valueType SessionValue;
            /// <summary>
            /// 索引标识
            /// </summary>
            public uint Identity;
            /// <summary>
            /// 检测会话是否有效
            /// </summary>
            /// <param name="sessionId"></param>
            /// <returns></returns>
            public bool Check(ref SessionId sessionId)
            {
                return Identity == sessionId.IndexIdentity && ((Low ^ sessionId.Low) | (High ^ sessionId.High)) == 0 && Timeout != DateTime.MinValue;
            }
            /// <summary>
            /// 设置会话信息
            /// </summary>
            /// <param name="sessionId"></param>
            /// <param name="timeout"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool Set(ref SessionId sessionId, DateTime timeout, valueType value)
            {
                if (((Low ^ sessionId.Low) | (High ^ sessionId.High) | (Identity ^ sessionId.IndexIdentity)) == 0 && Timeout != DateTime.MinValue)
                {
                    SessionValue = value;
                    Timeout = timeout;
                    return true;
                }
                return false;
            }
            /// <summary>
            /// 新建会话信息
            /// </summary>
            /// <param name="sessionId"></param>
            /// <param name="timeout"></param>
            /// <param name="value"></param>
            public void New(ref SessionId sessionId, DateTime timeout, valueType value)
            {
                Timeout = timeout;
                Low = sessionId.Low;
                High = sessionId.High;
                SessionValue = value;
                sessionId.IndexIdentity = Identity;
            }
            /// <summary>
            /// 获取会话信息
            /// </summary>
            /// <param name="sessionId"></param>
            /// <param name="timeout"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool Get(ref SessionId sessionId, DateTime timeout, ref valueType value)
            {
                if (((Low ^ sessionId.Low) | (High ^ sessionId.High) | (Identity ^ sessionId.IndexIdentity)) == 0 && Timeout != DateTime.MinValue)
                {
                    value = SessionValue;
                    Timeout = timeout;
                    return true;
                }
                return false;
            }
            /// <summary>
            /// 删除会话信息
            /// </summary>
            /// <param name="sessionId"></param>
            /// <returns></returns>
            public bool Remove(ref SessionId sessionId)
            {
                if (((Low ^ sessionId.Low) | (High ^ sessionId.High) | (Identity ^ sessionId.IndexIdentity)) == 0 && Timeout != DateTime.MinValue)
                {
                    SessionValue = default(valueType);
                    ++Identity;
                    Timeout = DateTime.MinValue;
                    return true;
                }
                return false;
            }
            /// <summary>
            /// 超时检测
            /// </summary>
            /// <param name="now"></param>
            /// <returns></returns>
            public bool CheckTimeout(DateTime now)
            {
                if (Timeout < now && Timeout != DateTime.MinValue)
                {
                    SessionValue = default(valueType);
                    ++Identity;
                    Timeout = DateTime.MinValue;
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Session值索引池
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct Pool
        {
            /// <summary>
            /// Session值索引池
            /// </summary>
            public AutoCSer.Threading.IndexValuePool<Value> IndexPool;
            /// <summary>
            /// 超时时钟周期
            /// </summary>
            private long timeoutTicks;
            /// <summary>
            /// 设置Session值索引池
            /// </summary>
            /// <param name="timeoutTicks"></param>
            public void Set(long timeoutTicks)
            {
                this.timeoutTicks = timeoutTicks;
                IndexPool.Reset(63);
            }
            /// <summary>
            /// 超时检测
            /// </summary>
            public void Refresh()
            {
                int index = IndexPool.PoolIndex;
                while (index != 0)
                {
                    object arrayLock = IndexPool.ArrayLock;
                    Monitor.Enter(arrayLock);
                    DateTime now = Date.NowTime.Now;
                    if (index > IndexPool.PoolIndex) index = IndexPool.PoolIndex;
                    try
                    {
                        for (int endIndex = Math.Max(index - 1024, 0); index != endIndex; )
                        {
                            if (IndexPool.Array[--index].CheckTimeout(now)) IndexPool.FreeContinue(index);
                        }
                    }
                    finally { Monitor.Exit(arrayLock); }
                }
            }
            /// <summary>
            /// 设置Session值
            /// </summary>
            /// <param name="sessionId">Session名称</param>
            /// <param name="value">值</param>
            /// <returns>是否设置成功</returns>
            public unsafe int Set(ref SessionId sessionId, valueType value)
            {
                if (sessionId.Low != 0 && (uint)sessionId.Index < (uint)IndexPool.Array.Length && IndexPool.Array[sessionId.Index].Check(ref sessionId))
                {
                    object arrayLock = IndexPool.ArrayLock;
                    Monitor.Enter(arrayLock);
                    if (IndexPool.Array[sessionId.Index].Set(ref sessionId, Date.NowTime.Now.AddTicks(timeoutTicks), value))
                    {
                        Monitor.Exit(arrayLock);
                        return 1;
                    }
                    Monitor.Exit(arrayLock);
                }
                return 0;
            }
            /// <summary>
            /// 设置Session值
            /// </summary>
            /// <param name="sessionId">Session名称</param>
            /// <param name="value">值</param>
            /// <returns>Session是否被更新</returns>
            public unsafe bool New(ref SessionId sessionId, valueType value)
            {
                object arrayLock = IndexPool.ArrayLock;
                Monitor.Enter(arrayLock);
                try
                {
                    sessionId.Index = IndexPool.GetIndexContinue();
                    IndexPool.Array[sessionId.Index].New(ref sessionId, Date.NowTime.Now.AddTicks(timeoutTicks), value);
                }
                finally { Monitor.Exit(arrayLock); }
                return true;
            }
            /// <summary>
            /// 获取Session值
            /// </summary>
            /// <param name="sessionId">Session名称</param>
            /// <param name="value">返回值</param>
            /// <returns>是否存在返回值</returns>
            public bool TryGet(ref SessionId sessionId, out valueType value)
            {
                value = default(valueType);
                if (sessionId.Low != 0 && (uint)sessionId.Index < (uint)IndexPool.Array.Length)
                {
                    object arrayLock = IndexPool.ArrayLock;
                    Monitor.Enter(arrayLock);
                    bool isValue = IndexPool.Array[sessionId.Index].Get(ref sessionId, Date.NowTime.Now.AddTicks(timeoutTicks), ref value);
                    Monitor.Exit(arrayLock);
                    return isValue;
                }
                return false;
            }
            /// <summary>
            /// 删除Session
            /// </summary>
            /// <param name="sessionId">Session名称</param>
            public void Remove(ref SessionId sessionId)
            {
                if (sessionId.Low != 0 && (uint)sessionId.Index < (uint)IndexPool.Array.Length)
                {
                    object arrayLock = IndexPool.ArrayLock;
                    Monitor.Enter(arrayLock);
                    if (IndexPool.Array[sessionId.Index].Remove(ref sessionId)) IndexPool.FreeExit(sessionId.Index);
                    else Monitor.Exit(arrayLock);
                }
            }
        }
        /// <summary>
        /// Session 值索引池
        /// </summary>
        private Pool[] valuePool;
        /// <summary>
        /// 会话标识
        /// </summary>
        public Session()
            : base(Http.SocketBase.Config.SessionMinutes, Http.SocketBase.Config.SessionRefreshMinutes)
        {
            valuePool = new Pool[256];
            for (int index = valuePool.Length; index != 0; valuePool[--index].Set(timeoutTicks)) ;
        }
        /// <summary>
        /// 超时检测
        /// </summary>
        internal unsafe override void RefreshTimeout()
        {
            try
            {
                for (int index = valuePool.Length; index != 0; valuePool[--index].Refresh()) ;
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
            }
        }
        /// <summary>
        /// 设置Session值
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        /// <param name="value">值</param>
        /// <returns>Session是否被更新</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Set(ref SessionId sessionId, object value)
        {
            return Set(ref sessionId, (valueType)value);
        }
        /// <summary>
        /// 设置Session值
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        /// <param name="value">值</param>
        /// <returns>Session是否被更新</returns>
        public unsafe bool Set(ref SessionId sessionId, valueType value)
        {
            if (sessionId.Ticks != (ulong)Pub.StartTime.Ticks || valuePool[(byte)sessionId.Low].Set(ref sessionId, value) == 0)
            {
                sessionId.NewNoIndex();
                return valuePool[(byte)sessionId.Low].New(ref sessionId, value);
            }
            return false;
        }
        /// <summary>
        /// 设置Session值
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        /// <param name="value">值</param>
        /// <returns>Session名称</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        public unsafe SessionId Set(SessionId sessionId, valueType value)
        {
            Set(ref sessionId, value);
            return sessionId;
        }
        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        /// <param name="nullValue">失败返回值</param>
        /// <returns>Session值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public object Get(ref SessionId sessionId, object nullValue)
        {
            valueType value;
            return TryGet(ref sessionId, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        /// <param name="nullValue">失败返回值</param>
        /// <returns>Session值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType Get(ref SessionId sessionId, valueType nullValue)
        {
            valueType value;
            return TryGet(ref sessionId, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        /// <param name="value">返回值</param>
        /// <returns>是否存在返回值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        public bool TryGet(SessionId sessionId, out valueType value)
        {
            return TryGet(ref sessionId, out value);
        }
        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        /// <returns>返回值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        public valueType Get(SessionId sessionId)
        {
            valueType value;
            TryGet(ref sessionId, out value);
            return value;
        }
        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>返回值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        public valueType Get(SessionId sessionId, valueType nullValue)
        {
            valueType value;
            return TryGet(ref sessionId, out value) ? value : nullValue;
        }
        /// <summary>
        /// 获取Session值
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        /// <param name="value">返回值</param>
        /// <returns>是否存在返回值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool TryGet(ref SessionId sessionId, out valueType value)
        {
            if (sessionId.Ticks == (ulong)Pub.StartTime.Ticks) return valuePool[(byte)sessionId.Low].TryGet(ref sessionId, out value);
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 删除Session
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        public void Remove(SessionId sessionId)
        {
            Remove(ref sessionId);
        }
        /// <summary>
        /// 删除Session
        /// </summary>
        /// <param name="sessionId">Session名称</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Remove(ref SessionId sessionId)
        {
            if (sessionId.Ticks == (ulong)Pub.StartTime.Ticks) valuePool[(byte)sessionId.Low].Remove(ref sessionId);
        }
    }
}
