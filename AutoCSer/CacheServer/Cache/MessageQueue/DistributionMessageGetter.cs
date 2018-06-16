using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 消息回调
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct DistributionMessageGetter
    {
        /// <summary>
        /// 当前可发送数量
        /// </summary>
        private int sendCount;
        /// <summary>
        /// 最大可发送数量
        /// </summary>
        private int maxSendCount;
        /// <summary>
        /// 获取消息回调委托
        /// </summary>
        internal Func<AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter>, bool> OnGetMessage;
        /// <summary>
        /// 是否反序列化网络流，否则需要 Copy 数据
        /// </summary>
        private bool isGetMessageStream;
        /// <summary>
        /// 设置消息回调信息
        /// </summary>
        /// <param name="onGetMessage">获取消息回调委托</param>
        /// <param name="sendCount">当前可发送数量</param>
        /// <param name="isGetMessageStream">是否反序列化网络流，否则需要 Copy 数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(Func<AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter>, bool> onGetMessage, int sendCount, bool isGetMessageStream)
        {
            OnGetMessage = onGetMessage;
            maxSendCount = this.sendCount = sendCount;
            this.isGetMessageStream = isGetMessageStream;
        }
        /// <summary>
        /// 消息回调
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="data"></param>
        /// <param name="isNext"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Send(ulong identity, ref ValueData.Data data, ref bool isNext)
        {
            data.IsReturnDeSerializeStream = isGetMessageStream;
            if (OnGetMessage(new IdentityReturnParameter(identity, ref data)))
            {
                if (--sendCount == 0)
                {
                    isNext = true;
                    sendCount = maxSendCount;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnDispose()
        {
            OnGetMessage(new IdentityReturnParameter(ReturnType.MessageQueueNotFoundReader));
        }
    }
}
