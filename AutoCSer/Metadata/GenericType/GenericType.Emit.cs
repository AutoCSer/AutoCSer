using System;
using System.Collections.Generic;
using AutoCSer.Threading;
using System.Reflection;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract partial class GenericType
    {
        /// <summary>
        /// 客户端回调转换
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo TcpClientCallbackGetMethod { get; }
        /// <summary>
        /// 同步等待调用类型
        /// </summary>
        /// <returns></returns>
        internal abstract System.Type TcpAutoWaitReturnValueType { get; }
        /// <summary>
        /// 弹出同步等待调用节点
        /// </summary>
        /// <returns></returns>
        internal abstract MethodInfo TcpAutoWaitReturnValuePopMethod { get; }
#if !DOTNET2 && !DOTNET4 && !UNITY3D
        /// <summary>
        /// 设置错误返回值类型
        /// </summary>
        internal abstract MethodInfo TcpAwaiterCallReturnTypeMethod { get; }
        /// <summary>
        /// await 返回值类型
        /// </summary>
        internal abstract Type AwaiterReturnValueType { get; }
#endif
        ///// <summary>
        ///// 获取服务端回调转换
        ///// </summary>
        ///// <returns></returns>
        //internal abstract MethodInfo TcpServerCallbackGetMethod { get; }
        /// <summary>
        /// 服务端回调委托返类型
        /// </summary>
        internal abstract System.Type TcpServerCallbackType { get; }
        /// <summary>
        /// 服务端回调委托返类型
        /// </summary>
        internal abstract System.Type TcpServerCallbackEmitType { get; }
    }
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal sealed partial class GenericType<T> : GenericType
    {
        /// <summary>
        /// 客户端回调转换
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo TcpClientCallbackGetMethod
        {
            get { return ((Func<Func<AutoCSer.Net.TcpServer.ReturnValue<T>, bool>, Action<AutoCSer.Net.TcpServer.ReturnValue<T>>>)AutoCSer.Net.TcpServer.Emit.ClientCallback<T>.Get).Method; }
        }
        /// <summary>
        /// 同步等待调用类型
        /// </summary>
        /// <returns></returns>
        internal override System.Type TcpAutoWaitReturnValueType
        {
            get { return typeof(AutoCSer.Net.TcpServer.AutoWaitReturnValue<T>); }
        }
        /// <summary>
        /// 弹出同步等待调用节点
        /// </summary>
        /// <returns></returns>
        internal override MethodInfo TcpAutoWaitReturnValuePopMethod
        {
            get { return ((Func<AutoCSer.Net.TcpServer.AutoWaitReturnValue<T>>)AutoCSer.Net.TcpServer.AutoWaitReturnValue<T>.Pop).Method; }
        }
#if !DOTNET2 && !DOTNET4 && !UNITY3D
        /// <summary>
        /// 异步等待
        /// </summary>
        private AutoCSer.Net.TcpServer.Emit.Awaiter<T> tcpAwaiter;
        /// <summary>
        /// 设置错误返回值类型
        /// </summary>
        internal override MethodInfo TcpAwaiterCallReturnTypeMethod
        {
            get
            {
                if (tcpAwaiter == null) tcpAwaiter = new AutoCSer.Net.TcpServer.Emit.Awaiter<T>();
                return ((Action<AutoCSer.Net.TcpServer.ReturnType>)tcpAwaiter.Call).Method;
            }
        }
        /// <summary>
        /// await 返回值类型
        /// </summary>
        internal override System.Type AwaiterReturnValueType { get { return typeof(AutoCSer.Net.TcpServer.Emit.AwaiterReturnValue<T>); } }
#endif
        ///// <summary>
        ///// 获取服务端回调转换
        ///// </summary>
        ///// <returns></returns>
        //internal override MethodInfo TcpServerCallbackGetMethod
        //{
        //    get { return ((Func<Func<AutoCSer.Net.TcpServer.ReturnValue<T>, bool>, Func<T, bool>>)AutoCSer.Net.TcpServer.Emit.ServerCallback<T>.Get).Method; }
        //}
        /// <summary>
        /// 服务端回调委托返类型
        /// </summary>
        internal override System.Type TcpServerCallbackType
        {
            get { return typeof(AutoCSer.Net.TcpServer.ServerCallback<T>); }
        }
        /// <summary>
        /// 服务端回调委托返类型
        /// </summary>
        internal override System.Type TcpServerCallbackEmitType
        {
            get { return typeof(Func<AutoCSer.Net.TcpServer.ReturnValue<T>, bool>); }
        }
    }
}
