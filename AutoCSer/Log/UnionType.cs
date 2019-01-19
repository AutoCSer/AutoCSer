using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Log
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
        /// 日志配置
        /// </summary>
        [FieldOffset(0)]
        public Config Config;
        /// <summary>
        /// 文件日志
        /// </summary>
        [FieldOffset(0)]
        public File File;
    }
}
