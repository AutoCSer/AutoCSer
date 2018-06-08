using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 链表节点 短路径
    /// </summary>
    public sealed partial class Link<valueType>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <returns></returns>
        public async Task<ReturnValue<valueType>> GetTask(int index)
        {
            return new ReturnValue<valueType>(await Client.QueryAwaiter(GetNode(index)));
        }
        /// <summary>
        /// 删除元素节点
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> RemoveTask(int index)
        {
            return Client.GetBool(await Client.OperationAwaiter(GetRemoveNode(index)));
        }
        /// <summary>
        /// 获取并删除数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <returns></returns>
        public async Task<ReturnValue<valueType>> GetRemoveTask(int index)
        {
            return new ReturnValue<valueType>(await Client.OperationAwaiter(GetGetRemoveNode(index)));
        }
        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public async Task<ReturnValue<valueType>> StackPopTask()
        {
            return await GetRemoveTask(-1);
        }
        /// <summary>
        /// 弹出第一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public async Task<ReturnValue<valueType>> DequeueTask()
        {
            return await GetRemoveTask(0);
        }
        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> InsertBeforeTask(int index, valueType value)
        {
            return Client.GetBool(await Client.OperationAwaiter(GetInsertBeforeNode(index, value)));
        }
        /// <summary>
        /// 后置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> InsertAfterTask(int index, valueType value)
        {
            return Client.GetBool(await Client.OperationAwaiter(GetInsertAfterNode(index, value)));
        }
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public async Task<ReturnValue<bool>> AppendTask(valueType value)
        {
            return await InsertAfterTask(-1, value);
        }
    }
}
