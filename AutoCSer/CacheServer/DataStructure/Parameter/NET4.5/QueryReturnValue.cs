using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 查询参数节点
    /// </summary>
    public sealed partial class QueryReturnValue<valueType>
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<valueType>> QueryTask()
        {
            return new ReturnValue<valueType>(await Parent.ClientDataStructure.Client.QueryAwaiter(this));
        }
    }
}

