using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 查询参数节点
    /// </summary>
    public partial struct QueryBoolValueAsynchronous
    {
        /// <summary>
        /// 查询参数节点
        /// </summary>
        public readonly Abstract.Node Node;
        /// <summary>
        /// 查询参数节点
        /// </summary>
        /// <param name="node"></param>
        internal QueryBoolValueAsynchronous(Abstract.Node node)
        {
            Node = node;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<bool> Query()
        {
            return Client.GetBool(Node.Parent.ClientDataStructure.Client.QueryAsynchronous(Node));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Query(Action<ReturnValue<bool>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Node.Parent.ClientDataStructure.Client.QueryAsynchronous(Node, onGet);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Query(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Node.Parent.ClientDataStructure.Client.QueryAsynchronous(Node, onGet);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void QueryStream(Action<ReturnValue<bool>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Node.Parent.ClientDataStructure.Client.QueryAsynchronousStream(Node, onGet);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void QueryStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Node.Parent.ClientDataStructure.Client.QueryAsynchronousStream(Node, onGet);
        }
    }
}
