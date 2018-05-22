using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 数组节点
    /// </summary>
    /// <typeparam name="nodeType">元素节点类型</typeparam>
    public sealed class ValueArray<nodeType> : Abstract.ValueArray<nodeType>
        where nodeType : Abstract.Node, Abstract.IValue
    {
        /// <summary>
        /// 数组节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private ValueArray(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.ValueArray);
            serializeParentDataStructure(stream);
        }

#if NOJIT
        /// <summary>
        /// 创建数组节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static ValueArray<nodeType> create(Abstract.Node parent)
        {
            return new ValueArray<nodeType>(parent);
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, ValueArray<nodeType>> constructor;
        static ValueArray()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, ValueArray<nodeType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, ValueArray<nodeType>>), typeof(ValueArray<nodeType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, ValueArray<nodeType>>)AutoCSer.Emit.Constructor.Create(typeof(ValueArray<nodeType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
