using System;
using System.Reflection;

namespace AutoCSer.CacheServer.DataStructure.MessageQueue
{
    /// <summary>
    /// 消息节点（多消费者无序）
    /// </summary>
    /// <typeparam name="valueType">元素节点类型</typeparam>
    public sealed partial class Messages<valueType> : Abstract.MessageQueue<valueType>
    {
        /// <summary>
        /// 消息节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private Messages(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.Messages);
            stream.Write((byte)ValueData.Data<valueType>.DataType);
            serializeParentDataStructure(stream);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, Messages<valueType>> constructor;
#if NOJIT
        /// <summary>
        /// 创建消息节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Messages<nodeType> create(Abstract.Node parent)
        {
            return new Messages<nodeType>(parent);
        }
#endif
        static Messages()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, Messages<nodeType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, Messages<nodeType>>), typeof(Messages<nodeType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, Messages<valueType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(Messages<valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}

