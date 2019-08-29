using System;
using AutoCSer.Search;
using System.Collections.Generic;

namespace AutoCSer.Web.SearchServer
{
    /// <summary>
    /// 线程参数
    /// </summary>
    internal class ThreadParameter : AutoCSer.Search.StaticSearcher<DataKey>.ThreadParameter
    {
        /// <summary>
        /// 索引结果
        /// </summary>
        internal LeftArray<KeyValuePair<DataKey, ResultIndexArray>> ResultIndexArray;
        /// <summary>
        /// 线程参数
        /// </summary>
        /// <param name="searcher">绑定结果池的分词搜索器</param>
        public ThreadParameter(StaticSearcher<DataKey> searcher) : base(searcher) { }
    }
}
