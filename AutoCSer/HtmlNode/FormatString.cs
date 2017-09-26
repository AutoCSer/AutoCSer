using System;
using System.Runtime.InteropServices;
using System.Web;

namespace AutoCSer.HtmlNode
{
    /// <summary>
    /// HTML 编码与文本值
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct FormatString
    {
        /// <summary>
        /// 编码过的 HTML 编码
        /// </summary>
        public SubString FormatHtml;
        /// <summary>
        /// HTML 编码
        /// </summary>
        public SubString Html
        {
            get
            {
                if (FormatHtml.String == null && FormatText.String != null) FormatHtml = HttpUtility.HtmlEncode(FormatText);
                return FormatHtml;
            }
            set
            {
                FormatHtml = value;
                FormatText.SetNull();
            }
        }
        /// <summary>
        /// 编码过的 HTML 文本值
        /// </summary>
        public SubString FormatText;
        /// <summary>
        /// HTML 文本值
        /// </summary>
        public SubString Text
        {
            get
            {
                if (FormatText.String == null && FormatHtml.String != null) FormatText = HttpUtility.HtmlDecode(FormatHtml);
                return FormatText;
            }
            set
            {
                FormatText = value;
                FormatHtml.SetNull();
            }
        }
    }
}
