using System;

namespace AutoCSer.Win32
{
    /// <summary>
    /// 访问标志
    /// </summary>
    [Flags]
    public enum DesiredAccess : uint
    {
        /// <summary>
        /// 
        /// </summary>
        All = 0x10000000U,
        /// <summary>
        /// 执行
        /// </summary>
        Execute = 0x20000000,
        /// <summary>
        /// 写
        /// </summary>
        Write = 0x40000000U,
        /// <summary>
        /// 读
        /// </summary>
        Read = 0x80000000U,
        /// <summary>
        /// 读写 Read | Write
        /// </summary>
        ReadWrite = Read | Write,
    }
}
