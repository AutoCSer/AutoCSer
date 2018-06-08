using System;
using System.Threading.Tasks;


namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 字典节点
    /// </summary>
    public abstract partial class Dictionary<keyType>
    {
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> ContainsKeyTask(keyType key)
        {
            return Client.GetBool(await ClientDataStructure.Client.QueryAwaiter(GetContainsKeyNode(key)));
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> RemoveTask(keyType key)
        {
            return Client.GetBool(await ClientDataStructure.Client.OperationAwaiter(GetRemoveNode(key)));
        }
    }
}
