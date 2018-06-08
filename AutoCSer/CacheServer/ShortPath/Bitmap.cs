using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 位图节点 短路径
    /// </summary>
    public sealed partial class Bitmap : Node<Bitmap>
    {
        /// <summary>
        /// 获取元素节点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool this[uint index]
        {
            get
            {
                ReturnValue<bool> value = Get(index);
                if (value.Type != ReturnType.Success) throw new Exception(value.Type.ToString());
                return value.Value;
            }
            set
            {
                ReturnValue<bool> isSet = value ? Set(index) : Clear(index);
                if (!isSet) throw new Exception(isSet.Type.ToString());
            }
        }
        /// <summary>
        /// 哈希表节点 短路径
        /// </summary>
        /// <param name="node"></param>
        internal Bitmap(DataStructure.Bitmap node) : base(node) { }


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
            return Client.GetBool(Client.Operation(GetClearNode()));
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <param name="onClear"></param>
        /// <returns></returns>
        public void Clear(Action<AutoCSer.CacheServer.ReturnValue<bool>> onClear)
        {
            if (onClear == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetClearNode(), onClear);
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <param name="onClear"></param>
        /// <returns></returns>
        public void Clear(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onClear)
        {
            if (onClear == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetClearNode(), onClear);
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <param name="onClear">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void ClearStream(Action<AutoCSer.CacheServer.ReturnValue<bool>> onClear)
        {
            if (onClear == null) throw new ArgumentNullException();
            Client.OperationStream(GetClearNode(), onClear);
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <param name="onClear">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void ClearStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onClear)
        {
            if (onClear == null) throw new ArgumentNullException();
            Client.OperationStream(GetClearNode(), onClear);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryBool GetNode(uint index)
        {
            Parameter.QueryBool node = new Parameter.QueryBool(this, OperationParameter.OperationType.GetValue);
            node.Parameter.Set(index);
            return node;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<bool> Get(uint index)
        {
            return Client.GetBool(Client.Query(GetNode(index)));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onGet"></param>
        public void Get(uint index, Action<ReturnValue<bool>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Client.Query(GetNode(index), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onGet"></param>
        public void Get(uint index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Client.Query(GetNode(index), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetStream(uint index, Action<ReturnValue<bool>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Client.QueryStream(GetNode(index), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetStream(uint index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            Client.QueryStream(GetNode(index), onGet);
        }

        /// <summary>
        /// 获取操作节点
        /// </summary>
        /// <param name="index"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private Parameter.OperationBool getOperationNode(uint index, OperationParameter.OperationType operationType)
        {
            Parameter.OperationBool node = new Parameter.OperationBool(this, operationType);
            node.Parameter.Set(index);
            return node;
        }
        /// <summary>
        /// 设置数据位
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetSetNode(uint index)
        {
            return getOperationNode(index, OperationParameter.OperationType.SetValue);
        }
        /// <summary>
        /// 设置数据位
        /// </summary>
        /// <param name="index"></param>
        /// <returns>设置前的位状态</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<bool> Set(uint index)
        {
            return Client.GetBool(Client.Operation(GetSetNode(index)));
        }
        /// <summary>
        /// 设置数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onSet">设置前的位状态</param>
        public void Set(uint index, Action<ReturnValue<bool>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetSetNode(index), onSet);
        }
        /// <summary>
        /// 设置数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onSet">设置前的位状态</param>
        public void Set(uint index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetSetNode(index), onSet);
        }
        /// <summary>
        /// 设置数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onSet">设置前的位状态，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        public void SetStream(uint index, Action<ReturnValue<bool>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Client.OperationStream(GetSetNode(index), onSet);
        }
        /// <summary>
        /// 设置数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onSet">设置前的位状态，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        public void SetStream(uint index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            Client.OperationStream(GetSetNode(index), onSet);
        }

        /// <summary>
        /// 清除数据位
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetClearNode(uint index)
        {
            return getOperationNode(index, OperationParameter.OperationType.Remove);
        }
        /// <summary>
        /// 清除数据位
        /// </summary>
        /// <param name="index"></param>
        /// <returns>清除前的位状态</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<bool> Clear(uint index)
        {
            return Client.GetBool(Client.Operation(GetClearNode(index)));
        }
        /// <summary>
        /// 清除数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onClear">清除前的位状态</param>
        public void Clear(uint index, Action<ReturnValue<bool>> onClear)
        {
            if (onClear == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetClearNode(index), onClear);
        }
        /// <summary>
        /// 清除数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onClear">清除前的位状态</param>
        public void Clear(uint index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onClear)
        {
            if (onClear == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetClearNode(index), onClear);
        }
        /// <summary>
        /// 清除数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onClear">清除前的位状态，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        public void ClearStream(uint index, Action<ReturnValue<bool>> onClear)
        {
            if (onClear == null) throw new ArgumentNullException();
            Client.OperationStream(GetClearNode(index), onClear);
        }
        /// <summary>
        /// 清除数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onClear">清除前的位状态，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        public void ClearStream(uint index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onClear)
        {
            if (onClear == null) throw new ArgumentNullException();
            Client.OperationStream(GetClearNode(index), onClear);
        }

        /// <summary>
        /// 数据位取反
        /// </summary>
        /// <param name="index"></param>
        /// <returns>取反后的结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetSetNegateNode(uint index)
        {
            return getOperationNode(index, OperationParameter.OperationType.SetNegate);
        }
        /// <summary>
        /// 数据位取反
        /// </summary>
        /// <param name="index"></param>
        /// <returns>取反后的结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<bool> SetNegate(uint index)
        {
            return Client.GetBool(Client.Operation(GetSetNegateNode(index)));
        }
        /// <summary>
        /// 数据位取反
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onNegate">取反后的结果</param>
        public void SetNegate(uint index, Action<ReturnValue<bool>> onNegate)
        {
            if (onNegate == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetSetNegateNode(index), onNegate);
        }
        /// <summary>
        /// 数据位取反
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onNegate">取反后的结果</param>
        public void SetNegate(uint index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onNegate)
        {
            if (onNegate == null) throw new ArgumentNullException();
            Client.OperationNotNull(GetSetNegateNode(index), onNegate);
        }
        /// <summary>
        /// 数据位取反
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onNegate">取反后的结果，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        public void SetNegateStream(uint index, Action<ReturnValue<bool>> onNegate)
        {
            if (onNegate == null) throw new ArgumentNullException();
            Client.OperationStream(GetSetNegateNode(index), onNegate);
        }
        /// <summary>
        /// 数据位取反
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onNegate">取反后的结果，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        public void SetNegateStream(uint index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onNegate)
        {
            if (onNegate == null) throw new ArgumentNullException();
            Client.OperationStream(GetSetNegateNode(index), onNegate);
        }
    }
}
