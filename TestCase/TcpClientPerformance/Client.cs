using System;
using System.Threading;
using System.Diagnostics;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.TcpInternalClientPerformance
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
                    mapPointer = AutoCSer.Unmanaged.GetSize64((Count + 7) >> 3);
                    try
                    {
                        addMap = new AutoCSer.MemoryMap(mapPointer.Byte);
                        do
                        {
                            Left = AutoCSer.Random.Default.Next();
                            runTest();
                        }
                        while (true);
                    }
                    finally { AutoCSer.Unmanaged.Free(ref mapPointer); }
                }
            }
        }
        /// <summary>
        /// 进程锁是否已经被创建
        /// </summary>
        internal static bool IsCreatedProcessWait;
        /// <summary>
        /// 混合测试数量
        /// </summary>
        private const int mixingCount = 5;
        /// <summary>
        /// 单次测试调用次数
        /// </summary>
#if NOJIT
        internal const int Count = 20 * 10000 * mixingCount;
#else
        internal const int Count = 200 * 10000 * mixingCount;
#endif
        /// <summary>
        /// 客户端同步测试线程数量
        /// </summary>
        internal static int ThreadCount;
        /// <summary>
        /// 验证位图
        /// </summary>
        private static Pointer.Size mapPointer;
        /// <summary>
        /// 验证位图
        /// </summary>
        private static AutoCSer.MemoryMap addMap;
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
        /// 当前未完成测试计数
        /// </summary>
        private static int waitCount;
        /// <summary>
        /// 当前测试错误计数
        /// </summary>
        internal static int ErrorCount;
        /// <summary>
        /// 当前循环数量
        /// </summary>
        internal static int LoopCount;
        /// <summary>
        /// 发送数据次数
        /// </summary>
        internal static int SendCount;
        /// <summary>
        /// 发送数据次数
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        internal static int GetSendCount(int count)
        {
            int sendCount = SendCount;
            SendCount = count;
            return count - sendCount;
        }
        /// <summary>
        /// 接收数据次数
        /// </summary>
        internal static int ReceiveCount;
        /// <summary>
        /// 接收数据次数
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        internal static int GetReceiveCount(int count)
        {
            int receiveCount = ReceiveCount;
            ReceiveCount = count;
            return count - receiveCount;
        }
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
            if (type != TestType.ClientSynchronous) AutoCSer.Memory.Clear(mapPointer.ULong, mapPointer.GetSize() >> 3);
            waitCount = LoopCount = count;
            ErrorCount = 0;
            TestType = type;
            WaitHandle.Reset();
            Time.Restart();
        }
        /// <summary>
        /// 测试回调
        /// </summary>
        /// <param name="value"></param>
        internal static void OnAdd(AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add> value)
        {
            int right;
            if (value.Value.CheckSum(Left, out right) != 0 || !addMap.SetWhenNullUnsafe(right))
            {
                ++ErrorCount;
            }
            //if (System.Threading.Interlocked.Decrement(ref waitCount) == 0)
            if (--waitCount == 0)
            {
                Time.Stop();
                WaitHandle.Set();
            }
        }
        /// <summary>
        /// 测试回调
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool OnAddEmit(AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.TestCase.TcpServerPerformance.Add> value)
        {
            int right;
            if (value.Value.CheckSum(Left, out right) != 0 || !addMap.SetWhenNullUnsafe(right))
            {
                ++ErrorCount;
            }
            //if (System.Threading.Interlocked.Decrement(ref waitCount) == 0)
            if (--waitCount == 0)
            {
                Time.Stop();
                WaitHandle.Set();
            }
            return true;
        }
    }
}
