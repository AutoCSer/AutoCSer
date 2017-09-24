using System;
using System.Net;

namespace AutoCSer.Example.WebView.Ajax
{
    /// <summary>
    /// ref / out 参数支持 示例
    /// </summary>
    [AutoCSer.WebView.Ajax(IsExportTypeScript = true)]
    class RefOut : AutoCSer.WebView.Ajax<RefOut>
    {
        /// <summary>
        /// 测试方法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.WebView.AjaxMethod(IsReferer = false)]
        public int Add(int left, ref int right, out int mul)
        {
            int value = left + right;
            mul = left * right;
            right <<= 1;
            return value;
        }

        /// <summary>
        /// ref / out 参数支持返回值
        /// </summary>
        struct RefOutReturn
        {
            /// <summary>
            /// 
            /// </summary>
            public int Return;
            /// <summary>
            /// 
            /// </summary>
            public int right;
            /// <summary>
            /// 
            /// </summary>
            public int mul;
        }

        /// <summary>
        /// ref / out 参数支持 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add(AutoCSer.Net.Http.HeaderName.ContentType, "application/json; charset=utf-8");
                string json = webClient.UploadString(WebConfig.HttpDomain + @"Ajax?n=RefOut.Add", "POST", AutoCSer.Json.Serializer.Serialize(new AddParameter { left = 3, right = 5 }));
                RefOutReturn value = AutoCSer.Json.Parser.Parse<RefOutReturn>(json);
                if (value.Return != 3 + 5 || value.right != 5 << 1 || value.mul != 3 * 5)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
