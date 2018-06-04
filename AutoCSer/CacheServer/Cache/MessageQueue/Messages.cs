using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 消息节点 数据节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class Messages<valueType> : Node
    {
        /// <summary>
        /// 消息节点 数据节点
        /// </summary>
        /// <param name="parser"></param>
        private Messages(ref OperationParameter.NodeParser parser) : base(ref parser) { }
        /// <summary>
        /// 删除节点操作
        /// </summary>
        internal override void OnRemoved()
        {
            isRemoved = true;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        protected override void start()
        {

        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="buffer"></param>
        internal override void Append(Buffer buffer)
        {
        }
        /// <summary>
        /// 获取当前读取数据标识
        /// </summary>
        /// <param name="getReadIdentity"></param>
        internal override void GetReadIdentity(ReaderQueue.GetIdentity getReadIdentity)
        {
        }

#if NOJIT
        /// <summary>
        /// 创建队列消费节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Messages<valueType> create(ref OperationParameter.NodeParser parser)
        {
            return new Messages<valueType>(ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<Messages<valueType>> nodeInfo;
        static Messages()
        {
            nodeInfo = new NodeInfo<Messages<valueType>>
            {
                IsOnRemovedEvent = true,
                IsCacheFile = true,
#if NOJIT
                Constructor = (Constructor<Messages<valueType>>)Delegate.CreateDelegate(typeof(Constructor<Messages<valueType>>), typeof(Messages<valueType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<Messages<valueType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(Messages<valueType>), NodeConstructorParameterTypes)
#endif
            };
        }
    }
}
