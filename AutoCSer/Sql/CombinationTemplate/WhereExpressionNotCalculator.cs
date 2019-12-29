using System;
/*Type:ulong,calculateNotULong;long,calculateNotLong;uint,calculateNotUInt;int,calculateNotInt;ushort,calculateNotUShort;short,calculateNotShort;byte,calculateNotByte;sbyte,calculateNotSByte*/

namespace AutoCSer.Sql
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct WhereExpression
    {
        /// <summary>
        /// 计算器
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object /*Type[1]*/calculateNotULong/*Type[1]*/(object value)
        {
            return ~(/*Type[0]*/ulong/*Type[0]*/)value;
        }
    }
}
