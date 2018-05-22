using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 数组节点
    /// </summary>
    /// <typeparam name="nodeType">元素节点类型</typeparam>
    public abstract partial class Array<nodeType> : Collections
        where nodeType : Node
    {
        /// <summary>
        /// 数组节点
        /// </summary>
        /// <param name="parent">父节点</param>
        protected Array(Node parent) : base(parent) { }
        /// <summary>
        /// 创建数据节点
        /// </summary>
        /// <returns></returns>
        internal override Abstract.Node CreateValueNode()
        {
            return nodeConstructor(this).CreateValueNode();
        }
        /// <summary>
        /// 获取数组长度
        /// </summary>
        public AutoCSer.CacheServer.ReturnValue<int> Length
        {
            get { return Count; }
        }

        /// <summary>
        /// 获取操作元素节点
        /// </summary>
        /// <param name="index"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        internal Parameter.OperationBool Get(int index, OperationParameter.OperationType operationType)
        {
            Parameter.OperationBool node = new Parameter.OperationBool(this, operationType);
            node.Parameter.Set(index);
            return node;
        }
        /// <summary>
        /// 清除元素节点数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetSetDefaultNode(int index)
        {
            return Get(index, OperationParameter.OperationType.Remove);
        }
        /// <summary>
        /// 清除元素节点数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public AutoCSer.CacheServer.ReturnValue<bool> SetDefault(int index)
        {
            return Client.GetBool(ClientDataStructure.Client.Operation(GetSetDefaultNode(index)));
        }
        /// <summary>
        /// 清除元素节点数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onRemove"></param>
        /// <returns></returns>
        public void SetDefault(int index, Action<AutoCSer.CacheServer.ReturnValue<bool>> onRemove)
        {
            ClientDataStructure.Client.Operation(GetSetDefaultNode(index), onRemove);
        }
        /// <summary>
        /// 清除元素节点数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onRemove"></param>
        /// <returns></returns>
        public void SetDefault(int index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onRemove)
        {
            ClientDataStructure.Client.OperationReturnParameter(GetSetDefaultNode(index), onRemove);
        }
        /// <summary>
        /// 清除元素节点数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onRemove">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void SetDefaultStream(int index, Action<AutoCSer.CacheServer.ReturnValue<bool>> onRemove)
        {
            if (onRemove == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetSetDefaultNode(index), onRemove);
        }
        /// <summary>
        /// 清除元素节点数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onRemove">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void SetDefaultStream(int index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onRemove)
        {
            if (onRemove == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetSetDefaultNode(index), onRemove);
        }

        /// <summary>
        /// 子节点构造函数
        /// </summary>
        protected static readonly Func<Node, nodeType> nodeConstructor;
        static Array()
        {
            nodeConstructor = (Func<Node, nodeType>)typeof(nodeType).GetField(Cache.Node.ConstructorFieldName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetValue(null);
        }
    }
}
