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
        private Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onReturn;
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
        internal Buffer(Node node, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onReturn, ref DistributionMessageItem message)
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
            if (onReturn != null) onReturn(new ReturnParameter(ReturnType.MessageQueueBufferDisposed));
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
            Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onReturn = this.onReturn;
            if (onReturn != null)
            {
                this.onReturn = null;
                onReturn(new ReturnParameter(returnType));
            }
        }
        /// <summary>
        /// 回调
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Buffer Callback()
        {
            FreeBuffer();
            return callback();
        }
        /// <summary>
        /// 回调
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Buffer callback()
        {
            if (onReturn != null)
            {
                ReturnParameter returnParameter = new ReturnParameter();
                returnParameter.Parameter.ReturnParameterSet(true);
                onReturn(returnParameter);
                onReturn = null;
            }
            return LinkNext;
        }

        /// <summary>
        /// 数据操作回调
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Buffer Append()
        {
            callback();
            Node.Append(this);
            return LinkNext;
        }
    }
}
