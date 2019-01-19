using System;

namespace AutoCSer.Net.HttpDomainServer
{
    /// <summary>
    /// 静态文件服务
    /// </summary>
    public abstract class StaticFileServer : FileServer
    {
        /// <summary>
        /// 客户端缓存时间(单位:秒)
        /// </summary>
        protected override int clientCacheSeconds
        {
            get { return Http.Response.StaticFileCacheControlSeconds; }
        }
        /// <summary>
        /// HTTP请求处理[TRY]
        /// </summary>
        /// <param name="socket">HTTP套接字</param>
        public override unsafe void Request(Http.SocketBase socket)
        {
            if ((socket.HttpHeader.Flag & Http.HeaderFlag.IsSetIfModifiedSince) == 0)
            {
                Http.Response response = file(socket.HttpHeader);
                if (response != null) socket.ResponseIdentity(ref response);
                else socket.ResponseErrorIdentity(Http.ResponseState.NotFound404);
            }
            else socket.ResponseIdentity(Http.Response.NotChanged304);
        }
    }
}
