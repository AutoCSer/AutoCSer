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
                    else if (AutoCSer.Sql.MsSql.ExpressionConverter.IsStringContains(method)) return false;
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 是否常量 null
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        internal static bool IsConstantNull(this Expression expression)
        {
            object value = null;
            return tryGetConstant(expression, ref value) && value == null;
        }
        /// <summary>
        /// 获取常量
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool tryGetConstant(this Expression expression, ref object value)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Constant: value = new Sql.UnionType { Value = expression }.ConstantExpression.Value; return true;
                case ExpressionType.MemberAccess: return TryGetConstant(new Sql.UnionType { Value = expression }.MemberExpression, ref value);
                case ExpressionType.Call: value = new Sql.UnionType { Value = expression }.MethodCallExpression.GetConstant(); return true;
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    return TryGetConstantConvert(new Sql.UnionType { Value = expression }.UnaryExpression, ref value);
            }
            return false;
        }
        /// <summary>
        /// 获取常量
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool TryGetConstant(this MemberExpression expression, ref object value)
        {
            object constantValue = null;
            if (expression.Expression == null || tryGetConstant(expression.Expression, ref constantValue))
            {
                FieldInfo fieldInfo = expression.Member as FieldInfo;
                if (fieldInfo != null)
                {
                    value = fieldInfo.GetValue(constantValue);
                    return true;
                }
                PropertyInfo propertyInfo = expression.Member as PropertyInfo;
                if (propertyInfo != null)
                {
                    value = propertyInfo.GetValue(constantValue, null);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取常量
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static object GetConstant(this MethodCallExpression expression)
        {
            Expression instance = expression.Object;
            return expression.Method.Invoke(instance == null ? null : instance.GetConstant(), expression.Arguments.getArray(argumentExpression => argumentExpression.GetConstant()));
        }
        /// <summary>
        /// 获取常量
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool TryGetConstantConvert(this UnaryExpression expression, ref object value)
        {
            if (expression.Operand.tryGetConstant(ref value))
            {
                Type type = value.GetType();
                if (type == expression.Type) return true;
                if (type.IsEnum)
                {
                    if (System.Enum.GetUnderlyingType(type) == expression.Type)
                    {
                        type = expression.Type;
                        if (type == typeof(int)) value = (int)value;
                        else if (type == typeof(long)) value = (long)value;
                        else if (type == typeof(uint)) value = (uint)value;
                        else if (type == typeof(ulong)) value = (ulong)value;
                        else if (type == typeof(byte)) value = (byte)value;
                        else if (type == typeof(sbyte)) value = (sbyte)value;
                        else if (type == typeof(ushort)) value = (ushort)value;
                        else if (type == typeof(short)) value = (short)value;
                        return true;
                    }
                }
                else
                {
                    MethodInfo method = AutoCSer.Emit.CastType.GetMethod(type, expression.Type);
                    if (method != null)
                    {
                        value = method.Invoke(null, new object[] { value });
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取常量
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static object GetConstant(this Expression expression)
        {
            object value = null;
            return tryGetConstant(expression, ref value) ? value : null;
        }
    }
}
