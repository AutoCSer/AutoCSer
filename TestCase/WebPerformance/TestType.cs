using System;

namespace AutoCSer.TestCase.WebPerformance
{
    /// <summary>
    /// 测试类型
    /// </summary>
    internal enum TestType : byte
    {
        /// <summary>
        /// WEB 同步调用
        /// </summary>
        WebCall,
        /// <summary>
        /// WEB 异步调用
        /// </summary>
        WebCallAsynchronous,
        /// <summary>
        /// WEB 视图页面 HTML 同步调用
        /// </summary>
        WebView,
        /// <summary>
        /// WEB 视图页面 HTML 异步调用
        /// </summary>
        WebViewAsynchronous,
        /// <summary>
        /// WEB 视图 AJAX 数据同步调用
        /// </summary>
        WebViewAjax,
        /// <summary>
        /// WEB 视图 AJAX 数据异步调用
        /// </summary>
        WebViewAjaxAsynchronous,
        /// <summary>
        /// AJAX 同步调用
        /// </summary>
        Ajax,
        /// <summary>
        /// AJAX 异步调用
        /// </summary>
        AjaxAsynchronous,
        /// <summary>
        /// 随机测试
        /// </summary>
        Random,
    }
}
