using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.ShortPath.Parameter
{
    /// <summary>
    /// 查询参数节点
    /// </summary>
    public sealed partial class QueryBoolAsynchronous : Node
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> QueryTask()
        {
            return Client.GetBool(await ShortPath.Client.QueryAsynchronousAwaiter(this));
        }
    }
}
