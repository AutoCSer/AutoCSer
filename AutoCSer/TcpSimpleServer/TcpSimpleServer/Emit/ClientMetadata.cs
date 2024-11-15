﻿using System;
using System.Reflection;
using AutoCSer.Extensions;

namespace AutoCSer.Net.TcpSimpleServer.Emit
{
    /// <summary>
    /// TCP 客户端元数据
    /// </summary>
    internal abstract class ClientMetadata
    {
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
        /// TCP 服务客户端调用函数信息
        /// </summary>
        internal readonly MethodInfo ClientCallMethod;
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
        /// <param name="methodClientType">TCP 客户端基类类型</param>
        /// <param name="getParameterGenericType">输出参数泛型类型元数据</param>
        /// <param name="getParameterGenericType2">输入+输出参数泛型类型元数据</param>
        /// <param name="clientCallMethod">TCP 服务客户端调用函数信息</param>
        internal ClientMetadata(Type clientType, Type methodClientType
            , Func<Type, ParameterGenericType> getParameterGenericType, Func<Type, Type, ParameterGenericType2> getParameterGenericType2
            , MethodInfo clientCallMethod)
        {
            MethodClientType = methodClientType;
            ClientTypeName = clientType.fullName();
            MethodClientGetTcpClientMethod = methodClientType.GetProperty(TcpServer.Emit.ClientMetadataBase.TcpClientName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetGetMethod();
            GetParameterGenericType = getParameterGenericType;
            GetParameterGenericType2 = getParameterGenericType2;
            ClientCallMethod = clientCallMethod;
        }

        /// <summary>
        /// TCP 客户端基类 是否已经释放资源函数信息
        /// </summary>
        internal static readonly MethodInfo MethodClientGetIsDisposedMethod = ((Func<TcpServer.Emit.MethodClient, int>)TcpServer.Emit.MethodClient.GetIsDisposed).Method;
    }
}
