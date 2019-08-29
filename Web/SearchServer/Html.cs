using System;
using System.IO;
using System.Text;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Threading;

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
        /// 图片集合
        /// </summary>
        [AutoCSer.Json.IgnoreMember]
        [AutoCSer.BinarySerialize.IgnoreMember]
        internal HtmlImage[] Images;

        /// <summary>
        /// HTML 信息缓存
        /// </summary>
        internal static SegmentArray<Html> Cache;
        /// <summary>
        /// 页面地址集合
        /// </summary>
        private static readonly Dictionary<string, Html> urls = DictionaryCreator.CreateOnly<string, Html>();
        /// <summary>
        /// HTML 文件监视器
        /// </summary>
        private static readonly FileSystemWatcher htmlWatcher;
        /// <summary>
        /// HTML 文件更新访问锁
        /// </summary>
        private static readonly object htmlLock = new object();
        /// <summary>
        /// 空 HTML 信息
        /// </summary>
        private static readonly Html Null = new Html();
        /// <summary>
        /// 新建 HTML 文件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void onCreatedHtml(object sender, FileSystemEventArgs e)
        {
            try
            {
                FileInfo htmlFile = new FileInfo(e.FullPath);
                Html newHtml = getHtml(htmlFile);
                if (newHtml != null)
                {
                    Html html;
                    string url = htmlFile.FullName.Substring(AutoCSer.Web.Config.Search.HtmlPath.Length).Replace('\\', '/'), lowerUrl = url.FileNameToLower();
                    Monitor.Enter(htmlLock);
                    if (urls.TryGetValue(lowerUrl, out html) && html != Null)
                    {
                        Monitor.Exit(htmlLock);
                        if (html.Title != newHtml.Title)
                        {
                            Searcher.SearchTaskQueue.Add(new Queue.Update(new DataKey { Type = DataType.HtmlTitle, Id = html.Id }, newHtml.Title, html.Title));
                            html.Title = newHtml.Title;
                        }
                        if (html.Text != newHtml.Text)
                        {
                            Searcher.SearchTaskQueue.Add(new Queue.Update(new DataKey { Type = DataType.HtmlBodyText, Id = html.Id }, newHtml.Text, html.Text));
                            html.Text = newHtml.Text;
                        }
                        HtmlImage.Free(ref html.Images);
                        html.Images = newHtml.Images;
                    }
                    else
                    {
                        try
                        {
                            urls[lowerUrl] = newHtml;
                        }
                        finally { Monitor.Exit(htmlLock); }
                        newHtml.Url = url;
                        newHtml.Id = Cache.GetIndex(newHtml);
                        Searcher.SearchTaskQueue.Add(new Queue.Append(new DataKey { Id = newHtml.Id, Type = DataType.HtmlTitle }, newHtml.Title));
                        Searcher.SearchTaskQueue.Add(new Queue.Append(new DataKey { Id = newHtml.Id, Type = DataType.HtmlBodyText }, newHtml.Text));
                        html = newHtml;
                    }
                    foreach (HtmlImage image in html.Images)
                    {
                        image.GetIndex(html.Id);
                        Searcher.SearchTaskQueue.Add(new Queue.Append(new DataKey { Id = image.Id, Type = DataType.HtmlImage }, image.Title));
                    }
                }
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
        }
        /// <summary>
        /// 删除 HTML 文件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void onDeleteHtml(object sender, FileSystemEventArgs e)
        {
            try
            {
                Html html;
                string url = new FileInfo(e.FullPath).FullName.Substring(AutoCSer.Web.Config.Search.HtmlPath.Length).Replace('\\', '/').FileNameToLower();
                Monitor.Enter(htmlLock);
                if (urls.TryGetValue(url, out html) && html != Null) urls[url] = Null;
                Monitor.Exit(htmlLock);
                if (html != Null && html != null) html.onDeleteHtml();
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
        }
        /// <summary>
        /// 删除 HTML 文件处理
        /// </summary>
        private void onDeleteHtml()
        {
            Searcher.SearchTaskQueue.Add(new Queue.Delete(new DataKey { Type = DataType.HtmlTitle, Id = Id }, Title));
            Searcher.SearchTaskQueue.Add(new Queue.Delete(new DataKey { Type = DataType.HtmlBodyText, Id = Id }, Text));
            HtmlImage.Free(ref Images);
        }
        /// <summary>
        /// Title 节点筛选器
        /// </summary>
        private static readonly AutoCSer.HtmlNode.Filter titleFilter = AutoCSer.HtmlNode.Filter.Get(@"/html[0]/head[0]/title[0]");
        /// <summary>
        /// BODY 节点筛选器
        /// </summary>
        private static readonly AutoCSer.HtmlNode.Filter bodyFilter = AutoCSer.HtmlNode.Filter.Get(@"/html[0]/body[0]");
        /// <summary>
        /// 获取 HTML 信息
        /// </summary>
        /// <param name="htmlFile">HTML 文件</param>
        /// <returns>HTML 信息</returns>
        private static Html getHtml(FileInfo htmlFile)
        {
            AutoCSer.HtmlNode.Node htmlNode = new AutoCSer.HtmlNode.Node(File.ReadAllText(htmlFile.FullName, Encoding.UTF8));
            foreach (AutoCSer.HtmlNode.Node bodyNode in bodyFilter.Get(htmlNode))
            {
                foreach (AutoCSer.HtmlNode.Node titleNode in titleFilter.Get(htmlNode))
                {
                    string title, bodyText;
                    if (!string.IsNullOrEmpty(title = titleNode.Text) && !string.IsNullOrEmpty(bodyText = bodyNode.Text))
                    {
                        LeftArray<HtmlImage> images = default(LeftArray<HtmlImage>);
                        foreach (AutoCSer.HtmlNode.Node imgNode in bodyNode.GetNodesByTagName("img"))
                        {
                            string alt = imgNode["alt"], src = imgNode["src"] ?? imgNode["@src"];
                            if (!string.IsNullOrEmpty(alt) && !string.IsNullOrEmpty(src)) images.Add(new HtmlImage { Url = src, Title = alt });
                        }
                        return new Html { Title = title, Text = bodyText, Images = images.ToArray() };
                    }
                    break;
                }
                break;
            }
            return null;
        }
        static Html()
        {
            Cache = new SegmentArray<Html>(8);
            int urlIndex = AutoCSer.Web.Config.Search.HtmlPath.Length;
            foreach (FileInfo htmlFile in new DirectoryInfo(AutoCSer.Web.Config.Search.HtmlPath).GetFiles("*.html", SearchOption.AllDirectories))
            {
                Html html = getHtml(htmlFile);
                if (html != null)
                {
                    html.Url = htmlFile.FullName.Substring(urlIndex).Replace('\\', '/');
                    urls.Add(html.Url.FileNameToLower(), html);
                    html.Id = Cache.GetIndex(html);
                    Searcher.SearchTaskQueue.Add(new Queue.Append(new DataKey { Id = html.Id, Type = DataType.HtmlTitle }, html.Title));
                    Searcher.SearchTaskQueue.Add(new Queue.Append(new DataKey { Id = html.Id, Type = DataType.HtmlBodyText }, html.Text));
                    foreach (HtmlImage image in html.Images)
                    {
                        image.GetIndex(html.Id);
                        Searcher.SearchTaskQueue.Add(new Queue.Append(new DataKey { Id = image.Id, Type = DataType.HtmlImage }, image.Title));
                    }
                }
            }

            htmlWatcher = new FileSystemWatcher(AutoCSer.Web.Config.Search.HtmlPath, "*.html");
            htmlWatcher.IncludeSubdirectories = false;
            htmlWatcher.EnableRaisingEvents = true;
            htmlWatcher.Created += onCreatedHtml;
            htmlWatcher.Deleted += onDeleteHtml;
        }
    }
}
