using System;
using System.Linq.Expressions;
using AutoCSer.Extensions;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;

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
                case ExpressionType.OrElse: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, "or"); return;
                case ExpressionType.AndAlso: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, "and"); return;
                case ExpressionType.Not: convertNot(new UnionType.UnaryExpression { Object = expression }.Value); return;
                case ExpressionType.Equal: convertEqual(new UnionType.BinaryExpression { Object = expression }.Value); return;
                case ExpressionType.NotEqual: convertNotEqual(new UnionType.BinaryExpression { Object = expression }.Value); return;
                case ExpressionType.GreaterThanOrEqual: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, '>', '='); return;
                case ExpressionType.GreaterThan: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, '>'); return;
                case ExpressionType.LessThan: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, '<'); return;
                case ExpressionType.LessThanOrEqual: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, '<', '='); return;
                case ExpressionType.Add:
                case ExpressionType.AddChecked: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, '+'); return;
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, '-'); return;
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, '*'); return;
                case ExpressionType.Divide: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, '/'); return;
                case ExpressionType.Modulo: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, '%'); return;
                case ExpressionType.Or: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, '|'); return;
                case ExpressionType.And: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, '&'); return;
                case ExpressionType.ExclusiveOr: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, '^'); return;
                case ExpressionType.LeftShift: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, '<', '<'); return;
                case ExpressionType.RightShift: convertBinaryExpression(new UnionType.BinaryExpression { Object = expression }.Value, '>', '>'); return;
                case ExpressionType.MemberAccess: convertMemberAccess(new UnionType.MemberExpression { Object = expression }.Value); return;
                case ExpressionType.Coalesce: convertCoalesce(new UnionType.BinaryExpression { Object = expression }.Value); return;
                case ExpressionType.Unbox:
                case ExpressionType.UnaryPlus:
                    convertIsSimple(new UnionType.UnaryExpression { Object = expression }.Value.Operand); return;
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked: convertNegate(new UnionType.UnaryExpression { Object = expression }.Value); return;
                case ExpressionType.IsTrue: convertIsTrue(new UnionType.UnaryExpression { Object = expression }.Value); return;
                case ExpressionType.IsFalse: convertIsFalse(new UnionType.UnaryExpression { Object = expression }.Value); return;
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked: convertConvert(new UnionType.UnaryExpression { Object = expression }.Value); return;
                case ExpressionType.Conditional: convertConditional(new UnionType.ConditionalExpression { Object = expression }.Value); return;
                case ExpressionType.Call: convertCall(new UnionType.MethodCallExpression { Object = expression }.Value); return;
                case ExpressionType.Constant: convertConstant(expression.GetConstantValue()); return;
                default: throw new InvalidCastException("未知表达式类型 " + expression.NodeType.ToString());
            }
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void convert(Expression expression)
        {
            whereExpression.Convert(expression);
            if (whereExpression.Type != WhereExpression.ConvertType.Unknown) Convert(whereExpression.Expression);
            else throwWhereExpressionError();
        }
        /// <summary>
        /// 条件表达式重组异常
        /// </summary>
        private void throwWhereExpressionError()
        {
            throw new InvalidCastException("未知表达式类型 " + whereExpression.UnknownType.ToString() + "." + whereExpression.NullExpression.ToString());
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ConvertIsSimple(Expression expression)
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
        /// <param name="expression"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void convertIsSimple(Expression expression)
        {
            if (expression.IsSimple()) convert(expression);
            else
            {
                SqlStream.Write('(');
                convert(expression);
                SqlStream.Write(')');
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
                    SqlStream.SimpleWrite(" is null");
                    return;
                }
                Expression left = whereExpression.Expression;
                whereExpression.Convert(expression.Right);
                if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
                {
                    ConvertIsSimple(left);
                    if (whereExpression.IsConstantNull) SqlStream.SimpleWrite(" is null");
                    else
                    {
                        SqlStream.Write('=');
                        ConvertIsSimple(whereExpression.Expression);
                    }
                    return;
                }
            }
            throwWhereExpressionError();
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
                    SqlStream.SimpleWrite(" is not null");
                    return;
                }
                Expression left = whereExpression.Expression;
                whereExpression.Convert(expression.Right);
                if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
                {
                    ConvertIsSimple(left);
                    if (whereExpression.IsConstantNull) SqlStream.SimpleWrite(" is not null");
                    else
                    {
                        SqlStream.Write('<');
                        SqlStream.Write('>');
                        ConvertIsSimple(whereExpression.NormalExpression);
                    }
                    return;
                }
            }
            throwWhereExpressionError();
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
            whereExpression.Convert(binaryExpression.Left);
            if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
            {
                convertWhereExpressionLogic();
                whereExpression.Convert(binaryExpression.Right);
                if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
                {
                    SqlStream.SimpleWrite(type);
                    convertWhereExpressionLogic();
                    return;
                }
            }
            throwWhereExpressionError();
        }
        /// <summary>
        /// 转换逻辑表达式
        /// </summary>
        private void convertWhereExpressionLogic()
        {
            Expression expression = whereExpression.Expression;
            SqlStream.Write('(');
            Convert(expression);
            if (expression.IsSimpleNotLogic())
            {
                SqlStream.Write('=');
                SqlStream.Write('1');
            }
            SqlStream.Write(')');
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression"></param>
        private void convertMemberAccess(MemberExpression expression)
        {
            //if (expression.Expression != null && typeof(ParameterExpression).IsAssignableFrom(expression.Expression.GetType()))
            if (expression.Expression != null && expression.Expression.NodeType == ExpressionType.Parameter)
            {
                string name = expression.Member.Name, sqlName = ConstantConverter.ConvertName(name);
                if (FirstMemberName == null)
                {
                    FirstMemberName = name;
                    FirstMemberSqlName = sqlName;
                }
                SqlStream.SimpleWrite(sqlName);
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
        /// <param name="binaryExpression"></param>
        private void convertCoalesce(BinaryExpression binaryExpression)
        {
            SqlStream.SimpleWrite("isnull(");
            convert(binaryExpression.Left);
            SqlStream.Write(',');
            convert(binaryExpression.Right);
            SqlStream.Write(')');
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="unaryExpression">表达式</param>
        private void convertNot(UnaryExpression unaryExpression)
        {
            whereExpression.Convert(unaryExpression.Operand);
            if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
            {
                Expression expression = whereExpression.Expression;
                if (expression.IsSimple())
                {
                    Convert(expression);
                    SqlStream.SimpleWrite("=0");
                }
                else
                {
                    SqlStream.Write('(');
                    Convert(expression);
                    SqlStream.SimpleWrite(")=0");
                }
            }
            else throwWhereExpressionError();
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="unaryExpression">表达式</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void convertNegate(UnaryExpression unaryExpression)
        {
            SqlStream.SimpleWrite("-(");
            convert(unaryExpression.Operand);
            SqlStream.Write(')');
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="unaryExpression">表达式</param>
        private void convertIsTrue(UnaryExpression unaryExpression)
        {
            whereExpression.Convert(unaryExpression.Operand);
            if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
            {
                Expression expression = whereExpression.Expression;
                if (expression.IsSimple())
                {
                    Convert(expression);
                    SqlStream.SimpleWrite("=1");
                }
                else
                {
                    SqlStream.Write('(');
                    Convert(expression);
                    SqlStream.SimpleWrite(")=1");
                }
            }
            else throwWhereExpressionError();
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="unaryExpression">表达式</param>
        private void convertIsFalse(UnaryExpression unaryExpression)
        {
            whereExpression.Convert(unaryExpression.Operand);
            if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
            {
                Expression expression = whereExpression.Expression;
                if (expression.IsSimple())
                {
                    Convert(expression);
                    SqlStream.SimpleWrite("=0");
                }
                else
                {
                    SqlStream.Write('(');
                    Convert(expression);
                    SqlStream.SimpleWrite(")=0");
                }
            }
            else throwWhereExpressionError();
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertConvert(UnaryExpression expression)
        {
            if (expression.Type == typeof(int))
            {
                Type operandType = expression.Operand.Type;
                if (operandType == typeof(byte) || operandType == typeof(sbyte) || operandType == typeof(short) || operandType == typeof(ushort))
                {
                    convert(expression.Operand);
                    return;
                }
            }
            SqlStream.SimpleWrite("cast(");
            convert(expression.Operand);
            SqlStream.SimpleWrite(" as ");
            SqlStream.SimpleWrite(expression.Type.formCSharpType().ToString());
            SqlStream.Write(')');
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
            whereExpression.Convert(expression.Test);
            if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
            {
                Expression test = whereExpression.Expression;
                SqlStream.SimpleWrite("case when ");
                if (test.IsSimpleNotLogic())
                {
                    Convert(test);
                    SqlStream.Write('=');
                    SqlStream.Write('1');
                }
                else Convert(test);
                SqlStream.SimpleWrite(" then ");
                convert(expression.IfTrue);
                SqlStream.SimpleWrite(" else ");
                convert(expression.IfFalse);
                SqlStream.SimpleWrite(" end");
            }
            else throwWhereExpressionError();
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
                switch (method.Name.Length)
                {
                    case 2:
                        switch (method.Name)
                        {
                            case "In": convertCallIn(expression, true); break;
                        }
                        break;
                    case 3:
                        switch (method.Name)
                        {
                            case "Sum":
                                SqlStream.SimpleWrite("sum(");
                                convert(expression.Arguments[0]);
                                SqlStream.Write(')');
                                break;
                            case "Len":
                                SqlStream.SimpleWrite("len(");
                                convert(expression.Arguments[0]);
                                SqlStream.Write(')');
                                break;
                        }
                        break;
                    case 4:
                        switch (method.Name)
                        {
                            case "Like": convertCallLike(expression, false, true, true); break;
                            case "Call":
                                int parameterIndex = 0;
                                foreach (Expression argumentExpression in expression.Arguments)
                                {
                                    switch (parameterIndex)
                                    {
                                        case 0:
                                            whereExpression.Convert(argumentExpression);
                                            if (whereExpression.Type == WhereExpression.ConvertType.Unknown || whereExpression.Expression.NodeType != ExpressionType.Constant)
                                            {
                                                throw new InvalidCastException("未知 SQL 函数名称 " + whereExpression.Expression.NodeType.ToString());
                                            }
                                            string functionName = (string)whereExpression.Expression.GetConstantValue();
                                            if (string.IsNullOrEmpty(functionName)) throw new InvalidCastException("SQL 函数名称不能为空 " + whereExpression.Expression.NodeType.ToString());
                                            SqlStream.SimpleWrite(functionName);
                                            SqlStream.Write('(');
                                            break;
                                        case 1: convert(argumentExpression); break;
                                        default:
                                            SqlStream.Write(',');
                                            convert(argumentExpression);
                                            break;
                                    }
                                    ++parameterIndex;
                                }
                                SqlStream.Write(')');
                                break;
                        }
                        break;
                    case 5:
                        switch (method.Name)
                        {
                            case "NotIn": convertCallIn(expression, false); break;
                            case "Count":
                                SqlStream.SimpleWrite("count(");
                                convert(expression.Arguments[0]);
                                SqlStream.Write(')');
                                break;
                        }
                        break;
                    case 6:
                        switch (method.Name)
                        {
                            case "IsNull":
                                SqlStream.SimpleWrite("isnull(");
                                convert(expression.Arguments[0]);
                                SqlStream.Write(',');
                                convert(expression.Arguments[1]);
                                SqlStream.Write(')');
                                break;
                        }
                        break;
                    case 7:
                        switch (method.Name)
                        {
                            case "NotLike": convertCallLike(expression, true, true, true); break;
                            case "GetDate": SqlStream.SimpleWrite("getdate()"); break;
                            case "Replace": convertCallReplace(expression); break;
                        }
                        break;
                    case 8:
                        switch (method.Name)
                        {
                            case "EndsWith": convertCallLike(expression, false, true, false); break;
                            case "DateDiff": convertDateDiff(expression); break;
                        }
                        break;
                    case 10:
                        switch (method.Name)
                        {
                            case "StartsWith": convertCallLike(expression, false, false, true); break;
                        }
                        break;
                    case 11:
                        switch (method.Name)
                        {
                            case "NotEndsWith": convertCallLike(expression, true, true, false); break;
                            case "SysDateTime": SqlStream.SimpleWrite("sysdatetime()"); break;
                        }
                        break;
                    case 13:
                        switch (method.Name)
                        {
                            case "NotStartsWith": convertCallLike(expression, true, false, true); break;
                        }
                        break;
                }
            }
            //else if (IsStringContains(method))
            //{
            //    convertIsSimple(expression.Object);
            //    SqlStream.SimpleWrite(" like ");
            //    ConstantConverter.ConvertLike(SqlStream, expression.Arguments[0].GetConstant(), true, true);
            //}
            else
            {
                whereExpression.ConvertCall(expression, method);
                if (whereExpression.Type != WhereExpression.ConvertType.Unknown) convertConstant(whereExpression.Expression.GetConstantValue());
                else throwWhereExpressionError();
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
        private void convertCallIn(MethodCallExpression expression, bool isIn)
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
                        LeftArray<object> array = new LeftArray<object>(0);
                        foreach (object value in values) array.Add(value);
                        switch (array.Length)
                        {
                            case 0: break;
                            case 1:
                                convertIsSimple(arguments[0]);
                                if (array[0] == null) SqlStream.SimpleWrite(isIn ? " is null" : " is not null");
                                else
                                {
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
                                convert(arguments[0]);
                                SqlStream.SimpleWrite(isIn ? " in(" : " not in(");
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
                    SqlStream.SimpleWrite(isIn ? "(1=0)" : "(1=1)");
                    return;
                }
                throw new InvalidCastException("未知函数表达式参数值类型 " + expression.Method.Name + " " + whereExpression.Expression.NodeType.ToString());
            }
            throw new InvalidCastException("未知函数表达式参数值 " + expression.Method.Name + " " + whereExpression.UnknownType.ToString());
        }
        /// <summary>
        /// 转换 LIKE 表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="isNot"></param>
        /// <param name="isStart"></param>
        /// <param name="isEnd"></param>
        private void convertCallLike(MethodCallExpression expression, bool isNot, bool isStart, bool isEnd)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments = expression.Arguments;
            whereExpression.Convert(arguments[1]);
            if (whereExpression.Type != WhereExpression.ConvertType.Unknown)
            {
                if (whereExpression.Expression.NodeType == ExpressionType.Constant)
                {
                    Expression like = whereExpression.Expression;
                    convertIsSimple(arguments[0]);
                    SqlStream.SimpleWrite(isNot ? " not like " : " like ");
                    ConstantConverter.ConvertLike(SqlStream, like.GetConstantValue(), isStart, isEnd);
                }
                else throw new InvalidCastException("未知 Like 函数表达式值类型 " + whereExpression.Expression.NodeType.ToString());
            }
            else throw new InvalidCastException("未知 Like 函数表达式值 " + whereExpression.UnknownType.ToString());
        }
        /// <summary>
        /// 转换 REPLACE 表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertCallReplace(MethodCallExpression expression)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments = expression.Arguments;
            SqlStream.SimpleWrite("replace(");
            convert(arguments[0]);
            SqlStream.Write(',');
            convert(arguments[1]);
            SqlStream.Write(',');
            convert(arguments[2]);
            SqlStream.Write(')');
        }
        /// <summary>
        /// 转换 DATEDIFF 表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        private void convertDateDiff(MethodCallExpression expression)
        {
            System.Collections.ObjectModel.ReadOnlyCollection<System.Linq.Expressions.Expression> arguments = expression.Arguments;
            SqlStream.SimpleWrite("datediff(");

            whereExpression.Convert(arguments[0]);
            if (whereExpression.Type == WhereExpression.ConvertType.ConvertExpression)
            {
                ExpressionCallDateDiffType type = (ExpressionCallDateDiffType)whereExpression.Expression.GetConstantValue();
                if (type != ExpressionCallDateDiffType.NONE)
                {
                    SqlStream.Write(type.ToString());
                    SqlStream.Write(',');
                }
            }
            else throw new InvalidCastException("未知函数表达式参数值类型 " + expression.Method.Name + " " + whereExpression.Expression.NodeType.ToString());
            convert(arguments[1]);
            SqlStream.Write(',');
            convert(arguments[2]);
            SqlStream.Write(')');
        }
    }
}
