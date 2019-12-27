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
        /// 条件表达式重组
        /// </summary>
        private WhereExpression whereExpression;
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

                case ExpressionType.Constant: convertConstant(expression.GetConstantValue()); return;
                default: throw new InvalidCastException(expression.NodeType.ToString());
            }
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertEqual(BinaryExpression expression)
        {
            whereExpression.Convert(expression.Left);
            if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
            {
                if (whereExpression.IsConstantNull)
                {
                    convertIsSimple(expression.Right);
                    SqlStream.SimpleWriteNotNull(" is null");
                    return;
                }
                WhereExpression left = whereExpression;
                whereExpression.Convert(expression.Right);
                if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
                {
                    if (whereExpression.IsConstantNull)
                    {
                        convertIsSimple(expression.Left);
                        SqlStream.SimpleWriteNotNull(" is null");
                    }
                    else
                    {
                        convertIsSimple(left.NormalExpression ?? expression.Left);
                        SqlStream.Write('=');
                        convertIsSimple(whereExpression.NormalExpression ?? expression.Right);
                    }
                }
                else throw new InvalidCastException("未知 == 表达式右值 " + whereExpression.UnknownExpression.ToString());
            }
            else throw new InvalidCastException("未知 == 表达式左值 " + whereExpression.UnknownExpression.ToString());
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertNotEqual(BinaryExpression expression)
        {
            whereExpression.Convert(expression.Left);
            if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
            {
                if (whereExpression.IsConstantNull)
                {
                    convertIsSimple(expression.Right);
                    SqlStream.SimpleWriteNotNull(" is not null");
                    return;
                }
                WhereExpression left = whereExpression;
                whereExpression.Convert(expression.Right);
                if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
                {
                    if (whereExpression.IsConstantNull)
                    {
                        convertIsSimple(expression.Left);
                        SqlStream.SimpleWriteNotNull(" is not null");
                    }
                    else
                    {
                        convertIsSimple(left.NormalExpression ?? expression.Left);
                        SqlStream.Write('<');
                        SqlStream.Write('>');
                        convertIsSimple(whereExpression.NormalExpression ?? expression.Right);
                    }
                }
                else throw new InvalidCastException("未知 != 表达式右值 " + whereExpression.UnknownExpression.ToString());
            }
            else throw new InvalidCastException("未知 != 表达式左值 " + whereExpression.UnknownExpression.ToString());
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
            whereExpression.ConvertMemberAccess(expression);
            if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
            {
                if (whereExpression.Expression.NodeType == ExpressionType.Constant) convertConstant(whereExpression.Expression.GetConstantValue());
                else throw new InvalidCastException("未知成员表达式类型 " + expression.Member.Name + " " + whereExpression.Expression.NodeType.ToString());
            }
            else throw new InvalidCastException("未知成员表达式 " + expression.Member.Name + " " + whereExpression.Type.ToString());
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
            whereExpression.ConvertConvert(expression);
            if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
            {
                if (whereExpression.Expression.NodeType == ExpressionType.Constant) convertConstant(whereExpression.Expression.GetConstantValue());
                else
                {
                    if (expression.Type == typeof(int))
                    {
                        Type operandType = expression.Operand.Type;
                        if (operandType == typeof(byte) || operandType == typeof(sbyte) || operandType == typeof(short) || operandType == typeof(ushort))
                        {
                            Convert(expression.Operand);
                            return;
                        }
                    }
                    SqlStream.SimpleWriteNotNull("cast(");
                    Convert(expression.Operand);
                    SqlStream.SimpleWriteNotNull(" as ");
                    SqlStream.SimpleWriteNotNull(expression.Type.formCSharpType().ToString());
                    SqlStream.Write(')');
                }
            }
            else throw new InvalidCastException("未知转换表达式 " + whereExpression.UnknownExpression.ToString());
        }
        /// <summary>
        /// 枚举转换整数
        /// </summary>
        internal static object ConvertEnum(object value, System.Type convertType)
        {
            if (convertType == typeof(int)) return (int)value;
            if (convertType == typeof(long)) return (long)value;
            if (convertType == typeof(uint)) return (uint)value;
            if (convertType == typeof(ulong)) return (ulong)value;
            if (convertType == typeof(byte)) return (byte)value;
            if (convertType == typeof(sbyte)) return (sbyte)value;
            if (convertType == typeof(ushort)) return (ushort)value;
            if (convertType == typeof(short)) return (short)value;
            return null;
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
                        {
                            System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments = expression.Arguments;
                            whereExpression.Convert(arguments[1]);
                            if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
                            {
                                if (whereExpression.Expression.NodeType == ExpressionType.Constant)
                                {
                                    WhereExpression like = whereExpression;
                                    convertIsSimple(arguments[0]);
                                    SqlStream.SimpleWriteNotNull(" like ");
                                    ConstantConverter.ConvertLike(SqlStream, like.Expression.GetConstantValue(), true, true);
                                }
                                else throw new InvalidCastException("未知 Like 函数表达式值类型 " + whereExpression.Expression.NodeType.ToString());
                            }
                            else throw new InvalidCastException("未知 Like 函数表达式值 " + whereExpression.UnknownExpression.ToString());
                        }
                        break;
                    case "Count":
                        SqlStream.SimpleWriteNotNull("count(");
                        Convert(expression.Arguments[0]);
                        SqlStream.Write(')');
                        break;
                    case "Sum":
                        SqlStream.SimpleWriteNotNull("sum(");
                        Convert(expression.Arguments[0]);
                        SqlStream.Write(')');
                        break;
                    case "GetDate": SqlStream.SimpleWriteNotNull("getdate()"); break;
                    case "SysDateTime": SqlStream.SimpleWriteNotNull("sysdatetime()"); break;
                    case "IsNull":
                        {
                            System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments = expression.Arguments;
                            SqlStream.SimpleWriteNotNull("isnull(");
                            convertIsSimple(arguments[0]);
                            SqlStream.Write(',');
                            whereExpression.Convert(arguments[1]);
                            if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
                            {
                                if (whereExpression.Expression.NodeType == ExpressionType.Constant) convertConstant(whereExpression.Expression.GetConstantValue());
                                else convertIsSimple(whereExpression.Expression);
                            }
                            else throw new InvalidCastException("未知 IsNull 函数表达式值类型 " + whereExpression.UnknownExpression.ToString());
                            SqlStream.Write(')');
                        }
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
            //else if (IsStringContains(method))
            //{
            //    convertIsSimple(expression.Object);
            //    SqlStream.SimpleWriteNotNull(" like ");
            //    ConstantConverter.ConvertLike(SqlStream, expression.Arguments[0].GetConstant(), true, true);
            //}
            else
            {
                whereExpression.ConvertCall(expression, method);
                if (whereExpression.Type != WhereExpression.ConvertType.Unknown && whereExpression.Expression.NodeType == ExpressionType.Constant)
                {
                    convertConstant(whereExpression.Expression.GetConstantValue());
                }
                else SqlStream.WriteJsonNull();
            }
        }
        ///// <summary>
        ///// 判断是否 string.Contains
        ///// </summary>
        ///// <param name="method"></param>
        ///// <returns></returns>
        //internal static bool IsStringContains(System.Reflection.MethodInfo method)
        //{
        //    if (method.ReflectedType == typeof(string) && method.Name == "Contains" && method.ReturnType == typeof(bool)
        //        && !method.IsGenericMethod)
        //    {
        //        ParameterInfo[] Parameters = method.GetParameters();
        //        if (Parameters.Length == 1 && Parameters[0].ParameterType == typeof(string))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="isIn"></param>
        private void convertCall(MethodCallExpression expression, bool isIn)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<Expression> arguments = expression.Arguments;
            whereExpression.Convert(arguments[1]);
            if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
            {
                if (whereExpression.Expression.NodeType == ExpressionType.Constant)
                {
                    System.Collections.IEnumerable values = (System.Collections.IEnumerable)whereExpression.Expression.GetConstantValue();
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
                    return;
                }
                throw new InvalidCastException("未知函数表达式参数值类型 " + expression.Method.Name + " " + whereExpression.Expression.NodeType.ToString());
            }
            throw new InvalidCastException("未知函数表达式参数值 " + expression.Method.Name + " " + whereExpression.UnknownExpression.ToString());
        }
    }
}
