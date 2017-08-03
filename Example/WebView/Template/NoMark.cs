using System;

namespace AutoCSer.Example.WebView.Template
{
    /// <summary>
    /// 禁止输出范围标识 测试
    /// </summary>
    partial class NoMark : AutoCSer.WebView.View<NoMark>
    {
        /// <summary>
        /// 客户端模板引擎正常情况下输出范围标识
        /// </summary>
        public string Mark
        {
            get { return "客户端模板引擎正常情况下输出范围标识"; }
        }
        /// <summary>
        /// 客户端模板引擎禁止输出范围标识
        /// </summary>
        public string NoMarkValue
        {
            get { return "客户端模板引擎禁止输出范围标识"; }
        }
    }
}
