﻿using System;
using AutoCSer.Threading;
using System.Reflection;
using AutoCSer.Net.TcpServer;

namespace AutoCSer.Net.TcpOpenServer.Emit
{
    /// <summary>
    /// 输出参数泛型类型元数据
    /// </summary>
    internal abstract partial class ParameterGenericType : AutoCSer.Net.TcpServer.Emit.ParameterGenericType
    {
        /// <summary>
        /// TCP 开放服务客户端
        /// </summary>
        internal static readonly TcpOpenServer.Client Client = new Client();
        /// <summary>
        /// TCP 内部服务客户端套接字数据发送
        /// </summary>
        internal static readonly ClientSocketSender ClientSocketSender = new ClientSocketSender();
        /// <summary>
        /// TCP 开放服务套接字数据发送
        /// </summary>
        internal static readonly ServerSocketSender ServerSocketSender = new ServerSocketSender();

        /// <summary>
        /// 泛型类型元数据缓存
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<HashType, ParameterGenericType> cache = new LockLastDictionary<HashType, ParameterGenericType>(getCurrentType);
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="parameterType"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static ParameterGenericType create<parameterType>()
        where parameterType : struct
        {
            return new ParameterGenericType<parameterType>();
        }
        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        private static readonly MethodInfo createMethod = typeof(ParameterGenericType).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="outputParameterType"></param>
        /// <returns></returns>
        public static ParameterGenericType Get(Type outputParameterType)
        {
            ParameterGenericType value;
            if (!cache.TryGetValue(outputParameterType, out value))
            {
                try
                {
                    value = new UnionType.ParameterGenericType { Object = createMethod.MakeGenericMethod(outputParameterType).Invoke(null, null) }.Value;
                    cache.Set(outputParameterType, value);
                }
                finally { cache.Exit(); }
            }
            return value;
        }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    /// <typeparam name="parameterType">泛型类型</typeparam>
    internal sealed partial class ParameterGenericType<parameterType> : ParameterGenericType
        where parameterType : struct
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(parameterType); } }

        /// <summary>
        /// 获取异步回调
        /// </summary>
        internal override MethodInfo ClientSocketSenderWaitGetMethod
        {
            get { return ((AutoCSer.Net.TcpInternalServer.Emit.ParameterGenericType<parameterType>.ClientSocketSenderWaitGet)ClientSocketSender.WaitGet<parameterType>).Method; }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderWaitCallMethod
        {
            get { return ((AutoCSer.Net.TcpInternalServer.Emit.ParameterGenericType<parameterType>.ClientSocketSenderWaitCall)ClientSocketSender.WaitCall<parameterType>).Method; }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderCallOnlyMethod
        {
            get { return ((AutoCSer.Net.TcpInternalServer.Emit.ParameterGenericType<parameterType>.ClientSocketSenderCallOnly)ClientSocketSender.CallOnly<parameterType>).Method; }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderGetMethod
        {
            get { return ((AutoCSer.Net.TcpInternalServer.Emit.ParameterGenericType<parameterType>.ClientSocketSenderGet)ClientSocketSender.Get<parameterType>).Method; }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderGetKeepMethod
        {
            get { return ((AutoCSer.Net.TcpInternalServer.Emit.ParameterGenericType<parameterType>.ClientSocketSenderGetKeep)ClientSocketSender.GetKeep<parameterType>).Method; }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderCallMethod
        {
            get { return ((AutoCSer.Net.TcpInternalServer.Emit.ParameterGenericType<parameterType>.ClientSocketSenderCall)ClientSocketSender.Call<parameterType>).Method; }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderCallKeepMethod
        {
            get { return ((AutoCSer.Net.TcpInternalServer.Emit.ParameterGenericType<parameterType>.ClientSocketSenderCallKeep)ClientSocketSender.CallKeep<parameterType>).Method; }
        }
#if !DOTNET2 && !DOTNET4 && !UNITY3D
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderGetAwaiterMethod
        {
            get { return ((AutoCSer.Net.TcpInternalServer.Emit.ParameterGenericType<parameterType>.ClientSocketSenderGetAwaiter)ClientSocketSender.GetAwaiter<parameterType>).Method; }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderGetAwaiterOutputMethod
        {
            get { return ((AutoCSer.Net.TcpInternalServer.Emit.ParameterGenericType<parameterType>.ClientSocketSenderGetAwaiterOutput)ClientSocketSender.GetAwaiter<parameterType>).Method; }
        }
#endif

        /// <summary>
        /// 发送数据
        /// </summary>
        internal override MethodInfo ServerSocketSenderPushMethod
        {
            get { return ((AutoCSer.Net.TcpInternalServer.Emit.ParameterGenericType<parameterType>.ServerSocketSenderPush)ServerSocketSender.Push<parameterType>).Method; }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        internal override MethodInfo ServerSocketSenderPushCommandMethod
        {
            get { return ((AutoCSer.Net.TcpInternalServer.Emit.ParameterGenericType<parameterType>.ServerSocketSenderPushCommand)ServerSocketSender.Push<parameterType>).Method; }
        }
        /// <summary>
        /// TCP 服务器端同步调用套接字发送对象通过函数验证处理
        /// </summary>
        internal override MethodInfo ServerSocketSenderDeSerializeMethod
        {
            get { return ((AutoCSer.Net.TcpInternalServer.Emit.ParameterGenericType<parameterType>.ServerSocketSenderDeSerialize)ServerSocketSenderBase.DeSerialize<parameterType>).Method; }
        }
    }
}
