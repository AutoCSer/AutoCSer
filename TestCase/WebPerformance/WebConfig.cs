using System;

namespace AutoCSer.TestCase.WebPerformance
{
    /// <summary>
    /// 网站生成配置
    /// </summary>
    [AutoCSer.Config.Type]
    [AutoCSer.WebView.Config]
    internal sealed class WebConfig : AutoCSer.WebView.Config
    {
        /// <summary>
        /// 默认主域名
        /// </summary>
        public override string MainDomain
        {
            get { return "127.0.0.1:12201"; }
        }
        /// <summary>
        /// 是否复制js脚本文件
        /// </summary>
        public override bool IsCopyScript
        {
            get { return false; }
        }
        /// <summary>
        /// HTTP 配置
        /// </summary>
        [AutoCSer.Config.Member]
        public static AutoCSer.Net.Http.Config HttpConfig
        {
            get
            {
                return new AutoCSer.Net.Http.Config
                {
                    MaxHeaderCount = 0,
                    MaxQueryCount = 4,
                    HeadSize = SubBuffer.Size.Kilobyte2,
                    BufferSize = SubBuffer.Size.Kilobyte2,
                    IsResponseServer = false,
                    IsResponseCacheControl = false,
                    IsResponseContentType = false,
                    IsResponseDate = false,
                    IsResponseLastModified = false
                };
            }
        }
    }
}
