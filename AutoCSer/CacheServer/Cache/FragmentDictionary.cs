using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache
{
    /// <summary>
    /// 256 基分片 字典节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="nodeType">数据节点类型</typeparam>
    internal sealed class FragmentDictionary<keyType, nodeType> : Node
        where keyType : IEquatable<keyType>
        where nodeType : Node
    {
        /// <summary>
        /// 字典
        /// </summary>
        private readonly System.Collections.Generic.Dictionary<HashCodeKey<keyType>, nodeType>[] dictionarys = new System.Collections.Generic.Dictionary<HashCodeKey<keyType>, nodeType>[256];
        /// <summary>
        /// 有效数据数量
        /// </summary>
        private int count;
        /// <summary>
        /// 256 基分片 字典节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private FragmentDictionary(Node parent, ref OperationParameter.NodeParser parser) : base(parent) { }

        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        private nodeType getNext(ref OperationParameter.NodeParser parser)
        {
            HashCodeKey<keyType> key;
            if (HashCodeKey<keyType>.Get(ref parser, out key))
            {
                System.Collections.Generic.Dictionary<HashCodeKey<keyType>, nodeType> dictionary = dictionarys[key.HashCode & 0xff];
                if (dictionary != null)
                {
                    nodeType node;
                    if (dictionary.TryGetValue(key, out node)) return node;
                }
                parser.ReturnParameter.Type = ReturnType.NotFoundDictionaryKey;
            }
            return null;
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Node GetOperationNext(ref OperationParameter.NodeParser parser)
        {
            return getNext(ref parser);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void OperationEnd(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.GetOrCreateNode: getOrCreateNode(ref parser); return;
                case OperationParameter.OperationType.Remove: remove(ref parser); return;
                case OperationParameter.OperationType.Clear:
                    if (count != 0)
                    {
                        onClear();
                        Array.Clear(dictionarys, 0, 256);
                        count = 0;
                        parser.IsOperation = true;
                    }
                    parser.ReturnParameter.Set(true);
                    return;
            }
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
        }
        /// <summary>
        /// 获取或者创建节点
        /// </summary>
        /// <param name="parser"></param>
        private void getOrCreateNode(ref OperationParameter.NodeParser parser)
        {
            HashCodeKey<keyType> key;
            if (HashCodeKey<keyType>.Get(ref parser, out key))
            {
                System.Collections.Generic.Dictionary<HashCodeKey<keyType>, nodeType> dictionary = dictionarys[key.HashCode & 0xff];
                if (dictionary == null) dictionarys[key.HashCode & 0xff] = dictionary = AutoCSer.DictionaryCreator<HashCodeKey<keyType>>.Create<nodeType>();
                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, nodeConstructor(this, ref parser));
                    ++count;
                    parser.IsOperation = true;
                }
                parser.ReturnParameter.Set(true);
            }
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="parser"></param>
        private void remove(ref OperationParameter.NodeParser parser)
        {
            HashCodeKey<keyType> key;
            if (HashCodeKey<keyType>.Get(ref parser, out key))
            {
                System.Collections.Generic.Dictionary<HashCodeKey<keyType>, nodeType> dictionary = dictionarys[key.HashCode & 0xff];
                if (dictionary != null)
                {
                    nodeType node;
                    if (dictionary.TryGetValue(key, out node))
                    {
                        dictionary.Remove(key);
                        --count;
                        parser.SetOperationReturnParameter();
                        node.OnRemoved();
                        return;
                    }
                }
                parser.ReturnParameter.Set(false);
            }
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Node GetQueryNext(ref OperationParameter.NodeParser parser)
        {
            return getNext(ref parser);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void QueryEnd(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.GetCount: parser.ReturnParameter.Set(count); return;
                case OperationParameter.OperationType.ContainsKey:
                    HashCodeKey<keyType> key;
                    if (HashCodeKey<keyType>.Get(ref parser, out key))
                    {
                        System.Collections.Generic.Dictionary<HashCodeKey<keyType>, nodeType> dictionary = dictionarys[key.HashCode & 0xff];
                        parser.ReturnParameter.Set(dictionary != null && dictionary.ContainsKey(key));
                    }
                    return;
                case OperationParameter.OperationType.CreateShortPath:
                    nodeType node = getNext(ref parser);
                    if (node != null) node.CreateShortPath(ref parser);
                    return;
            }
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
        }

        /// <summary>
        /// 删除节点操作
        /// </summary>
        internal override void OnRemoved()
        {
            base.OnRemoved();
            onClear();
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        private void onClear()
        {
            if (nodeInfo.IsOnRemovedEvent)
            {
                foreach (System.Collections.Generic.Dictionary<HashCodeKey<keyType>, nodeType> dictionary in dictionarys)
                {
                    if (dictionary != null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<HashCodeKey<keyType>, nodeType> node in dictionary) node.Value.OnRemoved();
                    }
                }
            }
        }
        /// <summary>
        /// 创建缓存快照
        /// </summary>
        /// <returns></returns>
        internal override Snapshot.Node CreateSnapshot()
        {
            KeyValue<keyType, Snapshot.Node>[] array = new KeyValue<keyType, Snapshot.Node>[count];
            int index = 0;
            foreach (System.Collections.Generic.Dictionary<HashCodeKey<keyType>, nodeType> dictionary in dictionarys)
            {
                if (dictionary != null)
                {
                    foreach (System.Collections.Generic.KeyValuePair<HashCodeKey<keyType>, nodeType> node in dictionary) array[index++].Set(node.Key.Value, node.Value.CreateSnapshot());
                }
            }
            return new Snapshot.Dictionary<keyType>(array);
        }
#if NOJIT
        /// <summary>
        /// 创建字典节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static FragmentDictionary<keyType, nodeType> create(Node parent, ref OperationParameter.NodeParser parser)
        {
            return new FragmentDictionary<keyType, nodeType>(parent, ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<FragmentDictionary<keyType, nodeType>> nodeInfo;
        /// <summary>
        /// 子节点构造函数
        /// </summary>
        private static readonly Constructor<nodeType> nodeConstructor;
        static FragmentDictionary()
        {
            NodeInfo<nodeType> nextNodeInfo = (NodeInfo<nodeType>)typeof(nodeType).GetField(NodeInfoFieldName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetValue(null);
            nodeInfo = new NodeInfo<FragmentDictionary<keyType, nodeType>>
            {
                IsOnRemovedEvent = nextNodeInfo.IsOnRemovedEvent,
#if NOJIT
                Constructor = (Constructor<FragmentDictionary<keyType, nodeType>>)Delegate.CreateDelegate(typeof(Constructor<FragmentDictionary<keyType, nodeType>>), typeof(FragmentDictionary<keyType, nodeType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<FragmentDictionary<keyType, nodeType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(FragmentDictionary<keyType, nodeType>), NodeConstructorParameterTypes)
#endif
            };
            nodeConstructor = nextNodeInfo.Constructor;
        }
    }
}
