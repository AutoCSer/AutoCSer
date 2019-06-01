using System;
using System.Reflection;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// TCP 客户端元数据
    /// </summary>
    [AutoCSer.IOS.Preserve(AllMembers = false)]
    internal class ClientMetadataBase
    {
        /// <summary>
        /// TCP 服务客户端属性名称
        /// </summary>
        internal const string TcpClientName = "_TcpClient_";
        /// <summary>
        /// TCP 客户端套接字发送数据类型
        /// </summary>
        internal readonly Type SenderType;
        /// <summary>
        /// TCP 客户端基类类型
        /// </summary>
        internal readonly Type MethodClientType;
        /// <summary>
        /// TCP 客户端类型名称
        /// </summary>
        internal readonly string ClientTypeName;
        /// <summary>
        /// TCP 客户端基类获取 TCP 客户端属性函数信息
        /// </summary>
        internal readonly MethodInfo MethodClientGetTcpClientMethod;
        /// <summary>
        /// TCP 服务客户端同步调用套接字发送对象函数信息
        /// </summary>
        internal readonly MethodInfo ClientGetSenderMethod;
        /// <summary>
        /// TCP 服务客户端调用函数信息
        /// </summary>
        internal readonly MethodInfo ClientSocketSenderCallOnlyMethod;
        ///// <summary>
        ///// TCP 服务客户端调用函数信息
        ///// </summary>
        //internal readonly MethodInfo ClientSocketSenderCallOnlyInputMethod;
        /// <summary>
        /// TCP 服务客户端调用函数信息
        /// </summary>
        internal readonly MethodInfo ClientSocketSenderWaitCallMethod;
        ///// <summary>
        ///// TCP 服务客户端调用函数信息
        ///// </summary>
        //internal readonly MethodInfo ClientSocketSenderWaitCallInputMethod;
        ///// <summary>
        ///// TCP 服务客户端调用函数信息
        ///// </summary>
        //internal readonly MethodInfo ClientSocketSenderWaitGetMethod;
        ///// <summary>
        ///// TCP 服务客户端调用函数信息
        ///// </summary>
        //internal readonly MethodInfo ClientSocketSenderWaitGetInputMethod;
        /// <summary>
        /// 输出参数泛型类型元数据
        /// </summary>
        internal readonly Func<Type, ParameterGenericType> GetParameterGenericType;
        /// <summary>
        /// 输入+输出参数泛型类型元数据
        /// </summary>
        internal readonly Func<Type, Type, ParameterGenericType2> GetParameterGenericType2;
        /// <summary>
        /// TCP 客户端元数据
        /// </summary>
        /// <param name="clientType">TCP 客户端类型</param>
        /// <param name="senderType">TCP 客户端套接字发送数据类型</param>
        /// <param name="methodClientType">TCP 客户端基类类型</param>
        /// <param name="clientGetSenderMethod">TCP 服务客户端同步调用套接字发送对象函数信息</param>
        /// <param name="getParameterGenericType">输出参数泛型类型元数据</param>
        /// <param name="getParameterGenericType2">输入+输出参数泛型类型元数据</param>
        /// <param name="clientSocketSenderWaitCallMethod">TCP 服务客户端调用函数信息</param>
        /// <param name="clientSocketSenderCallOnlyMethod">TCP 服务客户端调用函数信息</param>
        internal ClientMetadataBase(Type clientType, Type senderType, Type methodClientType
            , MethodInfo clientGetSenderMethod, Func<Type, ParameterGenericType> getParameterGenericType, Func<Type, Type, ParameterGenericType2> getParameterGenericType2
            , MethodInfo clientSocketSenderWaitCallMethod, MethodInfo clientSocketSenderCallOnlyMethod)
        {
            SenderType = senderType;
            MethodClientType = methodClientType;
            ClientTypeName = clientType.fullName();
            MethodClientGetTcpClientMethod = methodClientType.GetProperty(TcpClientName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetGetMethod();
            //ClientGetSenderMethod = clientType.GetProperty("Sender", BindingFlags.Public | BindingFlags.Instance).GetGetMethod();
            ClientGetSenderMethod = clientGetSenderMethod;
            GetParameterGenericType = getParameterGenericType;
            GetParameterGenericType2 = getParameterGenericType2;
            ClientSocketSenderWaitCallMethod = clientSocketSenderWaitCallMethod;
            ClientSocketSenderCallOnlyMethod = clientSocketSenderCallOnlyMethod;
            //Type autoWaitReturnValueRefType = typeof(AutoWaitReturnValue).MakeByRefType();
            //foreach (MethodInfo method in senderType.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            //{
            //    switch (method.Name)
            //    {
            //        case "WaitGet":
            //            if (method.ReturnType == typeof(ReturnType))
            //            {
            //                ParameterInfo[] types = method.GetParameters();
            //                if (types.Length >= 3 && types[0].ParameterType == typeof(CommandInfo))
            //                {
            //                    switch (types.Length)
            //                    {
            //                        case 3: ClientSocketSenderWaitGetMethod = method; break;
            //                        case 4: ClientSocketSenderWaitGetInputMethod = method; break;
            //                    }
            //                }
            //            }
            //            break;
            //        case "WaitCall":
            //            if (method.ReturnType == typeof(ReturnType))
            //            {
            //                ParameterInfo[] types = method.GetParameters();
            //                if (types.Length >= 2 && types[0].ParameterType == typeof(CommandInfo) && types[1].ParameterType == autoWaitReturnValueRefType)
            //                {
            //                    switch (types.Length)
            //                    {
            //                        case 2: ClientSocketSenderWaitCallMethod = method; break;
            //                        case 3: ClientSocketSenderWaitCallInputMethod = method; break;
            //                    }
            //                }
            //            }
            //            break;
            //        case "CallOnly":
            //            if (method.ReturnType == typeof(void))
            //            {
            //                ParameterInfo[] types = method.GetParameters();
            //                switch (types.Length)
            //                {
            //                    case 1:
            //                        if (types[0].ParameterType == typeof(CommandInfo)) ClientSocketSenderCallOnlyMethod = method;
            //                        break;
            //                    case 2:
            //                        if (types[0].ParameterType == typeof(CommandInfo) && types[1].ParameterType.IsByRef) ClientSocketSenderCallOnlyInputMethod = method;
            //                        break;
            //                }
            //            }
            //            break;
            //    }
            //}
        }
    }
    /// <summary>
    /// TCP 客户端元数据
    /// </summary>
    [AutoCSer.IOS.Preserve(AllMembers = false)]
    internal abstract class ClientMetadata : ClientMetadataBase
    {
        /// <summary>
        /// TCP 服务客户端调用函数信息
        /// </summary>
        internal readonly MethodInfo ClientSocketSenderCallMethod;
        ///// <summary>
        ///// TCP 服务客户端调用函数信息
        ///// </summary>
        //internal readonly MethodInfo ClientSocketSenderCallInputMethod;
        /// <summary>
        /// TCP 服务客户端调用函数信息
        /// </summary>
        internal readonly MethodInfo ClientSocketSenderCallKeepMethod;
        ///// <summary>
        ///// TCP 服务客户端调用函数信息
        ///// </summary>
        //internal readonly MethodInfo ClientSocketSenderCallKeepInputMethod;
        ///// <summary>
        ///// TCP 服务客户端调用函数信息
        ///// </summary>
        //internal readonly MethodInfo ClientSocketSenderGetAsynchronousMethod;
        ///// <summary>
        ///// TCP 服务客户端调用函数信息
        ///// </summary>
        //internal readonly MethodInfo ClientSocketSenderGetInputAsynchronousMethod;
        ///// <summary>
        ///// TCP 服务客户端调用函数信息
        ///// </summary>
        //internal readonly MethodInfo ClientSocketSenderGetKeepAsynchronousMethod;
        ///// <summary>
        ///// TCP 服务客户端调用函数信息
        ///// </summary>
        //internal readonly MethodInfo ClientSocketSenderGetKeepInputAsynchronousMethod;
        ///// <summary>
        ///// TCP 服务客户端获取异步回调函数信息
        ///// </summary>
        //internal readonly MethodInfo ClientGetCallbackMethod;
        /// <summary>
        /// 获取输出参数泛型类型元数据
        /// </summary>
        internal readonly Func<Type, Type, ReturnParameterGenericType> GetOutputParameterGenericType;
        /// <summary>
        /// TCP 客户端元数据
        /// </summary>
        /// <param name="clientType">TCP 客户端类型</param>
        /// <param name="senderType">TCP 客户端套接字发送数据类型</param>
        /// <param name="methodClientType">TCP 客户端基类类型</param>
        /// <param name="clientGetSenderMethod">TCP 服务客户端同步调用套接字发送对象函数信息</param>
        /// <param name="getOutputParameterGenericType">获取输出参数泛型类型元数据</param>
        /// <param name="getParameterGenericType">输出参数泛型类型元数据</param>
        /// <param name="getParameterGenericType2">输入+输出参数泛型类型元数据</param>
        /// <param name="clientSocketSenderWaitCallMethod">TCP 服务客户端调用函数信息</param>
        /// <param name="clientSocketSenderCallOnlyMethod">TCP 服务客户端调用函数信息</param>
        /// <param name="clientSocketSenderCallMethod">TCP 服务客户端调用函数信息</param>
        /// <param name="clientSocketSenderCallKeepMethod">TCP 服务客户端调用函数信息</param>
        internal ClientMetadata(Type clientType, Type senderType, Type methodClientType
            , MethodInfo clientGetSenderMethod, Func<Type, Type, ReturnParameterGenericType> getOutputParameterGenericType
            , Func<Type, ParameterGenericType> getParameterGenericType, Func<Type, Type, ParameterGenericType2> getParameterGenericType2
            , MethodInfo clientSocketSenderWaitCallMethod, MethodInfo clientSocketSenderCallOnlyMethod
            , MethodInfo clientSocketSenderCallMethod, MethodInfo clientSocketSenderCallKeepMethod)
            : base(clientType, senderType, methodClientType
            , clientGetSenderMethod, getParameterGenericType, getParameterGenericType2
            , clientSocketSenderWaitCallMethod, clientSocketSenderCallOnlyMethod)
        {
            GetOutputParameterGenericType = getOutputParameterGenericType;
            ClientSocketSenderCallMethod = clientSocketSenderCallMethod;
            ClientSocketSenderCallKeepMethod = clientSocketSenderCallKeepMethod;
            //foreach (MethodInfo method in clientType.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            //{
            //    switch (method.Name)
            //    {
            //        case "GetCallback":
            //            if (method.ReturnType.IsGenericType && method.GetParameters().Length == 1) ClientGetCallbackMethod = method;
            //            break;
            //    }
            //}
            //Type autoWaitReturnValueRefType = typeof(AutoWaitReturnValue).MakeByRefType();
            //foreach (MethodInfo method in senderType.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            //{
            //    switch (method.Name)
            //    {
            //        case "Get":
            //            if (method.ReturnType == typeof(void))
            //            {
            //                ParameterInfo[] types = method.GetParameters();
            //                if (types.Length >= 2 && types[0].ParameterType == typeof(CommandInfo) && types[1].ParameterType.IsByRef)
            //                {
            //                    switch (types.Length)
            //                    {
            //                        case 2:
            //                            if (method.GetGenericArguments().Length == 1) ClientSocketSenderGetAsynchronousMethod = method;
            //                            break;
            //                        case 3:
            //                            if (types[2].ParameterType.IsByRef && method.GetGenericArguments().Length == 2) ClientSocketSenderGetInputAsynchronousMethod = method;
            //                            break;
            //                    }
            //                }
            //            }
            //            break;
            //        case "GetKeep":
            //            if (method.ReturnType == typeof(KeepCallback))
            //            {
            //                ParameterInfo[] types = method.GetParameters();
            //                if (types.Length >= 2 && types[0].ParameterType == typeof(CommandInfo) && types[1].ParameterType.IsByRef)
            //                {
            //                    switch (types.Length)
            //                    {
            //                        case 2:
            //                            if (method.GetGenericArguments().Length == 1) ClientSocketSenderGetKeepAsynchronousMethod = method;
            //                            break;
            //                        case 3:
            //                            if (types[2].ParameterType.IsByRef && method.GetGenericArguments().Length == 2) ClientSocketSenderGetKeepInputAsynchronousMethod = method;
            //                            break;
            //                    }
            //                }
            //            }
            //            break;
            //        case "Call":
            //            if (method.ReturnType == typeof(void))
            //            {
            //                ParameterInfo[] types = method.GetParameters();
            //                if (types.Length >= 2 && types[0].ParameterType == typeof(CommandInfo) && types[1].ParameterType == typeof(Action<ReturnValue>))
            //                {
            //                    switch (types.Length)
            //                    {
            //                        case 2: ClientSocketSenderCallMethod = method; break;
            //                        case 3: ClientSocketSenderCallInputMethod = method; break;
            //                    }
            //                }
            //            }
            //            break;
            //        case "CallKeep":
            //            if (method.ReturnType == typeof(KeepCallback))
            //            {
            //                ParameterInfo[] types = method.GetParameters();
            //                if (types.Length >= 2 && types[0].ParameterType == typeof(CommandInfo) && types[1].ParameterType == typeof(Action<ReturnValue>))
            //                {
            //                    switch (types.Length)
            //                    {
            //                        case 2: ClientSocketSenderCallKeepMethod = method; break;
            //                        case 3: ClientSocketSenderCallKeepInputMethod = method; break;
            //                    }
            //                }
            //            }
            //            break;
            //    }
            //}
        }

        /// <summary>
        /// TCP 客户端回调转换函数信息
        /// </summary>
        internal static readonly MethodInfo ClientCallbackGetMethod = ((Func<Func<ReturnValue, bool>, Action<ReturnValue>>)ClientCallback.Get).Method;// typeof(ClientCallback).GetMethod("Get", BindingFlags.Public | BindingFlags.Static);
        /// <summary>
        /// 同步等待调用添加节点函数信息
        /// </summary>
        internal static readonly MethodInfo AutoWaitReturnValuePushNotNullMethod = ((Action<AutoWaitReturnValue>)AutoWaitReturnValue.PushNotNull).Method;// typeof(AutoWaitReturnValue).GetMethod("PushNotNull", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(AutoCSer.Net.TcpServer.AutoWaitReturnValue) }, null);
        /// <summary>
        /// TCP 服务客户端获取同步等待调用函数信息
        /// </summary>
        internal static readonly MethodInfo AutoWaitReturnValuePopMethod = ((Func<AutoWaitReturnValue>)AutoWaitReturnValue.Pop).Method;//typeof(AutoWaitReturnValue).GetMethod("Pop", BindingFlags.Static | BindingFlags.Public);
        /// <summary>
        /// 返回值类型字段信息
        /// </summary>
        internal static readonly FieldInfo ReturnValueTypeField = typeof(ReturnValue).GetField("Type", BindingFlags.Instance | BindingFlags.Public);
        /// <summary>
        /// 返回值类型字符串集合
        /// </summary>
        [AutoCSer.IOS.Preserve]
        internal static readonly string[] ReturnTypeStrings;
        /// <summary>
        /// 返回值类型字符串集合字段信息
        /// </summary>
        internal static readonly FieldInfo ReturnTypeStringsField = typeof(ClientMetadata).GetField("ReturnTypeStrings", BindingFlags.Static | BindingFlags.NonPublic);

        static ClientMetadata()
        {
            ReturnTypeStrings = new string[AutoCSer.EnumAttribute<TcpServer.ReturnType>.GetMaxValue(-1) + 1];
            foreach (TcpServer.ReturnType returnType in System.Enum.GetValues(typeof(TcpServer.ReturnType))) ReturnTypeStrings[(byte)returnType] = returnType.ToString();
        }
    }
}
