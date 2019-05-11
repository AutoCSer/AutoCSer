using System;
using System.IO;
using System.Net;

namespace AutoCSer.Example.WebView
{
    /// <summary>
    /// 文件输出 示例
    /// </summary>
    class File : AutoCSer.WebView.CallAsynchronous<File>
    {
        /// <summary>
        /// 文件输出
        /// </summary>
        public void Download()
        {
            RepsonseEnd(new FileInfo(fileName));
        }

        /// <summary>
        /// 测试文件名称
        /// </summary>
#if DotNetStandard
        private const string fileName = @"..\..\..\File.cs";
#else
        private const string fileName = @"..\..\File.cs";
#endif
        /// <summary>
        /// 文件输出 测试
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(WebConfig.HttpDomain + "File/Download");
                if (!AutoCSer.Memory.equal(data, System.IO.File.ReadAllBytes(fileName)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
