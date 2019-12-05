using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 数据缓冲区
    /// </summary>
    internal sealed class Buffer : AutoCSer.Threading.Link<Buffer>, IDisposable
    {
        /// <summary>
        /// 消息标识
        /// </summary>
        internal ulong Identity;
        /// <summary>
        /// 消息队列节点
        /// </summary>
        internal readonly Node Node;
        /// <summary>
        /// 数据缓冲区计数
        /// </summary>
        internal AutoCSer.CacheServer.BufferCount BufferCount;
        /// <summary>
        /// 参数数据
        /// </summary>
        internal ValueData.Data Data;
        /// <summary>
        /// 返回调用委托
        /// </summary>
        private AutoCSer.Net.TcpServer.ServerCallback<ReturnParameter> onReturn;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal Buffer() { }
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parser"></param>
        internal Buffer(Node node, ref OperationParameter.NodeParser parser)
        {
            Data = parser.ValueData;
            onReturn = parser.OnReturn;
            Node = node;
            BufferCount = Data.CopyToMessageQueueBufferCount();
            parser.OnReturn = null;
        }
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        /// <param name="message"></param>
        internal Buffer(Node node, AutoCSer.Net.TcpServer.ServerCallback<ReturnParameter> onReturn, ref DistributionMessageItem message)
        {
            Node = node;
            this.onReturn = onReturn;
            Data = message.Data;
            BufferCount = message.OnAppendFile();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (BufferCount != null) BufferCount.Free();
            if (onReturn != null) onReturn.Callback(new ReturnParameter(ReturnType.MessageQueueBufferDisposed));
        }
        /// <summary>
        /// 释放数据缓冲区
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FreeBuffer()
        {
            if (BufferCount != null)
            {
                BufferCount.Free();
                BufferCount = null;
            }
        }
        /// <summary>
        /// 错误回调
        /// </summary>
        /// <param name="returnType"></param>
        internal void Error(ReturnType returnType)
        {
            FreeBuffer();
            AutoCSer.Net.TcpServer.ServerCallback<ReturnParameter> onReturn = this.onReturn;
            if (onReturn != null)
            {
                this.onReturn = null;
                onReturn.Callback(new ReturnParameter(returnType));
            }
        }
        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="next"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Callback(ref Buffer next)
        {
            FreeBuffer();
            callback(ref next);
        }
        /// <summary>
        /// 回调
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void callback(ref Buffer next)
        {
            next = LinkNext;
            LinkNext = null;
            if (onReturn != null)
            {
                ReturnParameter returnParameter = new ReturnParameter();
                returnParameter.Parameter.ReturnParameterSet(true);
                onReturn.Callback(returnParameter);
                onReturn = null;
            }
        }

        /// <summary>
        /// 数据操作回调
        /// </summary>
        /// <param name="next"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Append(ref Buffer next)
        {
            callback(ref next);
            Node.Append(this);
        }
    }
}
