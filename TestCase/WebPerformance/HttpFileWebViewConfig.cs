using System;

namespace AutoCSer.TestCase.HttpFilePerformance
{
    /// <summary>
    /// 网站生成配置
    /// </summary>
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
    }
}
