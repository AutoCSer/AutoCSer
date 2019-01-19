using System;

namespace AutoCSer.Log
{
    /// <summary>
    /// 缓存类型
    /// </summary>
    public enum CacheType : byte
    {
        /// <summary>
        /// 不缓存
        /// </summary>
        None,
        /// <summary>
        /// 先进先出
        /// </summary>
        Queue,
        /// <summary>
        /// 最后一次
        /// </summary>
        Last,
    }
}
