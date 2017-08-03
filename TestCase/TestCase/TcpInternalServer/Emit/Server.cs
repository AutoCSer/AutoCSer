using System;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.TcpInternalServer.Emit
{
    /// <summary>
    /// TCP 服务测试
    /// </summary>
    class Server : IServer
    {
        /// <summary>
        /// 测试数据
        /// </summary>
        internal static int IntValue;
        /// <summary>
        /// 测试数据
        /// </summary>
        internal static long LongValue;
        /// <summary>
        /// 无参数无返回值调用测试
        /// </summary>
        public void Inc()
        {
            ++IntValue;
        }
        /// <summary>
        /// 无参数无返回值调用测试
        /// </summary>
        public AutoCSer.Net.TcpServer.ReturnValue IncLong()
        {
            ++LongValue;
            return AutoCSer.Net.TcpServer.ReturnType.Success;
        }
        /// <summary>
        /// 单参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        public void Set(int a)
        {
            IntValue = a;
        }
        /// <summary>
        /// 单参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        public AutoCSer.Net.TcpServer.ReturnValue Set(long a)
        {
            LongValue = a;
            return AutoCSer.Net.TcpServer.ReturnType.Success;
        }
        /// <summary>
        /// 单参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        public AutoCSer.Net.TcpServer.ReturnValue Set(string a)
        {
            IntValue = int.Parse(a);
            return AutoCSer.Net.TcpServer.ReturnType.Success;
        }
        /// <summary>
        /// 多参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public void Add(int a, int b)
        {
            IntValue = a + b;
        }
        /// <summary>
        /// 多参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public AutoCSer.Net.TcpServer.ReturnValue Add(long a, long b)
        {
            LongValue = a + b;
            return AutoCSer.Net.TcpServer.ReturnType.Success;
        }
        /// <summary>
        /// 多参数无返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public AutoCSer.Net.TcpServer.ReturnValue Add(string a, string b)
        {
            IntValue = int.Parse(a) + int.Parse(b);
            return AutoCSer.Net.TcpServer.ReturnType.Success;
        }
        /// <summary>
        /// 无参数有返回值调用测试
        /// </summary>
        /// <returns></returns>
        public int GetInc()
        {
            return ++IntValue;
        }
        /// <summary>
        /// 无参数有返回值调用测试
        /// </summary>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.ReturnValue<long> GetIncLong()
        {
            return ++LongValue;
        }
        /// <summary>
        /// 无参数有返回值调用测试
        /// </summary>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.ReturnValue<string> GetIncString()
        {
            return (++IntValue).toString();
        }
        /// <summary>
        /// 单参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public int GetAdd(int a)
        {
            return IntValue += a;
        }
        /// <summary>
        /// 单参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.ReturnValue<long> GetAdd(long a)
        {
            return LongValue += a;
        }
        /// <summary>
        /// 单参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.ReturnValue<string> GetAdd(string a)
        {
            return (IntValue += int.Parse(a)).toString();
        }
        /// <summary>
        /// 多参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int GetAdd(int a, int b)
        {
            return IntValue = a + b;
        }
        /// <summary>
        /// 多参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.ReturnValue<long> GetAdd(long a, long b)
        {
            return LongValue = a + b;
        }
        /// <summary>
        /// 多参数有返回值调用测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.ReturnValue<string> GetAdd(string a, string b)
        {
            return (IntValue = int.Parse(a) + int.Parse(b)).toString();
        }
        /// <summary>
        /// 输出参数测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public int GetAdd(int a, ref int b, out int c)
        {
            b += a;
            return c = a + b;
        }
        /// <summary>
        /// 输出参数测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.ReturnValue<long> GetAdd(long a, ref long b, out long c)
        {
            b += a;
            return c = a + b;
        }
        /// <summary>
        /// 输出参数测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.ReturnValue<string> GetAdd(string a, ref string b, out string c)
        {
            b = (int.Parse(a) + int.Parse(b)).toString();
            return c = (int.Parse(a) + int.Parse(b)).toString();
        }

        /// <summary>
        /// 无参数无返回值调用测试（服务端同步）
        /// </summary>
        public void IncSynchronous()
        {
            Inc();
        }
        /// <summary>
        /// 无参数无返回值调用测试（服务端同步）
        /// </summary>
        public AutoCSer.Net.TcpServer.ReturnValue IncLongSynchronous()
        {
            return IncLong();
        }
        /// <summary>
        /// 多参数无返回值调用测试（服务端同步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public void AddSynchronous(int a, int b)
        {
            Add(a, b);
        }
        /// <summary>
        /// 多参数无返回值调用测试（服务端同步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public AutoCSer.Net.TcpServer.ReturnValue AddSynchronous(long a, long b)
        {
            return Add(a, b);
        }
        /// <summary>
        /// 输出参数测试（服务端同步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.ReturnValue<long> GetAddSynchronous(long a, ref long b, out long c)
        {
            return GetAdd(a, ref b, out c);
        }
        /// <summary>
        /// 输出参数测试（服务端同步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.ReturnValue<string> GetAddSynchronous(string a, ref string b, out string c)
        {
            return GetAdd(a, ref b, out c);
        }

        /// <summary>
        /// 无参数无返回值调用测试（异步）
        /// </summary>
        /// <param name="onReturn"></param>
        public void IncLong(Func<AutoCSer.Net.TcpServer.ReturnValue, bool> onReturn)
        {
            onReturn(IncLong());
        }
        /// <summary>
        /// 多参数无返回值调用测试（异步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="onReturn"></param>
        public void Add(long a, long b, Func<AutoCSer.Net.TcpServer.ReturnValue, bool> onReturn)
        {
            onReturn(Add(a, b));
        }
        /// <summary>
        /// 多参数有返回值调用测试（异步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="onReturn"></param>
        public void GetAdd(int a, int b, Func<AutoCSer.Net.TcpServer.ReturnValue<Sum>, bool> onReturn)
        {
            onReturn(new Sum { Left = a, Right = b, Result = a + b });
        }
        /// <summary>
        /// 多参数有返回值调用测试（异步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="onReturn"></param>
        public void GetAdd(long a, long b, Func<AutoCSer.Net.TcpServer.ReturnValue<long>, bool> onReturn)
        {
            onReturn(GetAdd(a, b));
        }
        /// <summary>
        /// 多参数有返回值调用测试（异步）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="onReturn"></param>
        public void GetAdd(string a, string b, Func<AutoCSer.Net.TcpServer.ReturnValue<string>, bool> onReturn)
        {
            onReturn(GetAdd(a, b));
        }

        /// <summary>
        /// 无参数无返回值调用测试（异步回调保持）
        /// </summary>
        /// <param name="onReturn"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback IncLongKeepCallback(Func<AutoCSer.Net.TcpServer.ReturnValue, bool> onReturn)
        {
            onReturn(IncLong());
            onReturn(IncLong());
            return null;
        }
        /// <summary>
        /// 多参数有返回值调用测试（异步回调保持）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="onReturn"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback GetAddKeepCallback(int a, int b, Func<AutoCSer.Net.TcpServer.ReturnValue<int>, bool> onReturn)
        {
            onReturn(GetAdd(a, b));
            onReturn(GetAdd(a + 1, b + 2));
            return null;
        }

        /// <summary>
        /// 过期函数测试
        /// </summary>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.ReturnValue Expired()
        {
            return AutoCSer.Net.TcpServer.ReturnType.Success;
        }
        /// <summary>
        /// 服务端错误返回测试
        /// </summary>
        public AutoCSer.Net.TcpServer.ReturnValue ServerDeSerializeError()
        {
            return AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError;
        }
        /// <summary>
        /// 服务端异常测试
        /// </summary>
        public void ThrowException()
        {
            throw new Exception();
        }
        /// <summary>
        /// 服务端异常测试
        /// </summary>
        public AutoCSer.Net.TcpServer.ReturnValue ReturnThrowException()
        {
            throw new Exception();
        }

        /// <summary>
        /// TCP 服务测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            using (AutoCSer.Net.TcpInternalServer.Server server = AutoCSer.Net.TcpInternalServer.Emit.Server<IServer>.Create(new Server()))
            {
                if (server.IsListen)
                {
                    return testCase(AutoCSer.Net.TcpInternalServer.Emit.Client<IServer>.Create());
                }
            }
            return false;
        }
        /// <summary>
        /// TCP 服务测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected static bool testCase(IServer client)
        {
            using (client as IDisposable)
            using (System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false))
            {
                IntValue = 0;
                client.Inc();
                if (IntValue != 1) return false;

                LongValue = long.MaxValue - 2;
                client.IncLong();
                if (LongValue != long.MaxValue - 1) return false;

                client.Set(3);
                if (IntValue != 3) return false;

                client.Set(long.MaxValue - 3);
                if (LongValue != long.MaxValue - 3) return false;

                client.Set("5");
                if (IntValue != 5) return false;

                client.Add(4, 7);
                if (IntValue != 11) return false;

                client.Add(long.MaxValue >> 1, (long.MaxValue >> 1) - 1);
                if (LongValue != ((long.MaxValue >> 1) << 1) - 1) return false;

                client.Add("6", "11");
                if (IntValue != 17) return false;

                IntValue = 9;
                if (client.GetInc() != 10) return false;

                LongValue = long.MaxValue - 9;
                if (client.GetIncLong() != long.MaxValue - 8) return false;

                IntValue = 12;
                if (client.GetIncString() != "13") return false;

                IntValue = 4;
                if (client.GetAdd(3) != 7) return false;
                LongValue = (long.MaxValue >> 2) - 1;
                if (client.GetAdd((long.MaxValue >> 2) - 2) != ((long.MaxValue >> 2) << 1) - 3) return false;
                IntValue = 3;
                if (client.GetAdd("11") != "14") return false;

                if (client.GetAdd(8, 14) != 22) return false;
                if (client.GetAdd(long.MaxValue >> 2, (long.MaxValue >> 2) - 1) != ((long.MaxValue >> 2) << 1) - 1) return false;
                if (client.GetAdd("3", "15") != "18") return false;

                IntValue = 4;
                int outInt;
                if (client.GetAdd(5, ref IntValue, out outInt) != 14) return false;
                if (outInt != 14) return false;
                if (IntValue != 9) return false;

                LongValue = long.MaxValue >> 3;
                long outLong;
                if (client.GetAdd((long.MaxValue >> 3) - 5, ref LongValue, out outLong) != (long.MaxValue >> 3) * 3 - 10) return false;
                if (outLong != (long.MaxValue >> 3) * 3 - 10) return false;
                if (LongValue != ((long.MaxValue >> 3) << 1) - 5) return false;

                string refString = "6", outString;
                if (client.GetAdd("10", ref refString, out outString) != "26") return false;
                if (outString != "26") return false;
                if (refString != "16") return false;

                IntValue = 0;
                client.IncSynchronous();
                if (IntValue != 1) return false;

                LongValue = long.MaxValue - 2;
                client.IncLongSynchronous();
                if (LongValue != long.MaxValue - 1) return false;

                client.AddSynchronous(4, 7);
                if (IntValue != 11) return false;

                client.AddSynchronous(long.MaxValue >> 1, (long.MaxValue >> 1) - 1);
                if (LongValue != ((long.MaxValue >> 1) << 1) - 1) return false;

                LongValue = long.MaxValue >> 3;
                long outLongSynchronous;
                if (client.GetAddSynchronous((long.MaxValue >> 3) - 5, ref LongValue, out outLongSynchronous) != (long.MaxValue >> 3) * 3 - 10) return false;
                if (outLongSynchronous != (long.MaxValue >> 3) * 3 - 10) return false;
                if (LongValue != ((long.MaxValue >> 3) << 1) - 5) return false;

                string refStringSynchronous = "6", outStringSynchronous;
                if (client.GetAddSynchronous("10", ref refStringSynchronous, out outStringSynchronous) != "26") return false;
                if (outStringSynchronous != "26") return false;
                if (refStringSynchronous != "16") return false;

                LongValue = long.MaxValue - 2;
                bool isAsynchronous = false;
                client.IncLong(value =>
                {
                    isAsynchronous = value.Type == AutoCSer.Net.TcpServer.ReturnType.Success;
                    wait.Set();
                    return true;
                });
                wait.WaitOne();
                if (!isAsynchronous || LongValue != long.MaxValue - 1) return false;

                isAsynchronous = false;
                client.Add(long.MaxValue >> 1, (long.MaxValue >> 1) - 1, value =>
                {
                    isAsynchronous = value.Type == AutoCSer.Net.TcpServer.ReturnType.Success;
                    wait.Set();
                    return true;
                });
                wait.WaitOne();
                if (!isAsynchronous || LongValue != ((long.MaxValue >> 1) << 1) - 1) return false;

                isAsynchronous = false;
                client.GetAdd(8, 14, value =>
                {
                    isAsynchronous = value.Type == AutoCSer.Net.TcpServer.ReturnType.Success && value.Value.Left == 8 && value.Value.Right == 14 && value.Value.Result == 22;
                    wait.Set();
                    return true;
                });
                wait.WaitOne();
                if (!isAsynchronous) return false;

                isAsynchronous = false;
                client.GetAdd(long.MaxValue >> 2, (long.MaxValue >> 2) - 1, value =>
                {
                    isAsynchronous = value.Type == AutoCSer.Net.TcpServer.ReturnType.Success && value.Value == ((long.MaxValue >> 2) << 1) - 1;
                    wait.Set();
                    return true;
                });
                wait.WaitOne();
                if (!isAsynchronous) return false;

                isAsynchronous = false;
                client.GetAdd("3", "15", value =>
                {
                    isAsynchronous = value.Type == AutoCSer.Net.TcpServer.ReturnType.Success && value.Value == "18";
                    wait.Set();
                    return true;
                });
                wait.WaitOne();
                if (!isAsynchronous) return false;

                LongValue = long.MaxValue - 4;
                int keepCallbackCount = 2, successCount = 2;
                using (AutoCSer.Net.TcpServer.KeepCallback keepCallback = client.IncLongKeepCallback(value =>
                {
                    if (value.Type == AutoCSer.Net.TcpServer.ReturnType.Success) --successCount;
                    if (--keepCallbackCount == 0) wait.Set();
                    return true;
                }))
                {
                    wait.WaitOne();
                    if (successCount != 0 || LongValue != long.MaxValue - 2) return false;
                }

                keepCallbackCount = successCount = 2;
                using (AutoCSer.Net.TcpServer.KeepCallback keepCallback = client.GetAddKeepCallback(8, 14, value =>
                {
                    if (value.Type == AutoCSer.Net.TcpServer.ReturnType.Success)
                    {
                        switch (value.Value)
                        {
                            case 22:
                            case 25: --successCount; break;
                        }
                    }
                    if (--keepCallbackCount == 0) wait.Set();
                    return true;
                }))
                {
                    wait.WaitOne();
                    if (successCount != 0) return false;
                }

                if (client.Expired().Type != AutoCSer.Net.TcpServer.ReturnType.VersionExpired) return false;
                if (client.ServerDeSerializeError().Type != AutoCSer.Net.TcpServer.ReturnType.ServerDeSerializeError) return false;

                //try
                //{
                //    client.ThrowException();
                //    return false;
                //}
                //catch { }

                //AutoCSer.Net.TcpServer.ReturnValue value = client.ReturnThrowException();
                //if (value.Type != AutoCSer.Net.TcpServer.ReturnType.ServerException) return false;

                return true;
            }
        }
        /// <summary>
        /// 断线重连测试
        /// </summary>
        internal static void ConnectionTest()
        {
            IServer client = AutoCSer.Net.TcpInternalServer.Emit.Client<IServer>.Create();
            using (client as IDisposable)
            {
                long value;
                AutoCSer.Net.TcpServer.ReturnType returnType;
                do
                {
                    using (AutoCSer.Net.TcpInternalServer.Server server = AutoCSer.Net.TcpInternalServer.Emit.Server<IServer>.Create(new Server()))
                    {
                        if (server.IsListen)
                        {
                            value = LongValue;
                            returnType = client.IncLong().Type;
                            if (returnType == AutoCSer.Net.TcpServer.ReturnType.Success && LongValue == value + 1) Console.Write('.');
                            else Console.WriteLine(@"
" + returnType.ToString());
                        }
                        else Console.WriteLine(@"
Server is not listen");
                    }
                    returnType = client.IncLong().Type;
                    if (returnType == AutoCSer.Net.TcpServer.ReturnType.ClientDisposed) Console.Write('.');
                    else Console.WriteLine(@"
" + returnType.ToString());
                }
                while (true);
            }
        }
    }
}
