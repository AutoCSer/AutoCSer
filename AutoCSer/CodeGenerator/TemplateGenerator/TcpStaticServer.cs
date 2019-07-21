using System;
using AutoCSer.Metadata;
using AutoCSer.CodeGenerator.Metadata;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Reflection;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// TCP 静态服务代码生成
    /// </summary>
    internal sealed partial class TcpStaticServer : TcpServer
    {
        /// <summary>
        /// TCP 静态服务代码生成
        /// </summary>
#if DOTNET2 || NOJIT
        [Generator(Name = "TCP 静态服务", DependType = typeof(CSharper), IsAuto = true)]
#else
        [Generator(Name = "TCP 静态服务", DependType = typeof(SqlTable.Generator), IsAuto = true)]
#endif
        internal sealed partial class Generator : Generator<AutoCSer.Net.TcpStaticServer.ServerAttribute, AutoCSer.Net.TcpStaticServer.MethodAttribute, AutoCSer.Net.TcpInternalServer.ServerSocketSender>
        {
            /// <summary>
            /// 服务器端位置
            /// </summary>
            private const string serverPart = "TcpStaticServer";
            /// <summary>
            /// 客户端位置
            /// </summary>
            public string ClientPart
            {
                get { return "TcpStaticClient"; }
            }
            /// <summary>
            /// 调用参数位置
            /// </summary>
            public string ParameterPart
            {
                get { return ServiceAttribute.IsSegmentation ? ClientPart : serverPart; }
            }
            /// <summary>
            /// 生成部分
            /// </summary>
            public enum PartType
            {
                /// <summary>
                /// 服务端 / 客户端
                /// </summary>
                ServerType,
                /// <summary>
                /// 服务代理 / 客户端代理
                /// </summary>
                CallType,
                /// <summary>
                /// 远程调用链
                /// </summary>
                RemoteLink,
            }
            /// <summary>
            /// TCP 静态服务
            /// </summary>
            private sealed class Server
            {
                /// <summary>
                /// TCP调用服务配置
                /// </summary>
                public AutoCSer.Net.TcpStaticServer.ServerAttribute Attribute = new AutoCSer.Net.TcpStaticServer.ServerAttribute();
                /// <summary>
                /// 类型集合
                /// </summary>
                public LeftArray<ServerType> Types;
                /// <summary>
                /// 远程调用链类型集合
                /// </summary>
                public LeftArray<RemoteLinkType> RemoteLinkTypes;
                /// <summary>
                /// 其它组件添加客户端代码
                /// </summary>
                public LeftArray<string> ClientCodes;
                /// <summary>
                /// 配置类型
                /// </summary>
                public ExtensionType AttributeType;
                /// <summary>
                /// 是否存在方法
                /// </summary>
                public bool IsMethod;
                /// <summary>
                /// 是否默认时间验证服务
                /// </summary>
                public bool IsTimeVerify
                {
                    get { return typeof(AutoCSer.Net.TcpStaticServer.TimeVerify<>).isAssignableFromGenericDefinition(AttributeType); }
                }
            }
            /// <summary>
            /// TCP 静态服务类型信息
            /// </summary>
            private sealed class ServerType
            {
                /// <summary>
                /// TCP调用类型
                /// </summary>
                public ExtensionType Type;
                /// <summary>
                /// TCP调用配置
                /// </summary>
                public AutoCSer.Net.TcpStaticServer.ServerAttribute Attribute;
                /// <summary>
                /// 方法集合
                /// </summary>
                public ListArray<TcpMethod> Methods = new ListArray<TcpMethod>();
            }
            /// <summary>
            /// 远程调用链类型信息
            /// </summary>
            internal sealed class RemoteLinkType
            {
                /// <summary>
                /// TCP调用类型
                /// </summary>
                public ExtensionType Type;
                /// <summary>
                /// 第一个参数类型
                /// </summary>
                public Type ParameterType;
                /// <summary>
                /// 远程调用链根对象关键字
                /// </summary>
                public MemberIndex RemoteKeyMember;
                /// <summary>
                /// 获取远程调用链根对象的函数信息
                /// </summary>
                public MethodIndex RemoteKeyMethod;
                /// <summary>
                /// 获取远程调用链根对象的函数名称
                /// </summary>
                public string GetRemoteMethodName
                {
                    get { return RemoteKeyMethod.MethodName; }
                }
                /// <summary>
                /// 远程成员集合
                /// </summary>
                public RemoteMethod[] RemoteMethods;
                /// <summary>
                /// 远程成员集合
                /// </summary>
                public RemoteMember[] RemoteMembers;
                /// <summary>
                /// 远程缓存成员集合
                /// </summary>
                public RemoteLink[] RemoteLinks;
                /// <summary>
                /// 是否需要生成远程调用链
                /// </summary>
                public bool IsRemoteLink
                {
                    get { return (RemoteMethods.Length | RemoteMembers.Length | RemoteLinks.Length) != 0; }
                }
                /// <summary>
                /// 是否生成远程成员
                /// </summary>
                public bool IsRemoteMember;
            }
            /// <summary>
            /// 远程成员
            /// </summary>
            internal abstract class RemoteMemberBase
            {
                /// <summary>
                /// 远程成员配置
                /// </summary>
                public AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute Attribute;
                /// <summary>
                /// 是否生成函数
                /// </summary>
                public bool AttributeIsMethod
                {
                    get { return Attribute.IsMethod; }
                }
                /// <summary>
                /// 远程成员信息
                /// </summary>
                public MemberIndex Member;
                /// <summary>
                /// 获取函数信息
                /// </summary>
                public MethodIndex Method;
            }
            /// <summary>
            /// 远程成员
            /// </summary>
            internal sealed class RemoteMember : RemoteMemberBase
            {
                /// <summary>
                /// 获取数据函数名称
                /// </summary>
                public string GetMemberName
                {
                    get { return "get" + Member.MemberName; }
                }
                /// <summary>
                /// 获取数据函数名称
                /// </summary>
                public string GetTypeMemberName
                {
                    get { return GetMemberName; }
                }
                /// <summary>
                /// 远程函数名称
                /// </summary>
                public string RemoteMethodName
                {
                    get { return "remote_" + Method.MemberName; }
                }
                /// <summary>
                /// 远程函数名称
                /// </summary>
                public string RemoteTypeMethodName
                {
                    get { return RemoteMethodName; }
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
            internal class RemoteMethod : RemoteMemberBase
            {
                /// <summary>
                /// 当前类型
                /// </summary>
                public ExtensionType Type;
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
                    get
                    {
                        return Attribute.IsMethod || MethodParameters.Length > 1 || Method.ReturnType.Type == typeof(void);
                    }
                }
                /// <summary>
                /// 函数参数信息
                /// </summary>
                public SubArray<MethodParameter> NextParameters
                {
                    get { return new SubArray<MethodParameter>(MethodParameters, 1, MethodParameters.Length - 1); }
                }
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
                public string AttributeTypeMemberName
                {
                    get { return AttributeMemberName; }
                }
                /// <summary>
                /// 成员名称
                /// </summary>
                public string AttributeMethodName
                {
                    get
                    {
                        if (Attribute.IsAwait) return AttributeMemberName + "Awaiter";
                        return AttributeMemberName;
                    }
                }
                /// <summary>
                /// 成员名称
                /// </summary>
                public string AttributeTypeMethodName
                {
                    get { return AttributeMethodName; }
                }
                /// <summary>
                /// Awaiter
                /// </summary>
                public string Awaiter
                {
                    get
                    {
                        return Member == null && Attribute.IsAwait ? "Awaiter" : null;
                    }
                }
            }
            /// <summary>
            /// 远程缓存成员
            /// </summary>
            internal struct RemoteLinkMember
            {
                /// <summary>
                /// 远程成员缓存配置
                /// </summary>
                public AutoCSer.Net.TcpStaticServer.RemoteLinkAttribute Attribute;
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
                    get { return "_value" + Index.toString() +"_"; }
                }
                /// <summary>
                /// 局部变量名称
                /// </summary>
                public string ParentIndexName
                {
                    get { return Index == 0 ? "_value_" : ("_value" + (Index - 1).toString() + "_"); }
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
            /// 远程调用链成员
            /// </summary>
            internal sealed class RemoteLink : RemoteMemberBase
            {
                /// <summary>
                /// 前缀成员
                /// </summary>
                public RemoteLinkMember[] Members;
                /// <summary>
                /// 局部变量名称
                /// </summary>
                public string IndexName
                {
                    get { return "_value" + (Members.Length - 1).toString() + "_"; }
                }
                /// <summary>
                /// 属性参数集合
                /// </summary>
                public MethodParameter[] PropertyParameters;
                /// <summary>
                /// 获取远程成员名称
                /// </summary>
                public string GetLinkMemberName
                {
                    get
                    {
                        return "get_" + Members.joinString('_', value => value.Member.MemberName) + "_" + (Attribute.MemberName ?? Member.MemberName);
                    }
                }
                /// <summary>
                /// 获取远程成员名称
                /// </summary>
                public string GetLinkTypeMemberName
                {
                    get
                    {
                        return GetLinkMemberName;
                    }
                }
                /// <summary>
                /// 获取远程函数名称
                /// </summary>
                public string RemoteLinkMethodName
                {
                    get
                    {
                        return "remote_" + Members.joinString('_', value => value.Member.MemberName) + "_" + (Attribute.MemberName ?? Method.MethodName);
                    }
                }
                /// <summary>
                /// 获取远程函数名称
                /// </summary>
                public string RemoteLinkTypeMethodName
                {
                    get
                    {
                        return RemoteLinkMethodName;
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
                public string LinkMemberName
                {
                    get
                    {
                        string prefix = string.Empty;
                        switch (Attribute.NameType)
                        {
                            case AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute.Type.Concat: prefix = string.Concat(Members.getArray(value => value.Member.MemberName)); break;
                            case AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute.Type.Join: prefix = Members.joinString('_', value => value.Member.MemberName) + "_"; break;
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
            /// 远程调用链成员创建
            /// </summary>
            internal sealed class RemoteLinkBuilder
            {
                /// <summary>
                /// 远程缓存成员集合
                /// </summary>
                internal LeftArray<RemoteLink> Caches;
                /// <summary>
                /// 远程缓存成员集合
                /// </summary>
                private LeftArray<RemoteLinkMember> members;
                /// <summary>
                /// 添加远程缓存成员
                /// </summary>
                /// <param name="member"></param>
                internal void Push(MemberIndex member)
                {
                    members.Add(new RemoteLinkMember { Member = member, Attribute = member.GetSetupAttribute<AutoCSer.Net.TcpStaticServer.RemoteLinkAttribute>(false), Index = members.Length });
                    cache cache = getCache(member.MemberSystemType);
                    if (cache.Members.Length != 0)
                    {
                        RemoteLinkMember[] memberArray = members.ToArray();
                        foreach (RemoteLink nextMember in cache.Members)
                        {
                            Caches.Add(new RemoteLink { Members = memberArray, Method = nextMember.Method, Member = nextMember.Member, PropertyParameters = nextMember.PropertyParameters, Attribute = nextMember.Attribute });
                        }
                    }
                    foreach (MemberIndex nextMember in cache.Caches)
                    {
                        bool isMember = true;
                        foreach (RemoteLinkMember cacheMember in members)
                        {
                            if (cacheMember.Member.MemberSystemType == nextMember.MemberSystemType)
                            {
                                Messages.Message("远程调用链类型循环引用 " + members.JoinString(" . ", value => value.Member.MemberType.FullName + " " + value.Member.MemberName) + " . " + nextMember.MemberType.FullName + " " + nextMember.MemberName);
                                isMember = false;
                                break;
                            }
                        }
                        if (isMember) Push(nextMember);
                    }
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
                    public RemoteLink[] Members;
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
                        LeftArray<RemoteLink> remoteMembers = new LeftArray<RemoteLink>();
                        foreach (MemberIndex member in MemberIndex.GetMembers<AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute>(type, AutoCSer.Metadata.MemberFilters.Instance, true, false))
                        {
                            if (member.CanGet)
                            {
                                AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute remoteAttribute = member.GetSetupAttribute<AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute>(false);
                                if (remoteAttribute != null)
                                {
                                    remoteMembers.Add(new RemoteLink { Member = member, PropertyParameters = member.IsField ? NullValue<MethodParameter>.Array : MethodParameter.Get(((PropertyInfo)member.Member).GetGetMethod(true), NullValue<Type>.Array), Attribute = remoteAttribute });
                                }
                            }
                        }
                        foreach (MethodIndex member in MethodIndex.GetMethods<AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute>(type, AutoCSer.Metadata.MemberFilters.Instance, false, true, false))
                        {
                            if (!member.Method.IsGenericMethodDefinition)
                            {
                                AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute remoteAttribute = member.GetSetupAttribute<AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute>(false);
                                if (remoteAttribute != null) remoteMembers.Add(new RemoteLink { Method = member, Attribute = remoteAttribute });
                            }
                        }
                        memberCache.Add(type, members = new cache { Members = remoteMembers.ToArray(), Caches = MemberIndex.GetMembers<AutoCSer.Net.TcpStaticServer.RemoteLinkAttribute>(type, AutoCSer.Metadata.MemberFilters.Instance, true, false) });
                    }
                    return members;
                }
            }
            /// <summary>
            /// 生成部分
            /// </summary>
            public PartType Part;
            /// <summary>
            /// 配置类型
            /// </summary>
            public string TcpServerAttributeType;
            /// <summary>
            /// TCP 静态服务集合
            /// </summary>
            private Dictionary<HashString, Server> servers = DictionaryCreator.CreateHashString<Server>();
            /// <summary>
            /// TCP 静态服务类型集合
            /// </summary>
            private ReusableDictionary<HashString, ServerType> serverTypes = ReusableDictionary.CreateHashString<ServerType>();
            /// <summary>
            /// 服务类名称(临时变量)
            /// </summary>
            private string defaultServerName;
            /// <summary>
            /// TCP调用服务(临时变量)
            /// </summary>
            private Server defaultServer;
            /// <summary>
            /// TCP调用类型(临时变量)
            /// </summary>
            private ServerType defaultType;
            /// <summary>
            /// 时间验证函数
            /// </summary>
            private TcpMethod TimeVerifyMethod;
            /// <summary>
            /// 当前远程调用链类型信息
            /// </summary>
            public RemoteLinkType CurrentRemoteLinkType;
            /// <summary>
            /// 客户端时间验证类型
            /// </summary>
            public string TimeVerifyClientType
            {
                get
                {
                    return GetTcpCallTypeName(AutoParameter, Type);
                }
            }
            /// <summary>
            /// 获取 TcpCall 调用类型名称
            /// </summary>
            /// <param name="namespaceString"></param>
            /// <param name="type"></param>
            /// <returns></returns>
            public static string GetTcpCallTypeName(ProjectParameter parameter, ExtensionType type)
            {
                return parameter.DefaultNamespace + ".TcpCall." + type.TypeName;
            }
            /// <summary>
            /// 获取远程调用链类型信息
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            internal static RemoteLinkType GetRemoteLinkType(Type type)
            {
                foreach (MemberIndex member in MemberIndex.GetMembers<AutoCSer.Net.TcpStaticServer.RemoteKeyAttribute>(type, MemberFilters.PublicInstance, true, false))
                {
                    foreach (MethodIndex method in MethodIndex.GetMethods<AutoCSer.Net.TcpStaticServer.RemoteKeyAttribute>(type, MemberFilters.Static, false, true, false))
                    {
                        if (!method.Method.IsGenericMethodDefinition && type.IsAssignableFrom(method.Method.ReturnType))
                        {
                            ParameterInfo[] parameters = method.Method.GetParameters();
                            if (parameters.Length == 1 && parameters[0].ParameterType == member.MemberSystemType)
                            {
                                return new RemoteLinkType { Type = type, RemoteKeyMember = member, RemoteKeyMethod = method, ParameterType = member.MemberSystemType };
                            }
                        }
                    }
                    break;
                }
                return null;
            }
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void nextCreate()
            {
                if ((Attribute.IsAbstract || Type.Type.IsSealed || !Type.Type.IsAbstract) && !Type.Type.IsInterface)
                {
                    defaultServerName = Attribute.ServerName;
                    defaultServer = null;
                    defaultType = null;
                    if (defaultServerName != null)
                    {
                        HashString nameKey = defaultServerName;
                        if (!servers.TryGetValue(nameKey, out defaultServer)) servers.Add(nameKey, defaultServer = new Server());
                        defaultServer.Attribute.Name = defaultServerName;
                        defaultServer.Types.Add(defaultType = new ServerType { Type = Type, Attribute = Attribute });
                        if (Attribute.IsServer)
                        {
                            defaultServer.AttributeType = Type;
                            defaultServer.Attribute.CopyFrom(Attribute);
                        }
                    }
                    RemoteLinkType remoteLinkType = null;
                    if (Attribute.ServerName != null)
                    {
                        if (Attribute.IsRemoteLinkType) remoteLinkType = new RemoteLinkType { Type = Type, ParameterType = Type };
                        else if (Attribute.IsRemoteLink) remoteLinkType = GetRemoteLinkType(Type);
                    }
                    LeftArray<RemoteMethod> remoteMethods = new LeftArray<RemoteMethod>();
                    foreach (MethodIndex method in MethodIndex.GetMethods<AutoCSer.Net.TcpStaticServer.MethodAttribute>(Type, Attribute.MemberFilters, false, Attribute.IsAttribute, Attribute.IsBaseTypeAttribute))
                    {
                        next(new TcpMethod { Method = method, MethodType = Type });
                        if (remoteLinkType != null && !method.Method.IsGenericMethodDefinition)
                        {
                            AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute remoteAttribute = method.GetSetupAttribute<AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute>(false);
                            if (remoteAttribute != null && remoteAttribute.IsClientRemoteMember)
                            {
                                MethodParameter[] parameters = method.Parameters;
                                if (parameters.Length >= 1 && parameters[0].ParameterType.Type == remoteLinkType.ParameterType) remoteMethods.Add(new RemoteMethod { Type = Type, Method = method, MethodParameters = parameters, Attribute = remoteAttribute });
                            }
                        }
                    }
                    if (remoteLinkType != null)
                    {
                        remoteLinkType.RemoteMethods = remoteMethods.ToArray();

                        LeftArray<RemoteMember> remoteMembers = new LeftArray<RemoteMember>();
                        foreach (MemberIndex member in MemberIndex.GetMembers<AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute>(Type, AutoCSer.Metadata.MemberFilters.NonPublicInstanceProperty, true, false))
                        {
                            if (member.CanGet)
                            {
                                AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute remoteAttribute = member.GetSetupAttribute<AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute>(false);
                                if (remoteAttribute != null)
                                {
                                    if (member.IsField || ((PropertyInfo)member.Member).GetIndexParameters().Length == 0) remoteMembers.Add(new RemoteMember { Member = member, Attribute = remoteAttribute });
                                }
                            }
                        }
                        foreach (MethodIndex member in MethodIndex.GetMethods<AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute>(Type, AutoCSer.Metadata.MemberFilters.NonPublicInstance, false, true, false))
                        {
                            if (!member.Method.IsGenericMethodDefinition)
                            {
                                AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute remoteAttribute = member.GetSetupAttribute<AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute>(false);
                                if (remoteAttribute != null) remoteMembers.Add(new RemoteMember { Method = member, Attribute = remoteAttribute });
                            }
                        }
                        remoteLinkType.RemoteMembers = remoteMembers.ToArray();

                        RemoteLinkBuilder cacheBuilder = new RemoteLinkBuilder();
                        foreach (MemberIndex member in MemberIndex.GetMembers<AutoCSer.Net.TcpStaticServer.RemoteLinkAttribute>(Type, AutoCSer.Metadata.MemberFilters.NonPublicInstance, true, false))
                        {
                            cacheBuilder.Push(member);
                        }
                        remoteLinkType.RemoteLinks = cacheBuilder.Caches.ToArray();

                        if (remoteLinkType.IsRemoteLink) defaultServer.RemoteLinkTypes.Add(remoteLinkType);
                    }
                    if (!Type.Type.IsGenericType)
                    {
                        foreach (MemberIndexInfo member in StaticMemberIndexGroup.Get<AutoCSer.Net.TcpStaticServer.MethodAttribute>(Type, Attribute.MemberFilters, false, Attribute.IsAttribute, Attribute.IsBaseTypeAttribute))
                        {
                            if (member.IsField)
                            {
                                FieldInfo field = (FieldInfo)member.Member;
                                TcpMethod getMethod = new TcpMethod
                                {
                                    Method = new Metadata.MethodIndex(field, true),
                                    MemberIndex = member,
                                    MethodType = Type
                                };
                                if (!getMethod.Attribute.IsOnlyGetMember)
                                {
                                    getMethod.SetMethod = new TcpMethod { Method = new Metadata.MethodIndex(field, false), MemberIndex = member, MethodType = Type };
                                }
                                next(getMethod);
                                if (getMethod.SetMethod != null) next(getMethod.SetMethod);
                            }
                            else if (member.CanGet)
                            {
                                PropertyInfo property = (PropertyInfo)member.Member;
                                TcpMethod getMethod = new TcpMethod
                                {
                                    Method = new Metadata.MethodIndex(property, true),
                                    MemberIndex = member,
                                    MethodType = Type
                                };
                                if (member.CanSet && !getMethod.Attribute.IsOnlyGetMember)
                                {
                                    getMethod.SetMethod = new TcpMethod { Method = new Metadata.MethodIndex(property, false), MemberIndex = member, MethodType = Type };
                                }
                                next(getMethod);
                                if (getMethod.SetMethod != null) next(getMethod.SetMethod);
                            }
                        }
                    }
                    serverTypes.Empty();
                }
            }
            /// <summary>
            /// 下一个函数处理
            /// </summary>
            /// <param name="methodIndex"></param>
            private void next(TcpMethod methodIndex)
            {
                AutoCSer.Net.TcpStaticServer.MethodAttribute attribute = methodIndex.Attribute;
                Server server = defaultServer;
                ServerType serverType = defaultType;
                string serviceName = attribute.GetServerName;
                if (serviceName == null) serviceName = Attribute.ServerName;
                if (serviceName != defaultServerName)
                {
                    if (serviceName == null) serverType = null;
                    else
                    {
                        HashString nameKey = serviceName;
                        if (!servers.TryGetValue(nameKey, out server))
                        {
                            servers.Add(nameKey, server = new Server());
                            server.Attribute.Name = serviceName;
                        }
                        if (!serverTypes.TryGetValue(ref nameKey, out serverType))
                        {
                            server.Types.Add(serverType = new ServerType { Type = Type });
                            serverTypes.Set(ref nameKey, serverType);
                        }
                    }
                }
                if (serverType != null)
                {
                    server.IsMethod = true;
                    methodIndex.ServiceAttribute = server.Attribute;
                    //methodIndex.MethodIndex = server.MethodIndex++;
                    //methodIndex.ParameterIndex = parameterIndex++;
                    serverType.Methods.Add(methodIndex);
                }
            }
            /// <summary>
            /// 获取命令序号记忆数据
            /// </summary>
            /// <returns></returns>
            private Dictionary<HashString, int> getRememberIdentityName()
            {
                string serverTypeName = AutoParameter.DefaultNamespace + "." + serverPart + "." + ServerName;
                Type serverType = assembly.GetType(serverTypeName);
                return serverType == null ? nullRememberIdentityName : getRememberIdentityName(serverType);
            }
            /// <summary>
            /// 安装完成处理
            /// </summary>
            protected unsafe override void onCreated()
            {
                foreach (ClientCode code in clientCodes.Values)
                {
                    Server server;
                    if (servers.TryGetValue(code.Attribute.ServerName, out server))
                    {
                        if (server.Attribute.IsSegmentation) server.ClientCodes.Add(code.SegmentationCode);
                        else Coder.Add(code.Code);
                    }
                }
                if (clientCodes.Count != 0) clientCodes = DictionaryCreator.CreateOnly<Type, ClientCode>();

                StringArray clientCallCode = new StringArray();
                LeftArray<TcpMethod> methods = new LeftArray<TcpMethod>();
                TcpMethod[] methodIndexs;
                ParameterBuilder parameterBuilder = new ParameterBuilder();
                int staticMethodIndex = 0;
                foreach (Server server in servers.Values)
                {
                    if (server.IsMethod || server.RemoteLinkTypes.Length != 0 || server.ClientCodes.Length != 0)
                    {
                        ServiceAttribute = server.Attribute;
                        TcpServerAttributeType = server.AttributeType == null || server.AttributeType.Type == null ? null : server.AttributeType.FullName;

                        Part = PartType.RemoteLink;
                        foreach (RemoteLinkType remoteLinkType in server.RemoteLinkTypes)
                        {
                            if (remoteLinkType.IsRemoteLink)
                            {
                                Type = remoteLinkType.Type;
                                CurrentRemoteLinkType = remoteLinkType;
                                remoteLinkType.IsRemoteMember = remoteLinkType.RemoteMethods.Length != 0 && RemoteMemberTypes.Add(Type);
                                CSharpTypeDefinition definition = new CSharpTypeDefinition(Type, true, false);
                                _code_.Length = 0;
                                create(false);
                                Coder.Add(definition.Start + _partCodes_["SERVERREMOTE"] + definition.End);
                                if (ServiceAttribute.IsSegmentation)
                                {
                                    clientCallCode.Add(definition.Start + _partCodes_["CLIENTREMOTE"] + definition.End);
                                }
                                else Coder.Add(definition.Start + _partCodes_["CLIENTREMOTE"] + definition.End);
                            }
                        }
                        string clientCode = null;
                        if (server.IsMethod)
                        {
                            Part = PartType.CallType;
                            foreach (ServerType serverType in server.Types) methods.Add(serverType.Methods);
                            methodIndexs = methods.ToArray();
                            methods.Length = 0;
                            methodIndexs = TcpMethod.CheckIdentity(methodIndexs, getRememberIdentityName(), method => method.Method.MethodKeyFullName);
                            if (methodIndexs == null) return;
                            int index = 0;
                            IsVerifyMethod = IsCallQueue = false;
                            parameterBuilder.Clear(ServiceAttribute.IsSimpleSerialize);
                            foreach (TcpMethod method in methodIndexs)
                            {
                                method.MethodIndex = index++;
                                method.StaticMethodIndex = ++staticMethodIndex;
                                if (!method.IsNullMethod)
                                {
                                    if (IsVerifyMethod) method.Attribute.IsVerifyMethod = false;
                                    else if (method.IsVerifyMethod)
                                    {
                                        IsVerifyMethod = true;
                                        if (method.MethodType == server.AttributeType && server.IsTimeVerify) TimeVerifyMethod = method;
                                        method.Attribute.ServerTaskType = Net.TcpServer.ServerTaskType.Synchronous;
                                    }
                                    parameterBuilder.Add(method);

                                    IsCallQueue |= method.Attribute.ServerTaskType == Net.TcpServer.ServerTaskType.Queue;
                                }
                            }
                            ParameterTypes = parameterBuilder.Get();
                            foreach (ServerType serverType in server.Types)
                            {
                                if (serverType.Methods.Length != 0)
                                {
                                    Type = serverType.Type;
                                    //TimeVerifyType = Type == server.AttributeType && server.IsTimeVerify ? Type : ExtensionType.Null;
                                    Attribute = serverType.Attribute ?? server.Attribute;
                                    MethodIndexs = serverType.Methods.ToArray();
                                    CSharpTypeDefinition definition = new CSharpTypeDefinition(Type, true, false);
                                    _code_.Length = 0;
                                    create(false);
                                    Coder.Add(definition.Start + _partCodes_["SERVERCALL"] + definition.End);
                                    if (ServiceAttribute.IsSegmentation)
                                    {
                                        clientCallCode.Add(definition.Start + _partCodes_["CLIENTCALL"] + definition.End);
                                    }
                                    else Coder.Add(definition.Start + _partCodes_["CLIENTCALL"] + definition.End);
                                }
                            }
                            
                            Part = PartType.ServerType;
                            Type = server.AttributeType;
                            Attribute = server.Attribute;
                            MethodIndexs = methodIndexs;
                            methods.Length = 0;
                            _code_.Length = 0;
                            create(false);
                            Coder.Add(@"
namespace " + AutoParameter.DefaultNamespace + "." + serverPart + @"
{
" + _partCodes_["SERVER"] + @"
}");
                            clientCode = @"
namespace " + AutoParameter.DefaultNamespace + "." + ClientPart + @"
{
" + _partCodes_["CLIENT"] + @"
}";
                        }
                        if (ServiceAttribute.IsSegmentation)
                        {
                            if (clientCode != null) clientCallCode.Add(clientCode);
                            clientCallCode.Append(ref server.ClientCodes);
                            string fileName = AutoParameter.ProjectPath + "{" + AutoParameter.DefaultNamespace + "}.TcpStaticServer." + ServiceAttribute.ServerName + ".Client.cs";
                            clientCode = Coder.WarningCode + clientCallCode.ToString() + Coder.FileEndCode;
                            if (Coder.WriteFile(fileName, clientCode))
                            {
                                if (ServiceAttribute.ClientSegmentationCopyPath != null)
                                {
                                    string copyFileName = ServiceAttribute.ClientSegmentationCopyPath + "{" + AutoParameter.DefaultNamespace + "}.TcpStaticServer." + ServiceAttribute.ServerName + ".Client.cs";
                                    if (Coder.WriteFile(copyFileName, clientCode)) Messages.Message(copyFileName + " 被修改");
                                }
                                Messages.Message(fileName + " 被修改");
                            }
                            clientCallCode.Length = 0;
                        }
                        else if(clientCode != null) Coder.Add(clientCode);
                    }
                }
            }
            /// <summary>
            /// 其它组件产生的客户端代码
            /// </summary>
            private static Dictionary<Type, ClientCode> clientCodes = DictionaryCreator.CreateOnly<Type, ClientCode>();
            /// <summary>
            /// 其它组件产生的客户端代码
            /// </summary>
            internal sealed class ClientCode
            {
                /// <summary>
                /// 类型
                /// </summary>
                public Type Type;
                /// <summary>
                /// TCP 静态服务配置
                /// </summary>
                public AutoCSer.Net.TcpStaticServer.ServerAttribute Attribute;
                /// <summary>
                /// 客户端代码
                /// </summary>
                public string Code;
                /// <summary>
                /// 客户端代码
                /// </summary>
                public string SegmentationCode;
            }
            /// <summary>
            /// 其它组件添加客户端代码
            /// </summary>
            /// <param name="code"></param>
            internal static void AddClientCode(ClientCode code)
            {
                ClientCode cache;
                if (clientCodes.TryGetValue(code.Type, out cache))
                {
                    cache.Code += code.Code;
                    cache.SegmentationCode += code.SegmentationCode;
                }
                else clientCodes.Add(code.Type, code);
            }
            /// <summary>
            /// 生成远程成员的类型集合
            /// </summary>
            internal static readonly HashSet<Type> RemoteMemberTypes = HashSetCreator.CreateOnly<Type>();
        }
    }
}
