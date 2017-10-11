using System;
using System.Threading;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Web.SearchServer
{
    /// <summary>
    /// 搜索服务
    /// </summary>
    [AutoCSer.Net.TcpStaticServer.Server(Name = Server.ServerName, Host = "127.0.0.1", IsServer = true)]
    public partial class Server : AutoCSer.Net.TcpStaticServer.TimeVerify<Server>
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = "SearchServer";
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
        /// 搜索结果缓存访问锁
        /// </summary>
        private static readonly object resultCacheLock = new object();
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
        /// 关键字搜索
        /// </summary>
        /// <param name="key">字搜索</param>
        [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod]
        internal static SearchItem[] Search(string key)
        {
            AutoCSer.Search.Simplified simplified = new AutoCSer.Search.Simplified(key, maxTextSize);
            if (simplified.Text != null)
            {
                HashString cacheKey = simplified.Text;
                SearchItem[] value = resultCache.Get(ref cacheKey, null);
                if (value == null)
                {
                    long wordCount = Searcher.Default.Results.WordCount;
                    LeftArray<KeyValue<HashString, AutoCSer.Search.StaticSearcher<DataKey>.QueryResult>> results = Searcher.Default.Search(simplified);
                    switch (results.Count)
                    {
                        case 0: value = NullValue<SearchItem>.Array; break;
                        case 1:
                            value = results[0].Value.Result.getLeftArray(results[0].Value.Count)
                                .getRangeSortDesc(result => getWeight(result.Key.Type, result.Value.Indexs.Length), 0, pageSize, result => new SearchItem(results[0].Key, result));
                            break;
                        default:
                            value = Searcher.Default.GetWeights(ref results, Weight.Default)
                                .getArray(weight => new KeyValue<DataKey, int>(weight.Key, getWeight(weight.Key.Type, weight.Value)))
                                .getRangeSortDesc(weight => weight.Value, 0, pageSize, weight => new SearchItem(weight.Key, results));
                            break;
                    }
                    Monitor.Enter(resultCacheLock);
                    try
                    {
                        resultCache.Set(ref cacheKey, value);
                        if (resultCache.Count > maxResultCount) resultCache.Pop();
                    }
                    finally { Monitor.Exit(resultCacheLock); }
                }
                return value;
            }
            return NullValue<SearchItem>.Array;
        }

        /// <summary>
        /// 触发 HTML 初始化加载
        /// </summary>
        private static readonly Html html0 = Html.Cache[0];
        /// <summary>
        /// 当前进程标识
        /// </summary>
        internal static readonly int CurrentProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
        /// <summary>
        /// 当前服务实例
        /// </summary>
        internal static AutoCSer.Web.SearchServer.TcpStaticServer.SearchServer CurrentServer;
        /// <summary>
        /// 停止服务监听
        /// </summary>
        /// <param name="processId">请求方进程标识</param>
        [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod]
        internal static void StopListen(int processId)
        {
            if (processId != CurrentProcessId && CurrentServer != null)
            {
                CurrentServer.StopListen();
                AutoCSer.Web.Config.Pub.Exit();
            }
        }
    }
}
