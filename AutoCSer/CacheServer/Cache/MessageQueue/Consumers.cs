using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 多消费者队列消费节点 数据节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class Consumers<valueType> : Node
    {
        /// <summary>
        /// 队列数据 读文件
        /// </summary>
        private readonly System.Collections.Generic.Dictionary<int, FileReader> readerDictionary = AutoCSer.DictionaryCreator<int>.Create<FileReader>();
        /// <summary>
        /// 队列数据 读文件
        /// </summary>
        private FileReader[] readers = NullValue<FileReader>.Array;
        /// <summary>
        /// 多消费者队列消费节点 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private Consumers(Cache.Node parent, ref OperationParameter.NodeParser parser) : base(parent, ref parser) { }
        /// <summary>
        /// 删除节点操作
        /// </summary>
        internal override void OnRemoved()
        {
            base.OnRemoved();
            onRemoved();
            if (readers.Length != 0) Writer.OnStart(new QueueTaskThread.DisposeReaders(this, readerDictionary));
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="buffer"></param>
        internal override void Append(Buffer buffer)
        {
            BufferCount bufferCount = new BufferCount(buffer);
            try
            {
                if (readers.Length <= readerDictionary.Count >> 1)
                {
                    foreach (FileReader reader in readers)
                    {
                        if (reader != null) reader.Append(ref bufferCount);
                    }
                }
                else
                {
                    foreach (FileReader reader in readerDictionary.Values) reader.Append(ref bufferCount);
                }
            }
            finally { bufferCount.FreeBuffer(); }
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
                    int readerIndex = getReadIdentity.ReaderIndex;
                    if (readerIndex >= readers.Length) readers = readers.copyNew(Math.Min(Math.Max(readers.Length << 1, readerIndex + 1), (int)ReaderConfig.MaxReaderCount));
                    FileReader reader = readers[readerIndex];
                    if (reader == null || reader.IsDisposed)
                    {
                        reader = new FileReader(this, getReadIdentity.Config, readerIndex);
                        readerDictionary[readerIndex] = reader;
                        readers[readerIndex] = reader;
                    }
                    if (!reader.IsDisposed) getReadIdentity.CallOnReturn(reader.Identity);
                }
                else getReadIdentity.CallOnReturn(ReturnType.MessageQueueDisposed);
            }
            else getReadIdentity.CallOnReturn(ReturnType.MessageQueueNotFoundWriter);
        }
        /// <summary>
        /// 获取队列数据 读文件索引
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        private int getReaderIndex(ref OperationParameter.NodeParser parser)
        {
            if (parser.ValueData.Type == ValueData.DataType.Int)
            {
                int index = parser.ValueData.Int64.Int;
                if (parser.LoadValueData() && parser.IsEnd)
                {
                    if ((uint)index >= ReaderConfig.MaxReaderCount) parser.ReturnParameter.ReturnType = ReturnType.MessageQueueReaderIndexOutOfRange;
                    return index;
                }
            }
            parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
            return -1;
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetQueryNext(ref OperationParameter.NodeParser parser)
        {
            int readerIndex;
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.MessageQueueGetDequeueIdentity:
                    if ((readerIndex = getReaderIndex(ref parser)) >= 0)
                    {
                        ReaderConfig config = getReaderConfig<ReaderConfig>(ref parser);
                        if (config != null)
                        {
                            if (readerIndex < readers.Length)
                            {
                                FileReader reader = readers[readerIndex];
                                if (reader != null && !reader.IsDisposed)
                                {
                                    parser.ReturnParameter.ReturnParameterSet(reader.Identity);
                                    return null;
                                }
                            }
                            Writer.OnStart(new QueueTaskThread.GetIdentity(this, config, ref parser, readerIndex));
                        }
                    }
                    return null;
                case OperationParameter.OperationType.MessageQueueDequeue:
                    if ((readerIndex = getReaderIndex(ref parser)) >= 0)
                    {
                        if (parser.ValueData.Type == ValueData.DataType.ULong)
                        {
                            if (readerIndex < readers.Length)
                            {
                                FileReader reader = readers[readerIndex];
                                if (reader != null && !reader.IsDisposed)
                                {
                                    if (parser.OnReturn != null) Writer.OnStart(new QueueTaskThread.GetMessage(reader, ref parser));
                                    return null;
                                }
                            }
                            parser.ReturnParameter.ReturnType = ReturnType.MessageQueueNotFoundReader;
                        }
                        else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
                    }
                    return null;
                case OperationParameter.OperationType.MessageQueueSetDequeueIdentity:
                    if ((uint)(readerIndex = getReaderIndex(ref parser)) < readers.Length) setDequeueIdentity(ref parser, readers[readerIndex]);
                    return null;
            }
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
            return null;
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
        private static Consumers<valueType> create(Cache.Node parent, ref OperationParameter.NodeParser parser)
        {
            return new Consumers<valueType>(parent, ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<Consumers<valueType>> nodeInfo;
        static Consumers()
        {
            nodeInfo = new NodeInfo<Consumers<valueType>>
            {
                IsOnRemovedEvent = true,
                IsCacheFile = true,
#if NOJIT
                Constructor = (Constructor<Consumers<valueType>>)Delegate.CreateDelegate(typeof(Constructor<Consumers<valueType>>), typeof(Consumers<valueType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<Consumers<valueType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(Consumers<valueType>), NodeConstructorParameterTypes)
#endif
            };
        }
    }
}
