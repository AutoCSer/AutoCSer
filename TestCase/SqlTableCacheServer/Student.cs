using System;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.SqlTableCacheServer
{
    /// <summary>
    /// 学生表格定义
    /// </summary>
    [AutoCSer.Net.TcpStaticServer.Server(Name = Config.DataReaderServer)]
    [AutoCSer.Sql.Table(ConnectionName = Config.SqlConnectionName, IsLoadIdentity = false)]
    public partial class Student : AutoCSer.TestCase.SqlModel.Student.SqlModel<Student>
    {
        /// <summary>
        /// 远程对象扩展
        /// </summary>
        public struct RemoteExtension
        {
            /// <summary>
            /// 学生信息
            /// </summary>
            internal Student Value;
            /// <summary>
            /// 获取当前班级
            /// </summary>
            public Class Class
            {
                get { return ClientCache.Class[Value.ClassId]; }
            }
            /// <summary>
            /// 加入班级时间信息
            /// </summary>
            public struct JoinClassDate
            {
                /// <summary>
                /// 加入班级时间信息
                /// </summary>
                public SqlModel.Member.ClassDate ClassDate;
                /// <summary>
                /// 班级信息
                /// </summary>
                public Class Class
                {
                    get { return ClientCache.Class[ClassDate.ClassId]; }
                }
            }
            /// <summary>
            /// 按加入时间排序的班级集合
            /// </summary>
            public JoinClassDate[] Classes
            {
                get
                {
                    return Value.Classes.getArray(value => new JoinClassDate { ClassDate = value });
                }
            }
        }
        /// <summary>
        /// 远程对象扩展
        /// </summary>
        public RemoteExtension Remote
        {
            get { return new RemoteExtension { Value = this }; }
        }
        /// <summary>
        /// 获取学生信息
        /// </summary>
        /// <param name="id">学生标识</param>
        /// <returns>学生</returns>
        [AutoCSer.Net.TcpStaticServer.Method]
        internal static Student Get(int id)
        {
            return sqlCache[id];
        }

        /// <summary>
        /// 添加学生处理
        /// </summary>
        /// <param name="value"></param>
        private static void onInserted(Student value)
        {
            Class Class = Class.Loader.Cache[value.ClassId];
            if (Class != null) Class.SqlLogMember.StudentCount(Class.SqlLogProxyMember.StudentCount + 1);
        }
        /// <summary>
        /// 修改学生处理
        /// </summary>
        /// <param name="cacheValue">当前缓存对象</param>
        /// <param name="value">修改之后的对象数据</param>
        /// <param name="oldValue">修改之前的对象数据</param>
        /// <param name="memberMap">被修改成员位图</param>
        private static void onUpdated(Student cacheValue, Student value, Student oldValue, AutoCSer.Metadata.MemberMap<SqlModel.Student> memberMap)
        {
            if (MemberIndexs.Classes.IsMember(memberMap))
            {
                Class Class = Class.Loader.Cache[oldValue.ClassId];
                if (Class != null) Class.SqlLogMember.StudentCount(Class.SqlLogProxyMember.StudentCount - 1);

                onInserted(cacheValue);
            }
        }

        /// <summary>
        /// 缓存加载
        /// </summary>
        internal sealed class Loader
        {
            /// <summary>
            /// 学生数量
            /// </summary>
            internal static int Count
            {
                get { return sqlCache.Count; }
            }
            /// <summary>
            /// 数据缓存初始化
            /// </summary>
            static Loader()
            {
                if (sqlTable != null)
                {
                    var classCache = Class.WaitCache.Wait();
                    sqlCache.CreateMemberList(classCache, value => value.ClassId, value => value.Students, true);

                    if (sqlCache.Count == 0)
                    {
                        sqlLoaded(onInserted, onUpdated);

                        Class.Initialize();
                    }
                    else
                    {
                        foreach (Student value in sqlCache.Values)
                        {
                            Class.SqlLogProxyMembers classProxy = classCache[value.ClassId].SqlLogProxyMember;
                            ++classProxy.StudentCount;
                        }

                        sqlLoaded(onInserted, onUpdated);
                    }
                }
            }
        }
        /// <summary>
        /// 初始化测试数据
        /// </summary>
        /// <param name="student"></param>
        internal static void InitializeInsert(Student student)
        {
            sqlTable.InsertQueue(student, null);
        }
    }
}
