using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 256 基分片 哈希表节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed class FragmentHashSet<valueType> : Abstract.HashSet<valueType>, Abstract.IValueNode
        where valueType : IEquatable<valueType>
    {
        /// <summary>
        /// 256 基分片 哈希表节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private FragmentHashSet(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.FragmentHashSet);
            stream.Write((byte)ValueData.Data<valueType>.DataType);
            serializeParentDataStructure(stream);
        }

#if NOJIT
        /// <summary>
        /// 创建 256 基分片 哈希表节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static FragmentHashSet<valueType> create(Abstract.Node parent)
        {
            return new FragmentHashSet<valueType>(parent);
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, FragmentHashSet<valueType>> constructor;

        static FragmentHashSet()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, FragmentHashSet<valueType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, FragmentHashSet<valueType>>), typeof(FragmentHashSet<valueType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, FragmentHashSet<valueType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(FragmentHashSet<valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
