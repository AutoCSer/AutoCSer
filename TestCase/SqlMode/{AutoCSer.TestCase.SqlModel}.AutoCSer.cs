//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.TestCase.SqlModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false)]
        public partial class Class
        {
            /// <summary>
            /// 日志字段代理是否加载完毕
            /// </summary>
            [AutoCSer.Sql.Member(IsIgnoreCurrent = true)]
            protected bool _IsSqlLogProxyLoaded_;
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            /// <typeparam name="memberCacheType">成员绑定缓存类型</typeparam>
            [AutoCSer.Sql.MemberCacheLink]
            public abstract class SqlModel<tableType, memberCacheType> : AutoCSer.TestCase.SqlModel.Class, AutoCSer.Sql.LogStream.IMemberMapValueLink<tableType>
                where tableType : SqlModel<tableType, memberCacheType>
                where memberCacheType : class
            {
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.TestCase.SqlModel.Class> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.TestCase.SqlModel.Class>.Get(false);
                private static bool isSqlLoaded;
                /// <summary>
                /// 等待数据初始化完成
                /// </summary>
                public static void WaitSqlLoaded()
                {
                    if (!isSqlLoaded)
                    {
                        sqlTable.WaitLoad();
                        isSqlLoaded = true;
                    }
                }
                private static bool isEventCacheLoaded;
                /// <summary>
                /// 等待数据事件缓存数据初始化完成
                /// </summary>
                public static void WaitEventCacheLoaded()
                {
                    if (!isEventCacheLoaded)
                    {
                        if (sqlCache == null) throw new NullReferenceException(AutoCSer.Extension.TypeExtension.fullName(typeof(tableType)) + ".sqlCache is null");
                        isEventCacheLoaded = true;
                    }
                }
                /// <summary>
                /// 数据加载完成
                /// </summary>
                /// <param name="onInserted">添加记录事件</param>
                /// <param name="onUpdated">更新记录事件</param>
                /// <param name="onDeleted">删除记录事件</param>
                /// <param name="isMemberMap">是否支持成员位图</param>
                protected static void sqlLoaded(Action<tableType> onInserted = null, AutoCSer.Sql.Cache.Table<tableType, AutoCSer.TestCase.SqlModel.Class>.OnCacheUpdated onUpdated = null, Action<tableType> onDeleted = null, bool isMemberMap = true)
                {
                    sqlStream.Set(sqlCache, isMemberMap);
                    sqlCache/**/.Loaded(onInserted, onUpdated, onDeleted, false, false);
                    sqlTable.LoadMemberCache(typeof(memberCacheType));
                    sqlTable.AddLogStreamLoadedType(SqlLogMembers._LoadCount_, new AutoCSer.Sql.LogStream.LoadedType(typeof(AutoCSer.TestCase.SqlModel.Student), 0));
                    sqlTable.WaitMemberCache();
                }
                /// <summary>
                /// SQL默认缓存
                /// </summary>
                protected static readonly AutoCSer.Sql.Cache.Whole.Event.IdentityArray<tableType, AutoCSer.TestCase.SqlModel.Class, memberCacheType> sqlCache = sqlTable == null ? null : new AutoCSer.Sql.Cache.Whole.Event.IdentityArray<tableType, AutoCSer.TestCase.SqlModel.Class, memberCacheType>(sqlTable);




                [AutoCSer.Metadata.Ignore]
                tableType AutoCSer.Sql.LogStream.IMemberMapValueLink<tableType>.MemberMapValueLink { get; set; }
                protected readonly static AutoCSer.Sql.LogStream.Log<tableType, AutoCSer.TestCase.SqlModel.Class> sqlStream = sqlTable == null ? null : new AutoCSer.Sql.LogStream.Log<tableType, AutoCSer.TestCase.SqlModel.Class>(sqlTable, 4);
                /// <summary>
                /// 日志处理
                /// </summary>
                /// <param name="onLog"></param>
                [AutoCSer.Net.TcpStaticServer.KeepCallbackMethod(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerName = "DataLog")]
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                protected static void onSqlLog(Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<tableType, AutoCSer.TestCase.SqlModel.Class>.Data>, bool> onLog)
                {
                    sqlStream.Add(onLog);
                }
                /// <summary>
                /// 计算字段日志流
                /// </summary>
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                public struct SqlLogMembers
                {
                    /// <summary>
                    /// 数据对象
                    /// </summary>
                    internal SqlModel<tableType, memberCacheType> _value_;
                    private static readonly AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SqlModel.Class> _m4 = sqlStream.CreateMemberMap(value => value.StudentCount);
                    /// <summary>
                    /// 当前学生数量 (更新日志流)
                    /// </summary>
                    /// <param name="value"></param>
                    [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                    public void StudentCount(int value)
                    {
                        if (!value.Equals(_value_.StudentCount))
                        {
                            _value_.StudentCount = value;
                            StudentCount();
                        }
                    }
                    /// <summary>
                    /// 当前学生数量 (更新日志流)
                    /// </summary>
                    [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                    public void StudentCount()
                    {
                        sqlStream.Update((tableType)_value_, _m4);
                    }
                    /// <summary>
                    /// 根据日志流计数完成类型初始化完毕
                    /// </summary>
                    /// <param name="type"></param>
                    internal static void _LoadCount_(AutoCSer.Sql.LogStream.LoadedType type)
                    {
                        if (type.Equals(typeof(AutoCSer.TestCase.SqlModel.Student), 0)) sqlStream.LoadMember(4);
                    }
                }
                /// <summary>
                /// 计算字段日志流
                /// </summary>
                [AutoCSer.Metadata.Ignore]
                public SqlLogMembers SqlLogMember
                {
                    get { return new SqlLogMembers { _value_ = this }; }
                }
                /// <summary>
                /// 计算字段访问代理
                /// </summary>
                public struct SqlLogProxyMembers
                {
                    /// <summary>
                    /// 数据对象
                    /// </summary>
                    internal SqlModel<tableType, memberCacheType> _value_;
                    /// <summary>
                    /// 当前学生数量
                    /// </summary>
                    public int StudentCount
                    {
                        get { return _value_.StudentCount; }
                        set { _value_.StudentCount = value; }
                    }
                }
                /// <summary>
                /// 计算字段日志流
                /// </summary>
                [AutoCSer.Metadata.Ignore]
                public SqlLogProxyMembers SqlLogProxyMember
                {
                    get { return new SqlLogProxyMembers { _value_ = this }; }
                }
                /// <summary>
                /// 获取数据
                /// </summary>
                /// <param name="Id">班级标识（默认自增）</param>
                /// <returns></returns>
                [AutoCSer.Net.TcpStaticServer.RemoteKey]
                [AutoCSer.Net.TcpStaticServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ServerName = "DataLog")]
                protected static tableType getSqlCache(int Id)
                {
                    return sqlCache[Id];
                }
                /// <summary>
                /// 班级 URL
                /// </summary>
                [AutoCSer.Metadata.Ignore]
                public AutoCSer.TestCase.SqlModel.WebPath.Class Path
                {
                    get { return new AutoCSer.TestCase.SqlModel.WebPath.Class { Id = Id }; }
                }
            }
        }
}namespace AutoCSer.TestCase.SqlModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false)]
        public partial class Student
        {
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            public abstract class SqlModel<tableType> : AutoCSer.TestCase.SqlModel.Student, AutoCSer.Sql.LogStream.IMemberMapValueLink<tableType>
                where tableType : SqlModel<tableType>
            {
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.TestCase.SqlModel.Student, System.String> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.TestCase.SqlModel.Student, System.String>.Get(false);
                private static bool isSqlLoaded;
                /// <summary>
                /// 等待数据初始化完成
                /// </summary>
                public static void WaitSqlLoaded()
                {
                    if (!isSqlLoaded)
                    {
                        sqlTable.WaitLoad();
                        isSqlLoaded = true;
                    }
                }
                private static bool isEventCacheLoaded;
                /// <summary>
                /// 等待数据事件缓存数据初始化完成
                /// </summary>
                public static void WaitEventCacheLoaded()
                {
                    if (!isEventCacheLoaded)
                    {
                        if (sqlCache == null) throw new NullReferenceException(AutoCSer.Extension.TypeExtension.fullName(typeof(tableType)) + ".sqlCache is null");
                        isEventCacheLoaded = true;
                    }
                }
                /// <summary>
                /// 数据加载完成
                /// </summary>
                /// <param name="onInserted">添加记录事件</param>
                /// <param name="onUpdated">更新记录事件</param>
                /// <param name="onDeleted">删除记录事件</param>
                /// <param name="isMemberMap">是否支持成员位图</param>
                protected static void sqlLoaded(Action<tableType> onInserted = null, AutoCSer.Sql.Cache.Table<tableType, AutoCSer.TestCase.SqlModel.Student>.OnCacheUpdated onUpdated = null, Action<tableType> onDeleted = null, bool isMemberMap = true)
                {
                    sqlStream.Set(sqlCache, isMemberMap);
                    sqlCache/**/.Loaded(onInserted, onUpdated, onDeleted, false);
                    sqlTable.LoadMemberCache();
                }
                /// <summary>
                /// SQL默认缓存
                /// </summary>
                protected static readonly AutoCSer.Sql.Cache.Whole.Event.IdentityArray<tableType, AutoCSer.TestCase.SqlModel.Student> sqlCache = sqlTable == null ? null : new AutoCSer.Sql.Cache.Whole.Event.IdentityArray<tableType, AutoCSer.TestCase.SqlModel.Student>(sqlTable);




                /// <summary>
                /// 成员索引定义
                /// </summary>
                protected static class MemberIndexs
                {
                    /// <summary>
                    /// 按加入时间排序的班级集合（不可识别的字段映射为 JSON 字符串） (成员索引)
                    /// </summary>
                    public static readonly AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SqlModel.Student>.MemberIndex Classes = AutoCSer.Metadata.MemberMap<AutoCSer.TestCase.SqlModel.Student>.MemberIndex.Create(value => value.Classes);
                }
                [AutoCSer.Metadata.Ignore]
                tableType AutoCSer.Sql.LogStream.IMemberMapValueLink<tableType>.MemberMapValueLink { get; set; }
                protected readonly static AutoCSer.Sql.LogStream.Log<tableType, AutoCSer.TestCase.SqlModel.Student> sqlStream = sqlTable == null ? null : new AutoCSer.Sql.LogStream.Log<tableType, AutoCSer.TestCase.SqlModel.Student>(sqlTable);
                /// <summary>
                /// 日志处理
                /// </summary>
                /// <param name="onLog"></param>
                [AutoCSer.Net.TcpStaticServer.KeepCallbackMethod(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerName = "DataLog")]
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                protected static void onSqlLog(Func<AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Sql.LogStream.Log<tableType, AutoCSer.TestCase.SqlModel.Student>.Data>, bool> onLog)
                {
                    sqlStream.Add(onLog);
                }
                /// <summary>
                /// 获取数据
                /// </summary>
                /// <param name="Id">学生标识（默认自增）</param>
                /// <returns></returns>
                [AutoCSer.Net.TcpStaticServer.RemoteKey]
                [AutoCSer.Net.TcpStaticServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ServerName = "DataLog")]
                protected static tableType getSqlCache(int Id)
                {
                    return sqlCache[Id];
                }
                /// <summary>
                /// 学生 URL
                /// </summary>
                [AutoCSer.Metadata.Ignore]
                public AutoCSer.TestCase.SqlModel.WebPath.Student Path
                {
                    get { return new AutoCSer.TestCase.SqlModel.WebPath.Student { Id = Id }; }
                }
            }
        }
}
#endif