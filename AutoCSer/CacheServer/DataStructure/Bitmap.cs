using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 位图节点
    /// </summary>
    public sealed partial class Bitmap : Abstract.Node, Abstract.IValueNode
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
        /// 链表节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private Bitmap(Abstract.Node parent) : base(parent) { }
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
            stream.Write((byte)Abstract.NodeType.Bitmap);
            stream.Write((byte)ValueData.DataType.Null);
            serializeParentDataStructure(stream);
        }

        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<ShortPath.Bitmap> CreateShortPath()
        {
            if (Parent != null) return new ShortPath.Bitmap(this).Create();
            return new ReturnValue<ShortPath.Bitmap> { Type = ReturnType.CanNotCreateShortPath };
        }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="onCreated"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CreateShortPath(Action<ReturnValue<ShortPath.Bitmap>> onCreated)
        {
            if (onCreated == null) throw new ArgumentNullException();
            if (Parent != null) new ShortPath.Bitmap(this).Create(onCreated);
            else onCreated(new ReturnValue<ShortPath.Bitmap> { Type = ReturnType.CanNotCreateShortPath });
        }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="onCreated">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CreateShortPathStream(Action<ReturnValue<ShortPath.Bitmap>> onCreated)
        {
            if (onCreated == null) throw new ArgumentNullException();
            if (Parent != null) new ShortPath.Bitmap(this).CreateStream(onCreated);
            else onCreated(new ReturnValue<ShortPath.Bitmap> { Type = ReturnType.CanNotCreateShortPath });
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
        public ReturnValue<bool> Clear()
        {
            return Client.GetBool(ClientDataStructure.Client.Operation(GetClearNode()));
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        /// <param name="onClear"></param>
        /// <returns></returns>
        public void Clear(Action<ReturnValue<bool>> onClear)
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
        public void ClearStream(Action<ReturnValue<bool>> onClear)
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
            return Client.GetBool(ClientDataStructure.Client.Query(GetNode(index)));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onGet"></param>
        public void Get(uint index, Action<ReturnValue<bool>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetNode(index), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onGet"></param>
        public void Get(uint index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetNode(index), onGet);
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
            ClientDataStructure.Client.QueryStream(GetNode(index), onGet);
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
            ClientDataStructure.Client.QueryStream(GetNode(index), onGet);
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
            return Client.GetBool(ClientDataStructure.Client.Operation(GetSetNode(index)));
        }
        /// <summary>
        /// 设置数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onSet">设置前的位状态</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(uint index, Action<ReturnValue<bool>> onSet)
        {
            ClientDataStructure.Client.Operation(GetSetNode(index), onSet);
        }
        /// <summary>
        /// 设置数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onSet">设置前的位状态</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(uint index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
        {
            ClientDataStructure.Client.OperationReturnParameter(GetSetNode(index), onSet);
        }
        /// <summary>
        /// 设置数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onSet">设置前的位状态，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetStream(uint index, Action<ReturnValue<bool>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetSetNode(index), onSet);
        }
        /// <summary>
        /// 设置数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onSet">设置前的位状态，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetStream(uint index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onSet)
        {
            if (onSet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetSetNode(index), onSet);
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
            return Client.GetBool(ClientDataStructure.Client.Operation(GetClearNode(index)));
        }
        /// <summary>
        /// 清除数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onClear">清除前的位状态</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Clear(uint index, Action<ReturnValue<bool>> onClear)
        {
            ClientDataStructure.Client.Operation(GetClearNode(index), onClear);
        }
        /// <summary>
        /// 清除数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onClear">清除前的位状态</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Clear(uint index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onClear)
        {
            ClientDataStructure.Client.OperationReturnParameter(GetClearNode(index), onClear);
        }
        /// <summary>
        /// 清除数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onClear">清除前的位状态，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void ClearStream(uint index, Action<ReturnValue<bool>> onClear)
        {
            if (onClear == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetClearNode(index), onClear);
        }
        /// <summary>
        /// 清除数据位
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onClear">清除前的位状态，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void ClearStream(uint index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onClear)
        {
            if (onClear == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetClearNode(index), onClear);
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
            return Client.GetBool(ClientDataStructure.Client.Operation(GetSetNegateNode(index)));
        }
        /// <summary>
        /// 数据位取反
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onNegate">取反后的结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetNegate(uint index, Action<ReturnValue<bool>> onNegate)
        {
            ClientDataStructure.Client.Operation(GetSetNegateNode(index), onNegate);
        }
        /// <summary>
        /// 数据位取反
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onNegate">取反后的结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetNegate(uint index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onNegate)
        {
            ClientDataStructure.Client.OperationReturnParameter(GetSetNegateNode(index), onNegate);
        }
        /// <summary>
        /// 数据位取反
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onNegate">取反后的结果，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetNegateStream(uint index, Action<ReturnValue<bool>> onNegate)
        {
            if (onNegate == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetSetNegateNode(index), onNegate);
        }
        /// <summary>
        /// 数据位取反
        /// </summary>
        /// <param name="index"></param>
        /// <param name="onNegate">取反后的结果，直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetNegateStream(uint index, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onNegate)
        {
            if (onNegate == null) throw new ArgumentNullException();
            ClientDataStructure.Client.OperationStream(GetSetNegateNode(index), onNegate);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, Bitmap> constructor;
#if NOJIT
        /// <summary>
        /// 创建位图节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Bitmap create(Abstract.Node parent)
        {
            return new Bitmap(parent);
        }
#endif
        static Bitmap()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, Bitmap>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, Bitmap>), typeof(Bitmap).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, Bitmap>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(Bitmap), NodeConstructorParameterTypes);
#endif
        }
    }
}
