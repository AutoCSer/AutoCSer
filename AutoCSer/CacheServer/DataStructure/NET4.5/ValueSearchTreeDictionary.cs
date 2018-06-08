using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 搜索树字典节点
    /// </summary>
    public sealed partial class ValueSearchTreeDictionary<keyType, valueType>
    {
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnValue<ShortPath.SearchTreeDictionary<keyType, valueType>>> CreateShortPathTask()
        {
            return await new ShortPath.SearchTreeDictionary<keyType, valueType>(this).CreateTask();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ReturnValue<valueType>> GetTask(keyType key)
        {
            return new ReturnValue<valueType>(await ClientDataStructure.Client.QueryAwaiter(GetNode(key)));
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> SetTask(keyType key, valueType valueNode)
        {
            Parameter.OperationBool node = GetSetNode(key, valueNode);
            if (node != null) return Client.GetBool(await ClientDataStructure.Client.OperationAwaiter(node));
            return new ReturnValue<bool> { Type = ReturnType.NodeParentSetError };
        }
    }
}
