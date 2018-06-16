using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.MessageQueue
{
    /// <summary>
    /// 消费分发节点（无序消费）
    /// </summary>
    /// <typeparam name="valueType">元素节点类型</typeparam>
    public sealed partial class Distributor<valueType> : Abstract.MessageQueue<valueType>
    {
        /// <summary>
        /// 消费分发节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private Distributor(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.MessageDistributor);
            stream.Write((byte)ValueData.Data<valueType>.DataType);
            serializeParentDataStructure(stream);
        }

        /// <summary>
        /// 获取服务端可发送数量
        /// </summary>
        /// <param name="config">消息分发 读取配置</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Parameter.Value GetSendCountNode(Cache.MessageQueue.DistributionConfig config)
        {
            Parameter.Value node = new Parameter.Value(this, OperationParameter.OperationType.MessageQueueGetDequeueIdentity);
            node.Parameter.SetJson(config);
            return node;
        }
        /// <summary>
        /// 获取服务端可发送数量
        /// </summary>
        /// <param name="config">消息分发 读取配置</param>
        /// <returns></returns>
        public ReturnValue<uint> GetSendCount(Cache.MessageQueue.DistributionConfig config)
        {
            return Client.GetUInt(ClientDataStructure.Client.MasterQueryAsynchronous(GetSendCountNode(config)));
        }
        /// <summary>
        /// 获取服务端可发送数量
        /// </summary>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="onEnqueue"></param>
        /// <returns></returns>
        public void GetSendCount(Cache.MessageQueue.DistributionConfig config, Action<ReturnValue<uint>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronous(GetSendCountNode(config), onEnqueue);
        }
        /// <summary>
        /// 获取服务端可发送数量
        /// </summary>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="onEnqueue"></param>
        /// <returns></returns>
        public void GetSendCount(Cache.MessageQueue.DistributionConfig config, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronous(GetSendCountNode(config), onEnqueue);
        }
        /// <summary>
        /// 获取服务端可发送数量
        /// </summary>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="onEnqueue">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetSendCountStream(Cache.MessageQueue.DistributionConfig config, Action<ReturnValue<uint>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronousStream(GetSendCountNode(config), onEnqueue);
        }
        /// <summary>
        /// 获取服务端可发送数量
        /// </summary>
        /// <param name="config">消息分发 读取配置</param>
        /// <param name="onEnqueue">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetSendCountStream(Cache.MessageQueue.DistributionConfig config, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onEnqueue)
        {
            if (onEnqueue == null) throw new ArgumentNullException();
            ClientDataStructure.Client.MasterQueryAsynchronousStream(GetSendCountNode(config), onEnqueue);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sendCount">服务端单次可发送数据量</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Parameter.Value GetMessageNode(int sendCount)
        {
            return new Parameter.Value(this, OperationParameter.OperationType.MessageQueueDequeue, sendCount);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sendCount">服务端单次可发送数据量</param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback GetMessage(int sendCount, Action<ReturnValue<KeyValue<ulong, valueType>>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.MasterQueryKeepCallback(GetMessageNode(sendCount), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sendCount">服务端单次可发送数据量</param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback GetMessage(int sendCount, Action<AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.MasterQueryKeepCallback(GetMessageNode(sendCount), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sendCount">服务端单次可发送数据量</param>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback GetMessageStream(int sendCount, Action<ReturnValue<KeyValue<ulong, valueType>>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.MasterQueryKeepCallbackStream(GetMessageNode(sendCount), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sendCount">服务端单次可发送数据量</param>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback GetMessageStream(int sendCount, Action<AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.MasterQueryKeepCallbackStream(GetMessageNode(sendCount), onGet);
        }

        /// <summary>
        /// 消息队列设置当前读取数据标识
        /// </summary>
        /// <param name="identity">确认已完成消息标识</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Parameter.Value GetSetDequeueIdentityNode(ulong identity)
        {
            return new Parameter.Value(this, OperationParameter.OperationType.MessageQueueSetDequeueIdentity, identity);
        }
        /// <summary>
        /// 消息队列设置当前读取数据标识
        /// </summary>
        /// <param name="identity">确认已完成消息标识</param>
        public void SetDequeueIdentity(ulong identity)
        {
            ClientDataStructure.Client.MasterQueryOnly(GetSetDequeueIdentityNode(identity));
        }
        /// <summary>
        /// 消息队列设置当前读取数据标识
        /// </summary>
        /// <param name="identitys">确认已完成消息标识集合</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Parameter.Value GetSetDequeueIdentityNode(ulong[] identitys)
        {
            Parameter.Value node = new Parameter.Value(this, OperationParameter.OperationType.MessageQueueSetDequeueIdentity);
            node.Parameter.SetBinary(identitys);
            return node;
        }
        /// <summary>
        /// 消息队列设置当前读取数据标识
        /// </summary>
        /// <param name="identitys">确认已完成消息标识集合</param>
        public void SetDequeueIdentity(ulong[] identitys)
        {
            if (identitys != null)
            {
                switch (identitys.Length)
                {
                    case 0: return;
                    case 1: SetDequeueIdentity(identitys[0]); return;
                    default: ClientDataStructure.Client.MasterQueryOnly(GetSetDequeueIdentityNode(identitys)); return;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, Distributor<valueType>> constructor;
#if NOJIT
        /// <summary>
        /// 创建消费分发节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Distributor<valueType> create(Abstract.Node parent)
        {
            return new Distributor<valueType>(parent);
        }
#endif
        static Distributor()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, Distributor<valueType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, Distributor<valueType>>), typeof(Distributor<valueType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, Distributor<valueType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(Distributor<valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
