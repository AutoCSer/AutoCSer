using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 消息队列节点 数据节点
    /// </summary>
    internal abstract class Node : Cache.Node
    {
        /// <summary>
        /// 缓存管理
        /// </summary>
        internal readonly CacheManager Cache;
        /// <summary>
        /// 操作数据包
        /// </summary>
        private readonly byte[] packet;
        /// <summary>
        /// 是否已经被移除
        /// </summary>
        protected bool isRemoved;
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
        /// <param name="parser"></param>
        protected Node(ref OperationParameter.NodeParser parser)
        {
            Cache = parser.Cache;
            if (Cache.IsFile)
            {
                packet = parser.CreateReadPacket(OperationParameter.Serializer.HeaderSize);
                if (Cache.IsLoaded)
                {
                    if (packet.Length == IndexIdentity.SerializeSize) Cache.OnDataStructureCreated = onDataStructureCreated;
                    else start();
                }
                else Cache.LoadMessageQueues.Add(this);
            }
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
            if (!isRemoved) start();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        protected abstract void start();

        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetOperationNext(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
            return null;
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void OperationEnd(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
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
        internal abstract void GetReadIdentity(ReaderQueue.GetIdentity getReadIdentity);
    }
}
