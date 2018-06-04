using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 搜索树字典节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">数据节点类型</typeparam>
    public sealed partial class ValueSearchTreeDictionary<keyType, valueType> : Abstract.Dictionary<keyType>, Abstract.IValueNode
        where keyType : IEquatable<keyType>, IComparable<keyType>
    {
        /// <summary>
        /// 获取元素节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public valueType this[keyType key]
        {
            get
            {
                ReturnValue<valueType> value = Get(key);
                if (value.Type != ReturnType.Success) throw new Exception(value.Type.ToString());
                return value.Value;
            }
        }
        /// <summary>
        /// 搜索树字典节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private ValueSearchTreeDictionary(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 创建数据节点
        /// </summary>
        /// <returns></returns>
        internal override Abstract.Node CreateValueNode()
        {
            return this;
        }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.ValueSearchTreeDictionary);
            stream.Write((byte)ValueData.Data<valueType>.DataType);
            stream.Write((byte)ValueData.Data<keyType>.DataType);
            serializeParentDataStructure(stream);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryReturnValue<valueType> GetNode(keyType key)
        {
            Parameter.QueryReturnValue<valueType> node = new Parameter.QueryReturnValue<valueType>(this, OperationParameter.OperationType.GetValue);
            ValueData.Data<keyType>.SetData(ref node.Parameter, key);
            return node;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ReturnValue<valueType> Get(keyType key)
        {
            return new ReturnValue<valueType>(ClientDataStructure.Client.Query(GetNode(key)));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onGet"></param>
        public void Get(keyType key, Action<ReturnValue<valueType>> onGet)
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
        public void GetStream(keyType key, Action<ReturnValue<valueType>> onGet)
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
        /// <param name="value"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        private Parameter.OperationBool getOperation(keyType key, valueType value, OperationParameter.OperationType operationType)
        {
            Abstract.Node valueNode = ValueData.Data<valueType>.ToNode(this, value);
            if (valueNode != null)
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
        public Parameter.OperationBool GetSetNode(keyType key, valueType valueNode)
        {
            return getOperation(key, valueNode, OperationParameter.OperationType.SetValue);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        public ReturnValue<bool> Set(keyType key, valueType valueNode)
        {
            Parameter.OperationBool node = GetSetNode(key, valueNode);
            if (node != null) return Client.GetBool(ClientDataStructure.Client.Operation(node));
            return new ReturnValue<bool> { Type = ReturnType.NodeParentSetError };
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <param name="onSet"></param>
        /// <returns></returns>
        public void Set(keyType key, valueType valueNode, Action<ReturnValue<bool>> onSet)
        {
            Parameter.OperationBool node = GetSetNode(key, valueNode);
            if (node != null) ClientDataStructure.Client.Operation(node, onSet);
            else if (onSet != null) onSet(new ReturnValue<bool> { Type = ReturnType.NodeParentSetError });
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <param name="onSet"></param>
        /// <returns></returns>
        public void Set(keyType key, valueType valueNode, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
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
        public void SetStream(keyType key, valueType valueNode, Action<ReturnValue<bool>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Parameter.OperationBool node = GetSetNode(key, valueNode);
            if (node != null) ClientDataStructure.Client.OperationStream(node, onSet);
            else onSet(new ReturnValue<bool> { Type = ReturnType.NodeParentSetError });
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueNode"></param>
        /// <param name="onSet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void SetStream(keyType key, valueType valueNode, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Parameter.OperationBool node = GetSetNode(key, valueNode);
            if (node != null) ClientDataStructure.Client.OperationStream(node, onSet);
            else onSet(new AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> { Type = Net.TcpServer.ReturnType.ClientException, Value = new ReturnParameter { Type = ReturnType.NodeParentSetError } });
        }
#if NOJIT
        /// <summary>
        /// 创建搜索树字典节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static ValueSearchTreeDictionary<keyType, valueType> create(Abstract.Node parent)
        {
            return new ValueSearchTreeDictionary<keyType, valueType>(parent);
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, ValueSearchTreeDictionary<keyType, valueType>> constructor;
        static ValueSearchTreeDictionary()
        {
            if (!ValueData.Data<keyType>.IsSortKey) throw new InvalidCastException("不支持排序关键字类型 " + typeof(keyType).fullName());
            ValueData.Data<valueType>.CheckValueType();
#if NOJIT
            constructor = (Func<Abstract.Node, ValueSearchTreeDictionary<keyType, valueType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, ValueSearchTreeDictionary<keyType, valueType>>), typeof(ValueSearchTreeDictionary<keyType, valueType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, ValueSearchTreeDictionary<keyType, valueType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(ValueSearchTreeDictionary<keyType, valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
