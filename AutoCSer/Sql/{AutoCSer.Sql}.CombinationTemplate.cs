//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable

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
        private static object calculateLong(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (long)left + (long)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (long)left + (long)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (long)left - (long)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (long)left - (long)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (long)left * (long)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (long)left * (long)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (long)left / (long)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (long)left % (long)right;
                case System.Linq.Expressions.ExpressionType.Or: return (long)left | (long)right;
                case System.Linq.Expressions.ExpressionType.And: return (long)left & (long)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (long)left ^ (long)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (long)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (long)left >> (int)right;
                default: return null;
            }
        }
    }
}

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
        private static object calculateUInt(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (uint)left + (uint)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (uint)left + (uint)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (uint)left - (uint)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (uint)left - (uint)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (uint)left * (uint)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (uint)left * (uint)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (uint)left / (uint)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (uint)left % (uint)right;
                case System.Linq.Expressions.ExpressionType.Or: return (uint)left | (uint)right;
                case System.Linq.Expressions.ExpressionType.And: return (uint)left & (uint)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (uint)left ^ (uint)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (uint)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (uint)left >> (int)right;
                default: return null;
            }
        }
    }
}

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
        private static object calculateInt(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (int)left + (int)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (int)left + (int)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (int)left - (int)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (int)left - (int)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (int)left * (int)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (int)left * (int)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (int)left / (int)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (int)left % (int)right;
                case System.Linq.Expressions.ExpressionType.Or: return (int)left | (int)right;
                case System.Linq.Expressions.ExpressionType.And: return (int)left & (int)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (int)left ^ (int)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (int)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (int)left >> (int)right;
                default: return null;
            }
        }
    }
}

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
        private static object calculateUShort(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (ushort)left + (ushort)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (ushort)left + (ushort)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (ushort)left - (ushort)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (ushort)left - (ushort)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (ushort)left * (ushort)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (ushort)left * (ushort)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (ushort)left / (ushort)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (ushort)left % (ushort)right;
                case System.Linq.Expressions.ExpressionType.Or: return (ushort)left | (ushort)right;
                case System.Linq.Expressions.ExpressionType.And: return (ushort)left & (ushort)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (ushort)left ^ (ushort)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (ushort)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (ushort)left >> (int)right;
                default: return null;
            }
        }
    }
}

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
        private static object calculateShort(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (short)left + (short)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (short)left + (short)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (short)left - (short)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (short)left - (short)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (short)left * (short)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (short)left * (short)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (short)left / (short)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (short)left % (short)right;
                case System.Linq.Expressions.ExpressionType.Or: return (short)left | (short)right;
                case System.Linq.Expressions.ExpressionType.And: return (short)left & (short)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (short)left ^ (short)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (short)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (short)left >> (int)right;
                default: return null;
            }
        }
    }
}

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
        private static object calculateByte(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (byte)left + (byte)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (byte)left + (byte)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (byte)left - (byte)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (byte)left - (byte)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (byte)left * (byte)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (byte)left * (byte)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (byte)left / (byte)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (byte)left % (byte)right;
                case System.Linq.Expressions.ExpressionType.Or: return (byte)left | (byte)right;
                case System.Linq.Expressions.ExpressionType.And: return (byte)left & (byte)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (byte)left ^ (byte)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (byte)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (byte)left >> (int)right;
                default: return null;
            }
        }
    }
}

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
        private static object calculateSByte(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (sbyte)left + (sbyte)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (sbyte)left + (sbyte)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (sbyte)left - (sbyte)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (sbyte)left - (sbyte)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (sbyte)left * (sbyte)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (sbyte)left * (sbyte)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (sbyte)left / (sbyte)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (sbyte)left % (sbyte)right;
                case System.Linq.Expressions.ExpressionType.Or: return (sbyte)left | (sbyte)right;
                case System.Linq.Expressions.ExpressionType.And: return (sbyte)left & (sbyte)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (sbyte)left ^ (sbyte)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (sbyte)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (sbyte)left >> (int)right;
                default: return null;
            }
        }
    }
}

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
        private static object calculateChar(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (char)left + (char)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (char)left + (char)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (char)left - (char)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (char)left - (char)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (char)left * (char)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (char)left * (char)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (char)left / (char)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (char)left % (char)right;
                case System.Linq.Expressions.ExpressionType.Or: return (char)left | (char)right;
                case System.Linq.Expressions.ExpressionType.And: return (char)left & (char)right;
                case System.Linq.Expressions.ExpressionType.ExclusiveOr: return (char)left ^ (char)right;
                case System.Linq.Expressions.ExpressionType.LeftShift: return (char)left << (int)right;
                case System.Linq.Expressions.ExpressionType.RightShift: return (char)left >> (int)right;
                default: return null;
            }
        }
    }
}

namespace AutoCSer.Sql
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct WhereExpression
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static LogicType compareLong(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((long)left) >= ((long)right) ? LogicType.True : LogicType.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((long)left) > ((long)right) ? LogicType.True : LogicType.False;
                default: return LogicType.Unknown;
            }
        }
    }
}

namespace AutoCSer.Sql
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct WhereExpression
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static LogicType compareUInt(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((uint)left) >= ((uint)right) ? LogicType.True : LogicType.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((uint)left) > ((uint)right) ? LogicType.True : LogicType.False;
                default: return LogicType.Unknown;
            }
        }
    }
}

namespace AutoCSer.Sql
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct WhereExpression
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static LogicType compareInt(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((int)left) >= ((int)right) ? LogicType.True : LogicType.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((int)left) > ((int)right) ? LogicType.True : LogicType.False;
                default: return LogicType.Unknown;
            }
        }
    }
}

namespace AutoCSer.Sql
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct WhereExpression
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static LogicType compareUShort(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((ushort)left) >= ((ushort)right) ? LogicType.True : LogicType.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((ushort)left) > ((ushort)right) ? LogicType.True : LogicType.False;
                default: return LogicType.Unknown;
            }
        }
    }
}

namespace AutoCSer.Sql
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct WhereExpression
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static LogicType compareShort(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((short)left) >= ((short)right) ? LogicType.True : LogicType.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((short)left) > ((short)right) ? LogicType.True : LogicType.False;
                default: return LogicType.Unknown;
            }
        }
    }
}

namespace AutoCSer.Sql
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct WhereExpression
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static LogicType compareByte(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((byte)left) >= ((byte)right) ? LogicType.True : LogicType.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((byte)left) > ((byte)right) ? LogicType.True : LogicType.False;
                default: return LogicType.Unknown;
            }
        }
    }
}

namespace AutoCSer.Sql
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct WhereExpression
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static LogicType compareSByte(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((sbyte)left) >= ((sbyte)right) ? LogicType.True : LogicType.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((sbyte)left) > ((sbyte)right) ? LogicType.True : LogicType.False;
                default: return LogicType.Unknown;
            }
        }
    }
}

namespace AutoCSer.Sql
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct WhereExpression
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static LogicType compareDouble(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((double)left) >= ((double)right) ? LogicType.True : LogicType.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((double)left) > ((double)right) ? LogicType.True : LogicType.False;
                default: return LogicType.Unknown;
            }
        }
    }
}

namespace AutoCSer.Sql
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct WhereExpression
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static LogicType compareFloat(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((float)left) >= ((float)right) ? LogicType.True : LogicType.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((float)left) > ((float)right) ? LogicType.True : LogicType.False;
                default: return LogicType.Unknown;
            }
        }
    }
}

namespace AutoCSer.Sql
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct WhereExpression
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static LogicType compareDecimal(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((decimal)left) >= ((decimal)right) ? LogicType.True : LogicType.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((decimal)left) > ((decimal)right) ? LogicType.True : LogicType.False;
                default: return LogicType.Unknown;
            }
        }
    }
}

namespace AutoCSer.Sql
{
    /// <summary>
    /// 条件表达式重组
    /// </summary>
    public partial struct WhereExpression
    {
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static LogicType compareDateTime(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual: return ((DateTime)left) >= ((DateTime)right) ? LogicType.True : LogicType.False;
                case System.Linq.Expressions.ExpressionType.GreaterThan: return ((DateTime)left) > ((DateTime)right) ? LogicType.True : LogicType.False;
                default: return LogicType.Unknown;
            }
        }
    }
}

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
        private static object calculateFloat(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (float)left + (float)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (float)left + (float)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (float)left - (float)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (float)left - (float)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (float)left * (float)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (float)left * (float)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (float)left / (float)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (float)left % (float)right;
                default: return null;
            }
        }
    }
}

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
        private static object calculateDecimal(System.Linq.Expressions.ExpressionType type, object left, object right)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Add: return (decimal)left + (decimal)right;
                case System.Linq.Expressions.ExpressionType.AddChecked: checked { return (decimal)left + (decimal)right; }
                case System.Linq.Expressions.ExpressionType.Subtract: return (decimal)left - (decimal)right;
                case System.Linq.Expressions.ExpressionType.SubtractChecked: checked { return (decimal)left - (decimal)right; }
                case System.Linq.Expressions.ExpressionType.Multiply: return (decimal)left * (decimal)right;
                case System.Linq.Expressions.ExpressionType.MultiplyChecked: checked { return (decimal)left * (decimal)right; }
                case System.Linq.Expressions.ExpressionType.Divide: return (decimal)left / (decimal)right;
                case System.Linq.Expressions.ExpressionType.Modulo: return (decimal)left % (decimal)right;
                default: return null;
            }
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateUInt(System.Linq.Expressions.ExpressionType type, object value)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(uint)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(uint)value; }
            }
            return null;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateInt(System.Linq.Expressions.ExpressionType type, object value)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(int)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(int)value; }
            }
            return null;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateUShort(System.Linq.Expressions.ExpressionType type, object value)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(ushort)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(ushort)value; }
            }
            return null;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateShort(System.Linq.Expressions.ExpressionType type, object value)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(short)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(short)value; }
            }
            return null;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateByte(System.Linq.Expressions.ExpressionType type, object value)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(byte)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(byte)value; }
            }
            return null;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateSByte(System.Linq.Expressions.ExpressionType type, object value)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(sbyte)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(sbyte)value; }
            }
            return null;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateChar(System.Linq.Expressions.ExpressionType type, object value)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(char)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(char)value; }
            }
            return null;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateDouble(System.Linq.Expressions.ExpressionType type, object value)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(double)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(double)value; }
            }
            return null;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateFloat(System.Linq.Expressions.ExpressionType type, object value)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(float)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(float)value; }
            }
            return null;
        }
    }
}

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
        /// <param name="value"></param>
        /// <returns></returns>
        private static object calculateDecimal(System.Linq.Expressions.ExpressionType type, object value)
        {
            switch (type)
            {
                case System.Linq.Expressions.ExpressionType.Negate: return -(decimal)value;
                case System.Linq.Expressions.ExpressionType.NegateChecked: checked { return -(decimal)value; }
            }
            return null;
        }
    }
}

#endif