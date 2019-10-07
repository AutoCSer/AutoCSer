using System;
using System.Reflection;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpStreamServer.Emit
{
    /// <summary>
    /// TCP 服务端元数据
    /// </summary>
    internal sealed class ServerMetadata : TcpServer.Emit.ServerMetadataBase
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
        internal readonly MethodInfo ServerSocketSenderPushReturnTypeMethod;
        /// <summary>
        /// TCP 服务器端同步调用套接字错误日志处理函数信息
        /// </summary>
        internal readonly MethodInfo ServerSocketSenderAddLogMethod;
        /// <summary>
        /// 输出参数泛型类型元数据
        /// </summary>
        internal readonly Func<Type, AutoCSer.Net.TcpServer.Emit.ParameterGenericType> GetParameterGenericType;
        /// <summary>
        /// TCP 服务元数据
        /// </summary>
        /// <param name="serverType">TCP 服务类型</param>
        /// <param name="serverAttributeType">TCP 服务配置类型</param>
        /// <param name="senderType">TCP 服务套接字发送数据类型</param>
        /// <param name="getParameterGenericType">输出参数泛型类型元数据</param>
        /// <param name="serverSocketSenderPushMethod">TCP 服务器端同步调用套接字发送数据函数信息</param>
        /// <param name="serverSocketSenderPushReturnTypeMethod">TCP 服务器端同步调用套接字发送数据函数信息</param>
        /// <param name="serverSocketSenderAddLogMethod">TCP 服务器端同步调用套接字错误日志处理函数信息</param>
        internal ServerMetadata(Type serverType, Type serverAttributeType, Type senderType
            , Func<Type, AutoCSer.Net.TcpServer.Emit.ParameterGenericType> getParameterGenericType
            , MethodInfo serverSocketSenderPushMethod, MethodInfo serverSocketSenderPushReturnTypeMethod, MethodInfo serverSocketSenderAddLogMethod)
            : base(serverType, senderType)
        {
            ServerConstructorInfo = serverType.GetConstructor(new Type[] { serverAttributeType, typeof(Func<System.Net.Sockets.Socket, bool>), typeof(AutoCSer.Log.ILog) });
            GetParameterGenericType = getParameterGenericType;

            ServerSocketSenderPushMethod = serverSocketSenderPushMethod;
            ServerSocketSenderPushReturnTypeMethod = serverSocketSenderPushReturnTypeMethod;
            ServerSocketSenderAddLogMethod = serverSocketSenderAddLogMethod;
        }
    }
}
