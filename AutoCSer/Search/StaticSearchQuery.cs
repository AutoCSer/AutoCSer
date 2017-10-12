using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定结果池的分词搜索器
    /// </summary>
    public sealed partial class StaticSearcher<keyType>
    {
        /// <summary>
        /// 分词查询
        /// </summary>
        internal unsafe sealed class WordQuery : WordSegmenterBase, IDisposable
        {
            /// <summary>
            /// 绑定结果池的分词搜索器
            /// </summary>
            private new readonly StaticSearcher<keyType> searcher;
            /// <summary>
            /// 分词结果
            /// </summary>
            private LeftArray<KeyValue<HashString, QueryResult>> result;
            /// <summary>
            /// 分词结果
            /// </summary>
            private QueryResult queryResult;
            /// <summary>
            /// 初始化添加数据
            /// </summary>
            /// <param name="searcher">绑定结果池的分词搜索器</param>
            internal WordQuery(StaticSearcher<keyType> searcher) : base(searcher)
            {
                this.searcher = searcher;
            }
            /// <summary>
            /// 获取文本分词结果
            /// </summary>
            /// <param name="text"></param>
            /// <param name="result">分词结果</param>
            internal void Get(Simplified text, ref LeftArray<KeyValue<HashString, QueryResult>> result)
            {
                this.result = result;
                foramtLength = (formatText = text.FormatText).Length - 1;
                fixed (char* textFixed = formatText) get(textFixed);
                result = this.result;
            }
            /// <summary>
            /// 获取文本分词结果
            /// </summary>
            /// <param name="text"></param>
            /// <param name="result">分词结果</param>
            internal void Get(string text, ref LeftArray<KeyValue<HashString, QueryResult>> result)
            {
                this.result = result;
                formatText = AutoCSer.Extension.StringExtension.FastAllocateString((foramtLength = text.Length) + 1);
                fixed (char* textFixed = formatText)
                {
                    Simplified.FormatNotEmpty(text, textFixed, foramtLength);
                    get(textFixed);
                }
                result = this.result;
            }
            /// <summary>
            /// 获取文本分词结果
            /// </summary>
            /// <param name="textFixed"></param>
            private void get(char* textFixed)
            {
                char* start = textFixed, end = textFixed + foramtLength;
                try
                {
                    matchs.Length = 0;
                    byte type, nextType;
                    bool isMatchMap = false;
                    if (charTypeData != StringTrieGraph.DefaultCharTypeData.Byte)
                    {
                        StaticStringTrieGraph trieGraph = searcher.trieGraph;
                        int count, index, startIndex;
                        char trieGraphHeadChar = trieGraph.AnyHeadChar;
                        do
                        {
                            if (((type = charTypeData[*start]) & StringTrieGraph.TrieGraphHeadFlag) == 0)
                            {
                                *end = trieGraphHeadChar;
                                do
                                {
                                    if ((type & ((byte)WordType.Chinese | (byte)WordType.TrieGraph)) == ((byte)WordType.Chinese | (byte)WordType.TrieGraph)) addWord((int)(start - textFixed), 1);
                                    if (((nextType = charTypeData[*++start]) & StringTrieGraph.TrieGraphHeadFlag) != 0)
                                    {
                                        if (start == end) goto TRIEGRAPHEND;
                                        if ((nextType & (byte)WordType.Chinese) != 0
                                            || (type & nextType & ((byte)WordType.OtherLetter | (byte)WordType.Letter | (byte)WordType.Number | (byte)WordType.Keep)) == 0) goto TRIEGRAPH;
                                    }
                                    type = nextType;
                                }
                                while (true);
                            }
                        TRIEGRAPH:
                            *end = ' ';
                            char* segment = start, segmentEnd = (type & StringTrieGraph.TrieGraphEndFlag) == 0 ? start++ : ++start;
                            while (((type = charTypeData[*start]) & (byte)WordType.TrieGraph) != 0)
                            {
                                ++start;
                                if ((type & StringTrieGraph.TrieGraphEndFlag) != 0) segmentEnd = start;
                            }
                            if ((int)(start - segment) == 1)
                            {
                                if ((type & (byte)WordType.Chinese) != 0) addWord((int)(segment - textFixed), 1);
                            }
                            else
                            {
                                if (segment != segmentEnd)
                                {
                                    matchs.Length = 0;
                                    trieGraph.LeftRightMatchs(segment, segmentEnd, ref matchs);
                                    if ((count = matchs.Length) == 0)
                                    {
                                        segmentEnd = segment;
                                        goto CHINESE;
                                    }
                                    if (!isMatchMap)
                                    {
                                        checkMatchMap();
                                        isMatchMap = true;
                                    }
                                    startIndex = (int)(segment - textFixed);
                                    foreach (KeyValue<int, int> value in matchs.Array)
                                    {
                                        addWord(index = value.Key + startIndex, value.Value);
                                        matchMap.Set(index, value.Value);
                                        if (--count == 0) break;
                                    }
                                    index = (int)(segmentEnd - textFixed);
                                    do
                                    {
                                        if (matchMap.Get(startIndex) == 0 && (charTypeData[textFixed[startIndex]] & (byte)WordType.Chinese) != 0) addWord(startIndex, 1);
                                    }
                                    while (++startIndex != index);
                                }
                            CHINESE:
                                while (segmentEnd != start)
                                {
                                    if ((charTypeData[*segmentEnd] & (byte)WordType.Chinese) != 0) addWord((int)(segmentEnd - textFixed), 1);
                                    ++segmentEnd;
                                }
                            }
                        }
                        while (start != end);
                    TRIEGRAPHEND:
                        start = textFixed;
                    }
                    do
                    {
                        type = charTypeData[*start];
                        if ((type &= ((byte)WordType.Chinese | (byte)WordType.OtherLetter | (byte)WordType.Letter | (byte)WordType.Number | (byte)WordType.Keep)) == 0)
                        {
                            *end = '0';
                            do
                            {
                                type = charTypeData[*++start];
                                if ((type &= ((byte)WordType.Chinese | (byte)WordType.OtherLetter | (byte)WordType.Letter | (byte)WordType.Number | (byte)WordType.Keep)) != 0)
                                {
                                    if (start == end) return;
                                    goto OTHER;
                                }
                            }
                            while (true);
                        }
                    OTHER:
                        *end = ' ';
                        if ((type & (byte)WordType.Chinese) != 0)
                        {
                            do
                            {
                                if ((type & (byte)WordType.TrieGraph) == 0) addWord((int)(start - textFixed), 1);
                            }
                            while (((type = charTypeData[*++start]) & (byte)WordType.Chinese) != 0);
                        }
                        else
                        {
                            char* segment = start;
                            if ((type & (byte)WordType.OtherLetter) == 0)
                            {
                                char* word = start;
                                for (nextType = charTypeData[*++start]; (nextType &= ((byte)WordType.Letter | (byte)WordType.Number | (byte)WordType.Keep)) != 0; nextType = charTypeData[*++start])
                                {
                                    if (type != nextType)
                                    {
                                        if (type != (byte)WordType.Keep) addWord((int)(word - textFixed), (int)(start - word));
                                        type = nextType;
                                        word = start;
                                    }
                                }
                                if (word != segment && type != (byte)WordType.Keep) addWord((int)(word - textFixed), (int)(start - word));
                                addWord((int)(segment - textFixed), (int)(start - segment));
                            }
                            else
                            {
                                while ((charTypeData[*++start] & (byte)WordType.OtherLetter) != 0) ;
                                addWord((int)(segment - textFixed), (int)(start - segment));
                            }
                        }
                    }
                    while (start != end);
                }
                finally { *end = ' '; }
            }
            /// <summary>
            /// 添加分词
            /// </summary>
            /// <param name="start"></param>
            /// <param name="length"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            private void addWord(int start, int length)
            {
                HashString word = new SubString(start, length, formatText);
                if (searcher.results.GetResult(ref word, ref queryResult))
                {
                    result.PrepLength(1);
                    result.Array[result.Length++].Set(word, queryResult);
                }
            }
        }
    }
}
