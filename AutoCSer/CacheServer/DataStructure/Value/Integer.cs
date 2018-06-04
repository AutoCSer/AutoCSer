using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Value
{
    /// <summary>
    /// 整数数据节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed partial class Integer<valueType> : Abstract.Value<valueType>, Abstract.IValue
    {
        /// <summary>
        /// 整数数据节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private Integer(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 整数数据节点
        /// </summary>
        /// <param name="value">数据</param>
        public Integer(valueType value) : base(value) { }
        /// <summary>
        /// 数据转换为节点
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Integer<valueType>(valueType value)
        {
            return new Integer<valueType>(value);
        }
#if NOJIT
        /// <summary>
        /// 创建整数数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Integer<valueType> create(Abstract.Node parent)
        {
            return new Integer<valueType>(parent);
        }
#endif
        /// <summary>
        /// 获取返回值数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.ReturnValue<valueType> Get(ref ReturnValueNode<Integer<valueType>> value)
        {
            return value.Value.Get(ValueData.Data<valueType>.GetData);
        }


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryReturnValue<Integer<valueType>> GetNode()
        {
            return new Parameter.QueryReturnValue<Integer<valueType>>(Parent, ref Parameter, OperationParameter.OperationType.GetValue);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public ReturnValueNode<Integer<valueType>> Get()
        {
            return new ReturnValueNode<Integer<valueType>>(ClientDataStructure.Client.Query(GetNode()));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="onGet"></param>
        public void Get(Action<ReturnValueNode<Integer<valueType>>> onGet)
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
        public void GetStream(Action<ReturnValueNode<Integer<valueType>>> onGet)
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

        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, Integer<valueType>> constructor;
        static Integer()
        {
            if (!ValueData.Data<valueType>.IsInteger) throw new InvalidCastException("不支持整数数据类型 " + typeof(valueType).fullName());
#if NOJIT
            constructor = (Func<Abstract.Node, Integer<valueType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, Integer<valueType>>), typeof(Integer<valueType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, Integer<valueType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(Integer<valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}


