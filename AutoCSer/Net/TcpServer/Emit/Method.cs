using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// TCP 函数信息
    /// </summary>
    /// <typeparam name="methodType">TCP 函数信息类型</typeparam>
    /// <typeparam name="attributeType">TCP 服务配置类型</typeparam>
    /// <typeparam name="methodAttributeType">TCP 调用函数配置</typeparam>
    /// <typeparam name="serverSocketType">TCP 服务套接字</typeparam>
    internal abstract partial class Method<methodType, attributeType, methodAttributeType, serverSocketType>
        where methodType : Method<methodType, attributeType, methodAttributeType, serverSocketType>
        where attributeType : ServerBaseAttribute
        where methodAttributeType : TcpServer.MethodBaseAttribute
    {
        /// <summary>
        /// 接口类型
        /// </summary>
        internal readonly Type Type;
        /// <summary>
        /// 方法信息
        /// </summary>
        internal readonly MethodInfo MethodInfo;
        /// <summary>
        /// TCP 服务器端配置
        /// </summary>
        internal readonly attributeType ServerAttribute;
        /// <summary>
        /// TCP调用配置
        /// </summary>
        internal readonly methodAttributeType Attribute;
        /// <summary>
        /// 返回值类型
        /// </summary>
        internal readonly Type ReturnType;
        /// <summary>
        /// 真实返回值类型
        /// </summary>
        internal readonly Type ReturnValueType;
        /// <summary>
        /// 函数是否存在同步返回值
        /// </summary>
        internal bool IsReturnType
        {
            get { return ReturnValueType != null || ReturnType != typeof(void); }
        }
        /// <summary>
        /// 参数信息集合
        /// </summary>
        internal readonly ParameterInfo[] Parameters;
        /// <summary>
        /// 输出参数集合
        /// </summary>
        internal readonly ParameterInfo[] OutputParameters;
        /// <summary>
        /// 客户端标识参数
        /// </summary>
        internal readonly ParameterInfo ClientParameter;
        /// <summary>
        /// 是否异步回调方法
        /// </summary>
        internal readonly bool IsAsynchronousCallback;
        /// <summary>
        /// 验证方法是否支持异步
        /// </summary>
        internal virtual bool IsVerifyMethodAsynchronous { get { return false; } }
        /// <summary>
        /// 是否客户端异步回调方法
        /// </summary>
        internal readonly bool IsClientAsynchronousCallback;
        /// <summary>
        /// TCP 函数信息
        /// </summary>
        /// <param name="type">接口类型</param>
        /// <param name="method">方法信息</param>
        /// <param name="attribute">TCP 服务器端配置</param>
        /// <param name="serverMethodAttribute">TCP 调用函数配置</param>
        /// <param name="isClient">是否客户端</param>
        internal Method(Type type, MethodInfo method, attributeType attribute, methodAttributeType serverMethodAttribute, bool isClient)
        {
            Type = type;
            MethodInfo = method;
            ServerAttribute = attribute;
            Parameters = method.GetParameters();
            ReturnType = method.ReturnType;
            if (ReturnType == typeof(KeepCallback)) ReturnType = typeof(void);
            if (ReturnType == typeof(void) && Parameters.Length != 0)
            {
                Type parameterType = Parameters[Parameters.Length - 1].ParameterType;
                if (parameterType.IsGenericType)
                {
                    Type genericType = parameterType.GetGenericTypeDefinition();
                    if (genericType == typeof(Func<,>))
                    {
                        Type[] types = parameterType.GetGenericArguments();
                        if (types[1] == typeof(bool))
                        {
                            ReturnType = types[0];
                            IsAsynchronousCallback = true;
                        }
                    }
                    else if (isClient && genericType == typeof(Action<>))
                    {
                        ReturnType = parameterType.GetGenericArguments()[0];
                        IsAsynchronousCallback = IsClientAsynchronousCallback = true;
                    }
                }
            }
            if (ReturnType == typeof(ReturnValue))
            {
                ReturnValueType = ReturnType;
                ReturnType = typeof(void);
            }
            else if (ReturnType.IsGenericType && ReturnType.GetGenericTypeDefinition() == typeof(ReturnValue<>))
            {
                ReturnValueType = ReturnType;
                ReturnType = ReturnType.GetGenericArguments()[0];
            }
            else if (IsAsynchronousCallback)
            {
                ReturnType = typeof(void);
                IsAsynchronousCallback = IsClientAsynchronousCallback = false;
            }
            if (IsAsynchronousCallback) Parameters = new LeftArray<ParameterInfo> { Array = Parameters, Length = Parameters.Length - 1 }.GetArray();
            if (Parameters.Length != 0 && Parameters[0].ParameterType == typeof(serverSocketType))
            {
                ClientParameter = Parameters[0];
                Parameters = new SubArray<ParameterInfo> { Array = Parameters, Start = 1, Length = Parameters.Length - 1 }.GetArray();
            }
            foreach (methodAttributeType methodAttribute in method.GetCustomAttributes(typeof(methodAttributeType), false))
            {
                Attribute = methodAttribute;
                break;
            }
            if (Attribute == null) Attribute = AutoCSer.MemberCopy.Copyer<methodAttributeType>.MemberwiseClone(serverMethodAttribute);
            OutputParameters = Parameters.getFindArray(value => value.ParameterType.IsByRef);
        }
        /// <summary>
        /// 属性信息
        /// </summary>
        internal readonly PropertyInfo PropertyInfo;
        /// <summary>
        /// 是否属性设置 TCP 函数信息
        /// </summary>
        internal readonly bool IsPropertySetMethod;
        /// <summary>
        /// TCP 函数信息
        /// </summary>
        /// <param name="type">接口类型</param>
        /// <param name="property">属性信息</param>
        /// <param name="attribute">TCP 服务器端配置</param>
        /// <param name="serverMethodAttribute">TCP 调用函数配置</param>
        /// <param name="isSet">是否设置</param>
        internal Method(Type type, PropertyInfo property, attributeType attribute, methodAttributeType serverMethodAttribute, bool isSet)
        {
            Type = type;
            PropertyInfo = property;
            MethodInfo = (IsPropertySetMethod = isSet) ? property.GetSetMethod() : property.GetGetMethod();
            ServerAttribute = attribute;
            Parameters = MethodInfo.GetParameters();
            ReturnType = MethodInfo.ReturnType;
            if (ReturnType == typeof(ReturnValue))
            {
                ReturnValueType = ReturnType;
                ReturnType = typeof(void);
            }
            else if (ReturnType.IsGenericType && ReturnType.GetGenericTypeDefinition() == typeof(ReturnValue<>))
            {
                ReturnValueType = ReturnType;
                ReturnType = ReturnType.GetGenericArguments()[0];
            }
            if (Parameters.Length != 0 && Parameters[0].ParameterType == typeof(serverSocketType))
            {
                ClientParameter = Parameters[0];
                Parameters = new SubArray<ParameterInfo> { Array = Parameters, Start = 1, Length = Parameters.Length - 1 }.GetArray();
            }
            foreach (methodAttributeType methodAttribute in PropertyInfo.GetCustomAttributes(typeof(methodAttributeType), false))
            {
                Attribute = methodAttribute;
                if (isSet)
                {
                    Attribute = AutoCSer.MemberCopy.Copyer<methodAttributeType>.MemberwiseClone(Attribute);
                    Attribute.CommandIdentity = int.MinValue;
                }
                break;
            }
            if (Attribute == null) Attribute = AutoCSer.MemberCopy.Copyer<methodAttributeType>.MemberwiseClone(serverMethodAttribute);
            OutputParameters = NullValue<ParameterInfo>.Array;
        }
        /// <summary>
        /// 属性设置 TCP 函数信息
        /// </summary>
        internal methodType PropertySetMethod;
        /// <summary>
        /// 属性获取 TCP 函数信息
        /// </summary>
        internal methodType PropertyGetMethod;
        /// <summary>
        /// 属性创建
        /// </summary>
        internal PropertyBuilder PropertyBuilder;
        /// <summary>
        /// 服务端任务类型
        /// </summary>
        internal ServerTaskType ServerTask
        {
            get
            {
                //ServerTaskType taskType = Attribute.ServerTask;
                //return taskType == ServerTaskType.Queue && !ServerAttribute.IsCallQueue ? ServerTaskType.TcpQueue : taskType;
                return Attribute.ServerTaskType;
            }
        }
        /// <summary>
        /// 是否定义服务器端调用
        /// </summary>
        internal bool IsMethodServerCall
        {
            get
            {
                return !IsAsynchronousCallback && Attribute.ServerTaskType != ServerTaskType.Synchronous;
            }
        }
        /// <summary>
        /// 是否使用 JSON 序列化
        /// </summary>
        internal bool IsJsonSerialize
        {
            get { return ServerAttribute.GetIsJsonSerialize ^ !Attribute.IsServerSerialize; }
        }
        /// <summary>
        /// 输出参数类型
        /// </summary>
        internal ParameterType OutputParameterType;
        /// <summary>
        /// 参数类型
        /// </summary>
        internal ParameterType ParameterType;
        /// <summary>
        /// 引用参数检测
        /// </summary>
        /// <param name="errorString"></param>
        /// <returns></returns>
        internal abstract bool CheckRef(ref string errorString);
        /// <summary>
        /// 是否验证方法
        /// </summary>
        /// <param name="errorString">错误字符串提示信息</param>
        /// <returns></returns>
        internal bool CheckIsVerifyMethod(ref string errorString)
        {
            if (Attribute.IsVerifyMethod)
            {
                if (PropertyInfo != null) errorString = "验证方法不支持属性 " + PropertyInfo.Name;
                else if (IsAsynchronousCallback && !IsVerifyMethodAsynchronous) errorString = "方法 " + MethodInfo.Name + " 为异步回调方法,不符合验证方法要求";
                else if (ReturnType != typeof(bool)) errorString = "方法 " + MethodInfo.Name + " 的返回值类型为 " + ReturnType.fullName() + " ,不符合验证方法要求";
                else if (ParameterType == null) errorString = "方法 " + MethodInfo.Name + " 没有输入参数,不符合验证方法要求";
                //else if (Parameters[0].ParameterType != typeof(clientSocketSenderType)) errorString = "方法 " + MethodInfo.Name + " 的第一个参数类型不是 " + typeof(clientSocketSenderType).fullName() + " ,不符合验证方法要求";
                else return true;
            }
            return false;
        }
        /// <summary>
        /// 设置参数类型
        /// </summary>
        internal void SetParameterType()
        {
            if (Parameters.Length != 0)
            {
                ParameterInfo[] inputParamters = Parameters.getFindArray(value => ParameterType.IsInputParameter(value));
                if (inputParamters.Length != 0) ParameterType = Emit.ParameterType.Get(inputParamters, typeof(void), Attribute.IsInputSerializeReferenceMember, Attribute.IsInputSerializeBox);
            }
            if (ReturnType != typeof(void) || OutputParameters.Length != 0)
            {
                OutputParameterType = Emit.ParameterType.Get(OutputParameters, ReturnType, Attribute.IsOutputSerializeReferenceMember, Attribute.IsOutputSerializeBox);
            }
        }
        /// <summary>
        /// 执行命令 Switch
        /// </summary>
        internal Label DoCommandLabel;

        /// <summary>
        /// 检测方法序号
        /// </summary>
        /// <param name="methodIndexs">方法集合</param>
        /// <param name="errorString">错误字符串提示信息</param>
        /// <returns>方法集合,失败返回null</returns>
        internal static methodType[] CheckIdentity(ref LeftArray<methodType> methodIndexs, ref string errorString)
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
                        errorString = "命令序号重复 " + method.MethodInfo.Name + " [" + identity.toString() + "] " + identityMethod.MethodInfo.Name;
                        return null;
                    }
                    identitys.Add(identity, method);
                    if (identity > maxIdentity) maxIdentity = identity;
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
            return sortMethodIndexs;
        }
        /// <summary>
        /// TCP 函数排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static int Compare(methodType left, methodType right)
        {
            if (left.Type == right.Type)
            {
                if (left.PropertyInfo == null)
                {
                    if (right.PropertyInfo != null) return 1;
                    int value = string.CompareOrdinal(left.MethodInfo.Name, right.MethodInfo.Name);
                    if (value == 0)
                    {
                        value = left.Parameters.Length - right.Parameters.Length;
                        if (value == 0)
                        {
                            value = left.OutputParameters.Length - right.OutputParameters.Length;
                            if (value == 0)
                            {
                                value = left.ReturnType == right.ReturnType ? 0 : string.CompareOrdinal(left.ReturnType.Name, right.ReturnType.Name);
                                if (value == 0)
                                {
                                    value = compare(left.Parameters, right.Parameters);
                                    if (value == 0) return compare(left.OutputParameters, right.OutputParameters);
                                }
                            }
                        }
                    }
                    return value;
                }
                else
                {
                    if (right.PropertyInfo == null) return -1;
                    if (left.IsPropertySetMethod)
                    {
                        if (!right.IsPropertySetMethod) return 1;
                    }
                    else
                    {
                        if (right.IsPropertySetMethod) return -1;
                    }
                    int value = string.CompareOrdinal(left.PropertyInfo.Name, right.PropertyInfo.Name);
                    if (value == 0)
                    {
                        value = left.Parameters.Length - right.Parameters.Length;
                        if (value == 0) return compare(left.Parameters, right.Parameters);
                    }
                    return value;
                }
            }
            return string.CompareOrdinal(left.Type.Name, right.Type.Name);
        }
        /// <summary>
        /// 参数排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int compare(ParameterInfo[] left, ParameterInfo[] right)
        {
            int index = 0;
            foreach (ParameterInfo parameter in left)
            {
                int value = compare(parameter, right[index]);
                if (value != 0) return value;
                ++index;
            }
            return 0;
        }
        /// <summary>
        /// 参数排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static int compare(ParameterInfo left, ParameterInfo right)
        {
            int value = string.CompareOrdinal(left.Name, right.Name);
            return value == 0 ? (left.ParameterType == right.ParameterType ? 0 : string.CompareOrdinal(left.ParameterType.Name, right.ParameterType.Name)) : value;
        }
    }
    /// <summary>
    /// TCP 函数信息
    /// </summary>
    /// <typeparam name="attributeType">TCP 服务配置类型</typeparam>
    /// <typeparam name="methodAttributeType">TCP 调用函数配置</typeparam>
    /// <typeparam name="serverSocketSenderType">TCP 服务套接字数据发送</typeparam>
    internal sealed partial class Method<attributeType, methodAttributeType, serverSocketSenderType> : Method<Method<attributeType, methodAttributeType, serverSocketSenderType>, attributeType, methodAttributeType, serverSocketSenderType>
        where attributeType : ServerBaseAttribute
        where methodAttributeType : TcpServer.MethodAttribute
        where serverSocketSenderType : TcpServer.ServerSocketSender
    {
        /// <summary>
        /// 服务端需要应答客户端请求
        /// </summary>
        internal readonly bool IsClientSendOnly;
        /// <summary>
        /// TCP 函数信息
        /// </summary>
        /// <param name="type">接口类型</param>
        /// <param name="method">方法信息</param>
        /// <param name="attribute">TCP 服务器端配置</param>
        /// <param name="serverMethodAttribute">TCP 调用函数配置</param>
        /// <param name="isClient">是否客户端</param>
        internal Method(Type type, MethodInfo method, attributeType attribute, methodAttributeType serverMethodAttribute, bool isClient)
            : base(type, method, attribute, serverMethodAttribute, isClient)
        {
            IsClientSendOnly = Attribute.GetIsClientSendOnly && !IsAsynchronousCallback && ReturnType == typeof(void) && ClientParameter == null;
        }
        /// <summary>
        /// TCP 函数信息
        /// </summary>
        /// <param name="type">接口类型</param>
        /// <param name="property">属性信息</param>
        /// <param name="attribute">TCP 服务器端配置</param>
        /// <param name="serverMethodAttribute">TCP 调用函数配置</param>
        /// <param name="isSet">是否设置</param>
        internal Method(Type type, PropertyInfo property, attributeType attribute, methodAttributeType serverMethodAttribute, bool isSet = false)
            : base(type, property, attribute, serverMethodAttribute, isSet)
        {
            if (!isSet && property.CanWrite) PropertySetMethod = new Method<attributeType, methodAttributeType, serverSocketSenderType>(this, serverMethodAttribute);
            IsClientSendOnly = Attribute.GetIsClientSendOnly && ReturnType == typeof(void) && ClientParameter == null;
        }
        /// <summary>
        /// TCP 函数信息
        /// </summary>
        /// <param name="propertyGetMethod">属性获取 TCP 函数信息</param>
        /// <param name="serverMethodAttribute">TCP 调用函数配置</param>
        private Method(Method<attributeType, methodAttributeType, serverSocketSenderType> propertyGetMethod, methodAttributeType serverMethodAttribute)
            : this(propertyGetMethod.Type, propertyGetMethod.PropertyInfo, propertyGetMethod.ServerAttribute, serverMethodAttribute, true)
        {
            PropertyGetMethod = propertyGetMethod;
        }
        /// <summary>
        /// 是否保持异步回调
        /// </summary>
        internal bool IsKeepCallback
        {
            get
            {
                return IsAsynchronousCallback && !Attribute.IsVerifyMethod && MethodInfo.ReturnType == typeof(KeepCallback);
            }
        }
        /// <summary>
        /// 创建输出是否开启线程
        /// </summary>
        internal bool IsServerBuildOutputThread
        {
            get
            {
                if (!Attribute.IsExpired)
                {
                    if (IsAsynchronousCallback) return Attribute.IsServerAsynchronousCallbackBuildOutputThread;
                    if (IsMethodServerCall && Attribute.ServerTaskType == ServerTaskType.ThreadPool) return false;
                }
                return true;
            }
        }
        /// <summary>
        /// 引用参数检测
        /// </summary>
        /// <param name="errorString"></param>
        /// <returns></returns>
        internal override bool CheckRef(ref string errorString)
        {
            if (IsAsynchronousCallback || IsClientSendOnly)
            {
                foreach (ParameterInfo parameter in Parameters)
                {
                    if (parameter.ParameterType.IsByRef)
                    {
                        errorString = "异步/仅应答 方法 " + MethodInfo.Name + " 不支持 ref / out 参数 " + parameter.Name;
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
