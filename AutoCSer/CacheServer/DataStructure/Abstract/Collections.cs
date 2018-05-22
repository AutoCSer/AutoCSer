using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 集合节点
    /// </summary>
    public abstract partial class Collections : Node
    {
        /// <summary>
        /// 集合节点
        /// </summary>
        /// <param name="parent">父节点</param>
        protected Collections(Node parent) : base(parent) { }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        public AutoCSer.CacheServer.ReturnValue<int> Count
        {
            get
            {
                return Client.GetInt(ClientDataStructure.Client.Query(GetCountNode()));
            }
        }
        /// <summary>
        /// 获取数据数量查询节点
        /// </summary>
        /// <returns>获取数据数量查询节点</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryInt GetCountNode()
        {
            return new Parameter.QueryInt(this, OperationParameter.OperationType.GetCount);
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void GetCount(Action<AutoCSer.CacheServer.ReturnValue<int>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetCountNode(), onGet);
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void GetCountStream(Action<AutoCSer.CacheServer.ReturnValue<int>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryStream(GetCountNode(), onGet);
        }

        /// <summary>
        /// 获取清除数据查询节点
        /// </summary>
        /// <returns>清除数据查询节点</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetClearNode()
        {
            return new Parameter.OperationBool(this, OperationParameter.OperationType.Clear);
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <returns></returns>
        public AutoCSer.CacheServer.ReturnValue<bool> Clear()
        {
            return Client.GetBool(ClientDataStructure.Client.Operation(GetClearNode()));
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <param name="onClear"></param>
        /// <returns></returns>
        public void Clear(Action<AutoCSer.CacheServer.ReturnValue<bool>> onClear)
        {
            ClientDataStructure.Client.Operation(GetClearNode(), onClear);
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <param name="onClear"></param>
        /// <returns></returns>
        public void Clear(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onClear)
        {
            ClientDataStructure.Client.OperationReturnParameter(GetClearNode(), onClear);
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <param name="onClear">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void ClearStream(Action<AutoCSer.CacheServer.ReturnValue<bool>> onClear)
        {
            if (onClear == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetClearNode(), onClear);
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <param name="onClear">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void ClearStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onClear)
        {
            if (onClear == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetClearNode(), onClear);
        }
    }
}
