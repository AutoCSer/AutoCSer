using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 字典节点 短路径
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="nodeType">节点类型</typeparam>
    public abstract partial class DictionaryBase<keyType, nodeType> : Collections<nodeType>
        where keyType : IEquatable<keyType>
        where nodeType : DictionaryBase<keyType, nodeType>
    {
        /// <summary>
        /// 字典节点 短路径
        /// </summary>
        /// <param name="node"></param>
        internal DictionaryBase(DataStructure.Abstract.Dictionary<keyType> node) : base(node) { }

        /// <summary>
        /// 获取查询节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryBool GetContainsKeyNode(keyType key)
        {
            Parameter.QueryBool node = new Parameter.QueryBool(this, OperationParameter.OperationType.ContainsKey);
            ValueData.Data<keyType>.SetData(ref node.Parameter, key);
            return node;
        }
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">关键字</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.CacheServer.ReturnValue<bool> ContainsKey(keyType key)
        {
            return Client.GetBool(Client.Query(GetContainsKeyNode(key)));
        }
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void ContainsKey(keyType key, Action<AutoCSer.CacheServer.ReturnValue<bool>> onReturn)
        {
            if (onReturn == null) throw new ArgumentNullException();
            Client.Query(GetContainsKeyNode(key), onReturn);
        }
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="onReturn">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void ContainsKeyStream(keyType key, Action<AutoCSer.CacheServer.ReturnValue<bool>> onReturn)
        {
            if (onReturn == null) throw new ArgumentNullException();
            Client.QueryStream(GetContainsKeyNode(key), onReturn);
        }

        /// <summary>
        /// 获取删除节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetRemoveNode(keyType key)
        {
            Parameter.OperationBool node = new Parameter.OperationBool(this, OperationParameter.OperationType.Remove);
            ValueData.Data<keyType>.SetData(ref node.Parameter, key);
            return node;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.CacheServer.ReturnValue<bool> Remove(keyType key)
        {
            return Client.GetBool(Client.Operation(GetRemoveNode(key)));
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Remove(keyType key, Action<AutoCSer.CacheServer.ReturnValue<bool>> onReturn)
        {
            if (onReturn == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetRemoveNode(key), onReturn);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Remove(keyType key, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (onReturn == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetRemoveNode(key), onReturn);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="onReturn">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void RemoveStream(keyType key, Action<AutoCSer.CacheServer.ReturnValue<bool>> onReturn)
        {
            if (onReturn == null) throw new ArgumentNullException();
            Client.OperationStream(GetRemoveNode(key), onReturn);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="onReturn">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void RemoveStream(keyType key, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (onReturn == null) throw new ArgumentNullException();
            Client.OperationStream(GetRemoveNode(key), onReturn);
        }
    }
}
