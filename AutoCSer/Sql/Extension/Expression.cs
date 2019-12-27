using System;
using System.Linq.Expressions;
using System.Reflection;
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
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.Call:
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 是否非内置逻辑值简单表达式（不需要括号）
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        internal static bool IsSimpleNotLogic(this Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                case ExpressionType.MemberAccess:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    return true;
                case ExpressionType.Call:
                    MethodCallExpression methodCallExpression = new Sql.UnionType { Value = expression }.MethodCallExpression;
                    System.Reflection.MethodInfo method = methodCallExpression.Method;
                    if (method.ReflectedType == typeof(Sql.ExpressionCall))
                    {
                        switch (method.Name)
                        {
                            case "In":
                            case "NotIn":
                            case "Like":
                                return false;
                        }
                    }
                    //else if (AutoCSer.Sql.MsSql.ExpressionConverter.IsStringContains(method)) return false;
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 获取常量表达式常量值
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static object GetConstantValue(this Expression expression)
        {
            return new AutoCSer.Sql.UnionType { Value = expression }.ConstantExpression.Value;
        }
    }
}
