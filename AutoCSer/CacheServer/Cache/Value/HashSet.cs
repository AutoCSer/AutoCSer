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
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetOperationNext(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
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
                    parser.ReturnParameter.Set(true);
                    return;
            }
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
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
                if (hashSet.Remove(key))
                {
                    parser.IsOperation = true;
                    parser.ReturnParameter.Set(true);
                }
                else parser.ReturnParameter.Set(false);
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
                parser.IsOperation = true;
                parser.ReturnParameter.Set(true);
            }
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetQueryNext(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
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
                case OperationParameter.OperationType.GetCount: parser.ReturnParameter.Set(hashSet.Count); return;
                case OperationParameter.OperationType.ContainsKey:
                    if (HashCodeKey<valueType>.Get(ref parser, out key)) parser.ReturnParameter.Set(hashSet.Contains(key));
                    return;
            }
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
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
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static HashSet<valueType> create()
        {
            return new HashSet<valueType>();
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<HashSet<valueType>> constructor;
        static HashSet()
        {
#if NOJIT
            constructor = (Func<HashSet<valueType>>)Delegate.CreateDelegate(typeof(Func<HashSet<valueType>>), typeof(HashSet<valueType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NullValue<Type>.Array, null));
#else
            constructor = (Func<HashSet<valueType>>)AutoCSer.Emit.Constructor.Create(typeof(HashSet<valueType>));
#endif
        }
    }
}
