using System;

namespace AutoCSer.CacheServer.OperationUpdater
{
    /// <summary>
    /// 更新条件逻辑类型
    /// </summary>
    internal enum LogicType : byte
    {
        /// <summary>
        /// 无条件更新
        /// </summary>
        None,
        /// <summary>
        /// 等于则更新
        /// </summary>
        Equal,
        /// <summary>
        /// 不等于则更新
        /// </summary>
        NotEqual,
        /// <summary>
        /// 大于则更新
        /// </summary>
        More,
        /// <summary>
        /// 大于等于则更新
        /// </summary>
        MoreOrEqual,
        /// <summary>
        /// 小于则更新
        /// </summary>
        Less,
        /// <summary>
        /// 小于等于则更新
        /// </summary>
        LessOrEqual,
    }
}
