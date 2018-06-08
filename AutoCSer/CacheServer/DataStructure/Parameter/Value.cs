using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 数据参数节点
    /// </summary>
    internal sealed partial class Value : Node
    {
        /// <summary>
        /// 数据节点
        /// </summary>
        /// <param name="parent">父节点</param>
        internal Value(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 数据节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="value">数据</param>
        internal Value(Abstract.Node parent, int value) : base(parent)
        {
            Parameter.Set(value);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set<valueType>(valueType value)
        {
            ValueData.Data<valueType>.SetData(ref Parameter, value);
        }
    }
}
