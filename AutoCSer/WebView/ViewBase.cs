using System;
using System.Threading;
using System.IO;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.WebView
{
    /// <summary>
    /// WEB 页面视图
    /// </summary>
    public unsafe abstract partial class ViewBase : Page
    {
        ///// <summary>
        ///// 是否默认输出 ASCII
        ///// </summary>
        //private static readonly bool isDefaultResponseAscii;
        /// <summary>
        /// 是否异步
        /// </summary>
        internal bool IsAsynchronous;
        /// <summary>
        /// 异步返回值
        /// </summary>
        protected bool asyncReturn;
        /// <summary>
        /// 临时逻辑变量
        /// </summary>
        protected bool _if_;
        /// <summary>
        /// 当前循环索引
        /// </summary>
        protected int _loopIndex_;
        /// <summary>
        /// 当前循环数量
        /// </summary>
        protected int _loopCount_;
        /// <summary>
        /// 页面输出数据流
        /// </summary>
        internal UnmanagedStream ResponseStream;
        /// <summary>
        /// HTML 编码输出数据流
        /// </summary>
        internal UnmanagedStream EncodeStream;
        /// <summary>
        /// AJAX 输出数据流
        /// </summary>
        internal readonly CharStream AjaxStream = new CharStream(null, 0);
        /// <summary>
        /// JSON序列化是否使用默认模式(非视图模式模式)
        /// </summary>
        protected virtual bool isDefaultToJson
        {
            get { return false; }
        }
        /// <summary>
        /// 页面关键字
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        public string ViewKeywords { get; protected set; }
        /// <summary>
        /// 页面关键字
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        public string ViewMetaKeywords
        {
            get
            {
                if (!string.IsNullOrEmpty(ViewKeywords) && EncodeStream != null && EncodeStream.Data.Data != null && Socket.HttpHeader.IsSearchEngine != 0)
                {
                    Response bodyResponse = new Response { Stream = ResponseStream, EncodeStream = EncodeStream, Encoding = DomainServer.ResponseEncoding };
                    bodyResponse.Write(@"<meta name=""keywords"" content=""");
                    bodyResponse.WriteHtml(ViewKeywords);
                    bodyResponse.Write(@""" />");
                }
                return null;
            }
        }
        /// <summary>
        /// 页面描述
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        public string ViewDescription { get; protected set; }
        /// <summary>
        /// 页面描述
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        public string ViewMetaDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(ViewDescription) && EncodeStream != null && EncodeStream.Data.Data != null && Socket.HttpHeader.IsSearchEngine != 0)
                {
                    Response bodyResponse = new Response { Stream = ResponseStream, EncodeStream = EncodeStream, Encoding = DomainServer.ResponseEncoding };
                    bodyResponse.Write(@"<meta name=""description"" content=""");
                    bodyResponse.WriteHtml(ViewDescription);
                    bodyResponse.Write(@""" />");
                }
                return null;
            }
        }
        /// <summary>
        /// 视图错误重定向路径
        /// </summary>
        internal ViewLocationPath LocationPath;
        /// <summary>
        /// 视图错误重定向路径
        /// </summary>
        internal bool IsLocationPath;
        /// <summary>
        /// 404 重定向路径
        /// </summary>
        protected virtual string notFound404
        {
            get { return "/404.html"; }
        }
        /// <summary>
        /// 当前时间
        /// </summary>
        protected ServerTime serverTime
        {
            get { return new ServerTime { Now = Date.NowTime.Set() }; }
        }
        /// <summary>
        /// HTTP请求头部处理
        /// </summary>
        /// <returns>是否成功</returns>
        protected virtual bool loadHeader()
        {
            return true;
        }
        /// <summary>
        /// WEB视图加载
        /// </summary>
        /// <returns>是否成功</returns>
        protected virtual bool loadView()
        {
            return true;
        }
        /// <summary>
        /// HTTP响应输出处理
        /// </summary>
        /// <param name="response">页面输出</param>
        /// <returns>是否成功</returns>
        protected virtual bool page(ref Response response)
        {
            throw new InvalidOperationException(GetType().FullName);
        }
        /// <summary>
        /// AJAX响应输出处理
        /// </summary>
        /// <param name="js">JS输出流</param>
        protected virtual void ajax(CharStream js)
        {
            throw new InvalidOperationException(GetType().FullName);
        }
        /// <summary>
        /// 设置重定向路径
        /// </summary>
        /// <param name="locationPath">重定向路径</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void setLocationPath(string locationPath)
        {
            if (locationPath != null)
            {
                LocationPath.SetLocation(locationPath);
                IsLocationPath = true;
            }
        }
        /// <summary>
        /// 设置错误路径
        /// </summary>
        /// <param name="errorPath">错误重定向路径</param>
        /// <param name="returnPath">返回路径</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void setErrorPath(string errorPath, string returnPath)
        {
            if (errorPath != null)
            {
                LocationPath.SetError(errorPath, returnPath);
                IsLocationPath = true;
            }
        }
        /// <summary>
        /// 加载HTML数据
        /// </summary>
        /// <param name="fileName">HTML文件</param>
        /// <param name="htmlCount">HTML片段数量验证</param>
        /// <param name="htmlLock">HTML 数据创建访问锁</param>
        /// <param name="htmls">HTML 数据</param>
        /// <returns></returns>
        internal byte[][] LoadHtml(string fileName, int htmlCount, object htmlLock, ref byte[][] htmls)
        {
            fileName = DomainServer.WorkPath + fileName;
            Monitor.Enter(htmlLock);
            try
            {
                if (htmls == null)
                {
                    if (File.Exists(fileName))
                    {
                        ViewTreeBuilder treeBuilder = new ViewTreeBuilder(File.ReadAllText(fileName, DomainServer.ResponseEncoding.Encoding));
                        if (treeBuilder.Htmls.Count == htmlCount)
                        {
                            return htmls = treeBuilder.HtmlArray.getArray(value => DomainServer.ResponseEncoding.GetBytesNotNull(value));
                        }
                        DomainServer.RegisterServer.TcpServer.Log.Add(Log.LogType.Error, "HTML模版文件不匹配 " + fileName);
                    }
                    else DomainServer.RegisterServer.TcpServer.Log.Add(Log.LogType.Error, "没有找到HTML模版文件 " + fileName);
                }
            }
            catch (Exception error)
            {
                DomainServer.RegisterServer.TcpServer.Log.Add(Log.LogType.Error, error, fileName);
            }
            finally
            {
                if (htmls == null) htmls = NullValue<byte[]>.Array;
                Monitor.Exit(htmlLock);
            }
            return htmls.Length == 0 ? null : htmls;
        }
        /// <summary>
        /// 输出视图错误重定向路径
        /// </summary>
        /// <param name="socket"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ResponseLocationPath(AutoCSer.Net.Http.SocketBase socket)
        {
            if (LocationPath.ReturnPath == null) HttpResponse.SetLocation(socket.HttpHeader, LocationPath.ErrorPath, Net.Http.ResponseState.Found302);
            else
            {
                SubArray<byte> data = default(SubArray<byte>);
                socket.HttpHeader.SetResponseLocationRange(LocationPath.ErrorPath.Length + 5 + LocationPath.ReturnPath.Length, ref data);
                fixed (byte* dataFixed = data.Array)
                {
                    byte* write = dataFixed + data.Start;
                    fixed (char* pathFixed = LocationPath.ErrorPath) AutoCSer.Extension.StringExtension.WriteBytes(pathFixed, LocationPath.ErrorPath.Length, write);
                    *(write += LocationPath.ErrorPath.Length) = LocationPath.ErrorPath.IndexOf('#') >= 0 ? (byte)'&' : (byte)'#';
                    *(long*)(++write) = 'u' + ('r' << 16) + ((long)'l' << 32) + ((long)'=' << 48);
                    fixed (char* pathFixed = LocationPath.ReturnPath) AutoCSer.Extension.StringExtension.WriteBytes(pathFixed, LocationPath.ReturnPath.Length, write + sizeof(long));
                }
                HttpResponse.SetLocation(ref data, Net.Http.ResponseState.Found302);
            }
        }
        /// <summary>
        /// 输出视图错误重定向路径
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ResponseLocationPathAjax(ref byte* buffer)
        {
            AjaxStream.Reset(buffer = AutoCSer.UnmanagedPool.Default.Get(), AutoCSer.UnmanagedPool.DefaultSize);
            using (AjaxStream)
            {
                SetJsContentType();
                bool isCallBack = ResponseAjaxCallBack(AjaxStream);
                if (LocationPath.ErrorPath == null)
                {
                    AjaxStream.WriteNotNull(@"{""LocationPath"":");
                    AjaxStream.WriteJson(LocationPath.LocationPath ?? notFound404);
                }
                else
                {
                    AjaxStream.WriteNotNull(@"{""ErrorPath"":");
                    AjaxStream.WriteJson(LocationPath.ErrorPath);
                    if (LocationPath.ReturnPath != null)
                    {
                        AjaxStream.WriteNotNull(@",""ReturnPath"":");
                        AjaxStream.WriteJson(LocationPath.ReturnPath);
                    }
                }
                AjaxStream.Write('}');
                if (isCallBack) AjaxStream.Write(')');
                PageResponse.SetBody(AjaxStream, ref DomainServer.ResponseEncoding);
            }
        }
    }
}
