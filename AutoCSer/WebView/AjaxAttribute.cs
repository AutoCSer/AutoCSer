using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// AJAX 调用函数配置
    /// </summary>
    public sealed partial class AjaxAttribute : Attribute
    {
        /// <summary>
        /// 默认为 false 表示代码生成选择所有函数，否则仅选择申明了 AutoCSer.WebView.AjaxAttribute 的函数，有效域为当前 class。
        /// </summary>
        public bool IsAttribute;
        /// <summary>
        /// 指定是否搜索该成员的继承链以查找这些特性，参考System.Reflection.MemberInfo.GetCustomAttributes(bool inherit)，有效域为当前 class。
        /// </summary>
        public bool IsBaseTypeAttribute;
        /// <summary>
        /// 默认为 false 表示不生成 TypeScript 调用代理。
        /// </summary>
        public bool IsExportTypeScript;
        /// <summary>
        /// HTTP Body 接收数据的最大字节数，默认为 4MB。
        /// </summary>
        public int MaxPostDataSize = 4 << 20;
        /// <summary>
        /// 接收 HTTP Body 数据内存缓冲区的最大字节数，默认为 128KB，超出限定则使用文件储存方式。
        /// </summary>
        public SubBuffer.Size MaxMemoryStreamSize = SubBuffer.Size.Kilobyte128;
    }
}
