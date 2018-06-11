using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 字典节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">数据节点类型</typeparam>
    public abstract partial class ValueDictionary<keyType, valueType> : Dictionary<keyType>, IValueNode
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
        /// 字典节点
        /// </summary>
        /// <param name="parent">父节点</param>
        protected ValueDictionary(Node parent) : base(parent) { }
        /// <summary>
        /// 创建数据节点
        /// </summary>
        /// <returns></returns>
        internal override Abstract.Node CreateValueNode()
        {
            return this;
        }

        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<ShortPath.Dictionary<keyType, valueType>> CreateShortPath()
        {
            if (Parent != null) return new ShortPath.Dictionary<keyType, valueType>(this).Create();
            return new ReturnValue<ShortPath.Dictionary<keyType, valueType>> { Type = ReturnType.CanNotCreateShortPath };
        }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="onCreated"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CreateShortPath(Action<ReturnValue<ShortPath.Dictionary<keyType, valueType>>> onCreated)
        {
            if (onCreated == null) throw new ArgumentNullException();
            if (Parent != null) new ShortPath.Dictionary<keyType, valueType>(this).Create(onCreated);
            else onCreated(new ReturnValue<ShortPath.Dictionary<keyType, valueType>> { Type = ReturnType.CanNotCreateShortPath });
        }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="onCreated">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CreateShortPathStream(Action<ReturnValue<ShortPath.Dictionary<keyType, valueType>>> onCreated)
        {
            if (onCreated == null) throw new ArgumentNullException();
            if (Parent != null) new ShortPath.Dictionary<keyType, valueType>(this).CreateStream(onCreated);
            else onCreated(new ReturnValue<ShortPath.Dictionary<keyType, valueType>> { Type = ReturnType.CanNotCreateShortPath });
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
            return Client.GetBool(ClientDataStructure.Client.Operation(GetSetNode(key, value)));
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
            ClientDataStructure.Client.Operation(GetSetNode(key, value), onSet);
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
            ClientDataStructure.Client.OperationReturnParameter(GetSetNode(key, value), onSet);
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
            ClientDataStructure.Client.OperationStream(GetSetNode(key, value), onSet);
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
            ClientDataStructure.Client.OperationStream(GetSetNode(key, value), onSet);
        }

        /// <summary>
        /// 获取更新节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Parameter.Value getUpdateNode(keyType key)
        {
            Parameter.Value node = new Parameter.Value(this);
            ValueData.Data<keyType>.SetData(ref node.Parameter, key);
            return node;
        }
        /// <summary>
        /// 获取数字更新
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal OperationUpdater.Number<valueType> GetNumber(keyType key)
        {
            return new OperationUpdater.Number<valueType>(getUpdateNode(key));
        }
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal OperationUpdater.Integer<valueType> GetInteger(keyType key)
        {
            return new OperationUpdater.Integer<valueType>(getUpdateNode(key));
        }
        static ValueDictionary()
        {
            ValueData.Data<valueType>.CheckValueType();
        }
    }
}
