using System;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定结果池的分词搜索器
    /// </summary>
    public unsafe abstract partial class StaticSearcher
    {
        /// <summary>
        /// 数据处理
        /// </summary>
        public abstract class WordSegmenterBase
        {
            /// <summary>
            /// 绑定结果池的分词搜索器
            /// </summary>
            protected readonly StaticSearcher searcher;
            /// <summary>
            /// 分词字符类型集合
            /// </summary>
            protected readonly byte* charTypeData;
            /// <summary>
            /// 匹配位置位图
            /// </summary>
            protected Pointer.Size matchMapData;
            /// <summary>
            /// 分词索引位置
            /// </summary>
            protected LeftArray<KeyValue<int, int>> matchs;
            /// <summary>
            /// 格式化文本
            /// </summary>
            protected string formatText;
            /// <summary>
            /// 格式化文本长度
            /// </summary>
            protected int formatLength;
            /// <summary>
            /// 匹配位置位图
            /// </summary>
            protected MemoryMap matchMap;
            /// <summary>
            /// 初始化添加数据
            /// </summary>
            /// <param name="searcher">搜索器</param>
            protected WordSegmenterBase(StaticSearcher searcher)
            {
                this.searcher = searcher;
                charTypeData = searcher.trieGraph.CharTypeData.Byte;
            }
            /// <summary>
            ///  析构释放资源
            /// </summary>
            ~WordSegmenterBase()
            {
                Unmanaged.Free(ref matchMapData);
            }
            /// <summary>
            /// 释放资源
            /// </summary>
            public void Dispose()
            {
                Unmanaged.Free(ref matchMapData);
            }
            /// <summary>
            /// 检测匹配位置位图
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            protected void checkMatchMap()
            {
                int matchMapSize = (formatLength + 7) >> 3, matchMapDataSize = Math.Max((int)((uint)matchMapSize).UpToPower2(), 8);
                if (matchMapData.ByteSize < matchMapDataSize)
                {
                    Unmanaged.Free(ref matchMapData);
                    matchMapData = Unmanaged.GetSizeUnsafe64(matchMapDataSize, false);
                }
                matchMap.Set(matchMapData.ULong, (matchMapSize + 7) >> 3);
            }
        }
        /// <summary>
        /// 数据分词处理
        /// </summary>
        public abstract class WordSegmenter : WordSegmenterBase
        {
            /// <summary>
            /// 结果缓冲区
            /// </summary>
            protected readonly ReusableDictionary<HashString, ResultIndexLeftArray> result = ReusableDictionary.CreateHashString<ResultIndexLeftArray>();
            /// <summary>
            /// 匹配位置结果缓冲区
            /// </summary>
            protected LeftArray<int[]> indexArrays;
            /// <summary>
            /// 分词结果
            /// </summary>
            private LeftArray<KeyValue<SubString, WordType>> words;
            /// <summary>
            /// 数据分词处理
            /// </summary>
            /// <param name="searcher">搜索器</param>
            protected WordSegmenter(StaticSearcher searcher) : base(searcher) { }
            /// <summary>
            /// 获取文本分词结果
            /// </summary>
            /// <param name="text"></param>
            protected void getResult(string text)
            {
                result.Empty();
                formatLength = text.Length;
                formatText = AutoCSer.Extension.StringExtension.FastAllocateString(formatLength + 1);
                fixed (char* textFixed = formatText)
                {
                    Simplified.FormatNotEmpty(text, textFixed, formatLength);
                    words.Length = matchs.Length = 0;
                    char* start = textFixed, end = textFixed + formatLength;
                    byte type, nextType, wordType;
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
                                    if ((type & ((byte)WordType.Chinese | (byte)WordType.TrieGraph)) == ((byte)WordType.Chinese | (byte)WordType.TrieGraph)) addWord((int)(start - textFixed), 1, WordType.Chinese);
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
                                if ((type & (byte)WordType.Chinese) != 0) addWord((int)(segment - textFixed), 1, (type & (byte)WordType.TrieGraph) != 0 ? WordType.TrieGraph : WordType.Chinese);
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
                                        addWord(index = value.Key + startIndex, value.Value, WordType.TrieGraph);
                                        matchMap.Set(index, value.Value);
                                        if (--count == 0) break;
                                    }
                                    index = (int)(segmentEnd - textFixed);
                                    do
                                    {
                                        if (matchMap.Get(startIndex) == 0 && (charTypeData[textFixed[startIndex]] & (byte)WordType.Chinese) != 0) addWord(startIndex, 1, WordType.Chinese);
                                    }
                                    while (++startIndex != index);
                                }
                                CHINESE:
                                while (segmentEnd != start)
                                {
                                    if ((charTypeData[*segmentEnd] & (byte)WordType.Chinese) != 0) addWord((int)(segmentEnd - textFixed), 1, WordType.Chinese);
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
                                    if (start == end) goto END;
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
                                if ((type & (byte)WordType.TrieGraph) == 0) addWord((int)(start - textFixed), 1, WordType.Chinese);
                            }
                            while (((type = charTypeData[*++start]) & (byte)WordType.Chinese) != 0);
                        }
                        else
                        {
                            char* segment = start;
                            if ((type & (byte)WordType.OtherLetter) == 0)
                            {
                                char* word = start;
                                wordType = type;
                                for (nextType = charTypeData[*++start]; (nextType &= ((byte)WordType.Letter | (byte)WordType.Number | (byte)WordType.Keep)) != 0; nextType = charTypeData[*++start])
                                {
                                    if (type != nextType)
                                    {
                                        if (type != (byte)WordType.Keep) addWord((int)(word - textFixed), (int)(start - word), (WordType)type);
                                        wordType |= nextType;
                                        type = nextType;
                                        word = start;
                                    }
                                }
                                if (word != segment && type != (byte)WordType.Keep) addWord((int)(word - textFixed), (int)(start - word), (WordType)type);
                                addWord((int)(segment - textFixed), (int)(start - segment), (WordType)wordType);
                            }
                            else
                            {
                                while ((charTypeData[*++start] & (byte)WordType.OtherLetter) != 0) ;
                                addWord((int)(segment - textFixed), (int)(start - segment), WordType.OtherLetter);
                            }
                        }
                    }
                    while (start != end);
                    END:
                    if (words.Length != 0)
                    {
                        int count = words.Length, textLength = text.Length;
                        if ((searcher.flags & SearchFlags.ResultIndexs) == 0)
                        {
                            foreach (KeyValue<SubString, WordType> word in words.Array)
                            {
                                result.Set(word.Key, new ResultIndexLeftArray { WordType = word.Value, TextLength = textLength });
                                if (--count == 0) break;
                            }
                        }
                        else
                        {
                            ResultIndexLeftArray indexs;
                            foreach (KeyValue<SubString, WordType> word in words.Array)
                            {
                                HashString wordKey = word.Key;
                                if (result.TryGetValue(ref wordKey, out indexs))
                                {
                                    indexs.Indexs.Add(word.Key.Start);
                                    result.Set(ref wordKey, indexs);
                                }
                                else
                                {
                                    indexs.Set(textLength, word.Value);
                                    if (indexArrays.Length != 0) indexs.Indexs.Set(indexArrays.UnsafePopOnly(), 0);
                                    indexs.Indexs.Add(word.Key.Start);
                                    result.Set(ref wordKey, indexs);
                                }
                                if (--count == 0) break;
                            }
                            foreach (ResultIndexLeftArray indexArray in result.Values) indexArray.Indexs.Sort();
                        }
                    }
                }
            }
            /// <summary>
            /// 添加分词
            /// </summary>
            /// <param name="start"></param>
            /// <param name="length"></param>
            /// <param name="wordType"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            private void addWord(int start, int length, WordType wordType)
            {
                words.PrepLength(1);
                words.Array[words.Length++].Set(new SubString(start, length, formatText), wordType);
            }
        }
    }
}
