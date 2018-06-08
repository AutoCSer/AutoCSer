using System;

namespace AutoCSer.CacheServer.Cache
{
    /// <summary>
    /// 缓存节点构造函数
    /// </summary>
    /// <typeparam name="nodeType"></typeparam>
    /// <param name="parent"></param>
    /// <param name="parser"></param>
    /// <returns></returns>
    internal delegate nodeType Constructor<nodeType>(Node parent, ref OperationParameter.NodeParser parser);
}
