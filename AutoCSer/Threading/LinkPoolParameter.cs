using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 默认链表缓存池参数
    /// </summary>
    public struct LinkPoolParameter
    {
        /// <summary>
        /// 最大缓存对象数量，默认为 4096
        /// </summary>
        public int MaxObjectCount;
        /// <summary>
        /// 释放空闲缓存对象定时间隔秒数，默认为 60s
        /// </summary>
        public int ReleaseFreeTimeoutSeconds;
        /// <summary>
        /// 是否添加到公共清除缓存数据，默认为 true
        /// </summary>
        public bool IsClearCache;

        /// <summary>
        /// 默认链表缓存池参数
        /// </summary>
        public static LinkPoolParameter Default
        {
            get
            {
                return new LinkPoolParameter
                {
                    MaxObjectCount = 1 << 12,
                    ReleaseFreeTimeoutSeconds = 60,
                    IsClearCache = true
                };
            }
        }
    }
}
