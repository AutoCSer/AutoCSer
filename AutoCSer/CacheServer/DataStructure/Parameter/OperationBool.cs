using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 操作参数节点
    /// </summary>
    public sealed partial class OperationBool : Operation
    {
        /// <summary>
        /// 操作参数节点
        /// </summary>
        /// <param name="parent">父节点</param>
        internal OperationBool(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 操作参数节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="operationType">操作类型</param>
        internal OperationBool(Abstract.Node parent, OperationParameter.OperationType operationType) : base(parent, operationType) { }
        /// <summary>
        /// 操作参数节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="value">数据</param>
        /// <param name="operationType">操作类型</param>
        internal OperationBool(Abstract.Node parent, Abstract.Node value, OperationParameter.OperationType operationType) : base(parent)
        {
            Parameter = value.Parameter;
            Parameter.OperationType = operationType;
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<bool> Operation()
        {
            return Client.GetBool(Parent.ClientDataStructure.Client.Operation(this));
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Operation(Action<ReturnValue<bool>> onGet)
        {
            Parent.ClientDataStructure.Client.Operation(this, onGet);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Operation(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            Parent.ClientDataStructure.Client.OperationReturnParameter(this, onGet);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void OperationStream(Action<ReturnValue<bool>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Parent.ClientDataStructure.Client.OperationStream(this, onGet);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void OperationStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Parent.ClientDataStructure.Client.OperationStream(this, onGet);
        }
    }
}

