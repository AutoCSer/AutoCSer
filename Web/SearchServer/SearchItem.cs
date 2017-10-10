using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using AutoCSer.Extension;

namespace AutoCSer.Web.SearchServer
{
    /// <summary>
    /// 搜索结果项
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    [AutoCSer.WebView.ClientType(PrefixName = AutoCSer.Web.Config.Web.WebViewClientTypePrefix)]
    [AutoCSer.Net.TcpStaticServer.Server(Name = Server.ServerName)]
    public partial struct SearchItem
    {
        /// <summary>
        /// 最大文本长度
        /// </summary>
        private const int maxTextLength = 64;

        /// <summary>
        /// 搜索数据关键字
        /// </summary>
        [AutoCSer.Net.TcpStaticServer.RemoteKey]
        public DataKey DataKey;
        /// <summary>
        /// 远程调用链配置
        /// </summary>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpStaticServer.RemoteKey]
        private static SearchItem get(DataKey dataKey)
        {
            return new SearchItem { DataKey = dataKey };
        }
        /// <summary>
        /// 页面 URI
        /// </summary>
        public Html Html;
        /// <summary>
        /// 匹配文本
        /// </summary>
        internal string MatchText;
        /// <summary>
        /// 匹配文本
        /// </summary>
        public string Text
        {
            get { return MatchText ?? Html.Title; }
        }
        /// <summary>
        /// 匹配位置集合
        /// </summary>
        public KeyValue<int, int>[] Indexs;
        /// <summary>
        /// 图片地址
        /// </summary>
        [AutoCSer.Net.TcpStaticServer.RemoteMember(IsAwait = false)]
        internal string ImageUrl
        {
            get
            {
                return DataKey.Type == DataType.HtmlImage ? HtmlImage.Cache[DataKey.Id].Url : null;
            }
        }
        /// <summary>
        /// 搜索结果项
        /// </summary>
        /// <param name="word">分词</param>
        /// <param name="result">索引结果</param>
        internal SearchItem(SubString word, KeyValuePair<DataKey, AutoCSer.Search.ResultIndexArray> result)
        {
            DataKey = result.Key;
            switch (DataKey.Type)
            {
                case DataType.HtmlTitle:
                    Html = Html.Cache[DataKey.Id];
                    MatchText = null;
                    Indexs = result.Value.Indexs.getArray(index => new KeyValue<int, int>(index, word.Count));
                    break;
                case DataType.HtmlBodyText:
                    MatchText = (Html = Html.Cache[DataKey.Id]).Text;
                    Indexs = Searcher.Default.FormatTextIndexs(ref MatchText, result.Value.Indexs, word.Count, maxTextLength);
                    break;
                case DataType.HtmlImage:
                    HtmlImage image = HtmlImage.Cache[DataKey.Id];
                    Html = Html.Cache[image.HtmlId];
                    MatchText = image.Title;
                    Indexs = result.Value.Indexs.getArray(index => new KeyValue<int, int>(index, word.Count));
                    break;
                default:
                    Html = null;
                    MatchText = null;
                    Indexs = null;
                    break;
            }
        }
        /// <summary>
        /// 搜索结果项
        /// </summary>
        /// <param name="dataKey">搜索数据关键字</param>
        /// <param name="results">搜索结果</param>
        internal SearchItem(DataKey dataKey, LeftArray<KeyValue<HashString, AutoCSer.Search.StaticSearcher<DataKey>.QueryResult>> results)
        {
            DataKey = dataKey;
            switch (dataKey.Type)
            {
                case DataType.HtmlTitle:
                    Html = Html.Cache[dataKey.Id];
                    MatchText = null;
                    Indexs = Searcher.Default.GetResultIndexs(ref dataKey, Html.Title.Length, ref results);
                    break;
                case DataType.HtmlBodyText:
                    MatchText = (Html = Html.Cache[dataKey.Id]).Text;
                    Indexs = Searcher.Default.FormatTextIndexs(ref dataKey, ref MatchText, ref results, maxTextLength);
                    break;
                case DataType.HtmlImage:
                    HtmlImage image = HtmlImage.Cache[dataKey.Id];
                    Html = Html.Cache[image.HtmlId];
                    MatchText = image.Title;
                    Indexs = Searcher.Default.GetResultIndexs(ref dataKey, MatchText.Length, ref results);
                    break;
                default:
                    Html = null;
                    MatchText = null;
                    Indexs = null;
                    break;
            }
        }
    }
}
