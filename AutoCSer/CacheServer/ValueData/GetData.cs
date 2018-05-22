using System;

namespace AutoCSer.CacheServer.ValueData
{
    /// <summary>
    /// 获取参数数据委托
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    /// <param name="parameter"></param>
    /// <returns></returns>
    internal delegate valueType GetData<valueType>(ref Data parameter);
}
