using System;

namespace AutoCSer.TestCase.SqlTableWeb
{
    /// <summary>
    /// 班级列表
    /// </summary>
    [AutoCSer.WebView.View]
    partial class ClassList : View<ClassList>
    {
        /// <summary>
        /// 是否班级列表页面
        /// </summary>
        bool IsClassList = true;
        /// <summary>
        /// 班级信息集合
        /// </summary>
        SqlTableCacheServer.Class[] Classes
        {
            get
            {
                return AutoCSer.TestCase.SqlTableCacheServer.ClientCache.Class.Get(AutoCSer.TestCase.SqlTableCacheServer.TcpCall.Class.getIds());
            }
        }
    }
}
