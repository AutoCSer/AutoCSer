using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 链表节点
    /// </summary>
    /// <typeparam name="nodeType">元素节点类型</typeparam>
    public sealed partial class Link<nodeType> : Abstract.Collections
        where nodeType : Abstract.Node, Abstract.IValue
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
        /// 获取元素节点
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <returns></returns>
        public nodeType this[int index]
        {
            get
            {
                nodeType node = nodeConstructor(this);
                node.Parameter.Set(index);
                return node;
            }
        }
        /// <summary>
        /// 创建数据节点
        /// </summary>
        /// <returns></returns>
        internal override Abstract.Node CreateValueNode()
        {
            return nodeConstructor(this);
        }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.Link);
            serializeParentDataStructure(stream);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
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
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <returns></returns>
        public ReturnValueNode<nodeType> Get(int index)
        {
            return new ReturnValueNode<nodeType>(ClientDataStructure.Client.Query(GetNode(index)));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onGet"></param>
        public void Get(int index, Action<ReturnValueNode<nodeType>> onGet)
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
        public void GetStream(int index, Action<ReturnValueNode<nodeType>> onGet)
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
        public Parameter.OperationReturnValue<nodeType> GetGetRemoveNode(int index)
        {
            Parameter.OperationReturnValue<nodeType> node = new Parameter.OperationReturnValue<nodeType>(this, OperationParameter.OperationType.GetRemove);
            node.Parameter.Set(index);
            return node;
        }
        /// <summary>
        /// 获取并删除数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <returns></returns>
        public ReturnValueNode<nodeType> GetRemove(int index)
        {
            return new ReturnValueNode<nodeType>(ClientDataStructure.Client.Operation(GetGetRemoveNode(index)));
        }
        /// <summary>
        /// 获取并删除数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onRemove"></param>
        public void GetRemove(int index, Action<ReturnValueNode<nodeType>> onRemove)
        {
            ClientDataStructure.Client.Operation(GetGetRemoveNode(index), onRemove);
        }
        /// <summary>
        /// 获取并删除数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="onRemove">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetRemoveStream(int index, Action<ReturnValueNode<nodeType>> onRemove)
        {
            if (onRemove == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetGetRemoveNode(index), onRemove);
        }

        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationReturnValue<nodeType> GetStackPopNode()
        {
            return GetGetRemoveNode(-1);
        }
        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValueNode<nodeType> StackPop()
        {
            return GetRemove(-1);
        }
        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <param name="onPop"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void StackPop(Action<ReturnValueNode<nodeType>> onPop)
        {
            GetRemove(-1, onPop);
        }
        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <param name="onPop">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void StackPopStream(Action<ReturnValueNode<nodeType>> onPop)
        {
            GetRemoveStream(-1, onPop);
        }

        /// <summary>
        /// 弹出第一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationReturnValue<nodeType> GetDequeueNode()
        {
            return GetGetRemoveNode(0);
        }
        /// <summary>
        /// 弹出第一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValueNode<nodeType> Dequeue()
        {
            return GetRemove(0);
        }
        /// <summary>
        /// 弹出第一个数据
        /// </summary>
        /// <param name="onDequeue"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Dequeue(Action<ReturnValueNode<nodeType>> onDequeue)
        {
            GetRemove(0, onDequeue);
        }
        /// <summary>
        /// 弹出第一个数据
        /// </summary>
        /// <param name="onDequeue">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void DequeueStream(Action<ReturnValueNode<nodeType>> onDequeue)
        {
            GetRemoveStream(0, onDequeue);
        }

        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
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
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetInsertBeforeNode(int index, nodeType valueNode)
        {
            return getOperation(index, valueNode, OperationParameter.OperationType.InsertBefore);
        }
        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        public ReturnValue<bool> InsertBefore(int index, nodeType valueNode)
        {
            Parameter.OperationBool node = GetInsertBeforeNode(index, valueNode);
            if (node != null) return Client.GetBool(ClientDataStructure.Client.Operation(node));
            return new ReturnValue<bool> { Type = ReturnType.NodeParentSetError };
        }
        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="valueNode"></param>
        /// <param name="onInsertBefore"></param>
        /// <returns></returns>
        public void InsertBefore(int index, nodeType valueNode, Action<ReturnValue<bool>> onInsertBefore)
        {
            Parameter.OperationBool node = GetInsertBeforeNode(index, valueNode);
            if (node != null) ClientDataStructure.Client.Operation(node, onInsertBefore);
            else if(onInsertBefore != null) onInsertBefore(new ReturnValue<bool> { Type = ReturnType.NodeParentSetError });
        }
        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="valueNode"></param>
        /// <param name="onInsertBefore"></param>
        /// <returns></returns>
        public void InsertBefore(int index, nodeType valueNode, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onInsertBefore)
        {
            Parameter.OperationBool node = GetInsertBeforeNode(index, valueNode);
            if (node != null) ClientDataStructure.Client.OperationReturnParameter(node, onInsertBefore);
            else if (onInsertBefore != null) onInsertBefore(new AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> { Type = Net.TcpServer.ReturnType.ClientException, Value = new ReturnParameter { Type = ReturnType.NodeParentSetError } });
        }
        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="valueNode"></param>
        /// <param name="onInsertBefore">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void InsertBeforeStream(int index, nodeType valueNode, Action<ReturnValue<bool>> onInsertBefore)
        {
            if (onInsertBefore == null) throw new ArgumentNullException();
            Parameter.OperationBool node = GetInsertBeforeNode(index, valueNode);
            if (node != null) ClientDataStructure.Client.OperationStream(node, onInsertBefore);
            else onInsertBefore(new ReturnValue<bool> { Type = ReturnType.NodeParentSetError });
        }
        /// <summary>
        /// 前置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="valueNode"></param>
        /// <param name="onInsertBefore">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void InsertBeforeStream(int index, nodeType valueNode, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onInsertBefore)
        {
            if (onInsertBefore == null) throw new ArgumentNullException();
            Parameter.OperationBool node = GetInsertBeforeNode(index, valueNode);
            if (node != null) ClientDataStructure.Client.OperationStream(node, onInsertBefore);
            else onInsertBefore(new AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> { Type = Net.TcpServer.ReturnType.ClientException, Value = new ReturnParameter { Type = ReturnType.NodeParentSetError } });
        }

        /// <summary>
        /// 后置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetInsertAfterNode(int index, nodeType valueNode)
        {
            return getOperation(index, valueNode, OperationParameter.OperationType.InsertAfter);
        }
        /// <summary>
        /// 后置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        public ReturnValue<bool> InsertAfter(int index, nodeType valueNode)
        {
            Parameter.OperationBool node = GetInsertAfterNode(index, valueNode);
            if (node != null) return Client.GetBool(ClientDataStructure.Client.Operation(node));
            return new ReturnValue<bool> { Type = ReturnType.NodeParentSetError };
        }
        /// <summary>
        /// 后置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="valueNode"></param>
        /// <param name="onInsertAfter"></param>
        /// <returns></returns>
        public void InsertAfter(int index, nodeType valueNode, Action<ReturnValue<bool>> onInsertAfter)
        {
            Parameter.OperationBool node = GetInsertAfterNode(index, valueNode);
            if (node != null) ClientDataStructure.Client.Operation(node, onInsertAfter);
            else if (onInsertAfter != null) onInsertAfter(new ReturnValue<bool> { Type = ReturnType.NodeParentSetError });
        }
        /// <summary>
        /// 后置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="valueNode"></param>
        /// <param name="onInsertAfter"></param>
        /// <returns></returns>
        public void InsertAfter(int index, nodeType valueNode, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onInsertAfter)
        {
            Parameter.OperationBool node = GetInsertAfterNode(index, valueNode);
            if (node != null) ClientDataStructure.Client.OperationReturnParameter(node, onInsertAfter);
            else if (onInsertAfter != null) onInsertAfter(new AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> { Type = Net.TcpServer.ReturnType.ClientException, Value = new ReturnParameter { Type = ReturnType.NodeParentSetError } });
        }
        /// <summary>
        /// 后置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="valueNode"></param>
        /// <param name="onInsertAfter">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void InsertAfterStream(int index, nodeType valueNode, Action<ReturnValue<bool>> onInsertAfter)
        {
            if (onInsertAfter == null) throw new ArgumentNullException();
            Parameter.OperationBool node = GetInsertAfterNode(index, valueNode);
            if (node != null) ClientDataStructure.Client.OperationStream(node, onInsertAfter);
            else onInsertAfter(new ReturnValue<bool> { Type = ReturnType.NodeParentSetError });
        }
        /// <summary>
        /// 后置插入数据
        /// </summary>
        /// <param name="index">负数表示从尾部向前倒数，-1 表示最后一个</param>
        /// <param name="valueNode"></param>
        /// <param name="onInsertAfter">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void InsertAfterStream(int index, nodeType valueNode, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onInsertAfter)
        {
            if (onInsertAfter == null) throw new ArgumentNullException();
            Parameter.OperationBool node = GetInsertAfterNode(index, valueNode);
            if (node != null) ClientDataStructure.Client.OperationStream(node, onInsertAfter);
            else onInsertAfter(new AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> { Type = Net.TcpServer.ReturnType.ClientException, Value = new ReturnParameter { Type = ReturnType.NodeParentSetError } });
        }

        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetAppendNode(nodeType valueNode)
        {
            return getOperation(-1, valueNode, OperationParameter.OperationType.InsertAfter);
        }
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="valueNode"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<bool> Append(nodeType valueNode)
        {
            return InsertAfter(-1, valueNode);
        }
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="valueNode"></param>
        /// <param name="onAppend"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Append(nodeType valueNode, Action<ReturnValue<bool>> onAppend)
        {
            InsertAfter(-1, valueNode, onAppend);
        }
        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="valueNode"></param>
        /// <param name="onAppend">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void AppendStream(nodeType valueNode, Action<ReturnValue<bool>> onAppend)
        {
            InsertAfterStream(-1, valueNode, onAppend);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, Link<nodeType>> constructor;
#if NOJIT
        /// <summary>
        /// 创建链表节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Link<nodeType> create(Abstract.Node parent)
        {
            return new Link<nodeType>(parent);
        }
#endif
        /// <summary>
        /// 子节点构造函数
        /// </summary>
        private static readonly Func<Abstract.Node, nodeType> nodeConstructor;
        static Link()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, Link<nodeType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, Link<nodeType>>), typeof(Link<nodeType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, Link<nodeType>>)AutoCSer.Emit.Constructor.Create(typeof(Link<nodeType>), NodeConstructorParameterTypes);
#endif
            nodeConstructor = (Func<Abstract.Node, nodeType>)typeof(nodeType).GetField(Cache.Node.ConstructorFieldName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetValue(null);
        }
    }
}
