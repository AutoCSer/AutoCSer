using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 字符串 Trie 图
    /// </summary>
    public unsafe sealed partial class StringTrieGraph : TrieGraph<char, string>, IDisposable
    {
        /// <summary>
        /// Trie 图首字符
        /// </summary>
        internal const byte TrieGraphHeadFlag = 0x40;
        /// <summary>
        /// Trie 图首字符
        /// </summary>
        internal const byte TrieGraphEndFlag = 0x80;
        /// <summary>
        /// 分词字符类型数据
        /// </summary>
        internal Pointer.Size CharTypeData;
        /// <summary>
        /// 任意字符，用于搜索哨岗
        /// </summary>
        internal char AnyHeadChar;
        /// <summary>
        /// 字符串 Trie 图
        /// </summary>
        /// <param name="words">分词集合</param>
        /// <param name="threadCount">并行线程数量</param>
        /// <param name="log">日志处理</param>
        public StringTrieGraph(ref LeftArray<string> words, int threadCount = 0, AutoCSer.Log.ILog log = null)
        {
            CharTypeData = new Pointer.Size { Data = DefaultCharTypeData.Data };
            if (words.Length != 0)
            {
                buildTree(ref words);
                BuildGraph(threadCount, log);
            }
            if (Boot.Nodes == null) Boot.Nodes = DictionaryCreator.CreateChar<Node>();
            else
            {
                foreach (char key in Boot.Nodes.Keys)
                {
                    AnyHeadChar = key;
                    break;
                }
            }
        }
        /// <summary>
        /// 字符串 Trie 图
        /// </summary>
        /// <param name="words">分词集合</param>
        /// <param name="threadCount">并行线程数量</param>
        /// <param name="log">日志处理</param>
        public StringTrieGraph(LeftArray<string> words, int threadCount = 0, AutoCSer.Log.ILog log = null) : this(ref words, threadCount, log) { }
        /// <summary>
        /// 字符串 Trie 图
        /// </summary>
        /// <param name="words">分词集合</param>
        /// <param name="threadCount">并行线程数量</param>
        /// <param name="log">日志处理</param>
        public StringTrieGraph(string[] words, int threadCount = 0, AutoCSer.Log.ILog log = null) : this(new LeftArray<string>(words), threadCount, log) { }
        /// <summary>
        ///  析构释放资源
        /// </summary>
        ~StringTrieGraph()
        {
            if (CharTypeData.Data != DefaultCharTypeData.Data) Unmanaged.Free(ref CharTypeData);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (CharTypeData.Data != DefaultCharTypeData.Data)
            {
                Unmanaged.Free(ref CharTypeData);
                CharTypeData = new Pointer.Size { Data = DefaultCharTypeData.Data };
            }
        }
        /// <summary>
        /// 创建 Trie 树
        /// </summary>
        /// <param name="words">分词集合</param>
        private void buildTree(ref LeftArray<string> words)
        {
            if (words.Length != 0)
            {
                TreeBuilder treeBuilder = new TreeBuilder();
                foreach (string word in words)
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        if (CharTypeData.Data == DefaultCharTypeData.Data)
                        {
                            AutoCSer.Memory.CopyNotNull(DefaultCharTypeData.Byte, (CharTypeData = Unmanaged.GetSizeUnsafe64(1 << 16, false)).Byte, 1 << 16);
                        }
                        fixed (char* wordFixed = word)
                        {
                            treeBuilder.Build(Boot, word, wordFixed);
                            CharTypeData.Byte[*wordFixed] |= TrieGraphHeadFlag;
                            char* start = wordFixed, end = wordFixed + word.Length;
                            do
                            {
                                if (*start != ' ') CharTypeData.Byte[*start] |= (byte)WordType.TrieGraph;
                            }
                            while (++start != end);
                            CharTypeData.Byte[*(end - 1)] |= TrieGraphEndFlag;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 创建绑定静态节点池的字符串 Trie 图
        /// </summary>
        /// <param name="isCopyCharTypeData">是否复制分词字符类型数据，否则释放分词字符类型数据</param>
        /// <returns>绑定静态节点池的字符串 Trie 图</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public StaticStringTrieGraph CreateStaticGraph(bool isCopyCharTypeData = true)
        {
            StaticStringTrieGraph graph = new StaticStringTrieGraph();
            graph.Create(this, isCopyCharTypeData);
            return graph;
        }
        /// <summary>
        /// 从左到右最大匹配
        /// </summary>
        /// <param name="start">匹配起始位置</param>
        /// <param name="end">匹配结束位置</param>
        /// <param name="matchs">匹配结果集合</param>
        private void leftRightMatchs(char* start, char* end, ref LeftArray<KeyValue<int, int>> matchs)
        {
            Dictionary<char, Node> bootNode = Boot.Nodes;
            string value;
            Node node = Boot, linkNode = null;
            int index = -1;
            do
            {
                char letter = *start;
                if (node.GetLinkWhereNull(letter, ref node, ref linkNode) == 0)
                {
                    if (linkNode != null) goto LINK;
                    BOOT:
                    ++index;
                    node = Boot;
                    continue;
                    LINK:
                    do
                    {
                        int isGetValue = linkNode.GetNodeOrLink(letter, ref node, ref linkNode, out value);
                        if (value != null)
                        {
                            matchs.PrepLength(1);
                            matchs.Array[matchs.Length++].Set(index - value.Length + 1, value.Length);
                        }
                        if (isGetValue == 0)
                        {
                            if (linkNode == null) goto BOOT;
                        }
                        else break;
                    }
                    while (true);
                }
                ++index;
                if ((value = node.Value) != null)
                {
                    matchs.PrepLength(1);
                    matchs.Array[matchs.Length++].Set(index - value.Length + 1, value.Length);
                }
            }
            while (++start != end) ;
            node = node.Link;
            while (node != null)
            {
                if ((value = node.GetLink(ref node)) != null)
                {
                    matchs.PrepLength(1);
                    matchs.Array[matchs.Length++].Set(index - value.Length + 1, value.Length);
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
                fixed (char* valueFixed = text) leftRightMatchs(valueFixed, valueFixed + text.Length, ref matchs);
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
                    leftRightMatchs(start, start + text.Length, ref matchs);
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
        /// 默认分词字符类型数据
        /// </summary>
        internal static Pointer DefaultCharTypeData;
        static StringTrieGraph()
        {
            DefaultCharTypeData = new Pointer { Data = Unmanaged.GetStatic64(1 << 16, true) };
            byte* start = DefaultCharTypeData.Byte, end = DefaultCharTypeData.Byte + (1 << 16);
            for (char code = (char)0; start != end; ++start, ++code)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(code);
                switch (category)
                {
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.TitlecaseLetter:
                    case UnicodeCategory.ModifierLetter:
                        *start = (byte)WordType.Letter;
                        break;
                    case UnicodeCategory.DecimalDigitNumber:
                    case UnicodeCategory.LetterNumber:
                    case UnicodeCategory.OtherNumber:
                        *start = (byte)WordType.Number;
                        break;
                    case UnicodeCategory.OtherLetter: *start = (byte)WordType.OtherLetter; break;
                    default:
                        if (code == '&' || code == '.' || code == '+' || code == '#' || code == '-' || code == '_') *start = (byte)WordType.Keep;
                        break;
                }
            }
            Simplified.SetChinese(DefaultCharTypeData.Byte);
            DefaultCharTypeData.Byte[' '] = 0;
            DefaultCharTypeData.Byte['0'] |= (byte)WordType.Number;
        }
    }
}
