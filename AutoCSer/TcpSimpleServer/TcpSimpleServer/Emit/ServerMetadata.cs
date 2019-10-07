using System;
using System.Reflection;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpSimpleServer.Emit
{
    /// <summary>
    /// TCP 服务端元数据
    /// </summary>
    internal sealed class ServerMetadata
    {
        /// <summary>
        /// 服务端类型
        /// </summary>
        internal readonly Type ServerType;
        /// <summary>
        /// TCP 服务套接字类型
        /// </summary>
        internal readonly Type SocketType;
        /// <summary>
        /// 服务端类型名称
        /// </summary>
        internal readonly string ServerTypeName;
        /// <summary>
        /// TCP 服务构造函数
        /// </summary>
        internal readonly ConstructorInfo ServerConstructorInfo;
        /// <summary>
        /// TCP 服务器端套接字错误日志处理函数信息
        /// </summary>
        internal readonly MethodInfo ServerSocketLogMethod;
        /// <summary>
        /// TCP 服务器端套接字发送数据函数信息
        /// </summary>
        internal readonly MethodInfo ServerSocketSendOutputReturnTypeMethod;
        /// <summary>
        /// TCP 服务器端套接字发送数据函数信息
        /// </summary>
        internal readonly MethodInfo ServerSocketSendReturnTypeMethod;
        /// <summary>
        /// TCP 服务器端套接字发送数据函数信息
        /// </summary>
        internal readonly MethodInfo ServerSocketSendMethod;
        /// <summary>
        /// 输出参数泛型类型元数据
        /// </summary>
        internal readonly Func<Type, ParameterGenericType> GetParameterGenericType;
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
        /// <param name="serverAttributeType">TCP 服务配置类型</param>
        /// <param name="socketType">TCP 服务套接字类型</param>
        /// <param name="getParameterGenericType">输出参数泛型类型元数据</param>
        /// <param name="serverSocketSendMethod">TCP 服务器端套接字发送数据函数信息</param>
        /// <param name="serverSocketSendReturnTypeMethod">TCP 服务器端套接字发送数据函数信息</param>
        /// <param name="serverSocketSendOutputReturnTypeMethod">TCP 服务器端套接字发送数据函数信息</param>
        /// <param name="serverSocketLogMethod">TCP 服务器端套接字错误日志处理函数信息</param>
        internal ServerMetadata(Type serverType, Type serverAttributeType, Type socketType
            , Func<Type, ParameterGenericType> getParameterGenericType
            , MethodInfo serverSocketSendMethod, MethodInfo serverSocketSendReturnTypeMethod
            , MethodInfo serverSocketSendOutputReturnTypeMethod
            , MethodInfo serverSocketLogMethod)
        {
            ServerType = serverType;
            SocketType = socketType;
            ServerTypeName = serverType.fullName();
            ServerConstructorInfo = serverType.GetConstructor(new Type[] { serverAttributeType, typeof(Func<System.Net.Sockets.Socket, bool>), typeof(AutoCSer.Log.ILog), typeof(bool) });
            DoCommandParameterTypes = new Type[] { typeof(int), socketType, typeof(SubArray<byte>).MakeByRefType() };
            MethodParameterTypes = new Type[] { socketType, typeof(SubArray<byte>).MakeByRefType() };

            GetParameterGenericType = getParameterGenericType;
            ServerSocketSendMethod = serverSocketSendMethod;
            ServerSocketSendReturnTypeMethod = serverSocketSendReturnTypeMethod;
            ServerSocketSendOutputReturnTypeMethod = serverSocketSendOutputReturnTypeMethod;
            ServerSocketLogMethod = serverSocketLogMethod;
        }

        /// <summary>
        /// TCP 服务器端同步调用套接字对象通过函数验证处理
        /// </summary>
        internal static readonly MethodInfo ServerSocketDeSerializeMethod;
        /// <summary>
        /// TCP 服务器端同步调用套接字对象通过函数验证处理
        /// </summary>
        internal static readonly MethodInfo ServerSocketSetVerifyMethodMethod;
        static ServerMetadata()
        {
            Type subArrayByteRefType = typeof(SubArray<byte>).MakeByRefType();
            foreach (MethodInfo method in typeof(ServerSocket).GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (method.ReturnType == typeof(bool))
                {
                    if (method.IsGenericMethod && method.Name == "DeSerialize")
                    {
                        ParameterInfo[] parameters = method.GetParameters();
                        if (parameters.Length == 3 && parameters[0].ParameterType == subArrayByteRefType && parameters[1].ParameterType.IsByRef && parameters[2].ParameterType == typeof(bool)) ServerSocketDeSerializeMethod = method;
                    }
                }
                else if (method.ReturnType == typeof(void))
                {
                    if (method.Name == "SetVerifyMethod" && method.GetParameters().Length == 0)
                    {
                        ServerSocketSetVerifyMethodMethod = method;
                    }
                }
            }
        }
    }
}
