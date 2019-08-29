using System;
using AutoCSer.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Web.SearchServer.Queue
{
    /// <summary>
    /// 搜索
    /// </summary>
    internal sealed class Search : TaskQueueNode
    {
        /// <summary>
        /// 最大搜索文本长度
        /// </summary>
        private const int maxTextSize = 127;
        /// <summary>
        /// 搜索结果最大缓存数量
        /// </summary>
        private const int maxResultCount = 1024;
        /// <summary>
        /// 分页记录数量
        /// </summary>
        private const int pageSize = 20;
        /// <summary>
        /// 搜索结果缓存
        /// </summary>
        private static readonly AutoCSer.FifoPriorityQueue<HashString, SearchItem[]> resultCache = new AutoCSer.FifoPriorityQueue<HashString, SearchItem[]>();
        /// <summary>
        /// 获取数据权重
        /// </summary>
        /// <param name="type"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static int getWeight(DataType type, int weight)
        {
            switch (type)
            {
                case DataType.HtmlTitle:
                    return (weight << 4);
                case DataType.HtmlImage:
                    return (weight << 2);
            }
            return weight;
        }

        /// <summary>
        /// 关键字
        /// </summary>
        private readonly string key;
        /// <summary>
        /// 搜索回调
        /// </summary>
        private readonly Func<AutoCSer.Net.TcpServer.ReturnValue<SearchItem[]>, bool> onSearch;
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onSearch"></param>
        internal Search(string key, Func<AutoCSer.Net.TcpServer.ReturnValue<SearchItem[]>, bool> onSearch)
        {
            this.key = key;
            this.onSearch = onSearch;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        public override void RunTask()
        {
            SearchItem[] result = null;
            try
            {
                ThreadParameter threadParameter = Searcher.DefaultThreadParameter;
                SubString simplifiedKey = threadParameter.SetSimplified(key, maxTextSize);
                if (simplifiedKey.Count != 0)
                {
                    HashString cacheKey = simplifiedKey;
                    result = resultCache.Get(ref cacheKey, null);
                    if (result == null)
                    {
                        long wordCount = Searcher.Default.Results.WordCount;
                        LeftArray<KeyValue<HashString, AutoCSer.Search.StaticSearcher<DataKey>.QueryResult>> results = threadParameter.Search(); 
                        switch (results.Count)
                        {
                            case 0: result = NullValue<SearchItem>.Array; break;
                            case 1:
                                threadParameter.ResultIndexArray.Empty();
                                results[0].Value.CopyTo(ref threadParameter.ResultIndexArray);
                                result = threadParameter.ResultIndexArray.GetRangeSortDesc(value => getWeight(value.Key.Type, value.Value.Indexs.Length), 0, pageSize, value => new SearchItem(results[0].Key, value));
                                break;
                            default:
                                threadParameter.GetWeights();
                                result = threadParameter.GetWeightArray()
                                    .GetRangeSortDesc(weight => getWeight(weight.Key.Type, weight.Value), 0, pageSize, weight => new SearchItem(weight.Key));
                                break;
                        }
                        resultCache.Set(ref cacheKey, result);
                        if (resultCache.Count > maxResultCount) resultCache.Pop();
                    }
                }
            }
            finally
            {
                onSearch(result ?? NullValue<SearchItem>.Array);
            }
        }
    }
}
