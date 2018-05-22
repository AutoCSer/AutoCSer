using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 操作参数节点
    /// </summary>
    public abstract partial class Operation : Node
    {
        /// <summary>
        /// 操作参数节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="operationType">操作类型</param>
        internal Operation(Abstract.Node parent, OperationParameter.OperationType operationType) : base(parent, operationType) { }
        /// <summary>
        /// 操作参数节点
        /// </summary>
        /// <param name="parent">父节点</param>
        internal Operation(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void OperationOnly()
        {
            ClientDataStructure.Client.OperationOnly(this);
        }
    }
}
