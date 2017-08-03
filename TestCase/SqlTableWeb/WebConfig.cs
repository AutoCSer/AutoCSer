using System;

namespace AutoCSer.TestCase.SqlTableWeb
{
    /// <summary>
    /// 网站生成配置
    /// </summary>
    [AutoCSer.WebView.Config]
    internal sealed class WebConfig : AutoCSer.WebView.Config
    {
        /// <summary>
        /// 默认主域名
        /// </summary>
        public override string MainDomain
        {
            get { return "127.0.0.1:12303"; }
        }
        /// <summary>
        /// WEB Path 导出引导类型
        /// </summary>
        public override Type ExportPathType
        {
            get { return typeof(SqlModel.WebPath.Class); }
        }
    }
}
