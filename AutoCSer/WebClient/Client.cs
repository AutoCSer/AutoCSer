using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.WebClient
{
    /// <summary>
    /// WebClient 包装
    /// </summary>
    public class Client : System.Net.WebClient
    {
        /// <summary>
        /// 默认浏览器参数
        /// </summary>
        public const string DefaultUserAgent = @"Mozilla/5.0 (Windows NT 5.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.93 Safari/537.36";//@"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1;)";
        /// <summary>
        /// 字符集标识
        /// </summary>
        public const string CharsetName = "charset=";
        /// <summary>
        /// POST
        /// </summary>
        public const string PostName = "POST";

        /// <summary>
        /// cookie状态
        /// </summary>
        public CookieContainer Cookies { get; private set; }
        /// <summary>
        /// HTTP请求
        /// </summary>
        private WebRequest webRequest;
        /// <summary>
        /// HTTP请求
        /// </summary>
        public HttpWebRequest HttpRequest
        {
            get
            {
                return webRequest == null ? null : webRequest as HttpWebRequest;
            }
        }
        /// <summary>
        /// 导入证书
        /// </summary>
        public X509Certificate Certificate;
        /// <summary>
        /// 浏览器参数
        /// </summary>
        public string UserAgent = DefaultUserAgent;
        /// <summary>
        /// 超时毫秒数
        /// </summary>
        public int TimeOut;
        /// <summary>
        /// 是否允许跳转
        /// </summary>
        public bool AllowAutoRedirect = true;
        /// <summary>
        /// 是否保持连接
        /// </summary>
        public bool KeepAlive = true;
        /// <summary>
        /// 获取最后一次操作是否发生重定向
        /// </summary>
        public bool IsRedirect
        {
            get
            {
                return webRequest != null && webRequest is HttpWebRequest
                    && webRequest.RequestUri.Equals((webRequest as HttpWebRequest).Address);
            }
        }
        /// <summary>
        /// 获取最后一次重定向地址
        /// </summary>
        public Uri RedirectUri
        {
            get
            {
                return IsRedirect ? (webRequest as HttpWebRequest).Address : null;
            }
        }
        /// <summary>
        /// HTTP回应压缩流处理
        /// </summary>
        private CompressionStream compressionStream
        {
            get
            {
                if (ResponseHeaders != null)
                {
                    string contentEncoding = ResponseHeaders[AutoCSer.Net.Http.HeaderName.ContentEncoding];
                    if (contentEncoding != null)
                    {
                        if (contentEncoding.Length == 4)
                        {
                            return contentEncoding == "gzip" ? CompressionStream.GZip : null;
                        }
                        return contentEncoding == "deflate" ? CompressionStream.Deflate : null;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// HTTP回应编码字符集
        /// </summary>
        public Encoding TextEncoding
        {
            get
            {
                if (ResponseHeaders != null)
                {
                    string contentType = ResponseHeaders[AutoCSer.Net.Http.HeaderName.ContentType];
                    if (contentType != null) return getEncoding(contentType);
                }
                return null;
            }
        }
        /// <summary>
        /// 获取重定向地址
        /// </summary>
        public string Location
        {
            get
            {
                WebResponse response = webRequest == null ? null : webRequest.GetResponse();
                return response == null ? null : response.Headers[HeaderName.Location];
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="cookies">cookie状态</param>
        /// <param name="proxy">代理</param>
        public Client(CookieContainer cookies = null, WebProxy proxy = null)
        {
            Credentials = new CredentialCache();
            Cookies = cookies == null ? new CookieContainer() : cookies;
            Credentials = CredentialCache.DefaultCredentials;

            Proxy = proxy;
            //string header = client.ResponseHeaders[web.header.SetCookie];
            //client.Headers.Add(header.Cookie, header);
        }
        /// <summary>
        /// 获取HTTP请求
        /// </summary>
        /// <param name="address">URI地址</param>
        /// <returns>HTTP请求</returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            webRequest = base.GetWebRequest(address);
            HttpWebRequest request = HttpRequest;
            if (request != null)
            {
                request.KeepAlive = KeepAlive;
                request.AllowAutoRedirect = AllowAutoRedirect;
                request.CookieContainer = Cookies;
                if (Certificate != null)
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = onValidateCertificate;
                    request.ClientCertificates.Add(Certificate);
                }
                if (TimeOut > 0) request.Timeout = TimeOut;
            }
            return request;
        }
        /// <summary>
        /// 安全连接证书验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private static bool onValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None;
        }
        /// <summary>
        /// 抓取页面字节流
        /// </summary>
        /// <param name="request">URI请求信息</param>
        /// <returns>页面字节流,失败返回null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public byte[] CrawlData(Request request)
        {
            return CrawlData(ref request);
        }
        /// <summary>
        /// 抓取页面字节流
        /// </summary>
        /// <param name="request">URI请求信息</param>
        /// <returns>页面字节流,失败返回null</returns>
        public byte[] CrawlData(ref Request request)
        {
            if (request.Uri != null)
            {
                try
                {
                    Headers.Add(HeaderName.UserAgent, UserAgent);
                    Headers.Add(HeaderName.Referer, request.RefererUrl == null || request.RefererUrl.Length == 0 ? request.Uri.AbsoluteUri : request.RefererUrl);
                    return deCompress(request.Form == null ? (request.UploadData == null ? DownloadData(request.Uri) : UploadData(request.Uri, PostName, request.UploadData)) : UploadValues(request.Uri, PostName, request.Form), ref request);
                }
                catch (Exception error)
                {
                    onError(error, ref request);
                }
            }
            return null;
        }
        /// <summary>
        /// 抓取页面HTML代码
        /// </summary>
        /// <param name="request">URI请求信息</param>
        /// <param name="encoding">页面编码</param>
        /// <returns>页面HTML代码,失败返回null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public string CrawlHtml(ref Request request, Encoding encoding)
        {
            return ChineseEncoder.ToString(CrawlData(ref request), encoding ?? this.TextEncoding);
        }
        /// <summary>
        /// 抓取页面HTML代码
        /// </summary>
        /// <param name="request">URI请求信息</param>
        /// <param name="encoding">页面编码</param>
        /// <returns>页面HTML代码,失败返回null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public string CrawlHtml(Request request, Encoding encoding)
        {
            return CrawlHtml(ref request, encoding);
        }
        /// <summary>
        /// 错误处理
        /// </summary>
        /// <param name="error">异常信息</param>
        /// <param name="request">请求信息</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onError(Exception error, ref Request request)
        {
            if (request.IsErrorOut)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Debug | Log.LogType.Info, error, (request.IsErrorOutUri ? request.Uri.AbsoluteUri : null) + " 抓取失败", !request.IsErrorOutUri);
            }
        }
        /// <summary>
        /// 数据解压缩
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="request">请求信息</param>
        /// <returns>解压缩数据</returns>
        private byte[] deCompress(byte[] data, ref Request request)
        {
            CompressionStream compressionStream = this.compressionStream;
            if (compressionStream != null)
            {
                try
                {
                    return compressionStream.GetDeCompress(data);
                }
                catch (Exception error)
                {
                    onError(error, ref request);
                    return null;
                }
            }
            return data;
        }

        /// <summary>
        /// 根据提交类型获取编码字符集
        /// </summary>
        /// <param name="contentType">提交类型</param>
        /// <returns>编码字符集</returns>
        private static Encoding getEncoding(string contentType)
        {
            foreach (SubString value in contentType.split(';'))
            {
                SubString key = value.Trim();
                if (key.StartsWith(CharsetName))
                {
                    try
                    {
                        key.MoveStart(CharsetName.Length);
                        return Encoding.GetEncoding(key);
                    }
                    catch(Exception error)
                    {
                        AutoCSer.Log.Pub.Log.Add(Log.LogType.Debug | Log.LogType.Info, error, key.ToString(), true);
                    }
                }
            }
            return null;
        }
    }
}
