using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;ushort,UShort;short,Short;byte,Byte;sbyte,SByte;char,Char*/

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
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static object calculateULong(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (ulong)left + (ulong)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (ulong)left + (ulong)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (ulong)left - (ulong)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (ulong)left - (ulong)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (ulong)left * (ulong)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (ulong)left * (ulong)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (ulong)left / (ulong)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (ulong)left % (ulong)right;
                case System.Linq.Expressions.ExpressionType.Or: return (ulong)left | (ulong)right;
                case System.Linq.Expressions.ExpressionType.And: return (ulong)left & (ulong)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (ulong)left ^ (ulong)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (ulong)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (ulong)left >> (int)right;
                default: return null;
            }
        }
    }
}
