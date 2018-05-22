using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Value
{
    /// <summary>
    /// 整数数据节点
    /// </summary>
    public sealed partial class Integer<valueType>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValueNode<Integer<valueType>>> GetTask()
        {
            return new ReturnValueNode<Integer<valueType>>(await ClientDataStructure.Client.QueryTask(GetNode()));
        }
    }
}
