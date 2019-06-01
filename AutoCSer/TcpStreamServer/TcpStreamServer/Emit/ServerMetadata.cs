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
        /// 输出参数泛型类型元数据
        /// </summary>
        internal readonly Func<Type, AutoCSer.Net.TcpServer.Emit.ParameterGenericType> GetParameterGenericType;
        /// <summary>
        /// TCP 服务元数据
        /// </summary>
        /// <param name="serverType">TCP 服务类型</param>
        /// <param name="serverAttributeType">TCP 服务配置类型</param>
        /// <param name="senderType">TCP 服务套接字发送数据类型</param>
        /// <param name="serverCallType">TCP 服务器端同步调用类型</param>
        /// <param name="getParameterGenericType">输出参数泛型类型元数据</param>
        /// <param name="serverSocketSenderPushMethod">TCP 服务器端同步调用套接字发送数据函数信息</param>
        /// <param name="serverSocketSenderPushReturnTypeMethod">TCP 服务器端同步调用套接字发送数据函数信息</param>
        /// <param name="serverSocketSenderAddLogMethod">TCP 服务器端同步调用套接字错误日志处理函数信息</param>
        internal ServerMetadata(Type serverType, Type serverAttributeType, Type senderType, Type serverCallType
            , Func<Type, AutoCSer.Net.TcpServer.Emit.ParameterGenericType> getParameterGenericType
            , MethodInfo serverSocketSenderPushMethod, MethodInfo serverSocketSenderPushReturnTypeMethod, MethodInfo serverSocketSenderAddLogMethod)
            : base(serverType, senderType, serverCallType)
        {
            ServerConstructorInfo = serverType.GetConstructor(new Type[] { serverAttributeType, typeof(Func<System.Net.Sockets.Socket, bool>), typeof(AutoCSer.Log.ILog) });
            GetParameterGenericType = getParameterGenericType;

            ServerSocketSenderPushMethod = serverSocketSenderPushMethod;
            ServerSocketSenderPushReturnTypeMethod = serverSocketSenderPushReturnTypeMethod;
            ServerSocketSenderAddLogMethod = serverSocketSenderAddLogMethod;

            //Type returnValueRefType = typeof(TcpServer.ReturnValue).MakeByRefType();
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
            //                    if (!method.IsGenericMethod && parameters[0].ParameterType == typeof(TcpServer.ReturnType)) ServerSocketSenderPushReturnTypeMethod = method;
            //                    break;
            //                case 2:
            //                    if (method.IsGenericMethod && parameters[0].ParameterType == typeof(TcpServer.OutputInfo))
            //                    {
            //                        Type returnValueType = parameters[1].ParameterType;
            //                        if (returnValueType.IsByRef)
            //                        {
            //                            returnValueType = returnValueType.GetElementType();
            //                            if (returnValueType.IsGenericType && returnValueType.GetGenericTypeDefinition() == typeof(TcpServer.ReturnValue<>)) ServerSocketSenderPushOutputRefMethod = method;
            //                            else ServerSocketSenderPushOutputMethod = method;
            //                        }
            //                    }
            //                    break;
            //            }
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
            //}
        }
    }
}
