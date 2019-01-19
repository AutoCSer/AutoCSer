using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 响应 Cookie
    /// </summary>
    public unsafe sealed partial class Cookie : AutoCSer.Threading.Link<Cookie>
    {
        /// <summary>
        /// 名称
        /// </summary>
        internal byte[] Name;
        /// <summary>
        /// 值
        /// </summary>
        internal byte[] Value;
        /// <summary>
        /// 有效域名
        /// </summary>
        internal SubArray<byte> Domain;
        /// <summary>
        /// 有效路径
        /// </summary>
        internal byte[] Path;
        /// <summary>
        /// 超时时间
        /// </summary>
        internal DateTime Expires;
        /// <summary>
        /// 是否安全
        /// </summary>
        internal bool IsSecure;
        /// <summary>
        /// 是否 HTTP Only
        /// </summary>
        internal bool IsHttpOnly;
        /// <summary>
        /// 设置 Cookie
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="domain">有效域名</param>
        /// <param name="path">有效路径</param>
        /// <param name="expires">超时时间</param>
        /// <param name="isSecure">是否安全</param>
        /// <param name="isHttpOnly">是否 HTTP Only</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(byte[] value, DateTime expires, byte[] domain, byte[] path, bool isSecure, bool isHttpOnly)
        {
            Value = value;
            Domain.Set(domain);
            Path = path;
            Expires = expires;
            IsSecure = isSecure;
            IsHttpOnly = isHttpOnly;
        }
        /// <summary>
        /// 设置 Cookie
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="domain">有效域名</param>
        /// <param name="path">有效路径</param>
        /// <param name="expires">超时时间</param>
        /// <param name="isSecure">是否安全</param>
        /// <param name="isHttpOnly">是否 HTTP Only</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(byte[] value, DateTime expires, SubArray<byte> domain, byte[] path, bool isSecure, bool isHttpOnly)
        {
            Value = value;
            Domain = domain;
            Path = path;
            Expires = expires;
            IsSecure = isSecure;
            IsHttpOnly = isHttpOnly;
        }
        /// <summary>
        /// 获取 Cookie 字节数
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal Cookie GetSize(ref int index)
        {
            int size = setCookieSize + Name.Length + 3 + Value.Length;
            if (Domain.Length != 0) size += cookieDomainSize + Domain.Length;
            if (Path != null) size += cookiePathSize + Path.Length;
            if (Expires != DateTime.MinValue) size += cookieExpiresSize + Date.ToByteLength;
            if (IsSecure) size += cookieSecureSize;
            if (IsHttpOnly) size += cookieHttpOnlySize;
            index += size;
            return LinkNext;
        }
        /// <summary>
        /// Cookie 数据写入缓冲区
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        internal Cookie Write(ref byte* start)
        {
            writeSetCookie(start);
            byte* write = start + setCookieSize;
            int index = Name.Length;
            Memory.SimpleCopyNotNull64(Name, write, index);
            write += index;
            Name = null;
            *write++ = (byte)'=';
            if ((index = Value.Length) != 0)
            {
                Memory.SimpleCopyNotNull64(Value, write, index);
                Value = null;
                write += index;
            }
            if ((index = Domain.Length) != 0)
            {
                writeCookieDomain(write);
                fixed (byte* domainFixed = Domain.Array) Memory.SimpleCopyNotNull64(domainFixed + Domain.Start, write += cookieDomainSize, index);
                Domain.SetNull();
                write += index;
            }
            if (Path != null)
            {
                writeCookiePath(write);
                write += cookiePathSize;
                if ((index = Path.Length) != 0)
                {
                    Memory.SimpleCopyNotNull64(Path, write, index);
                    write += index;
                }
                Path = null;
            }
            if (Expires != DateTime.MinValue)
            {
                writeCookieExpires(write);
                write += cookieExpiresSize;
                if (Expires == Pub.MinTime) writeMinTimeCookieExpires(write);
                else Date.ToBytes(Expires, write);
                write += Date.ToByteLength;
            }
            if (IsSecure)
            {
                writeCookieSecure(write);
                write += cookieSecureSize;
            }
            if (IsHttpOnly)
            {
                writeCookieHttpOnly(write);
                write += cookieHttpOnlySize;
            }
            *(short*)write = 0x0a0d;
            start = write + sizeof(short);
            return LinkNext;
        }

        /// <summary>
        /// 默认路径
        /// </summary>
        internal static readonly byte[] DefaultPath = new byte[] { (byte)'/' };
        /// <summary>
        /// HTTP响应输出Cookie名称
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void writeSetCookie(byte* buffer)
        {//Set-Cookie
            *(int*)buffer = 'S' + ('e' << 8) + ('t' << 16) + ('-' << 24);
            *(int*)(buffer + sizeof(int)) = 'C' + ('o' << 8) + ('o' << 16) + ('k' << 24);
            *(int*)(buffer + sizeof(int) * 2) = 'i' + ('e' << 8) + (':' << 16) + (' ' << 24);
        }
        /// <summary>
        /// HTTP响应输出Cookie名称数据长度
        /// </summary>
        private const int setCookieSize = sizeof(int) * 3;
        /// <summary>
        /// Cookie域名
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void writeCookieDomain(byte* buffer)
        {//; Domain=
            *(int*)buffer = ';' + (' ' << 8) + ('D' << 16) + ('o' << 24);
            *(int*)(buffer + sizeof(int)) = 'm' + ('a' << 8) + ('i' << 16) + ('n' << 24);
            *(buffer + sizeof(int) * 2) = (byte)'=';
        }
        /// <summary>
        /// Cookie域名数据长度
        /// </summary>
        private const int cookieDomainSize = sizeof(int) * 2 + 1;
        /// <summary>
        /// Cookie有效路径
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void writeCookiePath(byte* buffer)
        {//; Path=
            *(int*)buffer = ';' + (' ' << 8) + ('P' << 16) + ('a' << 24);
            *(int*)(buffer + sizeof(int)) = 't' + ('h' << 8) + ('=' << 16);
        }
        /// <summary>
        /// Cookie有效路径数据长度
        /// </summary>
        private const int cookiePathSize = sizeof(int) + 3;
        /// <summary>
        /// Cookie域名
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void writeCookieExpires(byte* buffer)
        {//; Expires=
            *(int*)buffer = ';' + (' ' << 8) + ('E' << 16) + ('x' << 24);
            *(int*)(buffer + sizeof(int)) = 'p' + ('i' << 8) + ('r' << 16) + ('e' << 24);
            *(short*)(buffer + sizeof(int) * 2) = 's' + ('=' << 8);
        }
        /// <summary>
        /// Cookie域名数据长度
        /// </summary>
        private const int cookieExpiresSize = sizeof(int) * 2 + sizeof(short);
        /// <summary>
        /// Cookie安全
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void writeCookieSecure(byte* buffer)
        {//; Secure
            *(int*)buffer = ';' + (' ' << 8) + ('S' << 16) + ('e' << 24);
            *(int*)(buffer + sizeof(int)) = 'c' + ('u' << 8) + ('r' << 16) + ('e' << 24);
        }
        /// <summary>
        /// Cookie安全数据长度
        /// </summary>
        private const int cookieSecureSize = sizeof(int) * 2;
        /// <summary>
        /// Cookie是否http only
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void writeCookieHttpOnly(byte* buffer)
        {//; HttpOnly
            *(int*)buffer = ';' + (' ' << 8) + ('H' << 16) + ('t' << 24);
            *(int*)(buffer + sizeof(int)) = 't' + ('p' << 8) + ('O' << 16) + ('n' << 24);
            *(short*)(buffer + sizeof(int) * 2) = 'l' + ('y' << 8);
        }
        /// <summary>
        /// Cookie是否http only数据长度
        /// </summary>
        private const int cookieHttpOnlySize = sizeof(int) * 2 + sizeof(short);
        /// <summary>
        /// Cookie最小时间超时时间
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static void writeMinTimeCookieExpires(byte* buffer)
        {//Mon, 01 Jan 1900 00:00:00 GMT
            *(int*)buffer = 'M' + ('o' << 8) + ('n' << 16) + (',' << 24);
            *(int*)(buffer + sizeof(int)) = ' ' + ('0' << 8) + ('1' << 16) + (' ' << 24);
            *(int*)(buffer + sizeof(int) * 2) = 'J' + ('a' << 8) + ('n' << 16) + (' ' << 24);
            *(int*)(buffer + sizeof(int) * 3) = '1' + ('9' << 8) + ('0' << 16) + ('0' << 24);
            *(int*)(buffer + sizeof(int) * 4) = ' ' + ('0' << 8) + ('0' << 16) + (':' << 24);
            *(int*)(buffer + sizeof(int) * 5) = '0' + ('0' << 8) + (':' << 16) + ('0' << 24);
            *(int*)(buffer + sizeof(int) * 6) = '0' + (' ' << 8) + ('G' << 16) + ('M' << 24);
            *(buffer + sizeof(int) * 7) = (byte)'T';
        }
    }
}
