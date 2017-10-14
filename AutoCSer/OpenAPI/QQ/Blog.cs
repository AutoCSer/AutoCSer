using System;

namespace AutoCSer.OpenAPI.QQ
{
    /// <summary>
    /// 空间日志
    /// </summary>
    public partial class Blog : Form
    {
        /// <summary>
        /// 日志标题，纯文本，最大长度128个字节，UTF-8编码，必填项
        /// </summary>
        public string title;
        /// <summary>
        /// 日志内容。HTML格式字符串，最大长度100*1024个字节，UTF-8编码，必填项。注意：字符串中不允许包括以下特殊标签：html，head，body，script，input，frame，meta，form，applet，xml，textarea，base，link等
        /// </summary>
        public string content;
    }
}
