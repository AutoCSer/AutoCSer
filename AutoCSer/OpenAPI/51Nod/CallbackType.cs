using System;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// 回调类型
    /// </summary>
    public enum CallbackType : byte
    {
        /// <summary>
        /// 未知类型，可能反序列化失败
        /// </summary>
        Unknown,
        /// <summary>
        /// 外部提交回调
        /// </summary>
        OpenJudge
    }
}
