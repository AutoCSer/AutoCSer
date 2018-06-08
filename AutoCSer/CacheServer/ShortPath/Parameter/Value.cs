using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.ShortPath.Parameter
{
    /// <summary>
    /// 数据参数节点
    /// </summary>
    internal sealed partial class Value : Node
    {
        /// <summary>
        /// 操作参数节点
        /// </summary>
        /// <param name="node">短路径节点</param>
        internal Value(ShortPath.Node node) : base(node) { }
        /// <summary>
        /// 操作参数节点
        /// </summary>
        /// <param name="node">短路径节点</param>
        /// <param name="value">数据</param>
        internal Value(ShortPath.Node node, DataStructure.Abstract.Node value) : base(node)
        {
            Parameter = value.Parameter;
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
