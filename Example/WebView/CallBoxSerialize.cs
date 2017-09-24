using System;
using System.Net;

namespace AutoCSer.Example.WebView
{
    /// <summary>
    /// 序列化拆包 示例
    /// </summary>
    class CallBoxSerialize : AutoCSer.WebView.Call<CallBoxSerialize>
    {
        /// <summary>
        /// 测试方法
        /// </summary>
        /// <param name="value"></param>
        [AutoCSer.WebView.CallMethod(IsFirstParameter = true)]
        public void Inc(int value)
        {
            Response(value + 1);
        }

        /// <summary>
        /// 序列化拆包 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add(AutoCSer.Net.Http.HeaderName.ContentType, "application/json; charset=utf-8");
                string json = webClient.UploadString(WebConfig.HttpDomain + @"CallBoxSerialize/Inc", "POST", AutoCSer.Json.Serializer.Serialize<int>(5));
                if (AutoCSer.Json.Parser.Parse<int>(json) != 6)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
