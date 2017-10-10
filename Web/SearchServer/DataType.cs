using System;

namespace AutoCSer.Web.SearchServer
{
    /// <summary>
    /// 搜索数据类型
    /// </summary>
    [Flags]
    public enum DataType : uint
    {
        /// <summary>
        /// HTML 标题
        /// </summary>
        HtmlTitle = 1,
        /// <summary>
        /// HTML Body 文本
        /// </summary>
        HtmlBodyText = 2,
        /// <summary>
        /// HTML 图片 alt
        /// </summary>
        HtmlImage = 4
    }
}
