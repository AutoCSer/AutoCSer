using System;

namespace AutoCSer.TestCase.SqlTableCacheServer.RemotePrimaryKey
{
    /// <summary>
    /// 关键字远程成员映射测试
    /// </summary>
    [AutoCSer.Sql.RemotePrimaryKey]
    [AutoCSer.Net.TcpStaticServer.Server(Name = Config.DataReaderServer)]
    public partial struct ClassStudent
    {
        /// <summary>
        /// 班级标识
        /// </summary>
        public int ClassId;
        /// <summary>
        /// 学生标识
        /// </summary>
        public int StudentId;

        /// <summary>
        /// 获取学生信息
        /// </summary>
        [AutoCSer.Sql.RemoteMember(IsAwait = false)]
        internal Student Student
        {
            get
            {
                foreach (Student student in Class.Loader.Cache[ClassId].Extension.Students)
                {
                    if (student.Id == StudentId) return student;
                }
                return null;
            }
        }
    }
}
