using System;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 查询参数节点（保持回调）
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public sealed partial class QueryKeepCallbackReturnValue<valueType> : Node
    {
        /// <summary>
        /// 查询参数节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="operationType">操作类型</param>
        internal QueryKeepCallbackReturnValue(Abstract.Node parent, OperationParameter.OperationType operationType) : base(parent, operationType) { }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="onGet"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback Query(Action<ReturnValue<valueType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.QueryKeepCallback(this, onGet);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="onGet"></param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback Query(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.QueryKeepCallback(this, onGet);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback QueryStream(Action<ReturnValue<valueType>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.QueryKeepCallbackStream(this, onGet);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public AutoCSer.Net.TcpServer.KeepCallback QueryStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            return ClientDataStructure.Client.QueryKeepCallbackStream(this, onGet);
        }
    }
}
