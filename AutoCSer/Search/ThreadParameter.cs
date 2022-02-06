using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using AutoCSer.Extensions;
using AutoCSer.Memory;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定结果池的分词搜索器
    /// </summary>
    public sealed partial class StaticSearcher<keyType>
    {
        /// <summary>
        /// 线程参数
        /// </summary>
        public class ThreadParameter : IDisposable
        {
            /// <summary>
            /// 绑定结果池的分词搜索器
            /// </summary>
            private readonly StaticSearcher<keyType> searcher;
            /// <summary>
            /// 未匹配分词自定义过滤处理
            /// </summary>
            private readonly CheckLessWord checkLessWord;
            /// <summary>
            /// 数据权重
            /// </summary>
            private readonly ReusableDictionary<keyType, int> weights = ReusableDictionary<keyType>.Create<int>();
            /// <summary>
            /// 分词查询
            /// </summary>
            private WordQuery wordQuery;
            /// <summary>
            /// 数据更新队列
            /// </summary>
            private Queue queue;

            /// <summary>
            /// 搜索字符简体化
            /// </summary>
            internal Simplified Simplified;
            /// <summary>
            /// 分词结果
            /// </summary>
            private LeftArray<KeyValue<HashString, QueryResult>> queryResult = new LeftArray<KeyValue<HashString, QueryResult>>(0);
            /// <summary>
            /// 未匹配分词集合
            /// </summary>
            private LeftArray<SubString> lessWords;
            /// <summary>
            /// 文本匹配位图
            /// </summary>
            private AutoCSer.Memory.Pointer getResultIndexsMapBuffer;
            /// <summary>
            /// 匹配索引位置集合
            /// </summary>
            private KeyValue<int, int>[] resultIndexs;
            /// <summary>
            /// 线程参数
            /// </summary>
            /// <param name="searcher">绑定结果池的分词搜索器</param>
            /// <param name="checkLessWord">未匹配分词自定义过滤处理</param>
            /// <param name="checkLess">未匹配分词自定义过滤处理</param>
            public ThreadParameter(StaticSearcher<keyType> searcher, CheckLessWord checkLessWord = null, CheckLess checkLess = null)
            {
                this.searcher = searcher;
                this.checkLessWord = checkLessWord;
                wordQuery = new WordQuery(searcher);
                queue = new Queue(searcher);
            }
            /// <summary>
            /// 释放资源
            /// </summary>
            public void Dispose()
            {
                if (wordQuery != null)
                {
                    wordQuery.Dispose();
                    wordQuery = null;
                }
                if (queue != null)
                {
                    queue.Dispose();
                    queue = null;
                }
                Unmanaged.Free(ref getResultIndexsMapBuffer);
            }
            /// <summary>
            /// 搜索字符简体化
            /// </summary>
            /// <param name="text">搜索关键字</param>
            /// <param name="maxSize">关键字最大字符长度</param>
            /// <returns>格式化字符串</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public SubString SetSimplified(string text, int maxSize)
            {
                Simplified.Set(text, maxSize, true);
                return Simplified.Text;
            }
            /// <summary>
            /// 搜索字符简体化
            /// </summary>
            /// <param name="text">搜索关键字</param>
            /// <param name="maxSize">关键字最大字符长度</param>
            /// <returns>格式化字符串</returns>
            public unsafe SubString SetSimplifiedToLower(string text, int maxSize)
            {
                Simplified.Set(text, maxSize, true);
                if (Simplified.Size > 0)
                {
                    fixed (char* textFixed = Simplified.GetFixedBuffer()) StringExtension.ToLower(textFixed, textFixed + Simplified.Size);
                }
                return Simplified.Text;
            }
            /// <summary>
            /// 获取分词结果
            /// </summary>
            /// <param name="matchType"></param>
            /// <returns>分词结果</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public LeftArray<KeyValue<HashString, QueryResult>> Search(MatchType matchType = MatchType.None)
            {
                queryResult.Length = 0;
                wordQuery.Get(ref Simplified, matchType, ref queryResult, ref lessWords);
                return queryResult;
            }
            /// <summary>
            /// 获取分词结果
            /// </summary>
            /// <param name="lessWords"></param>
            /// <returns>分词结果</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public LeftArray<KeyValue<HashString, QueryResult>> Search(out LeftArray<SubString> lessWords)
            {
                queryResult.Length = 0;
                this.lessWords.Length = 0;
                wordQuery.Get(ref Simplified, MatchType.Less, ref queryResult, ref this.lessWords);
                lessWords = this.lessWords;
                return queryResult;
            }
            /// <summary>
            /// 数据权重计算
            /// </summary>
            /// <param name="isKey">数据标识匹配</param>
            /// <param name="getWeight">权重计算</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void GetWeights(Func<keyType, bool> isKey = null, IWeight getWeight = null)
            {
                weights.Empty();
                searcher.GetWeights(ref queryResult, weights, isKey, getWeight);
            }
            /// <summary>
            /// 获取多关键字数据权重结果
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public LeftArray<KeyValue<keyType, int>> GetWeightArray()
            {
                weightArray.Length = 0;
                weights.CopyTo(ref weightArray);
                return weightArray;
            }
            /// <summary>
            /// 获取文本的匹配索引位置
            /// </summary>
            /// <param name="key">数据关键字</param>
            /// <param name="textLength">数据文本长度</param>
            /// <returns>文本的匹配索引位置</returns>
            public unsafe KeyValue<int, int>[] GetResultIndexs(ref keyType key, int textLength)
            {
                int size = ((textLength + 63) >> 3) & (int.MaxValue - 7);
                if (getResultIndexsMapBuffer.ByteSize < size)
                {
                    Unmanaged.Free(ref getResultIndexsMapBuffer);
                    getResultIndexsMapBuffer = Unmanaged.GetPointer(size, true);
                }
                else AutoCSer.Memory.Common.Clear(getResultIndexsMapBuffer.ULong, size >> 3);
                int count = searcher.GetResultIndexs(ref key, textLength, ref queryResult, ref resultIndexs, getResultIndexsMapBuffer.Data);
                return new LeftArray<KeyValue<int, int>>(count, resultIndexs).GetArray();
            }
            /// <summary>
            /// 获取文本的匹配索引位置
            /// </summary>
            /// <param name="text">文本</param>
            /// <param name="indexs">匹配索引位置</param>
            /// <param name="wordLength">分词长度</param>
            /// <param name="maxLength">最大长度</param>
            /// <returns>匹配索引位置集合</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public unsafe KeyValue<int, int>[] FormatTextIndexs(ref SubString text, int[] indexs, int wordLength, int maxLength)
            {
                int count = searcher.FormatTextIndexs(ref text, indexs, wordLength, maxLength, ref resultIndexs);
                return new LeftArray<KeyValue<int, int>>(count, resultIndexs).GetArray();
            }
            /// <summary>
            /// 获取文本的匹配索引位置
            /// </summary>
            /// <param name="key">数据关键字</param>
            /// <param name="text">数据文本</param>
            /// <param name="maxLength">最大长度</param>
            /// <returns>匹配索引位置集合</returns>
            public unsafe KeyValue<int, int>[] FormatTextIndexs(ref keyType key, ref SubString text, int maxLength)
            {
                int size = ((text.Length + 63) >> 3) & (int.MaxValue - 7);
                if (getResultIndexsMapBuffer.ByteSize < size)
                {
                    Unmanaged.Free(ref getResultIndexsMapBuffer);
                    getResultIndexsMapBuffer = Unmanaged.GetPointer(size, true);
                }
                int count = searcher.FormatTextIndexs(ref key, ref text, ref queryResult, maxLength, ref resultIndexs, ref getResultIndexsMapBuffer);
                return new LeftArray<KeyValue<int, int>>(count, resultIndexs).GetArray();
            }
            /// <summary>
            /// 添加新的数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="text"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Add(ref keyType key, string text)
            {
                if (!string.IsNullOrEmpty(text)) queue.Add(ref key, text);
            }
            /// <summary>
            /// 删除数据
            /// </summary>
            private readonly SearchData removeSearchData = new SearchData();
            /// <summary>
            /// 删除旧的数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="text"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Remove(ref keyType key, string text)
            {
                removeSearchData.SetRemove(ref key, text);
                queue.Remove(removeSearchData);
            }
            /// <summary>
            /// 更新数据
            /// </summary>
            /// <param name="key">数据标识</param>
            /// <param name="text">数据文本</param>
            /// <param name="oldText">数据文本</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Update(ref keyType key, string text, string oldText = null)
            {
                Remove(ref key, oldText);
                if (!string.IsNullOrEmpty(text)) queue.Add(ref key, text);
            }
            /// <summary>
            /// 单关键字搜索结果
            /// </summary>
            private LeftArray<KeyValuePair<keyType, ResultIndexArray>> resultIndexArray = new LeftArray<KeyValuePair<keyType, ResultIndexArray>>(0);
            /// <summary>
            /// 多关键字数据权重结果
            /// </summary>
            private LeftArray<KeyValue<keyType, int>> weightArray = new LeftArray<KeyValue<keyType, int>>(0);
            /// <summary>
            /// 获取搜索数据标识集合（任意匹配分词，根据权重排序）
            /// </summary>
            /// <param name="text">搜索关键字</param>
            /// <param name="maxSize">关键字最大字符长度</param>
            /// <param name="skipCount">跳过记录数据</param>
            /// <param name="getCount">获取记录数据</param>
            /// <param name="count">匹配数据总数</param>
            /// <param name="isKey">数据标识过滤</param>
            /// <param name="getWeight">权重计算</param>
            /// <returns>数据标识集合</returns>
            public keyType[] SearchAny(string text, int maxSize, int skipCount, int getCount, out int count, Func<keyType, bool> isKey = null, IWeight getWeight = null)
            {
                Simplified.Set(text, maxSize, true);
                if (Simplified.Size == 0)
                {
                    count = 0;
                    return EmptyArray<keyType>.Array;
                }

                queryResult.Length = 0;
                wordQuery.Get(ref Simplified, MatchType.None, ref queryResult, ref lessWords);
                switch (queryResult.Count)
                {
                    case 0: count = 0; break;
                    case 1:
                        resultIndexArray.Length = 0;
                        foreach (KeyValuePair<keyType, ResultIndexArray> result in queryResult[0].Value.Dictionary)
                        {
                            if (isKey(result.Key)) resultIndexArray.Add(result);
                        }
                        count = resultIndexArray.Length;
                        if (count > skipCount) return resultIndexArray.GetRangeSortDesc(result => result.Value.Indexs.Length, skipCount, Math.Min(getCount, count - skipCount), result => result.Key);
                        break;
                    default:
                        GetWeights(isKey, getWeight);
                        count = weights.Count;
                        if (count > skipCount)
                        {
                            weightArray.Length = 0;
                            weights.CopyTo(ref weightArray);
                            return weightArray.GetRangeSortDesc(weight => weight.Value, skipCount, Math.Min(getCount, count - skipCount), result => result.Key);
                        }
                        break;
                }
                return EmptyArray<keyType>.Array;
            }
            /// <summary>
            /// 获取搜索数据标识集合（匹配所有分词结果）
            /// </summary>
            /// <param name="text">搜索关键字</param>
            /// <param name="maxSize">关键字最大字符长度</param>
            /// <param name="isKey">数据标识过滤</param>
            /// <returns>数据标识集合</returns>
            public IEnumerable<keyType> SearchAll(string text, int maxSize, Func<keyType, bool> isKey)
            {
                Simplified.Set(text, maxSize, true);
                if (Simplified.Size != 0)
                {
                    queryResult.Length = 0;
                    wordQuery.Get(ref Simplified, MatchType.All, ref queryResult, ref lessWords);
                    switch (queryResult.Count)
                    {
                        case 0: break;
                        case 1:
                            foreach (KeyValuePair<keyType, ResultIndexArray> result in queryResult[0].Value.Dictionary)
                            {
                                if (isKey(result.Key)) yield return result.Key;
                            }
                            break;
                        case 2:
                            Dictionary<keyType, ResultIndexArray> resultDictionary = sortResult();
                            Dictionary<keyType, ResultIndexArray> nextResultDictionary = queryResult.Array[0].Value.Dictionary;
                            foreach (keyType key in resultDictionary.Keys)
                            {
                                if (nextResultDictionary.ContainsKey(key) && isKey(key)) yield return key;
                            }
                            break;
                        default:
                            resultDictionary = sortResult();
                            KeyValue<HashString, QueryResult>[] resultArray = queryResult.Array;
                            foreach (keyType key in resultDictionary.Keys)
                            {
                                int count = queryResult.Length;
                                foreach (KeyValue<HashString, QueryResult> result in resultArray)
                                {
                                    if (!result.Value.Dictionary.ContainsKey(key)) break;
                                    if (--count == 0)
                                    {
                                        if (isKey(key)) yield return key;
                                        break;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            /// <summary>
            /// 多结果集排序
            /// </summary>
            /// <returns></returns>
            private Dictionary<keyType, ResultIndexArray> sortResult()
            {
                int resultIndex = 0, count = queryResult.Length;
                Dictionary<keyType, ResultIndexArray> resultDictionary = null;
                KeyValue<HashString, QueryResult>[] resultArray = queryResult.Array;
                foreach (KeyValue<HashString, QueryResult> result in resultArray)
                {
                    if (resultDictionary != null)
                    {
                        if (result.Value.Dictionary.Count < resultDictionary.Count)
                        {
                            resultIndex = queryResult.Length - count;
                            resultDictionary = result.Value.Dictionary;
                        }
                    }
                    else resultDictionary = result.Value.Dictionary;
                    if (--count == 0) break;
                }
                if (resultIndex != --queryResult.Length) resultArray[resultIndex] = resultArray[queryResult.Length];
                if (queryResult.Length != 1) AutoCSer.Algorithm.QuickSort.Sort(resultArray, resultSort, 0, queryResult.Length);
                return resultDictionary;
            }
            /// <summary>
            /// 获取搜索数据标识集合（记录未匹配分词）
            /// </summary>
            /// <param name="text">搜索关键字</param>
            /// <param name="maxSize">关键字最大字符长度</param>
            /// <param name="isKey">数据标识过滤</param>
            /// <returns>数据标识集合</returns>
            public IEnumerable<keyType> SearchLess(string text, int maxSize, CheckLess isKey)
            {
                Simplified.Set(text, maxSize, true);
                if (Simplified.Size != 0)
                {
                    queryResult.Length = 0;
                    wordQuery.Get(ref Simplified, MatchType.Less, ref queryResult, ref lessWords);
                    if (queryResult.Length > 0 && (lessWords.Length == 0 || checkLessWord(ref lessWords)))
                    {
                        if (queryResult.Length == 1)
                        {
                            foreach (KeyValuePair<keyType, ResultIndexArray> result in queryResult[0].Value.Dictionary)
                            {
                                if (isKey(result.Key, ref lessWords)) yield return result.Key;
                            }
                        }
                        else
                        {
                            Dictionary<keyType, ResultIndexArray> resultDictionary = sortResultLess();
                            IEnumerable<keyType> keys;
                            if (resultDictionary != null) keys = resultDictionary.Keys;
                            else
                            {
                                getLessKeys();
                                keys = weights.Keys;
                            }
                            KeyValue<HashString, QueryResult>[] resultArray = queryResult.Array;
                            foreach (keyType key in keys)
                            {
                                int count = queryResult.Length, lessWordCount = 0;
                                foreach (KeyValue<HashString, QueryResult> result in resultArray)
                                {
                                    if (!result.Value.Dictionary.ContainsKey(key))
                                    {
                                        if (result.Key.String.Length != 1) break;
                                        lessWords.Add(result.Key.String);
                                        ++lessWordCount;
                                    }
                                    if (--count == 0)
                                    {
                                        if (isKey(key, ref lessWords)) yield return key;
                                        break;
                                    }
                                }
                                lessWords.Length -= lessWordCount;
                            }
                        }
                    }
                }
            }
            /// <summary>
            /// 多结果集排序
            /// </summary>
            /// <returns></returns>
            private Dictionary<keyType, ResultIndexArray> sortResultLess()
            {
                int resultIndex = 0, count = queryResult.Length;
                Dictionary<keyType, ResultIndexArray> resultDictionary = null;
                KeyValue<HashString, QueryResult>[] resultArray = queryResult.Array;
                foreach (KeyValue<HashString, QueryResult> result in resultArray)
                {
                    if (result.Key.String.Length > 1)
                    {
                        if (resultDictionary != null)
                        {
                            if (result.Value.Dictionary.Count < resultDictionary.Count)
                            {
                                resultIndex = queryResult.Length - count;
                                resultDictionary = result.Value.Dictionary;
                            }
                        }
                        else resultDictionary = result.Value.Dictionary;
                    }
                    if (--count == 0) break;
                }
                if (resultDictionary == null) return null;
                if (resultIndex != --queryResult.Length) resultArray[resultIndex] = resultArray[queryResult.Length];
                if (queryResult.Length != 1) AutoCSer.Algorithm.QuickSort.Sort(resultArray, resultSort, 0, queryResult.Length);
                return resultDictionary;
            }
            /// <summary>
            /// 获取所有关键字
            /// </summary>
            private void getLessKeys()
            {
                weights.Empty();
                int resultIndex = 0, count = queryResult.Length;
                Dictionary<keyType, ResultIndexArray> resultDictionary = null;
                KeyValue<HashString, QueryResult>[] resultArray = queryResult.Array;
                foreach (KeyValue<HashString, QueryResult> result in resultArray)
                {
                    foreach (keyType key in result.Value.Dictionary.Keys) weights.Set(key, 0);
                    if (resultDictionary != null)
                    {
                        if (result.Value.Dictionary.Count > resultDictionary.Count)
                        {
                            resultIndex = queryResult.Length - count;
                            resultDictionary = result.Value.Dictionary;
                        }
                    }
                    else resultDictionary = result.Value.Dictionary;
                    if (--count == 0) break;
                }
                if (resultIndex != --queryResult.Length) resultArray[resultIndex] = resultArray[queryResult.Length];
            }

            /// <summary>
            /// 获取文本分词结果
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public LeftArray<KeyValue<SubString, WordType>> WordSegment(string text)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    queue.GetResult(text, true);
                    return queue.Words;
                }
                return new LeftArray<KeyValue<SubString, WordType>>(0);
            }

            /// <summary>
            /// 搜索结果数量排序
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            private static int sort(KeyValue<HashString, QueryResult> left, KeyValue<HashString, QueryResult> right)
            {
                return left.Value.Dictionary.Count - right.Value.Dictionary.Count;
            }
            /// <summary>
            /// 搜索结果数量排序
            /// </summary>
            private static readonly Func<KeyValue<HashString, QueryResult>, KeyValue<HashString, QueryResult>, int> resultSort = sort;
        }

        /// <summary>
        /// 未匹配分词自定义过滤处理
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lessWords"></param>
        /// <returns></returns>
        public delegate bool CheckLess(keyType key, ref LeftArray<SubString> lessWords);
    }
}
