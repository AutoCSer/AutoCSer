using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionType
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// 委托
        /// </summary>
        [FieldOffset(0)]
        public Action Action;
        /// <summary>
        /// 链表任务
        /// </summary>
        [FieldOffset(0)]
        public LinkTask LinkTask;
        /// <summary>
        /// 链表任务配置
        /// </summary>
        [FieldOffset(0)]
        public LinkTaskConfig LinkTaskConfig;
        /// <summary>
        /// 线程池
        /// </summary>
        [FieldOffset(0)]
        public ThreadPool ThreadPool;
        /// <summary>
        /// 类型数组
        /// </summary>
        [FieldOffset(0)]
        public Type[] TypeArray;
    }
}
