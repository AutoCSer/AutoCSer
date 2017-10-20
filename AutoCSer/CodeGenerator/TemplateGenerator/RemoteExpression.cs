using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using AutoCSer.CodeGenerator.Metadata;
using AutoCSer.Extension;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// 远程表达式 代码生成
    /// </summary>
    internal abstract partial class RemoteExpression
    {
        /// <summary>
        /// 远程表达式 代码生成
        /// </summary>
        [Generator(Name = "远程表达式", DependType = typeof(CSharper), IsAuto = true)]
        internal partial class Generator : Generator<AutoCSer.Net.RemoteExpression.TypeAttribute>
        {
            /// <summary>
            /// 远程表达式参数
            /// </summary>
            [StructLayout(LayoutKind.Auto)]
            internal struct ExpressionParameter
            {
                /// <summary>
                /// 输入参数集合
                /// </summary>
                public MethodParameter Parameter;
                /// <summary>
                /// 参数类型
                /// </summary>
                public ExtensionType ParameterType
                {
                    get { return Parameter.ParameterType; }
                }
                /// <summary>
                /// 参数名称
                /// </summary>
                public string ParameterName
                {
                    get { return Parameter.ParameterName; }
                }
                /// <summary>
                /// 参数连接名称，最后一个参数不带逗号
                /// </summary>
                public string ParameterJoinName
                {
                    get { return Parameter.ParameterJoinName; }
                }
                /// <summary>
                /// 是否远程表达式参数类型
                /// </summary>
                public bool IsClientNodeParameter
                {
                    get
                    {
                        Type type = ParameterType.Type;
                        return type.IsValueType && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(AutoCSer.Net.RemoteExpression.ClientNode<>);
                    }
                }
                /// <summary>
                /// 参数类型
                /// </summary>
                public ExtensionType RemoteExpressionParameterType
                {
                    get
                    {
                        if (IsClientNodeParameter) return typeof(AutoCSer.Net.RemoteExpression.Node<>).MakeGenericType(ParameterType.Type.GetGenericArguments());
                        return ParameterType;
                    }
                }
                /// <summary>
                /// 获取远程表达式参数
                /// </summary>
                /// <param name="parameters"></param>
                /// <returns></returns>
                internal static ExpressionParameter[] Get(MethodParameter[] parameters)
                {
                    return parameters.getArray(parameter => new ExpressionParameter { Parameter = parameter });
                }
            }
            /// <summary>
            /// 远程表达式成员
            /// </summary>
            internal abstract class Expression
            {
                /// <summary>
                /// 远程表达式成员配置
                /// </summary>
                public AutoCSer.Net.RemoteExpression.MemberAttribute Attribute;
                /// <summary>
                /// 成员信息
                /// </summary>
                public MemberIndex Member;
                /// <summary>
                /// 成员方法
                /// </summary>
                public MethodIndex Method;
                /// <summary>
                /// 远程表达式成员节点类型名称
                /// </summary>
                public string MemberNodeTypeName
                {
                    get
                    {
                        string name = StaticPropertyName + AutoCSer.Net.RemoteExpression.Node.RemoteExpressionTypeName;
                        if (Attribute.MemberIdentity != 0) name += Attribute.MemberIdentity.toString();
                        return name;
                    }
                }
                /// <summary>
                /// 远程表达式成员节点类型名称
                /// </summary>
                public string GenericMemberNodeTypeName
                {
                    get
                    {
                        string name = MemberNodeTypeName;
                        if (Method != null) name += Method.GenericParameterName;
                        return name;
                    }
                }
                /// <summary>
                /// 返回值类型
                /// </summary>
                public ExtensionType MethodReturnType
                {
                    get { return Method == null ? Member.MemberType : Method.ReturnType; }
                }
                /// <summary>
                /// XML 文档注释
                /// </summary>
                public string XmlDocument
                {
                    get { return Method == null ? Member.XmlDocument : Method.XmlDocument; }
                }
                /// <summary>
                /// 是否存在返回值
                /// </summary>
                public bool MemberIsReturn
                {
                    get { return Attribute.IsReturn && MethodReturnType.Type != typeof(void); }
                }
                /// <summary>
                /// 远程表达式成员节点继承类型
                /// </summary>
                public string MemberRemoteExpressionTypeName
                {
                    get
                    {
                        if (MemberIsReturn)
                        {
                            if (MethodReturnType.Type.IsGenericParameter) return typeof(AutoCSer.Net.RemoteExpression.GenericNode<>).MakeGenericType(MethodReturnType).fullName();
                            Type nodeType = MethodReturnType.Type.GetNestedType(AutoCSer.Net.RemoteExpression.Node.RemoteExpressionTypeName, BindingFlags.Public);
                            if (nodeType == null) return typeof(AutoCSer.Net.RemoteExpression.Node<>).MakeGenericType(MethodReturnType).fullName();
                            if (nodeType.IsGenericTypeDefinition) nodeType = nodeType.MakeGenericType(MethodReturnType.Type.GetGenericArguments());
                            return nodeType.fullName();
                        }
                        return (MethodReturnType.Type.IsGenericParameter ? typeof(AutoCSer.Net.RemoteExpression.GenericNode) : typeof(AutoCSer.Net.RemoteExpression.Node)).fullName();
                    }
                }
                /// <summary>
                /// 输入参数集合
                /// </summary>
                public ExpressionParameter[] IntputParameters;
                /// <summary>
                /// 是否存在表达式节点参数
                /// </summary>
                public bool IsClientNodeParameter
                {
                    get
                    {
                        foreach (ExpressionParameter parameter in IntputParameters)
                        {
                            if (parameter.IsClientNodeParameter) return true;
                        }
                        return false;
                    }
                }
                /// <summary>
                /// 泛型参数集合
                /// </summary>
                public ExtensionType[] GenericParameters
                {
                    get
                    {
                        return Method == null ? NullValue<ExtensionType>.Array : Method.GenericParameters;
                    }
                }
                /// <summary>
                /// 静态属性名称
                /// </summary>
                public string StaticPropertyName
                {
                    get { return Method == null ? Member.MemberName : Method.MethodName; }
                }
                /// <summary>
                /// 是否静态成员
                /// </summary>
                public virtual bool IsStatic
                {
                    get { return false; }
                }
            }
            /// <summary>
            /// 远程表达式成员
            /// </summary>
            internal class MemberExpression : Expression
            {
            }
            /// <summary>
            /// 远程表达式成员
            /// </summary>
            internal sealed class StaticMemberExpression : MemberExpression
            {
                /// <summary>
                /// 是否静态成员
                /// </summary>
                public override bool IsStatic
                {
                    get { return true; }
                }
            }
            /// <summary>
            /// 远程表达式成员
            /// </summary>
            internal class MethodExpression : Expression
            {
            }
            /// <summary>
            /// 远程表达式成员
            /// </summary>
            internal sealed class StaticMethodExpression : MethodExpression
            {
                /// <summary>
                /// 是否静态成员
                /// </summary>
                public override bool IsStatic
                {
                    get { return true; }
                }
            }
            /// <summary>
            /// 远程表达式容器类型名称
            /// </summary>
            internal string RemoteExpressionTypeName
            {
                get { return AutoCSer.Net.RemoteExpression.Node.RemoteExpressionTypeName; }
            }
            /// <summary>
            /// 实例字段 / 属性
            /// </summary>
            internal MemberExpression[] Members;
            /// <summary>
            /// 实例方法
            /// </summary>
            internal MethodExpression[] Methods;
            /// <summary>
            /// 静态字段 / 属性
            /// </summary>
            internal StaticMemberExpression[] StaticMembers;
            /// <summary>
            /// 静态方法
            /// </summary>
            internal StaticMethodExpression[] StaticMethods;
            /// <summary>
            /// 静态成员数量
            /// </summary>
            internal int StaticMemberCount
            {
                get { return StaticMembers.Length + StaticMethods.Length; }
            }
            /// <summary>
            /// 节点名称集合
            /// </summary>
            private HashSet<string> nodeTypeNames = HashSetCreator.CreateOnly<string>();

            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void nextCreate()
            {
                if (!Type.Type.IsInterface)
                {
                    LeftArray<MemberExpression> members = default(LeftArray<MemberExpression>);
                    nodeTypeNames.Clear();
                    bool isMember = true;
                    foreach (MemberIndex member in MemberIndex.GetMembers<AutoCSer.Net.RemoteExpression.MemberAttribute>(Type, AutoCSer.Metadata.MemberFilters.Instance, true, false))
                    {
                        if (!member.IsIgnore && member.CanGet)
                        {
                            AutoCSer.Net.RemoteExpression.MemberAttribute attribute = member.GetAttribute<AutoCSer.Net.RemoteExpression.MemberAttribute>(false);
                            isMember &= addMember(ref members, new MemberExpression { Attribute = attribute, Member = member, IntputParameters = member.IsField ? NullValue<ExpressionParameter>.Array : ExpressionParameter.Get(MethodParameter.Get(((PropertyInfo)member.Member).GetGetMethod(true), NullValue<Type>.Array)) });
                        }
                    }
                    Members = members.ToArray();
                    LeftArray<MethodExpression> methods = default(LeftArray<MethodExpression>);
                    foreach (MethodIndex member in MethodIndex.GetMethods<AutoCSer.Net.RemoteExpression.MemberAttribute>(Type, AutoCSer.Metadata.MemberFilters.Instance, false, true, false))
                    {
                        if (!member.IsIgnore)
                        {
                            AutoCSer.Net.RemoteExpression.MemberAttribute attribute = member.GetAttribute<AutoCSer.Net.RemoteExpression.MemberAttribute>(false);
                            isMember &= addMember(ref methods, new MethodExpression { Attribute = attribute, Method = member, IntputParameters = ExpressionParameter.Get(member.Parameters) });
                        }
                    }
                    Methods = methods.ToArray();
                    LeftArray<StaticMemberExpression> staticMembers = default(LeftArray<StaticMemberExpression>);
                    foreach (MemberIndex member in MemberIndex.GetStaticMembers<AutoCSer.Net.RemoteExpression.MemberAttribute>(Type, AutoCSer.Metadata.MemberFilters.Static, true, false))
                    {
                        if (!member.IsIgnore && member.CanGet)
                        {
                            AutoCSer.Net.RemoteExpression.MemberAttribute attribute = member.GetAttribute<AutoCSer.Net.RemoteExpression.MemberAttribute>(false);
                            isMember &= addMember(ref staticMembers, new StaticMemberExpression { Attribute = attribute, Member = member, IntputParameters = NullValue<ExpressionParameter>.Array });
                        }
                    }
                    StaticMembers = staticMembers.ToArray();
                    LeftArray<StaticMethodExpression> staticMethods = default(LeftArray<StaticMethodExpression>);
                    foreach (MethodIndex member in MethodIndex.GetMethods<AutoCSer.Net.RemoteExpression.MemberAttribute>(Type, AutoCSer.Metadata.MemberFilters.Static, false, true, false))
                    {
                        if (!member.IsIgnore)
                        {
                            AutoCSer.Net.RemoteExpression.MemberAttribute attribute = member.GetAttribute<AutoCSer.Net.RemoteExpression.MemberAttribute>(false);
                            isMember &= addMember(ref staticMethods, new StaticMethodExpression { Attribute = attribute, Method = member, IntputParameters = ExpressionParameter.Get(member.Parameters) });
                        }
                    }
                    StaticMethods = staticMethods.ToArray();
                    if (isMember && (Members.Length | Methods.Length | StaticMemberCount) != 0) create(true);
                }
            }
            /// <summary>
            /// 添加成员
            /// </summary>
            /// <typeparam name="expressionType"></typeparam>
            /// <param name="members"></param>
            /// <param name="member"></param>
            /// <returns></returns>
            private bool addMember<expressionType>(ref LeftArray<expressionType> members, expressionType member)
                where expressionType : Expression
            {
                string name = member.MemberNodeTypeName;
                if (!nodeTypeNames.Add(name))
                {
                    Messages.Add(Type.FullName + " 远程表达式节点类型名称冲突 " + name);
                    return false;
                }
                members.Add(member);
                return true;
            }
            /// <summary>
            /// 安装完成处理
            /// </summary>
            protected override void onCreated() { }
        }
    }
}
