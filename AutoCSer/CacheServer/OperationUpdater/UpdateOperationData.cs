using System;

namespace AutoCSer.CacheServer.OperationUpdater
{
    /// <summary>
    /// 修改操作数据包
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    /// <param name="parser"></param>
    /// <param name="value"></param>
    internal delegate void UpdateOperationData<valueType>(ref OperationParameter.NodeParser parser, valueType value);
}
