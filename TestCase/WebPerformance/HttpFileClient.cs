using System;
using System.Diagnostics;
using AutoCSer.Extension;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Net;

namespace AutoCSer.TestCase.WebPerformance
{
    /// <summary>
    /// 文件测试客户端
    /// </summary>
    internal sealed class HttpFileClient : Client
    {
        /// <summary>
        /// 客户端任务池
        /// </summary>
        public sealed class Task : Client.Task<HttpFileClient>
        {
            /// <summary>
            /// 客户端任务池
            /// </summary>
            /// <param name="maxClientCount">最大实例数量</param>
            /// <param name="isKeepAlive">保持连接最大次数</param>
            /// <param name="pipeline">客户端批量处理数量</param>
            public Task(int maxClientCount, int keepAliveCount)
                : base(maxClientCount, keepAliveCount)
            {
            }
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
                        (new HttpFileClient(this)).request();
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
                HttpFileClient client = clients[--clientIndex];
                Monitor.Exit(clientLock);
                client.request();
                return true;
            }
            /// <summary>
            /// 下一个任务
            /// </summary>
            /// <param name="client">客户端</param>
            public void Next(HttpFileClient client)
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
        private static readonly IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12200);
        /// <summary>
        /// 请求数据
        /// </summary>
        private static SubArray<byte> requestData;
        //        private static readonly byte[] requestData = Encoding.ASCII.GetBytes(keepRequestString + @"GET /hello.html HTTP/1.1
        //Host: 127.0.0.1
        //
        //");
        ///// <summary>
        ///// 请求数据字节数
        ///// </summary>
        //private static readonly int requestSize = requestData.Length - keepRequestString.Length;
        //        /// <summary>
        //        /// 正确的接收数据字节数
        //        /// </summary>
        //        private static readonly int receiveSize = @"HTTP/1.1 200 OK
        //Content-Length: 10
        //
        //HelloWorld".Length;
        /// <summary>
        /// 正确的接收数据字节数
        /// </summary>
        private static int receiveKeepAliveSize;
        /// <summary>
        /// 客户端任务池
        /// </summary>
        private Task task;
        /// <summary>
        /// 客户端接收数据字节数
        /// </summary>
        private int pipelineReceiveSize;
        /// <summary>
        /// 文件测试客户端
        /// </summary>
        /// <param name="task"></param>
        private HttpFileClient(Task task)
        {
            sendAsyncEventArgs.RemoteEndPoint = serverEndPoint;
            //sendAsyncEventArgs.SetBuffer(requestData, 0, keepRequestString.Length);
            sendAsyncEventArgs.Completed += onSend;
            recieveAsyncEventArgs.Completed += onReceive;
            this.task = task;
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
                sendAsyncEventArgs.SetBuffer(requestData.BufferArray, requestData.StartIndex, requestData.Count);
                if (keepAliveCount == 0)
                {
                    asyncType = ClientSocketAsyncType.Connect;
                    socket = new Socket(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
#if !MONO
                    socket.SendBufferSize = socket.ReceiveBufferSize = bufferSize;
#endif
                    socket.LingerState = lingerOption;
                    //if (task.KeepAliveCount == 1)
                    //{
                    //    keepAliveCount = 0;
                    //    sendAsyncEventArgs.SetBuffer(keepRequestString.Length, asyncSize = requestSize);
                    //}
                    //else
                    //{
                    //    keepAliveCount = task.KeepAliveCount - 1;
                    //    sendAsyncEventArgs.SetBuffer(0, asyncSize = keepRequestString.Length);
                    //}
                    keepAliveCount = task.KeepAliveCount - 1;
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
            if (!checkReceive()) onError();
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <returns></returns>
        private bool send()
        {
            asyncType = ClientSocketAsyncType.Send;
            //if (--keepAliveCount == 0) sendAsyncEventArgs.SetBuffer(keepRequestString.Length, asyncSize = requestSize);
            //else sendAsyncEventArgs.SetBuffer(0, asyncSize = keepRequestString.Length);
            return socket.SendAsync(sendAsyncEventArgs) || receive();
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <returns></returns>
        private bool receive()
        {
            //if (sendAsyncEventArgs.SocketError == SocketError.Success && sendAsyncEventArgs.BytesTransferred == asyncSize)
            if (sendAsyncEventArgs.SocketError == SocketError.Success && sendAsyncEventArgs.BytesTransferred == requestData.Count)
            {
                asyncType = ClientSocketAsyncType.Receive;
                recieveAsyncEventArgs.SetBuffer(0, bufferSize);
                pipelineReceiveSize = receiveKeepAliveSize;
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
                return checkReceive();
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
        /// 检测数据接收
        /// </summary>
        /// <returns></returns>
        private bool checkReceive()
        {
        CHECK:
            if (recieveAsyncEventArgs.SocketError == SocketError.Success)
            {
                int receiveSize = recieveAsyncEventArgs.BytesTransferred;
                if (receiveSize > 0 && (pipelineReceiveSize -= receiveSize) >= 0)
                {
                    if (pipelineReceiveSize == 0)
                    {
                        if (keepAliveCount == 0) closeNext();
                        else task.Next(this);
                        return true;
                    }
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
                    goto CHECK;
                }
            }
            return false;
        }
        ///// <summary>
        ///// 处理下一个任务
        ///// </summary>
        ///// <returns></returns>
        //private bool next()
        //{
        //    if (recieveAsyncEventArgs.SocketError == SocketError.Success && recieveAsyncEventArgs.BytesTransferred == (task.KeepAliveCount == 1 ? receiveSize : receiveKeepAliveSize))
        //    {
        //        if (keepAliveCount == 0) closeNext();
        //        else task.Next(this);
        //        return true;
        //    }
        //    return false;
        //}
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
        /// 测试类型
        /// </summary>
        private static TestType loopTestType = TestType.Json;
        /// <summary>
        /// 创建请求数据
        /// </summary>
        /// <param name="requestString"></param>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        private static byte[] createRequestData(string requestString, int pipeline)
        {
            if (pipeline == 1) return Encoding.ASCII.GetBytes(requestString);
            StringArray strings = new StringArray();
            for (int index = pipeline; index != 0; --index) strings.Add(requestString);
            return Encoding.ASCII.GetBytes(strings.ToString());
        }
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/WebView/Index.html
");
            int clientCountPerCpu = 256, maxClientCount = 256;
            bool isKeepAlive = true;

            byte[][] requestDatas = new byte[(byte)TestType.HelloFile + 1][];
            requestDatas[(byte)TestType.HelloFile] = createRequestData(@"GET /hello.html HTTP/1.1
Host: 127.0.0.1
Connection: Keep-Alive

", 16);
            requestDatas[(byte)TestType.Json] = createRequestData(@"GET /json HTTP/1.1
Host: 127.0.0.1
Connection: Keep-Alive

", 16);
            int[] responseSizes = new int[(byte)TestType.HelloFile + 1];
            responseSizes[(byte)TestType.HelloFile] = @"HTTP/1.1 200 OK
Content-Length: 10
Connection: Keep-Alive
Server: AutoCSer.HTTP/1.1
Date: Mon, 15 May 2017 12:16:35 GMT

Hello, World!".Length;
            responseSizes[(byte)TestType.Json] = @"HTTP/1.1 200 OK
Content-Length: 27
Connection: Keep-Alive
Server: AutoCSer.HTTP/1.1
Date: Mon, 15 May 2017 12:16:35 GMT

{""message"":""Hello, World!""}".Length;

            int cpuCount = AutoCSer.Threading.Pub.CpuCount, maxSocketCount = Math.Min(cpuCount * clientCountPerCpu, maxClientCount);
            using (Task task = new Task(maxSocketCount, 1))
            {
                do
                {
                    for (int pipeline = 16; pipeline != 0; pipeline >>= 1)
                    {
                        int count = maxSocketCount * pipeline * 256, keepAliveCount = isKeepAlive ? ((count <<= 2) + (maxSocketCount - 1)) / maxSocketCount : 1;
                        task.KeepAliveCount = keepAliveCount;
                        loopTestType = TestType.Json;
                        do
                        {
                            Console.WriteLine("Start request " + maxSocketCount.toString() + " / " + count.toString() + (isKeepAlive ? " +KeppAlive" : null) + (pipeline == 1 ? null : (" +pipeline[" + pipeline.toString() + "]")));
                            task.ErrorCount = task.RefusedCount = 0;
                            requestData = new SubArray<byte>(requestDatas[(byte)loopTestType], 0, requestDatas[(byte)loopTestType].Length / 16 * pipeline);
                            receiveKeepAliveSize = responseSizes[(byte)loopTestType] * pipeline;
                            long time = AutoCSer.Pub.StopwatchTicks;
                            task.Add(count / pipeline);
                            task.Wait();
                            time = AutoCSer.Pub.GetStopwatchTicks(time);
                            task.CloseClient();
                            long milliseconds = (long)new TimeSpan(time).TotalMilliseconds;
                            Console.WriteLine(@"Finally[" + count.toString() + "] Error[" + (task.ErrorCount * pipeline).toString() + "] Refused[" + task.RefusedCount.toString() + "] " + milliseconds.toString() + "ms" + (milliseconds == 0 ? null : ("[" + ((count - task.RefusedCount) / milliseconds).toString() + "/ms]")) + " " + loopTestType.ToString());
                            Console.WriteLine(@"Sleep 3000ms
");
                            Thread.Sleep(3000);
                        }
                        while (++loopTestType != TestType.LoopEnd);
                    }
                }
                while (true);
            }
        }
    }
}
