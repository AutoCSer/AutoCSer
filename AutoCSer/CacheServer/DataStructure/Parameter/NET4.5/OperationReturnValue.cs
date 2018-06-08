using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 操作参数节点
    /// </summary>
    public sealed partial class OperationReturnValue<valueType>
    {
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<valueType>> OperationTask()
        {
            return new ReturnValue<valueType>(await Parent.ClientDataStructure.Client.OperationAwaiter(this));
        }
    }
}

