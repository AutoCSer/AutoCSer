using System;

namespace AutoCSer.TestCase.HttpFilePerformance
{
    /// <summary>
    /// 网站生成配置
    /// </summary>
    [AutoCSer.Config.Type]
    [AutoCSer.WebView.Config]
    internal sealed class WebConfig : AutoCSer.WebView.Config
    {
        /// <summary>
        /// 是否进行WebView前期处理
        /// </summary>
        public override bool IsWebView
        {
            get { return false; }
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
                    MaxQueryCount = 0,
                    HeadSize = SubBuffer.Size.Kilobyte2,
                    BufferSize = SubBuffer.Size.Kilobyte2,
                    IsResponseServer = true,
                    IsResponseCacheControl = false,
                    IsResponseContentType = false,
                    IsResponseDate = true,
                    IsResponseLastModified = false
                };
            }
        }
    }
}
