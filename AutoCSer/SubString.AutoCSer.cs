using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 字符子串
    /// </summary>
    public unsafe partial struct SubString
    {
        /// <summary>
        /// 原字符串
        /// </summary>
        public string OriginalString
        {
            get { return String; }
        }
        /// <summary>
        /// 原字符串中的起始位置
        /// </summary>
        public int StartIndex
        {
            get { return Start; }
        }
        /// <summary>
        /// 字符子串长度
        /// </summary>
        public int Count
        {
            get { return Length; }
        }
        /// <summary>
        /// 原字符串中的结束位置
        /// </summary>
        public int EndIndex
        {
            get { return Start + Length; }
        }
        /// <summary>
        /// 字符子串
        /// </summary>
        /// <param name="value"></param>
        public SubString(string value)
        {
            String = value;
            Start = 0;
            Length = value == null ? 0 : value.Length;
        }
        /// <summary>
        /// 字符子串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">长度</param>
        public SubString(string value, int startIndex, int length)
        {
            FormatRange range = new FormatRange(value.Length, startIndex, length);
            if (range.GetCount != length) throw new IndexOutOfRangeException();
            String = value;
            Start = range.SkipCount;
            Length = length;
        }
        /// <summary>
        /// 字符子串
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="value">字符串</param>
        /// <param name="length">长度</param>
        internal SubString(int startIndex, int length, string value)
        {
            String = value;
            Start = startIndex;
            Length = length;
        }
        /// <summary>
        /// 修改起始位置
        /// </summary>
        /// <param name="count"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void MoveStart(int count)
        {
            Start += count;
            Length -= count;
        }
        /// <summary>
        /// 获取子串
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <returns>子串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal SubString GetSub(int startIndex)
        {
            return new SubString { String = String, Start = Start + startIndex, Length = Length - startIndex };
        }
        /// <summary>
        /// 设置为子串
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Sub(int startIndex, int length)
        {
            Start += startIndex;
            Length = length;
        }
        /// <summary>
        /// 设置为子串
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Sub(int startIndex)
        {
            Start += startIndex;
            Length -= startIndex;
        }
        /// <summary>
        /// 字符查找
        /// </summary>
        /// <param name="value">查找值</param>
        /// <returns>字符位置,失败返回-1</returns>
        public int IndexOf(char value)
        {
            if (Length != 0)
            {
                fixed (char* valueFixed = String)
                {
                    char* start = valueFixed + Start, find = AutoCSer.Extension.StringExtension.FindNotNull(start, start + Length, value);
                    if (find != null) return (int)(find - start);
                }
            }
            return -1;
        }
        ///// <summary>
        ///// 字符替换
        ///// </summary>
        ///// <param name="value">原字符</param>
        ///// <param name="replaceChar">替换后的字符</param>
        //public void Replace(char value, char replaceChar)
        //{
        //    if (Length != 0)
        //    {
        //        fixed (char* valueFixed = String)
        //        {
        //            char* start = valueFixed + StartIndex, end = start + Length;
        //            if (*--end == value)
        //            {
        //                do
        //                {
        //                    while (*start != value) ++start;
        //                    *start = replaceChar;
        //                    if (start == end) return;
        //                    ++start;
        //                }
        //                while (true);
        //            }
        //            while (start != end)
        //            {
        //                if (*start == value) *start = replaceChar;
        //                ++start;
        //            }
        //        }
        //    }
        //}
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="split">分割符</param>
        /// <returns>字符子串集合</returns>
        public LeftArray<SubString> Split(char split)
        {
            LeftArray<SubString> values = default(LeftArray<SubString>);
            if (String != null)
            {
                fixed (char* valueFixed = String)
                {
                    char* last = valueFixed + Start, end = last + Length;
                    for (char* start = last; start != end; )
                    {
                        if (*start == split)
                        {
                            values.PrepLength(1);
                            values.Array[values.Length++].Set(String, (int)(last - valueFixed), (int)(start - last));
                            last = ++start;
                        }
                        else ++start;
                    }
                    values.PrepLength(1);
                    values.Array[values.Length++].Set(String, (int)(last - valueFixed), (int)(end - last));
                }
            }
            return values;
        }
        /// <summary>
        /// 比较字符串大小
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        internal int CompareTo(ref SubString other)
        {
            int size = Math.Min(Length, other.Length);
            if (size == 0)
            {
                if (Length == 0)
                {
                    if (other.Length == 0)
                    {
                        if (String == null) return other.String == null ? 0 : -1;
                        return other.String == null ? 1 : 0;
                    }
                    return -1;
                }
                return 1;
            }
            fixed (char* stringFixed = String, otherStringFixed = other.String)
            {
                char* start = stringFixed + Start, end = start + size, otherStart = otherStringFixed + other.Start;
                do
                {
                    if (*start != *otherStart) return *start - *otherStart;
                    ++otherStart;
                }
                while (++start != end);
            }
            return Length - other.Length;
        }
        /// <summary>
        /// 删除前后空格
        /// </summary>
        /// <returns>删除前后空格</returns>
        public SubString Trim()
        {
            if (Length != 0)
            {
                fixed (char* valueFixed = String)
                {
                    char* start = valueFixed + Start, end = start + Length;
                    start = AutoCSer.Extension.StringExtension.trimStartNotEmpty(start, end);
                    if (start == null) return new SubString(string.Empty);
                    end = AutoCSer.Extension.StringExtension.trimEndNotEmpty(start, end);
                    if (end == null) return new SubString(string.Empty);
                    return new SubString((int)(start - valueFixed), (int)(end - start), String);
                }
            }
            return this;
        }
        /// <summary>
        /// 删除后缀
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public SubString TrimEnd(char value)
        {
            if (Length != 0)
            {
                fixed (char* valueFixed = String)
                {
                    char* start = valueFixed + Start, end = start + Length;
                    do
                    {
                        if (*--end != value) return new SubString(Start, (int)(end - start) + 1, String);
                    }
                    while (end != start);
                    return new SubString(string.Empty);
                }
            }
            return this;
        }
        /// <summary>
        /// 是否以字符串开始
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>是否以字符串开始</returns>
        public bool StartsWith(string value)
        {
            if (Length >= value.Length)
            {
                fixed (char* valueFixed = String, cmpFixed = value)
                {
                    return AutoCSer.Memory.EqualNotNull(valueFixed + Start, cmpFixed, value.Length << 1);
                }
            }
            return false;
        }
    }
}
