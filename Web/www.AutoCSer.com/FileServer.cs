using System;
using AutoCSer.IO;
using AutoCSer.Extensions;
using System.IO;

namespace AutoCSer.Web
{
    /// <summary>
    /// 静态文件服务
    /// </summary>
     sealed class FileServer : AutoCSer.Net.HttpDomainServer.StaticFileServer
    {
        /// <summary>
        /// 网站生成配置
        /// </summary>
        /// <returns>网站生成配置</returns>
        protected override AutoCSer.WebView.Config getWebConfig()
        {
            return WebServer.WebConfig;
        }
        /// <summary>
        /// 获取缓存文件名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override unsafe string getCacheFileName(SubArray<byte> path)
        {
            if (AutoCSer.Memory.Common.equal(exampleFileName, ref path))
            {
                if (exampleCacheFileName == null) exampleCacheFileName = WorkPath + "Download" + Path.DirectorySeparatorChar + "AutoCSer.zip";
                return exampleCacheFileName;
            }
            return base.getCacheFileName(path);
        }
        /// <summary>
        /// 下载压缩包缓存文件名称
        /// </summary>
        private string exampleCacheFileName;

        /// <summary>
        /// 下载压缩包文件名称
        /// </summary>
        private static SubArray<byte> exampleFileName;
        static FileServer()
        {
            exampleFileName = new SubArray<byte>(System.Text.Encoding.ASCII.GetBytes(("Download/AutoCSer.Example.zip").FileNameToLower()));
        }
    }
}
