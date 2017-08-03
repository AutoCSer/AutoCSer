using System;

namespace AutoCSer.Web.WebView
{
    /// <summary>
    /// 前后端一体 WEB 视图框架 HTML 模板视图
    /// </summary>
    internal partial class Template : AutoCSer.WebView.View<Template>
    {
        /// <summary>
        /// HTML 模板值前缀
        /// </summary>
        public string At
        {
            get { return "=@"; }
        }
    }
}
