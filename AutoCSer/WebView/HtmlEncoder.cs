using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.WebView
{
    /// <summary>
    /// HTML 编码器
    /// </summary>
    internal unsafe sealed class HtmlEncoder
    {
        /// <summary>
        /// HTML转义字符集合
        /// </summary>
        private readonly uint* htmls;
        /// <summary>
        /// 最大值
        /// </summary>
        private readonly int size;
        /// <summary>
        /// HTML编码器
        /// </summary>
        /// <param name="htmls">HTML转义字符集合</param>
        internal HtmlEncoder(string htmls)
        {
            size = 0;
            foreach (char htmlChar in htmls)
            {
                if (htmlChar > size) size = htmlChar;
            }
            this.htmls = (uint*)Unmanaged.GetStatic64(((++size + 1) & (int.MaxValue - 1)) * sizeof(uint), true);
            foreach (char value in htmls)
            {
                int div = (value * (int)Number.Div10_16Mul) >> Number.Div10_16Shift;
                this.htmls[value] = (uint)(((value - div * 10) << 16) | div | 0x300030);
            }
        }
        /// <summary>
        /// 文本转HTML
        /// </summary>
        /// <param name="response">页面输出</param>
        /// <param name="value">文本值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ToHtml(ref Response response, ref SubString value)
        {
            if (value.Length != 0)
            {
                fixed (char* valueFixed = value.String) ToHtml(ref response, valueFixed + value.Start, value.Length);
            }
        }
        /// <summary>
        /// 文本转HTML
        /// </summary>
        /// <param name="response">页面输出</param>
        /// <param name="value">文本值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ToHtml(ref Response response, string value)
        {
            if (value.Length != 0)
            {
                fixed (char* valueFixed = value) ToHtml(ref response, valueFixed, value.Length);
            }
        }
        /// <summary>
        /// 文本转HTML
        /// </summary>
        /// <param name="response"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        internal void ToHtml(ref Response response, char* start, int length)
        {
            char* end = start + length;
            int count = encodeCount(start, end);
            if (count == 0)
            {
                response.Encoding.WriteBytes(start, length, response.Stream);
                return;
            }
            UnmanagedStream stream = response.EncodeStream;
            length += count << 2;
            stream.PrepLength(length << 1);
            toHtml(start, end, (char*)stream.CurrentData);
            response.Encoding.WriteBytes(stream.Data.Char, length, response.Stream);
        }
        /// <summary>
        /// 计算编码字符数量
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        /// <returns>编码字符数量</returns>
        private int encodeCount(char* start, char* end)
        {
            int count = 0;
            while (start != end)
            {
                if (*start < size && htmls[*start] != 0) ++count;
                ++start;
            }
            return count;
        }
        /// <summary>
        /// HTML转义
        /// </summary>
        /// <param name="start">起始位置</param>
        /// <param name="end">结束位置</param>
        /// <param name="write">写入位置</param>
        private unsafe void toHtml(char* start, char* end, char* write)
        {
            while (start != end)
            {
                char code = *start++;
                if (code < size)
                {
                    uint html = htmls[code];
                    if (html == 0) *write++ = code;
                    else
                    {
                        *(int*)write = '&' + ('#' << 16);
                        write += 2;
                        *(uint*)write = html;
                        write += 2;
                        *write++ = ';';
                    }
                }
                else *write++ = code;
            }
        }

        /// <summary>
        /// 默认HTML编码器
        /// </summary>
        internal readonly static HtmlEncoder Html = new HtmlEncoder(@"& <>""'");
        /// <summary>
        /// 默认HTML编码器
        /// </summary>
        internal readonly static HtmlEncoder TextArea = new HtmlEncoder(@"&<>");
    }
}
