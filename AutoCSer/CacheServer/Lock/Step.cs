using System;

namespace AutoCSer.CacheServer.Lock
{
    /// <summary>
    /// 锁管理步骤
    /// </summary>
    internal enum Step : byte
    {
        /// <summary>
        /// 初始状态
        /// </summary>
        None,
        /// <summary>
        /// 已经申请到锁
        /// </summary>
        Lock,
        /// <summary>
        /// 已经释放锁
        /// </summary>
        Exit,
    }
}
