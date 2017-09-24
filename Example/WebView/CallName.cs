using System;
using System.Net;

namespace AutoCSer.Example.WebView
{
    /// <summary>
    /// 自定义 URL 调用 示例
    /// </summary>
    class CallName : AutoCSer.WebView.Call<CallName>
    {
        /// <summary>
        /// 测试方法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        [AutoCSer.WebView.CallMethod(FullName = "CallNameAdd")]
        public void Add(int left, int right)
        {
            Response(left + right);
        }

        /// <summary>
        /// 自定义 URL 调用 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add(AutoCSer.Net.Http.HeaderName.ContentType, "application/json; charset=utf-8");
                string json = webClient.UploadString(WebConfig.HttpDomain + @"CallNameAdd", "POST", AutoCSer.Json.Serializer.Serialize(new AddParameter { left = 5, right = 8 }));
                if (AutoCSer.Json.Parser.Parse<int>(json) != 5 + 8)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
