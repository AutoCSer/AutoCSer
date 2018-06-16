using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 字典节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="nodeType">数据节点类型</typeparam>
    public abstract partial class NodeDictionary<keyType, nodeType> : Dictionary<keyType, nodeType>
        where keyType : IEquatable<keyType>
        where nodeType : Node
    {
        /// <summary>
        /// 获取元素节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public nodeType this[keyType key]
        {
            get
            {
                nodeType node = nodeConstructor(this);
                ValueData.Data<keyType>.SetData(ref node.Parameter, key);
                return node;
            }
        }
        /// <summary>
        /// 字典节点
        /// </summary>
        /// <param name="parent">父节点</param>
        protected NodeDictionary(Node parent) : base(parent) { }

        /// <summary>
        /// 获取操作元素节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        private Parameter.OperationBool getOperation(keyType key, OperationParameter.OperationType operationType)
        {
            Parameter.OperationBool node = new Parameter.OperationBool(this, operationType);
            ValueData.Data<keyType>.SetData(ref node.Parameter, key);
            return node;
        }

        /// <summary>
        /// 获取或者创建元素节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetOrCreateNode(keyType key)
        {
            return getOperation(key, OperationParameter.OperationType.GetOrCreateNode);
        }
        /// <summary>
        /// 获取或者创建元素节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AutoCSer.CacheServer.ReturnValue<nodeType> GetOrCreate(keyType key)
        {
            AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> value = ClientDataStructure.Client.Operation(GetOrCreateNode(key));
            return value.Value.Parameter.GetBool(value.Type, value.Value.Parameter.Int64.Bool ? this[key] : null);
        }
        /// <summary>
        /// 获取或者创建元素节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        public void GetOrCreate(keyType key, Action<AutoCSer.CacheServer.ReturnValue<nodeType>> onGet)
        {
            if (onGet != null)
            {
                ClientDataStructure.Client.Operation(GetOrCreateNode(key),
                    value => value.Value.Parameter.GetBool(value.Type, value.Value.Parameter.Int64.Bool ? this[key] : null));
            }
            else ClientDataStructure.Client.OperationOnly(GetOrCreateNode(key));
        }

        ///// <summary>
        ///// 删除元素节点
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //public Parameter.OperationBool GetRemoveNode(keyType key)
        //{
        //    return getOperation(key, OperationParameter.OperationType.Remove);
        //}
        ///// <summary>
        ///// 删除元素节点
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public AutoCSer.CacheServer.ReturnValue<bool> Remove(keyType key)
        //{
        //    return Client.GetBool(ClientDataStructure.Client.Operation(GetRemoveNode(key)));
        //}
        ///// <summary>
        ///// 删除元素节点
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="onRemove"></param>
        ///// <returns></returns>
        //public void Remove(keyType key, Action<AutoCSer.CacheServer.ReturnValue<bool>> onRemove)
        //{
        //    ClientDataStructure.Client.Operation(GetRemoveNode(key), onRemove);
        //}
        ///// <summary>
        ///// 删除元素节点
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="onRemove">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        ///// <returns></returns>
        //public void RemoveStream(keyType key, Action<AutoCSer.CacheServer.ReturnValue<bool>> onRemove)
        //{
        //    ClientDataStructure.Client.OperationStream(GetRemoveNode(key), onRemove);
        //}
    }
}
