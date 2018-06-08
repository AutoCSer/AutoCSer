using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 位图节点 短路径
    /// </summary>
    public sealed partial class Bitmap
    {
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <returns></returns>
        public async Task<AutoCSer.CacheServer.ReturnValue<bool>> ClearTask()
        {
            return Client.GetBool(await Client.OperationAwaiter(GetClearNode()));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> GetTask(uint index)
        {
            return Client.GetBool(await Client.QueryAwaiter(GetNode(index)));
        }
        /// <summary>
        /// 设置数据位
        /// </summary>
        /// <param name="index"></param>
        /// <returns>设置前的位状态</returns>
        public async Task<ReturnValue<bool>> SetTask(uint index)
        {
            return Client.GetBool(await Client.OperationAwaiter(GetSetNode(index)));
        }
        /// <summary>
        /// 清除数据位
        /// </summary>
        /// <param name="index"></param>
        /// <returns>清除前的位状态</returns>
        public async Task<ReturnValue<bool>> ClearTask(uint index)
        {
            return Client.GetBool(await Client.OperationAwaiter(GetClearNode(index)));
        }
        /// <summary>
        /// 数据位取反
        /// </summary>
        /// <param name="index"></param>
        /// <returns>取反后的结果</returns>
        public async Task<ReturnValue<bool>> SetNegateTask(uint index)
        {
            return Client.GetBool(await Client.OperationAwaiter(GetSetNegateNode(index)));
        }
    }
}
