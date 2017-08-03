using System;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.SqlModel.WebPath
{
    /// <summary>
    /// 学生 URL
    /// </summary>
    [AutoCSer.WebView.Path(Type = typeof(SqlModel.Student))]
    public struct Student
    {
        /// <summary>
        /// 学生标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 查询字符串
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        public string Query
        {
            get { return "StudentId=" + Id.toString(); }
        }

        /// <summary>
        /// 班级首页
        /// </summary>
        public AutoCSer.WebView.HashUrl Index
        {
            get
            {
                return new AutoCSer.WebView.HashUrl { Path = "/Student.html", Query = Query };
            }
        }
    }
}
