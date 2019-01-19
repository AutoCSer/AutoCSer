using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Diagnostics
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
        /// 进程文件复制
        /// </summary>
        [FieldOffset(0)]
        public ProcessCopyer ProcessCopyer;
        /// <summary>
        /// 进程复制配置
        /// </summary>
        [FieldOffset(0)]
        public ProcessCopyConfig ProcessCopyConfig;
    }
}
