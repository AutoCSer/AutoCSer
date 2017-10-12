using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定静态节点池的 Trie 图
    /// </summary>
    public abstract partial class StaticTrieGraph<keyType, valueType>
    {
        /// <summary>
        /// Trie 图节点
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        internal struct Node
        {
            /// <summary>
            /// 子节点
            /// </summary>
            internal KeyValue<keyType, int>[] Nodes;
            /// <summary>
            /// 节点值
            /// </summary>
            public valueType Value;
            /// <summary>
            /// 失败节点
            /// </summary>
            internal int Link;
            /// <summary>
            /// 设置节点数据
            /// </summary>
            /// <param name="nodes"></param>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Set(KeyValue<keyType, int>[] nodes, valueType value)
            {
                Nodes = nodes;
                Value = value;
                Link = 0;
            }
            /// <summary>
            /// 初始化重置数据
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Reset()
            {
                Nodes = NullValue<KeyValue<keyType, int>>.Array;
                Link = 0;
            }
            /// <summary>
            /// 创建错误取消节点
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void CancelBuilder()
            {
                Nodes = NullValue<KeyValue<keyType, int>>.Array;
                Value = default(valueType);
            }
            /// <summary>
            /// 释放节点
            /// </summary>
            internal void Free()
            {
                if (Nodes.Length != 0)
                {
                    foreach (KeyValue<keyType, int> node in Nodes) NodePool.Pool[node.Value >> ArrayPool.ArraySizeBit][node.Value & ArrayPool.ArraySizeAnd].Free();
                    NodePool.FreeNoLock(Nodes);
                }
                CancelBuilder();
            }
            /// <summary>
            /// 二分查找子节点
            /// </summary>
            /// <param name="letter"></param>
            /// <returns></returns>
            internal int GetNode(keyType letter)
            {
                int start = 0, length = Nodes.Length, average;
                do
                {
                    if (letter.CompareTo(Nodes[average = start + ((length - start) >> 1)].Key) > 0) start = average + 1;
                    else length = average;
                }
                while (start != length);
                return start != Nodes.Length && letter.CompareTo(Nodes[start].Key) == 0 ? Nodes[start].Value : 0;
            }
            /// <summary>
            /// 子节点不存在时获取失败节点
            /// </summary>
            /// <param name="letter">当前字符</param>
            /// <param name="node">子节点</param>
            /// <param name="link">失败节点</param>
            /// <returns>是否成功</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal int GetLinkWhereNull(keyType letter, ref int node, ref int link)
            {
                if (Nodes.Length == 0 || (node = GetNode(letter)) == 0)
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
            internal int GetNodeOrLink(keyType letter, ref int node, ref int link, out valueType value)
            {
                value = Value;
                if (Nodes.Length == 0 || (node = GetNode(letter)) == 0)
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
            internal valueType GetLink(ref int link)
            {
                link = Link;
                return Value;
            }
        }
    }
}
