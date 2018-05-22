using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 数组节点
    /// </summary>
    /// <typeparam name="nodeType">元素节点类型</typeparam>
    public abstract partial class ValueArray<nodeType> : Array<nodeType>
        where nodeType : Node, Abstract.IValue
    {
        /// <summary>
        /// 获取元素节点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public nodeType this[int index]
        {
            get
            {
                nodeType node = nodeConstructor(this);
                node.Parameter.Set(index);
                return node;
            }
            set
            {
                AutoCSer.CacheServer.ReturnValue<bool> isSet = Set(index, value);
                if (!isSet) throw new Exception(isSet.Type.ToString());
            }
        }
        /// <summary>
        /// 数组节点
        /// </summary>
        /// <param name="parent">父节点</param>
        protected ValueArray(Node parent) : base(parent) { }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryReturnValue<nodeType> GetNode(int index)
        {
            Parameter.QueryReturnValue<nodeType> node = new Parameter.QueryReturnValue<nodeType>(this, OperationParameter.OperationType.GetValue);
            node.Parameter.Set(index);
            return node;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ReturnValueNode<nodeType> Get(int index)
        {
            return new ReturnValueNode<nodeType>(ClientDataStructure.Client.Query(GetNode(index)));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onGet"></param>
        public void Get(int index, Action<ReturnValueNode<nodeType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetNode(index), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onGet"></param>
        public void Get(int index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetNode(index), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetStream(int index, Action<ReturnValueNode<nodeType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryStream(GetNode(index), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetStream(int index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryStream(GetNode(index), onGet);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="valueNode"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        private Parameter.OperationBool getOperation(int index, nodeType valueNode, OperationParameter.OperationType operationType)
        {
            if (valueNode.TrySetParent(this))
            {
                Parameter.OperationBool node = new Parameter.OperationBool(valueNode, operationType);
                node.Parameter.Set(index);
                return node;
            }
            return null;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetSetNode(int index, nodeType valueNode)
        {
            return getOperation(index, valueNode, OperationParameter.OperationType.SetValue);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        public AutoCSer.CacheServer.ReturnValue<bool> Set(int index, nodeType valueNode)
        {
            Parameter.OperationBool node = GetSetNode(index, valueNode);
            if (node != null) return Client.GetBool(ClientDataStructure.Client.Operation(node));
            return new AutoCSer.CacheServer.ReturnValue<bool> { Type = ReturnType.NodeParentSetError };
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="valueNode"></param>
        /// <param name="onSet"></param>
        /// <returns></returns>
        public void Set(int index, nodeType valueNode, Action<AutoCSer.CacheServer.ReturnValue<bool>> onSet)
        {
            Parameter.OperationBool node = GetSetNode(index, valueNode);
            if (node != null) ClientDataStructure.Client.Operation(node, onSet);
            else if (onSet != null) onSet(new AutoCSer.CacheServer.ReturnValue<bool> { Type = ReturnType.NodeParentSetError });
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="valueNode"></param>
        /// <param name="onSet"></param>
        /// <returns></returns>
        public void Set(int index, nodeType valueNode, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
        {
            Parameter.OperationBool node = GetSetNode(index, valueNode);
            if (node != null) ClientDataStructure.Client.OperationReturnParameter(node, onSet);
            else if (onSet != null) onSet(new AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> { Type = Net.TcpServer.ReturnType.ClientException, Value = new ReturnParameter { Type = ReturnType.NodeParentSetError } });
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="valueNode"></param>
        /// <param name="onSet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void SetStream(int index, nodeType valueNode, Action<AutoCSer.CacheServer.ReturnValue<bool>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Parameter.OperationBool node = GetSetNode(index, valueNode);
            if (node != null) ClientDataStructure.Client.OperationStream(node, onSet);
            else onSet(new AutoCSer.CacheServer.ReturnValue<bool> { Type = ReturnType.NodeParentSetError });
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="valueNode"></param>
        /// <param name="onSet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void SetStream(int index, nodeType valueNode, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Parameter.OperationBool node = GetSetNode(index, valueNode);
            if (node != null) ClientDataStructure.Client.OperationStream(node, onSet);
            else onSet(new AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> { Type = Net.TcpServer.ReturnType.ClientException, Value = new ReturnParameter { Type = ReturnType.NodeParentSetError } });
        }
    }
}
