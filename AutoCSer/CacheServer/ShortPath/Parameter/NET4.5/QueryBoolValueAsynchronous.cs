using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.ShortPath.Parameter
{
    /// <summary>
    /// 查询参数节点
    /// </summary>
    public partial struct QueryBoolValueAsynchronous
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> QueryTask()
        {
            return Client.GetBool(await node.ShortPath.Client.QueryAsynchronousAwaiter(node));
        }
    }
}
