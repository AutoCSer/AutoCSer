using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 头部标志位
    /// </summary>
    [Flags]
    internal enum HeaderWebSocketFlag : uint
    {
        None = 0,
        /// <summary>
        /// 连接是否升级协议
        /// </summary>
        IsConnectionUpgrade = 1,
        /// <summary>
        /// 是否设置 WebSocket 确认连接值
        /// </summary>
        IsSetSecKey = 2,
        /// <summary>
        /// 升级协议是否支持 WebSocket
        /// </summary>
        IsUpgrade = 4,
        /// <summary>
        /// 是否设置 WebSocket 数据
        /// </summary>
        IsSetData = 8,
        /// <summary>
        /// 是否 WebSocket 连接
        /// </summary>
        IsWebSocket = IsConnectionUpgrade | IsUpgrade | IsSetSecKey,
        /// <summary>
        /// 是否设置访问来源
        /// </summary>
        IsSetOrigin = 0x10,
        /// <summary>
        /// 所有标志位
        /// </summary>
        All = 0xffffffffU,
    }
}
