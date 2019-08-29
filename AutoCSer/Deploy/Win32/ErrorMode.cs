using System;

namespace AutoCSer.Deploy.Win32
{
    /// <summary>
    /// 错误处理模式
    /// </summary>
    [Flags]
    public enum ErrorMode : uint
    {
        /// <summary>
        /// 使用系统默认的，既显示所有错误的对话框
        /// </summary>
        NONE,
        /// <summary>
        /// 系统不显示关键错误处理消息框。 相反，系统发送错误给调用进程。
        /// </summary>
        SEM_FAILCRITICALERRORS = 1,
        /// <summary>
        /// 系统不显示Windows错误报告对话框。
        /// </summary>
        SEM_NOGPFAULTERRORBOX = 2,
        /// <summary>
        /// 系统会自动修复故障此功能只支持部分处理器架构。
        /// </summary>
        SEM_NOALIGNMENTFAULTEXCEPT = 4,
        /// <summary>
        /// 当无法找到文件时不弹出错误对话框。 相反，错误返回给调用进程。
        /// </summary>
        SEM_NOOPENFILEERRORBOX = 0x8000
    }
}
