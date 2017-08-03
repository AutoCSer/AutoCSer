using System;

namespace AutoCSer.Example.WebView.Symbol
{
    /// <summary>
    /// 重新加载视图标识 测试
    /// </summary>
    [AutoCSer.WebView.View(IsPage = false)]
    partial class ReView : AutoCSer.WebView.View<ReView>
    {
        /// <summary>
        /// 是否重新加载视图
        /// </summary>
        bool IsReView
        {
            get { return (HeaderFlag & AutoCSer.Net.Http.HeaderFlag.IsReView) != 0; }
        }
        /// <summary>
        /// 是否重新加载视图
        /// </summary>
        bool IsMobileReView
        {
            get { return (HeaderFlag & AutoCSer.Net.Http.HeaderFlag.IsMobileReView) != 0; }
        }
        /// <summary>
        /// 是否第一次加载页面缓存
        /// </summary>
        bool IsLoadPageCache
        {
            get { return (HeaderFlag & AutoCSer.Net.Http.HeaderFlag.IsLoadPageCache) != 0; }
        }
        /// <summary>
        /// ViewOnly 默认为 false 正常输出数据
        /// </summary>
        ViewOnlyData OutputData
        {
            get { return new ViewOnlyData { Value1 = 1, Value2 = 2 }; }
        }
        /// <summary>
        /// ViewOnly 设置为 true 表示 AJAX 重新加载页面时不覆盖客户端数据，对于 AJAX 重新加载的页面 Value1 与 Value2 的赋值没有意义
        /// </summary>
        ViewOnlyData ViewOnlyData
        {
            get { return new ViewOnlyData { ViewOnly = true, Value1 = 1, Value2 = 2 }; }
        }
        /// <summary>
        /// 根据 IsReView 状态输出输出数据，一般用于性能开销相对较大的数据，在 AJAX 重新加载页面时禁用引用类型数据的输出。比如列表页翻页时，应该只是重新加载列表本身，其它页面信息可以选择性重新加载。
        /// </summary>
        ViewOnlyData ReViewData
        {
            get
            {
                if (IsReView) return new ViewOnlyData { ViewOnly = true };
                return new ViewOnlyData { Value1 = 1, Value2 = 2 };
            }
        }
    }
}
