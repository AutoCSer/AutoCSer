using System;
using System.Net;

namespace AutoCSer.Example.WebView.Ajax
{
    /// <summary>
    /// 异步 API支持 示例
    /// </summary>
    class Asynchronous : AutoCSer.WebView.Ajax<Asynchronous>
    {
        /// <summary>
        /// 测试方法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="onAdd"></param>
        [AutoCSer.WebView.AjaxMethod(IsReferer = false)]
        public void Add(int left, int right, Action<AutoCSer.Net.TcpServer.ReturnValue<int>> onAdd)
        {
            onAdd(left + right);
        }

        /// <summary>
        /// 异步 API支持 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add(AutoCSer.Net.Http.HeaderName.ContentType, "application/json; charset=utf-8");
                string json = webClient.UploadString(WebConfig.HttpDomain + @"Ajax?n=Asynchronous.Add", "POST", AutoCSer.Json.Serializer.Serialize(new AddParameter { left = 5, right = 8 }));
                if (AutoCSer.Json.Parser.Parse<AddReturn>(json).Return != 5 + 8)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
