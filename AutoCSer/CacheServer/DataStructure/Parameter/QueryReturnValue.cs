using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 查询参数节点
    /// </summary>
    /// <typeparam name="nodeType"></typeparam>
    public sealed partial class QueryReturnValue<nodeType> : Operation
        where nodeType : Abstract.Node, Abstract.IValue
    {
        /// <summary>
        /// 查询参数节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="operationType">操作类型</param>
        internal QueryReturnValue(Abstract.Node parent, OperationParameter.OperationType operationType) : base(parent, operationType) { }
        /// <summary>
        /// 查询参数节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="parameter">查询参数</param>
        /// <param name="operationType">操作类型</param>
        internal QueryReturnValue(Abstract.Node parent, ref ValueData.Data parameter, OperationParameter.OperationType operationType) : base(parent)
        {
            Parameter = parameter;
            Parameter.OperationType = operationType;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValueNode<nodeType> Operation()
        {
            return new ReturnValueNode<nodeType>(ClientDataStructure.Client.Query(this));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Query(Action<ReturnValueNode<nodeType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(this, onGet);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Query(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(this, onGet);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void QueryStream(Action<ReturnValueNode<nodeType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryStream(this, onGet);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void QueryStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryStream(this, onGet);
        }
    }
    ///// <summary>
    ///// 查询参数节点
    ///// </summary>
    ///// <typeparam name="valueType"></typeparam>
    //public sealed partial class QueryReturnValueNew<valueType> : Operation
    //{
    //    /// <summary>
    //    /// 查询参数节点
    //    /// </summary>
    //    /// <param name="parent">父节点</param>
    //    /// <param name="operationType">操作类型</param>
    //    internal QueryReturnValueNew(Abstract.Node parent, OperationParameter.OperationType operationType) : base(parent, operationType) { }
    //    /// <summary>
    //    /// 查询参数节点
    //    /// </summary>
    //    /// <param name="parent">父节点</param>
    //    /// <param name="parameter">查询参数</param>
    //    /// <param name="operationType">操作类型</param>
    //    internal QueryReturnValueNew(Abstract.Node parent, ref ValueData.Data parameter, OperationParameter.OperationType operationType) : base(parent)
    //    {
    //        Parameter = parameter;
    //        Parameter.OperationType = operationType;
    //    }
    //    /// <summary>
    //    /// 查询数据
    //    /// </summary>
    //    /// <returns></returns>
    //    [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
    //    public Abstract.ReturnValueNew<valueType> Operation()
    //    {
    //        return new Abstract.ReturnValueNew<valueType>(ClientDataStructure.Client.Query(this));
    //    }
    //    /// <summary>
    //    /// 查询数据
    //    /// </summary>
    //    /// <param name="onGet"></param>
    //    [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
    //    public void Query(Action<Abstract.ReturnValueNew<valueType>> onGet)
    //    {
    //        if (onGet == null) throw new ArgumentNullException();
    //        ClientDataStructure.Client.Query(this, onGet);
    //    }
    //    /// <summary>
    //    /// 查询数据
    //    /// </summary>
    //    /// <param name="onGet"></param>
    //    [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
    //    public void Query(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
    //    {
    //        if (onGet == null) throw new ArgumentNullException();
    //        ClientDataStructure.Client.Query(this, onGet);
    //    }
    //    /// <summary>
    //    /// 查询数据
    //    /// </summary>
    //    /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
    //    [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
    //    public void QueryStream(Action<Abstract.ReturnValueNew<valueType>> onGet)
    //    {
    //        if (onGet == null) throw new ArgumentNullException();
    //        ClientDataStructure.Client.QueryStream(this, onGet);
    //    }
    //    /// <summary>
    //    /// 查询数据
    //    /// </summary>
    //    /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
    //    [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
    //    public void QueryStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
    //    {
    //        if (onGet == null) throw new ArgumentNullException();
    //        ClientDataStructure.Client.QueryStream(this, onGet);
    //    }
    //}
}

