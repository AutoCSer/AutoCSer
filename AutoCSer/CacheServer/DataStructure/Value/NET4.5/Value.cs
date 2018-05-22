using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Value
{
    /// <summary>
    /// 数据节点
    /// </summary>
    public sealed partial class Value<valueType>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValueNode<Value<valueType>>> GetTask()
        {
            return new ReturnValueNode<Value<valueType>>(await ClientDataStructure.Client.QueryTask(GetNode()));
        }
    }
}
