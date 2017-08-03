using System;
using System.Net;

namespace AutoCSer.Example.WebView
{
    /// <summary>
    /// URL 重定向 示例
    /// </summary>
    class Location : AutoCSer.WebView.Call<Location>
    {
        /// <summary>
        /// URL 重定向
        /// </summary>
        public void Index()
        {
            location("/Location/Add?left=1&right=2");
        }

        /// <summary>
        /// 测试方法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public void Add(int left, int right)
        {
            Response(left + right);
        }

        /// <summary>
        /// URL 重定向 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (WebClient webClient = new WebClient())
            {
                string json = webClient.DownloadString(WebConfig.HttpDomain+ "Location/Index");
                if (AutoCSer.Json.Parser.Parse<int>(json) != 1 + 2)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
