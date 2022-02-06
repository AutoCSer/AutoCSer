using System;
using System.Reflection;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;

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
        /// TCP 客户端等待连接初始化检测函数信息
        /// </summary>
        internal readonly MethodInfo MethodClientCheckWaitConnectedMethod;
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
#if !DOTNET2 && !DOTNET4 && !UNITY3D
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
        /// <param name="clientSocketSenderGetAwaiterMethod">TCP 调用函数信息</param>
        internal ClientMetadataBase(Type clientType, Type senderType, Type methodClientType
            , MethodInfo clientGetSenderMethod, Func<Type, ParameterGenericType> getParameterGenericType, Func<Type, Type, ParameterGenericType2> getParameterGenericType2
            , MethodInfo clientSocketSenderWaitCallMethod, MethodInfo clientSocketSenderCallOnlyMethod
            , MethodInfo clientSocketSenderGetAwaiterMethod
            )
#else
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
            , MethodInfo clientSocketSenderWaitCallMethod, MethodInfo clientSocketSenderCallOnlyMethod
            )
#endif
        {
            SenderType = senderType;
            MethodClientType = methodClientType;
            ClientTypeName = clientType.fullName();
            MethodClientGetTcpClientMethod = methodClientType.GetProperty(TcpClientName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetGetMethod();
            MethodClientCheckWaitConnectedMethod = methodClientType.GetMethod("_CheckWaitConnected_", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            //ClientGetSenderMethod = clientType.GetProperty("Sender", BindingFlags.Public | BindingFlags.Instance).GetGetMethod();
            ClientGetSenderMethod = clientGetSenderMethod;
            GetParameterGenericType = getParameterGenericType;
            GetParameterGenericType2 = getParameterGenericType2;
            ClientSocketSenderWaitCallMethod = clientSocketSenderWaitCallMethod;
            ClientSocketSenderCallOnlyMethod = clientSocketSenderCallOnlyMethod;
#if !DOTNET2 && !DOTNET4 && !UNITY3D
            ClientSocketSenderGetAwaiterMethod = clientSocketSenderGetAwaiterMethod;
#endif
        }

#if !DOTNET2 && !DOTNET4 && !UNITY3D
        /// <summary>
        /// TCP 调用函数信息
        /// </summary>
        internal readonly MethodInfo ClientSocketSenderGetAwaiterMethod;

        /// <summary>
        /// 创建异步等待
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Awaiter CreateAwaiter()
        {
            return new Awaiter();
        }
        /// <summary>
        /// 异步等待构造函数
        /// </summary>
        internal static readonly MethodInfo CreateAwaiterMethod = ((Func<Awaiter>)CreateAwaiter).Method;
        /// <summary>
        /// 设置错误返回值类型
        /// </summary>
        internal static readonly MethodInfo AwaiterCallReturnTypeMethod = ((Action<Awaiter, ReturnType>)Awaiter.Call).Method;
#endif
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
#if !DOTNET2 && !DOTNET4 && !UNITY3D
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
        /// <param name="clientSocketSenderGetAwaiterMethod">TCP 调用函数信息</param>
        internal ClientMetadata(Type clientType, Type senderType, Type methodClientType
            , MethodInfo clientGetSenderMethod, Func<Type, Type, ReturnParameterGenericType> getOutputParameterGenericType
            , Func<Type, ParameterGenericType> getParameterGenericType, Func<Type, Type, ParameterGenericType2> getParameterGenericType2
            , MethodInfo clientSocketSenderWaitCallMethod, MethodInfo clientSocketSenderCallOnlyMethod
            , MethodInfo clientSocketSenderCallMethod, MethodInfo clientSocketSenderCallKeepMethod
            , MethodInfo clientSocketSenderGetAwaiterMethod
            )
#else
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
            , MethodInfo clientSocketSenderCallMethod, MethodInfo clientSocketSenderCallKeepMethod
            )
#endif
            : base(clientType, senderType, methodClientType
            , clientGetSenderMethod, getParameterGenericType, getParameterGenericType2
            , clientSocketSenderWaitCallMethod, clientSocketSenderCallOnlyMethod
#if !DOTNET2 && !DOTNET4 && !UNITY3D
            , clientSocketSenderGetAwaiterMethod
#endif
                  )
        {
            GetOutputParameterGenericType = getOutputParameterGenericType;
            ClientSocketSenderCallMethod = clientSocketSenderCallMethod;
            ClientSocketSenderCallKeepMethod = clientSocketSenderCallKeepMethod;
        }

        /// <summary>
        /// TCP 客户端回调转换函数信息
        /// </summary>
        internal static readonly MethodInfo ClientCallbackGetMethod = ((Func<Func<ReturnValue, bool>, Action<ReturnValue>>)ClientCallback.Get).Method;
        /// <summary>
        /// 同步等待调用添加节点函数信息
        /// </summary>
        internal static readonly MethodInfo AutoWaitReturnValuePushNotNullMethod = ((Action<AutoWaitReturnValue>)AutoWaitReturnValue.PushNotNull).Method;
        /// <summary>
        /// TCP 服务客户端获取同步等待调用函数信息
        /// </summary>
        internal static readonly MethodInfo AutoWaitReturnValuePopMethod = ((Func<AutoWaitReturnValue>)AutoWaitReturnValue.Pop).Method;
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
