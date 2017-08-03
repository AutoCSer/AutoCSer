using System;

namespace AutoCSer.TestCase.SqlTableCacheServer
{
    /// <summary>
    /// 数据服务推送客户端缓存
    /// </summary>
    public static class ClientCache
    {
        /// <summary>
        /// 数据服务推送客户端缓存初始化访问锁
        /// </summary>
        public static readonly object CacheLock = new object();
        /// <summary>
        /// 班级 客户端缓存
        /// </summary>
        public static AutoCSer.Sql.LogStream.IdentityClient<Class, SqlModel.Class> Class = AutoCSer.Sql.LogStream.IdentityClient<Class, SqlModel.Class>.Null;
        /// <summary>
        /// 学生 客户端缓存
        /// </summary>
        public static AutoCSer.Sql.LogStream.IdentityClient<Student, SqlModel.Student> Student = AutoCSer.Sql.LogStream.IdentityClient<Student, SqlModel.Student>.Null;
    }
}
