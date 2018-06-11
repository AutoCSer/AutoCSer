using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 查询参数节点
    /// </summary>
    public sealed partial class QueryAsynchronousReturnArray<valueType>
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<valueType[]>> QueryTask()
        {
            return ReturnArray<valueType>.Get(await Parent.ClientDataStructure.Client.QueryAsynchronousAwaiter(this));
        }
    }
}
