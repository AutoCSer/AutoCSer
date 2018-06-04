using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache
{
    /// <summary>
    /// 字典节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="nodeType">数据节点类型</typeparam>
    internal sealed class Dictionary<keyType, nodeType> : Node
        where keyType : IEquatable<keyType>
        where nodeType : Node
    {
        /// <summary>
        /// 字典
        /// </summary>
        private readonly System.Collections.Generic.Dictionary<HashCodeKey<keyType>, nodeType> dictionary = AutoCSer.DictionaryCreator<HashCodeKey<keyType>>.Create<nodeType>();
        /// <summary>
        /// 字典节点
        /// </summary>
        /// <param name="parser"></param>
        private Dictionary(ref OperationParameter.NodeParser parser) { }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        private Node getNext(ref OperationParameter.NodeParser parser)
        {
            HashCodeKey<keyType> key;
            if (HashCodeKey<keyType>.Get(ref parser, out key))
            {
                nodeType node;
                if (dictionary.TryGetValue(key, out node)) return node;
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
                    if (dictionary.Count != 0)
                    {
                        OnRemoved();
                        dictionary.Clear();
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
                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, nodeConstructor(ref parser));
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
                if (!nodeInfo.IsOnRemovedEvent)
                {
                    if (dictionary.Remove(key))
                    {
                        parser.IsOperation = true;
                        parser.ReturnParameter.Set(true);
                    }
                    else parser.ReturnParameter.Set(false);
                }
                else
                {
                    nodeType node;
                    if (dictionary.TryGetValue(key, out node))
                    {
                        dictionary.Remove(key);
                        parser.IsOperation = true;
                        parser.ReturnParameter.Set(true);
                        node.OnRemoved();
                    }
                    else parser.ReturnParameter.Set(false);
                }
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
                case OperationParameter.OperationType.GetCount: parser.ReturnParameter.Set(dictionary.Count); return;
                case OperationParameter.OperationType.ContainsKey:
                    HashCodeKey<keyType> key;
                    if (HashCodeKey<keyType>.Get(ref parser, out key)) parser.ReturnParameter.Set(dictionary.ContainsKey(key));
                    return;
            }
            parser.ReturnParameter.Type = ReturnType.OperationTypeError;
        }

        /// <summary>
        /// 删除节点操作
        /// </summary>
        internal override void OnRemoved()
        {
            if (nodeInfo.IsOnRemovedEvent)
            {
                foreach (System.Collections.Generic.KeyValuePair<HashCodeKey<keyType>, nodeType> node in dictionary) node.Value.OnRemoved();
            }
        }
        /// <summary>
        /// 创建缓存快照
        /// </summary>
        /// <returns></returns>
        internal override Snapshot.Node CreateSnapshot()
        {
            KeyValue<keyType, Snapshot.Node>[] array = new KeyValue<keyType, Snapshot.Node>[dictionary.Count];
            int index = 0;
            foreach (System.Collections.Generic.KeyValuePair<HashCodeKey<keyType>, nodeType> node in dictionary) array[index++].Set(node.Key.Value, node.Value.CreateSnapshot());
            return new Snapshot.Dictionary<keyType>(array);
        }

#if NOJIT
        /// <summary>
        /// 创建字典节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Dictionary<keyType, nodeType> create(ref OperationParameter.NodeParser parser)
        {
            return new Dictionary<keyType, nodeType>(ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<Dictionary<keyType, nodeType>> nodeInfo;
        /// <summary>
        /// 子节点构造函数
        /// </summary>
        private static readonly Constructor<nodeType> nodeConstructor;
        static Dictionary()
        {
            NodeInfo<nodeType> nextNodeInfo = (NodeInfo<nodeType>)typeof(nodeType).GetField(NodeInfoFieldName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetValue(null);
            nodeInfo = new NodeInfo<Dictionary<keyType, nodeType>>
            {
                IsOnRemovedEvent = nextNodeInfo.IsOnRemovedEvent,
#if NOJIT
                Constructor = (Constructor<Dictionary<keyType, nodeType>>)Delegate.CreateDelegate(typeof(Constructor<Dictionary<keyType, nodeType>>), typeof(Dictionary<keyType, nodeType>).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<Dictionary<keyType, nodeType>>)AutoCSer.Emit.Constructor.CreateCache(typeof(Dictionary<keyType, nodeType>), NodeConstructorParameterTypes)
#endif
            };
            nodeConstructor = nextNodeInfo.Constructor;
        }
    }
}
