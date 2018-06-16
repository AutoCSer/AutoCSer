using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 消息队列节点 数据节点
    /// </summary>
    internal abstract class Node : Value.Node
    {
        /// <summary>
        /// 缓存管理
        /// </summary>
        internal readonly CacheManager Cache;
        /// <summary>
        /// 队列数据 写文件
        /// </summary>
        internal FileWriter Writer;
        /// <summary>
        /// 操作数据包
        /// </summary>
        private readonly byte[] packet;
        /// <summary>
        /// 文件保存路径
        /// </summary>
        internal string FilePath
        {
            get
            {
                return Cache.MasterConfig.MessageQueuePath +  packet.toHex();
            }
        }
        /// <summary>
        /// 消息节点 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        protected Node(Cache.Node parent, ref OperationParameter.NodeParser parser) : base(parent)
        {
            Cache = parser.Cache;
            if (Cache.IsFile)
            {
                packet = parser.CreateReadPacket(OperationParameter.Serializer.HeaderSize);
                tryStart();
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        protected void tryStart()
        {
            if (Cache.IsLoaded)
            {
                if (packet.Length == IndexIdentity.SerializeSize) Cache.OnDataStructureCreated = onDataStructureCreated;
                else start();
            }
            else Cache.LoadMessageQueues.Add(this);
        }
        /// <summary>
        /// 创建缓存节点后的回调操作
        /// </summary>
        /// <param name="identity"></param>
        private unsafe void onDataStructureCreated(IndexIdentity identity)
        {
            fixed (byte* dataFixed = packet) identity.UnsafeSerialize(dataFixed);
            start();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Start()
        {
            if (IsNode) start();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void start()
        {
            AutoCSer.Threading.ThreadPool.Tiny.Start((Writer = new FileWriter(this)).Start);
        }
        /// <summary>
        /// 删除节点操作
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onRemoved()
        {
            if (Writer != null) Writer.TryDispose();
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="dataType"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void enqueue(ref OperationParameter.NodeParser parser, AutoCSer.CacheServer.ValueData.DataType dataType)
        {
            if (parser.ValueData.Type == dataType)
            {
                if (parser.OnReturn != null) Writer.Append(new Buffer(this, ref parser));
            }
            else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
        }
        /// <summary>
        /// 消息队列设置当前读取数据标识
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="reader"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void setDequeueIdentity(ref OperationParameter.NodeParser parser, FileReader reader)
        {
            if (reader != null && parser.ValueData.Type == ValueData.DataType.ULong) reader.TrySetIdentity(parser.ValueData.Int64.ULong);
        }
        /// <summary>
        /// 获取队列数据 读取配置
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected configType getReaderConfig<configType>(ref OperationParameter.NodeParser parser)
            where configType : ReaderConfig
        {
            if (parser.OnReturn != null)
            {
                if (parser.ValueData.Type == ValueData.DataType.Json)
                {
                    configType config = null;
                    if (parser.ValueData.GetJson(ref config) && config != null)
                    {
                        if (Writer != null)
                        {
                            if (!Writer.IsDisposed) return config;
                            else parser.ReturnParameter.ReturnType = ReturnType.MessageQueueDisposed;
                        }
                        else parser.ReturnParameter.ReturnType = ReturnType.MessageQueueNotFoundWriter;
                        return null;
                    }
                }
                parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
            }
            return null;
        }

        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetOperationNext(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
            return null;
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void OperationEnd(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
        }
        /// <summary>
        /// 创建缓存快照
        /// </summary>
        /// <returns></returns>
        internal override Snapshot.Node CreateSnapshot()
        {
            return Snapshot.NoSerialize.Default;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="buffer"></param>
        internal abstract void Append(Buffer buffer);
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="getReadIdentity"></param>
        internal abstract void GetReadIdentity(QueueTaskThread.GetIdentity getReadIdentity);
    }
}
