using System;
using System.Threading;

namespace AutoCSer.CacheServer.Lock
{
    /// <summary>
    /// 锁管理对象
    /// </summary>
    public sealed class TimeoutManager : ManagerBase
    {
        /// <summary>
        /// 申请锁等待事件
        /// </summary>
        private readonly AutoResetEvent enterWait = new AutoResetEvent(false);
        /// <summary>
        /// 申请锁
        /// </summary>
        private int enterLock;
        /// <summary>
        /// 申请锁返回值
        /// </summary>
        private AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> returnParameter;
        /// <summary>
        /// 锁管理对象
        /// </summary>
        /// <param name="node">锁节点</param>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数</param>
        internal TimeoutManager(DataStructure.Lock node, uint timeoutMilliseconds) : base(node, timeoutMilliseconds)
        {
            node.ClientDataStructure.Client.MasterQueryAsynchronous(node.GetEnterNode(timeoutTicks), enter);
        }
        /// <summary>
        /// 申请锁回调
        /// </summary>
        /// <param name="returnParameter"></param>
        private void enter(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> returnParameter)
        {
            try
            {
                if (Interlocked.CompareExchange(ref enterLock, 1, 0) == 0)
                {
                    this.returnParameter = returnParameter;
                    if (returnParameter.Value.Parameter.ReturnType == ReturnType.Success)
                    {
                        randomNo = returnParameter.Value.Parameter.Int64.ULong;
                        timeout = Date.NowTime.Now.AddTicks(timeoutTicks - TimeSpan.TicksPerSecond);
                        if (Step == Step.None)
                        {
                            Step = Step.Lock;
                            return;
                        }
                        exit();
                    }
                }
                else if (returnParameter.Value.Parameter.ReturnType == ReturnType.Success)
                {
                    randomNo = returnParameter.Value.Parameter.Int64.ULong;
                    timeout = Date.NowTime.Now.AddTicks(timeoutTicks - TimeSpan.TicksPerSecond);
                    exit();
                }
            }
            finally { enterWait.Set(); }
        }
        /// <summary>
        /// 等待申请锁
        /// </summary>
        /// <param name="timeoutMilliseconds">申请超时毫秒数</param>
        /// <returns></returns>
        internal ReturnValue<TimeoutManager> Wait(int timeoutMilliseconds)
        {
            if (!enterWait.WaitOne(timeoutMilliseconds) && Interlocked.CompareExchange(ref enterLock, 1, 0) == 0) return new ReturnValue<TimeoutManager> { Type = ReturnType.EnterLockTimeout };
            if (Step == Step.Lock) return this;
            return new ReturnValue<TimeoutManager> { Type = returnParameter.Value.Parameter.ReturnType, TcpReturnType = returnParameter.Type };
        }
    }
}
