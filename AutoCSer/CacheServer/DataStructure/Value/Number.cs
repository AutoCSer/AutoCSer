using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Value
{
    /// <summary>
    /// 数字数据节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed partial class Number<valueType> : Abstract.Value<valueType>, Abstract.IValue
    {
        /// <summary>
        /// 数字数据节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private Number(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 数字数据节点
        /// </summary>
        /// <param name="value">数据</param>
        public Number(valueType value) : base(value) { }
        /// <summary>
        /// 数据转换为节点
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Number<valueType>(valueType value)
        {
            return new Number<valueType>(value);
        }
#if NOJIT
        /// <summary>
        /// 创建数字数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Number<valueType> create(Abstract.Node parent)
        {
            return new Number<valueType>(parent);
        }
#endif
        /// <summary>
        /// 获取返回值数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.ReturnValue<valueType> Get(ref ReturnValueNode<Number<valueType>> value)
        {
            return value.Value.Get(ValueData.Data<valueType>.GetData);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.QueryReturnValue<Number<valueType>> GetNode()
        {
            return new Parameter.QueryReturnValue<Number<valueType>>(Parent, ref Parameter, OperationParameter.OperationType.GetValue);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public ReturnValue<Number<valueType>> Get()
        {
            return new ReturnValue<Number<valueType>>(ClientDataStructure.Client.Query(GetNode()));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="onGet"></param>
        public void Get(Action<ReturnValue<Number<valueType>>> onGet)
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
        public void GetStream(Action<ReturnValue<Number<valueType>>> onGet)
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
        private static readonly Func<Abstract.Node, Number<valueType>> constructor;
        static Number()
        {
            if (!ValueData.Data<valueType>.IsNumber) throw new InvalidCastException("不支持数字数据类型 " + typeof(valueType).fullName());
#if NOJIT
            constructor = (Func<Abstract.Node, Number<valueType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, Number<valueType>>), typeof(Number<valueType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, Number<valueType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(Number<valueType>), NodeConstructorParameterTypes);
#endif
        }
    }
}

