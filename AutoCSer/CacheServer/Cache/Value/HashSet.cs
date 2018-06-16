using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.Value
{
    /// <summary>
    /// 哈希表节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class HashSet<valueType> : Node
        where valueType : IEquatable<valueType>
    {
        /// <summary>
        /// 哈希表
        /// </summary>
        private readonly System.Collections.Generic.HashSet<HashCodeKey<valueType>> hashSet = AutoCSer.HashSetCreator<HashCodeKey<valueType>>.Create();
        /// <summary>
        /// 哈希表节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private HashSet(Cache.Node parent, ref OperationParameter.NodeParser parser) : base(parent) { }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetOperationNext(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
            return null;
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void OperationEnd(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.Remove: remove(ref parser); return;
                case OperationParameter.OperationType.SetValue: setValue(ref parser); return;
                case OperationParameter.OperationType.Clear:
                    if (hashSet.Count != 0)
                    {
                        hashSet.Clear();
                        parser.IsOperation = true;
                    }
                    parser.ReturnParameter.ReturnParameterSet(true);
                    return;
            }
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="parser"></param>
        private void remove(ref OperationParameter.NodeParser parser)
        {
            HashCodeKey<valueType> key;
            if (HashCodeKey<valueType>.Get(ref parser, out key))
            {
                if (hashSet.Remove(key)) parser.SetOperationReturnParameter();
                else parser.ReturnParameter.ReturnParameterSet(false);
            }
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="parser"></param>
        private void setValue(ref OperationParameter.NodeParser parser)
        {
            HashCodeKey<valueType> key;
            if (HashCodeKey<valueType>.Get(ref parser, out key))
            {
                hashSet.Add(key);
                parser.SetOperationReturnParameter();
            }
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetQueryNext(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
            return null;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void QueryEnd(ref OperationParameter.NodeParser parser)
        {
            HashCodeKey<valueType> key;
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.GetCount: parser.ReturnParameter.ReturnParameterSet(hashSet.Count); return;
                case OperationParameter.OperationType.ContainsKey:
                    if (HashCodeKey<valueType>.Get(ref parser, out key)) parser.ReturnParameter.ReturnParameterSet(hashSet.Contains(key));
                    return;
            }
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
        }

        /// <summary>
        /// 创建缓存快照
        /// </summary>
        /// <returns></returns>
        internal override Snapshot.Node CreateSnapshot()
        {
            valueType[] array = new valueType[hashSet.Count];
            int index = 0;
            foreach (HashCodeKey<valueType> node in hashSet) array[index++] = node.Value;
            return new Snapshot.Value.HashSet<valueType>(array);
        }
#if NOJIT
        /// <summary>
        /// 创建哈希表节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static HashSet<valueType> create(Cache.Node parent, ref OperationParameter.NodeParser parser)
        {
            return new HashSet<valueType>(parent, ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<HashSet<valueType>> nodeInfo;
        static HashSet()
        {
            nodeInfo = new NodeInfo<HashSet<valueType>>
            {
#if NOJIT
                Constructor = (Constructor<HashSet<valueType>>)Delegate.CreateDelegate(typeof(Constructor<HashSet<valueType>>), typeof(HashSet<valueType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<HashSet<valueType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(HashSet<valueType>), NodeConstructorParameterTypes)
#endif
            };
        }
    }
}
