using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 链表节点 短路径
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed partial class Link<valueType> : Collections<Link<valueType>>
    {
        /// <summary>
        /// 哈希表节点 短路径
        /// </summary>
        /// <param name="node"></param>
        internal Link(DataStructure.Link<valueType> node) : base(node) { }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
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
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <returns></returns>
        public ReturnValue<valueType> Get(int index)
        {
            return new ReturnValue<valueType>(Client.Query(GetNode(index)));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onGet"></param>
        public void Get(int index, Action<ReturnValue<valueType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Client.Query(GetNode(index), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onGet"></param>
        public void Get(int index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Client.Query(GetNode(index), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
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
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetStream(int index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Client.QueryStream(GetNode(index), onGet);
        }

        /// <summary>
        /// 删除元素节点
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetRemoveNode(int index)
        {
            Parameter.OperationBool node = new Parameter.OperationBool(this, OperationParameter.OperationType.Remove);
            node.Parameter.Set(index);
            return node;
        }
        /// <summary>
        /// 删除元素节点
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <returns></returns>
        public ReturnValue<bool> Remove(int index)
        {
            return Client.GetBool(Client.Operation(GetRemoveNode(index)));
        }
        /// <summary>
        /// 删除元素节点
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onRemove"></param>
        /// <returns></returns>
        public void Remove(int index, Action<ReturnValue<bool>> onRemove)
        {
            if (onRemove == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetRemoveNode(index), onRemove);
        }
        /// <summary>
        /// 删除元素节点
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onRemove"></param>
        /// <returns></returns>
        public void Remove(int index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onRemove)
        {
            if (onRemove == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetRemoveNode(index), onRemove);
        }
        /// <summary>
        /// 删除元素节点
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onRemove">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void RemoveStream(int index, Action<ReturnValue<bool>> onRemove)
        {
            if (onRemove == null) throw new ArgumentNullException();
            Client.OperationStream(GetRemoveNode(index), onRemove);
        }
        /// <summary>
        /// 删除元素节点
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onRemove">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void RemoveStream(int index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onRemove)
        {
            if (onRemove == null) throw new ArgumentNullException();
            Client.OperationStream(GetRemoveNode(index), onRemove);
        }

        /// <summary>
        /// 获取并删除数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationReturnValue<valueType> GetGetRemoveNode(int index)
        {
            Parameter.OperationReturnValue<valueType> node = new Parameter.OperationReturnValue<valueType>(this, OperationParameter.OperationType.GetRemove);
            node.Parameter.Set(index);
            return node;
        }
        /// <summary>
        /// 获取并删除数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <returns></returns>
        public ReturnValue<valueType> GetRemove(int index)
        {
            return new ReturnValue<valueType>(Client.Operation(GetGetRemoveNode(index)));
        }
        /// <summary>
        /// 获取并删除数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onRemove"></param>
        public void GetRemove(int index, Action<ReturnValue<valueType>> onRemove)
        {
            if (onRemove == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetGetRemoveNode(index), onRemove);
        }
        /// <summary>
        /// 获取并删除数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onRemove">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetRemoveStream(int index, Action<ReturnValue<valueType>> onRemove)
        {
            if (onRemove == null) throw new ArgumentNullException();
            Client.OperationStream(GetGetRemoveNode(index), onRemove);
        }

        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationReturnValue<valueType> GetStackPopNode()
        {
            return GetGetRemoveNode(-1);
        }
        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> StackPop()
        {
            return GetRemove(-1);
        }
        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <param name="onPop"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void StackPop(Action<ReturnValue<valueType>> onPop)
        {
            GetRemove(-1, onPop);
        }
        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <param name="onPop">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void StackPopStream(Action<ReturnValue<valueType>> onPop)
        {
            GetRemoveStream(-1, onPop);
        }

        /// <summary>
        /// 弹出第一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationReturnValue<valueType> GetDequeueNode()
        {
            return GetGetRemoveNode(0);
        }
        /// <summary>
        /// 弹出第一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Dequeue()
        {
            return GetRemove(0);
        }
        /// <summary>
        /// 弹出第一个数据
        /// </summary>
        /// <param name="onDequeue"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Dequeue(Action<ReturnValue<valueType>> onDequeue)
        {
            GetRemove(0, onDequeue);
        }
        /// <summary>
        /// 弹出第一个数据
        /// </summary>
        /// <param name="onDequeue">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void DequeueStream(Action<ReturnValue<valueType>> onDequeue)
        {
            GetRemoveStream(0, onDequeue);
        }

        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="value"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Parameter.OperationBool getOperation(int index, valueType value, OperationParameter.OperationType operationType)
        {
            return ValueData.Data<valueType>.GetOperationBool(new Parameter.Value(this, index), value, operationType);
        }
        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetInsertBeforeNode(int index, valueType value)
        {
            return getOperation(index, value, OperationParameter.OperationType.InsertBefore);
        }
        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ReturnValue<bool> InsertBefore(int index, valueType value)
        {
            return Client.GetBool(Client.Operation(GetInsertBeforeNode(index, value)));
        }
        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="value"></param>
        /// <param name="onInsertBefore"></param>
        /// <returns></returns>
        public void InsertBefore(int index, valueType value, Action<ReturnValue<bool>> onInsertBefore)
        {
            if (onInsertBefore == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetInsertBeforeNode(index, value), onInsertBefore);
        }
        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="value"></param>
        /// <param name="onInsertBefore"></param>
        /// <returns></returns>
        public void InsertBefore(int index, valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onInsertBefore)
        {
            if (onInsertBefore == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetInsertBeforeNode(index, value), onInsertBefore);
        }
        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="value"></param>
        /// <param name="onInsertBefore">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void InsertBeforeStream(int index, valueType value, Action<ReturnValue<bool>> onInsertBefore)
        {
            if (onInsertBefore == null) throw new ArgumentNullException();
            Client.OperationStream(GetInsertBeforeNode(index, value), onInsertBefore);
        }
        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="value"></param>
        /// <param name="onInsertBefore">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void InsertBeforeStream(int index, valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onInsertBefore)
        {
            if (onInsertBefore == null) throw new ArgumentNullException();
            Client.OperationStream(GetInsertBeforeNode(index, value), onInsertBefore);
        }

        /// <summary>
        /// 后置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetInsertAfterNode(int index, valueType value)
        {
            return getOperation(index, value, OperationParameter.OperationType.InsertAfter);
        }
        /// <summary>
        /// 后置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ReturnValue<bool> InsertAfter(int index, valueType value)
        {
            return Client.GetBool(Client.Operation(GetInsertAfterNode(index, value)));
        }
        /// <summary>
        /// 后置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="value"></param>
        /// <param name="onInsertAfter"></param>
        /// <returns></returns>
        public void InsertAfter(int index, valueType value, Action<ReturnValue<bool>> onInsertAfter)
        {
            if (onInsertAfter == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetInsertAfterNode(index, value), onInsertAfter);
        }
        /// <summary>
        /// 后置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="value"></param>
        /// <param name="onInsertAfter"></param>
        /// <returns></returns>
        public void InsertAfter(int index, valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onInsertAfter)
        {
            if (onInsertAfter == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetInsertAfterNode(index, value), onInsertAfter);
        }
        /// <summary>
        /// 后置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="value"></param>
        /// <param name="onInsertAfter">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void InsertAfterStream(int index, valueType value, Action<ReturnValue<bool>> onInsertAfter)
        {
            if (onInsertAfter == null) throw new ArgumentNullException();
            Client.OperationStream(GetInsertAfterNode(index, value), onInsertAfter);
        }
        /// <summary>
        /// 后置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="value"></param>
        /// <param name="onInsertAfter">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void InsertAfterStream(int index, valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onInsertAfter)
        {
            if (onInsertAfter == null) throw new ArgumentNullException();
            Client.OperationStream(GetInsertAfterNode(index, value), onInsertAfter);
        }

        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetAppendNode(valueType value)
        {
            return getOperation(-1, value, OperationParameter.OperationType.InsertAfter);
        }
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<bool> Append(valueType value)
        {
            return InsertAfter(-1, value);
        }
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="onAppend"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Append(valueType value, Action<ReturnValue<bool>> onAppend)
        {
            InsertAfter(-1, value, onAppend);
        }
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="onAppend">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void AppendStream(valueType value, Action<ReturnValue<bool>> onAppend)
        {
            InsertAfterStream(-1, value, onAppend);
        }
    }
}
