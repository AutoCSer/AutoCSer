using System;
using System.Net;

namespace AutoCSer.Example.WebView.Ajax
{
    /// <summary>
    /// 自定义调用名称 示例
    /// </summary>
    class Name : AutoCSer.WebView.Ajax<Name>
    {
        /// <summary>
        /// 测试方法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.WebView.AjaxMethod(IsReferer = false, MethodName = "AddName")]
        public int Add(int left, int right)
        {
            return left + right;
        }
        /// <summary>
        /// 测试方法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.WebView.AjaxMethod(IsReferer = false, FullName = "AddFullName")]
        public int Add2(int left, int right)
        {
            return left + right;
        }

        /// <summary>
        /// 自定义调用名称 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add(AutoCSer.Net.Http.HeaderName.ContentType, "application/json; charset=utf-8");
                string json = webClient.UploadString(WebConfig.HttpDomain + @"Ajax?n=Name.AddName", "POST", AutoCSer.Json.Serializer.Serialize(new AddParameter { left = 1, right = 2 }));
                if (AutoCSer.Json.Parser.Parse<AddReturn>(json).Return != 1 + 2)
                {
                    return false;
                }

                webClient.Headers.Add(AutoCSer.Net.Http.HeaderName.ContentType, "application/json; charset=utf-8");
                json = webClient.UploadString(WebConfig.HttpDomain + @"Ajax?n=AddFullName", "POST", AutoCSer.Json.Serializer.Serialize(new AddParameter { left = 2, right = 3 }));
                if (AutoCSer.Json.Parser.Parse<AddReturn>(json).Return != 2 + 3)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
