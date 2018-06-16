using System;

namespace AutoCSer.CacheServer.Cache
{
    /// <summary>
    /// 节点信息
    /// </summary>
    internal abstract class NodeInfo
    {
        ///// <summary>
        ///// 是否支持构造参数
        ///// </summary>
        //internal bool IsConstructorParameter;
        /// <summary>
        /// 是否需要处理删除事件
        /// </summary>
        internal bool IsOnRemovedEvent;
        /// <summary>
        /// 是否需要缓存文件持久化支持
        /// </summary>
        internal bool IsCacheFile;
        /// <summary>
        /// 调用构造函数
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal abstract Node CallConstructor(ref OperationParameter.NodeParser parser);
    }
    /// <summary>
    /// 节点信息
    /// </summary>
    /// <typeparam name="nodeType"></typeparam>
    internal sealed class NodeInfo<nodeType> : NodeInfo where nodeType: Node
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        internal Constructor<nodeType> Constructor;
        /// <summary>
        /// 调用构造函数
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Node CallConstructor(ref OperationParameter.NodeParser parser)
        {
            return Constructor(null, ref parser);
        }
    }
}
