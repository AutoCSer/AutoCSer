using System;
using System.Net;
using System.Text;

namespace AutoCSer.Example.WebView
{
    /// <summary>
    /// 文件上传 示例
    /// </summary>
    class Upload : AutoCSer.WebView.Call<Upload>
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        public void File()
        {
            byte[] data = System.IO.File.ReadAllBytes(fileName);
            foreach (AutoCSer.Net.Http.FormValue file in Files)
            {
                if (file.SaveFileName == null)
                {
                    if (AutoCSer.Memory.equal(file.Value.ToArray(), data))
                    {
                        Response(1);
                        return;
                    }
                }
                else
                {
                    if (AutoCSer.Memory.equal(System.IO.File.ReadAllBytes(file.SaveFileName), data))
                    {
                        Response(2);
                        return;
                    }
                }
            }
            Response(0);
        }

        /// <summary>
        /// 测试文件名称
        /// </summary>
#if DotNetStandard
        private const string fileName = @"..\..\..\Upload.cs";
#else
        private const string fileName = @"..\..\Upload.cs";
#endif
        /// <summary>
        /// 文件上传 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (WebClient webClient = new WebClient())
            {
                string json = Encoding.UTF8.GetString(webClient.UploadFile(WebConfig.HttpDomain + "Upload/File", fileName));
                if (AutoCSer.Json.Parser.Parse<int>(json) <= 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
