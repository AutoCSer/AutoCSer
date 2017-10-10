using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// Trie 图
    /// </summary>
    public abstract partial class TrieGraph<keyType, valueType>
    {
        /// <summary>
        /// Trie 图创建器
        /// </summary>
        internal class Builder
        {
            /// <summary>
            /// 根节点
            /// </summary>
            private Dictionary<keyType, Node> boot;
            /// <summary>
            /// 当前处理结果节点集合
            /// </summary>
            public LeftArray<Node> Writer;
            /// <summary>
            /// 当前处理节点集合
            /// </summary>
            public Node[] Reader;
            /// <summary>
            /// 处理节点起始索引位置
            /// </summary>
            public int StartIndex;
            /// <summary>
            /// 处理节点数量
            /// </summary>
            public int Count;
            /// <summary>
            /// Trie 图创建器
            /// </summary>
            /// <param name="boot">根节点</param>
            public Builder(Node boot)
            {
                this.boot = boot.Nodes;
            }
            /// <summary>
            /// 设置当前处理节点集合
            /// </summary>
            /// <param name="reader">当前处理节点集合</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Set(ref LeftArray<Node> reader)
            {
                this.Reader = reader.Array;
                StartIndex = 0;
                Count = reader.Length;
            }
            /// <summary>
            /// 建图
            /// </summary>
            public unsafe void Build()
            {
                Writer.Length = 0;
                int endIndex = StartIndex + Count;
                while (StartIndex != endIndex)
                {
                    Node fatherNode = Reader[StartIndex++];
                    if (fatherNode.Link == null)
                    {
                        foreach (KeyValuePair<keyType, Node> nextNode in fatherNode.Nodes)
                        {
                            if (nextNode.Value.GetNodeCount(boot, nextNode.Key) != 0) Writer.Add(nextNode.Value);
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<keyType, Node> nextNode in fatherNode.Nodes)
                        {
                            Node link = fatherNode.Link, linkNode = null;
                            do
                            {
                                if (link.GetLinkWhereNull(nextNode.Key, ref linkNode, ref link) == 0)
                                {
                                    if (link == null)
                                    {
                                        if ((boot.TryGetValue(nextNode.Key, out linkNode) ? nextNode.Value.GetNodeCount(linkNode) : nextNode.Value.GetNodeCount()) != 0) Writer.Add(nextNode.Value);
                                        break;
                                    }
                                }
                                else
                                {
                                    if (nextNode.Value.GetNodeCount(linkNode) != 0) Writer.Add(nextNode.Value);
                                    break;
                                }
                            }
                            while (true);
                        }
                    }
                }
            }
        }
    }
}
