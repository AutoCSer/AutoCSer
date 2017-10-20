using System;

namespace AutoCSer.TestCase.SqlTableCacheServer.RemoteLinkType
{
    /// <summary>
    /// 远程调用连类型映射测试
    /// </summary>
    [AutoCSer.Net.TcpStaticServer.Server(Name = Config.DataReaderServer, IsRemoteLinkType = true)]
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
        [AutoCSer.Net.TcpStaticServer.RemoteMember(IsAwait = false)]
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
