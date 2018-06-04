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
    internal sealed class QueueConsumers<valueType> : QueueNode
    {
        /// <summary>
        /// 队列数据 读文件
        /// </summary>
        private readonly System.Collections.Generic.Dictionary<int, File.QueueReader> readerDictionary = AutoCSer.DictionaryCreator<int>.Create<File.QueueReader>();
        /// <summary>
        /// 队列数据 读文件
        /// </summary>
        private File.QueueReader[] readers = NullValue<File.QueueReader>.Array;
        /// <summary>
        /// 多消费者队列消费节点 数据节点
        /// </summary>
        /// <param name="parser"></param>
        private QueueConsumers(ref OperationParameter.NodeParser parser) : base(ref parser) { }
        /// <summary>
        /// 删除节点操作
        /// </summary>
        internal override void OnRemoved()
        {
            onRemoved();
            if (readers.Length != 0) ReaderQueue.TaskThread.Default.Add(new ReaderQueue.DisposeQueueReaders(this, readerDictionary));
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
                    foreach (File.QueueReader reader in readers)
                    {
                        if (reader != null) reader.Append(ref bufferCount);
                    }
                }
                else
                {
                    foreach (File.QueueReader reader in readerDictionary.Values) reader.Append(ref bufferCount);
                }
            }
            finally { bufferCount.FreeBuffer(); }
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="getReadIdentity"></param>
        internal override void GetReadIdentity(ReaderQueue.GetIdentity getReadIdentity)
        {
            if (Writer != null)
            {
                if (!Writer.IsDisposed)
                {
                    int readerIndex = getReadIdentity.ReaderIndex;
                    if (readerIndex >= readers.Length) readers = readers.copyNew(Math.Min(Math.Max(readers.Length << 1, readerIndex + 1), (int)Config.QueueReader.MaxReaderCount));
                    File.QueueReader reader = readers[readerIndex];
                    if (reader == null || reader.IsDisposed)
                    {
                        reader = new File.QueueReader(this, getReadIdentity.Config, readerIndex);
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
                    if ((uint)index >= Config.QueueReader.MaxReaderCount) parser.ReturnParameter.Type = ReturnType.MessageQueueReaderIndexOutOfRange;
                    return index;
                }
            }
            parser.ReturnParameter.Type = ReturnType.ValueDataLoadError;
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
                        Config.QueueReader config = getReaderConfig(ref parser);
                        if (config != null)
                        {
                            if (readerIndex < readers.Length)
                            {
                                File.QueueReader reader = readers[readerIndex];
                                if (reader != null && !reader.IsDisposed)
                                {
                                    parser.ReturnParameter.Set(reader.Identity);
                                    return null;
                                }
                            }
                            ReaderQueue.TaskThread.Default.Add(new ReaderQueue.GetIdentity(this, config, ref parser, readerIndex));
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
                                File.QueueReader reader = readers[readerIndex];
                                if (reader != null && !reader.IsDisposed)
                                {
                                    if (parser.OnReturn != null) ReaderQueue.TaskThread.Default.Add(new ReaderQueue.GetMessage(reader, ref parser));
                                    return null;
                                }
                            }
                            parser.ReturnParameter.Type = ReturnType.MessageQueueNotFoundReader;
                        }
                        else parser.ReturnParameter.Type = ReturnType.ValueDataLoadError;
                    }
                    return null;
                case OperationParameter.OperationType.MessageQueueSetDequeueIdentity:
                    if ((uint)(readerIndex = getReaderIndex(ref parser)) < readers.Length) setDequeueIdentity(ref parser, readers[readerIndex]);
                    return null;
            }
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
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
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
        }

#if NOJIT
        /// <summary>
        /// 创建队列消费节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static QueueConsumers<valueType> create(ref OperationParameter.NodeParser parser)
        {
            return new QueueConsumers<valueType>(ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<QueueConsumers<valueType>> nodeInfo;
        static QueueConsumers()
        {
            nodeInfo = new NodeInfo<QueueConsumers<valueType>>
            {
                IsOnRemovedEvent = true,
                IsCacheFile = true,
#if NOJIT
                Constructor = (Constructor<QueueConsumers<valueType>>)Delegate.CreateDelegate(typeof(Constructor<QueueConsumers<valueType>>), typeof(QueueConsumers<valueType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<QueueConsumers<valueType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(QueueConsumers<valueType>), NodeConstructorParameterTypes)
#endif
            };
        }
    }
}
