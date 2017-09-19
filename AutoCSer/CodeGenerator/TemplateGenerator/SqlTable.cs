using System;
using System.Collections.Generic;
using System.Reflection;
using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extension;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 数据表格 代码生成
    /// </summary>
    internal abstract partial class SqlTable
    {
        /// <summary>
        /// 数据表格 代码生成
        /// </summary>
        [Generator(Name = "数据表格", DependType = typeof(CSharper), IsAuto = true, IsDotNet2 = false, IsMono = false)]
        internal partial class Generator : Generator<AutoCSer.Sql.TableAttribute>
        {
            /// <summary>
            /// 远程成员
            /// </summary>
            internal sealed class RemoteMember
            {
                /// <summary>
                /// 远程成员信息
                /// </summary>
                public MemberIndex Member;
                /// <summary>
                /// 获取数据函数名称
                /// </summary>
                public string GetMemberName
                {
                    get { return "get" + Member.MemberName; }
                }
                /// <summary>
                /// 获取函数信息
                /// </summary>
                public MethodIndex Method;
                /// <summary>
                /// 远程函数名称
                /// </summary>
                public string RemoteMethodName
                {
                    get { return "remote_" + Method.MemberName; }
                }
                /// <summary>
                /// 函数是否存在返回值
                /// </summary>
                public bool IsMethodReturn
                {
                    get { return Method != null && Method.ReturnType.Type != typeof(void); }
                }
                /// <summary>
                /// 返回值类型
                /// </summary>
                public ExtensionType MethodReturnType
                {
                    get { return Method.ReturnType; }
                }
            }
            /// <summary>
            /// 远程成员
            /// </summary>
            internal sealed class RemoteMethod
            {
                /// <summary>
                /// 当前类型
                /// </summary>
                public ExtensionType Type;
                /// <summary>
                /// 项目安装参数
                /// </summary>
                public ProjectParameter ProjectParameter;
                /// <summary>
                /// 远程成员信息
                /// </summary>
                public MemberIndex Member;
                /// <summary>
                /// 获取函数信息
                /// </summary>
                public MethodIndex Method;
                /// <summary>
                /// 函数是否存在返回值
                /// </summary>
                public bool IsMethodReturn
                {
                    get { return Method.ReturnType.Type != typeof(void); }
                }
                /// <summary>
                /// 函数参数信息
                /// </summary>
                public MethodParameter[] MethodParameters;
                /// <summary>
                /// 是生成函数还是属性
                /// </summary>
                public bool IsMethod
                {
                    get { return MethodParameters.Length > 1 || Method.ReturnType.Type == typeof(void); }
                }
                /// <summary>
                /// 函数参数信息
                /// </summary>
                public SubArray<MethodParameter> NextParameters
                {
                    get { return new SubArray<MethodParameter>(MethodParameters, 1, MethodParameters.Length - 1); }
                }
                /// <summary>
                /// 远程成员配置
                /// </summary>
                public AutoCSer.Sql.RemoteMemberAttribute Attribute;
                /// <summary>
                /// 是否日志字段代理
                /// </summary>
                public bool IsLogProxyMember;
                ///// <summary>
                ///// TcpCall 调用类型名称
                ///// </summary>
                //public string TcpCallType
                //{
                //    get { return TcpStaticServer.Generator.GetTcpCallTypeName(ProjectParameter, Type); }
                //}
                /// <summary>
                /// 函数返回值类型
                /// </summary>
                public ExtensionType MethodReturnType
                {
                    get { return Method.ReturnType; }
                }
                /// <summary>
                /// 成员名称
                /// </summary>
                public string AttributeMemberName
                {
                    get
                    {
                        return Attribute.MemberName ?? Method.MemberName;
                    }
                }
                /// <summary>
                /// 成员名称
                /// </summary>
                public string AttributeMethodName
                {
                    get { return AttributeMemberName; }
                }
            }
            /// <summary>
            /// 远程缓存成员
            /// </summary>
            internal struct RemoteCacheMember
            {
                /// <summary>
                /// 远程成员缓存配置
                /// </summary>
                public AutoCSer.Sql.RemoteMemberCacheAttribute Attribute;
                /// <summary>
                /// 远程成员信息
                /// </summary>
                public MemberIndex Member;
                /// <summary>
                /// 层级索引
                /// </summary>
                public int Index;
                /// <summary>
                /// 局部变量名称
                /// </summary>
                public string IndexName
                {
                    get { return "value" + Index.toString(); }
                }
                /// <summary>
                /// 局部变量名称
                /// </summary>
                public string ParentIndexName
                {
                    get { return Index == 0 ? "value" : ("value" + (Index - 1).toString()); }
                }
                /// <summary>
                /// 是否需要可空检查
                /// </summary>
                public bool IsNull
                {
                    get
                    {
                        if (Attribute.IsNull)
                        {
                            Type type = Member.MemberSystemType;
                            return !type.IsValueType && !type.IsEnum;
                        }
                        return false;
                    }
                }
            }
            /// <summary>
            /// 远程缓存成员
            /// </summary>
            internal sealed class RemoteCache
            {
                /// <summary>
                /// 远程成员配置
                /// </summary>
                public AutoCSer.Sql.RemoteMemberAttribute Attribute;
                /// <summary>
                /// 前缀成员
                /// </summary>
                public RemoteCacheMember[] Members;
                /// <summary>
                /// 局部变量名称
                /// </summary>
                public string IndexName
                {
                    get { return "value" + (Members.Length - 1).toString(); }
                }
                /// <summary>
                /// 远程成员信息
                /// </summary>
                public MemberIndex Member;
                /// <summary>
                /// 获取远程成员名称
                /// </summary>
                public string GetCacheMemberName
                {
                    get
                    {
                        return "get" + Members.joinString('_', value => value.Member.MemberName) + "_" + (Attribute.MemberName ?? Member.MemberName);
                    }
                }
                /// <summary>
                /// 获取函数信息
                /// </summary>
                public MethodIndex Method;
                /// <summary>
                /// 获取远程函数名称
                /// </summary>
                public string RemoteCacheMethodName
                {
                    get
                    {
                        return "remote_" + Members.joinString('_', value => value.Member.MemberName) + "_" + (Attribute.MemberName ?? Method.MethodName);
                    }
                }
                /// <summary>
                /// 函数是否存在返回值
                /// </summary>
                public bool IsMethodReturn
                {
                    get { return Method != null && Method.ReturnType.Type != typeof(void); }
                }
                /// <summary>
                /// 返回值类型
                /// </summary>
                public ExtensionType MethodReturnType
                {
                    get { return Method.ReturnType; }
                }
                /// <summary>
                /// 远程成员名称
                /// </summary>
                public string CacheMemberName
                {
                    get
                    {
                        string prefix = string.Empty;
                        switch (Attribute.NameType)
                        {
                            case AutoCSer.Sql.RemoteMemberAttribute.Type.Concat: prefix = string.Concat(Members.getArray(value => value.Member.MemberName)); break;
                            case AutoCSer.Sql.RemoteMemberAttribute.Type.Join: prefix = Members.joinString('_', value => value.Member.MemberName) + "_"; break;
                        }
                        return prefix + (Attribute.MemberName ?? (Member == null ? Method.MethodName : Member.MemberName));
                    }
                }
                /// <summary>
                /// 是否需要可空检查
                /// </summary>
                public bool IsAnyNull
                {
                    get { return Members.any(value => value.IsNull); }
                }
            }
            /// <summary>
            /// 远程缓存成员创建
            /// </summary>
            internal sealed class RemoteCacheBuilder
            {
                /// <summary>
                /// 远程缓存成员集合
                /// </summary>
                internal LeftArray<RemoteCache> Caches;
                /// <summary>
                /// 远程缓存成员集合
                /// </summary>
                private LeftArray<RemoteCacheMember> members;
                /// <summary>
                /// 添加远程缓存成员
                /// </summary>
                /// <param name="member"></param>
                internal void Push(MemberIndex member)
                {
                    members.Add(new RemoteCacheMember { Member = member, Attribute = member.GetSetupAttribute<AutoCSer.Sql.RemoteMemberCacheAttribute>(false), Index = members.Length });
                    cache cache = getCache(member.MemberSystemType);
                    if (cache.Members.Length != 0)
                    {
                        RemoteCacheMember[] memberArray = members.ToArray();
                        foreach (RemoteCache nextMember in cache.Members)
                        {
                            Caches.Add(new RemoteCache { Members = memberArray, Method = nextMember.Method, Member = nextMember.Member, Attribute = nextMember.Attribute });
                        }
                    }
                    foreach (MemberIndex nextMember in cache.Caches) Push(nextMember);
                    --members.Length;
                }

                /// <summary>
                /// 成员集合缓存
                /// </summary>
                [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
                private struct cache
                {
                    /// <summary>
                    /// 远程成员集合
                    /// </summary>
                    public RemoteCache[] Members;
                    /// <summary>
                    /// 远程缓存成员集合
                    /// </summary>
                    public MemberIndex[] Caches;
                }
                /// <summary>
                /// 成员集合缓存
                /// </summary>
                private static readonly Dictionary<Type, cache> memberCache = DictionaryCreator.CreateOnly<Type, cache>();
                /// <summary>
                /// 获取成员集合
                /// </summary>
                /// <param name="type"></param>
                /// <returns></returns>
                private static cache getCache(Type type)
                {
                    cache members;
                    if (!memberCache.TryGetValue(type, out members))
                    {
                        LeftArray<RemoteCache> remoteMembers = new LeftArray<RemoteCache>();
                        foreach (MemberIndex member in MemberIndex.GetMembers<AutoCSer.Sql.RemoteMemberAttribute>(type, AutoCSer.Metadata.MemberFilters.Instance, true, false))
                        {
                            if (member.CanGet)
                            {
                                AutoCSer.Sql.RemoteMemberAttribute remoteAttribute = member.GetSetupAttribute<AutoCSer.Sql.RemoteMemberAttribute>(false);
                                if (remoteAttribute != null) remoteMembers.Add(new RemoteCache { Member = member, Attribute = remoteAttribute });
                            }
                        }
                        foreach (MethodIndex member in MethodIndex.GetMethods<AutoCSer.Sql.RemoteMemberAttribute>(type, AutoCSer.Metadata.MemberFilters.Instance, false, true, false))
                        {
                            AutoCSer.Sql.RemoteMemberAttribute remoteAttribute = member.GetSetupAttribute<AutoCSer.Sql.RemoteMemberAttribute>(false);
                            if (remoteAttribute != null) remoteMembers.Add(new RemoteCache { Method = member, Attribute = remoteAttribute });
                        }
                        memberCache.Add(type, members = new cache { Members = remoteMembers.ToArray(), Caches = MemberIndex.GetMembers<AutoCSer.Sql.RemoteMemberCacheAttribute>(type, AutoCSer.Metadata.MemberFilters.Instance, true, false) });
                    }
                    return members;
                }
            }
            /// <summary>
            /// 自增成员
            /// </summary>
            internal MemberIndex Identity;
            /// <summary>
            /// 关键字成员集合
            /// </summary>
            internal MemberIndex[] PrimaryKeys;
            /// <summary>
            /// 关键字类型名称
            /// </summary>
            public string PrimaryKeyType
            {
                get { return PrimaryKeys.Length > 1 ? SqlModel.Generator.PrimaryKeyTypeName : PrimaryKeys[0].MemberSystemType.FullName; }
            }
            /// <summary>
            /// 关键字参数名称
            /// </summary>
            public string PrimaryKeyName
            {
                get { return PrimaryKeys.Length > 1 ? "key" : PrimaryKey0.MemberName; }
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
            /// 是否单关键字
            /// </summary>
            public bool IsSinglePrimaryKey
            {
                get { return PrimaryKeys.Length == 1; }
            }
            /// <summary>
            /// 日志同步字段集合
            /// </summary>
            internal SqlModel.Generator.LogMember[] LogMembers;
            /// <summary>
            /// SQL表格计算列日志字段代理名称
            /// </summary>
            public string LogProxyMemberName
            {
                get { return SqlModel.Generator.DefaultLogProxyMemberName; }
            }
            /// <summary>
            /// 计算列加载完成字段名称
            /// </summary>
            public string IsSqlLogProxyLoadedName
            {
                get { return AutoCSer.Sql.LogStream.Log.IsSqlLogProxyLoadedName; }
            }
            /// <summary>
            /// SQL表格默认缓存 字段名称
            /// </summary>
            public string IdentityArrayCacheName
            {
                get { return SqlModel.Generator.SqlCacheName; }
            }
            /// <summary>
            /// SQL表格默认缓存 字段名称
            /// </summary>
            public string PrimaryKeyCacheName
            {
                get { return IdentityArrayCacheName; }
            }
            /// <summary>
            /// 远程成员集合
            /// </summary>
            internal RemoteMethod[] RemoteMethods;
            /// <summary>
            /// 远程成员集合
            /// </summary>
            internal RemoteMember[] RemoteMembers;
            /// <summary>
            /// 远程缓存成员集合
            /// </summary>
            internal RemoteCache[] RemoteCaches;
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void nextCreate()
            {
                Type modelType;
                AutoCSer.Sql.ModelAttribute modelAttribute = Type.Type.customAttribute<AutoCSer.Sql.ModelAttribute>(out modelType);
                if (modelAttribute != null && modelAttribute.LogServerName != null)
                {
                    LeftArray<RemoteMethod> remoteMethods = new LeftArray<RemoteMethod>();
                    foreach (MethodIndex method in MethodIndex.GetMethods<AutoCSer.Net.TcpStaticServer.MethodAttribute>(Type, AutoCSer.Metadata.MemberFilters.Static, false, true, false))
                    {
                        AutoCSer.Sql.RemoteMemberAttribute remoteAttribute = method.GetSetupAttribute<AutoCSer.Sql.RemoteMemberAttribute>(false);
                        if (remoteAttribute != null)
                        {
                            MethodParameter[] parameters = method.Parameters;
                            if(parameters.Length >= 1) remoteMethods.Add(new RemoteMethod { Type = Type, ProjectParameter = AutoParameter, Method = method, MethodParameters = parameters, Attribute = remoteAttribute });
                        }
                    }
                    RemoteMethods = remoteMethods.ToArray();

                    MemberIndex identity = Identity = null;
                    int isIdentityCase = 0;
                    LeftArray<MemberIndex> primaryKeys = default(LeftArray<MemberIndex>);
                    LeftArray<SqlModel.Generator.LogMember> logMembers = new LeftArray<SqlModel.Generator.LogMember>();
                    foreach (MemberIndex member in MemberIndex.GetMembers(modelType, modelAttribute.MemberFilters))
                    {
                        if (!member.IsIgnore)
                        {
                            AutoCSer.Sql.MemberAttribute attribute = member.GetAttribute<AutoCSer.Sql.MemberAttribute>(false);
                            bool isMember = attribute == null || attribute.IsSetup;
                            if (modelAttribute.LogServerName != null)
                            {
                                AutoCSer.Sql.LogAttribute logAttribute = member.GetAttribute<AutoCSer.Sql.LogAttribute>(false);
                                if (logAttribute != null)
                                {
                                    SqlModel.Generator.LogMember logMember = new SqlModel.Generator.LogMember { Member = member, Attribute = logAttribute, MemberAttribute = attribute };
                                    if (logMember.IsProxy)
                                    {
                                        logMembers.Add(logMember);
                                        foreach (RemoteMethod remoteMethod in RemoteMethods)
                                        {
                                            if (remoteMethod.Attribute.MemberName == member.MemberName)
                                            {
                                                remoteMethod.Member = member;
                                                remoteMethod.IsLogProxyMember = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (!logAttribute.IsMember) isMember = false;
                                }
                            }
                            if (isMember)
                            {
                                if (attribute != null)
                                {
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

                            }
                        }
                    }
                    if (Identity == null) Identity = identity;
                    bool isRemote = Identity != null || modelAttribute.CacheType != Sql.Cache.Whole.Event.Type.CreateMemberKey;
                    if (RemoteMethods.Length != 0 && isRemote)
                    {
                        bool isError = false;
                        Type parameterType = Identity == null ? (PrimaryKeys.Length == 1 ? PrimaryKeys[0].MemberSystemType : null) : Identity.MemberSystemType;
                        if (parameterType != null)
                        {
                            foreach (RemoteMethod member in RemoteMethods)
                            {
                                if (member.MethodParameters[0].ParameterType != parameterType)
                                {
                                    Messages.Add(Type.FullName + "." + member.Method.MethodName + "(" + member.MethodParameters.joinString(',', parameter => parameter.ParameterType.FullName) + ") 的第一个参数类型不是 " + parameterType.fullName() + "，不能配置为远程成员");
                                    isError = true;
                                }
                            }
                        }
                        else
                        {
                            foreach (RemoteMethod member in RemoteMethods)
                            {
                                if (member.MethodParameters[0].ParameterType.Type.Name != SqlModel.Generator.PrimaryKeyTypeName)
                                {
                                    Messages.Add(Type.FullName + "." + member.Method.MethodName + "(" + member.MethodParameters.joinString(',', parameter => parameter.ParameterType.FullName) + ") 的第一个参数类型不是 " + SqlModel.Generator.PrimaryKeyTypeName + "，不能配置为远程成员");
                                    isError = true;
                                }
                            }
                        }
                        if (isError) return;
                    }
                    if (isRemote)
                    {
                        LeftArray<RemoteMember> remoteMembers = new LeftArray<RemoteMember>();
                        foreach (MemberIndex member in MemberIndex.GetMembers<AutoCSer.Sql.RemoteMemberAttribute>(Type, AutoCSer.Metadata.MemberFilters.NonPublicInstanceProperty, true, false))
                        {
                            if (member.CanGet)
                            {
                                AutoCSer.Sql.RemoteMemberAttribute remoteAttribute = member.GetSetupAttribute<AutoCSer.Sql.RemoteMemberAttribute>(false);
                                if (remoteAttribute != null) remoteMembers.Add(new RemoteMember { Member = member });
                            }
                        }
                        foreach (MethodIndex member in MethodIndex.GetMethods<AutoCSer.Sql.RemoteMemberAttribute>(Type, AutoCSer.Metadata.MemberFilters.NonPublicInstance, false, true, false))
                        {
                            AutoCSer.Sql.RemoteMemberAttribute remoteAttribute = member.GetSetupAttribute<AutoCSer.Sql.RemoteMemberAttribute>(false);
                            if (remoteAttribute != null) remoteMembers.Add(new RemoteMember { Method = member });
                        }
                        RemoteMembers = remoteMembers.ToArray();

                        RemoteCacheBuilder cacheBuilder = new RemoteCacheBuilder();
                        foreach (MemberIndex member in MemberIndex.GetMembers<AutoCSer.Sql.RemoteMemberCacheAttribute>(Type, AutoCSer.Metadata.MemberFilters.NonPublicInstance, true, false)) cacheBuilder.Push(member);
                        RemoteCaches = cacheBuilder.Caches.ToArray();
                    }
                    else
                    {
                        RemoteMembers = NullValue<RemoteMember>.Array;
                        RemoteCaches = NullValue<RemoteCache>.Array;
                    }
                    LogMembers = logMembers.Length != 0 && isRemote ? logMembers.ToArray() : NullValue<SqlModel.Generator.LogMember>.Array;
                    if ((LogMembers.Length | RemoteMethods.Length | RemoteMembers.Length | RemoteCaches.Length) != 0)
                    {
                        PrimaryKeys = primaryKeys.ToArray();
                        create(true);
                    }
                }
            }
            /// <summary>
            /// 安装完成处理
            /// </summary>
            protected override void onCreated() { }
        }
    }
}
