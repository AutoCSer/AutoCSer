﻿using System;
using AutoCSer.TestCase.TcpServerPerformance;
using System.Runtime.CompilerServices;

namespace AutoCSer.TestCase.TcpInternalServerPerformance
{
    /// <summary>
    /// TCP 内部服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Name = InternalServer.ServerName, Host = "127.0.0.1", Port = 12100, SendBufferSize = AutoCSer.Memory.BufferSize.Kilobyte64, ReceiveBufferSize = AutoCSer.Memory.BufferSize.Kilobyte64, IsRememberCommand = false, CheckSeconds = 0)]
    public partial class InternalServer
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = "TcpInternalServerPerformance";
        /// <summary>
        /// 客户端同步计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
#if DOTNET2 || DOTNET4
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
#else
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, IsClientTaskAsync = true)]
#endif
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int add(int left, int right)
        {
            return left + right;
        }
        /// <summary>
        /// 异步计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="onAdd"></param>
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void addAsynchronous(int left, int right, AutoCSer.Net.TcpServer.ServerCallback<Add> onAdd)
        {
            onAdd.Callback(new Add(left, right));
        }

        /// <summary>
        /// 简单计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Add addSynchronous(int left, int right)
        {
            return new Add(left, right);
        }
        /// <summary>
        /// 计算队列测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.TcpQueue, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Add addQueue(int left, int right)
        {
            return new Add(left, right);
        }
        /// <summary>
        /// 计算任务测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.TcpTask, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Add addTcpTask(int left, int right)
        {
            return new Add(left, right);
        }
        /// <summary>
        /// 计算任务测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Timeout, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Add addTimeoutTask(int left, int right)
        {
            return new Add(left, right);
        }
        /// <summary>
        /// 计算任务测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.ThreadPool, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsClientAsynchronous = true, IsClientSynchronous = false, IsClientAwaiter = false)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Add addThreadPool(int left, int right)
        {
            return new Add(left, right);
        }

        /// <summary>
        /// 计算回调
        /// </summary>
        private AutoCSer.Net.TcpServer.ServerCallback<Add> onAdd;
        /// <summary>
        /// 计算回调测试
        /// </summary>
        /// <param name="onAdd"></param>
        [AutoCSer.Net.TcpServer.KeepCallbackMethod(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void addRegister(AutoCSer.Net.TcpServer.ServerCallback<Add> onAdd)
        {
            this.onAdd = onAdd;
        }
        /// <summary>
        /// 计算回调测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, IsClientSendOnly = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void addRegister(int left, int right)
        {
            onAdd.Callback(new Add(left, right));
        }

        /// <summary>
        /// 计算回调
        /// </summary>
        private ServerCustomSerializeOutput customSerializeOutput;
        /// <summary>
        /// 计算回调测试
        /// </summary>
        /// <param name="onCustomSerialize"></param>
        [AutoCSer.Net.TcpServer.KeepCallbackMethod(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void addCustomSerialize(AutoCSer.Net.TcpServer.ServerCallback<ServerCustomSerialize> onCustomSerialize)
        {
            customSerializeOutput = new ServerCustomSerializeOutput(onCustomSerialize);
        }
        /// <summary>
        /// 计算回调测试
        /// </summary>
        /// <param name="value"></param>
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, IsClientSendOnly = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void addCustomSerialize(ClientCustomSerialize value)
        {
            customSerializeOutput.Append(ref value);
        }
        /// <summary>
        /// 计算测试结束
        /// </summary>
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, IsClientSendOnly = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void addCustomSerializeFinally()
        {
            customSerializeOutput = null;
        }

        /// <summary>
        /// 计算回调测试
        /// </summary>
        /// <param name="value"></param>
        //[AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, IsClientSendOnly = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe void addCustomSerializeRegister(ClientCustomSerialize value)
        {
            fixed (byte* bufferFixed = value.Buffer.BufferArray)
            {
                for (byte* read = bufferFixed + value.Buffer.StartIndex, end = read + value.Buffer.Count; read != end; read += sizeof(int) * 2) onAdd.Callback(new Add(*(int*)read, *(int*)(read + sizeof(int))));
            }
        }

        /// <summary>
        /// GC 垃圾回收
        /// </summary>
        [AutoCSer.Net.TcpServer.Method(IsClientAwaiter = false)]
        private void gcCollect()
        {
            GC.Collect();
        }
    }
}
