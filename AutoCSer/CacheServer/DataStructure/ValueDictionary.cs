using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 字典节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">数据节点类型</typeparam>
    public sealed class ValueDictionary<keyType, valueType> : Abstract.ValueDictionary<keyType, valueType>
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 字典节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private ValueDictionary(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.ValueDictionary);
            stream.Write((byte)ValueData.Data<valueType>.DataType);
            stream.Write((byte)ValueData.Data<keyType>.DataType);
            serializeParentDataStructure(stream);
        }

#if NOJIT
        /// <summary>
        /// 创建字典节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static ValueDictionary<keyType, valueType> create(Abstract.Node parent)
        {
            return new ValueDictionary<keyType, valueType>(parent);
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, ValueDictionary<keyType, valueType>> constructor;
        static ValueDictionary()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, ValueDictionary<keyType, valueType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, ValueDictionary<keyType, valueType>>), typeof(ValueDictionary<keyType, valueType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, ValueDictionary<keyType, valueType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(ValueDictionary<keyType, valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
