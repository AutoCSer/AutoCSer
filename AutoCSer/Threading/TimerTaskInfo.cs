using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 定时任务信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct TimerTaskInfo
    {
        /// <summary>
        /// 任务委托
        /// </summary>
        internal object Value;
        /// <summary>
        /// 调用类型
        /// </summary>
        internal Thread.CallType CallType;
        /// <summary>
        /// 是否启动线程池线程
        /// </summary>
        internal TimerTaskThreadType ThreadType;
        /// <summary>
        /// 任务抛到线程池
        /// </summary>
        /// <param name="threadPool"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Start(ThreadPool threadPool)
        {
            threadPool.FastStart(Value, CallType);
        }
        /// <summary>
        /// 任务调用
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Call()
        {
            new Thread.CallInfo { Value = Value, Type = CallType }.Call();
        }
    }
}
