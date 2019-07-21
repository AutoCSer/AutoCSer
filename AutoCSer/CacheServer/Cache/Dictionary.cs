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
        private System.Collections.Generic.Dictionary<HashCodeKey<keyType>, nodeType> dictionary = AutoCSer.DictionaryCreator<HashCodeKey<keyType>>.Create<nodeType>();
        /// <summary>
        /// 字典节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private Dictionary(Node parent, ref OperationParameter.NodeParser parser) : base(parent) { }
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
                nodeType node;
                if (dictionary.TryGetValue(key, out node)) return node;
                parser.ReturnParameter.ReturnType = ReturnType.NotFoundDictionaryKey;
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
            //if (nodeInfo.IsConstructorParameter && parser.OperationType == OperationParameter.OperationType.GetOrCreateNode)
            //{
            //    HashCodeKey<keyType> key;
            //    if (HashCodeKey<keyType>.Get(ref parser, out key))
            //    {
            //        nodeType node;
            //        if (!dictionary.TryGetValue(key, out node))
            //        {
            //            if ((node = nodeConstructor(this, ref parser)).IsNode)
            //            {
            //                dictionary.Add(key, node);
            //                parser.SetOperationReturnParameter();
            //                return null;
            //            }
            //        }
            //        else
            //        {
            //            if (parser.CheckConstructorParameter(new Value.UnionType { Value = node }.Node.ConstructorParameter)) parser.ReturnParameter.ReturnParameterSet(true);
            //            else parser.ReturnParameter.ReturnType = ReturnType.CheckConstructorParameterError;
            //            return null;
            //        }
            //    }
            //    parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
            //    return null;
            //}
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
                        onClear();
                        dictionary = AutoCSer.DictionaryCreator<HashCodeKey<keyType>>.Create<nodeType>();
                        parser.IsOperation = true;
                    }
                    parser.ReturnParameter.ReturnParameterSet(true);
                    return;
            }
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
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
                    dictionary.Add(key, nodeConstructor(this, ref parser));
                    parser.IsOperation = true;
                }
                parser.ReturnParameter.ReturnParameterSet(true);
            }
            else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
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
                nodeType node;
                if (dictionary.TryGetValue(key, out node))
                {
                    dictionary.Remove(key);
                    parser.SetOperationReturnParameter();
                    node.OnRemoved();
                }
                else parser.ReturnParameter.ReturnParameterSet(false);
            }
            else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
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
                case OperationParameter.OperationType.GetCount: parser.ReturnParameter.ReturnParameterSet(dictionary.Count); return;
                case OperationParameter.OperationType.ContainsKey:
                    HashCodeKey<keyType> key;
                    if (HashCodeKey<keyType>.Get(ref parser, out key)) parser.ReturnParameter.ReturnParameterSet(dictionary.ContainsKey(key));
                    return;
                case OperationParameter.OperationType.CreateShortPath:
                    nodeType node = getNext(ref parser);
                    if (node != null) node.CreateShortPath(ref parser);
                    return;
            }
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
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
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Dictionary<keyType, nodeType> create(Node parent, ref OperationParameter.NodeParser parser)
        {
            return new Dictionary<keyType, nodeType>(parent, ref parser);
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
