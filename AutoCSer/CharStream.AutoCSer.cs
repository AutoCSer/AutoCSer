using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Globalization;

namespace AutoCSer
{
    /// <summary>
    /// 内存字符流
    /// </summary>
    public sealed unsafe partial class CharStream
    {
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe void WriteJson(bool value)
        {
            Write(value ? '1' : '0');
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe void WriteJson(decimal value)
        {
            SimpleWriteNotNull(value.ToString());
        }
        /// <summary>
        /// 输出 JSON 字符串，不处理转义符
        /// </summary>
        /// <param name="value">字符串</param>
        public void CopyJsonNotNull(string value)
        {
            PrepLength(value.Length + 2);
            UnsafeWrite('"');
            UnsafeWrite(value);
            UnsafeWrite('"');
        }
        /// <summary>
        /// 写入 JSON 字符串
        /// </summary>
        /// <param name="value">不能为 null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteJson(string value)
        {
            fixed (char* valueFixed = value) WriteJson(valueFixed, value.Length, ' ');
        }
        /// <summary>
        /// 写入 JSON 字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteJson(SubString value)
        {
            fixed (char* valueFixed = value.String) WriteJson(valueFixed + value.Start, value.Length, ' ');
        }
        //XXX
        ///// <summary>
        ///// 对象转换成JSON字符串
        ///// </summary>
        ///// <param name="value">#!转换URL</param>
        //public void WriteJson(AutoCSer.WebView.HashUrl value)
        //{
        //    Write('"');
        //    if (!string.IsNullOrEmpty(value.Path)) WriteJson(value.Path);
        //    if (!string.IsNullOrEmpty(value.Query))
        //    {
        //        *(int*)GetPrepSizeCurrent(2) = '#' + ('!' << 16);
        //        ByteSize += 2 * sizeof(char);
        //        WriteJson(value.Query);
        //    }
        //    Write('"');
        //}
        /// <summary>
        /// 模拟javascript解码函数unescape
        /// </summary>
        /// <param name="value">原字符串,长度必须大于 0</param>
        public void JavascriptUnescape(SubArray<byte> value)
        {
            fixed (byte* valueFixed = value.Array)
            {
                PrepLength(value.Length + 2);
                byte* start = valueFixed + value.Start, end = start + value.Length;
                while (start != end && *start != '%')
                {
                    UnsafeWrite(*start == 0 ? ' ' : (char)*start);
                    ++start;
                }
                if (start != end) javascriptUnescape(start, end);
            }
        }
        /// <summary>
        /// 模拟javascript解码函数unescape
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void javascriptUnescape(byte* start, byte* end)
        {
        NEXT:
            if (*++start == 'u')
            {
                uint code = (uint)(*++start - '0'), number = (uint)(*++start - '0');
                if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
                if (number > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
                code <<= 12;
                code += (number << 8);
                if ((number = (uint)(*++start - '0')) > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
                code += (number << 4);
                number = (uint)(*++start - '0');
                code += (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number);
                UnsafeWrite(code == 0 ? ' ' : (char)code);
            }
            else
            {
                uint code = (uint)(*start - '0'), number = (uint)(*++start - '0');
                if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
                code = (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number) + (code << 4);
                UnsafeWrite(code == 0 ? ' ' : (char)code);
            }
            while (++start < end)
            {
                if (*start == '%') goto NEXT;
                UnsafeWrite(*start == 0 ? ' ' : (char)*start);
            }
        }
    }
}
