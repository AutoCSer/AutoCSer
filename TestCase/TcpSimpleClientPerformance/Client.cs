using System;
using System.Threading;
using System.Diagnostics;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.TcpInternalSimpleClientPerformance
{
    /// <summary>
    /// TCP 客户端操作
    /// </summary>
    internal unsafe static partial class Client
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="runTest"></param>
        internal static void Start(string name, Action runTest)
        {
#if DotNetStandard
            Console.WriteLine("WARN : Linux .NET Core not support name EventWaitHandle");
            IsCreatedProcessWait = true;
            {
#else
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, name, out IsCreatedProcessWait);
            if (IsCreatedProcessWait)
            {
                using (processWait)
#endif
                {
                    do
                    {
                        Left = AutoCSer.Random.Default.Next();
                        runTest();
                    }
                    while (true);
                }
            }
        }
        /// <summary>
        /// 进程锁是否已经被创建
        /// </summary>
        internal static bool IsCreatedProcessWait;
        /// <summary>
        /// 单次测试调用次数
        /// </summary>
#if NOJIT
        internal const int Count = 10 * 10000;
#else
        internal const int Count = 10 * 10000;
#endif
        /// <summary>
        /// 客户端同步测试线程数量
        /// </summary>
        internal static int ThreadCount;
        /// <summary>
        /// 测试计时器
        /// </summary>
        internal static readonly Stopwatch Time = new Stopwatch();
        /// <summary>
        /// 测试等待结束事件
        /// </summary>
        internal static System.Threading.EventWaitHandle WaitHandle = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);
        /// <summary>
        /// 随机左值
        /// </summary>
        internal static int Left;
        /// <summary>
        /// 当前测试错误计数
        /// </summary>
        internal static int ErrorCount;
        /// <summary>
        /// 当前循环数量
        /// </summary>
        internal static int LoopCount;
        /// <summary>
        /// 测试类型
        /// </summary>
        internal static TestType TestType;
        /// <summary>
        /// 开始测试初始化
        /// </summary>
        /// <param name="type">测试类型</param>
        /// <param name="count">测试数量</param>
        internal static void Start(TestType type, int count)
        {
            LoopCount = count;
            ErrorCount = 0;
            TestType = type;
            WaitHandle.Reset();
            Time.Restart();
        }
    }
}
