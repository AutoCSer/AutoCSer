using System;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 数据参数节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed partial class Value<valueType> : Node
    {
        /// <summary>
        /// 数据节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="value">数据</param>
        internal Value(Abstract.Node parent, valueType value) : base(parent)
        {
            ValueData.Data<valueType>.SetData(ref Parameter, value);
        }
    }
}
