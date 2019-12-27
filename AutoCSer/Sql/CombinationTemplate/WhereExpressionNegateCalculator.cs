using System;
/*Type:long,calculateLong;uint,calculateUInt;int,calculateInt;ushort,calculateUShort;short,calculateShort;byte,calculateByte;sbyte,calculateSByte;char,calculateChar;double,calculateDouble;float,calculateFloat;decimal,calculateDecimal*/

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
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object /*Type[1]*/calculateLong/*Type[1]*/(System.Linq.Expressions.ExpressionType type, object value)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(/*Type[0]*/long/*Type[0]*/)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(/*Type[0]*/long/*Type[0]*/)value; }
            }
            return null;
        }
    }
}
