using System;
using System.IO;
using System.Text;
using AutoCSer.Extension;

namespace AutoCSer.Web.SearchServer
{
    /// <summary>
    /// HTML 信息
    /// </summary>
    public sealed class Html
    {
        /// <summary>
        /// HTML 标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 页面地址
        /// </summary>
        public string Url;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title;
        /// <summary>
        /// 文本
        /// </summary>
        [AutoCSer.Json.IgnoreMember]
        [AutoCSer.BinarySerialize.IgnoreMember]
        internal string Text;

        /// <summary>
        /// HTML 信息缓存
        /// </summary>
        internal static SegmentArray<Html> Cache;
        static Html()
        {
            try
            {
                Cache = new SegmentArray<Html>(8);
                using (AutoCSer.Search.StaticSearcher<DataKey>.InitializeAdder adder = Searcher.Default.GetInitializeAdder())
                {
                    int urlIndex = AutoCSer.Web.Config.Search.HtmlPath.Length;
                    AutoCSer.HtmlNode.Filter bodyFilter = AutoCSer.HtmlNode.Filter.Get(@"/html[0]/body[0]"), titleFilter = AutoCSer.HtmlNode.Filter.Get(@"/html[0]/head[0]/title[0]");
                    foreach (FileInfo htmlFile in new DirectoryInfo(AutoCSer.Web.Config.Search.HtmlPath).GetFiles("*.html", SearchOption.AllDirectories))
                    {
                        string fileName = htmlFile.FullName;
                        AutoCSer.HtmlNode.Node htmlNode = new AutoCSer.HtmlNode.Node(File.ReadAllText(fileName, Encoding.UTF8));
                        foreach (AutoCSer.HtmlNode.Node bodyNode in bodyFilter.Get(htmlNode))
                        {
                            foreach (AutoCSer.HtmlNode.Node titleNode in titleFilter.Get(htmlNode))
                            {
                                string title = titleNode.Text, text = bodyNode.Text;
                                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(text))
                                {
                                    Html html = new Html { Url = fileName.Substring(urlIndex).Replace('\\', '/'), Title = title, Text = text };
                                    html.Id = Cache.GetIndex(html);
                                    adder.Add(new DataKey { Id = html.Id, Type = DataType.HtmlTitle }, html.Title);
                                    adder.Add(new DataKey { Id = html.Id, Type = DataType.HtmlBodyText }, html.Text);

                                    foreach (AutoCSer.HtmlNode.Node imgNode in bodyNode.GetNodesByTagName("img"))
                                    {
                                        string alt = imgNode["alt"], src = imgNode["@src"];
                                        if (!string.IsNullOrEmpty(alt) && !string.IsNullOrEmpty(src))
                                        {
                                            HtmlImage image = new HtmlImage { HtmlId = html.Id, Url = src, Title = alt };
                                            image.Id = HtmlImage.Cache.GetIndex(image);
                                            adder.Add(new DataKey { Id = image.Id, Type = DataType.HtmlImage }, image.Title);
                                        }
                                    }
                                }
                                break;
                            }
                            break;
                        }
                    }
                }
            }
            finally { Searcher.Default.Initialized(); }
        }
    }
}
