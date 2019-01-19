using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 常用公共定义
    /// </summary>
    public static class Pub
    {
        /// <summary>
        /// CPU核心数量
        /// </summary>
        public static readonly int CpuCount = Math.Max(Environment.ProcessorCount, 1);
    }
}
