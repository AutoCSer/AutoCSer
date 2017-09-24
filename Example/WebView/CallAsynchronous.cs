using System;
using AutoCSer.Extension;
using System.Net;

namespace AutoCSer.Example.WebView
{
    /// <summary>
    /// HTTP 异步调用函数 示例
    /// </summary>
    class CallAsynchronous : AutoCSer.WebView.CallAsynchronous<CallAsynchronous>
    {
        /// <summary>
        /// 测试方法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public void Add(int left, int right)
        {
            RepsonseEnd((left + right).toString(), true);
        }
        /// <summary>
        /// 输出缓冲区池测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public void AddBuffer(int left, int right)
        {
            Pointer.Size buffer = AutoCSer.UnmanagedPool.Tiny.GetSize();
            try
            {
                if (CreateResponse(ref buffer))
                {
                    Response(left + right);
                    RepsonseEnd();
                }
            }
            finally { AutoCSer.UnmanagedPool.Tiny.Push(ref buffer); }
        }

        /// <summary>
        /// HTTP 异步调用函数 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add(AutoCSer.Net.Http.HeaderName.ContentType, "application/json; charset=utf-8");
                string json = webClient.UploadString(WebConfig.HttpDomain + @"CallAsynchronous/Add", "POST", AutoCSer.Json.Serializer.Serialize(new AddParameter { left = 3, right = 5 }));
                if (AutoCSer.Json.Parser.Parse<int>(json) != 3 + 5)
                {
                    return false;
                }

                webClient.Headers.Add(AutoCSer.Net.Http.HeaderName.ContentType, "application/json; charset=utf-8");
                json = webClient.UploadString(WebConfig.HttpDomain + @"CallAsynchronous/AddBuffer", "POST", AutoCSer.Json.Serializer.Serialize(new AddParameter { left = 2, right = 3 }));
                if (AutoCSer.Json.Parser.Parse<int>(json) != 2 + 3)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
