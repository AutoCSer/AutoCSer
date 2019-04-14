using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Lock
{
    /// <summary>
    /// 锁管理对象
    /// </summary>
    public sealed class AsynchronousManager : ManagerBase
    {
        /// <summary>
        /// 获取锁以后的回调
        /// </summary>
        private readonly Action<ReturnValue<AsynchronousManager>> onEnter;
        /// <summary>
        /// 锁管理对象
        /// </summary>
        /// <param name="node">锁节点</param>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数</param>
        /// <param name="onEnter">获取锁以后的回调</param>
        internal AsynchronousManager(DataStructure.Lock node, uint timeoutMilliseconds, Action<ReturnValue<AsynchronousManager>> onEnter) : base(node, timeoutMilliseconds)
        {
            this.onEnter = onEnter;
        }
        /// <summary>
        /// 返回错误
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Error()
        {
            Step = Step.Exit;
            onEnter(default(ReturnValue<AsynchronousManager>));
        }
        /// <summary>
        /// 申请锁回调
        /// </summary>
        /// <param name="returnParameter"></param>
        private void enter(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> returnParameter)
        {
            if (returnParameter.Value.Parameter.ReturnType == ReturnType.Success)
            {
                randomNo = returnParameter.Value.Parameter.Int64.ULong;
                timeout = Date.NowTime.Now.AddTicks(timeoutTicks - TimeSpan.TicksPerSecond);
                if (Step == Step.None)
                {
                    Step = Step.Lock;
                    onEnter(this);
                }
                else exit();
            }
            else onEnter(new ReturnValue<AsynchronousManager> { Type = returnParameter.Value.Parameter.ReturnType, TcpReturnType = returnParameter.Type });
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Enter()
        {
            node.ClientDataStructure.Client.MasterQueryAsynchronous(node.GetEnterNode(timeoutTicks), enter);
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void EnterStream()
        {
            node.ClientDataStructure.Client.MasterQueryAsynchronousStream(node.GetEnterNode(timeoutTicks), enter);
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void TryEnter()
        {
            node.ClientDataStructure.Client.MasterQueryAsynchronous(node.GetTryEnterNode(timeoutTicks), enter);
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void TryEnterStream()
        {
            node.ClientDataStructure.Client.MasterQueryAsynchronousStream(node.GetTryEnterNode(timeoutTicks), enter);
        }
    }
}
