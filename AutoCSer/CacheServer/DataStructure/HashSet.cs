using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 哈希表节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed class HashSet<valueType> : Abstract.HashSet<valueType>, Abstract.IValueNode
        where valueType : IEquatable<valueType>
    {
        /// <summary>
        /// 哈希表节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private HashSet(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.HashSet);
            stream.Write((byte)ValueData.Data<valueType>.DataType);
            serializeParentDataStructure(stream);
        }

#if NOJIT
        /// <summary>
        /// 创建哈希表节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static HashSet<valueType> create(Abstract.Node parent)
        {
            return new HashSet<valueType>(parent);
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, HashSet<valueType>> constructor;

        static HashSet()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, HashSet<valueType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, HashSet<valueType>>), typeof(HashSet<valueType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, HashSet<valueType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(HashSet<valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
