using System;
/*long,Long;uint,UInt;int,Int;ushort,UShort;short,Short;byte,Byte;sbyte,SByte;char,Char;double,Double;float,Float;decimal,Decimal*/

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
        private static object calculateLong(System.Linq.Expressions.ExpressionType type, object value)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(long)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(long)value; }
            }
            return null;
        }
    }
}
