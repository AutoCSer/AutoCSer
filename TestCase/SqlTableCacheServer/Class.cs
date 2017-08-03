using System;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.SqlTableCacheServer
{
    /// <summary>
    /// 班级表格定义
    /// </summary>
    [AutoCSer.Net.TcpStaticServer.Server(Name = Config.DataReaderServer)]
    [AutoCSer.Sql.Table(ConnectionName = Config.SqlConnectionName, IsLoadIdentity = false)]
    public partial class Class : AutoCSer.TestCase.SqlModel.Class.SqlModel<Class, Class.MemberCache>
    {
        /// <summary>
        /// 远程对象扩展
        /// </summary>
        public struct RemoteExtension
        {
            /// <summary>
            /// 班级信息
            /// </summary>
            internal Class Value;
            /// <summary>
            /// 学生集合
            /// </summary>
            public Student[] Students
            {
                get
                {
#if NoAutoCSer
                    throw new InvalidCastException();
#else
                    return ClientCache.Student.Get(TcpCall.Class.GetStudentIds(Value.Id));
#endif
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
        /// 扩展缓存数据
        /// </summary>
        public sealed class MemberCache : AutoCSer.Sql.Cache.Whole.MemberCache<Class>
        {
            /// <summary>
            /// 学生列表
            /// </summary>
            internal ListArray<Student> Students;
        }
        /// <summary>
        /// 扩展缓存数据
        /// </summary>
        internal MemberCache Extension;

        /// <summary>
        /// 获取班级信息
        /// </summary>
        /// <param name="id">班级标识</param>
        /// <returns>班级</returns>
        [AutoCSer.Net.TcpStaticServer.Method]
        internal static Class Get(int id)
        {
            return Loader.Cache[id];
        }
        /// <summary>
        /// 获取班级标识集合
        /// </summary>
        /// <returns>班级标识集合</returns>
        [AutoCSer.Net.TcpStaticServer.Method]
        private static int[] getIds()
        {
            return Loader.Cache.Values.getLeftArray(value => value.Id, Loader.Cache.Count).ToArray();
        }
        /// <summary>
        /// 获取学生标识集合
        /// </summary>
        /// <param name="id">班级标识</param>
        /// <returns>学生标识集合</returns>
        [AutoCSer.Net.TcpStaticServer.Method]
        internal static int[] GetStudentIds(int id)
        {
            return Student.Loader.GetStudentIds(id);
        }

        /// <summary>
        /// 缓存加载
        /// </summary>
        internal static class Loader
        {
            /// <summary>
            /// 数据缓存
            /// </summary>
            internal static readonly AutoCSer.Sql.Cache.Whole.Event.IdentityArray<Class, SqlModel.Class, MemberCache> Cache = createCache(value => value.Extension);
            /// <summary>
            /// 数据缓存初始化
            /// </summary>
            static Loader()
            {
                if (sqlTable != null) sqlLoaded();
            }
        }
        /// <summary>
        /// 初始化测试数据
        /// </summary>
        internal static void Initialize()
        {
            sqlTable.Insert(new Class { Name = "软件 1 班", Discipline = SqlModel.Member.Discipline.软件工程, DateRange = new SqlModel.Member.DateRange { Start = new AutoCSer.Sql.Member.IntDate(2012, 9, 1), End = new AutoCSer.Sql.Member.IntDate(2015, 7, 1) } }, onInitializeInserted);
            sqlTable.Insert(new Class { Name = "软件 2 班", Discipline = SqlModel.Member.Discipline.软件工程, DateRange = new SqlModel.Member.DateRange { Start = new AutoCSer.Sql.Member.IntDate(2013, 9, 1), End = new AutoCSer.Sql.Member.IntDate(2016, 7, 1) } }, onInitializeInserted);
        }
        /// <summary>
        /// 初始化测试数据
        /// </summary>
        /// <param name="value"></param>
        private static void onInitializeInserted(Class value)
        {
            SqlModel.Member.ClassDate[] classes = new SqlModel.Member.ClassDate[] { new SqlModel.Member.ClassDate { ClassId = value.Id, Date = value.DateRange.Start } };
            switch (value.Name)
            {
                case "软件 1 班":
                    Student.InitializeInsert(new Student { Name = "张三", Email = "zhangsan@AutoCSer.com", Password = "zhangsan", Birthday = new DateTime(1994, 3, 3), Gender = SqlModel.Member.Gender.男, Classes = classes });
                    Student.InitializeInsert(new Student { Name = "李四", Email = "lisi@AutoCSer.com", Password = "lisi", Birthday = new DateTime(1994, 4, 4), Gender = SqlModel.Member.Gender.女, Classes = classes });
                    return;
                case "软件 2 班":
                    Student.InitializeInsert(new Student { Name = "王五", Email = "wangwu@AutoCSer.com", Password = "wangwu", Birthday = new DateTime(1995, 5, 5), Gender = SqlModel.Member.Gender.男, Classes = classes });
                    Student.InitializeInsert(new Student { Name = "赵六", Email = "zhaoliu@AutoCSer.com", Password = "zhaoliu", Birthday = new DateTime(1995, 4, 4), Gender = SqlModel.Member.Gender.女, Classes = classes });
                    return;
            }
        }
    }
}
