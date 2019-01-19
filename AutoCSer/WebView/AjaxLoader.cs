using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.WebView
{
    /// <summary>
    /// AJAX 调用加载
    /// </summary>
    /// <typeparam name="loaderType">AJAX调用加载类型</typeparam>
    [AutoCSer.WebView.ClearMember(IsIgnoreCurrent = true)]
    public abstract class AjaxLoader<loaderType> : CallAsynchronous<loaderType>
        where loaderType : AjaxLoader<loaderType>
    {
        /// <summary>
        /// AJAX调用
        /// </summary>
        protected void load()
        {
            try
            {
                AutoCSer.Net.Http.SocketBase socket = Socket;
                AutoCSer.Net.Http.Header header = Socket.HttpHeader;
                if ((header.Flag & Net.Http.HeaderFlag.IsSetAjaxCallName) != 0)
                {
#if MONO
                    AutoCSer.WebView.AjaxMethodInfo call = methods.Get(header.AjaxCallNameData);
#else
                    AutoCSer.WebView.AjaxMethodInfo call = DomainServer.WebConfigIgnoreCase ? methods.GetLower(header.AjaxCallNameData) : methods.Get(header.AjaxCallNameData);
#endif
                    if (call == null)
                    {
                        byte[] path = DomainServer.GetViewRewrite(header.AjaxCallNameData);
                        if (path.length() != 0) call = methods.GetNotEmpty(path);
                    }
                    if (call != null && (!call.IsReferer || header.IsReferer != 0))// || AutoCSer.Config.Pub.Default.IsDebug
                    {
                        if (call.IsViewPage)
                        {
                            if (header.Method == Net.Http.MethodType.GET)
                            {
                                loadView(call);
                                return;
                            }
                        }
                        else if ((header.Method == Net.Http.MethodType.POST || !call.IsPost))
                        {
                            if (header.ContentLength <= call.MaxPostDataSize) loadAjax(call);
                            else socket.ResponseErrorIdentity(AutoCSer.Net.Http.ResponseState.ServerError500);
                            return;
                        }
                    }
                }
                socket.ResponseErrorIdentity(AutoCSer.Net.Http.ResponseState.NotFound404);
            }
            finally { PushPool(); }
        }
        /// <summary>
        /// 加载页面视图
        /// </summary>
        /// <param name="ajaxInfo"></param>
        protected abstract void loadView(AutoCSer.WebView.AjaxMethodInfo ajaxInfo);
        /// <summary>
        /// 加载页面视图
        /// </summary>
        /// <param name="view">WEB视图接口</param>
        /// <param name="ajaxInfo"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void loadView<viewType>(viewType view, AutoCSer.WebView.AjaxMethodInfo ajaxInfo)
            where viewType : AutoCSer.WebView.View<viewType>
        {
            DomainServer.LoadAjax<viewType>(Socket, view);
        }
        /// <summary>
        /// 加载页面视图
        /// </summary>
        /// <param name="view">WEB视图接口</param>
        /// <param name="ajaxInfo"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void loadView(AutoCSer.WebView.View view, AutoCSer.WebView.AjaxMethodInfo ajaxInfo)
        {
            DomainServer.LoadAjax(Socket, view);
        }
        /// <summary>
        /// 加载页面视图
        /// </summary>
        /// <param name="view">WEB视图接口</param>
        /// <param name="ajaxInfo"></param>
        /// <param name="isAsynchronous"></param>
        /// <param name="isAwaitMethod"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void loadView(AutoCSer.WebView.View view, AutoCSer.WebView.AjaxMethodInfo ajaxInfo, bool isAsynchronous, bool isAwaitMethod)
        {
            view.SetAsynchronousAwaitMethod(isAsynchronous, isAwaitMethod);
            DomainServer.LoadAjax(Socket, view);
        }
        /// <summary>
        /// 加载页面视图[TRY]
        /// </summary>
        /// <param name="view">WEB视图接口</param>
        /// <param name="isAsynchronous">是否异步</param>
        /// <param name="isAwaitMethod">是否存在 await 函数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected static void setPage(AutoCSer.WebView.ViewBase view, bool isAsynchronous, bool isAwaitMethod)
        {
            view.SetAsynchronousAwaitMethod(isAsynchronous, isAwaitMethod);
        }
        /// <summary>
        /// 加载 AJAX 调用
        /// </summary>
        /// <param name="ajaxInfo"></param>
        protected abstract void loadAjax(AutoCSer.WebView.AjaxMethodInfo ajaxInfo);
        /// <summary>
        /// 加载 AJAX 调用
        /// </summary>
        /// <param name="ajax"></param>
        /// <param name="ajaxInfo"></param>
        protected void loadAjax<ajaxType>(ajaxType ajax, AutoCSer.WebView.AjaxMethodInfo ajaxInfo)
            where ajaxType : AutoCSer.WebView.Ajax<ajaxType>
        {
            AutoCSer.Net.Http.Header header = Socket.HttpHeader;
            ajax.LoadHeader(DomainServer, Socket, ajaxInfo.IsAsynchronous, ajax);
            if (header.Method == AutoCSer.Net.Http.MethodType.POST && header.ContentLength != 0)
            {
                ajax.SetLoader(this, ajaxInfo);
                Socket.GetForm(ajax, AutoCSer.Net.Http.GetFormType.Ajax);
                return;
            }
            if (this.callAjax(ajaxInfo.MethodIndex, ajax)) return;
            Socket.ResponseError(SocketIdentity, AutoCSer.Net.Http.ResponseState.ServerError500);
            ajax.PushPool();
        }
        /// <summary>
        /// 加载 AJAX 调用
        /// </summary>
        /// <param name="ajax"></param>
        /// <param name="ajaxInfo"></param>
        protected void loadAjax(AutoCSer.WebView.Ajax ajax, AutoCSer.WebView.AjaxMethodInfo ajaxInfo)
        {
            AutoCSer.Net.Http.Header header = Socket.HttpHeader;
            ajax.LoadHeader(DomainServer, Socket, ajaxInfo.IsAsynchronous);
            if (header.Method == AutoCSer.Net.Http.MethodType.POST && header.ContentLength != 0)
            {
                ajax.SetLoader(this, ajaxInfo);
                Socket.GetForm(ajax, AutoCSer.Net.Http.GetFormType.Ajax);
            }
            else if (!this.callAjax(ajaxInfo.MethodIndex, ajax)) Socket.ResponseError(SocketIdentity, AutoCSer.Net.Http.ResponseState.ServerError500);
        }
        /// <summary>
        /// AJAX 输出
        /// </summary>
        /// <typeparam name="ajaxType"></typeparam>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="ajax"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected static void responseAjax<ajaxType, valueType>(ajaxType ajax, ref valueType value)
            where ajaxType : AutoCSer.WebView.Ajax<ajaxType>
            where valueType : struct
        {
            ajax.Response(ref value);
        }
        /// <summary>
        /// AJAX 输出
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="ajax"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected static void responseAjax<valueType>(AutoCSer.WebView.Ajax ajax, ref valueType value)
            where valueType : struct
        {
            ajax.Response(ref value);
        }
        /// <summary>
        /// AJAX 输出
        /// </summary>
        /// <typeparam name="ajaxType"></typeparam>
        /// <param name="ajax"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected static void responseAjax<ajaxType>(ajaxType ajax)
            where ajaxType : AutoCSer.WebView.Ajax<ajaxType>
        {
            ajax.Response();
        }
        /// <summary>
        /// AJAX 输出
        /// </summary>
        /// <param name="ajax"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected static void responseAjax(AutoCSer.WebView.Ajax ajax)
        {
            ajax.Response();
        }
        /// <summary>
        /// 公用错误处理函数
        /// </summary>
        protected void pubError()
        {
            AutoCSer.Net.Http.Header header = Socket.HttpHeader;
            if (header.Method == AutoCSer.Net.Http.MethodType.POST && header.ContentLength != 0)
            {
                PubAjax ajax = PubAjax.Pop() ?? new PubAjax();
                ajax.LoadHeader(DomainServer, Socket, true, ajax);
                Socket.GetForm(ajax, AutoCSer.Net.Http.GetFormType.PubAjax);
                return;
            }
            else Socket.ResponseErrorIdentity(Net.Http.ResponseState.ServerError500);
        }
        
        /// <summary>
        /// AJAX 函数调用集合
        /// </summary>
        private static AutoCSer.StateSearcher.AsciiSearcher<AutoCSer.WebView.AjaxMethodInfo> methods;
        /// <summary>
        /// 设置 AJAX 函数调用集合
        /// </summary>
        /// <param name="names"></param>
        /// <param name="infos"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected static void setMethods(string[] names, AutoCSer.WebView.AjaxMethodInfo[] infos)
        {
            methods = new AutoCSer.StateSearcher.AsciiSearcher<AutoCSer.WebView.AjaxMethodInfo>(names, infos, true);
        }
    }
}
