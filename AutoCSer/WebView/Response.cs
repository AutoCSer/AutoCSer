using System;
using System.Text;
using System.Runtime.CompilerServices;

namespace AutoCSer.WebView
{
    /// <summary>
    /// 页面输出
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe struct Response
    {
        /// <summary>
        /// 输出数据流
        /// </summary>
        internal UnmanagedStream Stream;
        /// <summary>
        /// HTML 编码输出数据流
        /// </summary>
        internal UnmanagedStream EncodeStream;
        /// <summary>
        /// 字符编码
        /// </summary>
        internal EncodingCache Encoding;
        /// <summary>
        /// 输出 HTML
        /// </summary>
        /// <param name="html">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteNotNull(byte[] html)
        {
            Stream.WriteNotNull(html);
        }
        /// <summary>
        /// 输出 HTML
        /// </summary>
        /// <param name="html"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(string html)
        {
            Encoding.WriteBytes(html, Stream);
        }
        /// <summary>
        /// 输出 HTML
        /// </summary>
        /// <param name="html"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(SubString html)
        {
            Encoding.WriteBytes(ref html, Stream);
        }
        /// <summary>
        /// 输出 HTML
        /// </summary>
        /// <param name="html"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(ref SubString html)
        {
            Encoding.WriteBytes(ref html, Stream);
        }
        /// <summary>
        /// 输出 HTML
        /// </summary>
        /// <param name="html"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(char html)
        {
            char* start = (char*)Stream.GetPrepSizeCurrent(sizeof(char));
            *start = html;
            Encoding.WriteBytes(start, 1, Stream);
        }
        /// <summary>
        /// 输出 URL
        /// </summary>
        /// <param name="url"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(AutoCSer.WebView.HashUrl url)
        {
            Encoding.WriteBytes(url.Path, Stream);
            if (!string.IsNullOrEmpty(url.Query))
            {
                Encoding.WriteUrlHash(Stream);
                Encoding.WriteBytesNotEmpty(url.Query, Stream);
            }
        }
        /// <summary>
        /// 输出 HTML
        /// </summary>
        /// <param name="html"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteHtml(string html)
        {
            HtmlEncoder.Html.ToHtml(ref this, html);
        }
        /// <summary>
        /// 输出 HTML
        /// </summary>
        /// <param name="html"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteHtml(SubString html)
        {
            HtmlEncoder.Html.ToHtml(ref this, ref html);
        }
        /// <summary>
        /// 输出 HTML
        /// </summary>
        /// <param name="html"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteHtml(ref SubString html)
        {
            HtmlEncoder.Html.ToHtml(ref this, ref html);
        }
        /// <summary>
        /// 输出 HTML
        /// </summary>
        /// <param name="html"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteHtml(char html)
        {
            char* start = (char*)Stream.GetPrepSizeCurrent(sizeof(char));
            *start = html;
            HtmlEncoder.Html.ToHtml(ref this, start, 1);
        }
        /// <summary>
        /// 输出 URL
        /// </summary>
        /// <param name="url"></param>
        public void WriteHtml(AutoCSer.WebView.HashUrl url)
        {
            if (!string.IsNullOrEmpty(url.Path))
            {
                fixed (char* valueFixed = url.Path) HtmlEncoder.Html.ToHtml(ref this, valueFixed, url.Path.Length);
            }
            if (!string.IsNullOrEmpty(url.Query))
            {
                Encoding.WriteUrlHash(Stream);
                fixed (char* valueFixed = url.Query) HtmlEncoder.Html.ToHtml(ref this, valueFixed, url.Query.Length);
            }
        }
        /// <summary>
        /// 输出 HTML
        /// </summary>
        /// <param name="html"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteTextArea(string html)
        {
            HtmlEncoder.TextArea.ToHtml(ref this, html);
        }
        /// <summary>
        /// 输出 HTML
        /// </summary>
        /// <param name="html"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteTextArea(SubString html)
        {
            HtmlEncoder.TextArea.ToHtml(ref this, ref html);
        }
        /// <summary>
        /// 输出 HTML
        /// </summary>
        /// <param name="html"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteTextArea(ref SubString html)
        {
            HtmlEncoder.TextArea.ToHtml(ref this, ref html);
        }
        /// <summary>
        /// 输出 HTML
        /// </summary>
        /// <param name="html"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteTextArea(char html)
        {
            char* start = (char*)Stream.GetPrepSizeCurrent(sizeof(char));
            *start = html;
            HtmlEncoder.TextArea.ToHtml(ref this, start, 1);
        }
        /// <summary>
        /// 输出 URL
        /// </summary>
        /// <param name="url"></param>
        public void WriteTextArea(AutoCSer.WebView.HashUrl url)
        {
            if (!string.IsNullOrEmpty(url.Path))
            {
                fixed (char* valueFixed = url.Path) HtmlEncoder.TextArea.ToHtml(ref this, valueFixed, url.Path.Length);
            }
            if (!string.IsNullOrEmpty(url.Query))
            {
                Encoding.WriteUrlHash(Stream);
                fixed (char* valueFixed = url.Query) HtmlEncoder.TextArea.ToHtml(ref this, valueFixed, url.Query.Length);
            }
        }

        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(byte value)
        {
            Encoding.Write(value, Stream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(sbyte value)
        {
            Encoding.Write(value, Stream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(ushort value)
        {
            Encoding.Write(value, Stream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(short value)
        {
            Encoding.Write(value, Stream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(uint value)
        {
            Encoding.Write(value, Stream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(int value)
        {
            Encoding.Write(value, Stream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(ulong value)
        {
            Encoding.Write(value, Stream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(long value)
        {
            Encoding.Write(value, Stream);
        }
    }
}
