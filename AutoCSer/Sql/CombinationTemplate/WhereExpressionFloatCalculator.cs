using System;
/*double,Double;float,Float;decimal,Decimal*/

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
        private static object calculateDouble(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (double)left + (double)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (double)left + (double)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (double)left - (double)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (double)left - (double)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (double)left * (double)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (double)left * (double)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (double)left / (double)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (double)left % (double)right;
                default: return null;
            }
        }
    }
}
