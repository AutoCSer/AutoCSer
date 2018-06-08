using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 查询参数节点
    /// </summary>
    public sealed partial class QueryOnly : Node
    {
        /// <summary>
        /// 查询节点
        /// </summary>
        /// <param name="parent">父节点</param>
        internal QueryOnly(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 查询节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="operationType">操作类型</param>
        internal QueryOnly(Abstract.Node parent, OperationParameter.OperationType operationType) : base(parent, operationType) { }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Query()
        {
            Parent.ClientDataStructure.Client.QueryOnly(this);
        }
    }
}
