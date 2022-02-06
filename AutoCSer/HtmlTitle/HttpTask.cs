using System;
using System.Text;
using System.Net.Sockets;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.HtmlTitle
{
    /// <summary>
    /// HTML 标题获取客户端任务池
    /// </summary>
    public sealed class HttpTask : IDisposable
    {
        /// <summary>
        /// 最小URI字节长度
        /// </summary>
        internal const int MinUriSize = 10;
        /// <summary>
        /// 最小缓存区字节长度
        /// </summary>
        internal const AutoCSer.Memory.BufferSize MinBufferSize = AutoCSer.Memory.BufferSize.Kilobyte;

        /// <summary>
        /// 客户端集合
        /// </summary>
        private readonly HttpClient[] clients;
        /// <summary>
        /// 日志处理
        /// </summary>
        internal readonly AutoCSer.ILog Log;
        /// <summary>
        /// 数据缓存区池
        /// </summary>
        internal readonly AutoCSer.SubBuffer.Pool BufferPool;
        /// <summary>
        /// HTTP 头部接收超时
        /// </summary>
        private SocketTimeoutLink socketTimeout;
        /// <summary>
        /// Uri与回调函数信息集合
        /// </summary>
        private Uri.Queue uris;
        /// <summary>
        /// 当前客户端位置
        /// </summary>
        private int clientIndex;
        /// <summary>
        /// 客户端集合访问锁
        /// </summary>
        private AutoCSer.Threading.SpinLock clientLock;
        /// <summary>
        /// 当前实例数量
        /// </summary>
        private int clientCount;
        /// <summary>
        /// 收发数据缓冲区字节数
        /// </summary>
        internal readonly int BufferSize;
        /// <summary>
        /// 最大搜索字节数
        /// </summary>
        internal readonly int MaxSearchSize;
        /// <summary>
        /// 是否验证安全证书
        /// </summary>
        internal bool IsValidateCertificate;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private int isDisposed;
        /// <summary>
        /// HTML标题获取客户端任务池
        /// </summary>
        /// <param name="maxClientCount">最大实例数量</param>
        /// <param name="timeoutSeconds">套接字操作超时时间</param>
        /// <param name="bufferSize">收发数据缓冲区字节数</param>
        /// <param name="maxSearchSize">最大搜索字节数</param>
        /// <param name="isValidateCertificate">是否验证安全证书</param>
        /// <param name="log">日志处理</param>
        public HttpTask(int maxClientCount = 1, int timeoutSeconds = 15, AutoCSer.Memory.BufferSize bufferSize = AutoCSer.Memory.BufferSize.Kilobyte4, int maxSearchSize = 0, bool isValidateCertificate = false, AutoCSer.ILog log = null)
        {
            if (bufferSize < MinBufferSize) bufferSize = MinBufferSize;
            else if (bufferSize > AutoCSer.Memory.BufferSize.Kilobyte32) bufferSize = AutoCSer.Memory.BufferSize.Kilobyte32;
            BufferSize = (int)bufferSize;
            MaxSearchSize = Math.Min(BufferSize - sizeof(int), maxSearchSize);
            IsValidateCertificate = isValidateCertificate;
            this.Log = log ?? AutoCSer.LogHelper.Default;
            BufferPool = AutoCSer.SubBuffer.Pool.GetPool(bufferSize);
            uris = new Uri.Queue(new Uri());
            socketTimeout = new SocketTimeoutLink(Math.Max(timeoutSeconds, 15));
            clients = new HttpClient[maxClientCount <= 0 ? 1 : maxClientCount];
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (isDisposed == 0)
            {
                int clientIndex = 0;
                Uri uri = null;
                clientLock.EnterYield();
                if (isDisposed == 0)
                {
                    isDisposed = 1;
                    clientIndex = this.clientIndex;
                    this.clientIndex = 0;
                    uri = uris.GetClear();
                }
                clientLock.Exit();

                if (clientIndex != 0)
                {
                    foreach (HttpClient client in clients)
                    {
                        client.Free();
                        if (--clientIndex == 0) break;
                    }
                }
                Array.Clear(clients, 0, clients.Length);
                if (uri != null) uri.CancelQueue(Log);

                SocketTimeoutLink.Free(ref socketTimeout);
            }
        }
        /// <summary>
        /// 获取HTML标题
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <param name="onGet">获取HTML标题回调函数</param>
        /// <param name="encoding">默认编码格式</param>
        public void Get(string uri, Action<string> onGet, Encoding encoding = null)
        {
            if (onGet == null) throw new ArgumentNullException();
            bool isOnGet = false;
            try
            {
                if (isDisposed == 0 && uri.length() >= MinUriSize)
                {
                    Uri uriInfo = AutoCSer.Threading.RingPool<Uri>.Default.Pop() ?? new Uri();
                    if (uriInfo != null)
                    {
                        uriInfo.Set(uri, onGet, encoding);
                        get(uriInfo, ref isOnGet);
                    }
                }
            }
            finally { if (!isOnGet) onGet(null); }
        }
        /// <summary>
        /// 获取HTML标题
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <param name="onGet">获取HTML标题回调函数</param>
        /// <param name="encoding">默认编码格式</param>
        public void Get(ref SubArray<byte> uri, Action<string> onGet, Encoding encoding = null)
        {
            if (onGet == null) throw new ArgumentNullException();
            bool isOnGet = false;
            try
            {
                if (isDisposed == 0 && uri.Length >= MinUriSize)
                {
                    Uri uriInfo = AutoCSer.Threading.RingPool<Uri>.Default.Pop() ?? new Uri();
                    if (uriInfo != null)
                    {
                        uriInfo.Set(ref uri, onGet, encoding);
                        get(uriInfo, ref isOnGet);
                    }
                }
            }
            finally { if (!isOnGet) onGet(null); }
        }
        /// <summary>
        /// 获取HTML标题
        /// </summary>
        /// <param name="uri">Uri与回调函数信息</param>
        /// <param name="isOnGet"></param>
        private void get(Uri uri, ref bool isOnGet)
        {
            HttpClient client = null;
            clientLock.EnterYield();
            if (isDisposed == 0)
            {
                if (clientIndex == 0)
                {
                    if (clientCount == clients.Length)
                    {
                        uris.Push(uri);
                        clientLock.Exit();
                        isOnGet = true;
                        return;
                    }
                    ++clientCount;
                    clientLock.Exit();
                    try
                    {
                        client = new HttpClient(this);
                    }
                    catch (Exception error)
                    {
                        Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                    }
                    if (client == null)
                    {
                        clientLock.EnterYield();
                        if (isDisposed == 0)
                        {
                            uris.Push(uri);
                            --clientCount;
                            clientLock.Exit();
                            isOnGet = true;
                            return;
                        }
                        clientLock.Exit();
                        isOnGet = true;
                        uri.Cancel();
                        return;
                    }
                }
                else
                {
                    client = clients[--clientIndex];
                    clientLock.Exit();
                }
                isOnGet = client.Get(uri);
            }
            else
            {
                clientLock.Exit();
                isOnGet = true;
                uri.Cancel();
            }
        }
        /// <summary>
        /// 添加HTML标题获取客户端
        /// </summary>
        /// <param name="client">HTML标题获取客户端</param>
        /// <returns></returns>
        internal int Push(HttpClient client)
        {
        START:
            clientLock.EnterYield();
            if (!uris.IsEmpty)
            {
                Uri uri = uris.UnsafePopOnly();
                clientLock.Exit();
                try
                {
                    if (client.Get(uri)) return 0;
                }
                catch (Exception error)
                {
                    Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                }
                try
                {
                    uri.Cancel();
                }
                catch (Exception error)
                {
                    Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                }
                goto START;
            }
            if (isDisposed == 0)
            {
                clients[clientIndex++] = client;
                clientLock.Exit();
                return 0;
            }
            --clientCount;
            clientLock.Exit();
            return 1;
        }
        /// <summary>
        /// 取消超时操作
        /// </summary>
        /// <param name="client"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CancelTimeout(HttpClient client)
        {
            SocketTimeoutLink socketTimeout = this.socketTimeout;
            if (socketTimeout != null) socketTimeout.Cancel(client);
        }
        /// <summary>
        /// 添加超时操作
        /// </summary>
        /// <param name="client"></param>
        /// <param name="socket"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void PushTimeout(HttpClient client, Socket socket)
        {
            SocketTimeoutLink socketTimeout = this.socketTimeout;
            if (socketTimeout != null) socketTimeout.Push(client, socket);
        }
        
    }
}
