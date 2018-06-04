using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Value
{
    /// <summary>
    /// 二进制序列化数据节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed partial class Binary<valueType> : Abstract.Node//, Abstract.IValue
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
        /// Json 数据节点
        /// </summary>
        /// <param name="data">数据</param>
        internal Binary(ref ValueData.Data data)
        {
            Parameter = data;
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
        /// 获取二进制反序列化数据
        /// </summary>
        /// <returns></returns>
        internal ReturnValue<valueType> Get(ref ReturnValue<Binary<valueType>> returnValue)
        {
            if (returnValue.Type == ReturnType.Success)
            {
                valueType value = default(valueType);
                if (Parameter.GetBinary(ref value)) return value;
                return new ReturnValue<valueType> { Type = ReturnType.DeSerializeError };
            }
            return new ReturnValue<valueType> { Type = returnValue.TcpReturnType == Net.TcpServer.ReturnType.Success ? returnValue.Type : ReturnType.TcpError, TcpReturnType = returnValue.TcpReturnType };
        }
        /// <summary>
        /// 获取二进制反序列化数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool TryGet(ref valueType value)
        {
            return Parameter.GetBinary(ref value);
        }
        /// <summary>
        /// 获取 JSON 反序列化数据
        /// </summary>
        /// <param name="errorValue">失败返回值</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType Get(valueType errorValue = default(valueType))
        {
            valueType value = default(valueType);
            return Parameter.GetBinary(ref value) ? value : errorValue;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryReturnValue<Binary<valueType>> GetQueryNode()
        {
            return new Parameter.QueryReturnValue<Binary<valueType>>(Parent, ref Parameter, OperationParameter.OperationType.GetValue);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public ReturnValue<Binary<valueType>> Query()
        {
            return new ReturnValue<Binary<valueType>>(ClientDataStructure.Client.Query(GetQueryNode()));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="onGet"></param>
        public void Query(Action<ReturnValue<Binary<valueType>>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetQueryNode(), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="onGet"></param>
        public void Query(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.Query(GetQueryNode(), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void QueryStream(Action<ReturnValue<Binary<valueType>>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryStream(GetQueryNode(), onGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="onGet">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        /// <returns></returns>
        public void QueryStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onGet)
        {
            if (onGet == null) throw new ArgumentNullException();
            ClientDataStructure.Client.QueryStream(GetQueryNode(), onGet);
        }

        /// <summary>
        /// 获取参数数据委托
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static valueType GetData(ref ValueData.Data parameter)
        {
            valueType value = default(valueType);
            return parameter.GetBinary(ref value) ? value : default(valueType);
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
            constructor = (Func<Abstract.Node, Binary<valueType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(Binary<valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}
