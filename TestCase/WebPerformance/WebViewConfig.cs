using System;

namespace AutoCSer.TestCase.WebPerformance
{
    /// <summary>
    /// 网站生成配置
    /// </summary>
    [AutoCSer.WebView.Config]
    internal sealed class WebViewConfig : AutoCSer.WebView.Config
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
    }
}
