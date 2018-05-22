using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 数组节点
    /// </summary>
    public abstract partial class Array<nodeType>
    {
        /// <summary>
        /// 清除元素节点数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> SetDefaultTask(int index)
        {
            return Client.GetBool(await ClientDataStructure.Client.OperationTask(GetSetDefaultNode(index)));
        }
    }
}
