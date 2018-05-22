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
        /// 构造函数字段名称
        /// </summary>
        internal const string ConstructorFieldName = "constructor";

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
            else parser.ReturnParameter.Type = ReturnType.ValueDataLoadError;
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
            else parser.ReturnParameter.Type = ReturnType.ValueDataLoadError;
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
        /// 创建缓存快照
        /// </summary>
        /// <returns></returns>
        internal abstract Snapshot.Node CreateSnapshot();
    }
}
