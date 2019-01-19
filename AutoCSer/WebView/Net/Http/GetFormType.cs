using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 获取请求表单数据回调类型
    /// </summary>
    internal enum GetFormType : byte
    {
        /// <summary>
        /// WEB 调用
        /// </summary>
        Call,
        /// <summary>
        /// WEB 异步调用
        /// </summary>
        CallAsynchronous,
        /// <summary>
        /// Ajax 调用
        /// </summary>
        Ajax,
        /// <summary>
        /// 公用 AJAX 调用
        /// </summary>
        PubAjax,
    }
}
