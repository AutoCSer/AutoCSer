using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// WEB 函数配置
    /// </summary>
    public abstract class MethodAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        /// <summary>
        /// HTTP Body 接收数据的最大字节数，默认为 4MB。
        /// </summary>
        public int MaxPostDataSize = 4 << 20;
        /// <summary>
        /// 接收 HTTP Body 数据内存缓冲区的最大字节数，默认为 128KB，超出限定则使用文件储存方式。
        /// </summary>
        public SubBuffer.Size MaxMemoryStreamSize = SubBuffer.Size.Kilobyte128;
        /// <summary>
        /// AJAX 调用全名称，用于替换默认的调用全名称。
        /// </summary>
        public string FullName;
        /// <summary>
        /// WEB 调用函数名称，用于替换默认的函数名称。
        /// </summary>
        public string MethodName;
    }
}
