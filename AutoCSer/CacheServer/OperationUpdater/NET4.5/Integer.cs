using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.OperationUpdater
{
    /// <summary>
    /// 整数更新
    /// </summary>
    public partial struct Integer<valueType>
    {
        /// <summary>
        /// 逻辑异或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> XorTask(valueType value)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, OperationType.Xor)));
        }
        /// <summary>
        /// 逻辑异或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> XorTask(valueType value, Logic<valueType> logic)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, logic, OperationType.Xor)));
        }
        /// <summary>
        /// 逻辑与运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> AndTask(valueType value)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, OperationType.And)));
        }
        /// <summary>
        /// 逻辑与运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> AndTask(valueType value, Logic<valueType> logic)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, logic, OperationType.And)));
        }
        /// <summary>
        /// 逻辑或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> OrTask(valueType value)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, OperationType.Or)));
        }
        /// <summary>
        /// 逻辑或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> OrTask(valueType value, Logic<valueType> logic)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, logic, OperationType.Or)));
        }
        /// <summary>
        /// 逻辑取反运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> NotTask(valueType value)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(default(valueType), OperationType.Not)));
        }
        /// <summary>
        /// 逻辑取反运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> NotTask(valueType value, Logic<valueType> logic)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(default(valueType), logic, OperationType.Not)));
        }
    }
}
