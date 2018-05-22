using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 字典节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="nodeType">数据节点类型</typeparam>
    public abstract partial class ValueDictionary<keyType, nodeType> : Dictionary<keyType, nodeType>
        where keyType : IEquatable<keyType>
        where nodeType : Node, IValue
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
            set
            {
                ReturnValue<bool> isSet = Set(key, value);
                if (!isSet.Value) throw new Exception(isSet.Type.ToString());
            }
        }
        /// <summary>
        /// 字典节点
        /// </summary>
        /// <param name="parent">父节点</param>
        protected ValueDictionary(Node parent) : base(parent) { }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryReturnValue<nodeType> GetNode(keyType key)
        {
            Parameter.QueryReturnValue<nodeType> node = new Parameter.QueryReturnValue<nodeType>(this, OperationParameter.OperationType.GetValue);
            ValueData.Data<keyType>.SetData(ref node.Parameter, key);
            return node;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ReturnValueNode<nodeType> Get(keyType key)
        {
            return new ReturnValueNode<nodeType>(ClientDataStructure.Client.Query(GetNode(key)));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onGet"></param>
        public void Get(keyType key, Action<ReturnValueNode<nodeType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetNode(key), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onGet"></param>
        public void Get(keyType key, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetNode(key), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetStream(keyType key, Action<ReturnValueNode<nodeType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryStream(GetNode(key), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetStream(keyType key, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryStream(GetNode(key), onGet);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        private Parameter.OperationBool getOperation(keyType key, nodeType valueNode, OperationParameter.OperationType operationType)
        {
            if (valueNode.TrySetParent(this))
            {
                Parameter.OperationBool node = new Parameter.OperationBool(valueNode, operationType);
                ValueData.Data<keyType>.SetData(ref node.Parameter, key);
                return node;
            }
            return null;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetSetNode(keyType key, nodeType valueNode)
        {
            return getOperation(key, valueNode, OperationParameter.OperationType.SetValue);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        public AutoCSer.CacheServer.ReturnValue<bool> Set(keyType key, nodeType valueNode)
        {
            Parameter.OperationBool node = GetSetNode(key, valueNode);
            if (node != null) return Client.GetBool(ClientDataStructure.Client.Operation(node));
            return new AutoCSer.CacheServer.ReturnValue<bool> { Type = ReturnType.NodeParentSetError };
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <param name="onSet"></param>
        /// <returns></returns>
        public void Set(keyType key, nodeType valueNode, Action<AutoCSer.CacheServer.ReturnValue<bool>> onSet)
        {
            Parameter.OperationBool node = GetSetNode(key, valueNode);
            if (node != null) ClientDataStructure.Client.Operation(node, onSet);
            else if (onSet != null) onSet(new AutoCSer.CacheServer.ReturnValue<bool> { Type = ReturnType.NodeParentSetError });
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <param name="onSet"></param>
        /// <returns></returns>
        public void Set(keyType key, nodeType valueNode, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
        {
            Parameter.OperationBool node = GetSetNode(key, valueNode);
            if (node != null) ClientDataStructure.Client.OperationReturnParameter(node, onSet);
            else if (onSet != null) onSet(new AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> { Type = Net.TcpServer.ReturnType.ClientException, Value = new ReturnParameter { Type = ReturnType.NodeParentSetError } });
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <param name="onSet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void SetStream(keyType key, nodeType valueNode, Action<AutoCSer.CacheServer.ReturnValue<bool>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Parameter.OperationBool node = GetSetNode(key, valueNode);
            if (node != null) ClientDataStructure.Client.OperationStream(node, onSet);
            else onSet(new AutoCSer.CacheServer.ReturnValue<bool> { Type = ReturnType.NodeParentSetError });
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <param name="onSet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void SetStream(keyType key, nodeType valueNode, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Parameter.OperationBool node = GetSetNode(key, valueNode);
            if (node != null) ClientDataStructure.Client.OperationStream(node, onSet);
            else onSet(new AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> { Type = Net.TcpServer.ReturnType.ClientException, Value = new ReturnParameter { Type = ReturnType.NodeParentSetError } });
        }
    }
}
