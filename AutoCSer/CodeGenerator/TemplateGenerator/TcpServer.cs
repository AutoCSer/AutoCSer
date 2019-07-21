using System;
using AutoCSer.Extension;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.CodeGenerator.Metadata;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// TCP 服务代码生成基类
    /// </summary>
    internal abstract partial class TcpServer
    {
        /// <summary>
        /// TCP 服务代码生成
        /// </summary>
        /// <typeparam name="attributeType">TCP 服务配置</typeparam>
        /// <typeparam name="methodAttributeType">TCP 调用函数配置</typeparam>
        /// <typeparam name="serverSocketType">TCP 服务套接字类型</typeparam>
        internal abstract class GeneratorBase<attributeType, methodAttributeType, serverSocketType> : TemplateGenerator.Generator<attributeType>
            where attributeType : AutoCSer.Net.TcpServer.ServerBaseAttribute
            where methodAttributeType : AutoCSer.Net.TcpServer.MethodBaseAttribute
            where serverSocketType : class
        {
            /// <summary>
            /// 方法索引信息
            /// </summary>
            public abstract class TcpMethod : AsynchronousMethod
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
                /// TCP 调用命令名称
                /// </summary>
                public string MethodIdentityCommand
                {
                    get
                    {
                        return "_c" + MethodIndex.toString();
                    }
                }
                /// <summary>
                /// await TCP 调用命令名称
                /// </summary>
                public string AwaiterMethodIdentityCommand
                {
                    get
                    {
                        return "_a" + MethodIndex.toString();
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
                public string AsynchronousStaticMethodIdentityCommand
                {
                    get
                    {
                        return "_ac" + StaticMethodIndex.toString();
                    }
                }
                /// <summary>
                /// await TCP 调用命令名称
                /// </summary>
                public string StaticAwaiterMethodIdentityCommand
                {
                    get
                    {
                        return "_a" + StaticMethodIndex.toString();
                    }
                }
                /// <summary>
                /// 验证方法是否支持异步
                /// </summary>
                public virtual bool IsVerifyMethodAsynchronous { get { return false; } }
                /// <summary>
                /// 是否验证方法
                /// </summary>
                public bool IsVerifyMethod
                {
                    get
                    {
                        if (Attribute.IsVerifyMethod)
                        {
                            if (IsAsynchronousCallback && !IsVerifyMethodAsynchronous) Messages.Message("方法 " + MemberFullName + " 为异步回调方法,不符合验证方法要求");
                            else if (MethodReturnType.Type != typeof(bool)) Messages.Message("方法 " + MemberFullName + " 的返回值类型为 " + MethodReturnType.Type.fullName() + " ,不符合验证方法要求");
                            else if (MethodParameters.Length == 0) Messages.Message("方法 " + MemberFullName + " 没有输入参数,不符合验证方法要求");
                            else return true;
                        }
                        return false;
                    }
                }
                ///// <summary>
                ///// 设置验证方法服务端任务类型
                ///// </summary>
                //public void CheckVerifyMethodServerTaskType()
                //{
                //    if (!IsVerifyMethodAsynchronous) Attribute.ServerTaskType = Net.TcpServer.ServerTaskType.Synchronous;
                //}
                /// <summary>
                /// 是否使用 JSON 序列化
                /// </summary>
                public bool IsJsonSerialize
                {
                    get { return ServiceAttribute.GetIsJsonSerialize ^ !Attribute.IsServerSerialize; }
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
                        if ((attribute.GetParameterFlags & Net.TcpServer.ParameterFlags.ClientAsynchronousReturnInput) != 0 && returnInputParameter == null)
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
                /// 是否简单序列化输入参数
                /// </summary>
                public bool IsSimpleSerializeInputParamter;
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
                /// 是否简单序列化输出参数
                /// </summary>
                public bool IsSimpleSerializeOutputParamter;
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
                        AutoCSer.Net.TcpServer.ServerTaskType taskType = Attribute.ServerTaskType;
                        //if (taskType == AutoCSer.Net.TcpServer.ServerTaskType.Queue && !ServiceAttribute.IsCallQueue) taskType = AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue;
                        //if (taskType == AutoCSer.Net.TcpServer.MethodAttribute.DefaultServerTask) return "AutoCSer.Net.TcpServer.MethodAttribute.DefaultServerTask";
                        return serverTaskTypeName + "." + taskType.ToString();
                    }
                }
                /// <summary>
                /// 是否定义服务器端调用
                /// </summary>
                public virtual bool IsMethodServerCall
                {
                    get
                    {
                        return !IsAsynchronousCallback && Attribute.ServerTaskType != Net.TcpServer.ServerTaskType.Synchronous;
                        //return !IsAsynchronousCallback && (Attribute.IsServerTask || MemberIndex != null || IsClientSendOnly == 0 || Method.Method.IsGenericMethod);
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
                /// 是否存在输出参数
                /// </summary>
                public bool IsOutputParameter
                {
                    get
                    {
                        return MethodReturnType.Type != typeof(void) || Method.OutputParameters.Length != 0;
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
                    get { return Attribute.GetIsClientTaskAsync && IsKeepCallback == 0 && IsClientSendOnly == 0; }
                }
                /// <summary>
                /// 客户端调用是否支持异步
                /// </summary>
                public bool IsClientAsynchronous
                {
                    get
                    {
                        return Attribute.GetIsClientAsynchronous || IsKeepCallback != 0;
                        //return (Attribute.GetIsClientAsynchronous && IsOutputParameter) || IsKeepCallback != 0;
                    }
                }
                /// <summary>
                /// 是否仅发送数据
                /// </summary>
                public int IsClientSendOnly
                {
                    get
                    {
                        if (Attribute.GetIsClientSendOnly && !IsClientAsynchronous)
                        {
                            checkAsynchronousReturn();
                            if (MethodReturnType.Type == typeof(void)) return 1;
                        }
                        return 0;
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
                        AutoCSer.Net.TcpServer.ClientTaskType taskType = MemberIndex == null || !MemberIndex.IsField ? Attribute.ClientTaskType : AutoCSer.Net.TcpServer.ClientTaskType.Synchronous;
                        //if (taskType == AutoCSer.Net.TcpServer.MethodAttribute.DefaultClientTask) return "AutoCSer.Net.TcpServer.MethodAttribute.DefaultClientTask";
                        return clientTaskTypeName + "." + taskType.ToString();
                    }
                }
                /// <summary>
                /// 客户端是否支持 await
                /// </summary>
                public bool IsClientAwaiter
                {
                    get
                    {
                        if (IsKeepCallback == 0 && IsClientSendOnly == 0)
                        {
                            if (Attribute.GetIsClientAwaiter)
                            {
                                foreach (MethodParameter parameter in MethodParameters)
                                {
                                    if (parameter.IsRefOrOut)
                                    {
                                        //Messages.Message(Method.Method.fullName() + " 存在 ref / out 参数不支持 await");
                                        return false;
                                    }
                                }
                                return true;
                            }
                        }
                        return false;
                    }
                }
                /// <summary>
                /// await 类型名称
                /// </summary>
                private static readonly string awaiterTypeName = typeof(AutoCSer.Net.TcpServer.Awaiter<>).onlyName();
                /// <summary>
                /// await 类型名称
                /// </summary>
                private static readonly string awaiterBoxTypeName = typeof(AutoCSer.Net.TcpServer.AwaiterBox<>).onlyName();
                /// <summary>
                /// await 类型名称
                /// </summary>
                private static readonly string awaiterReferenceTypeName = typeof(AutoCSer.Net.TcpServer.AwaiterReference<>).onlyName();
                /// <summary>
                /// await 类型名称
                /// </summary>
                private static readonly string awaiterBoxReferenceTypeName = typeof(AutoCSer.Net.TcpServer.AwaiterBoxReference<>).onlyName();
                /// <summary>
                /// await 类型
                /// </summary>
                public string Awaiter
                {
                    get
                    {
                        if (MethodIsReturn)
                        {
                            if (Attribute.IsOutputSerializeReferenceMember)
                            {
                                return Attribute.IsOutputSerializeBox ? awaiterBoxReferenceTypeName : awaiterReferenceTypeName;
                            }
                            return Attribute.IsOutputSerializeBox ? awaiterBoxTypeName : awaiterTypeName;
                        }
                        return typeof(AutoCSer.Net.TcpServer.Awaiter).Name;
                    }
                }
                /// <summary>
                /// await 类型名称
                /// </summary>
                private static readonly string awaiterReturnValueTypeName = typeof(AutoCSer.Net.TcpServer.AwaiterReturnValue<>).onlyName();
                /// <summary>
                /// await 类型名称
                /// </summary>
                private static readonly string awaiterReturnValueBoxTypeName = typeof(AutoCSer.Net.TcpServer.AwaiterReturnValueBox<>).onlyName();
                /// <summary>
                /// await 类型名称
                /// </summary>
                private static readonly string awaiterReturnValueReferenceTypeName = typeof(AutoCSer.Net.TcpServer.AwaiterReturnValueReference<>).onlyName();
                /// <summary>
                /// await 类型名称
                /// </summary>
                private static readonly string awaiterReturnValueBoxReferenceTypeName = typeof(AutoCSer.Net.TcpServer.AwaiterReturnValueBoxReference<>).onlyName();
                /// <summary>
                /// await 返回值包装类型
                /// </summary>
                public string AwaiterReturnValue
                {
                    get
                    {
                        if (Attribute.IsOutputSerializeReferenceMember)
                        {
                            return Attribute.IsOutputSerializeBox ? awaiterReturnValueBoxReferenceTypeName : awaiterReturnValueReferenceTypeName;
                        }
                        return Attribute.IsOutputSerializeBox ? awaiterReturnValueBoxTypeName : awaiterReturnValueTypeName;
                    }
                }
                /// <summary>
                /// 是否生成同步 TCP 调用命令名称
                /// </summary>
                public bool IsSynchronousMethodIdentityCommand
                {
                    get
                    {
                        return MemberIndex != null || IsClientSendOnly != 0 || IsClientSynchronous || IsClientTaskAsync;
                    }
                }
                /// <summary>
                /// 是否生成 await TCP 调用命令名称
                /// </summary>
                public bool IsAwaiterMethodIdentityCommand
                {
                    get { return IsClientAwaiter || IsClientTaskAsync; }
                }

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
                            if (!methodParameters[0].IsRefOrOut && methodParameters[0].ParameterType.Type == typeof(serverSocketType))
                            {
                                clientParameterName = methodParameters[0].ParameterName;
                                methodParameters = MethodParameter.Get(methodParameters.getSub(1, methodParameters.Length - 1));
                            }
                        }
                    }
                }
            }
            /// <summary>
            /// 方法索引信息
            /// </summary>
            /// <typeparam name="methodType">方法索引信息类型</typeparam>
            public abstract class TcpMethod<methodType> : TcpMethod
                where methodType : TcpMethod<methodType>
            {
                /// <summary>
                /// 属性或者字段设置值函数信息
                /// </summary>
                public methodType SetMethod;

                /// <summary>
                /// 检测方法序号
                /// </summary>
                /// <param name="methodIndexs">方法集合</param>
                /// <param name="rememberIdentityCommand">命令序号记忆数据</param>
                /// <param name="getMethodKeyName">获取命令名称的委托</param>
                /// <param name="nullMethod">空方法索引信息</param>
                /// <returns>方法集合,失败返回null</returns>
                public static methodType[] CheckIdentity(methodType[] methodIndexs, Dictionary<HashString, int> rememberIdentityCommand, Func<methodType, string> getMethodKeyName, methodType nullMethod)
                {
                    int maxIdentity = methodIndexs.Length - 1;
                    Dictionary<int, methodType> identitys = DictionaryCreator.CreateInt<methodType>();
                    foreach (methodType method in methodIndexs)
                    {
                        int identity = method.Attribute.CommandIdentity;
                        if (identity >= 0)
                        {
                            methodType identityMethod;
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
                        foreach (methodType method in methodIndexs)
                        {
                            int identity = method.Attribute.CommandIdentity;
                            if (identity < 0 && rememberIdentityCommand.TryGetValue(getMethodKeyName(method), out identity))
                            {
                                identitys.Add(method.Attribute.CommandIdentity = identity, method);
                                if (identity > maxIdentity) maxIdentity = identity;
                            }
                        }
                    }
                    methodType[] sortMethodIndexs = new methodType[maxIdentity + 1];
                    foreach (KeyValuePair<int, methodType> methods in identitys) sortMethodIndexs[methods.Key] = methods.Value;
                    maxIdentity = 0;
                    foreach (methodType method in methodIndexs)
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
                }
            }
            /// <summary>
            /// 参数创建器
            /// </summary>
            public sealed class ParameterBuilder
            {
                /// <summary>
                /// 函数参数类型集合关键字
                /// </summary>
                public readonly ReusableDictionary<MethodParameterTypes, MethodParameterTypes> ParameterIndexs = ReusableDictionary<MethodParameterTypes>.Create<MethodParameterTypes>();
                /// <summary>
                /// 函数参数类型与名称集合关键字
                /// </summary>
                public readonly ReusableDictionary<MethodParameterTypeNames, MethodParameterTypeNames> JsonParameterIndexs = ReusableDictionary<MethodParameterTypeNames>.Create<MethodParameterTypeNames>();
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
                    ParameterIndexs.Empty();
                    JsonParameterIndexs.Empty();
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
                            if (!JsonParameterIndexs.TryGetValue(ref inputParameterTypeNames, out historyInputJsonParameterIndex))
                            {
                                inputParameterTypeNames.Copy(++ParameterIndex);
                                JsonParameterIndexs.Set(ref inputParameterTypeNames, historyInputJsonParameterIndex = inputParameterTypeNames);
                            }
                            method.InputParameterIndex = historyInputJsonParameterIndex.Index;
                        }
                        if (outputParameterTypeNames.IsParameter)
                        {
                            if (!JsonParameterIndexs.TryGetValue(ref outputParameterTypeNames, out historyOutputJsonParameterIndex))
                            {
                                outputParameterTypeNames.Copy(++ParameterIndex);
                                JsonParameterIndexs.Set(ref outputParameterTypeNames, historyOutputJsonParameterIndex = outputParameterTypeNames);
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
                            if (!ParameterIndexs.TryGetValue(ref inputParameterTypes, out historyInputParameterIndex))
                            {
                                inputParameterTypes.Copy(++ParameterIndex);
                                ParameterIndexs.Set(ref inputParameterTypes, historyInputParameterIndex = inputParameterTypes);
                            }
                            method.InputParameterIndex = historyInputParameterIndex.Index;
                            if (IsSimpleSerialize) method.IsSimpleSerializeInputParamter = historyInputParameterIndex.IsSimpleSerialize;
                        }
                        if (outputParameterTypes.IsParameter)
                        {
                            if (!ParameterIndexs.TryGetValue(ref outputParameterTypes, out historyOutputParameterIndex))
                            {
                                outputParameterTypes.Copy(++ParameterIndex);
                                ParameterIndexs.Set(ref outputParameterTypes, historyOutputParameterIndex = outputParameterTypes);
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
            /// 服务类名称
            /// </summary>
            public string StaticServerName { get { return ServerName; } }
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
            /// 是否提供独占的 TCP 服务器端同步调用队列
            /// </summary>
            public bool IsCallQueue;
            /// <summary>
            /// 仅程序集可见
            /// </summary>
            public string IsInternalClient
            {
                get
                {
                    return Attribute.GetIsInternalClient ? "internal " : "public ";
                }
            }
#if NOJIT
            /// <summary>
            /// 是否存在 AutoCSer.Net.TcpServer.ISetTcpServer 接口函数
            /// </summary>
            protected bool isSetTcpServer
            {
                get
                {
                    return typeof(AutoCSer.Net.TcpServer.ISetTcpServer).IsAssignableFrom(Type.Type)
                        || Type.Type.GetMethod("SetTcpServer", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(AutoCSer.Net.TcpServer.CommandBase) }, null) != null;
                }
            }
#endif

            /// <summary>
            /// 命令序号记忆数据
            /// </summary>
            protected static readonly Dictionary<HashString, int> nullRememberIdentityName = AutoCSer.DictionaryCreator.CreateHashString<int>();
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
            /// <summary>
            /// 获取简单序列化方法集合
            /// </summary>
            /// <typeparam name="tcpMethodType"></typeparam>
            /// <param name="methods"></param>
            /// <returns></returns>
            protected static tcpMethodType[] getSimpleSerializeMethods<tcpMethodType>(tcpMethodType[] methods)
                where tcpMethodType : TcpMethod
            {
                Dictionary<int, tcpMethodType> typeIndexs = DictionaryCreator.CreateInt<tcpMethodType>();
                foreach (tcpMethodType method in methods)
                {
                    if (method.InputParameterIndex != 0 && method.IsSimpleSerializeInputParamter && !typeIndexs.ContainsKey(method.InputParameterIndex))
                    {
                        typeIndexs.Add(method.InputParameterIndex, method);
                    }
                }
                return typeIndexs.getArray(value => value.Value);
            }
            /// <summary>
            /// 简单反序列化方法集合
            /// </summary>
            protected static tcpMethodType[] getSimpleDeSerializeMethods<tcpMethodType>(tcpMethodType[] methods)
                where tcpMethodType : TcpMethod
            {
                Dictionary<int, tcpMethodType> typeIndexs = DictionaryCreator.CreateInt<tcpMethodType>();
                foreach (tcpMethodType method in methods)
                {
                    if (method.OutputParameterIndex != 0 && method.IsSimpleSerializeOutputParamter && !typeIndexs.ContainsKey(method.OutputParameterIndex))
                    {
                        typeIndexs.Add(method.OutputParameterIndex, method);
                    }
                }
                return typeIndexs.getArray(value => value.Value);
            }
            /// <summary>
            /// 二进制序列化方法集合
            /// </summary>
            protected static tcpMethodType[] getSerializeMethods<tcpMethodType>(tcpMethodType[] methods)
                where tcpMethodType : TcpMethod
            {
                Dictionary<int, tcpMethodType> typeIndexs = DictionaryCreator.CreateInt<tcpMethodType>();
                foreach (tcpMethodType method in methods)
                {
                    if (method.InputParameterIndex != 0 && !method.IsSimpleSerializeInputParamter && !method.IsJsonSerialize && !typeIndexs.ContainsKey(method.InputParameterIndex))
                    {
                        typeIndexs.Add(method.InputParameterIndex, method);
                    }
                }
                return typeIndexs.getArray(value => value.Value);
            }
            /// <summary>
            /// 二进制反序列化方法集合
            /// </summary>
            protected static tcpMethodType[] getDeSerializeMethods<tcpMethodType>(tcpMethodType[] methods)
                where tcpMethodType : TcpMethod
            {
                Dictionary<int, tcpMethodType> typeIndexs = DictionaryCreator.CreateInt<tcpMethodType>();
                foreach (tcpMethodType method in methods)
                {
                    if (method.OutputParameterIndex != 0 && !method.IsSimpleSerializeOutputParamter && !method.IsJsonSerialize && !typeIndexs.ContainsKey(method.OutputParameterIndex))
                    {
                        typeIndexs.Add(method.OutputParameterIndex, method);
                    }
                }
                return typeIndexs.getArray(value => value.Value);
            }
            /// <summary>
            /// JSON 序列化方法集合
            /// </summary>
            protected static tcpMethodType[] getJsonSerializeMethods<tcpMethodType>(tcpMethodType[] methods)
                where tcpMethodType : TcpMethod
            {
                Dictionary<int, tcpMethodType> typeIndexs = DictionaryCreator.CreateInt<tcpMethodType>();
                foreach (tcpMethodType method in methods)
                {
                    if (method.InputParameterIndex != 0 && method.IsJsonSerialize && !typeIndexs.ContainsKey(method.InputParameterIndex))
                    {
                        typeIndexs.Add(method.InputParameterIndex, method);
                    }
                }
                return typeIndexs.getArray(value => value.Value);
            }
            /// <summary>
            /// JSON 序列化方法集合
            /// </summary>
            protected static tcpMethodType[] getJsonDeSerializeMethods<tcpMethodType>(tcpMethodType[] methods)
                where tcpMethodType : TcpMethod
            {
                Dictionary<int, tcpMethodType> typeIndexs = DictionaryCreator.CreateInt<tcpMethodType>();
                foreach (tcpMethodType method in methods)
                {
                    if (method.OutputParameterIndex != 0 && method.IsJsonSerialize && !typeIndexs.ContainsKey(method.OutputParameterIndex))
                    {
                        typeIndexs.Add(method.OutputParameterIndex, method);
                    }
                }
                return typeIndexs.getArray(value => value.Value);
            }
        }
        /// <summary>
        /// TCP 服务代码生成
        /// </summary>
        /// <typeparam name="attributeType">TCP 服务配置</typeparam>
        /// <typeparam name="methodAttributeType">TCP 调用函数配置</typeparam>
        /// <typeparam name="serverSocketSenderType">TCP 服务套接字数据发送</typeparam>
        internal abstract class Generator<attributeType, methodAttributeType, serverSocketSenderType> : GeneratorBase<attributeType, methodAttributeType, serverSocketSenderType>
            where attributeType : AutoCSer.Net.TcpServer.ServerBaseAttribute
            where methodAttributeType : AutoCSer.Net.TcpServer.MethodAttribute
            where serverSocketSenderType : AutoCSer.Net.TcpServer.ServerSocketSenderBase
        {
            /// <summary>
            /// 方法索引信息
            /// </summary>
            public new sealed class TcpMethod : TcpMethod<TcpMethod>
            {
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
                /// 服务端创建输出是否开启线程
                /// </summary>
                public bool IsServerBuildOutputThread
                {
                    get
                    {
                        if (!Attribute.IsExpired)
                        {
                            if (IsAsynchronousCallback) return Attribute.IsServerAsynchronousCallbackBuildOutputThread;
                            if (IsMethodServerCall && Attribute.ServerTaskType == Net.TcpServer.ServerTaskType.ThreadPool) return false;
                        }
                        return true;
                    }
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
                    return CheckIdentity(methodIndexs, rememberIdentityCommand, getMethodKeyName, nullMethod);
                }
            }
            /// <summary>
            /// 方法索引集合
            /// </summary>
            public TcpMethod[] MethodIndexs;
            /// <summary>
            /// 简单序列化方法集合
            /// </summary>
            public TcpMethod[] SimpleSerializeMethods
            {
                get
                {
                    return getSimpleSerializeMethods(MethodIndexs);
                }
            }
            /// <summary>
            /// 简单反序列化方法集合
            /// </summary>
            public TcpMethod[] SimpleDeSerializeMethods
            {
                get
                {
                    return getSimpleDeSerializeMethods(MethodIndexs);
                }
            }
            /// <summary>
            /// 二进制序列化方法集合
            /// </summary>
            public TcpMethod[] SerializeMethods
            {
                get
                {
                    return getSerializeMethods(MethodIndexs);
                }
            }
            /// <summary>
            /// 二进制反序列化方法集合
            /// </summary>
            public TcpMethod[] DeSerializeMethods
            {
                get
                {
                    return getDeSerializeMethods(MethodIndexs);
                }
            }
            /// <summary>
            /// JSON 序列化方法集合
            /// </summary>
            public TcpMethod[] JsonSerializeMethods
            {
                get
                {
                    return getJsonSerializeMethods(MethodIndexs);
                }
            }
            /// <summary>
            /// JSON 序列化方法集合
            /// </summary>
            public TcpMethod[] JsonDeSerializeMethods
            {
                get
                {
                    return getJsonDeSerializeMethods(MethodIndexs);
                }
            }
            /// <summary>
            /// TCP 客户端路由类型
            /// </summary>
            public string ClientRouteType
            {
                get
                {
                    Type type = Attribute.ClientRouteType;
                    if (type != null)
                    {
                        if (typeof(AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalServer.ClientSocketSender>).IsAssignableFrom(type))
                        {
                            return type.fullName();
                        }
                        throw new Exception(type.fullName() + " 无法转换为 " + typeof(AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalServer.ClientSocketSender>).fullName());
                    }
                    return null;
                }
            }
            /// <summary>
            /// TCP 客户端路由类型
            /// </summary>
            public string OpenClientRouteType
            {
                get
                {
                    Type type = Attribute.ClientRouteType;
                    if (type != null)
                    {
                        if (typeof(AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpOpenServer.ClientSocketSender>).IsAssignableFrom(type))
                        {
                            return type.fullName();
                        }
                        throw new Exception(type.fullName() + " 无法转换为 " + typeof(AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpOpenServer.ClientSocketSender>).fullName());
                    }
                    return null;
                }
            }
        }
    }
}
