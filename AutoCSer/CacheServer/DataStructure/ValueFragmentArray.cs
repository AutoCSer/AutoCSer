using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 32768 基分段 数组节点
    /// </summary>
    /// <typeparam name="valueType">元素节点类型</typeparam>
    public sealed class ValueFragmentArray<valueType> : Abstract.ValueArray<valueType>
    {
        /// <summary>
        /// 32768 基分段 数组节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private ValueFragmentArray(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.ValueFragmentArray);
            stream.Write((byte)ValueData.Data<valueType>.DataType);
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
        private static ValueFragmentArray<valueType> create(Abstract.Node parent)
        {
            return new ValueFragmentArray<valueType>(parent);
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, ValueFragmentArray<valueType>> constructor;
        static ValueFragmentArray()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, ValueFragmentArray<valueType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, ValueFragmentArray<valueType>>), typeof(ValueFragmentArray<valueType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, ValueFragmentArray<valueType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(ValueFragmentArray<valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}

