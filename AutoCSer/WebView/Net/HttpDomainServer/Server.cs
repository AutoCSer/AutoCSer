using System;
using System.Threading;
using System.Text;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.HttpDomainServer
{
    /// <summary>
    /// 域名服务
    /// </summary>
    public abstract class Server : IDisposable
    {
        /// <summary>
        /// 网站生成配置
        /// </summary>
        protected internal readonly AutoCSer.WebView.Config WebConfig;
#if !MONO
        /// <summary>
        /// 是否忽略大小写
        /// </summary>
        internal readonly bool WebConfigIgnoreCase;
#endif
#if !DOTNET2 && !DOTNET4
        /// <summary>
        /// GZip 是否启用快速压缩
        /// </summary>
        internal readonly bool WebConfigIsFastestCompressionLevel;
#endif
        /// <summary>
        /// HTTP 注册管理服务
        /// </summary>
        internal HttpRegister.Server RegisterServer;
        /// <summary>
        /// 域名信息集合
        /// </summary>
        protected HttpRegister.Domain[] domains;
        /// <summary>
        /// 停止服务处理
        /// </summary>
        protected event Action onStop;
        /// <summary>
        /// 获取Session
        /// </summary>
        public ISession Session { get; protected set; }
        /// <summary>
        /// 错误输出数据 + gzip 错误输出数据
        /// </summary>
        protected KeyValue<Http.Response, Http.Response>[] errorResponses;
        /// <summary>
        /// 加载检测路径
        /// </summary>
        internal string LoadCheckPath;
        /// <summary>
        /// 文件路径
        /// </summary>
        public string WorkPath { get; protected set; }
        /// <summary>
        /// 是否启动服务
        /// </summary>
        protected volatile int isStart;
        /// <summary>
        /// 是否停止服务
        /// </summary>
        private int isDisposed;
        /// <summary>
        /// 域名服务
        /// </summary>
        protected Server()
        {
            WebConfig = getWebConfig() ?? AutoCSer.WebView.Config.Null.Default;
#if !MONO
            WebConfigIgnoreCase = WebConfig.IgnoreCase;
#endif
#if !DOTNET2 && !DOTNET4
            WebConfigIsFastestCompressionLevel = WebConfig.IsFastestCompressionLevel;
#endif
        }
        /// <summary>
        /// 网站生成配置
        /// </summary>
        /// <returns>网站生成配置</returns>
        protected virtual AutoCSer.WebView.Config getWebConfig() { return null; }
        /// <summary>
        /// 启动HTTP服务
        /// </summary>
        /// <param name="registerServer">HTTP 注册管理服务</param>
        /// <param name="domains">域名信息集合</param>
        /// <param name="onStop">停止服务处理</param>
        /// <returns>是否启动成功</returns>
        public abstract bool Start(HttpRegister.Server registerServer, HttpRegister.Domain[] domains, Action onStop);
        /// <summary>
        /// 停止监听
        /// </summary>
        protected virtual void stopListen() { }
        /// <summary>
        /// 停止监听
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void StopListen()
        {
            stopListen();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected bool dispose()
        {
            //isStart = 1;
            Interlocked.Exchange(ref isStart, 1);
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0)
            {
                stopListen();
                if (onStop != null)
                {
                    try
                    {
                        onStop();
                    }
                    catch (Exception error)
                    {
                        RegisterServer.TcpServer.Log.Add(Log.LogType.Error, error);
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            dispose();
        }
        /// <summary>
        /// HTTP请求处理[TRY]
        /// </summary>
        /// <param name="socket">HTTP套接字</param>
        public abstract void Request(Http.SocketBase socket);
        /// <summary>
        /// WebSocket请求处理
        /// </summary>
        /// <param name="socket">HTTP套接字</param>
        public virtual void WebSocketRequest(Http.SocketBase socket)
        {
            socket.ResponseErrorIdentity(Http.ResponseState.NotFound404);
        }
        /// <summary>
        /// 获取错误数据
        /// </summary>
        /// <param name="state">错误状态</param>
        /// <param name="gzipFlag">是否支持 GZip 压缩</param>
        /// <returns>错误数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Http.Response GetErrorResponseData(Http.ResponseState state, Http.HeaderFlag gzipFlag)
        {
            if (errorResponses != null)
            {
                return gzipFlag == 0 ? errorResponses[(int)state].Key : errorResponses[(int)state].Value;
            }
            return null;
        }
        /// <summary>
        /// 查询参数反序列化预编译
        /// </summary>
        /// <param name="jsonTypes"></param>
        /// <param name="queryTypes"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal protected static void CompileQueryParse(Type[] jsonTypes, Type[] queryTypes)
        {
            if (jsonTypes.Length > 1) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(jsonTypes, AutoCSer.Threading.Thread.CallType.CompileJsonDeSerialize);
            if (queryTypes.Length > 1) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart((Action)(() => AutoCSer.Net.Http.HeaderQueryParser.Compile(queryTypes)), AutoCSer.Threading.Thread.CallType.Action);
        }
    }
}
