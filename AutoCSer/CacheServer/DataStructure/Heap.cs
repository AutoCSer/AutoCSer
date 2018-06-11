using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 最小堆节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">数据节点类型</typeparam>
    public sealed partial class Heap<keyType, valueType> : Abstract.Collections, Abstract.IValueNode
        where keyType : IEquatable<keyType>, IComparable<keyType>
    {
        /// <summary>
        /// 最小堆节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private Heap(Abstract.Node parent) : base(parent) { }
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
            stream.Write((byte)Abstract.NodeType.ArrayHeap);
            stream.Write((byte)ValueData.Data<valueType>.DataType);
            stream.Write((byte)ValueData.Data<keyType>.DataType);
            serializeParentDataStructure(stream);
        }

        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<ShortPath.Heap<keyType, valueType>> CreateShortPath()
        {
            if (Parent != null) return new ShortPath.Heap<keyType, valueType>(this).Create();
            return new ReturnValue<ShortPath.Heap<keyType, valueType>> { Type = ReturnType.CanNotCreateShortPath };
        }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="onCreated"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CreateShortPath(Action<ReturnValue<ShortPath.Heap<keyType, valueType>>> onCreated)
        {
            if (onCreated == null) throw new ArgumentNullException();
            if (Parent != null) new ShortPath.Heap<keyType, valueType>(this).Create(onCreated);
            else onCreated(new ReturnValue<ShortPath.Heap<keyType, valueType>> { Type = ReturnType.CanNotCreateShortPath });
        }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="onCreated">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CreateShortPathStream(Action<ReturnValue<ShortPath.Heap<keyType, valueType>>> onCreated)
        {
            if (onCreated == null) throw new ArgumentNullException();
            if (Parent != null) new ShortPath.Heap<keyType, valueType>(this).CreateStream(onCreated);
            else onCreated(new ReturnValue<ShortPath.Heap<keyType, valueType>> { Type = ReturnType.CanNotCreateShortPath });
        }

        /// <summary>
        /// 获取堆顶关键字（最小值）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryReturnValue<keyType> GetTopKeyNode()
        {
            return new Parameter.QueryReturnValue<keyType>(this, OperationParameter.OperationType.GetValue, true);
        }
        /// <summary>
        /// 获取堆顶关键字（最小值）
        /// </summary>
        /// <returns></returns>
        public ReturnValue<keyType> GetTopKey()
        {
            return new ReturnValue<keyType>(ClientDataStructure.Client.Query(GetTopKeyNode()));
        }
        /// <summary>
        /// 获取堆顶关键字（最小值）
        /// </summary>
        /// <param name="onGetTopKey"></param>
        public void GetTopKey(Action<ReturnValue<keyType>> onGetTopKey)
        {
            if (onGetTopKey == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetTopKeyNode(), onGetTopKey);
        }
        /// <summary>
        /// 获取堆顶关键字（最小值）
        /// </summary>
        /// <param name="onGetTopKey"></param>
        public void GetTopKey(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGetTopKey)
        {
            if (onGetTopKey == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetTopKeyNode(), onGetTopKey);
        }
        /// <summary>
        /// 获取堆顶关键字（最小值）
        /// </summary>
        /// <param name="onGetTopKey">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetTopKeyStream(Action<ReturnValue<keyType>> onGetTopKey)
        {
            if (onGetTopKey == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryStream(GetTopKeyNode(), onGetTopKey);
        }
        /// <summary>
        /// 获取堆顶关键字（最小值）
        /// </summary>
        /// <param name="onGetTopKey">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetTopKeyStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGetTopKey)
        {
            if (onGetTopKey == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryStream(GetTopKeyNode(), onGetTopKey);
        }

        /// <summary>
        /// 获取堆顶数据（最小关键字）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryReturnValue<valueType> GetTopValueNode()
        {
            return new Parameter.QueryReturnValue<valueType>(this, OperationParameter.OperationType.GetValue, false);
        }
        /// <summary>
        /// 获取堆顶数据（最小关键字）
        /// </summary>
        /// <returns></returns>
        public ReturnValue<valueType> GetTopValue()
        {
            return new ReturnValue<valueType>(ClientDataStructure.Client.Query(GetTopValueNode()));
        }
        /// <summary>
        /// 获取堆顶数据（最小关键字）
        /// </summary>
        /// <param name="onGetTopValue"></param>
        public void GetTopValue(Action<ReturnValue<valueType>> onGetTopValue)
        {
            if (onGetTopValue == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetTopValueNode(), onGetTopValue);
        }
        /// <summary>
        /// 获取堆顶数据（最小关键字）
        /// </summary>
        /// <param name="onGetTopValue"></param>
        public void GetTopValue(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGetTopValue)
        {
            if (onGetTopValue == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetTopValueNode(), onGetTopValue);
        }
        /// <summary>
        /// 获取堆顶数据（最小关键字）
        /// </summary>
        /// <param name="onGetTopValue">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetTopValueStream(Action<ReturnValue<valueType>> onGetTopValue)
        {
            if (onGetTopValue == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryStream(GetTopValueNode(), onGetTopValue);
        }
        /// <summary>
        /// 获取堆顶数据（最小关键字）
        /// </summary>
        /// <param name="onGetTopValue">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetTopValueStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGetTopValue)
        {
            if (onGetTopValue == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryStream(GetTopValueNode(), onGetTopValue);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetPushNode(keyType key, valueType value)
        {
            Parameter.Value keyNode = new Parameter.Value(this);
            ValueData.Data<keyType>.SetData(ref keyNode.Parameter, key);
            return ValueData.Data<valueType>.GetOperationBool(keyNode, value, OperationParameter.OperationType.SetValue);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ReturnValue<bool> Push(keyType key, valueType value)
        {
            return Client.GetBool(ClientDataStructure.Client.Operation(GetPushNode(key, value)));
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="onPush"></param>
        /// <returns></returns>
        public void Push(keyType key, valueType value, Action<ReturnValue<bool>> onPush)
        {
            ClientDataStructure.Client.Operation(GetPushNode(key, value), onPush);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="onPush"></param>
        /// <returns></returns>
        public void Push(keyType key, valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onPush)
        {
            ClientDataStructure.Client.OperationReturnParameter(GetPushNode(key, value), onPush);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="onPush">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void PushStream(keyType key, valueType value, Action<ReturnValue<bool>> onPush)
        {
            if (onPush == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetPushNode(key, value), onPush);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="onPush">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void PushStream(keyType key, valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onPush)
        {
            if (onPush == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetPushNode(key, value), onPush);
        }

        /// <summary>
        /// 删除堆顶数据（最小关键字）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetPopTopNode()
        {
            return new Parameter.OperationBool(this, OperationParameter.OperationType.Remove);
        }
        /// <summary>
        /// 删除堆顶数据（最小关键字）
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<bool> PopTop()
        {
            return Client.GetBool(ClientDataStructure.Client.Operation(GetPopTopNode()));
        }
        /// <summary>
        /// 删除堆顶数据（最小关键字）
        /// </summary>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void PopTop(Action<ReturnValue<bool>> onReturn)
        {
            ClientDataStructure.Client.Operation(GetPopTopNode(), onReturn);
        }
        /// <summary>
        /// 删除堆顶数据（最小关键字）
        /// </summary>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void PopTop(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            ClientDataStructure.Client.OperationReturnParameter(GetPopTopNode(), onReturn);
        }
        /// <summary>
        /// 删除堆顶数据（最小关键字）
        /// </summary>
        /// <param name="onReturn">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void PopTopStream(Action<ReturnValue<bool>> onReturn)
        {
            if (onReturn == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetPopTopNode(), onReturn);
        }
        /// <summary>
        /// 删除堆顶数据（最小关键字）
        /// </summary>
        /// <param name="onReturn">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void PopTopStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (onReturn == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetPopTopNode(), onReturn);
        }

        /// <summary>
        /// 删除堆顶数据并返回关键字（最小关键字）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationReturnValue<keyType> GetPopTopKeyNode()
        {
            return new Parameter.OperationReturnValue<keyType>(this, OperationParameter.OperationType.GetRemove, true);
        }
        /// <summary>
        /// 删除堆顶数据并返回关键字（最小关键字）
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<keyType> GetPopTopKey()
        {
            return new ReturnValue<keyType>(ClientDataStructure.Client.Operation(GetPopTopKeyNode()));
        }
        /// <summary>
        /// 删除堆顶数据并返回关键字（最小关键字）
        /// </summary>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void GetPopTopKey(Action<ReturnValue<keyType>> onReturn)
        {
            ClientDataStructure.Client.Operation(GetPopTopKeyNode(), onReturn);
        }
        /// <summary>
        /// 删除堆顶数据并返回关键字（最小关键字）
        /// </summary>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void GetPopTopKey(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            ClientDataStructure.Client.OperationReturnParameter(GetPopTopKeyNode(), onReturn);
        }
        /// <summary>
        /// 删除堆顶数据并返回关键字（最小关键字）
        /// </summary>
        /// <param name="onReturn">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void GetPopTopKeyStream(Action<ReturnValue<keyType>> onReturn)
        {
            if (onReturn == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetPopTopKeyNode(), onReturn);
        }
        /// <summary>
        /// 删除堆顶数据并返回关键字（最小关键字）
        /// </summary>
        /// <param name="onReturn">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void GetPopTopKeyStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (onReturn == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetPopTopKeyNode(), onReturn);
        }

        /// <summary>
        /// 删除堆顶数据并返回数据（最小关键字）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationReturnValue<valueType> GetPopTopValueNode()
        {
            return new Parameter.OperationReturnValue<valueType>(this, OperationParameter.OperationType.GetRemove, false);
        }
        /// <summary>
        /// 删除堆顶数据并返回数据（最小关键字）
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> GetPopTopValue()
        {
            return new ReturnValue<valueType>(ClientDataStructure.Client.Operation(GetPopTopValueNode()));
        }
        /// <summary>
        /// 删除堆顶数据并返回数据（最小关键字）
        /// </summary>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void GetPopTopValue(Action<ReturnValue<valueType>> onReturn)
        {
            ClientDataStructure.Client.Operation(GetPopTopValueNode(), onReturn);
        }
        /// <summary>
        /// 删除堆顶数据并返回数据（最小关键字）
        /// </summary>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void GetPopTopValue(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            ClientDataStructure.Client.OperationReturnParameter(GetPopTopValueNode(), onReturn);
        }
        /// <summary>
        /// 删除堆顶数据并返回数据（最小关键字）
        /// </summary>
        /// <param name="onReturn">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void GetPopTopValueStream(Action<ReturnValue<valueType>> onReturn)
        {
            if (onReturn == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetPopTopValueNode(), onReturn);
        }
        /// <summary>
        /// 删除堆顶数据并返回数据（最小关键字）
        /// </summary>
        /// <param name="onReturn">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void GetPopTopValueStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (onReturn == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetPopTopValueNode(), onReturn);
        }

#if NOJIT
        /// <summary>
        /// 创建搜索树字典节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Heap<keyType, valueType> create(Abstract.Node parent)
        {
            return new Heap<keyType, valueType>(parent);
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, Heap<keyType, valueType>> constructor;
        static Heap()
        {
            if (!ValueData.Data<keyType>.IsSortKey) throw new InvalidCastException("不支持排序关键字类型 " + typeof(keyType).fullName());
            ValueData.Data<valueType>.CheckValueType();
#if NOJIT
            constructor = (Func<Abstract.Node, Heap<keyType, valueType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, Heap<keyType, valueType>>), typeof(Heap<keyType, valueType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, Heap<keyType, valueType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(Heap<keyType, valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
