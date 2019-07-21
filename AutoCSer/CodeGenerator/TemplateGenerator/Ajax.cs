using System;
using AutoCSer.Extension;
using AutoCSer.CodeGenerator.Metadata;
using System.Collections.Generic;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// AJAX调用配置
    /// </summary>
    internal sealed partial class Ajax
    {
        /// <summary>
        /// 默认空AJAX调用配置
        /// </summary>
        internal static readonly AutoCSer.WebView.AjaxMethodAttribute Null = new AutoCSer.WebView.AjaxMethodAttribute();
        
        /// <summary>
        /// AJAX调用代码生成
        /// </summary>
        [Generator(Name = "AJAX 调用", DependType = typeof(WebView.Generator), IsAuto = true)]
        internal partial class Generator : WebView.Generator<AutoCSer.WebView.AjaxAttribute>
        {
            /// <summary>
            /// AJAX API代码生成
            /// </summary>
            private static readonly TypeScript typeScript = new TypeScript();
            /// <summary>
            /// 方法索引信息
            /// </summary>
            public sealed class AjaxMethod : AsynchronousMethod
            {
                /// <summary>
                /// 获取该方法的类型
                /// </summary>
                public ExtensionType WebAjaxMethodType;
                /// <summary>
                /// 方法索引
                /// </summary>
                public int MethodIndex;
                /// <summary>
                /// 异步处理类索引名称
                /// </summary>
                public string AsyncIndexName
                {
                    get
                    {
                        return "_a" + MethodIndex.toString() + Method.GenericParameterName;
                    }
                }
                /// <summary>
                /// 类型AJAX调用配置
                /// </summary>
                public AutoCSer.WebView.AjaxAttribute TypeAttribute;
                /// <summary>
                /// 类型调用名称
                /// </summary>
                public string TypeCallName;
                /// <summary>
                /// AJAX调用配置
                /// </summary>
                private AutoCSer.WebView.AjaxMethodAttribute attribute;
                /// <summary>
                /// AJAX调用配置
                /// </summary>
                public AutoCSer.WebView.AjaxMethodAttribute Attribute
                {
                    get
                    {
                        if (attribute == null)
                        {
                            attribute = (MemberIndex ?? Method).GetSetupAttribute<AutoCSer.WebView.AjaxMethodAttribute>(false);
                            if (attribute == null) attribute = Null;
                        }
                        return attribute;
                    }
                }
                /// <summary>
                /// 接收 HTTP Body 数据内存缓冲区的最大字节数
                /// </summary>
                public int MaxMemoryStreamSize
                {
                    get { return (int)Attribute.MaxMemoryStreamSize; }
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
                /// AJAX 回调处理类型名称
                /// </summary>
                public string AjaxCallbackPool
                {
                    get { return IsPoolType ? typeof(AutoCSer.WebView.CallBase.AjaxCallback).Name + "Pool" : typeof(AutoCSer.WebView.CallBase.AjaxCallback).Name; }
                }
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
                            if (Attribute.FullName != null) callName = IgnoreCase ? attribute.FullName.ToLower() : attribute.FullName;
                            else
                            {
                                string name = attribute.MethodName ?? Method.MethodName;
                                if (name.Length == 0) callName = IgnoreCase ? TypeCallName.ToLower() : TypeCallName;
                                else
                                {
                                    callName = TypeCallName + "." + name;
                                    if (IgnoreCase) callName = callName.toLowerNotEmpty();
                                }
                            }
                        }
                        return callName;
                    }
                }
                /// <summary>
                /// 输入参数索引
                /// </summary>
                public int InputParameterIndex;
                /// <summary>
                /// 输入参数名称
                /// </summary>
                public string InputParameterTypeName
                {
                    get
                    {
                        return MethodParameterTypes.GetParameterTypeName(InputParameterIndex);
                    }
                }
                /// <summary>
                /// 输入参数索引
                /// </summary>
                public int OutputParameterIndex;
                /// <summary>
                /// 输出参数名称
                /// </summary>
                public string OutputParameterTypeName
                {
                    get
                    {
                        return MethodParameterTypes.GetParameterTypeName(OutputParameterIndex);
                    }
                }
                /// <summary>
                /// 异步回调是否检测成功状态
                /// </summary>
                protected override bool isAsynchronousFunc { get { return false; } }
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
                public void Add(AjaxMethod method)
                {
                    MethodParameter[] inputParameters = method.MethodParameters, outputParameters = method.Method.OutputParameters;
                    MethodParameterTypeNames inputParameterTypeNames = new MethodParameterTypeNames(inputParameters, false, method.Attribute.IsInputSerializeBox);
                    MethodParameterTypeNames outputParameterTypeNames = method.MethodIsReturn ? new MethodParameterTypeNames(outputParameters, method.MethodReturnType, false, method.Attribute.IsOutputSerializeBox) : new MethodParameterTypeNames(outputParameters, false, method.Attribute.IsOutputSerializeBox);
                    MethodParameterTypeNames historyInputJsonParameterIndex = default(MethodParameterTypeNames), historyOutputJsonParameterIndex = default(MethodParameterTypeNames);
                    if (inputParameterTypeNames.IsParameter)
                    {
                        if (!ParameterIndexs.TryGetValue(ref inputParameterTypeNames, out historyInputJsonParameterIndex))
                        {
                            inputParameterTypeNames.Copy(++ParameterIndex);
                            ParameterIndexs.Set(ref inputParameterTypeNames, historyInputJsonParameterIndex = inputParameterTypeNames);
                        }
                        method.InputParameterIndex = historyInputJsonParameterIndex.Index;
                    }
                    if (outputParameterTypeNames.IsParameter)
                    {
                        if (!ParameterIndexs.TryGetValue(ref outputParameterTypeNames, out historyOutputJsonParameterIndex))
                        {
                            outputParameterTypeNames.Copy(++ParameterIndex);
                            ParameterIndexs.Set(ref outputParameterTypeNames, historyOutputJsonParameterIndex = outputParameterTypeNames);
                        }
                        method.OutputParameterIndex = historyOutputJsonParameterIndex.Index;
                    }
                    method.InputParameters = MethodParameterPair.Get(inputParameters, historyInputJsonParameterIndex);
                    method.OutputParameters = MethodParameterPair.Get(outputParameters, historyOutputJsonParameterIndex);
                    MethodParameterPair.SetInputParameter(method.OutputParameters, method.InputParameters);
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
            /// web视图AJAX调用索引信息
            /// </summary>
            public sealed class ViewMethod
            {
                /// <summary>
                /// 获取改方法的类型
                /// </summary>
                public ExtensionType WebViewMethodType;
                /// <summary>
                /// WEB视图配置
                /// </summary>
                public AutoCSer.WebView.ViewAttribute Attribute;
                /// <summary>
                /// 方法索引
                /// </summary>
                public int MethodIndex;
                /// <summary>
                /// 页面对象名称
                /// </summary>
                public string PageName
                {
                    get { return "_p" + MethodIndex.toString(); }
                }
                /// <summary>
                /// 调用名称
                /// </summary>
                public string CallName;
                /// <summary>
                /// 接收 HTTP Body 数据内存缓冲区的最大字节数
                /// </summary>
                public int MaxMemoryStreamSize
                {
                    get { return (int)Attribute.MaxMemoryStreamSize; }
                }
                /// <summary>
                /// 是否使用对象池
                /// </summary>
                public bool IsPoolType
                {
                    get
                    {
                        return typeof(AutoCSer.WebView.View<>).isAssignableFromGenericDefinition(WebViewMethodType);
                    }
                }
                /// <summary>
                /// 是否存在 await 函数
                /// </summary>
                public bool IsAwaitMethod;
                /// <summary>
                /// 页面对象是否需要初始化
                /// </summary>
                public bool IsSetPage
                {
                    get { return IsAwaitMethod || Attribute.IsAsynchronous; }
                }
            }
            /// <summary>
            /// AJAX函数
            /// </summary>
            private LeftArray<AjaxMethod> methods;
            /// <summary>
            /// AJAX 函数
            /// </summary>
            public AjaxMethod[] Methods;
            /// <summary>
            /// web视图AJAX调用
            /// </summary>
            public ViewMethod[] ViewMethods;
            /// <summary>
            /// 序列化返回值 AJAX 函数
            /// </summary>
            public AjaxMethod[] SerializeMethods
            {
                get
                {
                    Dictionary<int, AjaxMethod> typeIndexs = DictionaryCreator.CreateInt<AjaxMethod>();
                    foreach (AjaxMethod method in Methods)
                    {
                        if (method.OutputParameterIndex != 0 && method.Attribute.IsCompileJsonSerialize && !typeIndexs.ContainsKey(method.OutputParameterIndex))
                        {
                            typeIndexs.Add(method.OutputParameterIndex, method);
                        }
                    }
                    return typeIndexs.getArray(value => value.Value);
                }
            }
            /// <summary>
            /// 反序列化参数 AJAX 函数
            /// </summary>
            public AjaxMethod[] DeSerializeMethods
            {
                get
                {
                    Dictionary<int, AjaxMethod> typeIndexs = DictionaryCreator.CreateInt<AjaxMethod>();
                    foreach (AjaxMethod method in Methods)
                    {
                        if (method.InputParameterIndex != 0 && method.Attribute.IsCompileJsonSerialize && !typeIndexs.ContainsKey(method.InputParameterIndex))
                        {
                            typeIndexs.Add(method.InputParameterIndex, method);
                        }
                    }
                    return typeIndexs.getArray(value => value.Value);
                }
            }
            /// <summary>
            /// 函数参数类型集合关键字
            /// </summary>
            public MethodParameterTypeNames[] ParameterTypes;
            /// <summary>
            /// 是否存在公用错误处理函数
            /// </summary>
            public bool IsPubError;
            /// <summary>
            /// AJAX函数数量
            /// </summary>
            public int MethodCount;
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
                bool isPoolType = typeof(AutoCSer.WebView.Ajax<>).isAssignableFromGenericDefinition(Type);
                if (isPoolType)
                {
                    if (Attribute == null) Attribute = AutoCSer.WebView.Ajax.DefaultAttribute;
                }
                else
                {
                    if (Attribute == null) return;
                    if (!typeof(AutoCSer.WebView.Ajax).IsAssignableFrom(Type))
                    {
                        Messages.Add(Type.FullName + " 必须继承自 AutoCSer.WebView.Ajax 或者 AutoCSer.WebView.Ajax<" + Type.FullName + ">");
                        return;
                    }
                }

                string typeCallName = Type.FullName;
                if (typeCallName.StartsWith(AutoParameter.DefaultNamespace, StringComparison.Ordinal) && typeCallName[AutoParameter.DefaultNamespace.Length] == '.')
                {
                    typeCallName = typeCallName.Substring(AutoParameter.DefaultNamespace.Length + 1);
                }
                if (typeCallName.StartsWith("Ajax.", StringComparison.Ordinal)) typeCallName = typeCallName.Substring("Ajax.".Length);

                int methodIndex = methods.Length;
                AjaxMethod[] methodIndexs = Metadata.MethodIndex.GetMethods<AutoCSer.WebView.AjaxMethodAttribute>(Type, AutoCSer.Metadata.MemberFilters.PublicInstance, false, Attribute.IsAttribute, Attribute.IsBaseTypeAttribute)
                    .getArray(value => new AjaxMethod
                    {
                        Method = value,
                        MethodIndex = methodIndex++,
                        WebAjaxMethodType = Type,
                        TypeAttribute = Attribute,
                        TypeCallName = typeCallName,
                        IgnoreCase = AutoParameter.WebConfig.IgnoreCase,
                        IsPoolType = isPoolType
                    });
                if (methodIndexs.Length != 0)
                {
                    methods.Add(methodIndexs);
                    if (Attribute.IsExportTypeScript) typeScript.Create(AutoParameter, Type, methodIndexs);
                }
            }
            /// <summary>
            /// 安装完成处理
            /// </summary>
            protected override void onCreated()
            {
                int methodIndex = methods.Length;
                ViewMethods = new WebView.Generator { AutoParameter = AutoParameter }.GetTypeAttributes()
                    .getFind(value => typeof(AutoCSer.WebView.View<>).isAssignableFromGenericDefinition(value.Key) ? (value.Value == null || value.Value.IsAjax) : (value.Value != null && value.Value.IsAjax))
                    .GetArray(value => new ViewMethod
                    {
                        MethodIndex = methodIndex++,
                        WebViewMethodType = value.Key,
                        Attribute = value.Value ?? AutoCSer.WebView.View.DefaultAttribute,
                        CallName = new WebView.Generator.ViewType { Type = value.Key, Attribute = value.Value ?? AutoCSer.WebView.View.DefaultAttribute, DefaultNamespace = AutoParameter.DefaultNamespace + ".", IgnoreCase = AutoParameter.WebConfig.IgnoreCase }.CallName,
                        IsAwaitMethod = WebView.Generator.AjaxAwaitMethodTypes.Contains(value.Key)
                    });
                if (methodIndex != 0)
                {
                    LeftArray<KeyValuePair<string, int>> names = (Methods = methods.ToArray()).getArray(value => value.CallName)
                        .concat(ViewMethods.getArray(value => value.CallName))
                        .groupCount(value => value)
                        .getFind(value => value.Value != 1);
                    if (names.Length == 0)
                    {
                        IsPubError = false;
                        ParameterBuilder parameterBuilder = new ParameterBuilder();
                        foreach (AjaxMethod method in Methods)
                        {
                            if (!IsPubError) IsPubError = method.CallName == AutoCSer.WebView.AjaxBase.PubErrorCallName;
                            parameterBuilder.Add(method);
                        }
                        ParameterTypes = parameterBuilder.Get();
                        MethodCount = Methods.Length + ViewMethods.Length + (IsPubError ? 0 : 1);
                        _code_.Length = 0;
                        create(false);
                        Coder.Add("namespace " + AutoParameter.DefaultNamespace + @"
{
" + _code_.ToString() + @"
}");
                        typeScript.OnCreated();
                    }
                    else
                    {
                        Messages.Add(@"AJAX调用名称冲突：
" + names.JoinString(@"
", value => value.Key + "[" + value.Value.toString() + "]"));
                    }
                }
            }
        }
        /// <summary>
        /// AJAX API代码生成
        /// </summary>
        [Generator(Name = "AJAX API", Language = CodeLanguage.TypeScript)]
        internal sealed partial class TypeScript : Generator
        {
            /// <summary>
            /// API 命名空间
            /// </summary>
            public const string AutoCSerAPI = "AutoCSerAPI";
            /// <summary>
            /// 代码
            /// </summary>
            private static StringArray code = new StringArray();
            /// <summary>
            /// 命名空间
            /// </summary>
            public string Namespace;
            /// <summary>
            /// 创建代码
            /// </summary>
            /// <param name="parameter"></param>
            /// <param name="type"></param>
            /// <param name="methodIndexs"></param>
            public void Create(ProjectParameter parameter, ExtensionType type, AjaxMethod[] methodIndexs)
            {
                AutoParameter = parameter;
                Type = type;
                Methods = methodIndexs;

                Namespace = type.Type.Namespace;
                if (Namespace == AutoParameter.DefaultNamespace) Namespace = AutoCSerAPI;
                else Namespace = Namespace.StartsWith(AutoParameter.DefaultNamespace, StringComparison.Ordinal) && Namespace[AutoParameter.DefaultNamespace.Length] == '.' ? AutoCSerAPI + Namespace.Substring(AutoParameter.DefaultNamespace.Length) : Namespace;

                _code_.Length = 0;
                create(false);
                code.Add(_code_);
            }
            /// <summary>
            /// 安装完成处理
            /// </summary>
            public void OnCreated()
            {
                string webViewCode = WebView.TypeScript.Code;
                if (code.Length != 0 || webViewCode != null)
                {
                    string apiCode = @"//本文件由程序自动生成,请不要自行修改
" + code.ToString() + (webViewCode == null ? null : @"
" + webViewCode);
                    code.Length = 0;
                    string fileName = AutoParameter.ProjectPath + @"ViewJs\Api.ts";
                    if (Coder.WriteFileSuffix(fileName, apiCode)) Messages.Message(fileName + " 被修改");
                }
            }
        }
    }
}
