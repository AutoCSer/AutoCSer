using System;

namespace AutoCSer.TestCase.SqlTableWeb
{
    /// <summary>
    /// 班级页面
    /// </summary>
    [AutoCSer.WebView.View]
    partial class Class : View<Class>
    {
        /// <summary>
        /// 班级信息
        /// </summary>
        [AutoCSer.WebView.ClearMember]
        SqlTableCacheServer.Class ClassInfo;
        /// <summary>
        /// 班级页面
        /// </summary>
        /// <param name="ClassId">班级标识</param>
        /// <returns>是否成功</returns>
        private bool loadView(int ClassId)
        {
            ClassInfo = AutoCSer.TestCase.SqlTableCacheServer.ClientCache.Class[ClassId];
            return ClassInfo != null;
        }
    }
}
