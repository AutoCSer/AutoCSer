using System;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定结果池的分词搜索器
    /// </summary>
    public sealed partial class StaticSearcher<keyType>
    {
        /// <summary>
        /// 数据更新队列
        /// </summary>
        internal sealed unsafe class Queue : WordSegmenter, IDisposable
        {
            /// <summary>
            /// 绑定结果池的分词搜索器
            /// </summary>
            private new readonly StaticSearcher<keyType> searcher;
            /// <summary>
            /// 结果缓冲区
            /// </summary>
            private readonly HashSet<HashString> removeResult = HashSetCreator.CreateHashString();
            /// <summary>
            /// 初始化添加数据
            /// </summary>
            /// <param name="searcher">绑定结果池的分词搜索器</param>
            internal Queue(StaticSearcher<keyType> searcher)
                : base(searcher)
            {
                this.searcher = searcher;
            }
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="data">更新数据</param>
            internal void Add(SearchData data)
            {
                do
                {
                    try
                    {
                        do
                        {
                            if (data.IsRemove) Remove(data);
                            else Add(ref data.Key, data.Text);
                        }
                        while (searcher.isDisposed == 0 && (data = data.LinkNext) != null);
                        return;
                    }
                    catch (Exception error)
                    {
                        AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
                    }
                }
                while (searcher.isDisposed == 0 && (data = data.LinkNext) != null);
            }
            /// <summary>
            /// 添加新的数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="text"></param>
            internal void Add(ref keyType key, string text)
            {
                getResult(text);
                if (result.Count != 0)
                {
                    searcher.add(ref key, text, result);
                    indexArrays.PrepLength(result.Count);
                    foreach (ResultIndexLeftArray indexArray in result.Values) indexArrays.UnsafeAdd(indexArray.Indexs.Array);
                }
            }
            /// <summary>
            /// 删除旧数据
            /// </summary>
            /// <param name="data"></param>
            internal void Remove(SearchData data)
            {
                if (searcher.getRemoveText(data))
                {
                    getRemoveResult(data.Text);
                    searcher.remove(ref data.Key, removeResult);
                }
            }
            /// <summary>
            /// 获取文本分词结果
            /// </summary>
            /// <param name="text"></param>
            private void getRemoveResult(string text)
            {
                removeResult.Clear();
                formatLength = text.Length;
                formatText = AutoCSer.Extension.StringExtension.FastAllocateString(formatLength + 1);
                fixed (char* textFixed = formatText)
                {
                    Simplified.FormatNotEmpty(text, textFixed, formatLength);
                    matchs.Length = 0;
                    char* start = textFixed, end = textFixed + formatLength;
                    if (charTypeData != StringTrieGraph.DefaultCharTypeData.Byte)
                    {
                        StaticStringTrieGraph trieGraph = searcher.trieGraph;
                        int count, index, startIndex;
                        char trieGraphHeadChar = trieGraph.AnyHeadChar;
                        byte type, nextType;
                        bool isMatchMap = false;
                        do
                        {
                            if (((type = charTypeData[*start]) & StringTrieGraph.TrieGraphHeadFlag) == 0)
                            {
                                *end = trieGraphHeadChar;
                                do
                                {
                                    if ((type & ((byte)WordType.Chinese | (byte)WordType.TrieGraph)) == ((byte)WordType.Chinese | (byte)WordType.TrieGraph)) removeResult.Add(new SubString((int)(start - textFixed), 1, formatText));
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
                                if ((type & (byte)WordType.Chinese) != 0) removeResult.Add(new SubString((int)(segment - textFixed), 1, formatText));
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
                                        removeResult.Add(new SubString(index = value.Key + startIndex, value.Value, formatText));
                                        matchMap.Set(index, value.Value);
                                        if (--count == 0) break;
                                    }
                                    index = (int)(segmentEnd - textFixed);
                                    do
                                    {
                                        if (matchMap.Get(startIndex) == 0 && (charTypeData[textFixed[startIndex]] & (byte)WordType.Chinese) != 0) removeResult.Add(new SubString(startIndex, 1, formatText));
                                    }
                                    while (++startIndex != index);
                                }
                            CHINESE:
                                while (segmentEnd != start)
                                {
                                    if ((charTypeData[*segmentEnd] & (byte)WordType.Chinese) != 0) removeResult.Add(new SubString((int)(segmentEnd - textFixed), 1, formatText));
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
                        byte type = charTypeData[*start];
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
                                if ((type & (byte)WordType.TrieGraph) == 0) removeResult.Add(new SubString((int)(start - textFixed), 1, formatText));
                            }
                            while (((type = charTypeData[*++start]) & (byte)WordType.Chinese) != 0);
                        }
                        else
                        {
                            char* segment = start;
                            if ((type & (byte)WordType.OtherLetter) == 0)
                            {
                                char* word = start;
                                for (byte newType = charTypeData[*++start]; (newType &= ((byte)WordType.Letter | (byte)WordType.Number | (byte)WordType.Keep)) != 0; newType = charTypeData[*++start])
                                {
                                    if (type != newType)
                                    {
                                        if (type != (byte)WordType.Keep) removeResult.Add(new SubString((int)(word - textFixed), (int)(start - word), formatText));
                                        type = newType;
                                        word = start;
                                    }
                                }
                            }
                            else
                            {
                                while ((charTypeData[*++start] & (byte)WordType.OtherLetter) != 0) ;
                            }
                            removeResult.Add(new SubString((int)(segment - textFixed), (int)(start - segment), formatText));
                        }
                    }
                    while (start != end);
                }
            }
        }
    }
}
