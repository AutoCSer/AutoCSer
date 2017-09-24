using System;
using System.Net;

namespace AutoCSer.Example.WebView.Ajax
{
    /// <summary>
    /// 序列化拆包 示例
    /// </summary>
    class BoxSerialize : AutoCSer.WebView.Ajax<BoxSerialize>
    {
        /// <summary>
        /// 测试方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [AutoCSer.WebView.AjaxMethod(IsReferer = false, IsInputSerializeBox = true, IsOutputSerializeBox = true)]
        public int Inc(int value)
        {
            return value + 1;
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
                string json = webClient.UploadString(WebConfig.HttpDomain + @"Ajax?n=BoxSerialize.Inc", "POST", AutoCSer.Json.Serializer.Serialize<int>(4));
                if (AutoCSer.Json.Parser.Parse<int>(json) != 5)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
