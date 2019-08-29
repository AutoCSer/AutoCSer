using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定静态节点池的字符串 Trie 图
    /// </summary>
    public unsafe sealed partial class StaticStringTrieGraph : StaticTrieGraph<char, string>
    {
        /// <summary>
        /// 分词字符类型数据
        /// </summary>
        internal Pointer.Size CharTypeData;
        /// <summary>
        /// 字符分词类型
        /// </summary>
        public CharWordType CharWordType
        {
            get { return new CharWordType { Data = CharTypeData.Byte }; }
        }
        /// <summary>
        /// 任意字符，用于搜索哨岗
        /// </summary>
        internal char AnyHeadChar;
        /// <summary>
        /// 绑定静态节点池的字符串 Trie 图
        /// </summary>
        internal StaticStringTrieGraph()
        {
            CharTypeData = new Pointer.Size { Data = StringTrieGraph.DefaultCharTypeData.Data };
        }
        /// <summary>
        ///  析构释放资源
        /// </summary>
        ~StaticStringTrieGraph()
        {
            if (CharTypeData.Data != StringTrieGraph.DefaultCharTypeData.Data) Unmanaged.Free(ref CharTypeData);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (CharTypeData.Data != StringTrieGraph.DefaultCharTypeData.Data)
            {
                Unmanaged.Free(ref CharTypeData);
                CharTypeData = new Pointer.Size { Data = StringTrieGraph.DefaultCharTypeData.Data };
            }
        }
        /// <summary>
        /// 设置根节点
        /// </summary>
        /// <param name="boot"></param>
        private void setBoot(int boot)
        {
            this.boot = boot;
            nodes = (int*)Unmanaged.Get64((1 << 16) * sizeof(int), true);
            foreach (KeyValue<char, int> node in NodePool.Pool[boot >> ArrayPool.ArraySizeBit][boot & ArrayPool.ArraySizeAnd].Nodes) nodes[node.Key] = node.Value;
        }
        /// <summary>
        /// 创建 Trie 图
        /// </summary>
        /// <param name="trieGraph">字符串 Trie 图</param>
        /// <param name="isCopyCharTypeData">是否复制分词字符类型数据</param>
        internal void Create(StringTrieGraph trieGraph, bool isCopyCharTypeData)
        {
            if (trieGraph.Boot.Nodes.count() != 0) new Builder().Create(this, trieGraph);
            if (trieGraph.CharTypeData.Data != CharTypeData.Data && CharTypeData.Data != null)
            {
                if (isCopyCharTypeData)
                {
                    AutoCSer.Memory.CopyNotNull(trieGraph.CharTypeData.Byte, (CharTypeData = Unmanaged.GetSizeUnsafe64(1 << 16, false)).Byte, 1 << 16);
                }
                else
                {
                    CharTypeData = trieGraph.CharTypeData;
                    trieGraph.CharTypeData = new Pointer.Size { Data = StringTrieGraph.DefaultCharTypeData.Data };
                }
            }
            AnyHeadChar = trieGraph.AnyHeadChar;
        }
        /// <summary>
        /// 从左到右最大匹配
        /// </summary>
        /// <param name="start">匹配起始位置</param>
        /// <param name="end">匹配结束位置</param>
        /// <param name="matchs">匹配结果集合</param>
        internal void LeftRightMatchs(char* start, char* end, ref LeftArray<KeyValue<int, int>> matchs)
        {
            Node[][] pool = NodePool.Pool;
            if (nodes != null)
            {
                string value;
                int node = boot, linkNode = 0, index = -1;
                do
                {
                    char letter = *start;
                    if (node != boot) goto NEXT;
                BOOT:
                    if ((node = nodes[letter]) == 0)
                    {
                        ++index;
                        node = boot;
                        continue;
                    }
                    goto MATCH;
                NEXT:
                    if (pool[node >> ArrayPool.ArraySizeBit][node & ArrayPool.ArraySizeAnd].GetLinkWhereNull(letter, ref node, ref linkNode) == 0)
                    {
                        if (linkNode == 0) goto BOOT;
                        do
                        {
                            int isGetValue = pool[linkNode >> ArrayPool.ArraySizeBit][linkNode & ArrayPool.ArraySizeAnd].GetNodeOrLink(letter, ref node, ref linkNode, out value);
                            if (value != null)
                            {
                                matchs.PrepLength(1);
                                matchs.Array[matchs.Length++].Set(index - value.Length + 1, value.Length);
                            }
                            if (isGetValue == 0)
                            {
                                if (linkNode == 0) goto BOOT;
                            }
                            else break;
                        }
                        while (true);
                    }
                MATCH:
                    ++index;
                    if ((value = pool[node >> ArrayPool.ArraySizeBit][node & ArrayPool.ArraySizeAnd].Value) != null)
                    {
                        matchs.PrepLength(1);
                        matchs.Array[matchs.Length++].Set(index - value.Length + 1, value.Length);
                    }
                }
                while (++start != end);
                if (node != boot)
                {
                    node = pool[node >> ArrayPool.ArraySizeBit][node & ArrayPool.ArraySizeAnd].Link;
                    while (node != 0)
                    {
                        if ((value = pool[node >> ArrayPool.ArraySizeBit][node & ArrayPool.ArraySizeAnd].GetLink(ref node)) != null)
                        {
                            matchs.PrepLength(1);
                            matchs.Array[matchs.Length++].Set(index - value.Length + 1, value.Length);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 从左到右最大匹配
        /// </summary>
        /// <param name="text">匹配文本</param>
        /// <param name="matchs">匹配结果集合 [匹配字符起始位置,匹配字符数量]</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void LeftRightMatchs(string text, ref LeftArray<KeyValue<int, int>> matchs)
        {
            if (text.Length != 0)
            {
                fixed (char* valueFixed = text) LeftRightMatchs(valueFixed, valueFixed + text.Length, ref matchs);
            }
        }
        /// <summary>
        /// 从左到右最大匹配
        /// </summary>
        /// <param name="text">匹配文本</param>
        /// <param name="matchs">匹配结果集合 [匹配字符起始位置,匹配字符数量]</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void LeftRightMatchs(ref SubString text, ref LeftArray<KeyValue<int, int>> matchs)
        {
            if (text.Length != 0)
            {
                fixed (char* valueFixed = text.String)
                {
                    char* start = valueFixed + text.Start;
                    LeftRightMatchs(start, start + text.Length, ref matchs);
                }
            }
        }
        /// <summary>
        /// 从左到右最大匹配
        /// </summary>
        /// <param name="text">匹配文本</param>
        /// <param name="matchs">匹配结果集合 [匹配字符起始位置,匹配字符数量]</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void LeftRightMatchs(SubString text, ref LeftArray<KeyValue<int, int>> matchs)
        {
            LeftRightMatchs(ref text, ref matchs);
        }
        /// <summary>
        /// 从左到右最大匹配
        /// </summary>
        /// <param name="text">匹配文本</param>
        /// <returns>匹配结果集合 [匹配字符起始位置,匹配字符数量]</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public LeftArray<KeyValue<int, int>> LeftRightMatchs(string text)
        {
            LeftArray<KeyValue<int, int>> matchs = default(LeftArray<KeyValue<int, int>>);
            LeftRightMatchs(text, ref matchs);
            return matchs;
        }
        /// <summary>
        /// 从左到右最大匹配
        /// </summary>
        /// <param name="text">匹配文本</param>
        /// <returns>匹配结果集合 [匹配字符起始位置,匹配字符数量]</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public LeftArray<KeyValue<int, int>> LeftRightMatchs(ref SubString text)
        {
            LeftArray<KeyValue<int, int>> matchs = default(LeftArray<KeyValue<int, int>>);
            LeftRightMatchs(ref text, ref matchs);
            return matchs;
        }
        /// <summary>
        /// 从左到右最大匹配
        /// </summary>
        /// <param name="text">匹配文本</param>
        /// <returns>匹配结果集合 [匹配字符起始位置,匹配字符数量]</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public LeftArray<KeyValue<int, int>> LeftRightMatchs(SubString text)
        {
            LeftArray<KeyValue<int, int>> matchs = default(LeftArray<KeyValue<int, int>>);
            LeftRightMatchs(ref text, ref matchs);
            return matchs;
        }
        /// <summary>
        /// 空 Trie 图
        /// </summary>
        public static readonly StaticStringTrieGraph Null = new StaticStringTrieGraph();
    }
}
