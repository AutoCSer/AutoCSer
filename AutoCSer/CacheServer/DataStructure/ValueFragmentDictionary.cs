using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 256 基分片 字典节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="nodeType">数据节点类型</typeparam>
    public sealed class ValueFragmentDictionary<keyType, nodeType> : Abstract.ValueDictionary<keyType, nodeType>
        where keyType : IEquatable<keyType>
        where nodeType : Abstract.Node, Abstract.IValue
    {
        /// <summary>
        /// 256 基分片 字典节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private ValueFragmentDictionary(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.ValueFragmentDictionary);
            stream.Write((byte)ValueData.Data<keyType>.DataType);
            serializeParentDataStructure(stream);
        }

#if NOJIT
        /// <summary>
        /// 创建 256 基分片 字典节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static ValueFragmentDictionary<keyType, nodeType> create(Abstract.Node parent)
        {
            return new ValueFragmentDictionary<keyType, nodeType>(parent);
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, ValueFragmentDictionary<keyType, nodeType>> constructor;
        static ValueFragmentDictionary()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, ValueFragmentDictionary<keyType, nodeType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, ValueFragmentDictionary<keyType, nodeType>>), typeof(ValueFragmentDictionary<keyType, nodeType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, ValueFragmentDictionary<keyType, nodeType>>)AutoCSer.Emit.Constructor.Create(typeof(ValueFragmentDictionary<keyType, nodeType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
