using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 256 基分片 字典节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">数据节点类型</typeparam>
    public sealed class ValueFragmentDictionary<keyType, valueType> : Abstract.ValueDictionary<keyType, valueType>
        where keyType : IEquatable<keyType>
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
            stream.Write((byte)ValueData.Data<valueType>.DataType);
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
        private static ValueFragmentDictionary<keyType, valueType> create(Abstract.Node parent)
        {
            return new ValueFragmentDictionary<keyType, valueType>(parent);
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, ValueFragmentDictionary<keyType, valueType>> constructor;
        static ValueFragmentDictionary()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, ValueFragmentDictionary<keyType, valueType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, ValueFragmentDictionary<keyType, valueType>>), typeof(ValueFragmentDictionary<keyType, valueType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, ValueFragmentDictionary<keyType, valueType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(ValueFragmentDictionary<keyType, valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
