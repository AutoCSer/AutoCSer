//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.HttpFilePerformance
{

        /// <summary>
        /// WEB服务器
        /// </summary>
        public partial class WebServer : AutoCSer.Net.HttpDomainServer.ViewServer<int>
        {
            protected override string[] calls
            {
                get
                {
                    string[] names = new string[1];
                    names[0] = "/json";
                    return names;
                }
            }
            private static readonly AutoCSer.WebView.CallMethodInfo _i0 = new AutoCSer.WebView.CallMethodInfo { MethodIndex = 0, MaxMemoryStreamSize = (AutoCSer.SubBuffer.Size)131072, MaxPostDataSize = 4194304, IsOnlyPost = false };
            protected override void call(int callIndex, AutoCSer.Net.Http.SocketBase socket)
            {
                switch (callIndex)
                {
                    case 0:
                        loadAsynchronous(socket, AutoCSer.TestCase.WebPerformance.Json/**/.Pop() ?? new AutoCSer.TestCase.WebPerformance.Json(), _i0);
                        return;
                }
            }
            protected override bool call(AutoCSer.WebView.CallBase call, ref AutoCSer.UnmanagedStream responseStream)
            {
                switch (call.CallMethodIndex)
                {
                    default: return false;
                }
            }
            protected override bool call(AutoCSer.WebView.CallBase call)
            {
                switch (call.CallMethodIndex)
                {
                    case 0:
                        {
                                    {
                                        ((AutoCSer.TestCase.WebPerformance.Json)call).GetMessage();
                                        return true;
                                    }
                        }
                    default: return false;
                }
            }
        }
}
#endif