using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 响应输出标志位
    /// </summary>
    [Flags]
    internal enum ResponseFlag : int
    {
        None = 0,
        /// <summary>
        /// 是否使用 HTTP 响应池
        /// </summary>
        IsPool = 1,
        /// <summary>
        /// 是否设置了输出状态
        /// </summary>
        State = 2,
        /// <summary>
        /// 重定向
        /// </summary>
        Location = 4,

        /// <summary>
        /// 输出内容类型
        /// </summary>
        ContentType = 0x10,
        /// <summary>
        /// 输出内容压缩编码
        /// </summary>
        ContentEncoding = 0x20,
        /// <summary>
        /// 内容描述
        /// </summary>
        ContentDisposition = 0x40,

        /// <summary>
        /// 最后修改时间
        /// </summary>
        LastModified = 0x100,
        /// <summary>
        /// 缓存参数
        /// </summary>
        CacheControl = 0x200,
        /// <summary>
        /// 缓存匹配标识
        /// </summary>
        ETag = 0x400,

        /// <summary>
        /// Cookie
        /// </summary>
        Cookie = 0x1000,
        /// <summary>
        /// 跨域访问权限
        /// </summary>
        AccessControlAllowOrigin = 0x2000,

        /// <summary>
        /// 是否允许设置 HTTP 预留头部字节数量
        /// </summary>
        CanHeaderSize = 0x10000,
        /// <summary>
        /// 是否设置 HTTP 预留头部字节数量
        /// </summary>
        HeaderSize = 0x20000,

        /// <summary>
        /// 所有 HTTP 输出标志位
        /// </summary>
        All = 0x7fffffff
    }
}
