using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Threading.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct SecondTimerTaskNode
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 二维秒级定时任务节点
        /// </summary>
        [FieldOffset(0)]
        public AutoCSer.Threading.SecondTimerTaskNode Value;
    }
}
