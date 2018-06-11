using System;

namespace AutoCSer.CacheServer.OperationUpdater
{
    /// <summary>
    /// 修改数据委托
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <param name="updateValue"></param>
    /// <returns></returns>
    internal delegate ReturnType UpdateData<valueType>(OperationType type, ref valueType value, valueType updateValue);
}
