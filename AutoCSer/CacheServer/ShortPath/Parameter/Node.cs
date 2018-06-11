using System;

namespace AutoCSer.CacheServer.ShortPath.Parameter
{
    /// <summary>
    /// 短路径查询节点
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// 父节点
        /// </summary>
        private Node parent;
        /// <summary>
        /// 参数数据
        /// </summary>
        internal ValueData.Data Parameter;
        /// <summary>
        /// 短路径
        /// </summary>
        internal ShortPath.Node ShortPath
        {
            get { return parent != null ? parent.ShortPath : new UnionType { Value = Parameter.Value }.ShortPath; }
        }
        /// <summary>
        /// 查询节点
        /// </summary>
        /// <param name="node">短路径节点</param>
        internal Node(ShortPath.Node node)
        {
            Parameter.Value = node;
        }
        /// <summary>
        /// 查询节点
        /// </summary>
        /// <param name="node">短路径节点</param>
        /// <param name="operationType">操作类型</param>
        internal Node(ShortPath.Node node, OperationParameter.OperationType operationType)
        {
            Parameter.Value = node;
            Parameter.OperationType = operationType;
        }
        /// <summary>
        /// 查询节点
        /// </summary>
        /// <param name="parent">父节点</param>
        internal Node(Node parent)
        {
            this.parent = parent;
        }
        /// <summary>
        /// 查询节点
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="operationType">操作类型</param>
        internal Node(Node parent, OperationParameter.OperationType operationType)
        {
            this.parent = parent;
            Parameter.OperationType = operationType;
        }
        /// <summary>
        /// 序列化参数信息
        /// </summary>
        /// <param name="stream"></param>
        internal void SerializeParameter(UnmanagedStream stream)
        {
            if (parent != null) parent.SerializeParameter(stream);
            else ShortPath.Identity.Value.UnsafeSerialize(stream);
            Parameter.Serialize(stream);
        }
    }
}
