using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Lock
{
    /// <summary>
    /// 锁管理对象
    /// </summary>
    public abstract class ManagerBase : IDisposable
    {
        /// <summary>
        /// 锁节点
        /// </summary>
        protected readonly DataStructure.Lock node;
        /// <summary>
        /// 锁超时时钟周期
        /// </summary>
        protected readonly long timeoutTicks;
        /// <summary>
        /// 锁序号
        /// </summary>
        protected ulong randomNo;
        /// <summary>
        /// 锁过期时间
        /// </summary>
        protected DateTime timeout;
        /// <summary>
        /// 当前申请步骤
        /// </summary>
        internal volatile Step Step;
        /// <summary>
        /// 锁管理对象
        /// </summary>
        /// <param name="node">锁节点</param>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数</param>
        internal ManagerBase(DataStructure.Lock node, uint timeoutMilliseconds)
        {
            this.node = node;
            timeoutTicks = DataStructure.Lock.FotmatTimeoutTicks(timeoutMilliseconds);
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Exit()
        {
            if (Step == Step.Lock)
            {
                Step = Step.Exit;
                exit();
            }
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void exit()
        {
            if (Date.NowTime.Now.Ticks >= timeoutTicks) node.ClientDataStructure.Client.MasterClient.QueryAsynchronous(new OperationParameter.QueryNode { Node = node.GetExitNode(randomNo) }, null);
            else node.ClientDataStructure.Client.MasterClient.QueryAsynchronous(new OperationParameter.QueryNode { Node = node.GetExitNode(randomNo) }, onExit);
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="returnParameter"></param>
        private void onExit(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> returnParameter)
        {
            if (returnParameter.Type != Net.TcpServer.ReturnType.Success && Date.NowTime.Now.Ticks < timeoutTicks) AutoCSer.Threading.TimerTask.Default.Add(exit, Date.NowTime.Now.AddTicks(TimeSpan.TicksPerSecond));
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Exit();
        }
    }
}
