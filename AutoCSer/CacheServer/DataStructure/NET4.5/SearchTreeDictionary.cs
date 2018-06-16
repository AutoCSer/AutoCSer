using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 搜索树字典节点
    /// </summary>
    public sealed partial class SearchTreeDictionary<keyType, nodeType>
    {
        /// <summary>
        /// 获取或者创建元素节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<nodeType>> GetOrCreateTask(keyType key)
        {
            AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = await ClientDataStructure.Client.OperationAwaiter(GetOrCreateNode(key));
            return value.Value.Parameter.GetBool(value.Type, value.Value.Parameter.Int64.Bool ? this[key] : null);
        }
        ///// <summary>
        ///// 删除元素节点
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public async Task<AutoCSer.CacheServer.ReturnValue<bool>> RemoveTask(keyType key)
        //{
        //    return Client.GetBool(await ClientDataStructure.Client.OperationTask(GetRemoveNode(key)));
        //}
    }
}
