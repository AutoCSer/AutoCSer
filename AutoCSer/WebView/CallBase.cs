using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.IO;
using AutoCSer.Extension;

namespace AutoCSer.WebView
{
    /// <summary>
    /// HTTP 调用
    /// </summary>
    public abstract unsafe class CallBase : Page
    {
        /// <summary>
        /// AJAX 回调处理
        /// </summary>
        protected internal sealed class AjaxCallback
        {
            /// <summary>
            /// AJAX 调用
            /// </summary>
            public Ajax Ajax;
            /// <summary>
            /// 回调处理
            /// </summary>
            /// <param name="value"></param>
            public void Callback(AutoCSer.Net.TcpServer.ReturnValue value)
            {
                Ajax ajax = System.Threading.Interlocked.Exchange(ref Ajax, null);
                if (ajax != null)
                {
                    if (value.Type == AutoCSer.Net.TcpServer.ReturnType.Success) ajax.Response();
                    else ajax.ServerError500();
                }
            }
        }
        /// <summary>
        /// AJAX 回调处理
        /// </summary>
        /// <typeparam name="ajaxType"></typeparam>
        protected sealed class AjaxCallbackPool<ajaxType>
            where ajaxType : Ajax<ajaxType>
        {
            /// <summary>
            /// AJAX 调用
            /// </summary>
            public ajaxType Ajax;
            /// <summary>
            /// 回调处理
            /// </summary>
            /// <param name="value"></param>
            public void Callback(AutoCSer.Net.TcpServer.ReturnValue value)
            {
                ajaxType ajax = System.Threading.Interlocked.Exchange(ref Ajax, null);
                if (ajax != null)
                {
                    if (value.Type == AutoCSer.Net.TcpServer.ReturnType.Success) ajax.Response();
                    else ajax.ServerError500();
                }
            }
        }
        /// <summary>
        /// AJAX 回调处理
        /// </summary>
        /// <typeparam name="paramtereType"></typeparam>
        protected class AjaxCallback<paramtereType>
            where paramtereType : struct
        {
            /// <summary>
            /// AJAX 调用
            /// </summary>
            public Ajax Ajax;
            /// <summary>
            /// 输出参数
            /// </summary>
            public paramtereType Parameter;
            /// <summary>
            /// 回调处理
            /// </summary>
            /// <param name="value"></param>
            public void Callback(AutoCSer.Net.TcpServer.ReturnValue value)
            {
                Ajax ajax = System.Threading.Interlocked.Exchange(ref Ajax, null);
                if (ajax != null)
                {
                    if (value.Type == AutoCSer.Net.TcpServer.ReturnType.Success) ajax.Response(ref Parameter);
                    else ajax.ServerError500();
                }
            }
            /// <summary>
            /// 回调处理
            /// </summary>
            /// <param name="ajax"></param>
            /// <param name="returnType"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            protected void response(Ajax ajax, AutoCSer.Net.TcpServer.ReturnType returnType)
            {
                if (returnType == AutoCSer.Net.TcpServer.ReturnType.Success) ajax.Response(ref Parameter);
                else ajax.ServerError500();
            }
        }
        /// <summary>
        /// AJAX 回调处理
        /// </summary>
        /// <typeparam name="ajaxType"></typeparam>
        /// <typeparam name="paramtereType"></typeparam>
        protected class AjaxCallbackPool<ajaxType, paramtereType>
            where ajaxType : Ajax<ajaxType>
            where paramtereType : struct
        {
            /// <summary>
            /// AJAX 调用
            /// </summary>
            public ajaxType Ajax;
            /// <summary>
            /// 输出参数
            /// </summary>
            public paramtereType Parameter;
            /// <summary>
            /// 回调处理
            /// </summary>
            /// <param name="value"></param>
            public void Callback(AutoCSer.Net.TcpServer.ReturnValue value)
            {
                ajaxType ajax = System.Threading.Interlocked.Exchange(ref Ajax, null);
                if (ajax != null)
                {
                    if (value.Type == AutoCSer.Net.TcpServer.ReturnType.Success) ajax.Response(ref Parameter);
                    else ajax.ServerError500();
                }
            }
            /// <summary>
            /// 回调处理
            /// </summary>
            /// <param name="ajax"></param>
            /// <param name="returnType"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            protected void response(ajaxType ajax, AutoCSer.Net.TcpServer.ReturnType returnType)
            {
                if (returnType == AutoCSer.Net.TcpServer.ReturnType.Success) ajax.Response(ref Parameter);
                else ajax.ServerError500();
            }
        }
        /// <summary>
        /// HTTP 调用函数信息
        /// </summary>
        internal AutoCSer.WebView.CallMethodInfo MethodInfo;
        /// <summary>
        /// 获取 HTTP 调用函数编号
        /// </summary>
        public int CallMethodIndex
        {
            get { return MethodInfo.MethodIndex; }
        }
        /// <summary>
        /// 内存流最大字节数
        /// </summary>
        internal override SubBuffer.Size MaxMemoryStreamSize { get { return MethodInfo.MaxMemoryStreamSize; } }
        /// <summary>
        /// WEB 调用输出
        /// </summary>
        internal CallResponse CallResponse;
        /// <summary>
        /// 页面输出数据流
        /// </summary>
        internal UnmanagedStream ResponseStream;
        /// <summary>
        /// 获取页面输出数据流
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        internal UnmanagedStream GetResponseStream(byte* buffer, int size)
        {
            UnmanagedStream responseStream = Interlocked.Exchange(ref ResponseStream, null);
            if (responseStream == null) responseStream = new UnmanagedStream(buffer, size);
            else responseStream.Reset(buffer, size);
            CallResponse.Set(responseStream, ref DomainServer.ResponseEncoding);
            return responseStream;
        }
        /// <summary>
        /// AJAX 调用
        /// </summary>
        /// <param name="callIndex"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        protected virtual bool callAjax(int callIndex, AutoCSer.WebView.AjaxBase page)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// AJAX 调用
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CallAjax(AutoCSer.WebView.AjaxBase page)
        {
            return callAjax(page.MethodInfo.MethodIndex, page);
        }
        /// <summary>
        /// 重定向
        /// </summary>
        /// <param name="path">重定向地址</param>
        /// <param name="is302">是否临时重定向</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void location(string path, bool is302 = true)
        {
            if (SocketIdentity == Socket.Identity) HttpResponse.SetLocation(Socket.HttpHeader, path ?? string.Empty, is302 ? AutoCSer.Net.Http.ResponseState.Found302 : AutoCSer.Net.Http.ResponseState.MovedPermanently301);
        }
        /// <summary>
        /// 重定向
        /// </summary>
        /// <param name="path">重定向地址</param>
        /// <param name="is302">是否临时重定向</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void location(byte[] path, bool is302 = true)
        {
            HttpResponse.SetLocation(path, is302 ? AutoCSer.Net.Http.ResponseState.Found302 : AutoCSer.Net.Http.ResponseState.MovedPermanently301);
        }
        /// <summary>
        /// 静态文件版本号重定向
        /// </summary>
        protected sealed class LocationVersion
        {
            /// <summary>
            /// 重定向地址
            /// </summary>
            private string location;
            /// <summary>
            /// 重定向地址
            /// </summary>
            private byte[] locationData;
            /// <summary>
            /// 资源版本号
            /// </summary>
            private string version;
            /// <summary>
            /// 资源版本号访问锁
            /// </summary>
            private readonly object versionLock = new object();
            /// <summary>
            /// 静态文件版本号重定向
            /// </summary>
            /// <param name="location">重定向地址</param>
            public LocationVersion(string location)
            {
                locationData = System.Text.Encoding.ASCII.GetBytes(this.location = location);
            }
            /// <summary>
            /// 获取重定向地址
            /// </summary>
            /// <param name="call">HTTP 调用</param>
            /// <param name="is302">是否临时重定向</param>
            public void Location(CallBase call, bool is302 = true)
            {
                string version = call.DomainServer.StaticFileVersion;
                if (!object.ReferenceEquals(version, this.version))
                {
                    Monitor.Enter(versionLock);
                    try
                    {
                        if (!object.ReferenceEquals(version, this.version))
                        {
                            locationData = System.Text.Encoding.ASCII.GetBytes(location + "?" + AutoCSer.Net.Http.Header.VersionNameChar.ToString() + "=" + version);
                            this.version = version;
                        }
                    }
                    finally { Monitor.Exit(versionLock); }
                }
                call.location(locationData, is302);
            }
        }
    }
}
