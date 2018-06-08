using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 锁节点
    /// </summary>
    public sealed partial class Lock
    {
        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <returns></returns>
        public async Task<ReturnValue<ulong>> TryEnterTask(uint timeoutMilliseconds)
        {
            return Client.GetULong(await ClientDataStructure.Client.MasterQueryAsynchronousAwaiter(GetTryEnterNode(timeoutMilliseconds)));
        }
        /// <summary>
        /// 申请锁序号
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <returns></returns>
        public async Task<ReturnValue<ulong>> EnterTask(uint timeoutMilliseconds)
        {
            return Client.GetULong(await ClientDataStructure.Client.MasterQueryAsynchronousAwaiter(GetEnterNode(timeoutMilliseconds)));
        }
        /// <summary>
        /// 释放锁序号
        /// </summary>
        /// <param name="randomNo">锁序号</param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> ExitTask(ulong randomNo)
        {
            return Client.GetBool(await ClientDataStructure.Client.MasterQueryAsynchronousAwaiter(GetExitNode(randomNo)));
        }

        /// <summary>
        /// 创建锁管理对象
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <returns></returns>
        public async Task<ReturnValue<AutoCSer.CacheServer.Lock.Manager>> GetTryEnterTask(uint timeoutMilliseconds)
        {
            return getEnter(timeoutMilliseconds, Client.GetULong(await ClientDataStructure.Client.MasterQueryAsynchronousAwaiter(GetTryEnterNode(timeoutMilliseconds))));
        }
        /// <summary>
        /// 创建锁管理对象
        /// </summary>
        /// <param name="timeoutMilliseconds">锁的超时毫秒数，默认为 60*1000</param>
        /// <returns></returns>
        public async Task<ReturnValue<AutoCSer.CacheServer.Lock.Manager>> GetEnterTask(uint timeoutMilliseconds)
        {
            return getEnter(timeoutMilliseconds, Client.GetULong(await ClientDataStructure.Client.MasterQueryAsynchronousAwaiter(GetEnterNode(timeoutMilliseconds))));
        }
    }
}
