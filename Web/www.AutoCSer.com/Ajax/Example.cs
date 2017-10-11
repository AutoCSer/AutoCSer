using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Web.Ajax
{
    /// <summary>
    /// 示例代码 AJAX 调用
    /// </summary>
    [AutoCSer.WebView.Ajax(IsExportTypeScript = true)]
    class Example : Code<Example>
    {
        /// <summary>
        /// 示例代码路径
        /// </summary>
        private static readonly string path = (AutoCSer.Web.Config.Deploy.ServerPath + @"Example\").FileNameToLower();
        /// <summary>
        /// 获取示例代码
        /// </summary>
        /// <param name="file">代码路径</param>
        /// <returns>示例代码</returns>
        [AutoCSer.WebView.AjaxMethod(IsReferer = false, IsOnlyPost = false, IsVersion = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public string GetCode(string file)
        {
            return getCode(path, file);
        }
    }
}
