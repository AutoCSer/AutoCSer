using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// Trie 图
    /// </summary>
    public abstract partial class TrieGraph<keyType, valueType>
    {
        /// <summary>
        /// Trie 图节点
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        internal sealed class Node
        {
            /// <summary>
            /// 子节点
            /// </summary>
            internal Dictionary<keyType, Node> Nodes;
            /// <summary>
            /// 失败节点
            /// </summary>
            internal Node Link;
            /// <summary>
            /// 节点值
            /// </summary>
            internal valueType Value;
            /// <summary>
            /// 创建子节点
            /// </summary>
            /// <param name="letter">当前字符</param>
            /// <returns>子节点</returns>
            internal Node Create(keyType letter)
            {
                Node node;
                if (Nodes == null)
                {
                    Nodes = DictionaryCreator<keyType>.Create<Node>();
                    Nodes[letter] = node = new Node();
                }
                else if (!Nodes.TryGetValue(letter, out node))
                {
                    Nodes[letter] = node = new Node();
                }
                return node;
            }
            /// <summary>
            /// 获取子节点数量
            /// </summary>
            /// <returns>子节点数量</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal int GetNodeCount()
            {
                return Nodes == null ? 0 : Nodes.Count;
            }
            /// <summary>
            /// 设置失败节点并获取子节点数量
            /// </summary>
            /// <param name="link">失败节点</param>
            /// <returns>子节点数量</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal int GetNodeCount(Node link)
            {
                Link = link;
                return Nodes == null ? 0 : Nodes.Count;
            }
            /// <summary>
            /// 设置失败节点并获取子节点数量
            /// </summary>
            /// <param name="boot">根节点</param>
            /// <param name="letter">当前字符</param>
            /// <returns>子节点数量</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal int GetNodeCount(Dictionary<keyType, Node> boot, keyType letter)
            {
                boot.TryGetValue(letter, out Link);
                return Nodes == null ? 0 : Nodes.Count;
            }
            /// <summary>
            /// 子节点不存在时获取失败节点
            /// </summary>
            /// <param name="letter">当前字符</param>
            /// <param name="node">子节点</param>
            /// <param name="link">失败节点</param>
            /// <returns>是否成功</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal int GetLinkWhereNull(keyType letter, ref Node node, ref Node link)
            {
                if (Nodes == null || Nodes.Count == 0 || !Nodes.TryGetValue(letter, out node))
                {
                    link = Link;
                    return 0;
                }
                return 1;
            }
            /// <summary>
            /// 子节点不存在时获取失败节点
            /// </summary>
            /// <param name="letter">当前字符</param>
            /// <param name="node">子节点</param>
            /// <param name="link">失败节点</param>
            /// <param name="value">节点值</param>
            /// <returns>是否成功</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal int GetNodeOrLink(keyType letter, ref Node node, ref Node link, out valueType value)
            {
                value = Value;
                if (Nodes == null || Nodes.Count == 0 || !Nodes.TryGetValue(letter, out node))
                {
                    link = Link;
                    return 0;
                }
                return 1;
            }
            /// <summary>
            /// 获取失败节点
            /// </summary>
            /// <param name="link">失败节点</param>
            /// <returns>节点值</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal valueType GetLink(ref Node link)
            {
                link = Link;
                return Value;
            }
        }
    }
}
