using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Value
{
    /// <summary>
    /// 二进制序列化数据节点
    /// </summary>
    public sealed partial class Binary<valueType>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<Binary<valueType>>> QueryTask()
        {
            return new ReturnValue<Binary<valueType>>(await ClientDataStructure.Client.QueryAwaiter(GetQueryNode()));
        }
    }
}
