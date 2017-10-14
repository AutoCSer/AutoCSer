using System;
using System.Text;
using System.Threading;
using System.Collections.Specialized;
using AutoCSer.Extension;
using AutoCSer.Net.WebClient;

namespace AutoCSer.OpenAPI
{
    /// <summary>
    /// web请求
    /// </summary>
    internal class WebClient : IDisposable
    {
        /// <summary>
        /// web客户端
        /// </summary>
        private readonly Client webClient = new Client();
        /// <summary>
        /// web客户端 访问锁
        /// </summary>
        private readonly object webClientLock = new object();
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            webClient.Dispose();
        }
        /// <summary>
        /// 公用web请求
        /// </summary>
        public WebClient()
        {
            webClient.KeepAlive = false;
        }
        /// <summary>
        /// API请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="encoding">请求地址</param>
        /// <param name="form">POST表单内容</param>
        /// <param name="uploadData"></param>
        /// <returns>返回内容,失败为null</returns>
        public string Request(string url, Encoding encoding, NameValueCollection form = null, byte[] uploadData = null)
        {
            Request request = new Request
            {
                Uri = new Uri(url),
                Form = form,
                UploadData = uploadData,
                IsErrorOut = true,
                IsErrorOutUri = true
            };
            Monitor.Enter(webClientLock);
            try
            {
                return webClient.CrawlHtml(ref request, encoding);
            }
            finally { Monitor.Exit(webClientLock); }
        }
        /// <summary>
        /// 文件描述
        /// </summary>
        private static readonly byte[] contentDispositionData = @"
Content-Disposition: form-data; name=""".getBytes();
        /// <summary>
        /// 文件名称
        /// </summary>
        private static readonly byte[] filenameData = @"""; filename=""".getBytes();
        /// <summary>
        /// 文件类型
        /// </summary>
        private static readonly byte[] contentTypeData = @"""
Content-Type: ".getBytes();
        /// <summary>
        /// API请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="encoding">请求地址</param>
        /// <param name="data">文件数据</param>
        /// <param name="filename">文件名称</param>
        /// <param name="contentType">输出内容类型</param>
        /// <param name="form">表单数据</param>
        /// <returns>返回内容,失败为null</returns>
        public unsafe string Upload(string url, Encoding encoding, byte[] data, string filename, byte[] contentType, KeyValue<byte[], byte[]>[] form)
        {
            string header = "multipart/form-data; boundary=----fastCSharpBoundary" + ((ulong)Date.NowTime.Set().Ticks).toHex();
            int size = 40 + contentDispositionData.Length + filename.Length + filenameData.Length + filename.Length + data.Length + 46 + form.Length * (contentDispositionData.Length + 5 + 42) + (contentType == null ? 5 : (contentTypeData.Length + contentType.Length + 4));
            foreach (KeyValue<byte[], byte[]> keyValue in form) size += keyValue.Key.Length + keyValue.Value.Length;
            byte[] body = new byte[size];
            fixed (byte* bodyFixed = body)
            {
                *(short*)(bodyFixed) = (short)('-' + ('-' << 8));
                byte* write = bodyFixed + sizeof(short);
                fixed (char* headerFixed = header)
                {
                    for (char* start = headerFixed + 30, end = start + 38; start != end; *write++ = (byte)*start++) ;
                }
                foreach (KeyValue<byte[], byte[]> keyValue in form)
                {
                    AutoCSer.Extension.Memory_OpenAPI.CopyNotNull(contentDispositionData, write, contentDispositionData.Length);
                    write += contentDispositionData.Length;
                    if (keyValue.Key.Length != 0)
                    {
                        AutoCSer.Memory.SimpleCopyNotNull64(keyValue.Key, write, keyValue.Key.Length);
                        write += keyValue.Key.Length;
                    }
                    *write++ = (byte)'"';
                    *(int*)write = 0x0a0d0a0d;
                    AutoCSer.Extension.Memory_OpenAPI.CopyNotNull(keyValue.Value, write += sizeof(int), keyValue.Value.Length);
                    write += keyValue.Value.Length;
                    *(short*)write = 0x0a0d;
                    write += sizeof(short);
                    AutoCSer.Memory.SimpleCopyNotNull64(bodyFixed, write, 40);
                    write += 40;
                }
                AutoCSer.Extension.Memory_OpenAPI.CopyNotNull(contentDispositionData, write, contentDispositionData.Length);
                write += contentDispositionData.Length;
                fixed (char* filenameFixed = filename)
                {
                    for (char* start = filenameFixed, end = filenameFixed + filename.Length; start != end; *write++ = (byte)*start++) ;
                    if (filenameData.Length != 0)
                    {
                        AutoCSer.Memory.SimpleCopyNotNull64(filenameData, write, filenameData.Length);
                        write += filenameData.Length;
                    }
                    for (char* start = filenameFixed, end = filenameFixed + filename.Length; start != end; *write++ = (byte)*start++) ;
                }
                if (contentType == null) *write++ = (byte)'"';
                else
                {
                    if (contentTypeData.Length != 0)
                    {
                        AutoCSer.Memory.SimpleCopyNotNull64(contentTypeData, write, contentTypeData.Length);
                        write += contentTypeData.Length;
                    }
                    if (contentType.Length != 0)
                    {
                        AutoCSer.Memory.SimpleCopyNotNull64(contentType, write, contentType.Length);
                        write += contentType.Length;
                    }
                }
                *(int*)write = 0x0a0d0a0d;
                write += sizeof(int);
                AutoCSer.Extension.Memory_OpenAPI.CopyNotNull(data, write, data.Length);
                write += data.Length;
                *(short*)write = 0x0a0d;
                write += sizeof(short);
                AutoCSer.Memory.SimpleCopyNotNull64(bodyFixed, write, 40);
                *(int*)(write + 40) = (int)('-' + ('-' << 8) + 0x0a0d0000);
            }
            Request request = new Request
            {
                Uri = new Uri(url),
                UploadData = body,
                IsErrorOut = true,
                IsErrorOutUri = true
            };
            Monitor.Enter(webClientLock);
            try
            {
                webClient.Headers.Add(AutoCSer.Net.Http.HeaderName.ContentType, header);
                return webClient.CrawlHtml(ref request, encoding);
            }
            finally { Monitor.Exit(webClientLock); }
        }
        /// <summary>
        /// API请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="uploadData"></param>
        /// <returns>返回内容,失败为null</returns>
        public byte[] Download(string url, byte[] uploadData)
        {
            Request request = new Request
            {
                Uri = new Uri(url),
                UploadData = uploadData,
                IsErrorOut = true,
                IsErrorOutUri = true
            };
            Monitor.Enter(webClientLock);
            try
            {
                return webClient.CrawlData(ref request);
            }
            finally { Monitor.Exit(webClientLock); }
        }
        /// <summary>
        /// 公用web请求
        /// </summary>
        public static readonly WebClient Default = new WebClient();
    }
}
