using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Log
{
    /// <summary>
    /// 输出调试信息
    /// </summary>
    //public static class Trace
    {
        /// <summary>
        /// 控制台输出
        /// </summary>
        /// <param name="exception"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Console(Exception exception)
        {
            Console(exception.ToString());
        }
        /// <summary>
        /// 控制台输出
        /// </summary>
        /// <param name="writeLine"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Console(string writeLine)
        {
            System.Console.WriteLine(writeLine);
            System.Diagnostics.Trace.WriteLine(writeLine);
        }
    }
}
