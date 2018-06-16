using System;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 队列消费节点 数据节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class Consumer<valueType> : Node
    {
        /// <summary>
        /// 队列数据 读文件
        /// </summary>
        private FileReader reader;
        /// <summary>
        /// 队列消费节点 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private Consumer(Cache.Node parent, ref OperationParameter.NodeParser parser) : base(parent, ref parser) { }
        /// <summary>
        /// 删除节点操作
        /// </summary>
        internal override void OnRemoved()
        {
            base.OnRemoved();
            onRemoved();
            if (reader != null) Writer.OnStart(new QueueTaskThread.DisposeReader(reader));
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
            if (Writer != null)
            {
                if (!Writer.IsDisposed)
                {
                    if (reader == null || reader.IsDisposed) reader = new FileReader(this, getReadIdentity.Config, 0);
                    if (!reader.IsDisposed) getReadIdentity.CallOnReturn(reader.Identity);
                }
                else getReadIdentity.CallOnReturn(ReturnType.MessageQueueDisposed);
            }
            else getReadIdentity.CallOnReturn(ReturnType.MessageQueueNotFoundWriter);
        }
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
                    ReaderConfig config = getReaderConfig<ReaderConfig>(ref parser);
                    if (config != null)
                    {
                        if (reader == null || reader.IsDisposed) Writer.OnStart(new QueueTaskThread.GetIdentity(this, config, ref parser));
                        else parser.ReturnParameter.ReturnParameterSet(reader.Identity);
                    }
                    return;
                case OperationParameter.OperationType.MessageQueueDequeue:
                    if (parser.ValueData.Type == ValueData.DataType.ULong)
                    {
                        FileReader reader = this.reader;
                        if (reader != null && !reader.IsDisposed)
                        {
                            if (parser.OnReturn != null) Writer.OnStart(new QueueTaskThread.GetMessage(reader, ref parser));
                        }
                        else parser.ReturnParameter.ReturnType = ReturnType.MessageQueueNotFoundReader;
                    }
                    else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
                    return;
                case OperationParameter.OperationType.MessageQueueSetDequeueIdentity: setDequeueIdentity(ref parser, this.reader); return;
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
        private static Consumer<valueType> create(Cache.Node parent, ref OperationParameter.NodeParser parser)
        {
            return new Consumer<valueType>(parent, ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<Consumer<valueType>> nodeInfo;
        static Consumer()
        {
            nodeInfo = new NodeInfo<Consumer<valueType>>
            {
                IsOnRemovedEvent = true,
                IsCacheFile = true,
#if NOJIT
                Constructor = (Constructor<Consumer<valueType>>)Delegate.CreateDelegate(typeof(Constructor<Consumer<valueType>>), typeof(Consumer<valueType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<Consumer<valueType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(Consumer<valueType>), NodeConstructorParameterTypes)
#endif
            };
        }
    }
}
