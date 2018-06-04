using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 数组节点
    /// </summary>
    /// <typeparam name="valueType">元素节点类型</typeparam>
    public abstract partial class ValueArray<valueType> : Array, IValueNode
    {
        /// <summary>
        /// 获取元素节点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public valueType this[int index]
        {
            get
            {
                ReturnValue<valueType> value = Get(index);
                if(value.Type != ReturnType.Success) throw new Exception(value.Type.ToString());
                return value.Value;
            }
            set
            {
                ReturnValue<bool> isSet = Set(index, value);
                if (!isSet) throw new Exception(isSet.Type.ToString());
            }
        }
        /// <summary>
        /// 数组节点
        /// </summary>
        /// <param name="parent">父节点</param>
        protected ValueArray(Node parent) : base(parent) { }
        /// <summary>
        /// 创建数据节点
        /// </summary>
        /// <returns></returns>
        internal override Abstract.Node CreateValueNode()
        {
            return this;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryReturnValue<valueType> GetNode(int index)
        {
            Parameter.QueryReturnValue<valueType> node = new Parameter.QueryReturnValue<valueType>(this, OperationParameter.OperationType.GetValue);
            node.Parameter.Set(index);
            return node;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ReturnValue<valueType> Get(int index)
        {
            return new ReturnValue<valueType>(ClientDataStructure.Client.Query(GetNode(index)));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onGet"></param>
        public void Get(int index, Action<ReturnValue<valueType>> onGet)
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
        public void GetStream(int index, Action<ReturnValue<valueType>> onGet)
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
        /// <param name="value"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        private Parameter.OperationBool getOperation(int index, valueType value, OperationParameter.OperationType operationType)
        {
            Abstract.Node valueNode = ValueData.Data<valueType>.ToNode(this, value);
            if (valueNode != null)
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
        public Parameter.OperationBool GetSetNode(int index, valueType valueNode)
        {
            return getOperation(index, valueNode, OperationParameter.OperationType.SetValue);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        public ReturnValue<bool> Set(int index, valueType valueNode)
        {
            Parameter.OperationBool node = GetSetNode(index, valueNode);
            if (node != null) return Client.GetBool(ClientDataStructure.Client.Operation(node));
            return new ReturnValue<bool> { Type = ReturnType.NodeParentSetError };
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="valueNode"></param>
        /// <param name="onSet"></param>
        /// <returns></returns>
        public void Set(int index, valueType valueNode, Action<ReturnValue<bool>> onSet)
        {
            Parameter.OperationBool node = GetSetNode(index, valueNode);
            if (node != null) ClientDataStructure.Client.Operation(node, onSet);
            else if (onSet != null) onSet(new ReturnValue<bool> { Type = ReturnType.NodeParentSetError });
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="valueNode"></param>
        /// <param name="onSet"></param>
        /// <returns></returns>
        public void Set(int index, valueType valueNode, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
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
        public void SetStream(int index, valueType valueNode, Action<ReturnValue<bool>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Parameter.OperationBool node = GetSetNode(index, valueNode);
            if (node != null) ClientDataStructure.Client.OperationStream(node, onSet);
            else onSet(new ReturnValue<bool> { Type = ReturnType.NodeParentSetError });
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="valueNode"></param>
        /// <param name="onSet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void SetStream(int index, valueType valueNode, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Parameter.OperationBool node = GetSetNode(index, valueNode);
            if (node != null) ClientDataStructure.Client.OperationStream(node, onSet);
            else onSet(new AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> { Type = Net.TcpServer.ReturnType.ClientException, Value = new ReturnParameter { Type = ReturnType.NodeParentSetError } });
        }

        static ValueArray()
        {
            ValueData.Data<valueType>.CheckValueType();
        }
    }
}
