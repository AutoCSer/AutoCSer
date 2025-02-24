﻿using System;
using AutoCSer.Threading;
using System.Reflection;
using AutoCSer.Net.TcpServer;

namespace AutoCSer.Net.TcpOpenServer.Emit
{
    /// <summary>
    /// 输出参数泛型类型元数据
    /// </summary>
    internal abstract partial class ReturnParameterGenericType : AutoCSer.Net.TcpServer.Emit.ReturnParameterGenericType
    {
        /// <summary>
        /// 泛型类型元数据缓存
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<HashType, ReturnParameterGenericType> cache = new LockLastDictionary<HashType, ReturnParameterGenericType>(getCurrentType);
        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="returnType"></typeparam>
        /// <typeparam name="outputParameterType"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static ReturnParameterGenericType create<returnType, outputParameterType>()
        where outputParameterType : struct, IReturnParameter<returnType>
        {
            return new ReturnParameterGenericType<returnType, outputParameterType>();
        }
        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        private static readonly MethodInfo createMethod = typeof(ReturnParameterGenericType).GetMethod("create", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="outputParameterType"></param>
        /// <returns></returns>
        public static ReturnParameterGenericType Get(Type returnType, Type outputParameterType)
        {
            ReturnParameterGenericType value;
            if (!cache.TryGetValue(outputParameterType, out value))
            {
                try
                {
                    value = new UnionType.ReturnParameterGenericType { Object = createMethod.MakeGenericMethod(returnType, outputParameterType).Invoke(null, null) }.Value;
                    cache.Set(outputParameterType, value);
                }
                finally { cache.Exit(); }
            }
            return value;
        }
    }
    /// <summary>
    /// 输出参数泛型类型元数据
    /// </summary>
    /// <typeparam name="returnType"></typeparam>
    /// <typeparam name="outputParameterType"></typeparam>
    internal sealed partial class ReturnParameterGenericType<returnType, outputParameterType> : ReturnParameterGenericType
        where outputParameterType : struct, IReturnParameter<returnType>
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(outputParameterType); } }

        /// <summary>
        /// 获取异步回调
        /// </summary>
        internal override MethodInfo ClientGetCallbackMethod
        {
            get { return ((Func<Action<ReturnValue<returnType>>, Callback<ReturnValue<outputParameterType>>>)ParameterGenericType.Client.GetCallback<returnType, outputParameterType>).Method; }
        }

        /// <summary>
        /// 异步回调
        /// </summary>
        internal override MethodInfo ServerSocketSenderGetCallbackMethod
        {
            get { return ((AutoCSer.Net.TcpInternalServer.Emit.ReturnParameterGenericType<returnType, outputParameterType>.ServerSocketSenderGetCallback)ParameterGenericType.ServerSocketSender.GetCallback<outputParameterType, returnType>).Method; }
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        internal override MethodInfo ServerSocketSenderGetCallbackEmitMethod
        {
            get { return ((AutoCSer.Net.TcpInternalServer.Emit.ReturnParameterGenericType<returnType, outputParameterType>.ServerSocketSenderGetCallbackEmit)ParameterGenericType.ServerSocketSender.GetCallbackEmit<outputParameterType, returnType>).Method; }
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        internal override MethodInfo ServerSocketSenderGetCallbackReturnMethod
        {
            get { return ((AutoCSer.Net.TcpInternalServer.Emit.ReturnParameterGenericType<returnType, outputParameterType>.ServerSocketSenderGetCallbackReturn)ParameterGenericType.ServerSocketSender.GetCallbackReturn<outputParameterType, returnType>).Method; }
        }
    }
}
