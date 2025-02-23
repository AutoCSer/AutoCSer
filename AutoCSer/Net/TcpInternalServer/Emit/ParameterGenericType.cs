﻿using System;
using AutoCSer.Threading;
using System.Reflection;
using AutoCSer.Net.TcpServer;

namespace AutoCSer.Net.TcpInternalServer.Emit
{
    /// <summary>
    /// 输出参数泛型类型元数据
    /// </summary>
    internal abstract partial class ParameterGenericType : AutoCSer.Net.TcpServer.Emit.ParameterGenericType
    {
        /// <summary>
        /// TCP 内部服务客户端
        /// </summary>
        internal static readonly TcpInternalServer.Client Client = new Client();
        /// <summary>
        /// TCP 内部服务客户端套接字数据发送
        /// </summary>
        internal static readonly ClientSocketSender ClientSocketSender = new ClientSocketSender();
        /// <summary>
        /// TCP 内部服务套接字数据发送
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
        /// TCP调用并返回参数值
        /// </summary>
        /// <param name="identityCommand"></param>
        /// <param name="callback"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        internal delegate ReturnType ClientSocketSenderWaitGet(CommandInfo identityCommand, ref AutoCSer.Net.TcpServer.AutoWaitReturnValue<parameterType> callback, ref parameterType outputParameter);
        /// <summary>
        /// 获取异步回调
        /// </summary>
        internal override MethodInfo ClientSocketSenderWaitGetMethod
        {
            get { return ((ClientSocketSenderWaitGet)ClientSocketSender.WaitGet<parameterType>).Method; }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="identityCommand"></param>
        /// <param name="onCall"></param>
        /// <param name="inputParameter"></param>
        /// <returns></returns>
        internal delegate ReturnType ClientSocketSenderWaitCall(CommandInfo identityCommand, ref AutoWaitReturnValue onCall, ref parameterType inputParameter);
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderWaitCallMethod
        {
            get { return ((ClientSocketSenderWaitCall)ClientSocketSender.WaitCall<parameterType>).Method; }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        /// <param name="identityCommand"></param>
        /// <param name="inputParameter"></param>
        internal delegate void ClientSocketSenderCallOnly(CommandInfo identityCommand, ref parameterType inputParameter);
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderCallOnlyMethod
        {
            get { return ((ClientSocketSenderCallOnly)ClientSocketSender.CallOnly<parameterType>).Method; }
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        internal delegate void ClientSocketSenderGet(CommandInfo identityCommand, ref Callback<ReturnValue<parameterType>> callback);
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderGetMethod
        {
            get { return ((ClientSocketSenderGet)ClientSocketSender.Get<parameterType>).Method; }
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        internal delegate KeepCallback ClientSocketSenderGetKeep(CommandInfo identityCommand, ref Callback<ReturnValue<parameterType>> callback);
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderGetKeepMethod
        {
            get { return ((ClientSocketSenderGetKeep)ClientSocketSender.GetKeep<parameterType>).Method; }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        internal delegate void ClientSocketSenderCall(CommandInfo identityCommand, Action<ReturnValue> onCall, ref parameterType inputParameter);
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderCallMethod
        {
            get { return ((ClientSocketSenderCall)ClientSocketSender.Call<parameterType>).Method; }
        }
        /// <summary>
        /// TCP调用
        /// </summary>
        internal delegate KeepCallback ClientSocketSenderCallKeep(CommandInfo identityCommand, Action<ReturnValue> onCall, ref parameterType inputParameter);
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderCallKeepMethod
        {
            get { return ((ClientSocketSenderCallKeep)ClientSocketSender.CallKeep<parameterType>).Method; }
        }
#if !DOTNET2 && !DOTNET4 && !UNITY3D
        /// <summary>
        /// TCP调用
        /// </summary>
        internal delegate ReturnType ClientSocketSenderGetAwaiter(CommandInfo identityCommand, Awaiter onCall, ref parameterType inputParameter);
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderGetAwaiterMethod
        {
            get { return ((ClientSocketSenderGetAwaiter)ClientSocketSender.GetAwaiter).Method; }
        }
        /// <summary>
        /// TCP调用并返回参数值
        /// </summary>
        internal delegate ReturnType ClientSocketSenderGetAwaiterOutput(CommandInfo identityCommand, Callback<ReturnValue<parameterType>> callback, ref parameterType outputParameter);
        /// <summary>
        /// TCP调用
        /// </summary>
        internal override MethodInfo ClientSocketSenderGetAwaiterOutputMethod
        {
            get { return ((ClientSocketSenderGetAwaiterOutput)ClientSocketSender.GetAwaiter<parameterType>).Method; }
        }
#endif

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="outputInfo"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        internal delegate bool ServerSocketSenderPush(TcpServer.OutputInfo outputInfo, ref parameterType outputParameter);
        /// <summary>
        /// 发送数据
        /// </summary>
        internal override MethodInfo ServerSocketSenderPushMethod
        {
            get { return ((ServerSocketSenderPush)ServerSocketSender.Push<parameterType>).Method; }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="commandIndex"></param>
        /// <param name="outputInfo"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        internal delegate bool ServerSocketSenderPushCommand(uint commandIndex, TcpServer.OutputInfo outputInfo, ref ReturnValue<parameterType> outputParameter);
        /// <summary>
        /// 发送数据
        /// </summary>
        internal override MethodInfo ServerSocketSenderPushCommandMethod
        {
            get { return ((ServerSocketSenderPushCommand)ServerSocketSender.Push<parameterType>).Method; }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data">数据</param>
        /// <param name="value">目标对象</param>
        /// <param name="isSimpleSerialize"></param>
        /// <returns>是否成功</returns>
        internal delegate bool ServerSocketSenderDeSerialize(ServerSocketSenderBase socket, ref SubArray<byte> data, ref parameterType value, bool isSimpleSerialize);
        /// <summary>
        /// TCP 服务器端同步调用套接字发送对象通过函数验证处理
        /// </summary>
        internal override MethodInfo ServerSocketSenderDeSerializeMethod { get { return ((ServerSocketSenderDeSerialize)ServerSocketSenderBase.DeSerialize<parameterType>).Method; } }
    }
}
