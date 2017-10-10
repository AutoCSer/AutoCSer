using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 字符串 Trie 图
    /// </summary>
    public sealed partial class StringTrieGraph
    {
        /// <summary>
        /// 树创建器
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        internal unsafe struct TreeBuilder
        {
            /// <summary>
            /// 结束字符
            /// </summary>
            private char* end;
            /// <summary>
            /// 当前节点
            /// </summary>
            private Node node;
            /// <summary>
            /// 创建树
            /// </summary>
            /// <param name="boot"></param>
            /// <param name="word">分词</param>
            /// <param name="start"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Build(Node boot, string word, char* start)
            {
                node = boot;
                end = start + word.Length;
                build(start);
                node.Value = word;
            }
            /// <summary>
            /// 创建树
            /// </summary>
            /// <param name="start">当前字符位置</param>
            private void build(char* start)
            {
                node = node.Create(*start);
                if (++start != end) build(start);
            }
        }
    }
}
