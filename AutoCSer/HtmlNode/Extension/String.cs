using System;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 字符串相关操作
    /// </summary>
    internal unsafe static partial class String_Html
    {
        ///// <summary>
        ///// 字符查找
        ///// </summary>
        ///// <param name="start">起始位置,不能为null</param>
        ///// <param name="end">结束位置,不能为null,长度必须大于0</param>
        ///// <param name="value">查找值</param>
        ///// <returns>字符位置,失败为null</returns>
        //internal static char* FindLastNotNull(char* start, char* end, char value)
        //{
        //    if (*start == value)
        //    {
        //        while (*--end != value);
        //        return end;
        //    }
        //    ++start;
        //    while (start != end)
        //    {
        //        if (*--end == value) return end;
        //    }
        //    return null;
        //}
        /// <summary>
        /// 比较字符串(忽略大小写)
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static bool EqualCase(this SubString left, ref SubString right)
        {
            if (left.Length == right.Length)
            {
                if (left.Length != 0)
                {
                    if (object.ReferenceEquals(left.String, right.String))
                    {
                        if (left.Start == right.Start) return true;
                        fixed (char* leftFixed = left.String)
                        {
                            return AutoCSer.Extension.StringExtension.equalCaseNotNull(leftFixed + left.Start, leftFixed + right.Start, left.Length);
                        }
                    }
                    fixed (char* leftFixed = left.String, rightFixed = right.String)
                    {
                        return AutoCSer.Extension.StringExtension.equalCaseNotNull(leftFixed + left.Start, rightFixed + right.Start, left.Length);
                    }
                }
                return right.String == null ? left.String == null : left.String != null;
            }
            return false;
        }
        /// <summary>
        /// 比较字符串(忽略大小写)
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static bool EqualCase(this SubString left, string right)
        {
            if (right == null) return left.String == null;
            if (left.Length == right.Length)
            {
                if (left.Length != 0)
                {
                    if (object.ReferenceEquals(left.String, right)) return true;
                    fixed (char* leftFixed = left.String, rightFixed = right)
                    {
                        return AutoCSer.Extension.StringExtension.equalCaseNotNull(leftFixed + left.Start, rightFixed, left.Length);
                    }
                }
                return left.String != null;
            }
            return false;
        }
        /// <summary>
        /// 统计查找字符数量
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="findChar"></param>
        /// <returns></returns>
        private static int countNotEmpty(char* start, char* end, char findChar)
        {
            int count = 0;
            do
            {
                if (*start == findChar) ++count;
            }
            while (++start != end);
            return count;
        }
        /// <summary>
        /// 分割字符串并返回数值集合(不检查数字合法性)
        /// </summary>
        /// <param name="intString">原字符串</param>
        /// <param name="split">分割符</param>
        /// <returns>数值集合</returns>
        internal static int[] splitIntNoCheckNotEmpty(this string intString, char split)
        {
            fixed (char* intStringFixed = intString)
            {
                char* end = intStringFixed + intString.Length;
                int[] values = new int[countNotEmpty(intStringFixed, end, split) + 1];
                int intValue = 0;
                fixed (int* intFixed = values)
                {
                    int* write = intFixed;
                    char* start = intStringFixed;
                    do
                    {
                        if (*start == split)
                        {
                            *write++ = intValue;
                            intValue = 0;
                        }
                        else
                        {
                            intValue *= 10;
                            intValue += *(byte*)start - '0';
                        }
                    }
                    while (++start != end);
                    *write++ = intValue;
                }
                return values;
            }
        }
    }
}
