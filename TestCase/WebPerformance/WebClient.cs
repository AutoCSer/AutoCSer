using System;
using System.Threading;
using System.Net.Sockets;
using AutoCSer.Extension;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Net;

namespace AutoCSer.TestCase.WebPerformance
{
    /// <summary>
    /// WEB 测试客户端
    /// </summary>
    internal unsafe sealed class WebClient : Client
    {
        /// <summary>
        /// 客户端任务池
        /// </summary>
        public sealed class Task : Client.Task<WebClient>
        {
            /// <summary>
            /// 客户端任务池
            /// </summary>
            /// <param name="maxClientCount">最大实例数量</param>
            /// <param name="isKeepAlive">保持连接最大次数</param>
            public Task(int maxClientCount, int keepAliveCount) : base(maxClientCount, keepAliveCount) { }
            /// <summary>
            /// 添加请求数量
            /// </summary>
            /// <param name="count">请求数量</param>
            public void Add(int count)
            {
                freeWait.Reset();
                Interlocked.Add(ref taskCount, count);
                while (add()) ;
            }
            /// <summary>
            /// 添加任务
            /// </summary>
            /// <returns></returns>
            private bool add()
            {
                if (Interlocked.Decrement(ref taskCount) >= 0)
                {
                    if (clientCount == clients.Length) return tryAdd();
                    if (Interlocked.Increment(ref clientCount) > clients.Length)
                    {
                        Interlocked.Decrement(ref clientCount);
                        return tryAdd();
                    }
                    try
                    {
                        (new WebClient(this)).request();
                        return true;
                    }
                    catch { }
                    Interlocked.Decrement(ref clientCount);
                    Interlocked.Increment(ref taskCount);
                    return true;
                }
                Interlocked.Increment(ref taskCount);
                return false;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            private bool tryAdd()
            {
                Monitor.Enter(clientLock);
                if (clientIndex == 0)
                {
                    Monitor.Exit(clientLock);
                    Interlocked.Increment(ref taskCount);
                    return clientIndex == clients.Length;
                }
                WebClient client = clients[--clientIndex];
                Monitor.Exit(clientLock);
                client.request();
                return true;
            }
            /// <summary>
            /// 下一个任务
            /// </summary>
            /// <param name="client">客户端</param>
            public void Next(WebClient client)
            {
                if (Interlocked.Decrement(ref taskCount) >= 0)
                {
                    client.request();
                }
                else
                {
                    Interlocked.Increment(ref taskCount);
                    Monitor.Enter(clientLock);
                    clients[clientIndex++] = client;
                    if (clientCount == clientIndex)
                    {
                        Monitor.Exit(clientLock);
                        freeWait.Set();
                    }
                    else
                    {
                        Monitor.Exit(clientLock);
                    }
                }
            }
        }
        /// <summary>
        /// 服务器IP地址与端口号
        /// </summary>
        private static readonly IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12201);
        /// <summary>
        /// 客户端任务池
        /// </summary>
        private Task task;
        /// <summary>
        /// 发送数据缓冲区
        /// </summary>
        private byte[] sendBuffer;
        /// <summary>
        /// 输出数据
        /// </summary>
        private string output;
        /// <summary>
        /// 测试左值
        /// </summary>
        private int left;
        /// <summary>
        /// 测试右值
        /// </summary>
        private int right;
        /// <summary>
        /// 异步操作字节数
        /// </summary>
        private int asyncSize;
        /// <summary>
        /// 测试类型
        /// </summary>
        private TestType testType;
        /// <summary>
        /// 文件测试客户端
        /// </summary>
        /// <param name="task"></param>
        private WebClient(Task task)
        {
            sendAsyncEventArgs.RemoteEndPoint = serverEndPoint;
            sendAsyncEventArgs.SetBuffer(sendBuffer = new byte[bufferSize], 0, bufferSize);
            sendAsyncEventArgs.Completed += onSend;
            recieveAsyncEventArgs.Completed += onReceive;
            this.task = task;
            output = new string((char)0, 64);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            if (sendAsyncEventArgs != null)
            {
                sendAsyncEventArgs.Completed -= onSend;
                SocketAsyncEventArgsPool.PushNotNull(ref sendAsyncEventArgs);
                recieveAsyncEventArgs.Completed -= onReceive;
                SocketAsyncEventArgsPool.PushNotNull(ref recieveAsyncEventArgs);
            }
        }
        /// <summary>
        /// 客户端请求
        /// </summary>
        private void request()
        {
            try
            {
                sendAsyncEventArgs.SocketError = SocketError.Success;
                ulong random = AutoCSer.Random.Default.NextULong();
                this.testType = loopTestType == TestType.Random ? (TestType)((byte)random & 7) : loopTestType;
                byte[] url = urls[(byte)testType];
                System.Buffer.BlockCopy(url, 0, sendBuffer, 0, asyncSize = url.Length);
                fixed (byte* bufferFixed = sendBuffer)
                {
                    asyncSize += Number.ToString(left = (int)(uint)random, bufferFixed + asyncSize);
                    *(long*)(bufferFixed + asyncSize) = '&' + ('r' << 8) + ('i' << 16) + ('g' << 24) + ((long)'h' << 32) + ((long)'t' << 40) + ((long)'=' << 48);
                    asyncSize += 7;
                    asyncSize += Number.ToString(right = (int)(uint)(random >> 32), bufferFixed + asyncSize);
                    if (keepAliveCount == 1 || task.KeepAliveCount == 1)
                    {
                        Buffer.BlockCopy(httpVersion, 0, sendBuffer, asyncSize, httpVersion.Length);
                        asyncSize += httpVersion.Length;
                    }
                    else
                    {
                        Buffer.BlockCopy(httpVersionKeepAlive, 0, sendBuffer, asyncSize, httpVersionKeepAlive.Length);
                        asyncSize += httpVersionKeepAlive.Length;
                    }
                    switch (testType)
                    {
                        case TestType.WebView:
                        case TestType.WebViewAsynchronous:
                            Buffer.BlockCopy(AutoCSerSpiderUserAgent, 0, sendBuffer, asyncSize, AutoCSerSpiderUserAgent.Length);
                            asyncSize += AutoCSerSpiderUserAgent.Length;
                            break;
                    }
                    *(short*)(bufferFixed + asyncSize) = 0x0a0d;
                }
                asyncSize += sizeof(short);
                if (keepAliveCount == 0)
                {
                    asyncType = ClientSocketAsyncType.Connect;
                    socket = new Socket(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
#if !MONO
                    socket.SendBufferSize = socket.ReceiveBufferSize = bufferSize;
#endif
                    socket.LingerState = lingerOption;
                    keepAliveCount = task.KeepAliveCount - 1;
                    sendAsyncEventArgs.SetBuffer(0, asyncSize);
                    if (socket.ConnectAsync(sendAsyncEventArgs)) return;
                    if (sendAsyncEventArgs.SocketError == SocketError.Success)
                    {
                        if (sendAsyncEventArgs.BytesTransferred == 0 ? send() : receive()) return;
                    }
                }
                else
                {
                    if (send()) return;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            onError();
        }
        /// <summary>
        /// 异步回调操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async"></param>
        private void onSend(object sender, SocketAsyncEventArgs async)
        {
            switch (asyncType)
            {
                case ClientSocketAsyncType.Connect:
                    if (sendAsyncEventArgs.SocketError == SocketError.Success)
                    {
                        if (sendAsyncEventArgs.BytesTransferred == 0 ? send() : receive()) return;
                    }
                    break;
                case ClientSocketAsyncType.Send:
                    if (receive()) return;
                    break;
            }
            onError();
        }
        /// <summary>
        /// 异步回调操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async"></param>
        private void onReceive(object sender, SocketAsyncEventArgs async)
        {
            if (!next()) onError();
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <returns></returns>
        private bool send()
        {
            asyncType = ClientSocketAsyncType.Send;
            sendAsyncEventArgs.SetBuffer(0, asyncSize);
            return socket.SendAsync(sendAsyncEventArgs) || receive();
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <returns></returns>
        private bool receive()
        {
            if (sendAsyncEventArgs.SocketError == SocketError.Success && sendAsyncEventArgs.BytesTransferred == asyncSize)
            {
                asyncType = ClientSocketAsyncType.Receive;
                recieveAsyncEventArgs.SetBuffer(0, bufferSize);
#if !DotNetStandard
                while (Interlocked.CompareExchange(ref receiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
#endif
                if (socket.ReceiveAsync(recieveAsyncEventArgs))
                {
#if !DotNetStandard
                    Interlocked.Exchange(ref receiveAsyncLock, 0);
#endif
                    return true;
                }
#if !DotNetStandard
                receiveAsyncLock = 0;
#endif
                return next();
            }
            return false;
        }
        /// <summary>
        /// 错误处理
        /// </summary>
        private void onError()
        {
            if (sendAsyncEventArgs.SocketError == SocketError.ConnectionRefused) Interlocked.Increment(ref task.RefusedCount);
            else
            {
                switch (asyncType)
                {
                    case ClientSocketAsyncType.Connect:
                    case ClientSocketAsyncType.Send:
                        Console.WriteLine("ERROR " + asyncType.ToString() + " " + sendAsyncEventArgs.SocketError.ToString() + " " + sendAsyncEventArgs.BytesTransferred.toString());
                        break;
                    case ClientSocketAsyncType.Receive:
                        Console.WriteLine("ERROR " + asyncType.ToString() + " " + recieveAsyncEventArgs.SocketError.ToString() + " " + recieveAsyncEventArgs.BytesTransferred.toString());
                        break;
                }
                Interlocked.Increment(ref task.ErrorCount);
            }
            keepAliveCount = 0;
            closeNext();
        }
        /// <summary>
        /// 处理下一个任务
        /// </summary>
        /// <returns></returns>
        private bool next()
        {
            if (recieveAsyncEventArgs.SocketError == SocketError.Success && recieveAsyncEventArgs.BytesTransferred > 0)
            {
                fixed (byte* bufferFixed = receiveBuffer)
                {
                    byte* end = bufferFixed + recieveAsyncEventArgs.BytesTransferred, start = FindLast(bufferFixed, end, 10);
                    if (start != null)
                    {
                        int bodyCount = (int)(end - ++start);
                        if (bodyCount < output.Length && bodyCount != 0)
                        {
                            fixed (char* outputFixed = output)
                            {
                                char* write = outputFixed;
                                while (start != end) *write++ = (char)*start++;
                            }
                        }
                        SubString json = new SubString(output, 0, bodyCount);
                        int value = ((byte)testType & 1) == 0 ? left + right : (left ^ right), returnValue;
                        switch (testType)
                        {
                            case TestType.WebCall:
                            case TestType.WebCallAsynchronous:
                                returnValue = value + 1;
                                AutoCSer.Json.Parser.Parse<int>(ref json, ref returnValue);
                                break;
                            case TestType.WebView:
                            case TestType.WebViewAsynchronous:
                            case TestType.WebViewAjax:
                            case TestType.WebViewAjaxAsynchronous:
                            case TestType.Ajax:
                            case TestType.AjaxAsynchronous:
                                JsonReturn jsonReturn = new JsonReturn();
                                if (AutoCSer.Json.Parser.Parse<JsonReturn>(ref json, ref jsonReturn)) returnValue = jsonReturn.Return;
                                else returnValue = value + 1;
                                break;
                            default: returnValue = value + 1; break;
                        }
                        if (returnValue == value)
                        {
                            if (keepAliveCount == 0) closeNext();
                            else task.Next(this);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 关闭连接并处理下一个任务
        /// </summary>
        private void closeNext()
        {
            Socket socket = this.socket;
            task.Next(this);
            socket.Close();
        }

        /// <summary>
        /// JSON 参数
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        internal struct JsonReturn
        {
            /// <summary>
            /// 参数值
            /// </summary>
            public int Return;
        }
        /// <summary>
        /// 查找最后一个字节,数据长度不能为0
        /// </summary>
        /// <param name="start">起始位置,不能为null</param>
        /// <param name="end">结束位置,不能为null</param>
        /// <param name="value">字节值</param>
        /// <returns>字节位置,失败为null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static byte* FindLast(byte* start, byte* end, byte value)
        {
            do
            {
                if (*--end == value) return end;
            }
            while (start != end);
            return null;
        }
        /// <summary>
        /// 测试 URL
        /// </summary>
        private static readonly byte[][] urls = new byte[][]
        {
            "GET /WebCall/Add?left=".getBytes(),
            "GET /WebCallAsynchronous/Xor?left=".getBytes(),
            "GET /WebView.html?left=".getBytes(),
            "GET /WebViewAsynchronous.html?left=".getBytes(),
            "GET /Ajax?n=/WebView.html&left=".getBytes(),
            "GET /Ajax?n=/WebViewAsynchronous.html&left=".getBytes(),
            "GET /Ajax?n=Call.Add&left=".getBytes(),
            "GET /Ajax?n=Call.Xor&left=".getBytes(),
        };
        /// <summary>
        /// HTTP版本
        /// </summary>
        private static readonly byte[] httpVersion = (@" HTTP/1.1
Host: 127.0.0.1
").getBytes();
        /// <summary>
        /// HTTP版本
        /// </summary>
        private static readonly byte[] httpVersionKeepAlive = (@" HTTP/1.1
Host: 127.0.0.1
Connection: Keep-Alive
").getBytes();
        /// <summary>
        /// AutoCSer爬虫标识
        /// </summary>
        private static readonly byte[] AutoCSerSpiderUserAgent = ("User-Agent: " + AutoCSer.Pub.HttpSpiderUserAgent + @"
").getBytes();
        /// <summary>
        /// 测试类型
        /// </summary>
        private static TestType loopTestType = TestType.Random;
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/WebView/Index.html
");
#if MONO
            int clientCountPerCpu = 256, maxClientCount = 512;
#else
            int clientCountPerCpu = 256, maxClientCount = 1024;
#endif
            bool isKeepAlive = false;

            int cpuCount = AutoCSer.Threading.Pub.CpuCount, maxSocketCount = Math.Min(cpuCount * clientCountPerCpu, maxClientCount), count = maxSocketCount * 256;
            int keepAliveCount = isKeepAlive ? ((count <<= 2) + (maxSocketCount - 1)) / maxSocketCount : 1;
            using (Task task = new Task(maxSocketCount, keepAliveCount))
            {
                do
                {
                    Console.WriteLine("Start request " + maxSocketCount.toString() + " / " + count.toString() + (isKeepAlive ? " +KeppAlive" : null));
                    task.ErrorCount = task.RefusedCount = 0;
                    long time = AutoCSer.Pub.StopwatchTicks;
                    task.Add(count);
                    task.Wait();
                    time = AutoCSer.Pub.GetStopwatchTicks(time);
                    task.CloseClient();
                    long milliseconds = (long)new TimeSpan(time).TotalMilliseconds;
                    Console.WriteLine(@"Finally[" + count.toString() + "] Error[" + task.ErrorCount.toString() + "] Refused[" + task.RefusedCount.toString() + "] " + milliseconds.toString() + "ms" + (milliseconds == 0 ? null : ("[" + ((count - task.RefusedCount) / milliseconds).toString() + "/ms]")) + " " + loopTestType.ToString());
                    Console.WriteLine(@"Sleep 3000ms
");
                    Thread.Sleep(3000);
                    if (loopTestType == TestType.Random) loopTestType = TestType.WebCall;
                    else ++loopTestType;
                }
                while (true);
            }
        }
    }
}
