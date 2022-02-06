using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;ushort,UShort;short,Short;byte,Byte;sbyte,SByte;double,Double;float,Float;decimal,Decimal;bool,Bool*/

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
        internal static ReturnValue<ulong> GetULong(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            return value.Value.Parameter.GetULong(value.Type);
        }
    }
}
