using System;
using AutoCSer.Extension;
using AutoCSer.CodeGenerator.Metadata;
using System.Collections.Generic;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// web调用代码生成
    /// </summary>
    internal sealed partial class WebCall
    {
        /// <summary>
        /// 默认空WEB调用配置
        /// </summary>
        internal static readonly AutoCSer.WebView.CallMethodAttribute Null = new AutoCSer.WebView.CallMethodAttribute();
        /// <summary>
        /// web调用代码生成
        /// </summary>
        [Generator(Name = "WEB 调用", DependType = typeof(Html), IsAuto = true)]
        internal sealed partial class Generator : WebView.Generator<AutoCSer.WebView.CallAttribute>
        {
            /// <summary>
            /// WEB 调用函数集合
            /// </summary>
            public static CallMethod[] CallMethods = NullValue<CallMethod>.Array;
            /// <summary>
            /// 方法索引信息
            /// </summary>
            public sealed class CallMethod : AsynchronousMethod
            {
                /// <summary>
                /// 获取改方法的类型
                /// </summary>
                public ExtensionType WebCallMethodType;
                /// <summary>
                /// 获取改方法的类型
                /// </summary>
                public ExtensionType WebCallAsynchronousMethodType
                {
                    get { return WebCallMethodType; }
                }
                /// <summary>
                /// 方法索引
                /// </summary>
                public int MethodIndex;
                /// <summary>
                /// 类型WEB调用配置
                /// </summary>
                public AutoCSer.WebView.CallAttribute TypeAttribute;
                /// <summary>
                /// 类型调用名称
                /// </summary>
                public string TypeCallName;
                /// <summary>
                /// WEB调用配置
                /// </summary>
                private AutoCSer.WebView.CallMethodAttribute attribute;
                /// <summary>
                /// WEB调用配置
                /// </summary>
                public AutoCSer.WebView.CallMethodAttribute Attribute
                {
                    get
                    {
                        if (attribute == null)
                        {
                            attribute = (MemberIndex ?? Method).GetSetupAttribute<AutoCSer.WebView.CallMethodAttribute>(false);
                            if (attribute == null) attribute = Null;
                        }
                        return attribute;
                    }
                }
                /// <summary>
                /// 内存流最大字节数
                /// </summary>
                public int MaxMemoryStreamSize
                {
                    get
                    {
                        return (int)Attribute.MaxMemoryStreamSize;
                    }
                }
                /// <summary>
                /// 是否忽略大小写
                /// </summary>
                public bool IgnoreCase;
                /// <summary>
                /// 是否使用对象池
                /// </summary>
                public bool IsPoolType;
                /// <summary>
                /// 是否异步调用
                /// </summary>
                public bool IsAsynchronous;
                /// <summary>
                /// 调用名称
                /// </summary>
                private string callName;
                /// <summary>
                /// 调用名称
                /// </summary>
                public string CallName
                {
                    get
                    {
                        if (callName == null)
                        {
                            if (Attribute.FullName != null)
                            {
                                string name = attribute.FullName;
                                if (name.Length == 0) callName = "/";
                                else if (name[0] == '/') callName = IgnoreCase ? name.ToLower() : name;
                                else
                                {
                                    callName = "/" + name;
                                    if (IgnoreCase) callName = callName.toLowerNotEmpty();
                                }
                            }
                            else
                            {
                                string name = attribute.MethodName ?? (MemberIndex == null ? Method.MethodName : MemberIndex.Member.Name);
                                callName = "/" + (name.Length != 0 ? TypeCallName.Replace('.', '/') + "/" + name : TypeCallName.Replace('.', '/'));
                                if (IgnoreCase) callName = callName.toLowerNotEmpty();
                            }
                        }
                        return callName;
                    }
                }
                /// <summary>
                /// 参数索引
                /// </summary>
                public int ParameterIndex;
                ///// <summary>
                ///// 输入参数匹配集合
                ///// </summary>
                //public MethodParameterPair[] Parameters;
                /// <summary>
                /// 参数名称
                /// </summary>
                public string ParameterTypeName
                {
                    get
                    {
                        return MethodParameterTypes.GetParameterTypeName(ParameterIndex);
                    }
                }
                /// <summary>
                /// 是否AJAX加载器
                /// </summary>
                public bool IsAjaxLoad;
                /// <summary>
                /// 函数调用信息
                /// </summary>
                public string CallMethodInfo
                {
                    get
                    {
                        return "_i" + MethodIndex.toString();
                    }
                }
            }
            /// <summary>
            /// 参数创建器
            /// </summary>
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            public sealed class ParameterBuilder
            {
                /// <summary>
                /// 函数参数类型与名称集合关键字
                /// </summary>
                public readonly ReusableDictionary<MethodParameterTypeNames, MethodParameterTypeNames> ParameterIndexs = ReusableDictionary<MethodParameterTypeNames>.Create<MethodParameterTypeNames>();
                /// <summary>
                /// 参数序号
                /// </summary>
                public int ParameterIndex;
                /// <summary>
                /// 清楚数据
                /// </summary>
                public void Clear()
                {
                    ParameterIndexs.Empty();
                    ParameterIndex = 0;
                }
                /// <summary>
                /// 添加方法索引信息
                /// </summary>
                /// <param name="method">方法索引信息</param>
                public void Add(CallMethod method)
                {
                    MethodParameter[] parameters = method.MethodParameters;
                    MethodParameterTypeNames parameterTypeNames = new MethodParameterTypeNames(parameters, false, false);
                    MethodParameterTypeNames historyJsonParameterIndex = default(MethodParameterTypeNames);
                    if (parameterTypeNames.IsParameter)
                    {
                        if (!ParameterIndexs.TryGetValue(ref parameterTypeNames, out historyJsonParameterIndex))
                        {
                            parameterTypeNames.Copy(++ParameterIndex);
                            ParameterIndexs.Set(ref parameterTypeNames, historyJsonParameterIndex = parameterTypeNames);
                        }
                        method.ParameterIndex = historyJsonParameterIndex.Index;
                    }
                    //method.Parameters = MethodParameterPair.Get(parameters, historyJsonParameterIndex);
                }
                /// <summary>
                /// 获取函数参数类型集合关键字
                /// </summary>
                /// <returns></returns>
                public MethodParameterTypeNames[] Get()
                {
                    return ParameterIndexs.Keys.getLeftArray(ParameterIndexs.Count).ToArray();
                }
            }
            /// <summary>
            /// WEB调用函数
            /// </summary>
            private LeftArray<CallMethod> methods;
            /// <summary>
            /// WEB 调用函数集合
            /// </summary>
            public CallMethod[] Methods;
            /// <summary>
            /// 函数参数类型集合关键字
            /// </summary>
            public MethodParameterTypeNames[] ParameterTypes;
            /// <summary>
            /// 是否必须配置自定义属性
            /// </summary>
            public override bool IsAttribute
            {
                get { return false; }
            }
            /// <summary>
            /// 安装下一个类型
            /// </summary>
            protected override void nextCreate()
            {
                bool isAsynchronous = typeof(AutoCSer.WebView.CallAsynchronous<>).isAssignableFromGenericDefinition(Type), isPoolType = isAsynchronous || typeof(AutoCSer.WebView.Call<>).isAssignableFromGenericDefinition(Type);
                if (isPoolType)
                {
                    if (Attribute == null) Attribute = AutoCSer.WebView.Call.DefaultAttribute;
                }
                else
                {
                    if (Attribute == null) return;
                    isAsynchronous = typeof(AutoCSer.WebView.CallAsynchronous).IsAssignableFrom(Type);
                    if (!isAsynchronous && !typeof(AutoCSer.WebView.Call).IsAssignableFrom(Type))
                    {
                        Messages.Add(Type.FullName + " 必须继承自 AutoCSer.WebView.Call / AutoCSer.WebView.Call<" + Type.FullName + "> /  AutoCSer.WebView.CallAsynchronous / AutoCSer.WebView.CallAsynchronous<" + Type.FullName + ">");
                        return;
                    }
                }

                string typeCallName = Type.FullName;
                if (typeCallName.StartsWith(AutoParameter.DefaultNamespace, StringComparison.Ordinal) && typeCallName[AutoParameter.DefaultNamespace.Length] == '.')
                {
                    typeCallName = typeCallName.Substring(AutoParameter.DefaultNamespace.Length + 1);
                }

                bool isAjaxLoadType = false;
                Type baseType = Type.Type.BaseType;
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(AutoCSer.WebView.AjaxLoader<>)) isAjaxLoadType = true;

                methods.Add(Metadata.MethodIndex.GetMethods<AutoCSer.WebView.CallMethodAttribute>(Type, AutoCSer.Metadata.MemberFilters.PublicInstance, false, Attribute.IsAttribute, Attribute.IsBaseTypeAttribute)
                    .getArray(value => new CallMethod
                    {
                        Method = value,
                        WebCallMethodType = Type,
                        TypeAttribute = Attribute,
                        TypeCallName = typeCallName,
                        IsAjaxLoad = isAjaxLoadType && value.MemberName == "Load" && value.ReturnType.Type == typeof(void) && value.Parameters.Length == 0,
                        IgnoreCase = AutoParameter.WebConfig.IgnoreCase,
                        IsPoolType = isPoolType,
                        IsAsynchronous = isAsynchronous
                    }));
            }
            /// <summary>
            /// 安装完成处理
            /// </summary>
            protected override void onCreated()
            {
                if (methods.Length != 0)
                {
                    CallMethods = Methods = methods.ToArray();
                    int methodIndex = 0;
                    ParameterBuilder parameterBuilder = new ParameterBuilder();
                    foreach (CallMethod method in Methods)
                    {
                        method.MethodIndex = methodIndex++;
                        parameterBuilder.Add(method);
                    }
                    ParameterTypes = parameterBuilder.Get();

                    _code_.Length = 0;
                    create(false);
                    Coder.Add("namespace " + AutoParameter.DefaultNamespace + @"
{
" + _code_.ToString() + @"
}");
                }
            }
        }
    }
}
