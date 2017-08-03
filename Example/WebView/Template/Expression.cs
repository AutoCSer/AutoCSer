using System;

namespace AutoCSer.Example.WebView.Template
{
    /// <summary>
    /// 数据值表达式 测试
    /// </summary>
    [AutoCSer.WebView.View(IsPage = false)]
    partial class Expression : AutoCSer.WebView.View<Expression>
    {
        /// <summary>
        /// 服务端数据
        /// </summary>
        struct Data
        {
            /// <summary>
            /// 服务端数据
            /// </summary>
            public string Value;
        }
        /// <summary>
        /// 服务端数据
        /// </summary>
        Data ServerData
        {
            get { return new Data { Value = "服务端数据" }; }
        }
        /// <summary>
        /// 回溯根节点数据
        /// </summary>
        public string Value
        {
            get { return "回溯根节点数据"; }
        }
        protected override bool loadView()
        {
            return base.loadView();
        }
    }
}
