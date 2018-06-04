using System;

namespace AutoCSer.CacheServer.Cache
{
    /// <summary>
    /// 缓存节点构造函数
    /// </summary>
    /// <typeparam name="nodeType"></typeparam>
    /// <param name="parser"></param>
    /// <returns></returns>
    internal delegate nodeType Constructor<nodeType>(ref OperationParameter.NodeParser parser);
}
