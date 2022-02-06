using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;ushort,UShort;short,Short;byte,Byte;sbyte,SByte;double,Double;float,Float;decimal,Decimal;DateTime,DateTime*/

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
        private static LogicType compareULong(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((ulong)left) >= ((ulong)right) ? LogicType.True : LogicType.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((ulong)left) > ((ulong)right) ? LogicType.True : LogicType.False;
                default: return LogicType.Unknown;
            }
        }
    }
}
