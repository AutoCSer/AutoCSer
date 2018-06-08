using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 数组节点 短路径
    /// </summary>
    public sealed partial class Array<valueType>
    {
        /// <summary>
        /// 清除元素节点数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> SetDefaultTask(int index)
        {
            return Client.GetBool(await Client.OperationAwaiter(GetSetDefaultNode(index)));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<ReturnValue<valueType>> GetTask(int index)
        {
            return new ReturnValue<valueType>(await Client.QueryAwaiter(GetNode(index)));
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> SetTask(int index, valueType value)
        {
            return Client.GetBool(await Client.OperationAwaiter(GetSetNode(index, value)));
        }
    }
}
