using System;

namespace AutoCSer.Example.WebView
{
    /// <summary>
    /// 无参初始化 示例
    /// </summary>
    [AutoCSer.WebView.View(IsPage = false)]
    partial class LoadView : AutoCSer.WebView.View<LoadView>
    {
        /// <summary>
        /// 输出数据，页面回收时清除数据
        /// </summary>
        [AutoCSer.WebView.ClearMember]
        public bool IsView;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>是否成功</returns>
        protected override bool loadView()
        {
            if (base.loadView())
            {
                if (true)
                {
                    IsView = true;
                    return true;
                }
            }
            return false;
        }
    }
}
