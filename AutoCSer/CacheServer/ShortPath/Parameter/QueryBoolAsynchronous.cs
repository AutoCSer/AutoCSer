using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.ShortPath.Parameter
{
    /// <summary>
    /// 查询参数节点
    /// </summary>
    public sealed partial class QueryBoolAsynchronous : Node
    {
        /// <summary>
        /// 查询节点
        /// </summary>
        /// <param name="node">短路径节点</param>
        internal QueryBoolAsynchronous(ShortPath.Node node) : base(node) { }
        /// <summary>
        /// 查询节点
        /// </summary>
        /// <param name="node">短路径节点</param>
        /// <param name="value">数据</param>
        /// <param name="operationType">操作类型</param>
        internal QueryBoolAsynchronous(ShortPath.Node node, DataStructure.Abstract.Node value, OperationParameter.OperationType operationType) : base(node)
        {
            Parameter = value.Parameter;
            Parameter.OperationType = operationType;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<bool> Query()
        {
            return Client.GetBool(ShortPath.Client.QueryAsynchronous(this));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Query(Action<ReturnValue<bool>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ShortPath.Client.QueryAsynchronous(this, onGet);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void QueryStream(Action<ReturnValue<bool>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ShortPath.Client.QueryAsynchronousStream(this, onGet);
        }
    }
}
