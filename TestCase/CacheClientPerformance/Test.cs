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
        protected const int loopCount = 100 * 10000;

        /// <summary>
        /// 测试客户端
        /// </summary>
        protected readonly AutoCSer.CacheServer.Client client;
        /// <summary>
        /// 测试等待
        /// </summary>
        private readonly AutoResetEvent waitEvent = new AutoResetEvent(false);
        /// <summary>
        /// 计时器
        /// </summary>
        protected long time;
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
        protected Test(AutoCSer.CacheServer.Client client, bool isFile)
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
        /// <param name="mul"></param>
        protected void start(CallbackType callbackType, TestType type, int callbackMul = 1)
        {
            this.callbackType = callbackType;
            this.type = type;
            count = (callbackType == CallbackType.Synchronous ? loopCount / 50 : loopCount);
            callbackCount = count * callbackMul;
            errorCount = 0;
            Console.WriteLine("start " + count.toString() + " " + callbackType.ToString() + " " + type.ToString() + (isFile ? " + File" : null));
            waitEvent.Reset();
            time = AutoCSer.Pub.StopwatchTicks;
        }
        /// <summary>
        /// 测试回调
        /// </summary>
        /// <param name="value"></param>
        protected void setCallback(AutoCSer.CacheServer.ReturnValue<bool> value)
        {
            setCallback(value.Value);
        }
        /// <summary>
        /// 测试回调
        /// </summary>
        /// <param name="value"></param>
        protected void setCallback(bool value)
        {
            if (!value) ++errorCount;
            if (--callbackCount == 0) waitEvent.Set();
        }
        /// <summary>
        /// 测试回调
        /// </summary>
        /// <param name="value"></param>
        protected void setCallbackInterlocked(AutoCSer.CacheServer.ReturnValue<bool> value)
        {
            setCallbackInterlocked(value.Value);
        }
        /// <summary>
        /// 测试回调
        /// </summary>
        /// <param name="value"></param>
        protected void setCallbackInterlocked(bool value)
        {
            if (!value) Interlocked.Increment(ref errorCount);
            if (Interlocked.Decrement(ref callbackCount) == 0) waitEvent.Set();
        }
        /// <summary>
        /// 测试回调
        /// </summary>
        /// <param name="errorCount"></param>
        protected void setCallbackInterlocked(int errorCount)
        {
            Interlocked.Add(ref this.errorCount, errorCount);
            if (Interlocked.Add(ref this.callbackCount, -count) == 0) waitEvent.Set();
        }
        /// <summary>
        /// 测试回调
        /// </summary>
        /// <param name="value"></param>
        protected void getCallback(AutoCSer.CacheServer.ReturnValue<int> value)
        {
            if(value.Type != AutoCSer.CacheServer.ReturnType.Success) ++errorCount;
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
            Console.WriteLine("loop end " + ((long)new TimeSpan(AutoCSer.Pub.StopwatchTicks - time).TotalMilliseconds).toString() + "ms");
            waitEvent.WaitOne();
            long milliseconds = Math.Max((long)new TimeSpan(AutoCSer.Pub.StopwatchTicks - time).TotalMilliseconds, 1);
            Console.WriteLine(count.toString() + " / " + milliseconds.toString() + "ms = " + (count / milliseconds) + "/ms " + (errorCount == 0 ? null : (" ERROR[" + errorCount.toString() + "]")));
            Console.WriteLine(@"Sleep 3000ms
");
            System.Threading.Thread.Sleep(3000);
        }
    }
}
