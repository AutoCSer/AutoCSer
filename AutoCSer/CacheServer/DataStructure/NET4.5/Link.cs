using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 链表节点
    /// </summary>
    public sealed partial class Link<nodeType>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <returns></returns>
        public async Task<ReturnValueNode<nodeType>> GetTask(int index)
        {
            return new ReturnValueNode<nodeType>(await ClientDataStructure.Client.QueryTask(GetNode(index)));
        }
        /// <summary>
        /// 删除元素节点
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> RemoveTask(int index)
        {
            return Client.GetBool(await ClientDataStructure.Client.OperationTask(GetRemoveNode(index)));
        }
        /// <summary>
        /// 获取并删除数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <returns></returns>
        public async Task<ReturnValueNode<nodeType>> GetRemoveTask(int index)
        {
            return new ReturnValueNode<nodeType>(await ClientDataStructure.Client.OperationTask(GetGetRemoveNode(index)));
        }
        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public async Task<ReturnValueNode<nodeType>> StackPopTask()
        {
            return await GetRemoveTask(-1);
        }
        /// <summary>
        /// 弹出第一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public async Task<ReturnValueNode<nodeType>> DequeueTask()
        {
            return await GetRemoveTask(0);
        }
        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> InsertBeforeTask(int index, nodeType valueNode)
        {
            Parameter.OperationBool node = GetInsertBeforeNode(index, valueNode);
            if (node != null) return Client.GetBool(await ClientDataStructure.Client.OperationTask(node));
            return new ReturnValue<bool> { Type = ReturnType.NodeParentSetError };
        }
        /// <summary>
        /// 后置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> InsertAfterTask(int index, nodeType valueNode)
        {
            Parameter.OperationBool node = GetInsertAfterNode(index, valueNode);
            if (node != null) return Client.GetBool(await ClientDataStructure.Client.OperationTask(node));
            return new ReturnValue<bool> { Type = ReturnType.NodeParentSetError };
        }
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public async Task<ReturnValue<bool>> AppendTask(nodeType valueNode)
        {
            return await InsertAfterTask(-1, valueNode);
        }
    }
}
