using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 套接字标志位
    /// </summary>
    [Flags]
    internal enum SocketFlag : uint
    {
        None,
        /// <summary>
        /// 是否已经加载表单数据
        /// </summary>
        IsLoadForm = 1,
        /// <summary>
        /// 是否申请临时数据缓冲区
        /// </summary>
        BigBuffer = 2,
        /// <summary>
        /// 是否获取请求表单数据
        /// </summary>
        GetForm = 4,
        /// <summary>
        /// 所有标志位
        /// </summary>
        All = 0xffffffffU
    }
}
