using System;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.SqlModel.WebPath
{
    /// <summary>
    /// 班级 URL
    /// </summary>
    [AutoCSer.WebView.Path(Type = typeof(SqlModel.Class))]
    public struct Class
    {
        /// <summary>
        /// 班级标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 查询字符串
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        public string Query
        {
            get { return "ClassId=" + Id.toString(); }
        }

        /// <summary>
        /// 班级首页
        /// </summary>
        public AutoCSer.WebView.HashUrl Index
        {
            get
            {
                return new AutoCSer.WebView.HashUrl { Path = "/Class.html", Query = Query };
            }
        }
    }
}
