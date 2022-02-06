using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Threading.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct TaskSwitchThreadBase
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 链表任务
        /// </summary>
        [FieldOffset(0)]
        public AutoCSer.Threading.TaskSwitchThreadBase Value;
    }
}
