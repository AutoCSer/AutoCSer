﻿using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// TCP 客户端
    /// </summary>
    public abstract class MethodClient : AutoCSer.Net.TcpServer.MethodClient
    {
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        protected volatile int _isDisposed_;
        /// <summary>
        /// 获取是否已经释放资源
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static int GetIsDisposed(MethodClient client)
        {
            return client._isDisposed_;
        }
    }
    /// <summary>
    /// TCP 客户端
    /// </summary>
    /// <typeparam name="clientType">TCP 服务客户端类型</typeparam>
    public abstract class MethodClient<clientType> : MethodClient, IDisposable
        where clientType : CommandBase
    {
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        public clientType _TcpClient_ { get; internal set; }
        /// <summary>
        /// 客户端等待连接
        /// </summary>
        internal ClientWaitConnected _WaitConnected_;
        /// <summary>
        /// 客户端等待连接
        /// </summary>
        /// <returns></returns>
        public ClientWaitConnected _GetWaitConnected_() { return _WaitConnected_; }
        /// <summary>
        /// 等待连接初始化
        /// </summary>
        /// <returns>是否连接状态</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool _CheckWaitConnected_()
        {
            return _WaitConnected_.WaitConnected();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (System.Threading.Interlocked.CompareExchange(ref _isDisposed_, 1, 0) == 0)
            {
                _TcpClient_.Dispose();
                if (_WaitConnected_ != null) _WaitConnected_.Dispose();
            }
        }
    }
}
