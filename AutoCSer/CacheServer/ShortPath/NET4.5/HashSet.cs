using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 哈希表节点 短路径
    /// </summary>
    public sealed partial class HashSet<valueType>
    {
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> ContainsTask(valueType value)
        {
            return Client.GetBool(await Client.QueryAwaiter(GetContainsKeyNode(value)));
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> RemoveTask(valueType value)
        {
            return Client.GetBool(await Client.OperationAwaiter(GetRemoveNode(value)));
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> AddTask(valueType value)
        {
            return Client.GetBool(await Client.OperationAwaiter(GetAddNode(value)));
        }
    }
}
