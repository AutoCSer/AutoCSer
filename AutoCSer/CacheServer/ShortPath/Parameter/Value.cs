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
        /// 数据参数节点
        /// </summary>
        /// <param name="node">短路径节点</param>
        internal Value(ShortPath.Node node) : base(node) { }
        /// <summary>
        /// 数据参数节点
        /// </summary>
        /// <param name="node">短路径节点</param>
        /// <param name="value">数据</param>
        internal Value(ShortPath.Node node, DataStructure.Abstract.Node value) : base(node)
        {
            Parameter = value.Parameter;
        }
        /// <summary>
        /// 数据参数节点
        /// </summary>
        /// <param name="parent">短路径节点</param>
        /// <param name="value">数据</param>
        internal Value(ShortPath.Node parent, int value) : base(parent)
        {
            Parameter.Set(value);
        }
        ///// <summary>
        ///// 数据参数节点
        ///// </summary>
        ///// <param name="parent">父节点</param>
        ///// <param name="operationType">操作类型</param>
        //internal Value(Node parent, OperationParameter.OperationType operationType) : base(parent, operationType) { }
        /// <summary>
        /// 数据参数节点
        /// </summary>
        /// <param name="node">短路径节点</param>
        /// <param name="value">数据</param>
        /// <param name="operationType">操作类型</param>
        internal Value(ShortPath.Node node, DataStructure.Abstract.Node value, OperationParameter.OperationType operationType) : base(node)
        {
            Parameter = value.Parameter;
            Parameter.OperationType = operationType;
        }
    }
}
