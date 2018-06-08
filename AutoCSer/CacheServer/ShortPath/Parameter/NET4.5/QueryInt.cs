using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.ShortPath.Parameter
{
    /// <summary>
    /// 查询参数节点
    /// </summary>
    public sealed partial class QueryInt
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<int>> QueryTask()
        {
            return Client.GetInt(await ShortPath.Client.QueryAwaiter(this));
        }
    }
}
