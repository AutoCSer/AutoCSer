using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 字典节点 短路径
    /// </summary>
    public abstract partial class DictionaryBase<keyType, nodeType>
    {
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> ContainsKeyTask(keyType key)
        {
            return Client.GetBool(await Client.QueryAwaiter(GetContainsKeyNode(key)));
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> RemoveTask(keyType key)
        {
            return Client.GetBool(await Client.OperationAwaiter(GetRemoveNode(key)));
        }
    }
}
