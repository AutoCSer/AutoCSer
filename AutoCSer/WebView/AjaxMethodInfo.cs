using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// AJAX 调用信息
    /// </summary>
    public sealed class AjaxMethodInfo
    {
        /// <summary>
        /// 函数编号
        /// </summary>
        public int MethodIndex;
        /// <summary>
        /// 最大接收数据字节数
        /// </summary>
        public int MaxPostDataSize;
        /// <summary>
        /// 内存流最大字节数
        /// </summary>
        public SubBuffer.Size MaxMemoryStreamSize;
        /// <summary>
        /// 是否视图页面
        /// </summary>
        public bool IsViewPage;
        /// <summary>
        /// 是否只接受POST请求
        /// </summary>
        public bool IsPost;
        /// <summary>
        /// 是否判断来源页合法
        /// </summary>
        public bool IsReferer;
        /// <summary>
        /// 是否异步方法
        /// </summary>
        public bool IsAsynchronous;
    }
}
