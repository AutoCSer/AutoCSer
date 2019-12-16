using System;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 释放资源
    /// </summary>
    internal static class WaitHandleExtension
    {
        /// <summary>
        /// 失败资源
        /// </summary>
        /// <param name="waitHandle"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Dispose(this System.Threading.WaitHandle waitHandle)
        {
            waitHandle.Close();
        }
    }
}
