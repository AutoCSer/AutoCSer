using System;
using AutoCSer.CacheServer.DataStructure.Abstract;

namespace AutoCSer.CacheServer.DataStructure.Parameter
{
    /// <summary>
    /// 查询参数节点
    /// </summary>
    public abstract class Node : Abstract.Node
    {
        /// <summary>
        /// 查询节点
        /// </summary>
        /// <param name="parent">父节点</param>
        internal Node(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 查询节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="operationType">操作类型</param>
        internal Node(Abstract.Node parent, OperationParameter.OperationType operationType) : base(parent)
        {
            Parameter.OperationType = operationType;
        }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 创建数据节点
        /// </summary>
        /// <returns></returns>
        internal override Abstract.Node CreateValueNode()
        {
            throw new NotImplementedException();
        }
    }
}
