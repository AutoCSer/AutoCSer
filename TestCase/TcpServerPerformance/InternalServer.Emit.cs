using System;
using AutoCSer.TestCase.TcpServerPerformance;
using System.Runtime.CompilerServices;

namespace AutoCSer.TestCase.TcpInternalServerPerformance
{
    /// <summary>
    /// TCP 内部服务性能测试服务
    /// </summary>
    sealed class InternalServer : IServer
    {
        /// <summary>
        /// 客户端同步计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.ReturnValue<int> Add(int left, int right)
        {
            return left + right;
        }
        /// <summary>
        /// 简单计算测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="onAdd"></param>
        public void AddAsynchronous(int left, int right, Func<AutoCSer.Net.TcpServer.ReturnValue<Add>, bool> onAdd)
        {
            onAdd(new Add(left, right));
        }

        /// <summary>
        /// 计算回调
        /// </summary>
        private Func<AutoCSer.Net.TcpServer.ReturnValue<Add>, bool> onAdd;
        /// <summary>
        /// 计算回调测试
        /// </summary>
        /// <param name="onAdd"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback AddRegister(Func<AutoCSer.Net.TcpServer.ReturnValue<Add>, bool> onAdd)
        {
            this.onAdd = onAdd;
            return null;
        }
        /// <summary>
        /// 计算回调测试
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public void AddRegister(int left, int right)
        {
            onAdd(new Add(left, right));
        }

        /// <summary>
        /// 计算回调
        /// </summary>
        private ServerCustomSerializeOutput customSerializeOutput;
        /// <summary>
        /// 计算回调测试
        /// </summary>
        /// <param name="onCustomSerialize"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback AddCustomSerialize(Func<AutoCSer.Net.TcpServer.ReturnValue<ServerCustomSerialize>, bool> onCustomSerialize)
        {
            customSerializeOutput = new ServerCustomSerializeOutput(onCustomSerialize);
            return null;
        }
        /// <summary>
        /// 计算回调测试
        /// </summary>
        /// <param name="value"></param>
        public void AddCustomSerialize(ClientCustomSerialize value)
        {
            customSerializeOutput.Append(ref value);
        }
        /// <summary>
        /// 计算测试结束
        /// </summary>
        public void AddCustomSerializeFinally()
        {
            customSerializeOutput = null;
        }

        /// <summary>
        /// 计算回调测试
        /// </summary>
        /// <param name="value"></param>
        public unsafe void AddCustomSerializeRegister(ClientCustomSerialize value)
        {
            fixed (byte* bufferFixed = value.Buffer.BufferArray)
            {
                for (byte* read = bufferFixed + value.Buffer.StartIndex, end = read + value.Buffer.Count; read != end; read += sizeof(int) * 2) onAdd(new Add(*(int*)read, *(int*)(read + sizeof(int))));
            }
        }
    }
}
