using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 操作参数节点
    /// </summary>
    public sealed partial class OperationReturnValue<nodeType>
    {
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValueNode<nodeType>> OperationTask()
        {
            return new ReturnValueNode<nodeType>(await ClientDataStructure.Client.OperationTask(this));
        }
    }
}

