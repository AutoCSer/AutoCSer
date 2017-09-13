using System;
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
                        return Attribute.MemberName;
                    }
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
            internal RemoteMember[] RemoteMembers;
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void nextCreate()
            {
                Type modelType;
                AutoCSer.Sql.ModelAttribute modelAttribute = Type.Type.customAttribute<AutoCSer.Sql.ModelAttribute>(out modelType);
                if (modelAttribute != null && modelAttribute.LogServerName != null)
                {
                    LeftArray<RemoteMember> remoteMembers = new LeftArray<RemoteMember>();
                    foreach (MethodIndex method in MethodIndex.GetMethods<AutoCSer.Net.TcpStaticServer.MethodAttribute>(Type, AutoCSer.Metadata.MemberFilters.Static, false, true, false))
                    {
                        AutoCSer.Sql.RemoteMemberAttribute remoteAttribute = method.GetSetupAttribute<AutoCSer.Sql.RemoteMemberAttribute>(false);
                        if (remoteAttribute != null) remoteMembers.Add(new RemoteMember { Type = Type, ProjectParameter = AutoParameter, Method = method, Attribute = remoteAttribute });
                    }
                    RemoteMembers = remoteMembers.ToArray();

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
                                        foreach (RemoteMember remoteMember in RemoteMembers)
                                        {
                                            if (remoteMember.Attribute.MemberName == member.MemberName)
                                            {
                                                remoteMember.Member = member;
                                                remoteMember.IsLogProxyMember = true;
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
                    if (logMembers.Length != 0 && (Identity != null || modelAttribute.CacheType != Sql.Cache.Whole.Event.Type.CreateMemberKey)) LogMembers = logMembers.ToArray();
                    else LogMembers = NullValue<SqlModel.Generator.LogMember>.Array;
                    if (LogMembers.Length != 0 || RemoteMembers.Length != 0)
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
