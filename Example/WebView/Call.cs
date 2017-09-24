using System;
using System.Net;
using System.Collections.Specialized;
using System.Text;

namespace AutoCSer.Example.WebView
{
    /// <summary>
    /// HTTP 调用函数 示例
    /// </summary>
    class Call : AutoCSer.WebView.Call<Call>
    {
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
        /// HTTP 调用函数 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            string url = WebConfig.HttpDomain + @"Call/Add";
            using (WebClient webClient = new WebClient())
            {
                #region FORM + JSON 支持
                NameValueCollection form = new NameValueCollection();
                form.Add("j", AutoCSer.Json.Serializer.Serialize(new AddParameter { left = 1, right = 2 }));
                string json = Encoding.UTF8.GetString(webClient.UploadValues(url, form));
                if (AutoCSer.Json.Parser.Parse<int>(json) != 1 + 2)
                {
                    return false;
                }
                #endregion

                #region FORM + XML 支持
                form = new NameValueCollection();
                form.Add("x", AutoCSer.Xml.Serializer.Serialize(new AddParameter { left = 2, right = 3 }));
                json = Encoding.UTF8.GetString(webClient.UploadValues(url, form));
                if (AutoCSer.Json.Parser.Parse<int>(json) != 2 + 3)
                {
                    return false;
                }
                #endregion

                #region POST + JSON 支持
                webClient.Headers.Add(AutoCSer.Net.Http.HeaderName.ContentType, "application/json; charset=utf-8");
                json = webClient.UploadString(url, "POST", AutoCSer.Json.Serializer.Serialize(new AddParameter { left = 3, right = 5 }));
                if (AutoCSer.Json.Parser.Parse<int>(json) != 3 + 5)
                {
                    return false;
                }
                #endregion

                #region POST + XML 支持
                webClient.Headers.Add(AutoCSer.Net.Http.HeaderName.ContentType, "application/xml; charset=utf-8");
                json = webClient.UploadString(url, "POST", AutoCSer.Xml.Serializer.Serialize(new AddParameter { left = 5, right = 8 }));
                if (AutoCSer.Json.Parser.Parse<int>(json) != 5 + 8)
                {
                    return false;
                }
                #endregion

                #region Query + JSON 支持
                json = webClient.DownloadString(url + "?j=" + System.Web.HttpUtility.UrlEncode(AutoCSer.Json.Serializer.Serialize(new AddParameter { left = 1, right = 2 })));
                if (AutoCSer.Json.Parser.Parse<int>(json) != 1 + 2)
                {
                    return false;
                }
                #endregion

                #region Query + XML 支持
                json = webClient.DownloadString(url + "?x=" + System.Web.HttpUtility.UrlEncode(AutoCSer.Xml.Serializer.Serialize(new AddParameter { left = 2, right = 3 })));
                if (AutoCSer.Json.Parser.Parse<int>(json) != 2 + 3)
                {
                    return false;
                }
                #endregion

                #region Query 支持
                json = webClient.DownloadString(url + "?left=3&right=5");
                if (AutoCSer.Json.Parser.Parse<int>(json) != 3 + 5)
                {
                    return false;
                }
                #endregion
            }
            return true;
        }
    }
}
