using System;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Net.HttpDomainServer
{
    /// <summary>
    /// 重定向服务
    /// </summary>
    public abstract class LocationServer : Server
    {
        /// <summary>
        /// 重定向域名
        /// </summary>
        private byte[] locationDomain;
        /// <summary>
        /// SSL重定向域名
        /// </summary>
        private byte[] sslLocationDomain;
        /// <summary>
        /// HTTP 状态类型
        /// </summary>
        private AutoCSer.Net.Http.ResponseState locationState;
        /// <summary>
        /// 是否临时重定向
        /// </summary>
        protected virtual bool is302
        {
            get { return true; }
        }
        /// <summary>
        /// 启动HTTP服务
        /// </summary>
        /// <param name="registerServer">HTTP 注册管理服务</param>
        /// <param name="domains">域名信息集合</param>
        /// <param name="onStop">停止服务处理</param>
        /// <returns>是否启动成功</returns>
        public override bool Start(HttpRegister.Server registerServer, HttpRegister.Domain[] domains, Action onStop)
        {
            if (isStart == 0)
            {
                string domain = getLocationDomain();
                if (!string.IsNullOrEmpty(domain))
                {
                    byte[] domainData = getDomainData(domain);
                    if (Interlocked.CompareExchange(ref isStart, 1, 0) == 0)
                    {
                        locationState = is302 ? AutoCSer.Net.Http.ResponseState.Found302 : AutoCSer.Net.Http.ResponseState.MovedPermanently301;
                        locationDomain = sslLocationDomain = domainData;
                        string sslDomain = getSslLocationDomain();
                        if (!string.IsNullOrEmpty(sslDomain)) sslLocationDomain = getDomainData(domain);
                        RegisterServer = registerServer;
                        this.domains = domains;
                        this.onStop += onStop;
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取包含协议的重定向域名,比如 http://www.AutoCSer.com
        /// </summary>
        /// <returns>获取包含协议的重定向域名</returns>
        protected abstract string getLocationDomain();
        /// <summary>
        /// 获取包含协议的重定向域名,比如 https://www.AutoCSer.com
        /// </summary>
        /// <returns>获取包含协议的重定向域名</returns>
        protected virtual string getSslLocationDomain()
        {
            return getLocationDomain();
        }
        /// <summary>
        /// HTTP请求处理[TRY]
        /// </summary>
        /// <param name="socket">HTTP套接字</param>
        public override unsafe void Request(Http.SocketBase socket)
        {
            Http.Header header = socket.HttpHeader;
            if (((uint)header.ContentLength | (uint)header.IsBoundary) == 0)
            {
                Http.Response response = Http.Response.Get();
                SubBuffer.PoolBufferFull buffer = header.Buffer;
                byte[] domain = socket.IsSsl ? sslLocationDomain : locationDomain, bufferArray = buffer.Buffer;
                BufferIndex uri = header.UriIndex;
                int length = domain.Length + uri.Length;
                if (uri.Length != 0)
                {
                    if (bufferArray[buffer.StartIndex + uri.StartIndex] == '/')
                    {
                        uri.Next();
                        if (uri.Length == 0) goto END;
                        --length;
                    }
                    if (length <= header.HeaderEndIndex)
                    {
                        int startIndex = uri.StartIndex - domain.Length;
                        if (startIndex >= 0)
                        {
                            Buffer.BlockCopy(domain, 0, bufferArray, startIndex += buffer.StartIndex, domain.Length);
                            response.SetLocation(bufferArray, startIndex, length, locationState);
                        }
                        else
                        {
                            Buffer.BlockCopy(bufferArray, buffer.StartIndex + uri.StartIndex, bufferArray, buffer.StartIndex + domain.Length, uri.Length);
                            Buffer.BlockCopy(domain, 0, bufferArray, buffer.StartIndex, domain.Length);
                            response.SetLocation(bufferArray, buffer.StartIndex, length, locationState);
                        }
                        socket.ResponseIdentity(ref response);
                        return;
                        //int endIndex = uri.EndIndex;
                        //if (header.HeaderEndIndex - endIndex - 7 >= length)
                        //{
                        //    fixed (byte* bufferFixed = bufferArray)
                        //    {
                        //        byte* bufferStart = bufferFixed + buffer.StartIndex, write = bufferStart + endIndex;
                        //        Memory.SimpleCopyNotNull64(domain, write, domain.Length);
                        //        Memory.SimpleCopyNotNull64(bufferStart + uri.StartIndex, write + domain.Length, uri.Length);
                        //    }
                        //    response.SetLocation(bufferArray, buffer.StartIndex + endIndex, length, locationState);
                        //    socket.Response(socketIdentity, ref response);
                        //    return;
                        //}
                    }
                }
            END:
                response.SetLocation(domain, locationState);
                socket.ResponseIdentity(ref response);
            }
            else socket.ResponseErrorIdentity(Http.ResponseState.BadRequest400);
        }

        /// <summary>
        /// 获取域名数据
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        private unsafe static byte[] getDomainData(string domain)
        {
            byte[] domainData;
            if (domain[domain.Length - 1] == '/') domainData = domain.getBytes();
            else
            {
                domainData = new byte[domain.Length + 1];
                fixed (char* domainFixed = domain)
                fixed (byte* dataFixed = domainData)
                {
                    StringExtension.WriteBytes(domainFixed, domain.Length, dataFixed);
                }
                domainData[domain.Length] = (byte)'/';
            }
            return domainData;
        }
    }
}