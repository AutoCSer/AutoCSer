using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 计时器扩展
    /// </summary>
    public static class StopwatchExtension
    {
        /// <summary>
        /// 重启计时器
        /// </summary>
        /// <param name="stopwatch"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Restart(this Stopwatch stopwatch)
        {
            stopwatch.Reset();
            stopwatch.Start();
        }
    }
}
