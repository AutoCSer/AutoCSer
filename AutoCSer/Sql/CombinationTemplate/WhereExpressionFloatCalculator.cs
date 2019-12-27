using System;
/*Type:double,calculateDouble;float,calculateFloat;decimal,calculateDecimal*/

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
        private static object /*Type[1]*/calculateDouble/*Type[1]*/(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (/*Type[0]*/double/*Type[0]*/)left + (/*Type[0]*/double/*Type[0]*/)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (/*Type[0]*/double/*Type[0]*/)left + (/*Type[0]*/double/*Type[0]*/)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (/*Type[0]*/double/*Type[0]*/)left - (/*Type[0]*/double/*Type[0]*/)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (/*Type[0]*/double/*Type[0]*/)left - (/*Type[0]*/double/*Type[0]*/)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (/*Type[0]*/double/*Type[0]*/)left * (/*Type[0]*/double/*Type[0]*/)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (/*Type[0]*/double/*Type[0]*/)left * (/*Type[0]*/double/*Type[0]*/)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (/*Type[0]*/double/*Type[0]*/)left / (/*Type[0]*/double/*Type[0]*/)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (/*Type[0]*/double/*Type[0]*/)left % (/*Type[0]*/double/*Type[0]*/)right;
                default: return null;
            }
        }
    }
}
