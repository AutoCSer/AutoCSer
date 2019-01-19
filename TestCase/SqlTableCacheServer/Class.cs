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
        public partial struct RemoteExtension
        {
            /// <summary>
            /// 学生集合（远程实例成员调用）
            /// </summary>
            public Student[] Students
            {
                get
                {
                    return ClientCache.Student.Get(StudentIds);
                }
            }
            /// <summary>
            /// 学生集合（远程静态成员调用）
            /// </summary>
            public Student[] StaticStudents
            {
                get
                {
                    return ClientCache.Student.Get(GetStaticStudentIds());
                }
            }
            /// <summary>
            /// 学生集合（远程缓存成员调用）
            /// </summary>
            public Student[] ExtensionStudents
            {
                get
                {
                    return ClientCache.Student.Get(ExtensionStudentIds);
                }
            }
            /// <summary>
            /// 根据学生索引获取学生（远程实例成员调用）
            /// </summary>
            /// <param name="index">学生索引</param>
            /// <returns>学生</returns>
            public Student GetStudent(int index)
            {
                return ClientCache.Student[GetStudentId(index)];
            }
            /// <summary>
            /// 根据学生索引获取学生（远程静态成员调用）
            /// </summary>
            /// <param name="index">学生索引</param>
            /// <returns>学生</returns>
            public Student GetStaticStudent(int index)
            {
                return ClientCache.Student[GetStaticStudentId(index)];
            }
            /// <summary>
            /// 根据学生索引获取学生（远程缓存成员调用）
            /// </summary>
            /// <param name="index">学生索引</param>
            /// <returns>学生</returns>
            public Student GetExtensionStudent(int index)
            {
                return ClientCache.Student[ExtensionGetStudentId(index)];
            }
            /// <summary>
            /// 根据学生索引获取学生（远程缓存成员调用）
            /// </summary>
            /// <param name="index">学生索引</param>
            /// <returns>学生</returns>
            public Student GetExtensionItemStudent(int index)
            {
                return ClientCache.Student[ExtensionItem(index)];
            }
        }
        /// <summary>
        /// 扩展缓存数据
        /// </summary>
        public sealed class MemberCache : AutoCSer.Sql.Cache.Whole.MemberCache<Class>
        {
            /// <summary>
            /// 学生列表
            /// </summary>
            [AutoCSer.Sql.MemberCacheLink(CacheType = typeof(Student))]
            internal ListArray<Student> Students;
            /// <summary>
            /// 获取学生标识集合（远程缓存成员）
            /// </summary>
            [AutoCSer.Net.TcpStaticServer.RemoteMember(IsAwait = false)]
            internal int[] StudentIds
            {
                get
                {
                    return Students.ToLeftArray().GetArray(value => value.Id);
                }
            }
            /// <summary>
            /// 获取学生标识（远程缓存成员）
            /// </summary>
            /// <param name="index">学生索引</param>
            /// <returns>学生标识</returns>
            [AutoCSer.Net.TcpStaticServer.RemoteMethod(IsAwait = false)]
            internal int GetStudentId(int index)
            {
                return Students[index].Id;
            }
            /// <summary>
            /// 获取学生标识（远程缓存成员）
            /// </summary>
            /// <param name="index">学生索引</param>
            /// <returns>学生标识</returns>
            [AutoCSer.Net.TcpStaticServer.RemoteMethod(IsAwait = false)]
            internal int this[int index]
            {
                get { return Students[index].Id; }
            }
        }
        /// <summary>
        /// 扩展缓存数据
        /// </summary>
        [AutoCSer.Sql.MemberCache]
        private MemberCache extension;
        /// <summary>
        /// 扩展缓存数据
        /// </summary>
        [AutoCSer.Net.TcpStaticServer.RemoteLink(IsNull = false)]
        internal MemberCache Extension
        {
            get
            {
                return Loader.Cache[Id].extension;
            }
        }

        /// <summary>
        /// 获取学生标识集合（远程实例成员，推荐模式）
        /// </summary>
        [AutoCSer.Net.TcpStaticServer.RemoteMember(IsAwait = false)]
        internal int[] StudentIds
        {
            get
            {
                return Extension.Students.ToLeftArray().GetArray(value => value.Id);
            }
        }
        /// <summary>
        /// 获取学生标识集合（远程静态成员示例，需要自行保证第一个参数的正确性）
        /// </summary>
        /// <param name="id">班级标识</param>
        /// <returns>学生标识集合</returns>
        [AutoCSer.Net.TcpStaticServer.RemoteMethod(IsAwait = false)]
        [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod]
        internal static int[] GetStaticStudentIds(int id)
        {
            return Loader.Cache[id].extension.Students.ToLeftArray().GetArray(value => value.Id);
        }

        /// <summary>
        /// 获取学生标识（远程实例成员，推荐模式）
        /// </summary>
        /// <param name="index">学生索引</param>
        /// <returns>学生标识集合</returns>
        [AutoCSer.Net.TcpStaticServer.RemoteMethod(IsAwait = false)]
        internal int GetStudentId(int index)
        {
            return Extension.Students[index].Id;
        }
        /// <summary>
        /// 获取学生标识集合（远程静态成员示例，需要自行保证第一个参数的正确性）
        /// </summary>
        /// <param name="id">班级标识</param>
        /// <param name="index">学生索引</param>
        /// <returns>学生标识集合</returns>
        [AutoCSer.Net.TcpStaticServer.RemoteMethod(IsAwait = false)]
        [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod]
        internal static int GetStaticStudentId(int id, int index)
        {
            return Loader.Cache[id].extension.Students[index].Id;
        }


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
        /// 等待设置的缓存，用于其它缓存初始化访问
        /// </summary>
        internal static AutoCSer.Sql.WaitSetValue<AutoCSer.Sql.Cache.Whole.Event.IdentityArray<Class, SqlModel.Class, MemberCache>> WaitCache = new AutoCSer.Sql.WaitSetValue<AutoCSer.Sql.Cache.Whole.Event.IdentityArray<Class, TestCase.SqlModel.Class, MemberCache>>(null);
        /// <summary>
        /// 缓存加载
        /// </summary>
        internal sealed class Loader
        {
            /// <summary>
            /// 数据缓存
            /// </summary>
            internal static readonly AutoCSer.Sql.Cache.Whole.Event.IdentityArray<Class, SqlModel.Class, MemberCache> Cache;
            /// <summary>
            /// 数据缓存初始化
            /// </summary>
            static Loader()
            {
                if (sqlTable != null)
                {
                    Cache = WaitCache.Set(sqlCache);
                    sqlLoaded();
                }
            }
        }
        /// <summary>
        /// 初始化测试数据
        /// </summary>
        internal static void Initialize()
        {
            sqlTable.InsertQueue(new Class { Name = "软件 1 班", Discipline = SqlModel.Member.Discipline.软件工程, DateRange = new SqlModel.Member.DateRange { Start = new AutoCSer.Sql.Member.IntDate(2012, 9, 1), End = new AutoCSer.Sql.Member.IntDate(2015, 7, 1) } }, onInitializeInserted);
            sqlTable.InsertQueue(new Class { Name = "软件 2 班", Discipline = SqlModel.Member.Discipline.软件工程, DateRange = new SqlModel.Member.DateRange { Start = new AutoCSer.Sql.Member.IntDate(2013, 9, 1), End = new AutoCSer.Sql.Member.IntDate(2016, 7, 1) } }, onInitializeInserted);
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
