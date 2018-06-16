using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 消息分发 数据节点
    /// </summary>
    internal abstract class Distributor : Node
    {
        /// <summary>
        /// 消息分发 读文件
        /// </summary>
        protected DistributionFileReader reader;
        /// <summary>
        /// 消息分发 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        protected unsafe Distributor(Cache.Node parent, ref OperationParameter.NodeParser parser) : base(parent, ref parser) { }
        /// <summary>
        /// 删除节点操作
        /// </summary>
        internal override void OnRemoved()
        {
            base.OnRemoved();
            onRemoved();
            if (reader != null) QueueTaskThread.Thread.Default.Add(new QueueTaskThread.DisposeReader(reader));
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetQueryNext(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
            return null;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="buffer"></param>
        internal override void Append(Buffer buffer)
        {
            reader.Append(buffer);
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="getReadIdentity"></param>
        internal override void GetReadIdentity(QueueTaskThread.GetIdentity getReadIdentity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取可发送数量
        /// </summary>
        /// <param name="getSendCount"></param>
        internal void GetSendCount(QueueTaskThread.GetDistributionSendCount getSendCount)
        {
            if (Writer != null)
            {
                if (!Writer.IsDisposed)
                {
                    if (reader == null || reader.IsDisposed) reader = new DistributionFileReader(this, getSendCount.Config);
                    if (!reader.IsDisposed) getSendCount.CallOnReturn(reader.Config.SendClientCount);
                }
                else getSendCount.CallOnReturn(ReturnType.MessageQueueDisposed);
            }
            else getSendCount.CallOnReturn(ReturnType.MessageQueueNotFoundWriter);
        }
    }
    /// <summary>
    /// 消息分发 数据节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class Distributor<valueType> : Distributor
    {
        /// <summary>
        /// 消息分发 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private Distributor(Cache.Node parent, ref OperationParameter.NodeParser parser) : base(parent, ref parser) { }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void QueryEnd(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.MessageQueueEnqueue: enqueue(ref parser, ValueData.Data<valueType>.DataType); return;
                case OperationParameter.OperationType.MessageQueueGetDequeueIdentity:
                    DistributionConfig config = getReaderConfig<DistributionConfig>(ref parser);
                    if (config != null)
                    {
                        if (reader == null || reader.IsDisposed) Writer.OnStart(new QueueTaskThread.GetDistributionSendCount(this, config, ref parser));
                        else parser.ReturnParameter.ReturnParameterSet(reader.Config.SendClientCount);
                    }
                    return;
                case OperationParameter.OperationType.MessageQueueDequeue:
                    Func<AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter>, bool> onReturn = parser.ReturnParameter.Value as Func<AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter>, bool>;
                    if (onReturn != null)
                    {
                        if (parser.ValueData.Type == ValueData.DataType.Int)
                        {
                            DistributionFileReader reader = this.reader;
                            if (reader != null && !reader.IsDisposed)
                            {
                                Writer.OnStart(new QueueTaskThread.GetDistributionMessage(reader, onReturn, ref parser));
                            }
                            else parser.ReturnParameter.ReturnType = ReturnType.MessageQueueNotFoundReader;
                        }
                        else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
                        return;
                    }
                    break;
                case OperationParameter.OperationType.MessageQueueSetDequeueIdentity:
                    if (this.reader != null)
                    {
                        if (parser.ValueData.Type == ValueData.DataType.ULong) Writer.OnStart(new QueueTaskThread.SetDistributionIdentity(reader, parser.ValueData.Int64.ULong));
                        else
                        {
                            ulong[] identitys = null;
                            if (parser.ValueData.GetBinary(ref identitys) && identitys != null)
                            {
                                switch (identitys.Length)
                                {
                                    case 0: return;
                                    case 1: Writer.OnStart(new QueueTaskThread.SetDistributionIdentity(reader, identitys[0])); return;
                                    default: Writer.OnStart(new QueueTaskThread.SetDistributionIdentitys(reader, identitys)); return;
                                }
                            }
                        }
                    }
                    return;
            }
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
        }
#if NOJIT
        /// <summary>
        /// 创建队列消费节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Distributor<valueType> create(Cache.Node parent, ref OperationParameter.NodeParser parser)
        {
            return new Distributor<valueType>(parent, ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<Distributor<valueType>> nodeInfo;
        static Distributor()
        {
            nodeInfo = new NodeInfo<Distributor<valueType>>
            {
                //IsConstructorParameter = true,
                IsOnRemovedEvent = true,
                IsCacheFile = true,
#if NOJIT
                Constructor = (Constructor<Distributor<valueType>>)Delegate.CreateDelegate(typeof(Constructor<Distributor<valueType>>), typeof(Distributor<valueType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<Distributor<valueType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(Distributor<valueType>), NodeConstructorParameterTypes)
#endif
            };
        }
    }
}
