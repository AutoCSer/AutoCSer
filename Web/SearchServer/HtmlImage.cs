using System;

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
    }
}
