using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Value
{
    /// <summary>
    /// 二进制序列化数据节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed partial class Binary<valueType> : Abstract.Node, Abstract.IValue
    {
        /// <summary>
        /// 二进制序列化数据节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private Binary(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 二进制序列化数据节点
        /// </summary>
        /// <param name="value">数据</param>
        public Binary(valueType value)
        {
            Parameter.SetBinary(value);
        }
        /// <summary>
        /// 数据转换为节点
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Binary<valueType>(valueType value)
        {
            return new Binary<valueType>(value);
        }
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
            stream.Write((ushort)((byte)Abstract.NodeType.Value + ((int)(byte)ValueData.DataType.BinarySerialize << 8)));
            serializeParentDataStructure(stream);
        }

        /// <summary>
        /// 获取返回值数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.ReturnValue<valueType> Get(ref ReturnValueNode<Binary<valueType>> value)
        {
            return value.Value.GetBinary<valueType>();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryReturnValue<Binary<valueType>> GetNode()
        {
            return new Parameter.QueryReturnValue<Binary<valueType>>(Parent, ref Parameter, OperationParameter.OperationType.GetValue);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public ReturnValueNode<Binary<valueType>> Get()
        {
            return new ReturnValueNode<Binary<valueType>>(ClientDataStructure.Client.Query(GetNode()));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="onGet"></param>
        public void Get(Action<ReturnValueNode<Binary<valueType>>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetNode(), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="onGet"></param>
        public void Get(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetNode(), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetStream(Action<ReturnValueNode<Binary<valueType>>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryStream(GetNode(), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void GetStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryStream(GetNode(), onGet);
        }
#if NOJIT
        /// <summary>
        /// 创建二进制序列化数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Binary<valueType> create(Abstract.Node parent)
        {
            return new Binary<valueType>(parent);
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, Binary<valueType>> constructor;
        static Binary()
        {
#if NOJIT
            constructor = (Func<Abstract.Node, Binary<valueType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, Binary<valueType>>), typeof(Binary<valueType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, Binary<valueType>>)AutoCSer.Emit.Constructor.Create(typeof(Binary<valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
