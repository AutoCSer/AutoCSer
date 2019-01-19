using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.WebView
{
    /// <summary>
    /// WEB 页面视图
    /// </summary>
    public unsafe abstract partial class ViewBase
    {
        /// <summary>
        /// HTTP响应输出处理
        /// </summary>
        /// <param name="response">页面输出</param>
        /// <returns></returns>
        protected virtual void pageAsync(Response response)
        {
            throw new InvalidOperationException(GetType().FullName);
        }
        /// <summary>
        /// AJAX响应输出处理
        /// </summary>
        /// <param name="js">JS输出流</param>
        /// <returns></returns>
        protected virtual void ajaxAsync(CharStream js)
        {
            throw new InvalidOperationException(GetType().FullName);
        }
        /// <summary>
        /// 添加错误日志
        /// </summary>
        /// <param name="error"></param>
        internal void addLog(Exception error)
        {
            throw new InvalidOperationException(GetType().FullName);
        }
        /// <summary>
        /// 仅用于代码生成调用，请勿自行调用
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void asyncEnd()
        {
            throw new InvalidOperationException(GetType().FullName);
        }
        /// <summary>
        /// 设置异步与 await 函数状态
        /// </summary>
        /// <param name="isAsynchronous"></param>
        /// <param name="isAwaitMethod"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetAsynchronousAwaitMethod(bool isAsynchronous, bool isAwaitMethod)
        {
            if (isAwaitMethod) throw new InvalidOperationException(GetType().FullName);
            IsAsynchronous = isAsynchronous;
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
                    if (page(ref bodyResponse))
                    {
                        HttpResponse.SetBody(ResponseStream);
                        return true;
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
                ajax(AjaxStream);
                if (isCallBack) AjaxStream.Write(')');
                PageResponse.SetBody(AjaxStream, ref DomainServer.ResponseEncoding);
            }
        }
    }
}
