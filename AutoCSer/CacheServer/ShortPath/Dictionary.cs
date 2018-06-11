using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 字典节点 短路径
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">数据节点类型</typeparam>
    public sealed partial class Dictionary<keyType, valueType> : DictionaryBase<keyType, Dictionary<keyType, valueType>>
        where keyType : IEquatable<keyType>
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
            set
            {
                ReturnValue<bool> isSet = Set(key, value);
                if (!isSet.Value) throw new Exception(isSet.Type.ToString());
            }
        }
        /// <summary>
        /// 字典节点 短路径
        /// </summary>
        /// <param name="node"></param>
        internal Dictionary(DataStructure.Abstract.ValueDictionary<keyType, valueType> node) : base(node) { }

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
            return new ReturnValue<valueType>(Client.Query(GetNode(key)));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onGet"></param>
        public void Get(keyType key, Action<ReturnValue<valueType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Client.Query(GetNode(key), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onGet"></param>
        public void Get(keyType key, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Client.Query(GetNode(key), onGet);
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
            Client.QueryStream(GetNode(key), onGet);
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
            Client.QueryStream(GetNode(key), onGet);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetSetNode(keyType key, valueType value)
        {
            Parameter.Value keyNode = new Parameter.Value(this);
            ValueData.Data<keyType>.SetData(ref keyNode.Parameter, key);
            return ValueData.Data<valueType>.GetOperationBool(keyNode, value, OperationParameter.OperationType.SetValue);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ReturnValue<bool> Set(keyType key, valueType value)
        {
            return Client.GetBool(Client.Operation(GetSetNode(key, value)));
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="onSet"></param>
        /// <returns></returns>
        public void Set(keyType key, valueType value, Action<ReturnValue<bool>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetSetNode(key, value), onSet);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="onSet"></param>
        /// <returns></returns>
        public void Set(keyType key, valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetSetNode(key, value), onSet);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="onSet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void SetStream(keyType key, valueType value, Action<ReturnValue<bool>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Client.OperationStream(GetSetNode(key, value), onSet);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="onSet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void SetStream(keyType key, valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Client.OperationStream(GetSetNode(key, value), onSet);
        }
    }
}
