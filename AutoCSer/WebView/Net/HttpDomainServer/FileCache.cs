using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.HttpDomainServer
{
    /// <summary>
    /// 文件缓存
    /// </summary>
    public sealed class FileCache
    {
        /// <summary>
        /// HTTP头部预留字节数
        /// </summary>
        internal const int HttpHeaderSize = 256 + 64;

        /// <summary>
        /// 文件数据
        /// </summary>
        private SubArray<byte> data;
        /// <summary>
        /// 文件数据
        /// </summary>
        public SubArray<byte> Data
        {
            get
            {
                if (IsData == 0) wait();
                return data;
            }
        }
        /// <summary>
        /// 文件压缩数据
        /// </summary>
        private SubArray<byte> gZipData;
        /// <summary>
        /// 文件数据
        /// </summary>
        public SubArray<byte> GZipData
        {
            get
            {
                if (IsData == 0) wait();
                return gZipData;
            }
        }
        /// <summary>
        /// 数据加载访问锁
        /// </summary>
        private readonly object dataLock;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        internal byte[] LastModified;
        /// <summary>
        /// HTTP响应输出内容类型
        /// </summary>
        internal byte[] ContentType;
        /// <summary>
        /// 文件HTTP响应输出
        /// </summary>
        private Http.Response response;
        /// <summary>
        /// 文件HTTP响应输出
        /// </summary>
        internal Http.Response Response
        {
            get
            {
                if (IsData == 0) wait();
                return response;
            }
        }
        /// <summary>
        /// 文件HTTP响应输出
        /// </summary>
        private Http.Response gZipResponse;
        /// <summary>
        /// 文件HTTP响应输出
        /// </summary>
        internal Http.Response GZipResponse
        {
            get
            {
                if (IsData == 0) wait();
                return gZipResponse;
            }
        }
        /// <summary>
        /// 文件数据字节数
        /// </summary>
        internal int Size;
        /// <summary>
        /// 是否已经获取数据
        /// </summary>
        internal int IsData;
        /// <summary>
        /// 文件缓存
        /// </summary>
        internal FileCache()
        {
            Monitor.Enter(dataLock = new object());
        }
        /// <summary>
        /// 数据加载完毕
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void PulseAll()
        {
            IsData = 1;
            Monitor.PulseAll(dataLock);
            Monitor.Exit(dataLock);
        }
        /// <summary>
        /// 等待数据加载
        /// </summary>
        private void wait()
        {
            Monitor.Enter(dataLock);
            if (IsData == 0) Monitor.Wait(dataLock);
            Monitor.Exit(dataLock);
        }

        /// <summary>
        /// 文件缓存
        /// </summary>
        /// <param name="data">文件数据</param>
        /// <param name="contentType">HTTP响应输出内容类型</param>
        /// <param name="cacheControl">缓存控制参数</param>
        /// <param name="isGZip">是否压缩</param>
        /// <returns>文件数据字节数</returns>
        internal int Set(ref SubArray<byte> data, byte[] contentType, byte[] cacheControl, bool isGZip)
        {
            int size = data.Length;
            ContentType = contentType ?? NullValue<byte>.Array;
            try
            {
                this.data = gZipData = data;
                if (FileCacheQueue.IsFileCacheHeader && data.Start == FileCache.HttpHeaderSize)
                {
                    size += FileCache.HttpHeaderSize;
                    Http.Response response = Http.Response.New();
                    //response.State = Http.ResponseState.Ok200;
                    response.SetCanHeaderSize(ref data);
                    response.CacheControl = cacheControl;
                    response.ContentType = contentType;
                    response.SetLastModified(LastModified);
                    this.response = gZipResponse = response;
                }
                if (isGZip)
                {
#if DOTNET2 || DOTNET4
                    if (Http.Response.GetCompress(ref data, ref gZipData, data.Start))
#else
                    if (Http.Response.GetCompress(ref data, ref gZipData, data.Start, Http.SocketBase.Config.IsFileCacheFastestCompressionLevel))
#endif
                    {
                        size += gZipData.Length;
                        if (FileCacheQueue.IsFileCacheHeader && gZipData.Start == FileCache.HttpHeaderSize)
                        {
                            size += FileCache.HttpHeaderSize;
                            Http.Response gZipResponse = Http.Response.New();
                            //gZipResponse.State = Http.ResponseState.Ok200;
                            gZipResponse.SetCanHeaderSize(ref gZipData);
                            gZipResponse.ContentType = contentType;
                            gZipResponse.SetLastModified(LastModified);
                            gZipResponse.SetContentEncoding(Http.Response.GZipEncoding);
                            this.gZipResponse = gZipResponse;
                        }
                    }
                    else gZipData = data;
                }
            }
            finally { PulseAll(); }
            return size;
        }
    }
}
