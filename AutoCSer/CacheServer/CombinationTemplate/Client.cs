using System;
/*Type:ulong,GetULong;long,GetLong;uint,GetUInt;int,GetInt;ushort,GetUShort;short,GetShort;byte,GetByte;sbyte,GetSByte;double,GetDouble;float,GetFloat;decimal,GetDecimal;bool,GetBool*/

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public sealed partial class Client
    {
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnValue</*Type[0]*/ulong/*Type[0]*/> /*Type[1]*/GetULong/*Type[1]*/(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            return value.Value.Parameter./*Type[1]*/GetULong/*Type[1]*/(value.Type);
        }
    }
}
