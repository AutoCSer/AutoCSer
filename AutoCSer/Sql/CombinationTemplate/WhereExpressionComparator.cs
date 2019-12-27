using System;
/*Type:ulong,compareULong;long,compareLong;uint,compareUInt;int,compareInt;ushort,compareUShort;short,compareShort;byte,compareByte;sbyte,compareSByte;double,compareDouble;float,compareFloat;decimal,compareDecimal;DateTime,compareDateTime*/

namespace AutoCSer.Sql
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct WhereExpression
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static LogicType /*Type[1]*/compareULong/*Type[1]*/(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((/*Type[0]*/ulong/*Type[0]*/)left) >= ((/*Type[0]*/ulong/*Type[0]*/)right) ? LogicType.True : LogicType.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((/*Type[0]*/ulong/*Type[0]*/)left) > ((/*Type[0]*/ulong/*Type[0]*/)right) ? LogicType.True : LogicType.False;
                default: return LogicType.Unknown;
            }
        }
    }
}
