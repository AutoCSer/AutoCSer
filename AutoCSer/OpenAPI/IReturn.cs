using System;

namespace AutoCSer.OpenAPI
{
    /// <summary>
    /// 数据是否有效
    /// </summary>
    public interface IReturn
    {
        /// <summary>
        /// 数据是否有效
        /// </summary>
        bool IsReturn { get; }
        /// <summary>
        /// 提示信息
        /// </summary>
        string Message { get; }
    }
}
