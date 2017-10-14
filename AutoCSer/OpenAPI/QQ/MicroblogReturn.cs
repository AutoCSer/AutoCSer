using System;

namespace AutoCSer.OpenAPI.QQ
{
    /// <summary>
    /// 微博
    /// </summary>
    public sealed class MicroblogReturn : ErrorCode
    {
        /// <summary>
        /// 微博ID
        /// </summary>
        public MicroblogId data;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">微博信息</param>
        /// <returns>微博ID</returns>
        public static implicit operator string(MicroblogReturn value) { return value != null && value.IsReturn ? value.data.id : null; }
    }
}
