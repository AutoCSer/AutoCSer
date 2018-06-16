using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.Lock
{
    /// <summary>
    /// 锁链表节点
    /// </summary>
    internal sealed class LinkNode
    {
        /// <summary>
        /// 超时释放时钟周期
        /// </summary>
        private readonly long timeOutTicks;
        /// <summary>
        /// 锁节点
        /// </summary>
        private readonly Node node;
        /// <summary>
        /// 下一个节点
        /// </summary>
        internal LinkNode Next;
        /// <summary>
        /// 申请锁返回调用委托
        /// </summary>
        internal readonly Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> OnReturn;
        /// <summary>
        /// 锁链表节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parser"></param>
        internal LinkNode(Node node, ref OperationParameter.NodeParser parser)
        {
            OnReturn = parser.OnReturn;
            this.node = node;
            timeOutTicks = parser.ValueData.Int64.Long;
            parser.OnReturn = null;
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <returns></returns>
        internal bool Enter()
        {
            ReturnParameter returnParameter = new ReturnParameter();
            returnParameter.Parameter.ReturnParameterSet(++node.RandomNo);
            if (OnReturn(returnParameter))
            {
                AutoCSer.Threading.TimerTask.Default.Add(onTimeout, Date.NowTime.Set().AddTicks(timeOutTicks));
                return true;
            }
            return false;
        }
        /// <summary>
        /// 超时处理
        /// </summary>
        private void onTimeout()
        {
            node.OnTimeout(this);
        }
        /// <summary>
        /// 释放锁节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal LinkNode DisposeLock()
        {
            OnReturn(new ReturnParameter { Parameter = new ValueData.Data { ReturnType = ReturnType.Disposed } });
            return Next;
        }
    }
}
