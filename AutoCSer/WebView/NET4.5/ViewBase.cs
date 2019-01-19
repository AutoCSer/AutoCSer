using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.WebView
{
    /// <summary>
    /// WEB 页面视图
    /// </summary>
    public unsafe abstract partial class ViewBase// : INotifyCompletion
    {
        /// <summary>
        /// 是否存在 await 函数
        /// </summary>
        internal bool IsAwaitMethod;
        ///// <summary>
        ///// 异步回调
        ///// </summary>
        //private Action continuation;
        ///// <summary>
        ///// 完成状态
        ///// </summary>
        //public bool IsCompleted { get { return false; } }
        ///// <summary>
        ///// 获取 await
        ///// </summary>
        ///// <returns></returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //public ViewBase GetAwaiter()
        //{
        //    return this;
        //}
        ///// <summary>
        ///// 获取返回值
        ///// </summary>
        ///// <returns></returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //public bool GetResult()
        //{
        //    return asyncReturn;
        //}
        ///// <summary>
        ///// 设置异步回调
        ///// </summary>
        ///// <param name="continuation"></param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //public void OnCompleted(Action continuation)
        //{
        //    if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        //}
        ///// <summary>
        ///// 异步回调返回值
        ///// </summary>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //protected void asyncCallback()
        //{
        //    if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) continuation();
        //}
#pragma warning disable 1998
        /// <summary>
        /// HTTP响应输出处理
        /// </summary>
        /// <param name="response">页面输出</param>
        /// <returns></returns>
        protected virtual async void pageAsync(Response response)
        {
            try
            {
                DomainServer.RegisterServer.TcpServer.Log.Add(Log.LogType.Error, GetType().FullName + " pageAsync error");
            }
            catch { }
        }
        /// <summary>
        /// AJAX响应输出处理
        /// </summary>
        /// <param name="js">JS输出流</param>
        /// <returns></returns>
        protected virtual async void ajaxAsync(CharStream js)
        {
            try
            {
                DomainServer.RegisterServer.TcpServer.Log.Add(Log.LogType.Error, GetType().FullName + " ajaxAsync error");
            }
            catch { }
        }
#pragma warning restore 1998
        /// <summary>
        /// 添加错误日志
        /// </summary>
        /// <param name="error"></param>
        protected void addLog(Exception error)
        {
            try
            {
                DomainServer.RegisterServer.TcpServer.Log.Add(Log.LogType.Error, error);
            }
            catch { }
        }
        /// <summary>
        /// 异步等待事件
        /// </summary>
        internal AutoCSer.Threading.AutoWaitHandle AsyncWaitHandle;
        /// <summary>
        /// 设置异步与 await 函数状态
        /// </summary>
        /// <param name="isAsynchronous"></param>
        /// <param name="isAwaitMethod"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetAsynchronousAwaitMethod(bool isAsynchronous, bool isAwaitMethod)
        {
            IsAsynchronous = isAsynchronous;
            IsAwaitMethod = isAwaitMethod;
            if (isAwaitMethod) AsyncWaitHandle.Set(0);
        }
        /// <summary>
        /// 仅用于代码生成调用，请勿自行调用
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void asyncEnd()
        {
            AsyncWaitHandle.Set();
        }
        /// <summary>
        /// 页面输出
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="encodeBuffer"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool ResponsePage(ref byte* buffer, ref byte* encodeBuffer)
        {
            if (ResponseStream == null) ResponseStream = new UnmanagedStream(null, 0);
            ResponseStream.Reset(buffer = AutoCSer.UnmanagedPool.Default.Get(), AutoCSer.UnmanagedPool.DefaultSize);
            using (ResponseStream)
            {
                if (EncodeStream == null) EncodeStream = new UnmanagedStream(null, 0);
                EncodeStream.Reset(encodeBuffer = AutoCSer.UnmanagedPool.Default.Get(), AutoCSer.UnmanagedPool.DefaultSize);
                using (EncodeStream)
                {
                    Response bodyResponse = new Response { Stream = ResponseStream, EncodeStream = EncodeStream, Encoding = DomainServer.ResponseEncoding };
                    if (IsAwaitMethod)
                    {
                        pageAsync(bodyResponse);
                        AsyncWaitHandle.Wait();
                        if (asyncReturn)
                        {
                            asyncReturn = false;
                            HttpResponse.SetBody(ResponseStream);
                            return true;
                        }
                    }
                    else
                    {
                        if (page(ref bodyResponse))
                        {
                            HttpResponse.SetBody(ResponseStream);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// AJAX 视图输出
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ResponseAjax(ref byte* buffer)
        {
            AjaxStream.Reset(buffer = AutoCSer.UnmanagedPool.Default.Get(), AutoCSer.UnmanagedPool.DefaultSize);
            using (AjaxStream)
            {
                SetJsContentType();
                bool isCallBack = ResponseAjaxCallBack(AjaxStream);
                if (IsAwaitMethod)
                {
                    ajaxAsync(AjaxStream);
                    AsyncWaitHandle.Wait();
                }
                else ajax(AjaxStream);
                if (isCallBack) AjaxStream.Write(')');
                PageResponse.SetBody(AjaxStream, ref DomainServer.ResponseEncoding);
            }
        }
    }
}
