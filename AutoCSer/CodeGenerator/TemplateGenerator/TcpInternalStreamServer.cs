﻿using System;
using AutoCSer.Extension;
using System.Reflection;
using AutoCSer.Metadata;
using AutoCSer.CodeGenerator.Metadata;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// TCP 内部服务代码生成
    /// </summary>
    internal sealed partial class TcpInternalStreamServer : TcpStreamServer
    {
        /// <summary>
        /// TCP 内部服务代码生成
        /// </summary>
        [Generator(Name = "TCP 应答流服务", DependType = typeof(CSharper), IsAuto = true)]
        internal sealed partial class Generator : Generator<AutoCSer.Net.TcpInternalStreamServer.ServerAttribute, AutoCSer.Net.TcpStreamServer.MethodAttribute, AutoCSer.Net.TcpInternalStreamServer.ServerSocketSender>
        {
            /// <summary>
            /// 服务注册名称
            /// </summary>
            public override string ServerRegisterName { get { return Attribute.ServerName ?? Type.FullName; } }
            /// <summary>
            /// 是否默认时间验证服务
            /// </summary>
            public bool IsTimeVerify
            {
                get { return typeof(AutoCSer.Net.TcpInternalStreamServer.TimeVerifyServer).IsAssignableFrom(Type); }
            }
            /// <summary>
            /// 是否生成客户端代码
            /// </summary>
            public bool IsClientCode;
            /// <summary>
            /// 是否生成服务器端代码
            /// </summary>
            public bool IsServerCode;
            /// <summary>
            /// 是否存在 AutoCSer.Net.TcpServer.ISetTcpServer 接口函数
            /// </summary>
            public bool IsSetTcpServer
            {
                get
                {
#if NOJIT
                    return isSetTcpServer
#else
                    return typeof(AutoCSer.Net.TcpServer.ISetTcpServer<AutoCSer.Net.TcpInternalStreamServer.Server>).IsAssignableFrom(Type.Type)
#endif
                        || Type.Type.GetMethod("SetTcpServer", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(AutoCSer.Net.TcpInternalStreamServer.Server) }, null) != null;
                }
            }
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected unsafe override void nextCreate()
            {
                if (Type.Type.IsClass && !Type.Type.IsAbstract)
                {
                    LeftArray<TcpMethod> methodArray = new LeftArray<TcpMethod>(Metadata.MethodIndex.GetMethods<AutoCSer.Net.TcpStreamServer.MethodAttribute>(Type, Attribute.GetMemberFilters, false, Attribute.IsAttribute, Attribute.IsBaseTypeAttribute)
                        .getFind(value => !value.Method.IsGenericMethod)
                        .getArray(value => new TcpMethod
                        {
                            Method = value,
                            MethodType = Type,
                            ServiceAttribute = Attribute
                        }));
                    foreach (MemberIndexInfo member in MemberIndexGroup.Get<AutoCSer.Net.TcpStreamServer.MethodAttribute>(Type, Attribute.GetMemberFilters, false, Attribute.IsAttribute, Attribute.IsBaseTypeAttribute))
                    {
                        if (member.IsField)
                        {
                            FieldInfo field = (FieldInfo)member.Member;
                            TcpMethod getMethod = new TcpMethod
                            {
                                Method = new Metadata.MethodIndex(field, true),
                                MemberIndex = member,
                                MethodType = Type,
                                ServiceAttribute = Attribute
                            };
                            if (!getMethod.Attribute.IsOnlyGetMember)
                            {
                                getMethod.SetMethod = new TcpMethod { Method = new MethodIndex(field, false), MemberIndex = member, MethodType = Type, ServiceAttribute = Attribute };
                            }
                            methodArray.Add(getMethod);
                            if (getMethod.SetMethod != null) methodArray.Add(getMethod.SetMethod);
                        }
                        else if (member.CanGet)
                        {
                            PropertyInfo property = (PropertyInfo)member.Member;
                            TcpMethod getMethod = new TcpMethod
                            {
                                Method = new MethodIndex(property, true),
                                MemberIndex = member,
                                MethodType = Type,
                                ServiceAttribute = Attribute
                            };
                            if (member.CanSet && !getMethod.Attribute.IsOnlyGetMember)
                            {
                                getMethod.SetMethod = new TcpMethod { Method = new MethodIndex(property, false), MemberIndex = member, MethodType = Type, ServiceAttribute = Attribute };
                            }
                            methodArray.Add(getMethod);
                            if (getMethod.SetMethod != null) methodArray.Add(getMethod.SetMethod);
                        }
                    }
                    MethodIndexs = methodArray.ToArray();
                    MethodIndexs = TcpMethod.CheckIdentity(MethodIndexs, Attribute.CommandIdentityEnmuType, getRememberIdentityName(Attribute.CommandIdentityEnmuType == null ? Attribute.GenericType ?? Type : null), method => method.Method.MethodKeyFullName);
                    if (MethodIndexs == null) return;
                    int methodIndex = 0;
                    IsVerifyMethod = false;
                    ParameterBuilder parameterBuilder = new ParameterBuilder { IsSimpleSerialize = Attribute.IsSimpleSerialize };
                    foreach (TcpMethod method in MethodIndexs)
                    {
                        method.MethodIndex = methodIndex++;
                        if (!method.IsNullMethod)
                        {
                            if (IsVerifyMethod) method.Attribute.IsVerifyMethod = false;
                            else if (method.IsVerifyMethod) IsVerifyMethod = true;
                            parameterBuilder.Add(method);
                        }
                    }
                    ParameterTypes = parameterBuilder.Get();
                    //TcpMethod[] methodIndexs = MethodIndexs.getFindArray(value => !value.IsNullMethod);
                    if (ServiceAttribute.GetIsSegmentation)
                    {
                        IsClientCode = false;
                        create(IsServerCode = true);
                        CSharpTypeDefinition definition = new CSharpTypeDefinition(Type, IsClientCode = true, false, Type.Type.Namespace + ".TcpStreamClient");
                        _code_.Length = 0;
                        _code_.Add(definition.Start);
                        create(IsServerCode = false);
                        _code_.Add(definition.End);
                        string fileName = AutoParameter.ProjectPath + "{" + AutoParameter.DefaultNamespace + "}.TcpInternalStreamServer." + ServerName + ".Client.cs";
                        string clientCode = Coder.WarningCode + _code_.ToString() + Coder.FileEndCode;
                        if (Coder.WriteFile(fileName, clientCode))
                        {
                            if (ServiceAttribute.ClientSegmentationCopyPath != null)
                            {
                                string copyFileName = ServiceAttribute.ClientSegmentationCopyPath + "{" + AutoParameter.DefaultNamespace + "}.TcpInternalStreamServer." + ServerName + ".Client.cs";
                                if (Coder.WriteFile(copyFileName, clientCode)) Messages.Message(copyFileName + " 被修改");
                            }
                            Messages.Message(fileName + " 被修改");
                        }
                    }
                    else create(IsServerCode = IsClientCode = true);
                }
            }
            /// <summary>
            /// 安装完成处理
            /// </summary>
            protected override void onCreated() { }
        }
    }
}
