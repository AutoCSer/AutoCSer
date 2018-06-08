using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 操作参数节点
    /// </summary>
    public sealed partial class OperationBool
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> OperationTask()
        {
            return Client.GetBool(await Parent.ClientDataStructure.Client.OperationAwaiter(this));
        }
    }
}

