using System;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer.OperationUpdater
{
    /// <summary>
    /// 数字更新
    /// </summary>
    public partial struct Number<valueType>
    {
        /// <summary>
        /// 加法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> AddTask(valueType value)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, OperationType.Add)));
        }
        /// <summary>
        /// 加法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> AddTask(valueType value, Logic<valueType> logic)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, logic, OperationType.Add)));
        }
        /// <summary>
        /// 减法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> SubTask(valueType value)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, OperationType.Sub)));
        }
        /// <summary>
        /// 减法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> SubTask(valueType value, Logic<valueType> logic)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, logic, OperationType.Sub)));
        }
        /// <summary>
        /// 乘法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> MulTask(valueType value)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, OperationType.Mul)));
        }
        /// <summary>
        /// 乘法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> MulTask(valueType value, Logic<valueType> logic)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, logic, OperationType.Mul)));
        }
        /// <summary>
        /// 除法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> DivTask(valueType value)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, OperationType.Div)));
        }
        /// <summary>
        /// 除法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> DivTask(valueType value, Logic<valueType> logic)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, logic, OperationType.Div)));
        }
        /// <summary>
        /// 取余运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> ModTask(valueType value)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, OperationType.Mod)));
        }
        /// <summary>
        /// 取余运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        public async Task<ReturnValue<valueType>> ModTask(valueType value, Logic<valueType> logic)
        {
            return GetValue(await node.ClientDataStructure.Client.OperationAwaiter(getNode(value, logic, OperationType.Mod)));
        }
    }
}
