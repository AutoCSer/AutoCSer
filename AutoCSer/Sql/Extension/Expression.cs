using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 表达式扩展
    /// </summary>
    internal static class ExpressionExtension
    {
        /// <summary>
        /// 是否简单表达式（不需要括号）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool IsSimple(this Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                case ExpressionType.MemberAccess:
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 是否常量 null
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool IsConstantNull(this Expression expression)
        {
            return expression.NodeType == ExpressionType.Constant && new Sql.UnionType { Value = expression }.ConstantExpression.Value == null;
        }
        
    }
}
