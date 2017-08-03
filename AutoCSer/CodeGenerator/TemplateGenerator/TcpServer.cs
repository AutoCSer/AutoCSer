using System;
using AutoCSer.Extension;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.CodeGenerator.Metadata;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// TCP调用代码生成基类
    /// </summary>
    internal abstract partial class TcpServer
    {
        /// <summary>
        /// TCP 调用代码生成
        /// </summary>
        /// <typeparam name="attributeType">TCP 服务配置</typeparam>
        /// <typeparam name="methodAttributeType">TCP 调用函数配置</typeparam>
        /// <typeparam name="serverSocketSenderType">TCP 服务套接字数据发送</typeparam>
        internal abstract class Generator<attributeType, methodAttributeType, serverSocketSenderType> : TemplateGenerator.Generator<attributeType>
            where attributeType : AutoCSer.Net.TcpServer.ServerAttribute
            where methodAttributeType : AutoCSer.Net.TcpServer.MethodAttribute
            where serverSocketSenderType : AutoCSer.Net.TcpServer.ServerSocketSender
        {
            /// <summary>
            /// 方法索引信息
            /// </summary>
            public sealed class TcpMethod : AsynchronousMethod
            {
                /// <summary>
                /// 获取该方法的类型
                /// </summary>
                public Metadata.ExtensionType MethodType;
                /// <summary>
                /// TCP 服务器端配置
                /// </summary>
                public attributeType ServiceAttribute;
                /// <summary>
                /// TCP调用配置
                /// </summary>
                private methodAttributeType attribute;
                /// <summary>
                /// TCP调用配置
                /// </summary>
                public methodAttributeType Attribute
                {
                    get
                    {
                        if (attribute == null)
                        {
                            attribute = (MemberIndex ?? Method).GetSetupAttribute<methodAttributeType>(true);
                            if (MemberIndex != null && !Method.IsGetMember)
                            {
                                attribute = attribute == null ? AutoCSer.Emit.Constructor<methodAttributeType>.New() : AutoCSer.MemberCopy.Copyer<methodAttributeType>.MemberwiseClone(attribute);
                                attribute.CommandIdentity = int.MinValue;
                            }
                            else if (attribute == null) attribute = AutoCSer.Emit.Constructor<methodAttributeType>.New();
                            //attribute = AutoCSer.ui.reflection.memberInfo.customAttribute<AutoCSer.code.cSharp.tcpMethod>(Method.Method, false, true);
                        }
                        return attribute;
                    }
                }
                /// <summary>
                /// 服务方法索引
                /// </summary>
                public int MethodIndex;
                /// <summary>
                /// 方法索引名称
                /// </summary>
                public string MethodIndexName
                {
                    get
                    {
                        return "_M" + MethodIndex.toString();
                    }
                }
                /// <summary>
                /// TCP调用命令名称
                /// </summary>
                public string MethodIdentityCommand
                {
                    get
                    {
                        return "_c" + MethodIndex.toString();
                    }
                }
                /// <summary>
                /// TCP调用命令名称
                /// </summary>
                public string StaticMethodIdentityCommand
                {
                    get
                    {
                        return "_c" + StaticMethodIndex.toString();
                    }
                }
                /// <summary>
                /// TCP调用命令名称
                /// </summary>
                public string MethodAsynchronousIdentityCommand
                {
                    get
                    {
                        return "_ac" + MethodIndex.toString();
                    }
                }
                /// <summary>
                /// TCP调用命令名称
                /// </summary>
                public string AsynchronousStaticMethodIdentityCommand
                {
                    get
                    {
                        return "_ac" + StaticMethodIndex.toString();
                    }
                }
                /// <summary>
                /// 流式套接字调用索引名称
                /// </summary>
                public string MethodStreamName
                {
                    get
                    {
                        return "_s" + MethodIndex.toString();// +Method.GenericParameterName;
                    }
                }
                /// <summary>
                /// 静态服务方法索引
                /// </summary>
                public int StaticMethodIndex;
                /// <summary>
                /// 方法索引名称
                /// </summary>
                public string StaticMethodIndexName
                {
                    get
                    {
                        return "_M" + StaticMethodIndex.toString();
                    }
                }
                /// <summary>
                /// 是否验证方法
                /// </summary>
                public bool IsVerifyMethod
                {
                    get
                    {

                        if (Attribute.IsVerifyMethod)
                        {
                            if (IsAsynchronousCallback) Messages.Message("方法 " + MemberFullName + " 为异步回调方法,不符合验证方法要求");
                            else if (MethodReturnType.Type != typeof(bool)) Messages.Message("方法 " + MemberFullName + " 的返回值类型为 " + MethodReturnType.Type.fullName() + " ,不符合验证方法要求");
                            else if (MethodParameters.Length == 0) Messages.Message("方法 " + MemberFullName + " 没有输入参数,不符合验证方法要求");
                            else return true;
                        }
                        return false;
                    }
                }
                /// <summary>
                /// 任务类型名称
                /// </summary>
                private static readonly string serverTaskTypeName = typeof(AutoCSer.Net.TcpServer.ServerTaskType).fullName();
                /// <summary>
                /// 服务端任务类型
                /// </summary>
                public string ServerTask
                {
                    get
                    {
                        AutoCSer.Net.TcpServer.ServerTaskType taskType = Attribute.ServerTask;
                        if (taskType == AutoCSer.Net.TcpServer.ServerTaskType.Queue && !ServiceAttribute.IsCallQueue) taskType = AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue;
                        if (taskType == AutoCSer.Net.TcpServer.MethodAttribute.DefaultServerTask) return "AutoCSer.Net.TcpServer.MethodAttribute.DefaultServerTask";
                        return serverTaskTypeName + "." + taskType.ToString();
                    }
                }
                /// <summary>
                /// 任务类型名称
                /// </summary>
                private static readonly string clientTaskTypeName = typeof(AutoCSer.Net.TcpServer.ClientTaskType).fullName();
                /// <summary>
                /// 客户端任务类型
                /// </summary>
                public string ClientTask
                {
                    get 
                    {
                        //IsClientAsynchronous
                        AutoCSer.Net.TcpServer.ClientTaskType taskType = MemberIndex == null || !MemberIndex.IsField ? Attribute.ClientTask : AutoCSer.Net.TcpServer.ClientTaskType.Synchronous;
                        if (taskType == AutoCSer.Net.TcpServer.MethodAttribute.DefaultClientTask) return "AutoCSer.Net.TcpServer.MethodAttribute.DefaultClientTask";
                        return clientTaskTypeName + "." + taskType.ToString();
                    }
                }
                /// <summary>
                /// 返回值绑定输入参数
                /// </summary>
                private MethodParameter returnInputParameter;
                /// <summary>
                /// 返回值绑定输入参数名称
                /// </summary>
                public MethodParameter ReturnInputParameter
                {
                    get
                    {
                        if ((attribute.ParameterFlags & Net.TcpServer.ParameterFlags.ClientAsynchronousReturnInput) != 0 && returnInputParameter == null)
                        {
                            foreach (MethodParameterPair inputParameter in InputParameters)
                            {
                                if (inputParameter.MethodParameter.ParameterType.Type == MethodReturnType.Type) return returnInputParameter = inputParameter.MethodParameter;
                            }
                        }
                        return returnInputParameter;
                    }
                }
                /// <summary>
                /// 客户端标识参数名称
                /// </summary>
                private string clientParameterName;
                /// <summary>
                /// 客户端标识参数名称
                /// </summary>
                public string ClientParameterName
                {
                    get
                    {
                        checkAsynchronousReturn();
                        return clientParameterName;
                    }
                }
                /// <summary>
                /// 属性或者字段设置值函数信息
                /// </summary>
                public TcpMethod SetMethod;
                /// <summary>
                /// 输入参数索引
                /// </summary>
                public int InputParameterIndex;
                /// <summary>
                /// 是否简单序列化输入参数
                /// </summary>
                public bool IsSimpleSerializeInputParamter;
                ///// <summary>
                ///// 是否存在输入参数
                ///// </summary>
                //public bool IsInputParameter
                //{
                //    get
                //    {
                //        return MethodParameters.Length != 0 || Method.GenericParameters.Length != 0 || (IsAsynchronousCallback && MethodReturnType.Type.IsGenericParameter);
                //    }
                //}
                ///// <summary>
                ///// 输入参数类型名称class/struct
                ///// </summary>
                //public string InputParameterClassType
                //{
                //    get
                //    {
                //        return (Attribute.ParameterFlags & Net.TcpServer.ParameterFlags.InputClass) == 0 ? "struct" : "sealed class";
                //    }
                //}
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
                /// 是否简单序列化输出参数
                /// </summary>
                public bool IsSimpleSerializeOutputParamter;
                /// <summary>
                /// 是否存在输出参数
                /// </summary>
                public bool IsOutputParameter
                {
                    get
                    {
                        return MethodReturnType.Type != typeof(void) || Method.OutputParameters.Length != 0;
                    }
                }
                ///// <summary>
                ///// 输出参数类型名称class/struct
                ///// </summary>
                //public string OutputParameterClassType
                //{
                //    get
                //    {
                //        return (Attribute.ParameterFlags & Net.TcpServer.ParameterFlags.OutputClass) == 0 ? "struct" : "sealed class";
                //    }
                //}
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
                /// 是否定义服务器端调用
                /// </summary>
                public bool IsMethodServerCall
                {
                    get
                    {
                        return !IsAsynchronousCallback && Attribute.ServerTask != Net.TcpServer.ServerTaskType.Synchronous;
                        //return !IsAsynchronousCallback && (Attribute.IsServerTask || MemberIndex != null || IsClientSendOnly == 0 || Method.Method.IsGenericMethod);
                    }
                }
                /// <summary>
                /// 是否仅发送数据
                /// </summary>
                public int IsClientSendOnly
                {
                    get
                    {
                        if (Attribute.IsClientSendOnly && !IsClientAsynchronous)
                        {
                            checkAsynchronousReturn();
                            if (MethodReturnType.Type == typeof(void)) return 1;
                        }
                        return 0;
                    }
                }
                /// <summary>
                /// 客户端调用是否支持异步
                /// </summary>
                public bool IsClientAsynchronous
                {
                    get
                    {
                        return (Attribute.GetIsClientAsynchronous && IsOutputParameter) || IsKeepCallback != 0;
                        //return IsOutputParameter || IsKeepCallback;
                    }
                }
                /// <summary>
                /// 客户端调用是否支持同步
                /// </summary>
                public bool IsClientSynchronous
                {
                    get
                    {
                        return Attribute.GetIsClientSynchronous && IsKeepCallback == 0 && IsClientSendOnly == 0;
                    }
                }
                /// <summary>
                /// 客户端是否支持 async Task
                /// </summary>
                public bool IsClientTaskAsync
                {
                    get { return Attribute.IsClientTaskAsync; }
                }
                /// <summary>
                /// 是否生成同步 TCP 调用命令名称
                /// </summary>
                public bool IsSynchronousMethodIdentityCommand
                {
                    get
                    {
                        return MemberIndex != null || IsClientSendOnly != 0 || IsClientSynchronous;
                    }
                }
                /// <summary>
                /// 是否保持异步回调
                /// </summary>
                public int IsKeepCallback
                {
                    get
                    {
                        //return IsAsynchronousCallback && Method.Method.ReturnType == typeof(AutoCSer.Net.TcpServer.KeepCallback) && !IsVerifyMethod ? 1 : 0;
                        return Attribute.GetIsKeepCallback && !IsVerifyMethod ? 1 : 0;
                    }
                }
                /// <summary>
                /// 保持异步回调类型名称
                /// </summary>
                private static readonly string keepCallbackType = typeof(AutoCSer.Net.TcpServer.KeepCallback).fullName();
                /// <summary>
                /// 保持异步回调类型名称
                /// </summary>
                public string KeepCallbackType
                {
                    get
                    {
                        return IsKeepCallback == 0 ? "void" : keepCallbackType;
                    }
                }
                /// <summary>
                /// 是否使用 JSON 序列化
                /// </summary>
                public bool IsJsonSerialize
                {
                    get { return ServiceAttribute.GetIsJsonSerialize ^ !Attribute.IsServerSerialize; }
                }
                ///// <summary>
                ///// HTTP调用名称
                ///// </summary>
                //public string HttpMethodName
                //{
                //    get
                //    {
                //        if (Attribute.HttpName != null) return Attribute.HttpName;
                //        if (typeof(attributeType) == typeof(AutoCSer.Net.Tcp.CommandServerAttribute)) return MemberIndex == null ? Method.Method.Name : MemberIndex.Member.Name;
                //        return MethodType.Type.Name + "." + (MemberIndex == null ? Method.Method.Name : MemberIndex.Member.Name);
                //    }
                //}
                /// <summary>
                /// 是否空方法
                /// </summary>
                public bool IsNullMethod;
                /// <summary>
                /// 检测异步回调方法
                /// </summary>
                protected override void checkAsynchronousReturn()
                {
                    if (methodParameters == null)
                    {
                        base.checkAsynchronousReturn();
                        if (methodParameters.Length != 0)
                        {
                            if (!methodParameters[0].IsRefOrOut && methodParameters[0].ParameterType.Type == typeof(serverSocketSenderType))
                            {
                                clientParameterName = methodParameters[0].ParameterName;
                                methodParameters = MethodParameter.Get(methodParameters.getSub(1, methodParameters.Length - 1));
                            }
                        }
                    }
                }

                /// <summary>
                /// 空方法索引信息
                /// </summary>
                private static readonly TcpMethod nullMethod = new TcpMethod { IsNullMethod = true };
                /// <summary>
                /// 检测方法序号
                /// </summary>
                /// <param name="methodIndexs">方法集合</param>
                /// <param name="rememberIdentityCommand">命令序号记忆数据</param>
                /// <param name="getMethodKeyName">获取命令名称的委托</param>
                /// <returns>方法集合,失败返回null</returns>
                public static TcpMethod[] CheckIdentity(TcpMethod[] methodIndexs, Dictionary<HashString, int> rememberIdentityCommand, Func<TcpMethod, string> getMethodKeyName)
                {
                    int maxIdentity = methodIndexs.Length - 1;
                    Dictionary<int, TcpMethod> identitys = DictionaryCreator.CreateInt<TcpMethod>();
                    foreach (TcpMethod method in methodIndexs)
                    {
                        int identity = method.Attribute.CommandIdentity;
                        if (identity >= 0)
                        {
                            TcpMethod identityMethod;
                            if (identitys.TryGetValue(identity, out identityMethod))
                            {
                                Messages.Add(method.MethodType.FullName + " 命令序号重复 " + method.MemberFullName + " [" + identity.toString() + "] " + identityMethod.MemberFullName);
                                return null;
                            }
                            identitys.Add(identity, method);
                            if (identity > maxIdentity) maxIdentity = identity;
                        }
                    }
                    if (rememberIdentityCommand.Count != 0)
                    {
                        foreach (TcpMethod method in methodIndexs)
                        {
                            int identity = method.Attribute.CommandIdentity;
                            if (identity < 0 && rememberIdentityCommand.TryGetValue(getMethodKeyName(method), out identity))
                            {
                                identitys.Add(method.Attribute.CommandIdentity = identity, method);
                                if (identity > maxIdentity) maxIdentity = identity;
                            }
                        }
                    }
                    TcpMethod[] sortMethodIndexs = new TcpMethod[maxIdentity + 1];
                    foreach (KeyValuePair<int, TcpMethod> methods in identitys) sortMethodIndexs[methods.Key] = methods.Value;
                    maxIdentity = 0;
                    foreach (TcpMethod method in methodIndexs)
                    {
                        if (method.Attribute.CommandIdentity < 0)
                        {
                            while (sortMethodIndexs[maxIdentity] != null) ++maxIdentity;
                            sortMethodIndexs[method.Attribute.CommandIdentity = maxIdentity] = method;
                            ++maxIdentity;
                        }
                    }
                    while (maxIdentity != sortMethodIndexs.Length)
                    {
                        if (sortMethodIndexs[maxIdentity] == null) sortMethodIndexs[maxIdentity] = nullMethod;
                        ++maxIdentity;
                    }
                    return sortMethodIndexs;
                    //if (rememberIdentityCommand.Count != 0)
                    //{
                    //    foreach (TcpMethod method in methodIndexs)
                    //    {
                    //        if (method.Attribute.CommandIentity == int.MaxValue)
                    //        {
                    //            int identity;
                    //            if (rememberIdentityCommand.TryGetValue(getMethodKeyName(method), out identity)) method.Attribute.CommandIentity = identity;
                    //        }
                    //    }
                    //}
                    //TcpMethod[] setMethodIndexs = methodIndexs.getFindArray(value => value.Attribute.CommandIentity != int.MaxValue);
                    //int count = 0;
                    //foreach (TcpMethod method in setMethodIndexs)
                    //{
                    //    int identity = method.Attribute.CommandIentity;
                    //    if (identity != int.MaxValue && identity > count) count = identity;
                    //}
                    //if (++count < methodIndexs.Length) count = methodIndexs.Length;
                    //TcpMethod[] sortMethodIndexs = new TcpMethod[count];
                    //foreach (TcpMethod method in setMethodIndexs)
                    //{
                    //    int identity = method.Attribute.CommandIentity;
                    //    if (sortMethodIndexs[identity] == null) sortMethodIndexs[identity] = method;
                    //    else
                    //    {
                    //        Messages.Add(method.MethodType.FullName + " 命令序号重复 " + method.MemberFullName + "[" + identity.toString() + "]" + sortMethodIndexs[identity].MemberFullName);
                    //        return null;
                    //    }
                    //}
                    //count = 0;
                    //foreach (TcpMethod method in methodIndexs.getFind(value => value.Attribute.CommandIentity == int.MaxValue))
                    //{
                    //    while (sortMethodIndexs[count] != null) ++count;
                    //    (sortMethodIndexs[count] = method).Attribute.CommandIentity = count;
                    //    ++count;
                    //}
                    //while (count != sortMethodIndexs.Length)
                    //{
                    //    if (sortMethodIndexs[count] == null) sortMethodIndexs[count] = nullMethod;
                    //    ++count;
                    //}
                    //return sortMethodIndexs;
                }
                ///// <summary>
                ///// HTTP调用名称冲突检测
                ///// </summary>
                ///// <param name="methodIndexs">方法集合</param>
                ///// <returns>是否成功</returns>
                //public static bool CheckHttpMethodName(TcpMethod[] methodIndexs)
                //{
                //    Dictionary<string, ListArray<TcpMethod>> groups = methodIndexs.getFindArray(value => !value.IsNullMethod).group(value => value.HttpMethodName);
                //    if (groups.Count == methodIndexs.Length) return true;
                //    foreach (KeyValuePair<string, ListArray<TcpMethod>> group in groups)
                //    {
                //        if (group.Value.Length != 1) Messages.Add("HTTP调用命令冲突 " + group.Key + "[" + group.Value.Length.toString() + "]");
                //    }
                //    return false;
                //}
            }
            /// <summary>
            /// 参数创建器
            /// </summary>
            public sealed class ParameterBuilder
            {
                /// <summary>
                /// 函数参数类型集合关键字
                /// </summary>
                public readonly Dictionary<MethodParameterTypes, MethodParameterTypes> ParameterIndexs = DictionaryCreator<MethodParameterTypes>.Create<MethodParameterTypes>();
                /// <summary>
                /// 函数参数类型与名称集合关键字
                /// </summary>
                public readonly Dictionary<MethodParameterTypeNames, MethodParameterTypeNames> JsonParameterIndexs = DictionaryCreator<MethodParameterTypeNames>.Create<MethodParameterTypeNames>();
                /// <summary>
                /// 参数序号
                /// </summary>
                public int ParameterIndex;
                /// <summary>
                /// 是否支持简单序列化操作
                /// </summary>
                public bool IsSimpleSerialize;
                /// <summary>
                /// 清除数据
                /// </summary>
                /// <param name="isSimpleSerialize">是否支持简单序列化操作</param>
                public void Clear(bool isSimpleSerialize)
                {
                    IsSimpleSerialize = isSimpleSerialize;
                    ParameterIndexs.Clear();
                    JsonParameterIndexs.Clear();
                }
                /// <summary>
                /// 添加方法索引信息
                /// </summary>
                /// <param name="method">方法索引信息</param>
                public void Add(TcpMethod method)
                {
                    MethodParameter[] inputParameters = method.MethodParameters, outputParameters = method.Method.OutputParameters;
                    if (method.IsJsonSerialize)
                    {
                        MethodParameterTypeNames inputParameterTypeNames = new MethodParameterTypeNames(inputParameters, method.Attribute.IsInputSerializeReferenceMember, method.Attribute.IsInputSerializeBox);
                        MethodParameterTypeNames outputParameterTypeNames = method.MethodIsReturn ? new MethodParameterTypeNames(outputParameters, method.MethodReturnType, method.Attribute.IsOutputSerializeReferenceMember, method.Attribute.IsOutputSerializeBox) : new MethodParameterTypeNames(outputParameters, method.Attribute.IsOutputSerializeReferenceMember, method.Attribute.IsOutputSerializeBox);
                        MethodParameterTypeNames historyInputJsonParameterIndex = default(MethodParameterTypeNames), historyOutputJsonParameterIndex = default(MethodParameterTypeNames);
                        if (inputParameterTypeNames.IsParameter)
                        {
                            if (!JsonParameterIndexs.TryGetValue(inputParameterTypeNames, out historyInputJsonParameterIndex))
                            {
                                inputParameterTypeNames.Copy(++ParameterIndex);
                                JsonParameterIndexs.Add(inputParameterTypeNames, historyInputJsonParameterIndex = inputParameterTypeNames);
                            }
                            method.InputParameterIndex = historyInputJsonParameterIndex.Index;
                        }
                        if (outputParameterTypeNames.IsParameter)
                        {
                            if (!JsonParameterIndexs.TryGetValue(outputParameterTypeNames, out historyOutputJsonParameterIndex))
                            {
                                outputParameterTypeNames.Copy(++ParameterIndex);
                                JsonParameterIndexs.Add(outputParameterTypeNames, historyOutputJsonParameterIndex = outputParameterTypeNames);
                            }
                            method.OutputParameterIndex = historyOutputJsonParameterIndex.Index;
                        }
                        method.InputParameters = MethodParameterPair.Get(inputParameters, historyInputJsonParameterIndex);
                        method.OutputParameters = MethodParameterPair.Get(outputParameters, historyOutputJsonParameterIndex);
                    }
                    else
                    {
                        MethodParameterTypes inputParameterTypes = new MethodParameterTypes(inputParameters, method.Attribute.IsInputSerializeReferenceMember, method.Attribute.IsInputSerializeBox);
                        MethodParameterTypes outputParameterTypes = method.MethodIsReturn ? new MethodParameterTypes(outputParameters, method.MethodReturnType, method.Attribute.IsOutputSerializeReferenceMember, method.Attribute.IsOutputSerializeBox) : new MethodParameterTypes(outputParameters, method.Attribute.IsOutputSerializeReferenceMember, method.Attribute.IsOutputSerializeBox);
                        MethodParameterTypes historyInputParameterIndex = default(MethodParameterTypes), historyOutputParameterIndex = default(MethodParameterTypes);
                        if (inputParameterTypes.IsParameter)
                        {
                            if (!ParameterIndexs.TryGetValue(inputParameterTypes, out historyInputParameterIndex))
                            {
                                inputParameterTypes.Copy(++ParameterIndex);
                                ParameterIndexs.Add(inputParameterTypes, historyInputParameterIndex = inputParameterTypes);
                            }
                            method.InputParameterIndex = historyInputParameterIndex.Index;
                            if(IsSimpleSerialize) method.IsSimpleSerializeInputParamter = historyInputParameterIndex.IsSimpleSerialize;
                        }
                        if (outputParameterTypes.IsParameter)
                        {
                            if (!ParameterIndexs.TryGetValue(outputParameterTypes, out historyOutputParameterIndex))
                            {
                                outputParameterTypes.Copy(++ParameterIndex);
                                ParameterIndexs.Add(outputParameterTypes, historyOutputParameterIndex = outputParameterTypes);
                            }
                            method.OutputParameterIndex = historyOutputParameterIndex.Index;
                            if (IsSimpleSerialize) method.IsSimpleSerializeOutputParamter = historyOutputParameterIndex.IsSimpleSerialize;
                        }
                        method.InputParameters = MethodParameterPair.Get(inputParameters, historyInputParameterIndex);
                        method.OutputParameters = MethodParameterPair.Get(outputParameters, historyOutputParameterIndex);
                    }
                    MethodParameterPair.SetInputParameter(method.OutputParameters, method.InputParameters);
                }
                /// <summary>
                /// 获取函数参数类型集合关键字
                /// </summary>
                /// <returns></returns>
                public MethodParameterTypes[] Get()
                {
                    MethodParameterTypes[] parameters = new MethodParameterTypes[ParameterIndexs.Count + JsonParameterIndexs.Count];
                    ParameterIndex = 0;
                    foreach (MethodParameterTypes parameter in ParameterIndexs.Keys)
                    {
                        if (parameter.IsParameter) parameters[ParameterIndex++] = parameter;
                    }
                    foreach (MethodParameterTypeNames parameter in JsonParameterIndexs.Keys)
                    {
                        if (parameter.IsParameter) parameters[ParameterIndex++].Set(parameter);
                    }
                    return parameters;
                }
            }
            /// <summary>
            /// TCP 服务器端配置
            /// </summary>
            public attributeType ServiceAttribute
            {
                get { return Attribute; }
                set { Attribute = value; }
            }
            /// <summary>
            /// 服务类名称
            /// </summary>
            public virtual string ServerName { get { return Attribute.ServerName; } }
            /// <summary>
            /// 服务注册名称
            /// </summary>
            public virtual string ServerRegisterName { get { return Attribute.ServerName; } }
            /// <summary>
            /// TCP服务调用配置JSON
            /// </summary>
            public string AttributeJson
            {
                get
                {
                    return AutoCSer.Json.Serializer.Serialize(Attribute).Json.Replace(@"""", @"""""");
                }
            }
            /// <summary>
            /// 方法索引集合
            /// </summary>
            public TcpMethod[] MethodIndexs;
            /// <summary>
            /// 函数参数类型集合关键字
            /// </summary>
            public MethodParameterTypes[] ParameterTypes;
            /// <summary>
            /// 是否存在验证函数
            /// </summary>
            public bool IsVerifyMethod;
            /// <summary>
            /// 命令序号记忆字段名称
            /// </summary>
            public string RememberIdentityCommeandName
            {
                get { return "_identityCommandNames_"; }
            }
            /// <summary>
            /// 用户命令起始位置
            /// </summary>
            public int CommandStartIndex
            {
                get { return AutoCSer.Net.TcpServer.Server.CommandStartIndex; }
            }
            /// <summary>
            /// 异步关键字
            /// </summary>
            public string Async
            {
                get { return "async"; }
            }
            /// <summary>
            /// 异步等待关键字
            /// </summary>
            public string Await
            {
                get { return "await"; }
            }
            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            protected static readonly Dictionary<HashString, int> nullRememberIdentityName = new Dictionary<HashString, int>();
            /// <summary>
            /// 获取命令序号记忆数据
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            protected Dictionary<HashString, int> getRememberIdentityName(Type type)
            {
                MethodInfo rememberIdentity = type.GetMethod(RememberIdentityCommeandName, BindingFlags.Static | BindingFlags.NonPublic);
                if (rememberIdentity != null)
                {
                    KeyValue<string, int>[] nameArray = (KeyValue<string, int>[])rememberIdentity.Invoke(null, null);
                    if (nameArray.Length != 0)
                    {
                        Dictionary<HashString, int> names = DictionaryCreator.CreateHashString<int>(nameArray.Length << 1);
                        foreach (KeyValue<string, int> name in (KeyValue<string, int>[])rememberIdentity.Invoke(null, null))
                        {
                            if (name.Key != null) names[name.Key] = name.Value;
                        }
                        return names;
                    }
                }
                return nullRememberIdentityName;
            }
        }
    }
}
