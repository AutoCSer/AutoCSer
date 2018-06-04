using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 数组节点
    /// </summary>
    /// <typeparam name="nodeType">元素节点类型</typeparam>
    public sealed class Array<nodeType> : Abstract.Array<nodeType> where nodeType : Abstract.Node
    {
        /// <summary>
        /// 数组节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private Array(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.Array);
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
        private static Array<nodeType> create(Abstract.Node parent)
        {
            return new Array<nodeType>(parent);
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, Array<nodeType>> constructor;
        static Array()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, Array<nodeType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, Array<nodeType>>), typeof(Array<nodeType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, Array<nodeType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(Array<nodeType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
