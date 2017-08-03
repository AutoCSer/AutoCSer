using System;

namespace AutoCSer.TestCase.SqlTableWeb
{
    /// <summary>
    /// 公共全局视图
    /// </summary>
    /// <typeparam name="viewType">视图类型</typeparam>
    abstract class View<viewType> : AutoCSer.WebView.View<viewType>
         where viewType : View<viewType>
    {
        /// <summary>
        /// 公共路径
        /// </summary>
        internal SqlModel.WebPath.Pub PubPath = new SqlModel.WebPath.Pub();
        /// <summary>
        /// 全局false
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal bool FalseFlag;
    }
}
