using System;
using System.Threading;

namespace AutoCSer.Web.SearchServer
{
    /// <summary>
    /// HTML 图片信息
    /// </summary>
    internal sealed class HtmlImage
    {
        /// <summary>
        /// HTML 图片标识
        /// </summary>
        public int Id;
        /// <summary>
        /// HTML 标识
        /// </summary>
        public int HtmlId;
        /// <summary>
        /// 图片地址
        /// </summary>
        public string Url;
        /// <summary>
        /// 图片标题
        /// </summary>
        public string Title;

        /// <summary>
        /// HTML 图片信息缓存
        /// </summary>
        internal static SegmentArray<HtmlImage> Cache = new SegmentArray<HtmlImage>(8);
        /// <summary>
        /// 被释放的图片信息头结点
        /// </summary>
        private static HtmlImage freeImageHead;
        /// <summary>
        /// 被释放的图片信息尾节点
        /// </summary>
        private static HtmlImage freeImageEnd;
        /// <summary>
        /// 释放的图片信息访问锁
        /// </summary>
        private static readonly object freeImageLock = new object();
        /// <summary>
        /// 释放图片信息集合
        /// </summary>
        /// <param name="images">图片信息集合</param>
        internal static void Free(ref HtmlImage[] images)
        {
            int headIndex = 0;
            Monitor.Enter(freeImageLock);
            try
            {
                if (images.Length != 0)
                {
                    foreach (HtmlImage image in images)
                    {
                        Searcher.SearchTaskQueue.Add(new Queue.Delete(new DataKey { Type = DataType.HtmlImage, Id = image.Id }, image.Title));
                        image.Title = image.Url = null;
                        image.HtmlId = headIndex;
                        headIndex = image.Id;
                    }
                    if (freeImageEnd == null) freeImageHead = images[images.Length - 1];
                    else freeImageEnd.HtmlId = headIndex;
                    freeImageEnd = images[0];
                    images = NullValue<HtmlImage>.Array;
                }
            }
            finally { Monitor.Exit(freeImageLock); }
        }
        /// <summary>
        /// 获取数组索引
        /// </summary>
        /// <param name="htmlId">HTML 标识</param>
        internal void GetIndex(int htmlId)
        {
            HtmlId = htmlId;
            Monitor.Enter(freeImageLock);
            if (freeImageEnd == null)
            {
                Monitor.Exit(freeImageLock);
                Id = Cache.GetIndex(this);
            }
            else
            {
                Id = freeImageHead.Id;
                if (freeImageHead.HtmlId == 0) freeImageEnd = null;
                else freeImageHead = Cache[freeImageHead.HtmlId];
                Monitor.Exit(freeImageLock);
                Cache[Id] = this;
            }
        }
    }
}
