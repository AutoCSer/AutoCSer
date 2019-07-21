using System;
using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Reflection;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 数据模型 代码生成
    /// </summary>
    internal abstract partial class SqlModel
    {
        /// <summary>
        /// 数据模型 代码生成
        /// </summary>
        [Generator(Name = "数据模型", DependType = typeof(CSharper), IsAuto = true, IsDotNet2 = false, IsMono = false)]
        internal partial class Generator : Generator<AutoCSer.Sql.ModelAttribute>
        {
            /// <summary>
            /// SQL表格计算列日志字段代理名称
            /// </summary>
            public const string DefaultLogProxyMemberName = "SqlLogProxyMember";
            /// <summary>
            /// SQL表格默认缓存 字段名称
            /// </summary>
            public const string SqlCacheName = "sqlCache";
            /// <summary>
            /// 关键字类型名称
            /// </summary>
            public const string PrimaryKeyTypeName = "DataPrimaryKey";
            /// <summary>
            /// 数据关键字 代码生成
            /// </summary>
            private static readonly DataPrimaryKey.Generator dataPrimaryKey = new DataPrimaryKey.Generator();
            /// <summary>
            /// 日志同步成员信息
            /// </summary>
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct LogMember
            {
                /// <summary>
                /// 成员信息
                /// </summary>
                public MemberIndex Member;
                /// <summary>
                /// 日志同步成员信息
                /// </summary>
                public AutoCSer.Sql.LogAttribute Attribute;
                /// <summary>
                /// 数据库成员信息
                /// </summary>
                public AutoCSer.Sql.MemberAttribute MemberAttribute;
                /// <summary>
                /// 成员位图名称
                /// </summary>
                public string MemberMapName
                {
                    get { return "_m" + Member.MemberIndex.toString(); }
                }
                /// <summary>
                /// 成员加载函数名称
                /// </summary>
                public string MemberLoadedMethodName
                {
                    get { return Member.MemberName + "Loaded"; }
                }
                /// <summary>
                /// 获取数据函数名称
                /// </summary>
                public string GetMethodName
                {
                    get { return "Get" + Member.MemberName; }
                }
                /// <summary>
                /// 是否生成日志流计数
                /// </summary>
                public bool IsSqlStreamCount
                {
                    get { return MemberAttribute == null; }
                }
                /// <summary>
                /// 日志流计数完成类型
                /// </summary>
                public ExtensionType SqlStreamCountType
                {
                    get { return Attribute.CountType == null ? null : (ExtensionType)Attribute.CountType; }
                }
                /// <summary>
                /// 日志流计数完成类型表格编号
                /// </summary>
                public int CountTypeNumber
                {
                    get { return Attribute.CountTypeNumber; }
                }
                /// <summary>
                /// 是否需要生成日志字段代理属性
                /// </summary>
                public bool IsProxy
                {
                    get { return (Member.MemberFilters & AutoCSer.Metadata.MemberFilters.PublicInstanceField) != AutoCSer.Metadata.MemberFilters.PublicInstanceField; }
                }
            }
            /// <summary>
            /// 计数成员信息
            /// </summary>
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct CountMember
            {
                /// <summary>
                /// 成员信息
                /// </summary>
                public MemberIndex Member;
                /// <summary>
                /// 计数成员信息
                /// </summary>
                public AutoCSer.Sql.CountAttribute Attribute;
                /// <summary>
                /// 计数总计 TCP 调用属性名称
                /// </summary>
                public string TotalMemberName
                {
                    get { return Member.MemberName + "Total"; }
                }
                /// <summary>
                /// 计数 TCP 调用函数名称
                /// </summary>
                public string GetMethodName
                {
                    get { return "get" + Member.MemberName; }
                }
                /// <summary>
                /// 增加计数 TCP 调用函数名称
                /// </summary>
                public string IncMethodName
                {
                    get { return "inc" + Member.MemberName; }
                }
            }
            /// <summary>
            /// WEB Path 类型
            /// </summary>
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct WebPathType
            {
                /// <summary>
                /// WEB Path 类型
                /// </summary>
                public ExtensionType MemberType;
                /// <summary>
                /// WEB Path 配置
                /// </summary>
                public AutoCSer.WebView.PathAttribute Attribute;
                /// <summary>
                /// WEB Path 关联成员集合
                /// </summary>
                public MemberIndex[] Members;
                /// <summary>
                /// 服务端生成属性名称
                /// </summary>
                public string PathMemberName
                {
                    get { return Attribute.MemberName; }
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="members"></param>
                /// <returns></returns>
                public MemberIndex[] CheckMembers(ref LeftArray<MemberIndex> members)
                {
                    int count = 0;
                    foreach (MemberIndex member in members)
                    {
                        foreach (MemberIndex field in Members)
                        {
                            if (field.MemberSystemType == member.MemberSystemType && field.MemberName == member.MemberName)
                            {
                                ++count;
                                break;
                            }
                        }
                    }
                    if (count == Members.Length) return Members;
                    if (count == 0) return null;
                    MemberIndex[] fields = new MemberIndex[count];
                    foreach (MemberIndex member in members)
                    {
                        foreach (MemberIndex field in Members)
                        {
                            if (field.MemberSystemType == member.MemberSystemType && field.MemberName == member.MemberName)
                            {
                                int memberIndex = fields.Length - count;
                                fields[--count] = new MemberIndex((FieldInfo)field.Member, memberIndex);
                                break;
                            }
                        }
                    }
                    return fields;
                }
            }
            /// <summary>
            /// 日志流计数完成类型
            /// </summary>
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct LogCountType
            {
                /// <summary>
                /// 数据模型类型
                /// </summary>
                public ExtensionType SqlStreamCountType;
                /// <summary>
                /// 表格编号，主要使用枚举识别同一数据模型下的不同表格
                /// </summary>
                public int CountTypeNumber;
            }
            /// <summary>
            /// SQL表格计算列日志 字段名称
            /// </summary>
            public string LogMemberName
            {
                get { return "SqlLogMember"; }
            }
            /// <summary>
            /// SQL表格计算列日志字段代理名称
            /// </summary>
            public string LogProxyMemberName
            {
                get { return DefaultLogProxyMemberName; }
            }
            /// <summary>
            /// 计算列加载完成字段名称
            /// </summary>
            public string IsSqlLogProxyLoadedName
            {
                get { return AutoCSer.Sql.LogStream.Log.IsSqlLogProxyLoadedName; }
            }
            /// <summary>
            /// 日志流计数完成类型集合
            /// </summary>
            internal LogCountType[] SqlStreamCountTypes
            {
                get
                {
                    if (LogMembers != null)
                    {
                        HashSet<AutoCSer.Sql.LogStream.LoadedType> types = new HashSet<AutoCSer.Sql.LogStream.LoadedType>();
                        foreach (LogMember member in LogMembers)
                        {
                            if (member.Attribute.CountType != null) types.Add(new AutoCSer.Sql.LogStream.LoadedType(member.Attribute.CountType, member.Attribute.CountTypeNumber));
                        }
                        if (types.Count != 0) return types.getArray(value => new LogCountType { SqlStreamCountType = value.Type, CountTypeNumber = value.TableNumber });
                    }
                    return null;
                }
            }
            /// <summary>
            /// 日志同步字段集合
            /// </summary>
            internal LogMember[] LogMembers;
            /// <summary>
            /// 是否需要生成日志字段代理属性
            /// </summary>
            internal bool IsLogProxyMember;
            /// <summary>
            /// 计数字段集合
            /// </summary>
            internal CountMember[] CounterMembers;
            /// <summary>
            /// 更新字段集合
            /// </summary>
            internal MemberIndex[] UpdateMembers;
            /// <summary>
            /// 关键字成员集合
            /// </summary>
            internal MemberIndex[] PrimaryKeys;
            /// <summary>
            /// 关键字类型名称
            /// </summary>
            public string PrimaryKeyType
            {
                get { return PrimaryKeys.Length > 1 ? PrimaryKeyTypeName : PrimaryKeys[0].MemberSystemType.FullName; }
            }
            /// <summary>
            /// 是否多关键字
            /// </summary>
            public bool IsManyPrimaryKey
            {
                get { return PrimaryKeys.Length > 1; }
            }
            /// <summary>
            /// 第一个关键字成员
            /// </summary>
            internal MemberIndex PrimaryKey0
            {
                get { return PrimaryKeys[0]; }
            }
            /// <summary>
            /// 后续关键字成员集合
            /// </summary>
            internal MemberIndex[] NextPrimaryKeys
            {
                get { return PrimaryKeys.getSub(1, PrimaryKeys.Length - 1); }
            }
            /// <summary>
            /// 自增成员
            /// </summary>
            internal MemberIndex Identity;
            /// <summary>
            /// 是否 64b 自增标识
            /// </summary>
            public bool IsIdentity64
            {
                get
                {
                    Type type = Identity.MemberSystemType;
                    return type == typeof(long) || type == typeof(ulong);
                }
            }
            /// <summary>
            /// WEB Path 类型集合
            /// </summary>
            internal LeftArray<WebPathType> WebPaths;
            /// <summary>
            /// WEB Path 类型集合
            /// </summary>
            private Dictionary<Type, ListArray<WebPathType>> webPathTypes;
            /// <summary>
            /// 日志流计数完成类型数量
            /// </summary>
            public int SqlStreamTypeCount;
            /// <summary>
            /// 是否生成数据加载完成代码
            /// </summary>
            public bool IsSqlLoaded
            {
                get
                {
                    return SqlStreamTypeCount != 0 || IsSqlCacheLoaded;
                }
            }
            /// <summary>
            /// 索引成员
            /// </summary>
            public MemberIndex[] IndexMembers;
            /// <summary>
            /// 当前时间生成成员
            /// </summary>
            internal MemberIndex[] NowTimeMembers;
            /// <summary>
            /// 当前时间生成成员数组大小
            /// </summary>
            public int NowTimeArraySize;
            /// <summary>
            /// 默认二进制序列化是否序列化成员位图
            /// </summary>
            public bool IsDefaultSerializeIsMemberMap
            {
                get
                {
                    return (Attribute.LogServerName != null && Attribute.IsLogMemberMap) || Attribute.IsDefaultSerializeIsMemberMap;
                }
            }
            /// <summary>
            /// 默认缓存类型
            /// </summary>
            public AutoCSer.Sql.Cache.Whole.Event.Type CacheType
            {
                get
                {
                    switch (Attribute.CacheType)
                    {
                        case AutoCSer.Sql.Cache.Whole.Event.Type.IdentityArray:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.IdentityTree:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreateIdentityArray:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreateIdentityTree:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreateIdentityArrayWhere:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreateIdentityArrayWhereExpression:
                            if (Identity != null) return Attribute.CacheType;
                            break;
                        case AutoCSer.Sql.Cache.Whole.Event.Type.PrimaryKeyArray:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.PrimaryKey:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreatePrimaryKeyArray:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreatePrimaryKey:
                            if (PrimaryKeys.Length != 0) return Attribute.CacheType;
                            break;
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreateMemberKey:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.Custom:
                            if (Identity != null || PrimaryKeys.Length != 0) return Attribute.CacheType;
                            break;
                    }
                    return AutoCSer.Sql.Cache.Whole.Event.Type.Unknown;
                }
            }
            /// <summary>
            /// 是否等待数据事件缓存数据初始化完成
            /// </summary>
            public bool IsEventCacheLoaded
            {
                get
                {
                    switch (CacheType)
                    {
                        case AutoCSer.Sql.Cache.Whole.Event.Type.IdentityArray:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.IdentityTree:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.PrimaryKeyArray:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.PrimaryKey:
                            return true;
                    }
                    return IsCreateEventCache;
                }
            }
            /// <summary>
            /// 是否等待创建数据事件缓存数据初始化完成
            /// </summary>
            public bool IsCreateEventCache
            {
                get
                {
                    switch (CacheType)
                    {
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreateIdentityArray:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreateIdentityTree:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreatePrimaryKeyArray:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreatePrimaryKey:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreateMemberKey:
                            return true;
                    }
                    return false;
                }
            }
            /// <summary>
            /// 是否生成数据加载完成代码
            /// </summary>
            public bool IsSqlCacheLoaded
            {
                get
                {
                    switch (CacheType)
                    {
                        case AutoCSer.Sql.Cache.Whole.Event.Type.IdentityArray:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.IdentityTree:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.PrimaryKeyArray:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.PrimaryKey:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreateIdentityArray:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreateIdentityTree:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreatePrimaryKeyArray:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreatePrimaryKey:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.CreateMemberKey:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.Custom:
                            return true;
                    }
                    return false;
                }
            }
            /// <summary>
            /// 是否自动初始化当前时间
            /// </summary>
            public bool IsLoadNowTimeCache
            {
                get
                {
                    switch (CacheType)
                    {
                        case AutoCSer.Sql.Cache.Whole.Event.Type.IdentityArray:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.IdentityTree:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.PrimaryKeyArray:
                        case AutoCSer.Sql.Cache.Whole.Event.Type.PrimaryKey:
                            return true;
                    }
                    return false;
                }
            }
            /// <summary>
            /// SQL表格默认缓存 字段名称
            /// </summary>
            public string IdentityArrayCacheName
            {
                get { return SqlCacheName; }
            }
            /// <summary>
            /// SQL表格默认缓存 字段名称
            /// </summary>
            public string IdentityTreeCacheName
            {
                get { return IdentityArrayCacheName; }
            }
            /// <summary>
            /// SQL表格默认缓存 字段名称
            /// </summary>
            public string PrimaryKeyCacheName
            {
                get { return IdentityArrayCacheName; }
            }
            /// <summary>
            /// SQL表格默认缓存 字段名称
            /// </summary>
            public string PrimaryKeyArrayCacheName
            {
                get { return IdentityArrayCacheName; }
            }
            /// <summary>
            /// SQL表格默认缓存 字段名称
            /// </summary>
            public string CreateIdentityArrayCacheName
            {
                get { return IdentityArrayCacheName; }
            }
            /// <summary>
            /// SQL表格默认缓存 字段名称
            /// </summary>
            public string CreateIdentityTreeCacheName
            {
                get { return IdentityArrayCacheName; }
            }
            /// <summary>
            /// SQL表格默认缓存 字段名称
            /// </summary>
            public string CreatePrimaryKeyCacheName
            {
                get { return IdentityArrayCacheName; }
            }
            /// <summary>
            /// SQL表格默认缓存 字段名称
            /// </summary>
            public string CreatePrimaryKeyArrayCacheName
            {
                get { return IdentityArrayCacheName; }
            }
            /// <summary>
            /// SQL表格默认缓存 字段名称
            /// </summary>
            public string CreateMemberKeyCacheName
            {
                get { return IdentityArrayCacheName; }
            }
            /// <summary>
            /// SQL表格默认缓存 字段名称
            /// </summary>
            public string CustomCacheName
            {
                get { return IdentityArrayCacheName; }
            }
            /// <summary>
            /// 计数缓存类型
            /// </summary>
            public AutoCSer.Sql.Cache.Counter.Type CounterCacheType
            {
                get
                {
                    switch (Attribute.CounterCacheType)
                    {
                        case AutoCSer.Sql.Cache.Counter.Type.IdentityCounter:
                        case AutoCSer.Sql.Cache.Counter.Type.CreateIdentityCounterQueue:
                        case AutoCSer.Sql.Cache.Counter.Type.CreateIdentityCounterQueueList:
                            if (Identity != null) return Attribute.CounterCacheType;
                            break;
                        case AutoCSer.Sql.Cache.Counter.Type.CreateIdentityCounterMemberQueue:
                            if (Identity != null && !IsIdentity64)
                            {
                                switch (CacheType)
                                {
                                    case AutoCSer.Sql.Cache.Whole.Event.Type.CreateIdentityArray:
                                    case AutoCSer.Sql.Cache.Whole.Event.Type.CreateIdentityTree:
                                        return Attribute.CounterCacheType;
                                }
                            }
                            break;
                        case AutoCSer.Sql.Cache.Counter.Type.PrimaryKeyCounter:
                        case AutoCSer.Sql.Cache.Counter.Type.CreatePrimaryKeyCounterQueue:
                        case AutoCSer.Sql.Cache.Counter.Type.CreatePrimaryKeyCounterQueueList:
                        case AutoCSer.Sql.Cache.Counter.Type.CreatePrimaryKeyCounterQueueDictionary:
                            if (PrimaryKeys.Length != 0) return Attribute.CounterCacheType;
                            break;
                    }
                    return AutoCSer.Sql.Cache.Counter.Type.Unknown;
                }
            }
            /// <summary>
            /// 是否绑定成员缓存类型
            /// </summary>
            public bool IsMemberCache
            {
                get { return Attribute.IsMemberCache || CounterMembers.Length != 0 || CounterCacheType == AutoCSer.Sql.Cache.Counter.Type.CreateIdentityCounterMemberQueue; }
            }
            /// <summary>
            /// 成员绑定类型约束基类
            /// </summary>
            public string MemberCacheBaseType
            {
                get
                {
                    if (CounterCacheType == AutoCSer.Sql.Cache.Counter.Type.CreateIdentityCounterMemberQueue) return null;
                    return "class";
                }
            }
            /// <summary>
            /// SQL表格默认计数缓存 字段名称
            /// </summary>
            public string IdentityCounterCacheName
            {
                get { return "sqlCacheCounter"; }
            }
            /// <summary>
            /// SQL表格默认计数缓存 字段名称
            /// </summary>
            public string IdentityCounter32CacheName
            {
                get { return IdentityCounterCacheName; }
            }
            /// <summary>
            /// SQL表格默认计数缓存 字段名称
            /// </summary>
            public string PrimaryKeyCounterCacheName
            {
                get { return IdentityCounterCacheName; }
            }
            /// <summary>
            /// SQL表格默认计数缓存 字段名称
            /// </summary>
            public string CreateIdentityCounterMemberQueueCacheName
            {
                get { return IdentityCounterCacheName; }
            }
            /// <summary>
            /// SQL表格默认计数缓存 字段名称
            /// </summary>
            public string CreateIdentityCounterQueueCacheName
            {
                get { return IdentityCounterCacheName; }
            }
            /// <summary>
            /// SQL表格默认计数缓存 字段名称
            /// </summary>
            public string CreateIdentityCounter32QueueCacheName
            {
                get { return IdentityCounterCacheName; }
            }
            /// <summary>
            /// SQL表格默认计数缓存 字段名称
            /// </summary>
            public string CreateIdentityCounterQueueListCacheName
            {
                get { return IdentityCounterCacheName; }
            }
            /// <summary>
            /// SQL表格默认计数缓存 字段名称
            /// </summary>
            public string CreateIdentityCounter32QueueListCacheName
            {
                get { return IdentityCounterCacheName; }
            }
            /// <summary>
            /// SQL表格默认计数缓存 字段名称
            /// </summary>
            public string CreatePrimaryKeyCounterQueueCacheName
            {
                get { return IdentityCounterCacheName; }
            }
            /// <summary>
            /// SQL表格默认计数缓存 字段名称
            /// </summary>
            public string CreatePrimaryKeyCounterQueueListCacheName
            {
                get { return IdentityCounterCacheName; }
            }
            /// <summary>
            /// SQL表格默认计数缓存 字段名称
            /// </summary>
            public string CreatePrimaryKeyCounterQueueDictionaryCacheName
            {
                get { return IdentityCounterCacheName; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string CreateIdentityCounterQueueMethodName
            {
                get { return "createIdentityCounterQueue"; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string CreateIdentityCounterQueueListMethodName
            {
                get { return "createIdentityCounterQueueList"; }
            }
            /// <summary>
            /// 数据更新成员类型类型
            /// </summary>
            public string UpdateValueClass
            {
                get { return Attribute.IsUpdateMemberMapClassType ? " sealed class" : " struct"; }
            }
            /// <summary>
            /// 是否生成默认的获取缓存的函数
            /// </summary>
            public bool IsGetSqlCache
            {
                get
                {
                    //if (Attribute.LogServerName != null && Attribute.IsLogClientGetCache) return true;
                    if (Attribute.IsRemoteKey || (Attribute.LogServerName != null && Attribute.IsLogClientGetCache))
                    {
                        switch (CacheType)
                        {
                            case Sql.Cache.Whole.Event.Type.IdentityArray:
                            case Sql.Cache.Whole.Event.Type.IdentityTree:
                            case Sql.Cache.Whole.Event.Type.PrimaryKeyArray:
                            case Sql.Cache.Whole.Event.Type.PrimaryKey:
                            case Sql.Cache.Whole.Event.Type.CreateIdentityArray:
                            case Sql.Cache.Whole.Event.Type.CreateIdentityTree:
                            case Sql.Cache.Whole.Event.Type.CreatePrimaryKey:
                            case Sql.Cache.Whole.Event.Type.CreatePrimaryKeyArray:
                                return true;
                        }
                    }
                    return false;
                }
            }
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void nextCreate()
            {
                MemberIndex identity = Identity = null;
                int isIdentityCase = SqlStreamTypeCount = NowTimeArraySize = 0;
                LeftArray<MemberIndex> members = default(LeftArray<MemberIndex>), primaryKeys = default(LeftArray<MemberIndex>), indexMembers = default(LeftArray<MemberIndex>);
                LeftArray<CountMember> counterMembers = default(LeftArray<CountMember>);
                LeftArray<MemberIndex> nowTimeMembers = default(LeftArray<MemberIndex>);
                LeftArray<LogMember> logMembers = new LeftArray<LogMember>();
                LeftArray<string> strings = default(LeftArray<string>);
                IsLogProxyMember = false;
                foreach (MemberIndex member in MemberIndex.GetMembers(Type, Attribute.MemberFilters))
                {
                    if (!member.IsIgnore)
                    {
                        AutoCSer.Sql.MemberAttribute attribute = member.GetAttribute<AutoCSer.Sql.MemberAttribute>(false);
                        bool isMember = attribute == null || attribute.IsSetup;
                        if (Attribute.LogServerName != null)
                        {
                            AutoCSer.Sql.LogAttribute logAttribute = member.GetAttribute<AutoCSer.Sql.LogAttribute>(false);
                            if (logAttribute != null)
                            {
                                LogMember logMember = new LogMember { Member = member, Attribute = logAttribute, MemberAttribute = attribute };
                                logMembers.Add(logMember);
                                if (logMember.IsProxy) IsLogProxyMember = true;
                                if (!logAttribute.IsMember) isMember = false;
                            }
                        }
                        if (isMember)
                        {
                            members.Add(member);
                            if (attribute != null)
                            {
                                if (attribute.IsMemberIndex) indexMembers.Add(member);
                                if (attribute.IsNowTime && member.MemberSystemType == typeof(DateTime))
                                {
                                    nowTimeMembers.Add(member);
                                    if (member.MemberIndex >= NowTimeArraySize) NowTimeArraySize = member.MemberIndex + 1;
                                }
                                if (attribute.PrimaryKeyIndex != 0) primaryKeys.Add(member);
                                if (attribute.IsIdentity) Identity = member;
                            }
                            if (Identity == null)
                            {
                                if (isIdentityCase == 0 && member.MemberName == AutoCSer.Sql.Field.IdentityName)
                                {
                                    identity = member;
                                    isIdentityCase = 1;
                                }
                                else if (identity == null && member.MemberName.Length == AutoCSer.Sql.Field.IdentityName.Length && member.MemberName.ToLower() == AutoCSer.Sql.Field.IdentityName) identity = member;
                            }
                            AutoCSer.Sql.CountAttribute countAttribute = member.GetAttribute<AutoCSer.Sql.CountAttribute>(false);
                            if (countAttribute != null) counterMembers.Add(new CountMember { Member = member, Attribute = countAttribute });
                            if (member.MemberSystemType == typeof(string) && (attribute == null || (attribute.MaxStringLength == 0 && !attribute.IsIgnoreMaxStringLength))) strings.Add(member.MemberName);
                        }
                    }
                }
                if (Identity == null) Identity = identity;
                if (Attribute.LogServerName == null) LogMembers = NullValue<LogMember>.Array;
                else
                {
                    LogMembers = logMembers.ToArray();
                    if (!Attribute.IsDefaultSerialize && Attribute.IsLogMemberMap)
                    {
                        AutoCSer.BinarySerialize.SerializeAttribute dataSerialize = Type.Type.customAttribute<AutoCSer.BinarySerialize.SerializeAttribute>();
                        if (dataSerialize != null && !dataSerialize.IsMemberMap) Messages.Message("数据库日志流处理类型 " + Type.FullName + " 序列化不支持成员位图");
                    }
                    foreach (LogMember member in LogMembers)
                    {
                        if (member.Attribute.CountType != null) ++SqlStreamTypeCount;
                    }
                }
                CounterMembers = counterMembers.ToArray();
                NowTimeMembers = nowTimeMembers.ToArray();
                if (strings.Length != 0) Messages.Message(Type.FullName + " 字符串字段缺少最大长度限制 " + string.Join(",", strings.ToArray()));
                WebPaths.Length = 0;
                ListArray<WebPathType> types;
                if (webPathTypes == null)
                {
                    webPathTypes = DictionaryCreator.CreateOnly<Type, ListArray<WebPathType>>();
                    foreach (Type nextType in AutoParameter.Types)
                    {
                        AutoCSer.WebView.PathAttribute webPath = nextType.customAttribute<AutoCSer.WebView.PathAttribute>();
                        if (webPath != null && webPath.Type != null && webPath.MemberName != null)
                        {
                            FieldInfo[] fields = nextType.GetFields(BindingFlags.Instance | BindingFlags.Public);
                            if (fields.Length != 0)
                            {
                                if (!webPathTypes.TryGetValue(webPath.Type, out types)) webPathTypes.Add(webPath.Type, types = new ListArray<WebPathType>());
                                int memberIndex = fields.Length;
                                types.Add(new WebPathType { MemberType = nextType, Attribute = webPath, Members = fields.getArray(value => new MemberIndex(value, --memberIndex)) });
                            }
                        }
                    }
                }
                if (webPathTypes.TryGetValue(Type, out types))
                {
                    foreach (WebPathType webPath in types)
                    {
                        MemberIndex[] fields = webPath.CheckMembers(ref members);
                        if (fields != null) WebPaths.Add(new WebPathType { MemberType = webPath.MemberType, Attribute = webPath.Attribute, Members = fields });
                    }
                }
                dataPrimaryKey.Run(Type, PrimaryKeys = primaryKeys.ToArray());
                if (Attribute.IsUpdateMemberMap)
                {
                    if (Identity == null) UpdateMembers = members.GetFindArray(value => Array.IndexOf(PrimaryKeys, value) < 0);
                    else UpdateMembers = members.GetFindArray(value => value != Identity);
                    if (indexMembers.Length != 0) indexMembers.Remove(value => Array.IndexOf(UpdateMembers, value) >= 0);
                    if (indexMembers.Length == 0) indexMembers.Set(UpdateMembers);
                    else indexMembers.Add(UpdateMembers);
                }
                else UpdateMembers = NullValue<MemberIndex>.Array;
                IndexMembers = indexMembers.ToArray();
                create(true);
            }
            /// <summary>
            /// 安装完成处理
            /// </summary>
            protected override void onCreated() { }
        }
    }
}
