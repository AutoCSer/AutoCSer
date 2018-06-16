using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache
{
    /// <summary>
    /// 缓存节点
    /// </summary>
    internal abstract class Node
    {
#if NOJIT
        /// <summary>
        /// 创建节点函数名称
        /// </summary>
        internal const string CreateMethodName = "create";
#endif
        /// <summary>
        /// 节点信息字段名称
        /// </summary>
        internal const string NodeInfoFieldName = "nodeInfo";

        /// <summary>
        /// 父节点
        /// </summary>
        private Node parent;
        /// <summary>
        /// 节点是否有效（没有被删除）
        /// </summary>
        internal bool IsNode
        {
            get
            {
                return parent != null && (ReferenceEquals(parent, this) || parent.IsNode);
            }
        }
        /// <summary>
        /// 缓存节点
        /// </summary>
        /// <param name="parent"></param>
        protected Node(Node parent)
        {
            this.parent = parent ?? this;
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal void Operation(ref OperationParameter.NodeParser parser)
        {
            if (parser.LoadValueData())
            {
                if (parser.IsEnd) OperationEnd(ref parser);
                else
                {
                    Node node = GetOperationNext(ref parser);
                    if (node != null) node.Operation(ref parser);
                }
            }
            else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal abstract Node GetOperationNext(ref OperationParameter.NodeParser parser);
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal abstract void OperationEnd(ref OperationParameter.NodeParser parser);
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal void Query(ref OperationParameter.NodeParser parser)
        {
            if (parser.LoadValueData())
            {
                if (parser.IsEnd) QueryEnd(ref parser);
                else
                {
                    Node node = GetQueryNext(ref parser);
                    if (node != null) node.Query(ref parser);
                }
            }
            else parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal abstract Node GetQueryNext(ref OperationParameter.NodeParser parser);
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal abstract void QueryEnd(ref OperationParameter.NodeParser parser);

        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="parser"></param>
        internal virtual void CreateShortPath(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.ReturnType = ReturnType.CanNotCreateShortPath;
        }

        /// <summary>
        /// 删除节点操作
        /// </summary>
        internal virtual void OnRemoved()
        {
            parent = null;
        }
        /// <summary>
        /// 创建缓存快照
        /// </summary>
        /// <returns></returns>
        internal abstract Snapshot.Node CreateSnapshot();

        /// <summary>
        /// 节点构造函数参数类型集合
        /// </summary>
        internal static readonly Type[] NodeConstructorParameterTypes = new Type[] { typeof(Node), typeof(OperationParameter.NodeParser).MakeByRefType() };
    }
}
