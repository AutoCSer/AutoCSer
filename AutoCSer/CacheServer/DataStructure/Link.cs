using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 链表节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed partial class Link<valueType> : Abstract.Collections, Abstract.IValueNode
    {
        /// <summary>
        /// 链表节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private Link(Abstract.Node parent) : base(parent) { }
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
            stream.Write((byte)Abstract.NodeType.Link);
            stream.Write((byte)ValueData.Data<valueType>.DataType);
            serializeParentDataStructure(stream);
        }

        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<ShortPath.Link<valueType>> CreateShortPath()
        {
            if (Parent != null) return new ShortPath.Link<valueType>(this).Create();
            return new ReturnValue<ShortPath.Link<valueType>> { Type = ReturnType.CanNotCreateShortPath };
        }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="onCreated"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CreateShortPath(Action<ReturnValue<ShortPath.Link<valueType>>> onCreated)
        {
            if (onCreated == null) throw new ArgumentNullException();
            if (Parent != null) new ShortPath.Link<valueType>(this).Create(onCreated);
            else onCreated(new ReturnValue<ShortPath.Link<valueType>> { Type = ReturnType.CanNotCreateShortPath });
        }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="onCreated">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CreateShortPathStream(Action<ReturnValue<ShortPath.Link<valueType>>> onCreated)
        {
            if (onCreated == null) throw new ArgumentNullException();
            if (Parent != null) new ShortPath.Link<valueType>(this).CreateStream(onCreated);
            else onCreated(new ReturnValue<ShortPath.Link<valueType>> { Type = ReturnType.CanNotCreateShortPath });
        }

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
            return new ReturnValue<valueType>(ClientDataStructure.Client.Query(GetNode(index)));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onGet"></param>
        public void Get(int index, Action<ReturnValue<valueType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetNode(index), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onGet"></param>
        public void Get(int index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetNode(index), onGet);
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
            ClientDataStructure.Client.QueryStream(GetNode(index), onGet);
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
            ClientDataStructure.Client.QueryStream(GetNode(index), onGet);
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
            return Client.GetBool(ClientDataStructure.Client.Operation(GetRemoveNode(index)));
        }
        /// <summary>
        /// 删除元素节点
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onRemove"></param>
        /// <returns></returns>
        public void Remove(int index, Action<ReturnValue<bool>> onRemove)
        {
            ClientDataStructure.Client.Operation(GetRemoveNode(index), onRemove);
        }
        /// <summary>
        /// 删除元素节点
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onRemove"></param>
        /// <returns></returns>
        public void Remove(int index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onRemove)
        {
            ClientDataStructure.Client.OperationReturnParameter(GetRemoveNode(index), onRemove);
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
            ClientDataStructure.Client.OperationStream(GetRemoveNode(index), onRemove);
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
            ClientDataStructure.Client.OperationStream(GetRemoveNode(index), onRemove);
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
            return new ReturnValue<valueType>(ClientDataStructure.Client.Operation(GetGetRemoveNode(index)));
        }
        /// <summary>
        /// 获取并删除数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onRemove"></param>
        public void GetRemove(int index, Action<ReturnValue<valueType>> onRemove)
        {
            ClientDataStructure.Client.Operation(GetGetRemoveNode(index), onRemove);
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
            ClientDataStructure.Client.OperationStream(GetGetRemoveNode(index), onRemove);
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
            return Client.GetBool(ClientDataStructure.Client.Operation(GetInsertBeforeNode(index, value)));
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
            ClientDataStructure.Client.Operation(GetInsertBeforeNode(index, value), onInsertBefore);
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
            ClientDataStructure.Client.OperationReturnParameter(GetInsertBeforeNode(index, value), onInsertBefore);
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
            ClientDataStructure.Client.OperationStream(GetInsertBeforeNode(index, value), onInsertBefore);
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
            ClientDataStructure.Client.OperationStream(GetInsertBeforeNode(index, value), onInsertBefore);
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
            return Client.GetBool(ClientDataStructure.Client.Operation(GetInsertAfterNode(index, value)));
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
            ClientDataStructure.Client.Operation(GetInsertAfterNode(index, value), onInsertAfter);
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
            ClientDataStructure.Client.OperationReturnParameter(GetInsertAfterNode(index, value), onInsertAfter);
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
            ClientDataStructure.Client.OperationStream(GetInsertAfterNode(index, value), onInsertAfter);
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
            ClientDataStructure.Client.OperationStream(GetInsertAfterNode(index, value), onInsertAfter);
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

        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, Link<valueType>> constructor;
#if NOJIT
        /// <summary>
        /// 创建链表节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Link<valueType> create(Abstract.Node parent)
        {
            return new Link<valueType>(parent);
        }
#endif
        static Link()
        {
            ValueData.Data<valueType>.CheckValueType();
#if NOJIT
            constructor = (Func<Abstract.Node, Link<valueType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, Link<valueType>>), typeof(Link<valueType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, Link<valueType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(Link<valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
