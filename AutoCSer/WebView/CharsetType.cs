using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// 字符集类型
    /// </summary>
    internal enum CharsetType
    {
        /// <summary>
        /// UTF-8
        /// </summary>
        [CharsetTypeAttribute(CharsetString = "UTF-8")]
        Utf8,
        /// <summary>
        /// GB2312
        /// </summary>
        [CharsetTypeAttribute(CharsetString = "GB2312")]
        Gb2312,
    }
    /// <summary>
    /// 字符集类型属性
    /// </summary>
    internal sealed class CharsetTypeAttribute : Attribute
    {
        /// <summary>
        /// 字符串表示
        /// </summary>
        public string CharsetString;

        /// <summary>
        /// 字符集类型名称集合
        /// </summary>
        private static readonly CharsetTypeAttribute[] charsetTypes = EnumAttribute<CharsetType, CharsetTypeAttribute>.GetAttributes();
        /// <summary>
        /// 获取字符集代码
        /// </summary>
        /// <returns>字符集代码</returns>
        public static string GetHtml(CharsetType type)
        {
            int typeIndex = (int)type;
            if (typeIndex >= charsetTypes.Length) typeIndex = -1;
            string html = string.Empty;
            if (typeIndex >= 0) html = @"<meta http-equiv=""content-type"" content=""text/html; charset=" + charsetTypes[typeIndex].CharsetString + @""">
";
            return html;
        }
    }
}
