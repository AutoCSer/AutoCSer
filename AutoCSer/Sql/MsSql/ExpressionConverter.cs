using System;
using System.Linq.Expressions;
using AutoCSer.Extension;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.MsSql
{
    /// <summary>
    /// 表达式转换
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ExpressionConverter
    {
        /// <summary>
        /// SQL 字符串流
        /// </summary>
        internal CharStream SqlStream;
        /// <summary>
        /// 常量转换
        /// </summary>
        internal ConstantConverter ConstantConverter;
        /// <summary>
        /// 第一个参数成员名称
        /// </summary>
        internal string FirstMemberName;
        /// <summary>
        /// 第一个参数成员SQL名称
        /// </summary>
        internal string FirstMemberSqlName;
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression"></param>
        internal void Convert(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.OrElse: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, "or"); return;
                case ExpressionType.AndAlso: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, "and"); return;
                case ExpressionType.Equal: convertEqual(new UnionType { Value = expression }.BinaryExpression); return;
                case ExpressionType.NotEqual: convertNotEqual(new UnionType { Value = expression }.BinaryExpression); return;
                case ExpressionType.GreaterThanOrEqual: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, '>', '='); return;
                case ExpressionType.GreaterThan: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, '>'); return;
                case ExpressionType.LessThan: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, '<'); return;
                case ExpressionType.LessThanOrEqual: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, '<', '='); return;
                case ExpressionType.Add:
                case ExpressionType.AddChecked: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, '+'); return;
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, '-'); return;
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, '*'); return;
                case ExpressionType.Divide: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, '/'); return;
                case ExpressionType.Modulo: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, '%'); return;
                case ExpressionType.Or: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, '|'); return;
                case ExpressionType.And: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, '&'); return;
                case ExpressionType.ExclusiveOr: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, '^'); return;
                case ExpressionType.LeftShift: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, '<', '<'); return;
                case ExpressionType.RightShift: convertBinaryExpression(new UnionType { Value = expression }.BinaryExpression, '>', '>'); return;
                case ExpressionType.MemberAccess: convertMemberAccess(new UnionType { Value = expression }.MemberExpression); return;
                case ExpressionType.Not: convertNot(new UnionType { Value = expression }.UnaryExpression); return;
                case ExpressionType.Unbox: convertIsSimple(new UnionType { Value = expression }.UnaryExpression.Operand); return;
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked: convertNegate(new UnionType { Value = expression }.UnaryExpression); return;
                case ExpressionType.UnaryPlus: convertUnaryPlus(new UnionType { Value = expression }.UnaryExpression); return;
                case ExpressionType.IsTrue: convertIsTrue(new UnionType { Value = expression }.UnaryExpression); return;
                case ExpressionType.IsFalse: convertIsFalse(new UnionType { Value = expression }.UnaryExpression); return;
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked: convertConvert(new UnionType { Value = expression }.UnaryExpression); return;
                case ExpressionType.Conditional: convertConditional(new UnionType { Value = expression }.ConditionalExpression); return;
                case ExpressionType.Call: convertCall(new UnionType { Value = expression }.MethodCallExpression); return;

                case ExpressionType.Constant: convertConstant(new UnionType { Value = expression }.ConstantExpression); return;
                default: throw new InvalidCastException(expression.NodeType.ToString());
            }
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void convertEqual(BinaryExpression expression)
        {
            if (expression.Left.IsConstantNull())
            {
                convertIsSimple(expression.Right);
                SqlStream.SimpleWriteNotNull(" is null");
            }
            else if (expression.Right.IsConstantNull())
            {
                convertIsSimple(expression.Left);
                SqlStream.SimpleWriteNotNull(" is null");
            }
            else convertBinaryExpression(expression, '=');
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertNotEqual(BinaryExpression expression)
        {
            if (expression.Left.IsConstantNull())
            {
                convertIsSimple(expression.Right);
                SqlStream.SimpleWriteNotNull(" is not null");
            }
            else if (expression.Right.IsConstantNull())
            {
                convertIsSimple(expression.Left);
                SqlStream.SimpleWriteNotNull(" is not null");
            }
            else convertBinaryExpression(expression, '<', '>');
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void convertIsSimple(Expression expression)
        {
            if (expression.IsSimple()) Convert(expression);
            else
            {
                SqlStream.Write('(');
                Convert(expression);
                SqlStream.Write(')');
            }
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="binaryExpression">表达式</param>
        /// <param name="char1">操作字符1</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void convertBinaryExpression(BinaryExpression binaryExpression, char char1)
        {
            convertIsSimple(binaryExpression.Left);
            SqlStream.Write(char1);
            convertIsSimple(binaryExpression.Right);
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="binaryExpression">表达式</param>
        /// <param name="char1">操作字符1</param>
        /// <param name="char2">操作字符2</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void convertBinaryExpression(BinaryExpression binaryExpression, char char1, char char2)
        {
            convertIsSimple(binaryExpression.Left);
            SqlStream.Write(char1);
            SqlStream.Write(char2);
            convertIsSimple(binaryExpression.Right);
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="binaryExpression">表达式</param>
        /// <param name="type">操作字符串</param>
        private void convertBinaryExpression(BinaryExpression binaryExpression, string type)
        {
            Expression left = binaryExpression.Left, right = binaryExpression.Right;
            SqlStream.Write('(');
            if (left.IsSimpleNotLogic())
            {
                Convert(left);
                SqlStream.Write('=');
                SqlStream.Write('1');
            }
            else Convert(left);
            SqlStream.Write(')');
            SqlStream.SimpleWriteNotNull(type);
            SqlStream.Write('(');
            if (right.IsSimpleNotLogic())
            {
                Convert(right);
                SqlStream.Write('=');
                SqlStream.Write('1');
            }
            else Convert(right);
            SqlStream.Write(')');
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression"></param>
        private void convertMemberAccess(MemberExpression expression)
        {
            if (expression.Expression != null && typeof(ParameterExpression).IsAssignableFrom(expression.Expression.GetType()))
            {
                string name = expression.Member.Name, sqlName = ConstantConverter.ConvertName(name);
                if (FirstMemberName == null)
                {
                    FirstMemberName = name;
                    FirstMemberSqlName = sqlName;
                }
                SqlStream.SimpleWriteNotNull(sqlName);
                return;
            }
            object value = null;
            if (expression.TryGetConstant(ref value))
            {
                convertConstant(value);
                return;
            }
            throw new InvalidCastException("未知成员表达式类型 " + expression.Expression.GetType().Name);
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="unaryExpression">表达式</param>
        private void convertNot(UnaryExpression unaryExpression)
        {
            Expression expression = unaryExpression.Operand;
            if (expression.IsSimple())
            {
                Convert(expression);
                SqlStream.SimpleWriteNotNull("=0");
            }
            else
            {
                SqlStream.Write('(');
                Convert(expression);
                SqlStream.SimpleWriteNotNull(")=0");
            }
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="unaryExpression">表达式</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void convertNegate(UnaryExpression unaryExpression)
        {
            SqlStream.SimpleWriteNotNull("-(");
            Convert(unaryExpression.Operand);
            SqlStream.Write(')');
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="unaryExpression">表达式</param>
        private void convertUnaryPlus(UnaryExpression unaryExpression)
        {
            SqlStream.SimpleWriteNotNull("+(");
            Convert(unaryExpression.Operand);
            SqlStream.Write(')');
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="unaryExpression">表达式</param>
        private void convertIsTrue(UnaryExpression unaryExpression)
        {
            Expression expression = unaryExpression.Operand;
            if (expression.IsSimple())
            {
                Convert(expression);
                SqlStream.SimpleWriteNotNull("=1");
            }
            else
            {
                SqlStream.Write('(');
                Convert(expression);
                SqlStream.SimpleWriteNotNull(")=1");
            }
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="unaryExpression">表达式</param>
        private void convertIsFalse(UnaryExpression unaryExpression)
        {
            Expression expression = unaryExpression.Operand;
            if (expression.IsSimple())
            {
                Convert(expression);
                SqlStream.SimpleWriteNotNull("=0");
            }
            else
            {
                SqlStream.Write('(');
                Convert(expression);
                SqlStream.SimpleWriteNotNull(")=0");
            }
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertConvert(UnaryExpression expression)
        {
            object value = null;
            if (expression.TryGetConstantConvert(ref value)) convertConstant(value);
            else
            {
                SqlStream.SimpleWriteNotNull("cast(");
                Convert(expression.Operand);
                SqlStream.SimpleWriteNotNull(" as ");
                SqlStream.SimpleWriteNotNull(expression.Type.formCSharpType().ToString());
                SqlStream.Write(')');
            }
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertConditional(ConditionalExpression expression)
        {
            Expression test = expression.Test, ifTrue = expression.IfTrue, ifFalse = expression.IfFalse;
            SqlStream.SimpleWriteNotNull("case when ");
            if (test.IsSimpleNotLogic())
            {
                Convert(test);
                SqlStream.Write('=');
                SqlStream.Write('1');
            }
            else Convert(test);
            SqlStream.SimpleWriteNotNull(" then ");
            convertIsSimple(ifTrue);
            SqlStream.SimpleWriteNotNull(" else ");
            convertIsSimple(ifFalse);
            SqlStream.SimpleWriteNotNull(" end");
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void convertConstant(ConstantExpression expression)
        {
            convertConstant(expression.Value);
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="value"></param>
        private void convertConstant(object value)
        {
            if (value != null)
            {
                Action<CharStream, object> toString = ConstantConverter[value.GetType()];
                if (toString != null) toString(SqlStream, value);
                else ConstantConverter.Convert(SqlStream, value.ToString());
            }
            else SqlStream.WriteJsonNull();
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertCall(MethodCallExpression expression)
        {
            MethodInfo method = expression.Method;
            if (method.ReflectedType == typeof(ExpressionCall))
            {
                switch (method.Name)
                {
                    case "In": convertCall(expression, true); break;
                    case "NotIn": convertCall(expression, false); break;
                    case "Like":
                        System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments = expression.Arguments;
                        convertIsSimple(arguments[0]);
                        SqlStream.SimpleWriteNotNull(" like ");
                        convertIsSimple(expression.Object);
                        ConstantConverter.ConvertLike(SqlStream, arguments[1].GetConstant(), true, true);
                        break;
                    default:
                        SqlStream.SimpleWriteNotNull(method.Name);
                        SqlStream.Write('(');
                        if (expression.Arguments != null)
                        {
                            bool isNext = false;
                            foreach (Expression argumentExpression in expression.Arguments)
                            {
                                if (isNext) SqlStream.Write(',');
                                Convert(argumentExpression);
                                isNext = true;
                            }
                        }
                        SqlStream.Write(')');
                        break;
                }
            }
            else if (IsStringContains(method))
            {
                convertIsSimple(expression.Object);
                SqlStream.SimpleWriteNotNull(" like ");
                ConstantConverter.ConvertLike(SqlStream, expression.Arguments[0].GetConstant(), true, true);
            }
            else convertConstant(expression.GetConstant());
        }
        /// <summary>
        /// 判断是否 string.Contains
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal static bool IsStringContains(System.Reflection.MethodInfo method)
        {
            if (method.ReflectedType == typeof(string) && method.Name == "Contains" && method.ReturnType == typeof(bool)
                && !method.IsGenericMethod)
            {
                ParameterInfo[] Parameters = method.GetParameters();
                if (Parameters.Length == 1 && Parameters[0].ParameterType == typeof(string))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="isIn"></param>
        private void convertCall(MethodCallExpression expression, bool isIn)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<Expression> arguments = expression.Arguments;
            System.Collections.IEnumerable values = (System.Collections.IEnumerable)arguments[1].GetConstant();
            if (values != null)
            {
                LeftArray<object> array = new LeftArray<object>();
                foreach (object value in values) array.Add(value);
                switch (array.Length)
                {
                    case 0: break;
                    case 1:
                        Expression leftExpression = arguments[0];
                        if (array[0] == null)
                        {
                            convertIsSimple(leftExpression);
                            SqlStream.SimpleWriteNotNull(isIn ? " is null" : " is not null");
                        }
                        else
                        {
                            convertIsSimple(leftExpression);
                            if (isIn) SqlStream.Write('=');
                            else
                            {
                                SqlStream.Write('<');
                                SqlStream.Write('>');
                            }
                            convertConstant(array[0]);
                        }
                        return;
                    default:
                        Convert(arguments[0]);
                        SqlStream.SimpleWriteNotNull(isIn ? " In(" : " Not In(");
                        Action<CharStream, object> toString = ConstantConverter[array[0].GetType()];
                        int index = 0;
                        if (toString == null)
                        {
                            foreach (object value in array)
                            {
                                if (index == 0) index = 1;
                                else SqlStream.Write(',');
                                ConstantConverter.Convert(SqlStream, value.ToString());
                            }
                        }
                        else
                        {
                            foreach (object value in array)
                            {
                                if (index == 0) index = 1;
                                else SqlStream.Write(',');
                                toString(SqlStream, value);
                            }
                        }
                        SqlStream.Write(')');
                        return;
                }
            }
            SqlStream.SimpleWriteNotNull(isIn ? "(1=0)" : "(1=1)");
        }
    }
}
