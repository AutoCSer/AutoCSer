using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Win32
{
    /// <summary>
    /// kernel32.dll API
    /// </summary>
    internal static class Kernel32
    {
        /// <summary>
        /// 内存复制
        /// </summary>
        /// <param name="dest">目标位置</param>
        /// <param name="src">源位置</param>
        /// <param name="length">字节长度</param>
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static unsafe extern void RtlMoveMemory(void* dest, void* src, int length);
    }
}
