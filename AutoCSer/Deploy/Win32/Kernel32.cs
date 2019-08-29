using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Deploy.Win32
{
    /// <summary>
    /// kernel32.dll API
    /// </summary>
    public static class Kernel32
    {
        /// <summary>
        /// 控制 Windows 是否处理 指定类型的严重错误或使调用应用程序来处理它们
        /// </summary>
        /// <param name="errorMode">错误处理模式</param>
        /// <returns>错误处理模式</returns>
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern ErrorMode SetErrorMode(ErrorMode errorMode);
    }
}
