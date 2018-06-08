using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Value
{
    /// <summary>
    /// Json 数据节点
    /// </summary>
    public sealed partial class Json<valueType>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<Json<valueType>>> QueryTask()
        {
            return new ReturnValue<Json<valueType>>(await ClientDataStructure.Client.QueryAwaiter(GetQueryNode()));
        }
    }
}
