using System;

namespace AutoCSer.CacheServer.Lock
{
    /// <summary>
    /// 锁管理对象
    /// </summary>
    public sealed class Manager : ManagerBase
    {
        /// <summary>
        /// 锁管理对象
        /// </summary>
        /// <param name="node">锁节点</param>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数</param>
        /// <param name="randomNo">锁序号</param>
        internal Manager(DataStructure.Lock node, uint timeoutMilliseconds, ref ReturnValue<ulong> randomNo) : base(node, timeoutMilliseconds)
        {
            timeout = Date.NowTime.Now.AddTicks(timeoutTicks - TimeSpan.TicksPerSecond);
            Step = Step.Lock;
            this.randomNo = randomNo.Value;
            randomNo.Type = ReturnType.Unknown;
        }
    }
}
