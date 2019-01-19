using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 接收数据下一步操作类型
    /// </summary>
    internal enum ReceiveType : byte
    {
        /// <summary>
        /// 忽略数据后输出 HTTP 响应
        /// </summary>
        Response,
        /// <summary>
        /// 获取请求表单数据
        /// </summary>
        GetForm,
    }
}
