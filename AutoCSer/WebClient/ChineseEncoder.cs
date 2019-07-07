using System;
using System.Text;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 汉字编码检测
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe struct ChineseEncoder
    {
        /// <summary>
        /// UTF32文件BOM
        /// </summary>
        private static readonly uint utf32Bom = AutoCSer.IO.FileBom.Get(Encoding.UTF32).Bom;
        /// <summary>
        /// UTF8文件BOM
        /// </summary>
        private static readonly uint utf8Bom = AutoCSer.IO.FileBom.Get(Encoding.UTF8).Bom;
        /// <summary>
        /// Unicode文件BOM
        /// </summary>
        private static readonly uint unicodeBom = AutoCSer.IO.FileBom.Get(Encoding.Unicode).Bom;
        /// <summary>
        /// 大端Unicode文件BOM
        /// </summary>
        private static readonly uint bigEndianUnicodeBom = AutoCSer.IO.FileBom.Get(Encoding.BigEndianUnicode).Bom;
        /// <summary>
        /// 当前位置
        /// </summary>
        private byte* start;
        /// <summary>
        /// 结束位置
        /// </summary>
        private byte* end;
        /// <summary>
        /// 第一个非ASCII码位置
        /// </summary>
        private byte* noAscii;
        /// <summary>
        /// GB2312命中数量
        /// </summary>
        private int gb2312Count;
        /// <summary>
        /// GBK命中数量
        /// </summary>
        private int gbkCount;
        /// <summary>
        /// GB18030命中数量
        /// </summary>
        private int gb18030Count;
        /// <summary>
        /// UTF8命中数量
        /// </summary>
        private int utf8Count;
        /// <summary>
        /// BIG5命中数量
        /// </summary>
        private int big5Count;
        /// <summary>
        /// UTF16命中数量
        /// </summary>
        private int utf16Count;
        /// <summary>
        /// 大端UTF16命中数量
        /// </summary>
        private int bigUtf16Count;
        /// <summary>
        /// UTF32命中数量
        /// </summary>
        private int utf32Count;
        ///// <summary>
        ///// 大端UTF32命中数量
        ///// </summary>
        //private int bigUtf32Count;
        /// <summary>
        /// 最后一个字符
        /// </summary>
        private int endValue;
        /// <summary>
        /// GB18030匹配表
        /// </summary>
        private char* gb18030Char;
        /// <summary>
        /// UTF16匹配表
        /// </summary>
        private char* utf16Char;
        /// <summary>
        /// 大端TF16匹配表
        /// </summary>
        private char* bigUtf16Char;
        //的是不了个，。
        //的是不了個，。
        /// <summary>
        /// 获取编码
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>编码,失败为null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Encoding Get(byte[] data)
        {
            fixed (byte* dataFixed = data) return get(dataFixed, data.Length);
        }
        /// <summary>
        /// 获取编码
        /// </summary>
        /// <param name="dataFixed">字节数组</param>
        /// <param name="length">字节数量</param>
        /// <returns>编码,失败为null</returns>
        private Encoding get(byte* dataFixed, int length)
        {
            uint bom = *(uint*)dataFixed;
            if (bom == utf32Bom && (length & 3) == 0) return Encoding.UTF32;
            //else if (bom == 0xfffe0000U && (length & 3) == 0) return Encoding.UTF32;//大端
            else if ((bom & 0xffffff) == utf8Bom) return Encoding.UTF8;
            else if ((bom &= 0xffff) == unicodeBom && (length & 1) == 0) return Encoding.Unicode;
            else if (bom == bigEndianUnicodeBom && (length & 1) == 0) return Encoding.BigEndianUnicode;

            #region ASCII
            start = dataFixed;
            end = dataFixed + length;
            endValue = *--end;
            if ((length & 1) == 0)
            {
                if (isAscii())
                {
                    int* counts = stackalloc int[2];
                    for (start = dataFixed, *end = 0, counts[1] = *counts = 0; *start != 0; ++start) ;
                    while (start != end)
                    {
                        ++counts[(uint)start & 1];
                        while (*++start != 0) ;
                    }
                    *end = (byte)endValue;
                    return counts[1] >= *counts ? (counts[1] == 0 ? Encoding.ASCII : Encoding.Unicode) : Encoding.BigEndianUnicode;
                }
                utf16Count = bigUtf16Count = 0;
                //utf32Count = bigUtf32Count = (length & 3) != 0 ? int.MinValue : 0;
                utf32Count = (length & 3) != 0 ? int.MinValue : 0;
            }
            else
            {
                if (isAscii())
                {
                    *end = (byte)endValue;
                    return Encoding.ASCII;
                }
                //utf16Count = bigUtf16Count = utf32Count = bigUtf32Count = int.MinValue;
                utf16Count = bigUtf16Count = utf32Count = int.MinValue;
            }
            noAscii = start;
            #endregion

            #region GB2312
            char* gb18030Chars = stackalloc char[0x40];
            AutoCSer.Memory.Fill((ulong*)(gb18030Char = gb18030Chars), 0UL, 0x40 * sizeof(char) / sizeof(ulong));
            gb18030Char[0] = char.MaxValue;
            gb18030Char[0xc4b5 & 0x3f] = (char)0xc4b5;
            gb18030Char[0xc7ca & 0x3f] = (char)0xc7ca;
            gb18030Char[0xbbb2 & 0x3f] = (char)0xbbb2;
            gb18030Char[0xcbc1 & 0x3f] = (char)0xcbc1;
            gb18030Char[0xf6b8 & 0x3f] = (char)0xf6b8;
            gb18030Char[0xaca3 & 0x3f] = (char)0xaca3;
            gb18030Char[0xa3a1 & 0x3f] = (char)0xa3a1;
            gb18030Char[0x8082 & 0x3f] = (char)0x8082;
            gb2312();
            if (gb2312Count >= 0) gb18030Count = gbkCount = int.MinValue;
            else
            {
                *end++ = (byte)endValue;
                gb18030();
                gbk();
                --end;
            }
            #endregion

            utf8();

            *end++ = (byte)endValue;
            big5();

            if (utf16Count >= 0)
            {
                #region UTF16
                char* utf16Chars = stackalloc char[0x10];
                AutoCSer.Memory.Fill((ulong*)(utf16Char = utf16Chars), 0UL, 0x10 * sizeof(char) / sizeof(ulong));
                utf16Char[0] = char.MaxValue;
                utf16Char[0x7684 & 0xf] = (char)0x7684;
                //utf16Char[0x662f & 0xf] = (char)0x662f;
                //utf16Char[0x4e0d & 0xf] = (char)0x4e0d;
                utf16Char[0x4e86 & 0xf] = (char)0x4e86;
                //utf16Char[0x4e2a & 0xf] = (char)0x4e2a;
                utf16Char[0xff0c & 0xf] = (char)0xff0c;
                utf16Char[0x3002 & 0xf] = (char)0x3002;
                utf16Char[0x500b & 0xf] = (char)0x500b;
                char* bigUtf16Chars = stackalloc char[0x10];
                AutoCSer.Memory.Fill((ulong*)(bigUtf16Char = bigUtf16Chars), 0UL, 0x10 * sizeof(char) / sizeof(ulong));
                bigUtf16Char[0] = char.MaxValue;
                bigUtf16Char[(0x8476 >> 8) & 0xf] = (char)0x8476;
                //bigUtf16Char[(0x2f66 >> 8) & 0xf] = (char)0x2f66;
                //bigUtf16Char[(0x0d4e >> 8) & 0xf] = (char)0x0d4e;
                bigUtf16Char[(0x864e >> 8) & 0xf] = (char)0x864e;
                //bigUtf16Char[(0x2a4e >> 8) & 0xf] = (char)0x2a4e;
                bigUtf16Char[(0x0cff >> 8) & 0xf] = (char)0x0cff;
                bigUtf16Char[(0x0230 >> 8) & 0xf] = (char)0x0230;
                bigUtf16Char[(0x0b50 >> 8) & 0xf] = (char)0x0b50;
                #endregion
                start = dataFixed;
                if (utf32Count >= 0)
                {
                    utf16();
                    start = dataFixed;
                    utf32();
                }
                else
                {
                    end -= 2;
                    utf16();
                    bom = *(ushort*)end;
                    if (utf16Char[bom & 0xf] == bom) ++utf16Count;
                    else if (bigUtf16Char[(bom >> 8) & 0xf] == bom) ++bigUtf16Count;
                }
            }

            Encoding value = null;
            endValue = -1;
            if (gb2312Count > endValue)
            {
                value = AutoCSer.EncodingCacheOther.Gb2312.Encoding;
                endValue = gb2312Count;
            }
            if (gb18030Count > endValue)
            {
                value = AutoCSer.EncodingCacheOther.Gb18030.Encoding;
                endValue = gb18030Count;
            }
            if (gbkCount > endValue)
            {
                value = AutoCSer.EncodingCacheOther.Gbk.Encoding;
                endValue = gbkCount;
            }
            if (utf8Count > endValue)
            {
                value = Encoding.UTF8;
                endValue = utf8Count;
            }
            if (big5Count > endValue)
            {
                value = AutoCSer.EncodingCacheOther.Big5.Encoding;
                endValue = big5Count;
            }
            if (utf32Count > endValue)
            {
                value = Encoding.UTF32;
                endValue = utf32Count;
            }
            //if (bigUtf32 > endValue)
            //{
            //    value = Encoding.UTF32;//大端
            //    endValue = bigUtf32;
            //}
            if (utf16Count > endValue)
            {
                value = Encoding.Unicode;
                endValue = utf16Count;
            }
            if (bigUtf16Count > endValue)
            {
                value = Encoding.BigEndianUnicode;
                endValue = bigUtf16Count;
            }
            return value;
        }
        /// <summary>
        /// 判断是否ASCII(包括结束字符)
        /// </summary>
        /// <returns></returns>
        private bool isAscii()
        {
            *end = 0xff;
            uint code = *(uint*)start;
            while ((code & 0x80808080) == 0) code = *(uint*)(start += 4);
            if (start == end) return (endValue & 0x80) == 0;
            else if ((code & 0x80) == 0)
            {
                if (++start == end) return (endValue & 0x80) == 0;
                else if ((code & 0x8000) == 0)
                {
                    if (++start == end) return (endValue & 0x80) == 0;
                    else if ((code & 0x800000) == 0)
                    {
                        if (++start == end) return (endValue & 0x80) == 0;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 扫描ASCII(不包括结束字符)
        /// </summary>
        private void ascii()
        {
            *end = 0x80;
            uint code = *(uint*)start;
            while ((code & 0x80808080) == 0) code = *(uint*)(start += 4);
            if (start != end && (code & 0x80) == 0
                && ++start != end && (code & 0x8000) == 0
                && ++start != end && (code & 0x800000) == 0)
            {
                ++start;
            }
        }
        /// <summary>
        /// 匹配GB2312
        /// </summary>
        private void gb2312()
        {
            uint code;
            gb2312Count = 0;
            do
            {
                *end = 0;
                for (code = *(uint*)start; (code & 0x80808080) == 0x80808080; code = *(uint*)(start += 4))
                {
                    if ((byte)(code - 0xa1) <= (0xf7 - 0xa1) && (ushort)(code - 0xa100) < (0xfe00 - 0xa100 + 0x100))
                    {
                        if (gb18030Char[(ushort)code & 0x3f] == (ushort)code) ++gb2312Count;
                        else if ((byte)(code - (0xa0 + 10)) <= (15 - 10)
                            || ((byte)code == 0xd7 && (ushort)(code - 0xfad7) <= (0xfed7 - 0xfad7)))
                        {
                            gb2312Count = int.MinValue;
                            return;
                        }
                    }
                    code >>= 16;
                    if ((byte)(code - 0xa1) <= (0xf7 - 0xa1) && (ushort)(code - 0xa100) < (0xfe00 - 0xa100 + 0x100))
                    {
                        if (gb18030Char[code & 0x3f] == code) ++gb2312Count;
                        else if ((byte)(code - (0xa0 + 10)) <= (15 - 10)
                            || ((byte)code == 0xd7 && (ushort)(code - 0xfad7) <= (0xfed7 - 0xfad7)))
                        {
                            gb2312Count = int.MinValue;
                            return;
                        }
                    }
                }
                if ((code & 0x8080) == 0x8080)
                {
                    if ((byte)(code - 0xa1) <= (0xf7 - 0xa1) && (ushort)(code - 0xa100) < (0xfe00 - 0xa100 + 0x100))
                    {
                        if (gb18030Char[(ushort)code & 0x3f] == (ushort)code) ++gb2312Count;
                        else if ((byte)(code - (0xa0 + 10)) <= (15 - 10)
                            || ((byte)code == 0xd7 && (ushort)(code - 0xfad7) <= (0xfed7 - 0xfad7)))
                        {
                            gb2312Count = int.MinValue;
                            return;
                        }
                    }
                    code >>= 16;
                    start += 2;
                }
                if (start != end)
                {
                    if ((code & 0x80) == 0)
                    {
                        if (++start != end)
                        {
                            if ((code & 0x8000) == 0)
                            {
                                ++start;
                                ascii();
                                if (start == end)
                                {
                                    if ((endValue & 0x80) != 0) gb2312Count = int.MinValue;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if ((endValue & 0x80) != 0) gb2312Count = int.MinValue;
                            return;
                        }
                    }
                    else
                    {
                        gb2312Count = int.MinValue;
                        return;
                    }
                }
                else
                {
                    if ((endValue & 0x80) != 0) gb2312Count = int.MinValue;
                    return;
                }
            }
            while (true);
        }
        /// <summary>
        /// 匹配GB18030
        /// </summary>
        private void gb18030()
        {
            uint code;
            start = noAscii;
            gb18030Count = 0;
            while (start != end)
            {
                code = *(ushort*)start;
                if ((code & 0x80) != 0)
                {
                    if ((byte)(code - 0x81) <= (0xfe - 0x81) && (ushort)(code - 0x3000) < (0xfe00 - 0x3000 + 0x100))
                    {
                        if (code >= 0x4000)
                        {
                            if ((code & 0xff00) != 0x7f00 && ++start != end)
                            {
                                ++start;
                                if (gb18030Char[code & 0x3f] == code) ++gb18030Count;
                                continue;
                            }
                        }
                        else if (code < 0x3a00 && ++start != end && ++start != end)
                        {
                            code = *(ushort*)start;
                            if ((byte)(code - 0x81) <= (0xfe - 0x81)
                                && (ushort)(code - 0x3000) < (0x3900 - 0x3000 + 0x100) && ++start != end)
                            {
                                ++start;
                                continue;
                            }
                        }
                    }
                    gb18030Count = int.MinValue;
                    return;
                }
                else if (++start != end && (code & 0x8000) == 0 && ++start != end)
                {
                    --end;
                    ascii();
                    if (start == end)
                    {
                        ++end;
                        if ((endValue & 0x80) != 0) gb18030Count = int.MinValue;
                        return;
                    }
                    else *end++ = (byte)endValue;
                }
            }
        }
        /// <summary>
        /// 匹配GBK
        /// </summary>
        private void gbk()
        {
            uint code;
            start = noAscii;
            gbkCount = 0;
            while (start != end)
            {
                code = *(ushort*)start;
                if ((code & 0x80) != 0)
                {
                    if (code >= 0xa100)
                    {
                        if (code <= 0xfe00)
                        {
                            if ((byte)code >= 0xb0 ? (byte)code <= 0xf7 : (byte)(code - 0x81) <= (0xa9 - 0x81))
                            {
                                if (++start != end)
                                {
                                    ++start;
                                    if (gb18030Char[code & 0x3f] == code) ++gbkCount;
                                    continue;
                                }
                            }
                        }
                    }
                    else if (code >= 0x4000 && (code & 0xff00) != 0x7f00)
                    {
                        if ((byte)code >= 0xa8 ? (byte)code <= 0xfe : (byte)(code - 0x81) <= (0xa0 - 0x81))
                        {
                            if (++start != end)
                            {
                                ++start;
                                continue;
                            }
                        }
                    }
                    gbkCount = int.MinValue;
                    return;
                }
                else if (++start != end && (code & 0x8000) == 0 && ++start != end)
                {
                    --end;
                    ascii();
                    if (start == end)
                    {
                        ++end;
                        if ((endValue & 0x80) != 0) gbkCount = int.MinValue;
                        return;
                    }
                    else *end++ = (byte)endValue;
                }
            }
        }
        /// <summary>
        /// 匹配UTF8
        /// </summary>
        private void utf8()
        {
            uint* utf8Char = stackalloc uint[0x10];
            AutoCSer.Memory.Fill((ulong*)utf8Char, 0UL, 0x10 >> 1);
            utf8Char[0] = uint.MaxValue;
            utf8Char[(0x849ae7 >> 16) & 0xf] = 0x849ae7;
            utf8Char[(0xaf98e6 >> 16) & 0xf] = 0xaf98e6;
            utf8Char[(0x8db8e4 >> 16) & 0xf] = 0x8db8e4;
            utf8Char[(0x86bae4 >> 16) & 0xf] = 0x86bae4;
            utf8Char[(0xaab8e4 >> 16) & 0xf] = 0xaab8e4;
            utf8Char[(0x8cbcef >> 16) & 0xf] = 0x8cbcef;
            utf8Char[(0x8280e3 >> 16) & 0xf] = 0x8280e3;
            utf8Char[(0x8b80e5 >> 16) & 0xf] = 0x8b80e5;
            uint code, code1;
            start = noAscii;
            utf8Count = 0;
            do
            {
                *end = 0;
                for (code = *(uint*)start; (code & 0x80808080) == 0x80808080;)
                {
                    code1 = code;
                    if ((code &= 0xe0) >= 0xe0)
                    {
                        if (code == 0xe0)
                        {
                            if ((code1 & 0xc0c000) == 0x808000)
                            {
                                code = *(uint*)(start += 3);
                                if (utf8Char[(code1 >> 16) & 0xf] == (code1 & 0xffffff)) ++utf8Count;
                                continue;
                            }
                        }
                        else if ((code1 & 0xc0c0c000U) == 0x80808000U)
                        {
                            code = *(uint*)(start += 4);
                            continue;
                        }
                    }
                    else if (code == 0xc0 && (code1 & 0xc000) == 0x8000)
                    {
                        if ((code1 & 0xe00000) == 0xc00000)
                        {
                            if ((code1 & 0xc0000000U) == 0x80000000U)
                            {
                                code = *(uint*)(start += 4);
                                continue;
                            }
                        }
                        else
                        {
                            code = *(uint*)(start += 2);
                            continue;
                        }
                    }
                    utf8Count = int.MinValue;
                    return;
                }
                while ((code & 0x80) != 0)
                {
                    code1 = code;
                    if ((code &= 0xe0) >= 0xe0)
                    {
                        if (code == 0xe0)
                        {
                            if ((code1 & 0xc0c000) == 0x808000)
                            {
                                start += 3;
                                if (utf8Char[(code1 >> 16) & 0xf] == (code1 & 0xffffff)) ++utf8Count;
                                break;
                            }
                            else if ((code1 & 0xffc000) == 0x8000 && (start + 2) == end && (endValue & 0xc0) == 0x80)
                            {
                                code1 |= (uint)endValue << 16;
                                if (utf8Char[(code1 >> 16) & 0xf] == (code1 & 0xffffff)) ++utf8Count;
                                return;
                            }
                        }
                        else if ((code1 & 0xc0c0c000U) == 0x80808000U)
                        {
                            start += 4;
                            break;
                        }
                        else if ((code1 & 0xffc0c000U) == 0x808000 && (start + 3) == end && (endValue & 0xc0) == 0x80)
                        {
                            return;
                        }
                    }
                    else if (code == 0xc0)
                    {
                        if ((code1 & 0xc000) == 0x8000)
                        {
                            start += 2;
                            if ((code1 & 0x800000) == 0) break;
                            if ((code1 & 0xe00000) == 0xc00000 && ++start == end && (endValue & 0xc0) == 0x80)
                            {
                                return;
                            }
                        }
                        else if (++start == end && (endValue & 0xc0) == 0x80) return;
                    }
                    utf8Count = int.MinValue;
                    return;
                }
                ascii();
                if (start == end)
                {
                    if ((endValue & 0x80) != 0) utf8Count = int.MinValue;
                    return;
                }
            }
            while (true);
        }
        /// <summary>
        /// 匹配BIG5
        /// </summary>
        private void big5()
        {
            char* big5Char = stackalloc char[0x40];
            AutoCSer.Memory.Fill((ulong*)big5Char, 0UL, 0x40 * sizeof(char) / sizeof(ulong));
            big5Char[0] = char.MaxValue;
            big5Char[(0xbaaa >> 8) & 0x3f] = (char)0xbaaa;
            big5Char[(0x4fac >> 8) & 0x3f] = (char)0x4fac;
            big5Char[(0xa3a4 >> 8) & 0x3f] = (char)0xa3a4;
            big5Char[(0x46a4 >> 8) & 0x3f] = (char)0x46a4;
            big5Char[(0xd3ad >> 8) & 0x3f] = (char)0xd3ad;
            big5Char[(0x41a1 >> 8) & 0x3f] = (char)0x41a1;
            big5Char[(0x43a1 >> 8) & 0x3f] = (char)0x43a1;
            uint code;
            start = noAscii;
            big5Count = 0;
            while (start != end)
            {
                code = *(ushort*)start;
                if ((code & 0x80) != 0)
                {
                    if ((byte)(code - 0x81) <= (0xfe - 0x81)
                        && (ushort)(code - 0x4000) < (0xfe00 - 0x4000 + 0x100) && (code & 0xff00) != 0x7f00)
                    {
                        if ((byte)code >= 0xa4)
                        {
                            if ((byte)code <= 0xc6)
                            {
                                if (((byte)code != 0xc6 || (code & 0x8000) == 0) && ++start != end)
                                {
                                    ++start;
                                    if (big5Char[(code >> 8) & 0x3f] == code) ++big5Count;
                                    continue;
                                }
                            }
                            else if ((byte)(code - 0xc9) <= (0xf9 - 0xc9) && ++start != end)
                            {
                                ++start;
                                continue;
                            }
                        }
                        else if ((byte)code >= 0xa1)
                        {
                            if (((byte)code != 0xa3 || (code & 0xc000) == 0) && ++start != end)
                            {
                                ++start;
                                if (big5Char[(code >> 8) & 0x3f] == code) ++big5Count;
                                continue;
                            }
                        }
                    }
                    big5Count = int.MinValue;
                    return;
                }
                else if (++start != end && (code & 0x8000) == 0 && ++start != end)
                {
                    --end;
                    ascii();
                    if (start == end)
                    {
                        ++end;
                        if ((endValue & 0x80) != 0) big5Count = int.MinValue;
                        return;
                    }
                    else *end++ = (byte)endValue;
                }
            }
        }
        /// <summary>
        /// 匹配UTF16
        /// </summary>
        private void utf16()
        {
            while (start != end)
            {
                uint code = *(uint*)start;
                if (utf16Char[code & 0xf] == (char)code) ++utf16Count;
                else if (bigUtf16Char[(code >> 8) & 0xf] == (char)code) ++bigUtf16Count;
                code >>= 16;
                if (utf16Char[code & 0xf] == code) ++utf16Count;
                else if (bigUtf16Char[(code >> 8) & 0xf] == code) ++bigUtf16Count;
                start += 4;
            }
        }
        /// <summary>
        /// 匹配UTF32
        /// </summary>
        private void utf32()
        {
            uint* utf32Char = stackalloc uint[0x10];
            AutoCSer.Memory.Fill((ulong*)utf32Char, 0UL, 0x10 >> 1);
            utf32Char[0] = uint.MaxValue;
            utf32Char[0x7684 & 0xf] = 0x7684;
            utf32Char[0x662f & 0xf] = 0x662f;
            //utf32Char[0x4e0d & 0xf] = 0x4e0d;
            utf32Char[0x4e86 & 0xf] = 0x4e86;
            utf32Char[0x4e2a & 0xf] = 0x4e2a;
            utf32Char[0xff0c & 0xf] = 0xff0c;
            utf32Char[0x3002 & 0xf] = 0x3002;
            utf32Char[0x500b & 0xf] = 0x500b;
            //uint* bigUtf32Char = stackalloc uint[0x10];
            //fastCSharp.sys.memory.set((byte*)bigUtf32Char, 0U, 0x10);
            //bigUtf32Char[0] = uint.MaxValue;
            //bigUtf32Char[(0x84760000 >> 24) & 0xf] = 0x84760000;
            //bigUtf32Char[(0x2f660000 >> 24) & 0xf] = 0x2f660000;
            //bigUtf32Char[(0x0d4e0000 >> 24) & 0xf] = 0x0d4e0000;
            //bigUtf32Char[(0x864e0000 >> 24) & 0xf] = 0x864e0000;
            //bigUtf32Char[(0x2a4e0000 >> 24) & 0xf] = 0x2a4e0000;
            //bigUtf32Char[(0x0cff0000 >> 24) & 0xf] = 0x0cff0000;
            //bigUtf32Char[(0x02300000 >> 24) & 0xf] = 0x02300000;
            //bigUtf32Char[(0x0b500000 >> 24) & 0xf] = 0x0b500000;
            while (start != end)
            {
                uint code = *(uint*)start;
                if (utf32Char[code & 0xf] == code) ++utf32Count;
                //else if (bigUtf32Char[(code >> 24) & 0xf] == code) ++bigUtf32;
                start += 4;
            }
        }
        /// <summary>
        /// 汉字编码检测
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>编码,失败为null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Encoding ChineseEncoding(byte[] data)
        {
            return data != null ? (data.Length != 0 ? new ChineseEncoder().Get(data) : Encoding.ASCII) : null;
        }
        /// <summary>
        /// 汉字编码检测
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="length">字节数量</param>
        /// <returns>编码,失败为null</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Encoding ChineseEncoding(byte* data, int length)
        {
            return new ChineseEncoder().get(data, length);
        }
        /// <summary>
        /// 字节流转字符串
        /// </summary>
        /// <param name="data">字节流</param>
        /// <param name="encoding">编码,检测失败为本地编码</param>
        /// <returns>字符串</returns>
        public static string ToString(byte[] data, Encoding encoding)
        {
            if (data != null)
            {
                if (data.Length != 0)
                {
                    if (encoding == null) encoding = ChineseEncoding(data) ?? Encoding.Default;
                    return encoding == Encoding.ASCII ? data.BytesToStringNotEmpty() : encoding.GetString(data);
                }
                return string.Empty;
            }
            return null;
        }
    }
}
