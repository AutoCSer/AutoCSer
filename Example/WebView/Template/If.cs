using System;

namespace AutoCSer.Example.WebView.Template
{
    /// <summary>
    /// 逻辑值表达式条件判定 测试
    /// </summary>
    [AutoCSer.WebView.View(IsPage = false)]
    partial class If : AutoCSer.WebView.View<If>
    {
        /// <summary>
        /// 逻辑真值
        /// </summary>
        public bool True
        {
            get { return true; }
        }
        /// <summary>
        /// 逻辑假值
        /// </summary>
        public string False
        {
            get { return string.Empty; }
        }
        /// <summary>
        /// 字符串匹配判定数据
        /// </summary>
        public string String
        {
            get { return "ABC"; }
        }
    }
}
