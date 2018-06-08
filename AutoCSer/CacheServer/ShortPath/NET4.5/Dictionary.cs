using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 字典节点 短路径
    /// </summary>
    public sealed partial class Dictionary<keyType, valueType>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ReturnValue<valueType>> GetTask(keyType key)
        {
            return new ReturnValue<valueType>(await Client.QueryAwaiter(GetNode(key)));
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> SetTask(keyType key, valueType valueNode)
        {
            return Client.GetBool(await Client.OperationAwaiter(GetSetNode(key, valueNode)));
        }
    }
}
