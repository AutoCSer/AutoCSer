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
        public async Task<ReturnValueNode<Json<valueType>>> GetTask()
        {
            return new ReturnValueNode<Json<valueType>>(await ClientDataStructure.Client.QueryTask(GetNode()));
        }
    }
}
