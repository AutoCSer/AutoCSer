using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 锁节点
    /// </summary>
    public sealed partial class Lock : Abstract.Node, Abstract.IValueNode
    {
        /// <summary>
        /// 锁节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private Lock(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 创建数据节点
        /// </summary>
        /// <returns></returns>
        internal override Abstract.Node CreateValueNode()
        {
            return this;
        }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.Lock);
            stream.Write((byte)ValueData.DataType.Null);
            serializeParentDataStructure(stream);
        }

        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutTicks"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Parameter.Value GetTryEnterNode(long timeoutTicks)
        {
            return new Parameter.Value(this, OperationParameter.OperationType.InsertBefore, timeoutTicks);
        }
        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Parameter.Value GetTryEnterNode(uint timeoutMilliseconds)
        {
            return GetTryEnterNode(FotmatTimeoutTicks(timeoutMilliseconds));
        }
        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <returns></returns>
        public ReturnValue<ulong> TryEnter(uint timeoutMilliseconds)
        {
            return Client.GetULong(ClientDataStructure.Client.MasterQueryAsynchronous(GetTryEnterNode(timeoutMilliseconds)));
        }
        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <param name="onTryEnter"></param>
        /// <returns></returns>
        public void TryEnter(uint timeoutMilliseconds, Action<ReturnValue<ulong>> onTryEnter)
        {
            if (onTryEnter == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronous(GetTryEnterNode(timeoutMilliseconds), onTryEnter);
        }
        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <param name="onTryEnter"></param>
        /// <returns></returns>
        public void TryEnter(uint timeoutMilliseconds, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onTryEnter)
        {
            if (onTryEnter == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronous(GetTryEnterNode(timeoutMilliseconds), onTryEnter);
        }
        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <param name="onTryEnter">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void TryEnterStream(uint timeoutMilliseconds, Action<ReturnValue<ulong>> onTryEnter)
        {
            if (onTryEnter == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronousStream(GetTryEnterNode(timeoutMilliseconds), onTryEnter);
        }
        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <param name="onTryEnter">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void TryEnterStream(uint timeoutMilliseconds, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onTryEnter)
        {
            if (onTryEnter == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronousStream(GetTryEnterNode(timeoutMilliseconds), onTryEnter);
        }

        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutTicks"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Parameter.Value GetEnterNode(long timeoutTicks)
        {
            return new Parameter.Value(this, OperationParameter.OperationType.SetValue, timeoutTicks);
        }
        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Parameter.Value GetEnterNode(uint timeoutMilliseconds)
        {
            return GetEnterNode(FotmatTimeoutTicks(timeoutMilliseconds));
        }
        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <returns></returns>
        public ReturnValue<ulong> Enter(uint timeoutMilliseconds)
        {
            return Client.GetULong(ClientDataStructure.Client.MasterQueryAsynchronous(GetEnterNode(timeoutMilliseconds)));
        }
        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <param name="onEnter"></param>
        /// <returns></returns>
        public void Enter(uint timeoutMilliseconds, Action<ReturnValue<ulong>> onEnter)
        {
            if (onEnter == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronous(GetEnterNode(timeoutMilliseconds), onEnter);
        }
        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <param name="onEnter"></param>
        /// <returns></returns>
        public void Enter(uint timeoutMilliseconds, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onEnter)
        {
            if (onEnter == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronous(GetEnterNode(timeoutMilliseconds), onEnter);
        }
        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <param name="onEnter">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void EnterStream(uint timeoutMilliseconds, Action<ReturnValue<ulong>> onEnter)
        {
            if (onEnter == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronousStream(GetEnterNode(timeoutMilliseconds), onEnter);
        }
        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <param name="onEnter">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void EnterStream(uint timeoutMilliseconds, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onEnter)
        {
            if (onEnter == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronousStream(GetEnterNode(timeoutMilliseconds), onEnter);
        }

        /// <summary>
        /// 释放锁序号
        /// </summary>
        /// <param name="randomNo">锁序号</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Parameter.Value GetExitNode(ulong randomNo)
        {
            return new Parameter.Value(this, OperationParameter.OperationType.Remove, randomNo);
        }
        /// <summary>
        /// 释放锁序号
        /// </summary>
        /// <param name="randomNo">锁序号</param>
        /// <returns></returns>
        public ReturnValue<bool> Exit(ulong randomNo)
        {
            return Client.GetBool(ClientDataStructure.Client.MasterQueryAsynchronous(GetExitNode(randomNo)));
        }
        /// <summary>
        /// 释放锁序号
        /// </summary>
        /// <param name="randomNo">锁序号</param>
        /// <param name="onExit"></param>
        /// <returns></returns>
        public void Exit(ulong randomNo, Action<ReturnValue<bool>> onExit)
        {
            if (onExit == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronous(GetExitNode(randomNo), onExit);
        }
        /// <summary>
        /// 释放锁序号
        /// </summary>
        /// <param name="randomNo">锁序号</param>
        /// <param name="onExit"></param>
        /// <returns></returns>
        public void Exit(ulong randomNo, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onExit)
        {
            if (onExit == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronous(GetExitNode(randomNo), onExit);
        }
        /// <summary>
        /// 释放锁序号
        /// </summary>
        /// <param name="randomNo">锁序号</param>
        /// <param name="onExit">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void ExitStream(ulong randomNo, Action<ReturnValue<bool>> onExit)
        {
            if (onExit == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronousStream(GetExitNode(randomNo), onExit);
        }
        /// <summary>
        /// 释放锁序号
        /// </summary>
        /// <param name="randomNo">锁序号</param>
        /// <param name="onExit">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void ExitStream(ulong randomNo, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onExit)
        {
            if (onExit == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronousStream(GetExitNode(randomNo), onExit);
        }

        /// <summary>
        /// 创建锁管理对象
        /// </summary>
        /// <param name="randomNo"></param>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <returns></returns>
        private ReturnValue<AutoCSer.CacheServer.Lock.Manager> getEnter(uint timeoutMilliseconds, ReturnValue<ulong> randomNo)
        {
            if (randomNo.Type == ReturnType.Success)
            {
                try
                {
                    return new AutoCSer.CacheServer.Lock.Manager(this, timeoutMilliseconds, ref randomNo);
                }
                finally
                {
                    if (randomNo.Type == ReturnType.Success) ClientDataStructure.Client.MasterClient.QueryAsynchronous(new OperationParameter.QueryNode { Node = GetExitNode(randomNo) }, null);
                }
            }
            return new ReturnValue<CacheServer.Lock.Manager> { Type = randomNo.Type, TcpReturnType = randomNo.TcpReturnType };
        }
        /// <summary>
        /// 创建锁管理对象
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<AutoCSer.CacheServer.Lock.Manager> GetTryEnter(uint timeoutMilliseconds)
        {
            return getEnter(timeoutMilliseconds, TryEnter(timeoutMilliseconds));
        }
        /// <summary>
        /// 创建锁管理对象
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <param name="onEnter"></param>
        public void GetTryEnter(uint timeoutMilliseconds, Action<ReturnValue<AutoCSer.CacheServer.Lock.AsynchronousManager>> onEnter)
        {
            if (onEnter == null) throw new ArgumentNullException();
            AutoCSer.CacheServer.Lock.AsynchronousManager manager = null;
            try
            {
                manager = new CacheServer.Lock.AsynchronousManager(this, timeoutMilliseconds, onEnter);
                onEnter = null;
                manager.TryEnter();
                manager = null;
            }
            finally
            {
                if (onEnter != null) onEnter(default(ReturnValue<AutoCSer.CacheServer.Lock.AsynchronousManager>));
                else if (manager != null) manager.Error();
            }
        }
        /// <summary>
        /// 创建锁管理对象
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <param name="onEnter">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        public void GetTryEnterStream(uint timeoutMilliseconds, Action<ReturnValue<AutoCSer.CacheServer.Lock.AsynchronousManager>> onEnter)
        {
            if (onEnter == null) throw new ArgumentNullException();
            AutoCSer.CacheServer.Lock.AsynchronousManager manager = null;
            try
            {
                manager = new CacheServer.Lock.AsynchronousManager(this, timeoutMilliseconds, onEnter);
                onEnter = null;
                manager.TryEnterStream();
                manager = null;
            }
            finally
            {
                if (onEnter != null) onEnter(default(ReturnValue<AutoCSer.CacheServer.Lock.AsynchronousManager>));
                else if (manager != null) manager.Error();
            }
        }

        /// <summary>
        /// 创建锁管理对象
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<AutoCSer.CacheServer.Lock.Manager> GetEnter(uint timeoutMilliseconds)
        {
            return getEnter(timeoutMilliseconds, Enter(timeoutMilliseconds));
        }
        /// <summary>
        /// 创建锁管理对象
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <param name="onEnter"></param>
        public void GetEnter(uint timeoutMilliseconds, Action<ReturnValue<AutoCSer.CacheServer.Lock.AsynchronousManager>> onEnter)
        {
            if (onEnter == null) throw new ArgumentNullException();
            AutoCSer.CacheServer.Lock.AsynchronousManager manager = null;
            try
            {
                manager = new CacheServer.Lock.AsynchronousManager(this, timeoutMilliseconds, onEnter);
                onEnter = null;
                manager.Enter();
                manager = null;
            }
            finally
            {
                if (onEnter != null) onEnter(default(ReturnValue<AutoCSer.CacheServer.Lock.AsynchronousManager>));
                else if (manager != null) manager.Error();
            }
        }
        /// <summary>
        /// 创建锁管理对象
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <param name="onEnter">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        public void GetEnterStream(uint timeoutMilliseconds, Action<ReturnValue<AutoCSer.CacheServer.Lock.AsynchronousManager>> onEnter)
        {
            if (onEnter == null) throw new ArgumentNullException();
            AutoCSer.CacheServer.Lock.AsynchronousManager manager = null;
            try
            {
                manager = new CacheServer.Lock.AsynchronousManager(this, timeoutMilliseconds, onEnter);
                onEnter = null;
                manager.EnterStream();
                manager = null;
            }
            finally
            {
                if (onEnter != null) onEnter(default(ReturnValue<AutoCSer.CacheServer.Lock.AsynchronousManager>));
                else if (manager != null) manager.Error();
            }
        }

        /// <summary>
        /// 创建锁管理对象
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <param name="enterTimeoutMilliseconds">申请超时毫秒数</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<AutoCSer.CacheServer.Lock.TimeoutManager> GetEnter(uint timeoutMilliseconds, int enterTimeoutMilliseconds)
        {
            return new CacheServer.Lock.TimeoutManager(this, timeoutMilliseconds).Wait(enterTimeoutMilliseconds);
        }
        /// <summary>
        /// 创建锁管理对象
        /// </summary>
        /// <param name="enterTimeoutMilliseconds">申请超时毫秒数</param>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <param name="onEnter"></param>
        public void GetEnter(uint enterTimeoutMilliseconds, uint timeoutMilliseconds, Action<ReturnValue<AutoCSer.CacheServer.Lock.AsynchronousTimeoutManager>> onEnter)
        {
            if (onEnter == null) throw new ArgumentNullException();
            AutoCSer.CacheServer.Lock.AsynchronousTimeoutManager manager = null;
            try
            {
                manager = new CacheServer.Lock.AsynchronousTimeoutManager(this, timeoutMilliseconds, onEnter);
                onEnter = null;
                manager.Enter(enterTimeoutMilliseconds);
                manager = null;
            }
            finally
            {
                if (onEnter != null) onEnter(default(ReturnValue<AutoCSer.CacheServer.Lock.AsynchronousTimeoutManager>));
                else if (manager != null) manager.Error();
            }
        }
        /// <summary>
        /// 创建锁管理对象
        /// </summary>
        /// <param name="enterTimeoutMilliseconds">申请超时毫秒数</param>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <param name="onEnter">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        public void GetEnterStream(uint enterTimeoutMilliseconds, uint timeoutMilliseconds, Action<ReturnValue<AutoCSer.CacheServer.Lock.AsynchronousTimeoutManager>> onEnter)
        {
            if (onEnter == null) throw new ArgumentNullException();
            AutoCSer.CacheServer.Lock.AsynchronousTimeoutManager manager = null;
            try
            {
                manager = new CacheServer.Lock.AsynchronousTimeoutManager(this, timeoutMilliseconds, onEnter);
                onEnter = null;
                manager.EnterStream(enterTimeoutMilliseconds);
                manager = null;
            }
            finally
            {
                if (onEnter != null) onEnter(default(ReturnValue<AutoCSer.CacheServer.Lock.AsynchronousTimeoutManager>));
                else if (manager != null) manager.Error();
            }
        }

        /// <summary>
        /// 格式化超时
        /// </summary>
        /// <param name="timeoutMilliseconds"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static long FotmatTimeoutTicks(uint timeoutMilliseconds)
        {
            return timeoutMilliseconds > 0 ? (long)(ulong)timeoutMilliseconds * TimeSpan.TicksPerMillisecond : TimeSpan.TicksPerMinute;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, Lock> constructor;
#if NOJIT
        /// <summary>
        /// 创建锁节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Lock create(Abstract.Node parent)
        {
            return new Lock(parent);
        }
#endif
        static Lock()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, Lock>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, Lock>), typeof(Lock).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, Lock>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(Lock), NodeConstructorParameterTypes);
#endif
        }
    }
}
