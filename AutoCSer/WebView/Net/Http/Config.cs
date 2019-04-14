using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 配置
    /// </summary>
    public partial class Config
    {
        /// <summary>
        /// HTTP 头部最大未定义项数
        /// </summary>
        internal const int DefaultHeaderCount = 8;
        /// <summary>
        /// HTTP 头部最大未定义项数，默认为 8
        /// </summary>
        public int MaxHeaderCount = DefaultHeaderCount;
        /// <summary>
        /// URI 最大查询参数项数
        /// </summary>
        internal const int DefaultQueryCount = 32;
        /// <summary>
        /// URI 最大查询参数项数，默认为 8
        /// </summary>
        public int MaxQueryCount = DefaultQueryCount;
        /// <summary>
        /// 默认 HTTP 头部缓存数据字节大小
        /// </summary>
        internal const SubBuffer.Size DefaultHeadSize = SubBuffer.Size.Kilobyte2;
        /// <summary>
        /// HTTP 头部缓存数据字节大小，默认为 2KB
        /// </summary>
        public SubBuffer.Size HeadSize = DefaultHeadSize;
        /// <summary>
        /// 临时缓存数据字节大小，默认为 2KB
        /// </summary>
        public SubBuffer.Size BufferSize = SubBuffer.Size.Kilobyte2;
        /// <summary>
        /// HTTP 头部接收默认超时为 9 秒，超时客户端将被当作攻击者被抛弃。
        /// </summary>
        internal const int DefaultReceiveHeadSeconds = 9;
        /// <summary>
        /// HTTP 头部接收默认超时为 9 秒，超时客户端将被当作攻击者被抛弃。
        /// </summary>
        public int ReceiveHeadSeconds = DefaultReceiveHeadSeconds;
        /// <summary>
        /// HTTP 头部接收默认超时为 89 秒，超时客户端将被被抛弃。
        /// </summary>
        internal const int DefaultKeepAliveReceiveHeadSeconds = 89;
        /// <summary>
        /// HTTP 头部接收默认超时为 89 秒，超时客户端将被被抛弃。
        /// </summary>
        public int KeepAliveReceiveHeadSeconds = DefaultKeepAliveReceiveHeadSeconds;
        /// <summary>
        /// 套接字每秒最少收发字节数量,默认为 2KB
        /// </summary>
        public int SocketSizePerSecond = 2 << 10;
        /// <summary>
        /// 获取套接字操作超时时间
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal DateTime GetTimeout(int size)
        {
            return Date.NowTime.Now.AddTicks(Math.Max(size / SocketSizePerSecond + 1, 2) * TimeSpan.TicksPerSecond);
        }
        /// <summary>
        /// 获取套接字操作超时时间
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal DateTime GetTimeout(long size)
        {
            return size <= int.MaxValue ? GetTimeout((int)size) : Date.NowTime.Now.AddTicks(Math.Max(size / SocketSizePerSecond + 1, 2) * TimeSpan.TicksPerSecond);
        }
        /// <summary>
        /// 获取套接字操作超时时间
        /// </summary>
        /// <param name="size"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal DateTime GetTimeout(int size, out ushort count)
        {
            int seconds = Math.Max(size / SocketSizePerSecond + 1, 2);
            count = seconds <= ReceiveHeadSeconds ? (ushort)0 : (ushort)Math.Min(seconds / ReceiveHeadSeconds, ushort.MaxValue);
            return Date.NowTime.Now.AddTicks(seconds * TimeSpan.TicksPerSecond);
        }
        /// <summary>
        /// Session 超时分钟数
        /// </summary>
        public int SessionMinutes = 60;
        /// <summary>
        /// Session 超时刷新分钟数
        /// </summary>
        public int SessionRefreshMinutes = 10;
        /// <summary>
        /// 文件缓存最大字节总数，默认为 128MB
        /// </summary>
        public long FileCacheTotalSize = 128 << 20;
        /// <summary>
        /// 最大缓存文件节数，默认为 256KB
        /// </summary>
        public int FileCacheSize = 256 << 10;
#if !DOTNET2 && !DOTNET4
        /// <summary>
        /// 默认为 false 表示文件缓存 GZip 使用默认压缩，否则启用快速压缩
        /// </summary>
        public bool IsFileCacheFastestCompressionLevel;
#endif
        /// <summary>
        /// 文件缓存是否预留 HTTP 头部空间
        /// </summary>
        public bool IsFileCacheHeader = true;
        /// <summary>
        /// KeepAlive 是否保持相同的域名服务（不再根据 host 查找不同的域名服务）
        /// </summary>
        public bool IsKeepAliveDomainServer = true;
        /// <summary>
        /// 是否输出服务器信息
        /// </summary>
        public bool IsResponseServer = true;
        /// <summary>
        /// 是否输出缓存参数
        /// </summary>
        public bool IsResponseCacheControl = true;
        /// <summary>
        /// 输出内容类型
        /// </summary>
        public bool IsResponseContentType = true;
        /// <summary>
        /// 是否输出日期
        /// </summary>
        public bool IsResponseDate = true;
        /// <summary>
        /// 是否输出最后修改日志
        /// </summary>
        public bool IsResponseLastModified = true;
        /// <summary>
        /// Session 名称
        /// </summary>
        public string SessionName = "AutoCSerSession";
    }
}
