using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// 外部提交测试结果
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct JudgeItem
    {
        /// <summary>
        /// 内存使用字节数
        /// </summary>
        public long MemoryUse;
        /// <summary>
        /// 用时毫秒数
        /// </summary>
        public int TimeUse;
        /// <summary>
        /// 测试数据标识
        /// </summary>
        public int TestId;
        /// <summary>
        /// 测试结果
        /// </summary>
        public JudgeResultEnum Result;
    }
}
