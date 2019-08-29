using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using AutoCSer.Extension;

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
            private LeftArray<KeyValue<HashString, QueryResult>> queryResult;
            /// <summary>
            /// 文本匹配位图
            /// </summary>
            private Pointer.Size getResultIndexsMapBuffer;
            /// <summary>
            /// 匹配索引位置集合
            /// </summary>
            private KeyValue<int, int>[] resultIndexs;
            /// <summary>
            /// 线程参数
            /// </summary>
            /// <param name="searcher">绑定结果池的分词搜索器</param>
            public ThreadParameter(StaticSearcher<keyType> searcher)
            {
                this.searcher = searcher;
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
                    fixed (char* textFixed = Simplified.FormatText) StringExtension.ToLower(textFixed, textFixed + Simplified.Size);
                }
                return Simplified.Text;
            }
            /// <summary>
            /// 获取分词结果
            /// </summary>
            /// <param name="isAllMatch">是否要求关键字全匹配</param>
            /// <returns>分词结果</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public LeftArray<KeyValue<HashString, QueryResult>> Search(bool isAllMatch = false)
            {
                queryResult.Length = 0;
                wordQuery.Get(ref Simplified, isAllMatch, ref queryResult);
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
                    getResultIndexsMapBuffer = Unmanaged.GetSize64(size, true);
                }
                else Memory.ClearUnsafe(getResultIndexsMapBuffer.ULong, size >> 3);
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
                    getResultIndexsMapBuffer = Unmanaged.GetSize64(size);
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
                queue.Add(ref key, text);
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
                queue.Add(ref key, text);
            }
            /// <summary>
            /// 单关键字搜索结果
            /// </summary>
            private LeftArray<KeyValuePair<keyType, ResultIndexArray>> resultIndexArray;
            /// <summary>
            /// 多关键字数据权重结果
            /// </summary>
            private LeftArray<KeyValue<keyType, int>> weightArray;
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
                    return NullValue<keyType>.Array;
                }

                queryResult.Length = 0;
                wordQuery.Get(ref Simplified, false, ref queryResult);
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
                return NullValue<keyType>.Array;
            }
            /// <summary>
            /// 单关键字搜索结果
            /// </summary>
            private LeftArray<keyType> resultArray;
            /// <summary>
            /// 多关键字搜索结果
            /// </summary>
            private readonly ReusableDictionary<keyType, int> resultCountDictionary = ReusableDictionary<keyType>.Create<int>();
            /// <summary>
            /// 获取搜索数据标识集合（匹配所有分词结果）
            /// </summary>
            /// <param name="text">搜索关键字</param>
            /// <param name="maxSize">关键字最大字符长度</param>
            /// <param name="isKey">数据标识过滤</param>
            /// <returns>数据标识集合</returns>
            public LeftArray<keyType> SearchAll(string text, int maxSize, Func<keyType, bool> isKey = null)
            {
                resultArray.Length = 0;
                Simplified.Set(text, maxSize, true);
                if (Simplified.Size != 0)
                {
                    queryResult.Length = 0;
                    wordQuery.Get(ref Simplified, true, ref queryResult);
                    switch (queryResult.Count)
                    {
                        case 0: break;
                        case 1:
                            foreach (KeyValuePair<keyType, ResultIndexArray> result in queryResult[0].Value.Dictionary)
                            {
                                if (isKey(result.Key)) resultArray.Add(result.Key);
                            }
                            break;
                        default:
                            Dictionary<keyType, ResultIndexArray> resultDictionary = null;
                            foreach (KeyValue<HashString, QueryResult> result in queryResult)
                            {
                                if (resultDictionary == null || result.Value.Dictionary.Count < resultDictionary.Count) resultDictionary = result.Value.Dictionary;
                            }
                            resultCountDictionary.Empty();
                            foreach (keyType key in resultDictionary.Keys)
                            {
                                if (isKey(key))
                                {
                                    resultCountDictionary.Set(key, 0);
                                }
                            }
                            if (resultCountDictionary.Count != 0)
                            {
                                int count = 0, keyCount = resultCountDictionary.Count;
                                foreach (KeyValue<HashString, QueryResult> result in queryResult)
                                {
                                    if (!object.ReferenceEquals(result.Value.Dictionary, resultDictionary))
                                    {
                                        int nextCount = count + 1, resultCount;
                                        keyCount = 0;
                                        foreach (keyType key in result.Value.Dictionary.Keys)
                                        {
                                            if (resultCountDictionary.TryGetValue(key, out resultCount) && resultCount == count)
                                            {
                                                resultCountDictionary.Set(key, nextCount);
                                                ++keyCount;
                                            }
                                        }
                                        if (keyCount == 0)
                                        {
                                            resultCountDictionary.Empty();
                                            break;
                                        }
                                        count = nextCount;
                                    }
                                }
                                if (keyCount > 0)
                                {
                                    resultArray.PrepLength(keyCount);
                                    foreach (KeyValue<keyType, int> result in resultCountDictionary.KeyValues)
                                    {
                                        if (result.Value == count) resultArray.UnsafeAdd(result.Key);
                                    }
                                }
                            }
                            break;
                    }
                }
                return resultArray;
            }
        }
    }
}
