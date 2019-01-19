using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 解析状态
    /// </summary>
    internal enum HeaderQueryParseState : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 逻辑值解析错误
        /// </summary>
        NotBool,
        /// <summary>
        /// 非数字解析错误
        /// </summary>
        NotNumber,
        /// <summary>
        /// 16进制数字解析错误
        /// </summary>
        NotHex,
        /// <summary>
        /// 时间解析错误
        /// </summary>
        NotDateTime,
        /// <summary>
        /// Guid解析错误
        /// </summary>
        NotGuid,
        /// <summary>
        /// 未知类型解析错误
        /// </summary>
        Unknown,
    }
}
