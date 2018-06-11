using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 数组节点 短路径
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed partial class Array<valueType> : Collections<Array<valueType>>
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
                if (value.Type != ReturnType.Success) throw new Exception(value.Type.ToString());
                return value.Value;
            }
            set
            {
                ReturnValue<bool> isSet = Set(index, value);
                if (!isSet) throw new Exception(isSet.Type.ToString());
            }
        }
        /// <summary>
        /// 数组节点 短路径
        /// </summary>
        /// <param name="node"></param>
        internal Array(DataStructure.Abstract.ValueArray<valueType> node) : base(node) { }

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
            return Client.GetBool(Client.Operation(GetSetDefaultNode(index)));
        }
        /// <summary>
        /// 清除元素节点数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onRemove"></param>
        /// <returns></returns>
        public void SetDefault(int index, Action<AutoCSer.CacheServer.ReturnValue<bool>> onRemove)
        {
            if (onRemove == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetSetDefaultNode(index), onRemove);
        }
        /// <summary>
        /// 清除元素节点数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onRemove"></param>
        /// <returns></returns>
        public void SetDefault(int index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onRemove)
        {
            if (onRemove == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetSetDefaultNode(index), onRemove);
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
            Client.OperationStream(GetSetDefaultNode(index), onRemove);
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
            Client.OperationStream(GetSetDefaultNode(index), onRemove);
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
            return new ReturnValue<valueType>(Client.Query(GetNode(index)));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onGet"></param>
        public void Get(int index, Action<ReturnValue<valueType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Client.Query(GetNode(index), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onGet"></param>
        public void Get(int index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Client.Query(GetNode(index), onGet);
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
            Client.QueryStream(GetNode(index), onGet);
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
            Client.QueryStream(GetNode(index), onGet);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetSetNode(int index, valueType value)
        {
            return ValueData.Data<valueType>.GetOperationBool(new Parameter.Value(this, index), value, OperationParameter.OperationType.SetValue);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ReturnValue<bool> Set(int index, valueType value)
        {
            return Client.GetBool(Client.Operation(GetSetNode(index, value)));
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="onSet"></param>
        /// <returns></returns>
        public void Set(int index, valueType value, Action<ReturnValue<bool>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetSetNode(index, value), onSet);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="onSet"></param>
        /// <returns></returns>
        public void Set(int index, valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetSetNode(index, value), onSet);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="onSet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void SetStream(int index, valueType value, Action<ReturnValue<bool>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Client.OperationStream(GetSetNode(index, value), onSet);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="onSet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void SetStream(int index, valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Client.OperationStream(GetSetNode(index, value), onSet);
        }
    }
}
