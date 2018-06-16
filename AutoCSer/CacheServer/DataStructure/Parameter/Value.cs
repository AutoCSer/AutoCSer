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
        /// <param name="operationType">操作类型</param>
        internal Value(Abstract.Node parent, OperationParameter.OperationType operationType) : base(parent, operationType) { }
        ///// <summary>
        ///// 数据节点
        ///// </summary>
        ///// <param name="parent">父节点</param>
        ///// <param name="operationType">操作类型</param>
        ///// <param name="value">数据</param>
        //internal Value(Abstract.Node parent, OperationParameter.OperationType operationType, Abstract.Node value) : base(parent, operationType)
        //{
        //    Parameter = value.Parameter;
        //}
        /// <summary>
        /// 数据节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="value">数据</param>
        internal Value(Abstract.Node parent, OperationParameter.OperationType operationType, int value) : base(parent, operationType)
        {
            Parameter.Set(value);
        }
        /// <summary>
        /// 数据节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="value">数据</param>
        internal Value(Abstract.Node parent, OperationParameter.OperationType operationType, long value) : base(parent, operationType)
        {
            Parameter.Set(value);
        }
        /// <summary>
        /// 数据节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="value">数据</param>
        internal Value(Abstract.Node parent, OperationParameter.OperationType operationType, ulong value) : base(parent, operationType)
        {
            Parameter.Set(value);
        }
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
        /// 数据节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="value">数据</param>
        internal Value(Abstract.Node parent, uint value) : base(parent)
        {
            Parameter.Set(value);
        }
    }
}
