using System;
using System.Net;
using System.Text;
using System.Collections.Specialized;

namespace AutoCSer.Example.WebView.Ajax
{
    /// <summary>
    /// 默认测试 示例
    /// </summary>
    class Post : AutoCSer.WebView.Ajax<Post>
    {
        /// <summary>
        /// 测试方法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
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
        public int Mul(int left, int right)
        {
            return left * right;
        }
        /// <summary>
        /// 忽略实例方法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Metadata.Ignore]
        public int Ignore(int left, int right)
        {
            return left + right;
        }

        /// <summary>
        /// 默认测试 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            string addUrl = WebConfig.HttpDomain + @"Ajax?n=Post.Add", mulUrl = WebConfig.HttpDomain + @"Ajax?n=Post.Mul";
            using (WebClient webClient = new WebClient())
            {
                #region FORM + JSON 支持
                NameValueCollection form = new NameValueCollection();
                form.Add("j", AutoCSer.Json.Serializer.Serialize(new AddParameter { left = 1, right = 2 }));
                webClient.Headers.Add("Referer", WebConfig.HttpDomain);
                string json = Encoding.UTF8.GetString(webClient.UploadValues(addUrl, form));
                if (AutoCSer.Json.Parser.Parse<AddReturn>(json).Return != 1 + 2)
                {
                    return false;
                }
                #endregion

                #region FORM + XML 支持
                form = new NameValueCollection();
                form.Add("x", AutoCSer.Xml.Serializer.Serialize(new AddParameter { left = 2, right = 3 }));
                webClient.Headers.Add("Referer", WebConfig.HttpDomain);
                json = Encoding.UTF8.GetString(webClient.UploadValues(addUrl, form));
                if (AutoCSer.Json.Parser.Parse<AddReturn>(json).Return != 2 + 3)
                {
                    return false;
                }
                #endregion

                #region POST + JSON 支持
                webClient.Headers.Add(AutoCSer.Net.Http.HeaderName.ContentType, "application/json; charset=utf-8");
                webClient.Headers.Add("Referer", WebConfig.HttpDomain);
                json = webClient.UploadString(mulUrl, "POST", AutoCSer.Json.Serializer.Serialize(new AddParameter { left = 3, right = 5 }));
                if (AutoCSer.Json.Parser.Parse<AddReturn>(json).Return != 3 * 5)
                {
                    return false;
                }
                #endregion

                #region POST + XML 支持
                webClient.Headers.Add(AutoCSer.Net.Http.HeaderName.ContentType, "application/xml; charset=utf-8");
                webClient.Headers.Add("Referer", WebConfig.HttpDomain);
                json = webClient.UploadString(mulUrl, "POST", AutoCSer.Xml.Serializer.Serialize(new AddParameter { left = 5, right = 8 }));
                if (AutoCSer.Json.Parser.Parse<AddReturn>(json).Return != 5 * 8)
                {
                    return false;
                }
                #endregion
            }
            return true;
        }
    }
}
