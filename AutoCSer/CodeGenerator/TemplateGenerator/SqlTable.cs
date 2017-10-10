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
        internal partial class Generator : TemplateGenerator.Generator<AutoCSer.Sql.TableAttribute>
        {
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
            /// 远程调用链类型信息
            /// </summary>
            public TcpStaticServer.Generator.RemoteLinkType RemoteLinkType;
            /// <summary>
            /// 远程成员集合
            /// </summary>
            internal TcpStaticServer.Generator.RemoteMethod[] RemoteMethods;
            /// <summary>
            /// 是否生成远程成员
            /// </summary>
            public bool IsRemoteMember;
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void nextCreate()
            {
                Type modelType;
                AutoCSer.Sql.ModelAttribute modelAttribute = Type.Type.customAttribute<AutoCSer.Sql.ModelAttribute>(out modelType);
                if (modelAttribute != null)
                {
                    AutoCSer.Net.TcpStaticServer.ServerAttribute serverAttribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpStaticServer.ServerAttribute>(Type, false);
                    if (serverAttribute != null && serverAttribute.ServerName != null && !serverAttribute.IsRemoteLinkType && serverAttribute.IsRemoteLink && (RemoteLinkType = TcpStaticServer.Generator.GetRemoteLinkType(Type)) != null)
                    {
                        LeftArray<TcpStaticServer.Generator.RemoteMethod> remoteMethods = new LeftArray<TcpStaticServer.Generator.RemoteMethod>();
                        foreach (MethodIndex method in MethodIndex.GetMethods<AutoCSer.Net.TcpStaticServer.MethodAttribute>(Type, serverAttribute.MemberFilters, false, serverAttribute.IsAttribute, serverAttribute.IsBaseTypeAttribute))
                        {
                            if (!method.Method.IsGenericMethodDefinition)
                            {
                                AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute remoteAttribute = method.GetSetupAttribute<AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute>(false);
                                if (remoteAttribute != null && !remoteAttribute.IsClientRemoteMember)
                                {
                                    MethodParameter[] parameters = method.Parameters;
                                    if (parameters.Length >= 1 && parameters[0].ParameterType == RemoteLinkType.ParameterType) remoteMethods.Add(new TcpStaticServer.Generator.RemoteMethod { Type = Type, Method = method, MethodParameters = parameters, Attribute = remoteAttribute });
                                }
                            }
                        }

                        LeftArray<SqlModel.Generator.LogMember> logMembers = new LeftArray<SqlModel.Generator.LogMember>();
                        foreach (MemberIndex member in MemberIndex.GetMembers(modelType, modelAttribute.MemberFilters))
                        {
                            if (!member.IsIgnore)
                            {
                                AutoCSer.Sql.MemberAttribute attribute = member.GetAttribute<AutoCSer.Sql.MemberAttribute>(false);
                                if (modelAttribute.LogServerName != null)
                                {
                                    AutoCSer.Sql.LogAttribute logAttribute = member.GetAttribute<AutoCSer.Sql.LogAttribute>(false);
                                    if (logAttribute != null)
                                    {
                                        SqlModel.Generator.LogMember logMember = new SqlModel.Generator.LogMember { Member = member, Attribute = logAttribute, MemberAttribute = attribute };
                                        if (modelAttribute.LogServerName != null && logMember.IsProxy)
                                        {
                                            logMembers.Add(logMember);
                                            foreach (TcpStaticServer.Generator.RemoteMethod remoteMethod in remoteMethods)
                                            {
                                                if (remoteMethod.Attribute.MemberName == member.MemberName)
                                                {
                                                    remoteMethod.Member = member;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        RemoteMethods = remoteMethods.GetFindArray(value => value.Member != null);
                        LogMembers = logMembers.Length != 0 && (modelAttribute.CacheType != AutoCSer.Sql.Cache.Whole.Event.Type.Unknown && modelAttribute.CacheType != AutoCSer.Sql.Cache.Whole.Event.Type.CreateMemberKey) ? logMembers.ToArray() : NullValue<SqlModel.Generator.LogMember>.Array;
                        if ((LogMembers.Length | RemoteMethods.Length) != 0)
                        {
                            //create(true);
                            IsRemoteMember = RemoteMethods.Length != 0 && TcpStaticServer.Generator.RemoteMemberTypes.Add(Type);
                            CSharpTypeDefinition definition = new CSharpTypeDefinition(Type, true, false);
                            _code_.Length = 0;
                            create(false);
                            Coder.Add(definition.Start + _partCodes_["SERVER"] + definition.End);
                            TcpStaticServer.Generator.AddClientCode(new TcpStaticServer.Generator.ClientCode { Type = Type, Attribute = serverAttribute, Code = definition.Start + _partCodes_["CLIENT"] + definition.End, SegmentationCode = definition.Start + _partCodes_["CLIENTCALL"] + definition.End });
                        }
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
