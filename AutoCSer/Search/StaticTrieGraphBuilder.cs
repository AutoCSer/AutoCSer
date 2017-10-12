using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定静态节点池的字符串 Trie 图
    /// </summary>
    public sealed partial class StaticStringTrieGraph
    {
        /// <summary>
        /// Trie 图创建器
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        internal struct Builder
        {
            /// <summary>
            /// 已创建缓存集合
            /// </summary>
            private Dictionary<StringTrieGraph.Node, int> cache;
            /// <summary>
            /// 当前创建的节点索引
            /// </summary>
            private int nodeIndex;
            /// <summary>
            /// 创建 Trie 图
            /// </summary>
            /// <param name="staticGraph"></param>
            /// <param name="graph"></param>
            internal void Create(StaticStringTrieGraph staticGraph, StringTrieGraph graph)
            {
                cache = DictionaryCreator.CreateOnly<StringTrieGraph.Node, int>();
                StringTrieGraph.Node boot = graph.Boot;
                bool isCreate = false;
                Monitor.Enter(NodePool.Lock);
                try
                {
                    staticGraph.setBoot(create(boot));
                    isCreate = true;
                }
                finally
                {
                    if (isCreate || cache == null) Monitor.Exit(NodePool.Lock);
                    else
                    {
                        try
                        {
                            staticGraph.CancelBuilder();
                            foreach (int index in cache.Values)
                            {
                                if (index == nodeIndex) nodeIndex = 0;
                                NodePool.Pool[index >> ArrayPool.ArraySizeBit][index & ArrayPool.ArraySizeAnd].CancelBuilder();
                            }
                            NodePool.FreeNoLock(cache.Values);
                            if (nodeIndex != 0)
                            {
                                NodePool.Pool[nodeIndex >> ArrayPool.ArraySizeBit][nodeIndex & ArrayPool.ArraySizeAnd].CancelBuilder();
                                NodePool.FreeNoLock(nodeIndex);
                            }
                        }
                        finally { Monitor.Exit(NodePool.Lock); }
                    }
                }
            }
            /// <summary>
            /// 创建节点
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            private int create(StringTrieGraph.Node node)
            {
                if (node == null) return 0;
                if (cache.TryGetValue(node, out this.nodeIndex)) return this.nodeIndex;
                Node[] nodes;
                cache.Add(node, this.nodeIndex = GetNodeIndex(out nodes));
                int nodeIndex = this.nodeIndex;
                nodes[nodeIndex & ArrayPool.ArraySizeAnd].Set(create(node.Nodes), node.Value);//create(node.Link)
                return nodeIndex;
            }
            /// <summary>
            /// 创建节点集合
            /// </summary>
            /// <param name="nodes">节点集合</param>
            /// <returns></returns>
            private KeyValue<char, int>[] create(Dictionary<char, StringTrieGraph.Node> nodes)
            {
                if (nodes == null || nodes.Count == 0) return NullValue<KeyValue<char, int>>.Array;
                int index = 0;
                KeyValue<char, int>[] array = new KeyValue<char, int>[nodes.Count];
                foreach (KeyValuePair<char, StringTrieGraph.Node> value in nodes) array[index++].Set(value.Key, create(value.Value));
                if(array.Length > 1) Array.Sort(array, sortHandle);
                return array;
            }
            /// <summary>
            /// 创建失败节点
            /// </summary>
            /// <param name="node"></param>
            private void createLink(StringTrieGraph.Node node)
            {
                if (node != null)
                {
                    if (node.Link != null)
                    {
                        this.nodeIndex = cache[node];
                        NodePool.Pool[this.nodeIndex >> ArrayPool.ArraySizeBit][this.nodeIndex & ArrayPool.ArraySizeAnd].Link = cache[node.Link];
                    }
                    createLink(node.Nodes);
                }
            }
            /// <summary>
            /// 创建失败节点
            /// </summary>
            /// <param name="nodes">节点集合</param>
            private void createLink(Dictionary<char, StringTrieGraph.Node> nodes)
            {
                if (nodes != null && nodes.Count != 0)
                {
                    foreach (StringTrieGraph.Node value in nodes.Values) createLink(value);
                }
            }
            /// <summary>
            /// 节点字符排序
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            private static int sort(KeyValue<char, int> left, KeyValue<char, int> right)
            {
                return (int)left.Key - (int)right.Key;
            }
            /// <summary>
            /// 节点字符排序委托
            /// </summary>
            private static readonly Comparison<KeyValue<char, int>> sortHandle = sort;
        }
    }
}
