using System;
using AutoCSer.TestCase.TcpServerPerformance;

namespace AutoCSer.TestCase.TcpInternalServerPerformance
{
    /// <summary>
    /// TCP 内部服务性能测试服务
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Host = "127.0.0.1", Port = 12102, SendBufferSize = SubBuffer.Size.Kilobyte64, ReceiveBufferSize = SubBuffer.Size.Kilobyte64, CheckSeconds = 0)]
    public interface IServer
    {
        /// <summary>
        /// 客户端同步计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right);
        /// <summary>
        /// 异步计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="onAdd"></param>
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous)]
        void AddAsynchronous(int left, int right, Func<AutoCSer.Net.TcpServer.ReturnValue<Add>, bool> onAdd);

        /// <summary>
        /// 计算回调测试
        /// </summary>
        /// <param name="onAdd"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.KeepCallbackMethod(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous)]
        AutoCSer.Net.TcpServer.KeepCallback AddRegister(Func<AutoCSer.Net.TcpServer.ReturnValue<Add>, bool> onAdd);
        /// <summary>
        /// 计算回调测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, IsClientSendOnly = true)]
        void AddRegister(int left, int right);
        /// <summary>
        /// 计算回调测试
        /// </summary>
        /// <param name="onCustomSerialize"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.KeepCallbackMethod(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous)]
        AutoCSer.Net.TcpServer.KeepCallback AddCustomSerialize(Func<AutoCSer.Net.TcpServer.ReturnValue<ServerCustomSerialize>, bool> onCustomSerialize);
        /// <summary>
        /// 计算回调测试
        /// </summary>
        /// <param name="value"></param>
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, IsClientSendOnly = true)]
        void AddCustomSerialize(ClientCustomSerialize value);
        /// <summary>
        /// 计算测试结束
        /// </summary>
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, IsClientSendOnly = true)]
        void AddCustomSerializeFinally();
    }
}
