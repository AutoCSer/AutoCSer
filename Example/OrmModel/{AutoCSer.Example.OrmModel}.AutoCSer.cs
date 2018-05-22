//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.OrmModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        public partial class AsciiMember
        {
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            public abstract class SqlModel<tableType> : AutoCSer.Example.OrmModel.AsciiMember
                where tableType : SqlModel<tableType>
            {
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.AsciiMember> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.AsciiMember>.Get(false);
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




            }
        }
}namespace AutoCSer.Example.OrmModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        public partial class CastMember
        {
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            public abstract class SqlModel<tableType> : AutoCSer.Example.OrmModel.CastMember
                where tableType : SqlModel<tableType>
            {
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.CastMember> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.CastMember>.Get(false);
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




            }
        }
}namespace AutoCSer.Example.OrmModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        public partial class CombinationMember
        {
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            public abstract class SqlModel<tableType> : AutoCSer.Example.OrmModel.CombinationMember
                where tableType : SqlModel<tableType>
            {
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.CombinationMember> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.CombinationMember>.Get(false);
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




            }
        }
}namespace AutoCSer.Example.OrmModel
{
        public partial class ComparablePrimaryKey
        {
            /// <summary>
            /// 关键字
            /// </summary>
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            public struct DataPrimaryKey : IEquatable<DataPrimaryKey>, IComparable<DataPrimaryKey>
            {
                /// <summary>
                /// 关键字 1
                /// </summary>
                public int Key1;
                /// <summary>
                /// 关键字 2
                /// </summary>
                public int Key2;
                /// <summary>
                /// 关键字比较
                /// </summary>
                /// <param name="other">关键字</param>
                /// <returns>是否相等</returns>
                public bool Equals(DataPrimaryKey other)
                {
                    return Key1/**/.Equals(other.Key1) && Key2/**/.Equals(other.Key2);
                }
                /// <summary>
                /// 哈希编码
                /// </summary>
                /// <returns></returns>
                public override int GetHashCode()
                {
                    return Key1.GetHashCode() ^ Key2/**/.GetHashCode();
                }
                /// <summary>
                /// 关键字比较
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public override bool Equals(object obj)
                {
                    return Equals((DataPrimaryKey)obj);
                }
                /// <summary>
                /// 关键字比较
                /// </summary>
                /// <param name="other"></param>
                /// <returns></returns>
                public int CompareTo(DataPrimaryKey other)
                {
                    int _value_ = Key1/**/.CompareTo(other.Key1);
                    if (_value_ == 0)
                    {
                        _value_ = Key2/**/.CompareTo(other.Key2);
                    }
                    return _value_;
                }
            }
        }
}namespace AutoCSer.Example.OrmModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        public partial class ComparablePrimaryKey
        {
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            public abstract class SqlModel<tableType> : AutoCSer.Example.OrmModel.ComparablePrimaryKey
                where tableType : SqlModel<tableType>
            {
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.ComparablePrimaryKey, DataPrimaryKey> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.ComparablePrimaryKey, DataPrimaryKey>.Get(false);
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




            }
        }
}namespace AutoCSer.Example.OrmModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        public partial class EnumMember
        {
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            public abstract class SqlModel<tableType> : AutoCSer.Example.OrmModel.EnumMember
                where tableType : SqlModel<tableType>
            {
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.EnumMember> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.EnumMember>.Get(false);
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




            }
        }
}namespace AutoCSer.Example.OrmModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        public partial class IdentityMember
        {
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            public abstract class SqlModel<tableType> : AutoCSer.Example.OrmModel.IdentityMember
                where tableType : SqlModel<tableType>
            {
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.IdentityMember> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.IdentityMember>.Get(false);
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




            }
        }
}namespace AutoCSer.Example.OrmModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        public partial class IgnoreMember
        {
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            public abstract class SqlModel<tableType> : AutoCSer.Example.OrmModel.IgnoreMember
                where tableType : SqlModel<tableType>
            {
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.IgnoreMember> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.IgnoreMember>.Get(false);
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




            }
        }
}namespace AutoCSer.Example.OrmModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        public partial class InstanceField
        {
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            public abstract class SqlModel<tableType> : AutoCSer.Example.OrmModel.InstanceField
                where tableType : SqlModel<tableType>
            {
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.InstanceField> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.InstanceField>.Get(false);
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




            }
        }
}namespace AutoCSer.Example.OrmModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        public partial class JsonMember
        {
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            public abstract class SqlModel<tableType> : AutoCSer.Example.OrmModel.JsonMember
                where tableType : SqlModel<tableType>
            {
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.JsonMember> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.JsonMember>.Get(false);
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




            }
        }
}namespace AutoCSer.Example.OrmModel
{
        public partial class ManyPrimaryKey
        {
            /// <summary>
            /// 关键字
            /// </summary>
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            public struct DataPrimaryKey : IEquatable<DataPrimaryKey>
            {
                /// <summary>
                /// 关键字 1
                /// </summary>
                public int Key1;
                /// <summary>
                /// 关键字 2
                /// </summary>
                public int Key2;
                /// <summary>
                /// 关键字比较
                /// </summary>
                /// <param name="other">关键字</param>
                /// <returns>是否相等</returns>
                public bool Equals(DataPrimaryKey other)
                {
                    return Key1/**/.Equals(other.Key1) && Key2/**/.Equals(other.Key2);
                }
                /// <summary>
                /// 哈希编码
                /// </summary>
                /// <returns></returns>
                public override int GetHashCode()
                {
                    return Key1.GetHashCode() ^ Key2/**/.GetHashCode();
                }
                /// <summary>
                /// 关键字比较
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public override bool Equals(object obj)
                {
                    return Equals((DataPrimaryKey)obj);
                }
            }
        }
}namespace AutoCSer.Example.OrmModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        public partial class ManyPrimaryKey
        {
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            public abstract class SqlModel<tableType> : AutoCSer.Example.OrmModel.ManyPrimaryKey
                where tableType : SqlModel<tableType>
            {
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.ManyPrimaryKey, DataPrimaryKey> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.ManyPrimaryKey, DataPrimaryKey>.Get(false);
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




            }
        }
}namespace AutoCSer.Example.OrmModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        public partial class MemberCache
        {
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            /// <typeparam name="memberCacheType">成员绑定缓存类型</typeparam>
            [AutoCSer.Sql.MemberCacheLink]
            public abstract class SqlModel<tableType, memberCacheType> : AutoCSer.Example.OrmModel.MemberCache
                where tableType : SqlModel<tableType, memberCacheType>
                where memberCacheType : class
            {
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.MemberCache> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.MemberCache>.Get(false);
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
                protected static void sqlLoaded(Action<tableType> onInserted = null, AutoCSer.Sql.Cache.Table<tableType, AutoCSer.Example.OrmModel.MemberCache>.OnCacheUpdated onUpdated = null, Action<tableType> onDeleted = null)
                {
                    sqlCache/**/.Loaded(onInserted, onUpdated, onDeleted, false);
                    sqlTable.LoadMemberCache(typeof(memberCacheType));
                    sqlTable.WaitMemberCache();
                }
                /// <summary>
                /// SQL默认缓存
                /// </summary>
                protected static readonly AutoCSer.Sql.Cache.Whole.Event.IdentityArray<tableType, AutoCSer.Example.OrmModel.MemberCache, memberCacheType> sqlCache = sqlTable == null ? null : new AutoCSer.Sql.Cache.Whole.Event.IdentityArray<tableType, AutoCSer.Example.OrmModel.MemberCache, memberCacheType>(sqlTable);




                /// <summary>
                /// 获取数据
                /// </summary>
                /// <param name="Id">默认自增标识</param>
                /// <returns></returns>
                [AutoCSer.Net.TcpStaticServer.RemoteKey]
                protected static tableType getSqlCache(int Id)
                {
                    return sqlCache[Id];
                }
            }
        }
}namespace AutoCSer.Example.OrmModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        public partial class MemberIndex
        {
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            public abstract class SqlModel<tableType> : AutoCSer.Example.OrmModel.MemberIndex
                where tableType : SqlModel<tableType>
            {
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.MemberIndex> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.MemberIndex>.Get(false);
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




                /// <summary>
                /// 成员索引定义
                /// </summary>
                protected static class MemberIndexs
                {
                    /// <summary>
                    /// 生成成员索引 (成员索引)
                    /// </summary>
                    public static readonly AutoCSer.Metadata.MemberMap<AutoCSer.Example.OrmModel.MemberIndex>.MemberIndex UpdateIndex = AutoCSer.Metadata.MemberMap<AutoCSer.Example.OrmModel.MemberIndex>.MemberIndex.Create(value => value.UpdateIndex);
                }
            }
        }
}namespace AutoCSer.Example.OrmModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        public partial class NowTime
        {
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            public abstract class SqlModel<tableType> : AutoCSer.Example.OrmModel.NowTime
                where tableType : SqlModel<tableType>
            {
                private static readonly AutoCSer.Sql.NowTime[] nowTimeArray = new AutoCSer.Sql.NowTime[1];
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.NowTime> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.NowTime>.Get(false, nowTimeArray);
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




                /// <summary>
                /// 当前时间定义
                /// </summary>
                [AutoCSer.IOS.Preserve(AllMembers = true)]
                protected static class NowTimes
                {
                    /// <summary>
                    /// 生成当前时间精度 当前时间
                    /// </summary>
                    public static readonly AutoCSer.Sql.NowTime AppendTime = sqlTable == null ? null : new AutoCSer.Sql.NowTime(sqlTable.NowTimeMilliseconds);
                    /// <summary>
                    /// 初始化当前时间
                    /// </summary>
                    /// <param name="values">缓存数据</param>
                    public static void Load(System.Collections.Generic.IEnumerable<tableType> values)
                    {
                        foreach (tableType value in values)
                        {
                            AppendTime/**/.SetMaxTime(value.AppendTime);
                        }
                        AppendTime/**/.SetMaxTime();
                    }
                }
                static SqlModel()
                {
                    nowTimeArray[0] = NowTimes.AppendTime;
                }
            }
        }
}namespace AutoCSer.Example.OrmModel
{
        [AutoCSer.Json.Serialize]
        [AutoCSer.Json.Parse]
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        public partial class NullMember
        {
            /// <summary>
            /// 数据库表格模型
            /// </summary>
            /// <typeparam name="tableType">表格映射类型</typeparam>
            public abstract class SqlModel<tableType> : AutoCSer.Example.OrmModel.NullMember
                where tableType : SqlModel<tableType>
            {
                /// <summary>
                /// SQL表格操作工具
                /// </summary>
                protected static readonly AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.NullMember> sqlTable = AutoCSer.Sql.Table<tableType, AutoCSer.Example.OrmModel.NullMember>.Get(false);
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




            }
        }
}
#endif