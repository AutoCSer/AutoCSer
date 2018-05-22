using System;
using System.Threading;
using System.Diagnostics;
using AutoCSer.Extension;
using AutoCSer.CacheServer.DataStructure.Value;

namespace AutoCSer.TestCase.CacheClientPerformance
{
    /// <summary>
    /// 测试基类
    /// </summary>
    abstract class Test
    {
        /// <summary>
        /// 测试总数
        /// </summary>
        private const int loopCount = 100 * 10000;

        /// <summary>
        /// 测试客户端
        /// </summary>
        protected readonly AutoCSer.CacheServer.MasterClient client;
        /// <summary>
        /// 测试等待
        /// </summary>
        private readonly AutoResetEvent waitEvent = new AutoResetEvent(false);
        /// <summary>
        /// 计时器
        /// </summary>
        protected readonly Stopwatch time = new Stopwatch();
        /// <summary>
        /// 测试回调
        /// </summary>
        protected readonly Action<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter>> setCallbackReturnParameter;
        /// <summary>
        /// 测试回调类型
        /// </summary>
        private CallbackType callbackType;
        /// <summary>
        /// 测试类型
        /// </summary>
        private TestType type;
        /// <summary>
        /// 当前测试总数
        /// </summary>
        protected int count;
        /// <summary>
        /// 等待回调次数
        /// </summary>
        private int callbackCount;
        /// <summary>
        /// 错误次数
        /// </summary>
        protected int errorCount;
        /// <summary>
        /// 服务端是否文件持久化
        /// </summary>
        private readonly bool isFile;
        /// <summary>
        /// 测试基类
        /// </summary>
        /// <param name="client">测试客户端</param>
        /// <param name="isFile">服务端是否文件持久化</param>
        protected Test(AutoCSer.CacheServer.MasterClient client, bool isFile)
        {
            this.client = client;
            this.isFile = isFile;
            setCallbackReturnParameter = AutoCSer.CacheServer.ReturnParameter.GetCallback(setCallback);
        }
        /// <summary>
        /// 测试前初始化
        /// </summary>
        /// <param name="callbackType"></param>
        /// <param name="type"></param>
        protected void start(CallbackType callbackType, TestType type)
        {
            this.callbackType = callbackType;
            this.type = type;
            callbackCount = count = callbackType == CallbackType.Synchronous ? loopCount / 50 : loopCount;
            errorCount = 0;
            waitEvent.Reset();
            time.Restart();
        }
        /// <summary>
        /// 测试回调
        /// </summary>
        /// <param name="value"></param>
        protected void setCallback(AutoCSer.CacheServer.ReturnValue<bool> value)
        {
            if (!value.Value) ++errorCount;
            if (--callbackCount == 0) waitEvent.Set();
        }
        /// <summary>
        /// 测试回调
        /// </summary>
        /// <param name="value"></param>
        protected void getCallback(AutoCSer.CacheServer.ReturnValueNode<Value<int>> value)
        {
            if(value.Get().Type != AutoCSer.CacheServer.ReturnType.Success) ++errorCount;
            getCallback();
        }
        protected void getCallback()
        {
            if (Interlocked.Decrement(ref callbackCount) == 0) waitEvent.Set();
        }
        /// <summary>
        /// 等待测试结束
        /// </summary>
        protected void wait()
        {
            Console.WriteLine("loop end " + time.ElapsedMilliseconds.toString() + "ms");
            waitEvent.WaitOne();
            time.Stop();
            long milliseconds = Math.Max(time.ElapsedMilliseconds, 1);
            Console.WriteLine(count.toString() + " / " + milliseconds.toString() + "ms = " + (count / milliseconds) + "/ms " + (errorCount == 0 ? null : (" ERROR[" + errorCount.toString() + "]")) + " " + callbackType.ToString() + " " + type.ToString() + (isFile ? " + File" : null));
            Console.WriteLine(@"Sleep 3000ms
");
            System.Threading.Thread.Sleep(3000);
        }
    }
}
