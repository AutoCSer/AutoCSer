using System;
using System.Net;
using System.Text;

namespace AutoCSer.Example.WebView.Ajax
{
    /// <summary>
    /// GET 请求支持 示例
    /// </summary>
    class Get : AutoCSer.WebView.Ajax<Get>
    {
        /// <summary>
        /// 测试方法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.WebView.AjaxMethod(IsReferer = false, IsOnlyPost = false)]
        public int Add(int left, int right)
        {
            return left + right;
        }

        /// <summary>
        /// GET 请求支持 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            string addUrl = WebConfig.HttpDomain + @"Ajax?n=Get.Add";
            using (WebClient webClient = new WebClient())
            {
                #region Query + JSON 支持
                string json = webClient.DownloadString(addUrl + "&j=" + System.Web.HttpUtility.UrlEncode(AutoCSer.Json.Serializer.Serialize(new AddParameter { left = 1, right = 2 })));
                if (AutoCSer.Json.Parser.Parse<AddReturn>(json).Return != 1 + 2)
                {
                    return false;
                }
                #endregion

                #region Query + XML 支持
                json = webClient.DownloadString(addUrl + "&x=" + System.Web.HttpUtility.UrlEncode(AutoCSer.Xml.Serializer.Serialize(new AddParameter { left = 2, right = 3 })));
                if (AutoCSer.Json.Parser.Parse<AddReturn>(json).Return != 2 + 3)
                {
                    return false;
                }
                #endregion

                #region Query 支持
                json = webClient.DownloadString(addUrl + "&left=3&right=5");
                if (AutoCSer.Json.Parser.Parse<AddReturn>(json).Return != 3 + 5)
                {
                    return false;
                }
                #endregion
            }
            return true;
        }
    }
}
