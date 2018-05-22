using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Value
{
    /// <summary>
    /// 数字数据节点
    /// </summary>
    public sealed partial class Number<valueType>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValueNode<Number<valueType>>> GetTask()
        {
            return new ReturnValueNode<Number<valueType>>(await ClientDataStructure.Client.QueryTask(GetNode()));
        }
    }
}
