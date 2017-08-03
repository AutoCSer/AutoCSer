using System;

namespace AutoCSer.TestCase.SqlTableWeb
{
    /// <summary>
    /// 学生页面
    /// </summary>
    [AutoCSer.WebView.View]
    partial class Student : View<Student>
    {
        /// <summary>
        /// 学生信息
        /// </summary>
        [AutoCSer.WebView.ClearMember]
        SqlTableCacheServer.Student StudentInfo;
        /// <summary>
        /// 学生页面
        /// </summary>
        /// <param name="StudentId">学生标识</param>
        /// <returns>是否成功</returns>
        private bool loadView(int StudentId)
        {
            StudentInfo = SqlTableCacheServer.ClientCache.Student[StudentId];
            return StudentInfo != null;
        }
    }
}
