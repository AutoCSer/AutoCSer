using System;
using System.Runtime.CompilerServices;
using System.Text;
using AutoCSer.Extension;

namespace AutoCSer
{
    /// <summary>
    /// 编码类型
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct EncodingCache
    {
        /// <summary>
        /// 字符编码
        /// </summary>
        internal Encoding Encoding;
        /// <summary>
        /// 编码类型
        /// </summary>
        internal int Type;
        /// <summary>
        /// 是否 ASCII
        /// </summary>
        internal int IsAscii
        {
            get { return Type & 1; }
        }
        /// <summary>
        /// 是否兼容 ASCII
        /// </summary>
        internal int IsAsciiOther
        {
            get { return Type & 2; }
        }
        /// <summary>
        /// 是否 Unicode
        /// </summary>
        internal int IsUnicode
        {
            get { return Type & 4; }
        }
        /// <summary>
        /// 编码类型
        /// </summary>
        /// <param name="encoding"></param>
        internal EncodingCache(Encoding encoding)
        {
            Encoding = encoding;
            if (encoding.CodePage == Utf8.Encoding.CodePage) Type = Utf8.Type;
            else if (encoding.CodePage == Ascii.Encoding.CodePage) Type = Ascii.Type;
            else if (encoding.CodePage == Unicode.Encoding.CodePage) Type = Unicode.Type;
            else if (EncodingCacheOther.Gb2312.Encoding != null && encoding.CodePage == EncodingCacheOther.Gb2312.Encoding.CodePage) Type = EncodingCacheOther.Gb2312.Type;
            else if (EncodingCacheOther.Gbk.Encoding != null && encoding.CodePage == EncodingCacheOther.Gbk.Encoding.CodePage) Type = EncodingCacheOther.Gbk.Type;
            else if (EncodingCacheOther.Gb18030.Encoding != null && encoding.CodePage == EncodingCacheOther.Gb18030.Encoding.CodePage) Type = EncodingCacheOther.Gb18030.Type;
            else if (EncodingCacheOther.Big5.Encoding != null && encoding.CodePage == EncodingCacheOther.Big5.Encoding.CodePage) Type = EncodingCacheOther.Big5.Type;
            else Type = 0;
        }
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        internal string GetString(byte[] buffer, int index, int length)
        {
            if (length == 0) return string.Empty;
            if ((Type & (1 | 4)) == 0) return Encoding.GetString(buffer, index, length);
            if (IsUnicode != 0)
            {
                string text = AutoCSer.Extension.StringExtension.FastAllocateString(length >> 1);
                fixed (char* textFixed = text)
                fixed (byte* bufferFixed = buffer)
                {
                    Memory.CopyNotNull(bufferFixed + index, textFixed, length);
                }
                return text;
            }
            fixed (byte* bufferFixed = buffer) return AutoCSer.Memory.ToString(bufferFixed + index, length);
        }
        /// <summary>
        /// 获取字节数据
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        internal byte[] GetBytesNotEmpty(string text)
        {
            if ((Type & (1 | 4)) == 0) return Encoding.GetBytes(text);
            int length = text.Length;
            if ((Type & 4) != 0)
            {
                byte[] buffer = new byte[length <<= 1];
                fixed (char* textFixed = text)
                fixed (byte* bufferFixed = buffer)
                {
                    Memory.CopyNotNull(textFixed, bufferFixed, length);
                }
                return buffer;
            }
            fixed (char* textFixed = text) return AutoCSer.Extension.StringExtension.GetBytes(textFixed, length);
        }
        /// <summary>
        /// 获取字节数据
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal byte[] GetBytesNotNull(string text)
        {
            return text.Length == 0 ? NullValue<byte>.Array : GetBytesNotEmpty(text);
        }
        /// <summary>
        /// 写字节数据
        /// </summary>
        /// <param name="text"></param>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal int WriteBytesNotEmpty(string text, byte[] buffer, int index)
        {
            int length = text.Length;
            if ((Type & (1 | 4)) == 0) return Encoding.GetBytes(text, 0, length, buffer, index);
            fixed (char* textFixed = text)
            fixed (byte* bufferFixed = buffer)
            {
                if ((Type & 4) != 0) Memory.CopyNotNull(textFixed, bufferFixed + index, length <<= 1);
                else AutoCSer.Extension.StringExtension.WriteBytes(textFixed, length, bufferFixed + index);
            }
            return length;
        }
        /// <summary>
        /// 写字节数据
        /// </summary>
        /// <param name="text"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal int WriteBytesNotEmpty(string text, byte[] buffer)
        {
            int length = text.Length;
            if ((Type & (1 | 4)) == 0) return Encoding.GetBytes(text, 0, length, buffer, 0);
            fixed (char* textFixed = text)
            fixed (byte* bufferFixed = buffer)
            {
                if ((Type & 4) != 0) Memory.CopyNotNull(textFixed, bufferFixed, length <<= 1);
                else AutoCSer.Extension.StringExtension.WriteBytes(textFixed, length, bufferFixed);
            }
            return length;
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
        internal void WriteBytesNotEmpty(string value, UnmanagedStream stream)
        {
            fixed (char* valueFixed = value) WriteBytes(valueFixed, value.Length, stream);
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
                fixed (char* valueFixed = value.String) WriteBytes(valueFixed + value.Start, value.Length, stream);
            }
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
            byte* write = stream.GetPrepSizeCurrent(size);
            if ((Type & (1 | 4)) == 0) Encoding.GetBytes(start, count, write, size);
            else if ((Type & 4) != 0) Memory.CopyNotNull(start, write, size);
            else AutoCSer.Extension.StringExtension.WriteBytes(start, size, write);
            stream.ByteSize += size;
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
                if ((Type & (1 | 4)) == 0) Encoding.GetBytes(charStream.Char, charStream.ByteSize >> 1, bufferFixed, buffer.Length);
                else if ((Type & 4) != 0) Memory.CopyNotNull(charStream.Char, bufferFixed, buffer.Length);
                else AutoCSer.Extension.StringExtension.WriteBytes(charStream.Char, buffer.Length, bufferFixed);
            }
        }
        /// <summary>
        /// 写字节数据
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="buffer"></param>
        internal void WriteBytes(CharStream charStream, ref SubArray<byte> buffer)
        {
            fixed (byte* bufferFixed = buffer.Array)
            {
                if ((Type & (1 | 4)) == 0) Encoding.GetBytes(charStream.Char, charStream.ByteSize >> 1, bufferFixed + buffer.Start, buffer.Length);
                else if ((Type & 4) != 0) Memory.CopyNotNull(charStream.Char, bufferFixed + buffer.Start, buffer.Length);
                else AutoCSer.Extension.StringExtension.WriteBytes(charStream.Char, buffer.Length, bufferFixed + buffer.Start);
            }
        }
        /// <summary>
        /// 获取字节数量
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int GetByteCountNotNull(string text)
        {
            if ((Type & (1 | 4)) == 0) return Encoding.GetByteCount(text);
            return (Type & 1) == 0 ? text.Length << 1 : text.Length;
        }
        /// <summary>
        /// 获取字节数量
        /// </summary>
        /// <param name="charStream"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int GetByteCountNotNull(CharStream charStream)
        {
            if ((Type & (1 | 4)) == 0) return Encoding.GetByteCount(charStream.Char, charStream.ByteSize >> 1);
            return (Type & 1) == 0 ? charStream.ByteSize : (charStream.ByteSize >> 1);
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
            if ((Type & (1 | 4)) == 0) return Encoding.GetByteCount(start, count);
            return (Type & 1) == 0 ? count << 1 : count;
        }

        /// <summary>
        /// 写字节数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private int writeNumberNotUnicode(char* start, int count)
        {
            if ((Type & 2) != 0)
            {
                AutoCSer.Extension.StringExtension.WriteBytes(start, count, (byte*)start);
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
            int count = AutoCSer.Extension.Number.ToString(value, write);
            stream.ByteSize += (Type & 4) == 0 ? writeNumberNotUnicode(write, count) : (count * sizeof(char));
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
            int count = AutoCSer.Extension.Number.ToString(value, write);
            stream.ByteSize += (Type & 4) == 0 ? writeNumberNotUnicode(write, count) : (count * sizeof(char));
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
            int count = AutoCSer.Extension.Number.ToString(value, write);
            stream.ByteSize += (Type & 4) == 0 ? writeNumberNotUnicode(write, count) : (count * sizeof(char));
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
            int count = AutoCSer.Extension.Number.ToString(value, write);
            stream.ByteSize += (Type & 4) == 0 ? writeNumberNotUnicode(write, count) : (count * sizeof(char));
        }
        /// <summary>
        /// 写数值
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="stream"></param>
        internal void Write(uint value, UnmanagedStream stream)
        {
            if ((Type & 4) == 0)
            {
                if ((Type & 2) != 0)
                {
                    if (value < 10) stream.Write((byte)((int)value + '0'));
                    else stream.ByteSize += AutoCSer.Extension.Number.ToBytes(value, stream.GetPrepSizeCurrent(10));
                }
                else
                {
                    char* write = (char*)stream.GetPrepSizeCurrent(10 * sizeof(char));
                    int count = AutoCSer.Extension.Number.ToString(value, write);
                    int size = Encoding.GetByteCount(write, count);
                    Encoding.GetBytes(write, count, (byte*)write, size);
                    stream.ByteSize += size;
                }
            }
            else stream.ByteSize += AutoCSer.Extension.Number.ToString(value, (char*)stream.GetPrepSizeCurrent(10 * sizeof(char))) * sizeof(char);
        }
        /// <summary>
        /// 写数值
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="stream"></param>
        internal void Write(int value, UnmanagedStream stream)
        {
            if ((Type & 4) == 0)
            {
                if ((Type & 2) != 0)
                {
                    if ((uint)value < 10) stream.Write((byte)(value + '0'));
                    else stream.ByteSize += AutoCSer.Extension.Number.ToBytes(value, stream.GetPrepSizeCurrent(11));
                }
                else
                {
                    char* write = (char*)stream.GetPrepSizeCurrent(11 * sizeof(char));
                    int count = AutoCSer.Extension.Number.ToString(value, write);
                    int size = Encoding.GetByteCount(write, count);
                    Encoding.GetBytes(write, count, (byte*)write, size);
                    stream.ByteSize += size;
                }
            }
            else stream.ByteSize += AutoCSer.Extension.Number.ToString(value, (char*)stream.GetPrepSizeCurrent(11 * sizeof(char))) * sizeof(char);
        }
        /// <summary>
        /// 写数值
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="stream"></param>
        internal void Write(ulong value, UnmanagedStream stream)
        {
            if ((Type & 4) == 0)
            {
                if ((Type & 2) != 0)
                {
                    if (value < 10) stream.Write((byte)((int)value + '0'));
                    else
                    {
                        byte* write = stream.GetPrepSizeCurrent(24);
                        RangeLength index = AutoCSer.Extension.Number.ToBytes(value, write);
                        if (index.Start != 0) AutoCSer.Memory.SimpleCopyNotNull64(write + index.Start, write, index.Length);
                        stream.ByteSize += index.Length;
                    }
                }
                else
                {
                    char* write = (char*)stream.GetPrepSizeCurrent(20 * sizeof(char));
                    RangeLength index = AutoCSer.Extension.Number.ToString(value, write);
                    char* start = write + index.Start;
                    int size = Encoding.GetByteCount(start, index.Length);
                    Encoding.GetBytes(start, index.Length, (byte*)write, size);
                    stream.ByteSize += size;
                }
            }
            else
            {
                char* write = (char*)stream.GetPrepSizeCurrent(20 * sizeof(char));
                RangeLength index = AutoCSer.Extension.Number.ToString(value, write);
                if (index.Start != 0) AutoCSer.Memory.SimpleCopyNotNull64((byte*)(write + index.Start), (byte*)write, index.Length << 1);
                stream.ByteSize += (index.Length * sizeof(char));
            }
        }
        /// <summary>
        /// 写数值
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="stream"></param>
        internal void Write(long value, UnmanagedStream stream)
        {
            char* write = (char*)stream.GetPrepSizeCurrent(22 * sizeof(char));
            RangeLength index = AutoCSer.Extension.Number.ToString(value, write);
            if ((Type & 4) == 0)
            {
                char* start = write + index.Start;
                if ((Type & 2) != 0)
                {
                    AutoCSer.Extension.StringExtension.WriteBytes(start, index.Length, (byte*)write);
                    stream.ByteSize += index.Length;
                }
                else
                {
                    int size = Encoding.GetByteCount(start, index.Length);
                    Encoding.GetBytes(start, index.Length, (byte*)write, size);
                    stream.ByteSize += size;
                }
            }
            else
            {
                if (index.Start != 0) AutoCSer.Memory.SimpleCopyNotNull64((byte*)(write + index.Start), (byte*)write, index.Length << 1);
                stream.ByteSize += (index.Length * sizeof(char));
            }
        }
        /// <summary>
        /// URL 哈希字符
        /// </summary>
        /// <param name="stream"></param>
        internal void WriteUrlHash(UnmanagedStream stream)
        {
            if ((Type & 2) != 0) stream.Write(*urlHashAscii.Short);
            else if ((Type & 4) != 0) stream.Write(*urlHashUnicode.Int);
            else
            {
                int size = Encoding.GetByteCount(urlHashUnicode.Char, 2);
                byte* write = stream.GetPrepSizeCurrent(size);
                Encoding.GetBytes(urlHashUnicode.Char, 2, write, size);
                stream.ByteSize += size;
            }
        }

        /// <summary>
        /// UTF8 编码
        /// </summary>
        public static readonly EncodingCache Utf8 = new EncodingCache { Encoding = Encoding.UTF8, Type = 2 };
        /// <summary>
        /// ASCII 编码
        /// </summary>
        public static readonly EncodingCache Ascii = new EncodingCache { Encoding = Encoding.ASCII, Type = 1 | 2 };
        /// <summary>
        /// Unicode 编码
        /// </summary>
        public static readonly EncodingCache Unicode = new EncodingCache { Encoding = Encoding.Unicode, Type = 4 };
        /// <summary>
        /// URL 哈希字符
        /// </summary>
        private static Pointer urlHashUnicode;
        /// <summary>
        /// URL 哈希字符
        /// </summary>
        private static Pointer urlHashAscii;
        static EncodingCache()
        {
            urlHashUnicode = new Pointer { Data = Unmanaged.GetStatic64(8, false) };
            *urlHashUnicode.Int = '#' + ('!' << 16);
            urlHashAscii = new Pointer { Data = urlHashUnicode.Byte + (sizeof(char) * 2) };
            *urlHashAscii.Short = '#' + ('!' << 8);
        }
    }
    /// <summary>
    /// 编码类型
    /// </summary>
    public static class EncodingCacheOther
    {
        /// <summary>
        /// gb2312 编码
        /// </summary>
        internal static readonly EncodingCache Gb2312 = new EncodingCache { Encoding = GetEncoding("GB2312"), Type = 2 };
        /// <summary>
        /// gb2312 编码
        /// </summary>
        public static Encoding GB2312
        {
            get { return Gb2312.Encoding; }
        }
        /// <summary>
        /// gb18030 编码
        /// </summary>
        internal static readonly EncodingCache Gb18030 = new EncodingCache { Encoding = GetEncoding("GB18030"), Type = 2 };
        /// <summary>
        /// gbk 编码
        /// </summary>
        internal static readonly EncodingCache Gbk = new EncodingCache { Encoding = GetEncoding("GBK"), Type = 2 };
        /// <summary>
        /// big5 编码
        /// </summary>
        internal static readonly EncodingCache Big5 = new EncodingCache { Encoding = GetEncoding("BIG5"), Type = 2 };
        /// <summary>
        /// 获取字符编码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static Encoding GetEncoding(string name)
        {
            try
            {
                return Encoding.GetEncoding(name);
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
            }
            return null;
        }
#if DotNetStandard
        static EncodingCacheOther()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
#endif
    }
}
