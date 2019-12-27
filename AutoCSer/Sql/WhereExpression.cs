using AutoCSer.Extension;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct WhereExpression
    {
        /// <summary>
        /// 表达式类型
        /// </summary>
        internal enum ConvertType : byte
        {
            /// <summary>
            /// 原始表达式
            /// </summary>
            Expression,
            /// <summary>
            /// 转换表达式
            /// </summary>
            ConvertExpression,
            /// <summary>
            /// 未知类型表达式
            /// </summary>
            Unknown,
        }
        /// <summary>
        /// 逻辑值类型
        /// </summary>
        internal enum LogicType : byte
        {
            /// <summary>
            /// 逻辑真值
            /// </summary>
            False,
            /// <summary>
            /// 逻辑假值
            /// </summary>
            True,
            /// <summary>
            /// 未知
            /// </summary>
            Unknown
        }
        /// <summary>
        /// 空引用类型
        /// </summary>
        internal enum NullReferenceType : byte
        {
            /// <summary>
            /// 成员表达式目标对象
            /// </summary>
            MemberAccessTarget = 1,
            /// <summary>
            /// 函数调用目标对象
            /// </summary>
            MethodCallTarget,
        }
        /// <summary>
        /// 条件表达式
        /// </summary>
        internal Expression Expression;
        /// <summary>
        /// 表达式类型
        /// </summary>
        internal ConvertType Type;
        /// <summary>
        /// 未知表达式类型
        /// </summary>
        internal ExpressionType UnknownExpression;
        /// <summary>
        /// 空引用类型
        /// </summary>
        internal NullReferenceType NullReference;
        ///// <summary>
        ///// 表达式转换是否错误
        ///// </summary>
        //internal bool ExpressionConverterError;
        /// <summary>
        /// 条件表达式
        /// </summary>
        internal Expression NullExpression
        {
            get
            {
                return IsWhereTrue ? null : Expression;
            }
        }
        /// <summary>
        /// 条件表达式是否为真
        /// </summary>
        internal bool IsWhereTrue
        {
            get
            {
                if (Expression == null || object.ReferenceEquals(Expression, constantTrue)) return true;
                return Expression.NodeType == ExpressionType.Constant && (bool)Expression.GetConstantValue();
            }
        }
        /// <summary>
        /// 条件表达式是否为假
        /// </summary>
        internal bool IsWhereFalse
        {
            get
            {
                if (Expression == null) return false;
                if (object.ReferenceEquals(Expression, constantFalse)) return true;
                return Expression.NodeType == ExpressionType.Constant && !(bool)Expression.GetConstantValue();
            }
        }
        /// <summary>
        /// 判断是否常量 null
        /// </summary>
        internal bool IsConstantNull
        {
            get
            {
                return Expression.NodeType == ExpressionType.Constant && Expression.GetConstantValue() == null && Type != ConvertType.Unknown;
            }
        }
        /// <summary>
        /// 正常表达式
        /// </summary>
        internal Expression NormalExpression
        {
            get { return Type != ConvertType.Unknown ? Expression : null; }
        }

        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void TryConvert(Expression expression)
        {
            if (expression != null)
            {
                Expression = expression;
                Convert();
            }
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <param name="expression"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Convert(Expression expression)
        {
            Expression = expression;
            Type = ConvertType.Expression;
            Convert();
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        internal void Convert()
        {
            switch (Expression.NodeType)
            {
                case ExpressionType.OrElse: convertOrElse(); return;
                case ExpressionType.AndAlso: convertAndAlso(); return;
                case ExpressionType.Not: convertNot(); return;
                case ExpressionType.Equal: 
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.Or:
                case ExpressionType.And:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.LeftShift:
                case ExpressionType.RightShift:
                    convertBinaryExpression();
                    return;
                case ExpressionType.MemberAccess: convertMemberAccess(); return;
                case ExpressionType.Unbox: convertUnbox(); return;
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    convertUnaryCalculator();
                    return;
                case ExpressionType.IsTrue: convertIsTrue(); return;
                case ExpressionType.IsFalse: convertIsFalse(); return;
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    convertConvert(new UnionType { Value = Expression }.UnaryExpression);
                    return;
                case ExpressionType.Conditional: convertConditional(); return;
                case ExpressionType.Call: convertCall(); return;

                case ExpressionType.UnaryPlus:
                case ExpressionType.Constant: Type = ConvertType.Expression; return;
                default: Type = ConvertType.Unknown; UnknownExpression = Expression.NodeType; return;
            }
        }

        /// <summary>
        /// 获取逻辑值类型
        /// </summary>
        /// <returns></returns>
        private LogicType getLogicType()
        {
            if (Expression.NodeType == ExpressionType.Constant)
            {
                return (bool)Expression.GetConstantValue() ? LogicType.True : LogicType.False;
            }
            return LogicType.Unknown;
        }
        /// <summary>
        /// || 表达式
        /// </summary>
        private void convertOrElse()
        {
            BinaryExpression binaryExpression = new UnionType { Value = Expression }.BinaryExpression;
            Expression = binaryExpression.Left;
            Convert();
            if (Type != ConvertType.Unknown)
            {
                switch(getLogicType())
                {
                    case LogicType.False:
                        Expression = binaryExpression.Right;
                        Convert();
                        if (Type != ConvertType.Unknown) Type = ConvertType.ConvertExpression;
                        return;
                    case LogicType.True:
                        Expression = constantTrue;
                        Type = ConvertType.ConvertExpression;
                        return;
                    default:
                        WhereExpression left = this;
                        Expression = binaryExpression.Right;
                        Convert();
                        if (Type != ConvertType.Unknown)
                        {
                            switch (getLogicType())
                            {
                                case LogicType.False: Expression = left.Expression; break;
                                case LogicType.True: Expression = constantTrue; break;
                                default:
                                    if (Type == ConvertType.Expression && left.Type == ConvertType.Expression)
                                    {
                                        Expression = binaryExpression;
                                        return;
                                    }
                                    Expression = Expression.OrElse(left.Expression, Expression);
                                    break;
                            }
                            Type = ConvertType.ConvertExpression;
                        }
                        return;
                }
            }
        }
        /// <summary>
        /// AND 表达式
        /// </summary>
        private void convertAndAlso()
        {
            BinaryExpression binaryExpression = new UnionType { Value = Expression }.BinaryExpression;
            Expression = binaryExpression.Left;
            Convert();
            if (Type != ConvertType.Unknown)
            {
                switch (getLogicType())
                {
                    case LogicType.False:
                        Expression = constantFalse;
                        Type = ConvertType.ConvertExpression;
                        return;
                    case LogicType.True:
                        Expression = binaryExpression.Right;
                        Convert();
                        if (Type != ConvertType.Unknown) Type = ConvertType.ConvertExpression;
                        return;
                    default:
                        WhereExpression left = this;
                        Expression = binaryExpression.Right;
                        Convert();
                        if (Type != ConvertType.Unknown)
                        {
                            switch (getLogicType())
                            {
                                case LogicType.False: Expression = constantFalse; break;
                                case LogicType.True: Expression = left.Expression; break;
                                default:
                                    if (Type == ConvertType.Expression && left.Type == ConvertType.Expression)
                                    {
                                        Expression = binaryExpression;
                                        return;
                                    }
                                    Expression = Expression.AndAlso(left.Expression, Expression);
                                    break;
                            }
                            Type = ConvertType.ConvertExpression;
                        }
                        return;
                }
            }
        }
        /// <summary>
        /// ! 表达式
        /// </summary>
        private void convertNot()
        {
            UnaryExpression unaryExpression = new UnionType { Value = Expression }.UnaryExpression;
            Expression = unaryExpression.Operand;
            Convert();
            if (Type != ConvertType.Unknown)
            {
                switch(getLogicType())
                {
                    case LogicType.True: Expression = constantFalse; break;
                    case LogicType.False: Expression = constantTrue; break;
                    default:
                        if (Type == ConvertType.Expression)
                        {
                            Expression = unaryExpression;
                            return;
                        }
                        Expression = Expression.Not(Expression);
                        break;
                }
                Type = ConvertType.ConvertExpression;
            }
        }
        /// <summary>
        /// 二元表达式
        /// </summary>
        private void convertBinaryExpression()
        {
            BinaryExpression binaryExpression = new UnionType { Value = Expression }.BinaryExpression;
            Expression = binaryExpression.Left;
            Convert();
            if (Type != ConvertType.Unknown)
            {
                WhereExpression left = this;
                Expression = binaryExpression.Right;
                Convert();
                if (Type != ConvertType.Unknown)
                {
                    switch(binaryExpression.NodeType)
                    {
                        case ExpressionType.Equal:
                            if (convertEqual(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                        case ExpressionType.NotEqual:
                            if (convertNotEqual(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                        case ExpressionType.GreaterThanOrEqual:
                            if (convertGreaterThanOrEqual(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                        case ExpressionType.GreaterThan:
                            if (convertGreaterThan(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                        case ExpressionType.LessThan:
                            if (convertLessThan(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                        case ExpressionType.LessThanOrEqual:
                            if (convertLessThanOrEqual(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                        case ExpressionType.Add:
                        case ExpressionType.AddChecked:
                        case ExpressionType.Subtract:
                        case ExpressionType.SubtractChecked:
                        case ExpressionType.Multiply:
                        case ExpressionType.MultiplyChecked:
                        case ExpressionType.Divide:
                        case ExpressionType.Modulo:
                        case ExpressionType.Or:
                        case ExpressionType.And:
                        case ExpressionType.ExclusiveOr:
                            object value = getConstantCalculator(left.Expression, binaryExpression.NodeType);
                            if (value != null) Expression = Expression.Constant(value);
                            else if (Type == ConvertType.Expression && left.Type == ConvertType.Expression)
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            else
                            {
                                switch (binaryExpression.NodeType)
                                {
                                    case ExpressionType.Add: Expression = Expression.Add(left.Expression, Expression); break;
                                    case ExpressionType.AddChecked: Expression = Expression.AddChecked(left.Expression, Expression); break;
                                    case ExpressionType.Subtract: Expression = Expression.Subtract(left.Expression, Expression); break;
                                    case ExpressionType.SubtractChecked: Expression = Expression.SubtractChecked(left.Expression, Expression); break;
                                    case ExpressionType.Multiply: Expression = Expression.Multiply(left.Expression, Expression); break;
                                    case ExpressionType.MultiplyChecked: Expression = Expression.MultiplyChecked(left.Expression, Expression); break;
                                    case ExpressionType.Divide: Expression = Expression.Divide(left.Expression, Expression); break;
                                    case ExpressionType.Modulo: Expression = Expression.Modulo(left.Expression, Expression); break;
                                    case ExpressionType.Or: Expression = Expression.Or(left.Expression, Expression); break;
                                    case ExpressionType.And: Expression = Expression.And(left.Expression, Expression); break;
                                    case ExpressionType.ExclusiveOr: Expression = Expression.ExclusiveOr(left.Expression, Expression); break;
                                }
                            }
                            break;
                        case ExpressionType.LeftShift:
                            if (convertLeftShift(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                        case ExpressionType.RightShift:
                            if (convertRightShift(ref left))
                            {
                                Expression = binaryExpression;
                                return;
                            }
                            break;
                    }
                    Type = ConvertType.ConvertExpression;
                }
            }
        }
        /// <summary>
        /// 获取常量 == 比较结果类型
        /// </summary>
        /// <param name="leftExpression"></param>
        /// <returns></returns>
        private LogicType getConstantEqualType(Expression leftExpression)
        {
            if (Expression.NodeType == ExpressionType.Constant && leftExpression.NodeType == ExpressionType.Constant)
            {
                object left = leftExpression.GetConstantValue(), right = Expression.GetConstantValue();
                if (left != null) return left.Equals(right) ? LogicType.True : LogicType.False;
                return right == null || right.Equals(left) ? LogicType.True : LogicType.False;
            }
            return LogicType.Unknown;
        }
        /// <summary>
        /// == 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertEqual(ref WhereExpression left)
        {
            switch (getConstantEqualType(left.Expression))
            {
                case LogicType.True: Expression = constantTrue; return false;
                case LogicType.False: Expression = constantFalse; return false;
                default:
                    if (Type == ConvertType.Expression && left.Type == ConvertType.Expression) return true;
                    Expression = Expression.Equal(left.Expression, Expression);
                    return false;
            }
        }
        /// <summary>
        /// != 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertNotEqual(ref WhereExpression left)
        {
            switch (getConstantEqualType(left.Expression))
            {
                case LogicType.True: Expression = constantFalse; return false;
                case LogicType.False: Expression = constantTrue; return false;
                default:
                    if (Type == ConvertType.Expression && left.Type == ConvertType.Expression) return true;
                    Expression = Expression.NotEqual(left.Expression, Expression);
                    return false;
            }
        }
        /// <summary>
        /// 获取常量比较结果类型
        /// </summary>
        /// <param name="leftExpression"></param>
        /// <param name="expressionType"></param>
        /// <returns></returns>
        private LogicType getConstantComparatorType(Expression leftExpression, ExpressionType expressionType)
        {
            if (Expression.NodeType == ExpressionType.Constant && leftExpression.NodeType == ExpressionType.Constant)
            {
                object left = leftExpression.GetConstantValue();
                if (left != null)
                {
                    object right = Expression.GetConstantValue();
                    if (right != null)
                    {
                        System.Type type = left.GetType();
                        if (type == right.GetType())
                        {
                            Func<ExpressionType, object, object, LogicType> comparator;
                            if (comparators.TryGetValue(type, out comparator)) return comparator(expressionType, left, right);
                        }
                    }
                }
            }
            return LogicType.Unknown;
        }
        /// <summary>
        /// 大于等于 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertGreaterThanOrEqual(ref WhereExpression left)
        {
            switch (getConstantComparatorType(left.Expression, ExpressionType.GreaterThanOrEqual))
            {
                case LogicType.True: Expression = constantTrue; return false;
                case LogicType.False: Expression = constantFalse; return false;
                default:
                    if (Type == ConvertType.Expression && left.Type == ConvertType.Expression) return true;
                    Expression = Expression.GreaterThanOrEqual(left.Expression, Expression);
                    return false;
            }
        }
        /// <summary>
        /// 大于 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertGreaterThan(ref WhereExpression left)
        {
            switch (getConstantComparatorType(left.Expression, ExpressionType.GreaterThan))
            {
                case LogicType.True: Expression = constantTrue; return false;
                case LogicType.False: Expression = constantFalse; return false;
                default:
                    if (Type == ConvertType.Expression && left.Type == ConvertType.Expression) return true;
                    Expression = Expression.GreaterThan(left.Expression, Expression);
                    return false;
            }
        }
        /// <summary>
        /// 小于 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertLessThan(ref WhereExpression left)
        {
            switch (getConstantComparatorType(left.Expression, ExpressionType.GreaterThanOrEqual))
            {
                case LogicType.True: Expression = constantFalse; return false;
                case LogicType.False: Expression = constantTrue; return false;
                default:
                    if (Type == ConvertType.Expression && left.Type == ConvertType.Expression) return true;
                    Expression = Expression.LessThan(left.Expression, Expression);
                    return false;
            }
        }
        /// <summary>
        /// 大于等于 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertLessThanOrEqual(ref WhereExpression left)
        {
            switch (getConstantComparatorType(left.Expression, ExpressionType.GreaterThan))
            {
                case LogicType.True: Expression = constantFalse; return false;
                case LogicType.False: Expression = constantTrue; return false;
                default:
                    if (Type == ConvertType.Expression && left.Type == ConvertType.Expression) return true;
                    Expression = Expression.LessThanOrEqual(left.Expression, Expression);
                    return false;
            }
        }
        /// <summary>
        /// 获取常量计算结果
        /// </summary>
        /// <param name="leftExpression"></param>
        /// <param name="expressionType"></param>
        /// <returns></returns>
        private object getConstantCalculator(Expression leftExpression, ExpressionType expressionType)
        {
            if (Expression.NodeType == ExpressionType.Constant && leftExpression.NodeType == ExpressionType.Constant)
            {
                object left = leftExpression.GetConstantValue();
                if (left != null)
                {
                    object right = Expression.GetConstantValue();
                    if (right != null)
                    {
                        System.Type type = left.GetType();
                        if (type == right.GetType())
                        {
                            Func<ExpressionType, object, object, object> calculator;
                            if (calculators.TryGetValue(type, out calculator)) return calculator(expressionType, left, right);
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 获取常量计算结果
        /// </summary>
        /// <param name="leftExpression"></param>
        /// <param name="expressionType"></param>
        /// <returns></returns>
        private object getConstantCalculatorShift(Expression leftExpression, ExpressionType expressionType)
        {
            if (Expression.NodeType == ExpressionType.Constant && leftExpression.NodeType == ExpressionType.Constant)
            {
                object left = leftExpression.GetConstantValue();
                if (left != null)
                {
                    object right = Expression.GetConstantValue();
                    if (right != null && right.GetType() == typeof(int))
                    {
                        Func<ExpressionType, object, object, object> calculator;
                        if (calculators.TryGetValue(left.GetType(), out calculator)) return calculator(expressionType, left, right);
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 左移 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertLeftShift(ref WhereExpression left)
        {
            object value = getConstantCalculator(left.Expression, ExpressionType.LeftShift);
            if (value != null)
            {
                Expression = Expression.Constant(value);
                return false;
            }
            if (Type == ConvertType.Expression && left.Type == ConvertType.Expression) return true;
            Expression = Expression.LeftShift(left.Expression, Expression);
            return false;
        }
        /// <summary>
        /// 右移 表达式
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private bool convertRightShift(ref WhereExpression left)
        {
            object value = getConstantCalculator(left.Expression, ExpressionType.RightShift);
            if (value != null)
            {
                Expression = Expression.Constant(value);
                return false;
            }
            if (Type == ConvertType.Expression && left.Type == ConvertType.Expression) return true;
            Expression = Expression.RightShift(left.Expression, Expression);
            return false;
        }
        /// <summary>
        /// 成员表达式
        /// </summary>
        private void convertMemberAccess()
        {
            MemberExpression memberExpression = new UnionType { Value = Expression }.MemberExpression;
            if (memberExpression.Expression != null && typeof(ParameterExpression).IsAssignableFrom(memberExpression.Expression.GetType()))
            {
                Type = ConvertType.Expression;
            }
            else convertMemberAccess(memberExpression);
        }
        /// <summary>
        /// 成员表达式
        /// </summary>
        /// <param name="memberExpression"></param>
        private void convertMemberAccess(MemberExpression memberExpression)
        {
            object target = null;
            if (memberExpression.Expression != null)
            {
                Expression = memberExpression.Expression;
                Convert();
                if (Type == ConvertType.Unknown) return;
                if (Expression.NodeType != ExpressionType.Constant) goto UNKNOWN;
                target = Expression.GetConstantValue();
                if (target == null)
                {
                    NullReference = NullReferenceType.MemberAccessTarget;
                    goto UNKNOWN;
                }
            }
            FieldInfo fieldInfo = memberExpression.Member as FieldInfo;
            if (fieldInfo != null)
            {
                Expression = Expression.Constant(fieldInfo.GetValue(target));
                Type = ConvertType.ConvertExpression;
                return;
            }
            PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo != null)
            {
                Expression = Expression.Constant(propertyInfo.GetValue(target, null));
                Type = ConvertType.ConvertExpression;
                return;
            }
        UNKNOWN:
            Type = ConvertType.Unknown;
            UnknownExpression = ExpressionType.MemberAccess;
            Expression = memberExpression;
        }
        /// <summary>
        /// 成员表达式
        /// </summary>
        /// <param name="memberExpression"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ConvertMemberAccess(MemberExpression memberExpression)
        {
            Expression = memberExpression;
            Type = ConvertType.Expression;
            convertMemberAccess(memberExpression);
        }
        /// <summary>
        /// 拆箱表达式
        /// </summary>
        private void convertUnbox()
        {
            UnaryExpression unaryExpression = new UnionType { Value = Expression }.UnaryExpression;
            Expression = unaryExpression.Operand;
            Convert();
            if (Type == ConvertType.Expression) Expression = unaryExpression;
        }
        /// <summary>
        /// 单值计算表达式
        /// </summary>
        private void convertUnaryCalculator()
        {
            UnaryExpression unaryExpression = new UnionType { Value = Expression }.UnaryExpression;
            Expression = unaryExpression.Operand;
            Convert();
            if (Type != ConvertType.Unknown)
            {
                if (Expression.NodeType == ExpressionType.Constant)
                {
                    object value = Expression.GetConstantValue();
                    if (value != null)
                    {
                        Func<ExpressionType, object, object> calculator;
                        if (unaryCalculators.TryGetValue(value.GetType(), out calculator))
                        {
                            value = calculator(unaryExpression.NodeType, value);
                            if (value != null)
                            {
                                Expression = Expression.Constant(value);
                                Type = ConvertType.ConvertExpression;
                                return;
                            }
                        }
                    }
                }
                if (Type == ConvertType.Expression)
                {
                    Expression = unaryExpression;
                    return;
                }
                switch (unaryExpression.NodeType)
                {
                    case ExpressionType.Negate: Expression = Expression.Negate(Expression); break;
                    case ExpressionType.NegateChecked: Expression = Expression.NegateChecked(Expression); break;
                }
                Type = ConvertType.ConvertExpression;
            }
        }
        /// <summary>
        /// 真值判断 表达式
        /// </summary>
        private void convertIsTrue()
        {
            UnaryExpression unaryExpression = new UnionType { Value = Expression }.UnaryExpression;
            Expression = unaryExpression.Operand;
            Convert();
            if (Type != ConvertType.Unknown && getLogicType() == LogicType.Unknown)
            {
                if (Type == ConvertType.Expression) Expression = unaryExpression;
                else
                {
                    Expression = Expression.IsTrue(Expression);
                    Type = ConvertType.ConvertExpression;
                }
            }
        }
        /// <summary>
        /// 假值判断 表达式
        /// </summary>
        private void convertIsFalse()
        {
            UnaryExpression unaryExpression = new UnionType { Value = Expression }.UnaryExpression;
            Expression = unaryExpression.Operand;
            Convert();
            if (Type != ConvertType.Unknown)
            {
                switch (getLogicType())
                {
                    case LogicType.True: Expression = constantFalse; break;
                    case LogicType.False: Expression = constantTrue; break;
                    default:
                        if (Type == ConvertType.Expression)
                        {
                            Expression = unaryExpression;
                            return;
                        }
                        Expression = Expression.IsFalse(Expression);
                        break;
                }
                Type = ConvertType.ConvertExpression;
            }
        }
        /// <summary>
        /// 类型转换表达式
        /// </summary>
        /// <param name="unaryExpression"></param>
        private void convertConvert(UnaryExpression unaryExpression)
        {
            Expression = unaryExpression.Operand;
            Convert();
            if (Type != ConvertType.Unknown)
            {
                if (Expression.NodeType == ExpressionType.Constant)
                {
                    object value = Expression.GetConstantValue();
                    if (value != null)
                    {
                        System.Type valueType = value.GetType(), convertType = unaryExpression.Type;
                        if (valueType == convertType)
                        {
                            Type = ConvertType.ConvertExpression;
                            return;
                        }
                        if (valueType.IsEnum)
                        {
                            if (System.Enum.GetUnderlyingType(valueType) == convertType)
                            {
                                Expression = Expression.Constant(AutoCSer.Sql.MsSql.ExpressionConverter.ConvertEnum(value, convertType));
                                Type = ConvertType.ConvertExpression;
                                return;
                            }
                        }
                        else
                        {
                            MethodInfo method = AutoCSer.Emit.CastType.GetMethod(valueType, convertType);
                            if (method != null)
                            {
                                Expression = Expression.Constant(method.Invoke(null, new object[] { value }));
                                Type = ConvertType.ConvertExpression;
                                return;
                            }
                        }
                    }
                    else if (unaryExpression.Type.IsClass)
                    {
                        Expression = Expression.Constant(null, unaryExpression.Type);
                        Type = ConvertType.ConvertExpression;
                        return;
                    }
                }
                if (Type == ConvertType.Expression)
                {
                    Expression = unaryExpression;
                    return;
                }
                Expression = Expression.Convert(Expression, unaryExpression.Type);
                Type = ConvertType.ConvertExpression;
            }
        }
        /// <summary>
        /// 类型转换表达式
        /// </summary>
        /// <param name="unaryExpression"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ConvertConvert(UnaryExpression unaryExpression)
        {
            Expression = unaryExpression;
            Type = ConvertType.Expression;
            convertConvert(unaryExpression);
        }
        /// <summary>
        /// 三元表达式
        /// </summary>
        private void convertConditional()
        {
            ConditionalExpression conditionalExpression = new UnionType { Value = Expression }.ConditionalExpression;
            Expression = conditionalExpression.Test;
            Convert();
            if (Type != ConvertType.Unknown)
            {
                switch (getLogicType())
                {
                    case LogicType.True:
                        Expression = conditionalExpression.IfTrue;
                        Convert();
                        if (Type != ConvertType.Unknown) Type = ConvertType.ConvertExpression;
                        return;
                    case LogicType.False:
                        Expression = conditionalExpression.IfFalse;
                        Convert();
                        if (Type != ConvertType.Unknown) Type = ConvertType.ConvertExpression;
                        return;
                    default:
                        WhereExpression test = this;
                        Expression = conditionalExpression.IfTrue;
                        Convert();
                        if (Type != ConvertType.Unknown)
                        {
                            WhereExpression left = this;
                            Expression = conditionalExpression.IfFalse;
                            Convert();
                            if (Type != ConvertType.Unknown)
                            {
                                if (Type == ConvertType.Expression && test.Type == ConvertType.Expression && left.Type == ConvertType.Expression)
                                {
                                    Expression = conditionalExpression;
                                    return;
                                }
                                Expression = Expression.Condition(test.Expression, left.Expression, Expression);
                                Type = ConvertType.ConvertExpression;
                            }
                        }
                        return;
                }
            }
        }
        /// <summary>
        /// 函数表达式
        /// </summary>
        private void convertCall()
        {
            MethodCallExpression methodCallExpression = new UnionType { Value = Expression }.MethodCallExpression;
            MethodInfo method = methodCallExpression.Method;
            if (method.ReflectedType == typeof(ExpressionCall))
            {
                switch (method.Name)
                {
                    case "In":
                    case "NotIn":
                        Expression = methodCallExpression.Arguments[1];
                        Convert();
                        if (Type == ConvertType.Unknown) return;
                        if (Expression.NodeType == ExpressionType.Constant)
                        {
                            object values = Expression.GetConstantValue();
                            if (values == null || (values.GetType().IsArray && ((Array)values).Length == 0))
                            {
                                Expression = method.Name.Length == 2 ? constantFalse : constantTrue;
                                Type = ConvertType.ConvertExpression;
                                return;
                            }
                        }
                        break;
                }
                Expression = methodCallExpression;
                Type = ConvertType.Expression;
                return;
            }
            convertCall(methodCallExpression, method);
        }
        /// <summary>
        /// 函数表达式
        /// </summary>
        /// <param name="methodCallExpression"></param>
        /// <param name="method"></param>
        private void convertCall(MethodCallExpression methodCallExpression, MethodInfo method)
        {
            object target = null;
            if (methodCallExpression.Object != null)
            {
                Expression = methodCallExpression.Object;
                Convert();
                if (Type == ConvertType.Unknown) return;
                if (Expression.NodeType != ExpressionType.Constant) goto UNKNOWN;
                target = Expression.GetConstantValue();
                if (target == null)
                {
                    NullReference = NullReferenceType.MethodCallTarget;
                    goto UNKNOWN;
                }
            }
            object[] arguments = NullValue<object>.Array;
            if (methodCallExpression.Arguments != null && methodCallExpression.Arguments.Count != 0)
            {
                arguments = new object[methodCallExpression.Arguments.Count];
                int argumentIndex = 0;
                foreach (Expression agrumentExpression in methodCallExpression.Arguments)
                {
                    Expression = agrumentExpression;
                    Convert();
                    if (Type == ConvertType.Unknown) return;
                    if (Expression.NodeType != ExpressionType.Constant) goto UNKNOWN;
                    arguments[argumentIndex++] = Expression.GetConstantValue();
                }
            }
            Expression = Expression.Constant(method.Invoke(target, arguments));
            Type = ConvertType.ConvertExpression;
            return;
        UNKNOWN:
            Type = ConvertType.Unknown;
            UnknownExpression = ExpressionType.Call;
            Expression = methodCallExpression;
        }
        /// <summary>
        /// 函数表达式
        /// </summary>
        /// <param name="methodCallExpression"></param>
        /// <param name="method"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ConvertCall(MethodCallExpression methodCallExpression, MethodInfo method)
        {
            Expression = methodCallExpression;
            Type = ConvertType.Expression;
            convertCall(methodCallExpression, method);
        }

        /// <summary>
        /// 常量真值
        /// </summary>
        private static readonly ConstantExpression constantTrue = Expression.Constant(true);
        /// <summary>
        /// 常量假值
        /// </summary>
        private static readonly ConstantExpression constantFalse = Expression.Constant(false);

        /// <summary>
        /// 常量比较器集合
        /// </summary>
        private static readonly Dictionary<System.Type, Func<ExpressionType, object, object, LogicType>> comparators;
        /// <summary>
        /// 常量计算器集合
        /// </summary>
        private static readonly Dictionary<System.Type, Func<ExpressionType, object, object, object>> calculators;
        /// <summary>
        /// 常量计算器集合
        /// </summary>
        private static readonly Dictionary<System.Type, Func<ExpressionType, object, object>> unaryCalculators;
        static WhereExpression()
        {
            comparators = DictionaryCreator.CreateOnly<System.Type, Func<ExpressionType, object, object, LogicType>>();
            comparators.Add(typeof(ulong), compareULong);
            comparators.Add(typeof(long), compareLong);
            comparators.Add(typeof(uint), compareUInt);
            comparators.Add(typeof(int), compareInt);
            comparators.Add(typeof(ushort), compareUShort);
            comparators.Add(typeof(short), compareShort);
            comparators.Add(typeof(byte), compareByte);
            comparators.Add(typeof(sbyte), compareSByte);
            comparators.Add(typeof(double), compareDouble);
            comparators.Add(typeof(float), compareFloat);
            comparators.Add(typeof(decimal), compareDecimal);
            comparators.Add(typeof(DateTime), compareDateTime);

            calculators = DictionaryCreator.CreateOnly<System.Type, Func<ExpressionType, object, object, object>>();
            calculators.Add(typeof(ulong), calculateULong);
            calculators.Add(typeof(long), calculateLong);
            calculators.Add(typeof(uint), calculateUInt);
            calculators.Add(typeof(int), calculateInt);
            calculators.Add(typeof(ushort), calculateUShort);
            calculators.Add(typeof(short), calculateShort);
            calculators.Add(typeof(byte), calculateByte);
            calculators.Add(typeof(sbyte), calculateSByte);
            calculators.Add(typeof(char), calculateChar);
            calculators.Add(typeof(double), calculateDouble);
            calculators.Add(typeof(float), calculateFloat);
            calculators.Add(typeof(decimal), calculateDecimal);

            unaryCalculators = DictionaryCreator.CreateOnly<System.Type, Func<ExpressionType, object, object>>();
            unaryCalculators.Add(typeof(long), calculateLong);
            unaryCalculators.Add(typeof(uint), calculateUInt);
            unaryCalculators.Add(typeof(int), calculateInt);
            unaryCalculators.Add(typeof(ushort), calculateUShort);
            unaryCalculators.Add(typeof(short), calculateShort);
            unaryCalculators.Add(typeof(byte), calculateByte);
            unaryCalculators.Add(typeof(sbyte), calculateSByte);
            unaryCalculators.Add(typeof(char), calculateChar);
            unaryCalculators.Add(typeof(double), calculateDouble);
            unaryCalculators.Add(typeof(float), calculateFloat);
            unaryCalculators.Add(typeof(decimal), calculateDecimal);
        }
    }
}
