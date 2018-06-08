using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 哈希表节点 短路径
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed partial class HashSet<valueType> : Collections<HashSet<valueType>>
        where valueType : IEquatable<valueType>
    {
        /// <summary>
        /// 哈希表节点 短路径
        /// </summary>
        /// <param name="node"></param>
        internal HashSet(DataStructure.Abstract.HashSet<valueType> node) : base(node) { }

        /// <summary>
        /// 获取查询节点
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryBool GetContainsKeyNode(valueType value)
        {
            Parameter.QueryBool node = new Parameter.QueryBool(this, OperationParameter.OperationType.ContainsKey);
            ValueData.Data<valueType>.SetData(ref node.Parameter, value);
            return node;
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value">匹配数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<bool> Contains(valueType value)
        {
            return Client.GetBool(Client.Query(GetContainsKeyNode(value)));
        }
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Contains(valueType value, Action<ReturnValue<bool>> onReturn)
        {
            if (onReturn == null) throw new ArgumentNullException();
            Client.Query(GetContainsKeyNode(value), onReturn);
        }
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <param name="onReturn">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void ContainsStream(valueType value, Action<ReturnValue<bool>> onReturn)
        {
            if (onReturn == null) throw new ArgumentNullException();
            Client.QueryStream(GetContainsKeyNode(value), onReturn);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        private Parameter.OperationBool getOperation(valueType value, OperationParameter.OperationType operationType)
        {
            Parameter.OperationBool node = new Parameter.OperationBool(this, operationType);
            ValueData.Data<valueType>.SetData(ref node.Parameter, value);
            return node;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetRemoveNode(valueType value)
        {
            return getOperation(value, OperationParameter.OperationType.Remove);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ReturnValue<bool> Remove(valueType value)
        {
            return Client.GetBool(Client.Operation(GetRemoveNode(value)));
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="onRemove"></param>
        /// <returns></returns>
        public void Remove(valueType value, Action<ReturnValue<bool>> onRemove)
        {
            if (onRemove == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetRemoveNode(value), onRemove);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="onRemove"></param>
        /// <returns></returns>
        public void Remove(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onRemove)
        {
            if (onRemove == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetRemoveNode(value), onRemove);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="onRemove">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void RemoveStream(valueType value, Action<ReturnValue<bool>> onRemove)
        {
            if (onRemove == null) throw new ArgumentNullException();
            Client.OperationStream(GetRemoveNode(value), onRemove);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="onRemove">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void RemoveStream(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onRemove)
        {
            if (onRemove == null) throw new ArgumentNullException();
            Client.OperationStream(GetRemoveNode(value), onRemove);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetAddNode(valueType value)
        {
            return getOperation(value, OperationParameter.OperationType.SetValue);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ReturnValue<bool> Add(valueType value)
        {
            return Client.GetBool(Client.Operation(GetAddNode(value)));
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="onAdd"></param>
        /// <returns></returns>
        public void Add(valueType value, Action<ReturnValue<bool>> onAdd)
        {
            if (onAdd == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetAddNode(value), onAdd);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="onAdd"></param>
        /// <returns></returns>
        public void Add(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onAdd)
        {
            if (onAdd == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetAddNode(value), onAdd);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="onAdd">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void AddStream(valueType value, Action<ReturnValue<bool>> onAdd)
        {
            if (onAdd == null) throw new ArgumentNullException();
            Client.OperationStream(GetAddNode(value), onAdd);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="onAdd">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void AddStream(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onAdd)
        {
            if (onAdd == null) throw new ArgumentNullException();
            Client.OperationStream(GetAddNode(value), onAdd);
        }
    }
}
