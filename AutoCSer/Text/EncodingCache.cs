using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace AutoCSer
{
    /// <summary>
    /// 编码缓存
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe struct EncodingCache
    {
        /// <summary>
        /// 字符编码
        /// </summary>
        public readonly Encoding Encoding;
        /// <summary>
        /// 编码类型
        /// </summary>
        internal readonly EncodingType Type;
        /// <summary>
        /// 是否 ASCII
        /// </summary>
        internal EncodingType IsAscii
        {
            get { return Type & EncodingType.Ascii; }
        }
        /// <summary>
        /// 是否兼容 ASCII
        /// </summary>
        internal EncodingType IsCompatibleAscii
        {
            get { return Type & EncodingType.CompatibleAscii; }
        }
        /// <summary>
        /// 是否 Unicode
        /// </summary>
        internal EncodingType IsUnicode
        {
            get { return Type & EncodingType.Unicode; }
        }
        /// <summary>
        /// 编码类型
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="type"></param>
        internal EncodingCache(Encoding encoding, EncodingType type)
        {
            Encoding = encoding;
            Type = type;
        }
        /// <summary>
        /// 编码类型
        /// </summary>
        /// <param name="encodingName"></param>
        /// <param name="type"></param>
        internal EncodingCache(string encodingName, EncodingType type)
        {
            try
            {
                Encoding = Encoding.GetEncoding(encodingName);
                Type = type;
            }
            catch (Exception exception)
            {
                Encoding = new AutoCSer.Text.ExceptionEncoding(exception);
                Type = (EncodingType)0;
            }
        }
        /// <summary>
        /// 编码类型
        /// </summary>
        /// <param name="encoding"></param>
        internal EncodingCache(Encoding encoding)
        {
            Encoding = encoding;
            if (encoding.CodePage == UTF8.Encoding.CodePage) Type = UTF8.Type;
            else if (encoding.CodePage == Unicode.Encoding.CodePage) Type = Unicode.Type;
            else if (encoding.CodePage == Ascii.Encoding.CodePage) Type = Ascii.Type;
            else if (encoding.CodePage == EncodingCacheOther.GB2312.Encoding.CodePage) Type = EncodingCacheOther.GB2312.Type;
            else if (encoding.CodePage == EncodingCacheOther.GBK.Encoding.CodePage) Type = EncodingCacheOther.GBK.Type;
            else if (encoding.CodePage == EncodingCacheOther.GB18030.Encoding.CodePage) Type = EncodingCacheOther.GB18030.Type;
            else if (encoding.CodePage == EncodingCacheOther.BIG5.Encoding.CodePage) Type = EncodingCacheOther.BIG5.Type;
            else if (encoding.CodePage == UTF32.Encoding.CodePage) Type = UTF32.Type;
            else if (encoding.CodePage == BigEndianUnicode.Encoding.CodePage) Type = BigEndianUnicode.Type;
            else Type = (EncodingType)0;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Encoding(EncodingCache value) { return value.Encoding; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator EncodingCache(Encoding value) { return new EncodingCache(value); }
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        internal string GetString(byte[] buffer, int index, int length)
        {
            if (length != 0)
            {
                if ((Type & (EncodingType.Ascii | EncodingType.Unicode)) == 0) return Encoding.GetString(buffer, index, length);
                if (IsUnicode != 0)
                {
                    fixed (byte* bufferFixed = buffer) return new string((char*)bufferFixed, 0, length >> 1);
                }
                fixed (byte* bufferFixed = buffer) return AutoCSer.Memory.Common.ToString(bufferFixed + index, length);
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取字节数据
        /// </summary>
        /// <param name="text">长度必须大于 0</param>
        /// <returns></returns>
        internal byte[] GetBytesNotEmpty(string text)
        {
            if ((Type & (EncodingType.Ascii | EncodingType.Unicode)) == 0) return Encoding.GetBytes(text);
            int length = text.Length;
            if (IsUnicode != 0)
            {
                byte[] buffer = new byte[length <<= 1];
                fixed (byte* bufferFixed = buffer) text.AsSpan().CopyTo(new Span<char>(bufferFixed, length));
                return buffer;
            }
            fixed (char* textFixed = text) return AutoCSer.Extensions.StringExtension.GetBytes(textFixed, length);
        }
        /// <summary>
        /// 获取字节数量
        /// </summary>
        /// <param name="charStream"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int GetByteCountNotNull(CharStream charStream)
        {
            if ((Type & (EncodingType.Ascii | EncodingType.Unicode)) == 0) return Encoding.GetByteCount(charStream.Char, charStream.Data.CurrentIndex >> 1);
            return (Type & EncodingType.Ascii) == 0 ? charStream.Data.CurrentIndex : (charStream.Data.CurrentIndex >> 1);
        }
        /// <summary>
        /// 获取字节数量
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int GetByteCountNotNull(string text)
        {
            if ((Type & (EncodingType.Ascii | EncodingType.Unicode)) == 0) return Encoding.GetByteCount(text);
            return (Type & EncodingType.Ascii) == 0 ? text.Length << 1 : text.Length;
        }
        /// <summary>
        /// 写字节数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private int writeNumberNotUnicode(char* start, int count)
        {
            if ((Type & EncodingType.CompatibleAscii) != 0)
            {
                AutoCSer.Extensions.StringExtension.WriteBytes(start, count, (byte*)start);
                return count;
            }
            int size = Encoding.GetByteCount(start, count);
            Encoding.GetBytes(start, count, (byte*)start, size);
            return size;
        }
        /// <summary>
        /// 写数值
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="stream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(byte value, UnmanagedStream stream)
        {
            char* write = (char*)stream.GetPrepSizeCurrent(3 * sizeof(char));
            int count = AutoCSer.Extensions.NumberExtension.ToString(value, write);
            stream.Data.CurrentIndex += (Type & EncodingType.Unicode) == 0 ? writeNumberNotUnicode(write, count) : (count * sizeof(char));
        }
        /// <summary>
        /// 写数值
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="stream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(sbyte value, UnmanagedStream stream)
        {
            char* write = (char*)stream.GetPrepSizeCurrent(4 * sizeof(char));
            int count = AutoCSer.Extensions.NumberExtension.ToString(value, write);
            stream.Data.CurrentIndex += (Type & EncodingType.Unicode) == 0 ? writeNumberNotUnicode(write, count) : (count * sizeof(char));
        }
        /// <summary>
        /// 写数值
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="stream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(ushort value, UnmanagedStream stream)
        {
            char* write = (char*)stream.GetPrepSizeCurrent(5 * sizeof(char));
            int count = AutoCSer.Extensions.NumberExtension.ToString(value, write);
            stream.Data.CurrentIndex += (Type & EncodingType.Unicode) == 0 ? writeNumberNotUnicode(write, count) : (count * sizeof(char));
        }
        /// <summary>
        /// 写数值
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="stream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(short value, UnmanagedStream stream)
        {
            char* write = (char*)stream.GetPrepSizeCurrent(6 * sizeof(char));
            int count = AutoCSer.Extensions.NumberExtension.ToString(value, write);
            stream.Data.CurrentIndex += (Type & EncodingType.Unicode) == 0 ? writeNumberNotUnicode(write, count) : (count * sizeof(char));
        }
        /// <summary>
        /// 写数值
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="stream"></param>
        internal void Write(uint value, UnmanagedStream stream)
        {
            if ((Type & EncodingType.Unicode) == 0)
            {
                if ((Type & EncodingType.CompatibleAscii) != 0)
                {
                    if (value < 10) stream.Write((byte)((int)value + '0'));
                    else stream.Data.CurrentIndex += AutoCSer.Extensions.NumberExtension.ToBytes(value, stream.GetPrepSizeCurrent(10));
                }
                else
                {
                    char* write = (char*)stream.GetPrepSizeCurrent(10 * sizeof(char));
                    int count = AutoCSer.Extensions.NumberExtension.ToString(value, write);
                    int size = Encoding.GetByteCount(write, count);
                    Encoding.GetBytes(write, count, (byte*)write, size);
                    stream.Data.CurrentIndex += size;
                }
            }
            else stream.Data.CurrentIndex += AutoCSer.Extensions.NumberExtension.ToString(value, (char*)stream.GetPrepSizeCurrent(10 * sizeof(char))) * sizeof(char);
        }
        /// <summary>
        /// 写数值
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="stream"></param>
        internal void Write(int value, UnmanagedStream stream)
        {
            if ((Type & EncodingType.Unicode) == 0)
            {
                if ((Type & EncodingType.CompatibleAscii) != 0)
                {
                    if ((uint)value < 10) stream.Write((byte)(value + '0'));
                    else stream.Data.CurrentIndex += AutoCSer.Extensions.NumberExtension.ToBytes(value, stream.GetPrepSizeCurrent(11));
                }
                else
                {
                    char* write = (char*)stream.GetPrepSizeCurrent(11 * sizeof(char));
                    int count = AutoCSer.Extensions.NumberExtension.ToString(value, write);
                    int size = Encoding.GetByteCount(write, count);
                    Encoding.GetBytes(write, count, (byte*)write, size);
                    stream.Data.CurrentIndex += size;
                }
            }
            else stream.Data.CurrentIndex += AutoCSer.Extensions.NumberExtension.ToString(value, (char*)stream.GetPrepSizeCurrent(11 * sizeof(char))) * sizeof(char);
        }
        /// <summary>
        /// 写数值
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="stream"></param>
        internal void Write(ulong value, UnmanagedStream stream)
        {
            if ((Type & EncodingType.Unicode) == 0)
            {
                if ((Type & EncodingType.CompatibleAscii) != 0)
                {
                    if (value < 10) stream.Write((byte)((int)value + '0'));
                    else
                    {
                        byte* write = stream.GetPrepSizeCurrent(24);
                        RangeLength index = AutoCSer.Extensions.NumberExtension.ToBytes(value, write);
                        if (index.Start != 0) AutoCSer.Memory.Common.SimpleCopyNotNull64(write + index.Start, write, index.Length);
                        stream.Data.CurrentIndex += index.Length;
                    }
                }
                else
                {
                    char* write = (char*)stream.GetPrepSizeCurrent(20 * sizeof(char));
                    int length = AutoCSer.Extensions.NumberExtension.ToString(value, write);
                    int size = Encoding.GetByteCount(write, length);
                    Encoding.GetBytes(write, length, (byte*)write, size);
                    stream.Data.CurrentIndex += size;
                }
            }
            else stream.Data.CurrentIndex += AutoCSer.Extensions.NumberExtension.ToString(value, (char*)stream.GetPrepSizeCurrent(20 * sizeof(char))) << 1;
        }
        /// <summary>
        /// 写数值
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="stream"></param>
        internal void Write(long value, UnmanagedStream stream)
        {
            char* write = (char*)stream.GetPrepSizeCurrent(22 * sizeof(char));
            int length = AutoCSer.Extensions.NumberExtension.ToString(value, write);
            if ((Type & EncodingType.Unicode) == 0)
            {
                if ((Type & EncodingType.CompatibleAscii) != 0)
                {
                    AutoCSer.Extensions.StringExtension.WriteBytes(write, length, (byte*)write);
                    stream.Data.CurrentIndex += length;
                }
                else
                {
                    int size = Encoding.GetByteCount(write, length);
                    Encoding.GetBytes(write, length, (byte*)write, size);
                    stream.Data.CurrentIndex += size;
                }
            }
            else stream.Data.CurrentIndex += length << 1;
        }
        /// <summary>
        /// 获取字节数量
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int GetByteCountNotNull(char* start, int count)
        {
            if ((Type & (EncodingType.Ascii | EncodingType.Unicode)) == 0) return Encoding.GetByteCount(start, count);
            return (Type & EncodingType.Ascii) == 0 ? count << 1 : count;
        }
        /// <summary>
        /// 写字节数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="stream"></param>
        internal void WriteBytes(char* start, int count, UnmanagedStream stream)
        {
            int size = GetByteCountNotNull(start, count);
            byte* write = stream.GetBeforeMove(size);
            if ((Type & (EncodingType.Ascii | EncodingType.Unicode)) == 0) Encoding.GetBytes(start, count, write, size);
            else if ((Type & EncodingType.Unicode) != 0) AutoCSer.Memory.Common.CopyNotNull(start, write, size);
            else AutoCSer.Extensions.StringExtension.WriteBytes(start, size, write);
        }
        /// <summary>
        /// 写字节数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteBytes(string value, UnmanagedStream stream)
        {
            if (!string.IsNullOrEmpty(value))
            {
                fixed (char* valueFixed = value) WriteBytes(valueFixed, value.Length, stream);
            }
        }
        /// <summary>
        /// 写字节数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteBytes(ref SubString value, UnmanagedStream stream)
        {
            if (value.Length != 0)
            {
                fixed (char* valueFixed = value.GetFixedBuffer()) WriteBytes(valueFixed + value.Start, value.Length, stream);
            }
        }
        /// <summary>
        /// 写字节数据
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="buffer"></param>
        internal void WriteBytes(CharStream charStream, byte[] buffer)
        {
            fixed (byte* bufferFixed = buffer)
            {
                if ((Type & (EncodingType.Ascii | EncodingType.Unicode)) == 0) Encoding.GetBytes(charStream.Char, charStream.Data.CurrentIndex >> 1, bufferFixed, buffer.Length);
                else if ((Type & EncodingType.Unicode) != 0) AutoCSer.Memory.Common.CopyNotNull(charStream.Char, bufferFixed, buffer.Length);
                else AutoCSer.Extensions.StringExtension.WriteBytes(charStream.Char, buffer.Length, bufferFixed);
            }
        }
        /// <summary>
        /// 写字节数据
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="buffer"></param>
        internal void WriteBytes(CharStream charStream, ref SubArray<byte> buffer)
        {
            fixed (byte* bufferFixed = buffer.GetFixedBuffer())
            {
                if ((Type & (EncodingType.Ascii | EncodingType.Unicode)) == 0) Encoding.GetBytes(charStream.Char, charStream.Data.CurrentIndex >> 1, bufferFixed + buffer.Start, buffer.Length);
                else if ((Type & EncodingType.Unicode) != 0) AutoCSer.Memory.Common.CopyNotNull(charStream.Char, bufferFixed + buffer.Start, buffer.Length);
                else AutoCSer.Extensions.StringExtension.WriteBytes(charStream.Char, buffer.Length, bufferFixed + buffer.Start);
            }
        }
        /// <summary>
        /// 获取字节数据
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        internal byte[] GetBytesNotNull(string text)
        {
            return text.Length == 0 ? EmptyArray<byte>.Array : GetBytesNotEmpty(text);
        }
        /// <summary>
        /// 写字节数据
        /// </summary>
        /// <param name="text">长度必须大于 0</param>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal int WriteBytesNotEmpty(string text, byte[] buffer, int index = 0)
        {
            int length = text.Length;
            if ((Type & (EncodingType.Ascii | EncodingType.Unicode)) == 0) return Encoding.GetBytes(text, 0, length, buffer, index);
            fixed (byte* bufferFixed = buffer)
            {
                if (IsUnicode != 0) text.AsSpan().CopyTo(new Span<char>(bufferFixed + index, length));
                else
                {
                    fixed (char* textFixed = text) AutoCSer.Extensions.StringExtension.WriteBytes(textFixed, length, bufferFixed + index);
                }
            }
            return length;
        }
        /// <summary>
        /// 写字节数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteBytesNotEmpty(string value, UnmanagedStream stream)
        {
            fixed (char* valueFixed = value) WriteBytes(valueFixed, value.Length, stream);
        }
        /// <summary>
        /// URL 哈希字符
        /// </summary>
        /// <param name="stream"></param>
        internal void WriteUrlHash(UnmanagedStream stream)
        {
            if ((Type & EncodingType.CompatibleAscii) != 0) stream.Write(*urlHashAscii.Short);
            else if ((Type & EncodingType.Unicode) != 0) stream.Write(*urlHashUnicode.Int);
            else
            {
                int size = Encoding.GetByteCount(urlHashUnicode.Char, 2);
                byte* write = stream.GetBeforeMove(size);
                Encoding.GetBytes(urlHashUnicode.Char, 2, write, size);
            }
        }

        /// <summary>
        /// UTF8 编码
        /// </summary>
        public static readonly EncodingCache UTF8 = new EncodingCache(Encoding.UTF8, EncodingType.CompatibleAscii);
        /// <summary>
        /// ASCII 编码
        /// </summary>
        public static readonly EncodingCache Ascii = new EncodingCache(Encoding.ASCII, EncodingType.Ascii | EncodingType.CompatibleAscii);
        /// <summary>
        /// Unicode 编码
        /// </summary>
        public static readonly EncodingCache Unicode = new EncodingCache(Encoding.Unicode, EncodingType.Unicode);
        /// <summary>
        ///  大端 Unicode 编码
        /// </summary>
        public static readonly EncodingCache BigEndianUnicode = new EncodingCache(Encoding.BigEndianUnicode, (EncodingType)0);
        /// <summary>
        ///  UTF32 编码
        /// </summary>
        public static readonly EncodingCache UTF32 = new EncodingCache(Encoding.UTF32, (EncodingType)0);
        /// <summary>
        /// GB2312 编码
        /// </summary>
        public static EncodingCache GB2312 { get { return EncodingCacheOther.GB2312; } }
        /// <summary>
        /// GB18030 编码
        /// </summary>
        public static EncodingCache GB18030 { get { return EncodingCacheOther.GB18030; } }
        /// <summary>
        /// GBK 编码
        /// </summary>
        public static EncodingCache GBK { get { return EncodingCacheOther.GBK; } }
        /// <summary>
        /// BIG5 编码
        /// </summary>
        public static EncodingCache BIG5 { get { return EncodingCacheOther.BIG5; } }
        /// <summary>
        /// URL 哈希字符
        /// </summary>
        private static AutoCSer.Memory.Pointer urlHashUnicode;
        /// <summary>
        /// URL 哈希字符
        /// </summary>
        private static AutoCSer.Memory.Pointer urlHashAscii;
        ///// <summary>
        ///// URL 哈希字符
        ///// </summary>
        //private static Pointer urlHashUnicode;
        ///// <summary>
        ///// URL 哈希字符
        ///// </summary>
        //private static Pointer urlHashAscii;
        static EncodingCache()
        {
            urlHashUnicode = Unmanaged.GetStaticPointer(8, false);
            *urlHashUnicode.Int = '#' + ('!' << 16);
            urlHashAscii = new Pointer { Data = urlHashUnicode.Byte + (sizeof(char) * 2) };
            *urlHashAscii.Short = '#' + ('!' << 8);
        }
    }
}
