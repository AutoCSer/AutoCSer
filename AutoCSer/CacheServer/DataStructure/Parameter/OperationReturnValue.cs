using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 操作参数节点
    /// </summary>
    /// <typeparam name="nodeType"></typeparam>
    public sealed partial class OperationReturnValue<nodeType> : Operation
        where nodeType : Abstract.Node, Abstract.IValue
    {
        /// <summary>
        /// 操作参数节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="operationType">操作类型</param>
        internal OperationReturnValue(Abstract.Node parent, OperationParameter.OperationType operationType) : base(parent, operationType) { }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValueNode<nodeType> Operation()
        {
            return new ReturnValueNode<nodeType>(ClientDataStructure.Client.Operation(this));
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Operation(Action<ReturnValueNode<nodeType>> onGet)
        {
            ClientDataStructure.Client.Operation(this, onGet);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void OperationStream(Action<ReturnValueNode<nodeType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(this, onGet);
        }
    }
}

