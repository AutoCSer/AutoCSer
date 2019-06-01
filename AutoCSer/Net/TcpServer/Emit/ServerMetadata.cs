using System;
using System.Reflection;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// TCP 服务端元数据
    /// </summary>
    internal abstract class ServerMetadataBase
    {
        /// <summary>
        /// 服务端类型
        /// </summary>
        internal readonly Type ServerType;
        /// <summary>
        /// TCP 服务套接字发送数据类型
        /// </summary>
        internal readonly Type SenderType;
        /// <summary>
        /// TCP 服务器端同步调用类型
        /// </summary>
        internal readonly Type ServerCallType;
        /// <summary>
        /// 服务端类型名称
        /// </summary>
        internal readonly string ServerTypeName;
        /// <summary>
        /// TCP 服务器端同步调用类型名称
        /// </summary>
        internal readonly string ServerCallTypeName;
        /// <summary>
        /// TCP 服务器端同步调用套接字发送对象字段信息
        /// </summary>
        internal readonly FieldInfo ServerCallSenderField;
        /// <summary>
        /// TCP 服务器端同步调用套接字服务器目标对象字段信息
        /// </summary>
        internal readonly FieldInfo ServerCallServerValueField;
        /// <summary>
        /// TCP 服务器端同步调用入池函数信息
        /// </summary>
        internal readonly MethodInfo ServerCallPushMethod;
        /// <summary>
        /// TCP 服务器端同步调用函数信息
        /// </summary>
        internal readonly MethodInfo ServerCallCallMethod;
        /// <summary>
        /// 服务端调用参数类型集合
        /// </summary>
        internal readonly Type[] MethodParameterTypes;
        /// <summary>
        /// 命令处理参数类型集合
        /// </summary>
        internal readonly Type[] DoCommandParameterTypes;
        /// <summary>
        /// 生成标识
        /// </summary>
        internal int Identity;
        /// <summary>
        /// TCP 服务元数据
        /// </summary>
        /// <param name="serverType">TCP 服务类型</param>
        /// <param name="senderType">TCP 服务套接字发送数据类型</param>
        /// <param name="serverCallType">TCP 服务器端同步调用类型</param>
        internal ServerMetadataBase(Type serverType, Type senderType, Type serverCallType)
        {
            ServerType = serverType;
            SenderType = senderType;
            ServerCallType = serverCallType;
            ServerTypeName = serverType.fullName();
            ServerCallTypeName = serverCallType.fullName();
            ServerCallSenderField = serverCallType.GetField("Sender", BindingFlags.Public | BindingFlags.Instance);
            ServerCallServerValueField = serverCallType.GetField("serverValue", BindingFlags.NonPublic | BindingFlags.Instance);
            ServerCallPushMethod = serverCallType.GetMethod("push", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            ServerCallCallMethod = serverCallType.GetMethod("Call", BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);
            MethodParameterTypes = new Type[] { senderType, typeof(SubArray<byte>).MakeByRefType() };
            DoCommandParameterTypes = new Type[] { typeof(int), senderType, typeof(SubArray<byte>).MakeByRefType() };
        }
    }
    /// <summary>
    /// TCP 服务端元数据
    /// </summary>
    internal sealed class ServerMetadata : ServerMetadataBase
    {
        /// <summary>
        /// TCP 服务构造函数
        /// </summary>
        internal readonly ConstructorInfo ServerConstructorInfo;
        /// <summary>
        /// TCP 服务器端同步调用套接字发送数据函数信息
        /// </summary>
        internal readonly MethodInfo ServerSocketSenderPushMethod;
        /// <summary>
        /// TCP 服务器端同步调用套接字发送数据函数信息
        /// </summary>
        internal readonly MethodInfo ServerSocketSenderPushReturnValueMethod;
        /// <summary>
        /// TCP 服务器端同步调用套接字发送数据函数信息
        /// </summary>
        internal readonly MethodInfo ServerSocketSenderPushNoThreadMethod;
        ///// <summary>
        ///// TCP 服务器端同步调用套接字发送数据函数信息
        ///// </summary>
        //internal readonly MethodInfo ServerSocketSenderPushOutputMethod;
        ///// <summary>
        ///// TCP 服务器端同步调用套接字发送数据函数信息
        ///// </summary>
        //internal readonly MethodInfo ServerSocketSenderPushOutputRefMethod;
        /// <summary>
        /// TCP 服务器端同步调用套接字发送数据函数信息
        /// </summary>
        internal readonly MethodInfo ServerSocketSenderPushReturnTypeMethod;
        /// <summary>
        /// TCP 服务器端同步调用套接字错误日志处理函数信息
        /// </summary>
        internal readonly MethodInfo ServerSocketSenderAddLogMethod;
        /// <summary>
        /// TCP 服务器端同步调用套接字获取异步回调函数信息
        /// </summary>
        internal readonly MethodInfo ServerSocketSenderGetCallbackMethod;
        ///// <summary>
        ///// TCP 服务器端同步调用套接字获取异步回调函数信息
        ///// </summary>
        //internal readonly MethodInfo ServerSocketSenderGetCallbackReturnMethod;
        /// <summary>
        /// 输出参数泛型类型元数据
        /// </summary>
        internal readonly Func<Type, ParameterGenericType> GetParameterGenericType;
        /// <summary>
        /// 获取输出参数泛型类型元数据
        /// </summary>
        internal readonly Func<Type, Type, ReturnParameterGenericType> GetOutputParameterGenericType;
        /// <summary>
        /// TCP 服务元数据
        /// </summary>
        /// <param name="serverType">TCP 服务类型</param>
        /// <param name="serverAttributeType">TCP 服务配置类型</param>
        /// <param name="senderType">TCP 服务套接字发送数据类型</param>
        /// <param name="serverCallType">TCP 服务器端同步调用类型</param>
        /// <param name="getParameterGenericType">输出参数泛型类型元数据</param>
        /// <param name="getOutputParameterGenericType">获取输出参数泛型类型元数据</param>
        /// <param name="serverSocketSenderPushMethod">TCP 服务器端同步调用套接字发送数据函数信息</param>
        /// <param name="serverSocketSenderPushReturnTypeMethod">TCP 服务器端同步调用套接字发送数据函数信息</param>
        /// <param name="serverSocketSenderPushReturnValueMethod">TCP 服务器端同步调用套接字发送数据函数信息</param>
        /// <param name="serverSocketSenderPushNoThreadMethod">TCP 服务器端同步调用套接字发送数据函数信息</param>
        /// <param name="serverSocketSenderGetCallbackMethod">TCP 服务器端同步调用套接字获取异步回调函数信息</param>
        /// <param name="serverSocketSenderAddLogMethod">TCP 服务器端同步调用套接字错误日志处理函数信息</param>
        internal ServerMetadata(Type serverType, Type serverAttributeType, Type senderType, Type serverCallType
            , Func<Type, ParameterGenericType> getParameterGenericType, Func<Type, Type, ReturnParameterGenericType> getOutputParameterGenericType
            , MethodInfo serverSocketSenderPushMethod, MethodInfo serverSocketSenderPushReturnTypeMethod, MethodInfo serverSocketSenderPushReturnValueMethod
            , MethodInfo serverSocketSenderPushNoThreadMethod, MethodInfo serverSocketSenderGetCallbackMethod, MethodInfo serverSocketSenderAddLogMethod)
            : base(serverType, senderType, serverCallType)
        {
            ServerConstructorInfo = serverType.GetConstructor(new Type[] { serverAttributeType, typeof(Func<System.Net.Sockets.Socket, bool>), typeof(Action<SubArray<byte>>), typeof(AutoCSer.Log.ILog), typeof(bool) });
            GetParameterGenericType = getParameterGenericType;
            GetOutputParameterGenericType = getOutputParameterGenericType;

            ServerSocketSenderPushMethod = serverSocketSenderPushMethod;
            ServerSocketSenderPushReturnTypeMethod = serverSocketSenderPushReturnTypeMethod;
            ServerSocketSenderPushReturnValueMethod = serverSocketSenderPushReturnValueMethod;
            ServerSocketSenderPushNoThreadMethod = serverSocketSenderPushNoThreadMethod;
            ServerSocketSenderGetCallbackMethod = serverSocketSenderGetCallbackMethod;
            ServerSocketSenderAddLogMethod = serverSocketSenderAddLogMethod;

            //Type returnValueRefType = typeof(ReturnValue).MakeByRefType();
            //foreach (MethodInfo method in senderType.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            //{
            //    if (method.ReturnType == typeof(bool))
            //    {
            //        if (method.Name == "Push")
            //        {
            //            ParameterInfo[] parameters = method.GetParameters();
            //            switch (parameters.Length)
            //            {
            //                case 0: ServerSocketSenderPushMethod = method; break;
            //                case 1:
            //                    if (!method.IsGenericMethod && parameters[0].ParameterType == typeof(ReturnType)) ServerSocketSenderPushReturnTypeMethod = method;
            //                    break;
            //                case 2:
            //                    if (method.IsGenericMethod)
            //                    {
            //                        if (parameters[0].ParameterType == typeof(OutputInfo) && parameters[1].ParameterType.IsByRef) ServerSocketSenderPushOutputMethod = method;
            //                    }
            //                    else if (parameters[0].ParameterType == typeof(uint) && parameters[1].ParameterType == typeof(ReturnType)) ServerSocketSenderPushReturnValueMethod = method;
            //                    break;
            //                case 3:
            //                    if (method.IsGenericMethod && parameters[0].ParameterType == typeof(uint) && parameters[1].ParameterType == typeof(OutputInfo))
            //                    {
            //                        Type returnValueType = parameters[2].ParameterType;
            //                        if (returnValueType.IsByRef)
            //                        {
            //                            returnValueType = returnValueType.GetElementType();
            //                            if (returnValueType.IsGenericType && returnValueType.GetGenericTypeDefinition() == typeof(ReturnValue<>)) ServerSocketSenderPushOutputRefMethod = method;
            //                        }
            //                    }
            //                    break;
            //            }
            //        }
            //        else if (method.Name == "PushNoThread")
            //        {
            //            ParameterInfo[] parameters = method.GetParameters();
            //            if (parameters.Length == 2 && parameters[1].ParameterType == typeof(ReturnType) && parameters[0].ParameterType == typeof(uint)) ServerSocketSenderPushNoThreadMethod = method;
            //        }
            //    }
            //    else if (method.ReturnType == typeof(void))
            //    {
            //        if (method.Name == "AddLog")
            //        {
            //            ParameterInfo[] parameters = method.GetParameters();
            //            if (parameters.Length == 1 && parameters[0].ParameterType == typeof(Exception)) ServerSocketSenderAddLogMethod = method;
            //        }
            //    }
            //    else
            //    {
            //        if (method.Name == "GetCallback")
            //        {
            //            if (method.ReturnType == typeof(Func<ReturnValue, bool>))
            //            {
            //                if (!method.IsGenericMethod)
            //                {
            //                    ParameterInfo[] parameters = method.GetParameters();
            //                    if (parameters.Length == 1 && parameters[0].ParameterType == typeof(OutputInfo)) ServerSocketSenderGetCallbackMethod = method;
            //                }
            //            }
            //            else if (method.IsGenericMethod)
            //            {
            //                ParameterInfo[] parameters = method.GetParameters();
            //                if (parameters.Length == 2 && parameters[0].ParameterType == typeof(OutputInfo) && parameters[1].ParameterType.IsByRef)
            //                {
            //                    ServerSocketSenderGetCallbackReturnMethod = method;
            //                }
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// TCP 服务器端同步调用会话标识字段信息
        /// </summary>
        internal static readonly FieldInfo ServerCallCommandIndexField = typeof(TcpServer.ServerCall).GetField("CommandIndex", BindingFlags.Instance | BindingFlags.Public);
        /// <summary>
        /// TCP 服务器端同步调用套接字发送对象套接字是否有效函数信息
        /// </summary>
        internal static readonly MethodInfo ServerSocketSenderGetIsSocketMethod = typeof(ServerSocketSenderBase).GetProperty("IsSocket", BindingFlags.Instance | BindingFlags.Public).GetGetMethod();
        /// <summary>
        /// TCP 服务器端同步调用套接字发送对象通过函数验证处理
        /// </summary>
        internal static readonly MethodInfo ServerSocketSenderSetVerifyMethod;
        /// <summary>
        /// TCP 服务器端同步调用套接字发送对象通过函数验证处理
        /// </summary>
        internal static readonly MethodInfo ServerSocketSenderDeSerializeMethod;
        /// <summary>
        /// 返回值类型字段信息
        /// </summary>
        internal static readonly FieldInfo ReturnValueTypeField = typeof(ReturnValue).GetField("Type", BindingFlags.Instance | BindingFlags.Public);

        static ServerMetadata()
        {
            Type subArrayByteRefType = typeof(SubArray<byte>).MakeByRefType();
            foreach (MethodInfo method in typeof(ServerSocketSenderBase).GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (method.ReturnType == typeof(bool))
                {
                    if (method.IsGenericMethod && method.Name == "DeSerialize")
                    {
                        ParameterInfo[] parameters = method.GetParameters();
                        if (parameters.Length == 3 && parameters[0].ParameterType == subArrayByteRefType && parameters[1].ParameterType.IsByRef && parameters[2].ParameterType == typeof(bool)) ServerSocketSenderDeSerializeMethod = method;
                    }
                }
                else if (method.ReturnType == typeof(void))
                {
                    if (method.Name == "SetVerifyMethod" && method.GetParameters().Length == 0)
                    {
                        ServerSocketSenderSetVerifyMethod = method;
                    }
                }
            }
        }
    }
}
