using System;

namespace AutoCSer.Example.WebView.Symbol
{
    /// <summary>
    /// 禁用输出测试数据
    /// </summary>
    struct ViewOnlyData
    {
        /// <summary>
        /// 是否重新加载视图
        /// </summary>
        [AutoCSer.WebView.OutputAjaxAttribute]
        public bool ViewOnly;
        /// <summary>
        /// 测试数据
        /// </summary>
        public int Value1;
        /// <summary>
        /// 测试数据
        /// </summary>
        public int Value2;
    }
}
