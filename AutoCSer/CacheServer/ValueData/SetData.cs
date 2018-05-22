using System;

namespace AutoCSer.CacheServer.ValueData
{
    /// <summary>
    /// 设置参数数据委托
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="value"></param>
    internal delegate void SetData<valueType>(ref Data parameter, valueType value);
}
