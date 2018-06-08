using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 集合节点 短路径
    /// </summary>
    public abstract partial class Collections<nodeType>
    {
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<int>> GetCountTask()
        {
            return CacheServer.Client.GetInt(await Client.QueryAwaiter(GetCountNode()));
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> ClearTask()
        {
            return CacheServer.Client.GetBool(await Client.OperationAwaiter(GetClearNode()));
        }
    }
}
