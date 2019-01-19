using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// HTTP 调用函数信息
    /// </summary>
    public sealed class CallMethodInfo
    {
        /// <summary>
        /// 函数编号
        /// </summary>
        public int MethodIndex;
        /// <summary>
        /// HTTP Body 接收数据的最大字节数
        /// </summary>
        public int MaxPostDataSize;
        /// <summary>
        /// 接收 HTTP Body 数据内存缓冲区的最大字节数，超出限定则使用文件储存方式。
        /// </summary>
        public SubBuffer.Size MaxMemoryStreamSize;
        ///// <summary>
        ///// 是否异步请求
        ///// </summary>
        //public bool IsAsynchronous;
        ///// <summary>
        ///// 是否使用对象池
        ///// </summary>
        //public bool IsPool;
        /// <summary>
        /// 是否仅支持POST请求
        /// </summary>
        public bool IsOnlyPost;
    }
}
