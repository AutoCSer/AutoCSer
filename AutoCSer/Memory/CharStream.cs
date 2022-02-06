using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 内存字符流
    /// </summary>
    public sealed unsafe class CharStream : UnmanagedStreamBase
    {
        /// <summary>
        /// 数据
        /// </summary>
        public char* Char
        {
            get { return Data.Char; }
        }
        /// <summary>
        /// 当前写入位置
        /// </summary>
        public char* CurrentChar
        {
            get { return (char*)Data.Current; }
        }
        /// <summary>
        /// 当前数据长度
        /// </summary>
        public int Length
        {
            get { return Data.CurrentIndex >> 1; }
        }
        /// <summary>
        /// 内存数据流
        /// </summary>
        /// <param name="size">容器初始尺寸</param>
        public CharStream(int size = UnmanagedPool.TinySize >> 1) : base(size << 1) { }
        /// <summary>
        /// 非托管内存数据流
        /// </summary>
        /// <param name="data">无需释放的数据</param>
        internal CharStream(ref Pointer data) : base(ref data) { }
        /// <summary>
        /// 非托管内存数据流
        /// </summary>
        /// <param name="data">无需释放的数据</param>
        internal CharStream(Pointer data) : base(ref data) { }
        /// <summary>
        /// 预增数据流字符长度
        /// </summary>
        /// <param name="size">增加字符长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal char* GetPrepCharSizeCurrent(int size)
        {
            PrepCharSize(size);
            return (char*)Data.Current;
        }
        /// <summary>
        /// 预增数据流字符长度
        /// </summary>
        /// <param name="size">增加字符长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void PrepCharSize(int size)
        {
#if DEBUG
            if (size <= 0) throw new Exception(size.toString() + " <= 0");
            if ((long)size << 1 > int.MaxValue) throw new Exception(size.toString() + " * 2 > int.MaxValue");
#endif
            PrepSize(size << 1);
        }
        /// <summary>
        /// 预增数据流字符长度并返回
        /// </summary>
        /// <param name="size">字符长度</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Span<char> GetPrepChar(int size)
        {
            PrepSize(size);
            return new Span<char>(Data.Current, size);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(char value)
        {
            PrepSize(sizeof(char));
            Data.Write(value);
        }
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Write(CharStream charStream, char value)
        {
            charStream.Write(value);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        public void Write(string value)
        {
            int length = value.Length;
            if (length != 0) value.AsSpan().CopyTo(new Span<char>(GetBeforeMove(length * sizeof(char)), length));
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="array">字符串</param>
        internal void Write(params string[] array)
        {
            foreach (string value in array) Write(value);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Write(CharStream charStream, string value)
        {
            charStream.Write(value);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">长度必须大于0</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(string value, int index, int count)
        {
            value.AsSpan(index, count).CopyTo(new Span<char>(GetBeforeMove(count * sizeof(char)), count));
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Write(char* value, int size)
        {
            new Span<char>(value, size).CopyTo(new Span<char>(GetBeforeMove(size << 1), size));
        }
        /// <summary>
        /// 写字符串，适合零碎短小数据(不足8字节按8字节算)
        /// </summary>
        /// <param name="value">字符串</param>
        internal void SimpleWrite(string value)
        {
            PrepSize((value.Length << 1) + 6);
            Data.SimpleWrite(value);
        }
        /// <summary>
        /// 写字符串，适合零碎短小数据(不足8字节按8字节算)
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void SimpleWrite(CharStream charStream, string value)
        {
            charStream.SimpleWrite(value);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value">字符串</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(ref SubString value)
        {
            if (value.Length != 0) WriteNotEmpty(ref value);
        }
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteNotEmpty(ref SubString value)
        {
            value.CharSpan.CopyTo(new Span<char>(GetBeforeMove(value.Length * sizeof(char)), value.Length));
        }

        /// <summary>
        /// 输出 null 值
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteJsonNull()
        {
            *(long*)GetBeforeMove(4 * sizeof(char)) = 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48);
        }
        /// <summary>
        /// 输出空对象
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteJsonObject()
        {
            *(int*)GetBeforeMove(2 * sizeof(char)) = '{' + ('}' << 16);
        }
        /// <summary>
        /// 输出空数组
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteJsonArray()
        {
            *(int*)GetBeforeMove(2 * sizeof(char)) = '[' + (']' << 16);
        }
        /// <summary>
        /// 输出对象字符串 [object Object]
        /// </summary>
        public void WriteJsonObjectString()
        {
            char* chars = GetPrepCharSizeCurrent(16);
            *(long*)chars = '[' + ('o' << 16) + ((long)'b' << 32) + ((long)'j' << 48);
            *(long*)(chars + 4) = 'e' + ('c' << 16) + ((long)'t' << 32) + ((long)' ' << 48);
            *(long*)(chars + 8) = 'O' + ('b' << 16) + ((long)'j' << 32) + ((long)'e' << 48);
            *(long*)(chars + 12) = 'c' + ('t' << 16) + ((long)']' << 32);
            Data.CurrentIndex += 15 * sizeof(char);
        }
        /// <summary>
        /// 预申请数组长度并写入数组开始符号 [
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteJsonArrayStart(int size)
        {
            PrepCharSize(size);
            Data.Write('[');
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        public void WriteJsonBool(bool value)
        {
            if (value) *(long*)GetBeforeMove(4 * sizeof(char)) = 't' + ('r' << 16) + ((long)'u' << 32) + ((long)'e' << 48);
            else
            {
                byte* chars = GetBeforeMove(5 * sizeof(char));
                *(long*)chars = 'f' + ('a' << 16) + ((long)'l' << 32) + ((long)'s' << 48);
                *(char*)(chars + sizeof(long)) = 'e';
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        internal void WriteJsonHex(byte value)
        {
            byte* chars = GetBeforeMove(4 * sizeof(char));
            *(int*)chars = '0' + ('x' << 16);
            *(char*)(chars + sizeof(char) * 2) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)value >> 4);
            *(char*)(chars + sizeof(char) * 3) = (char)AutoCSer.Extensions.NumberExtension.ToHex((uint)value & 15);
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        internal void WriteJsonHex(sbyte value)
        {
            if (value < 0)
            {
                byte* chars = GetBeforeMove(5 * sizeof(char));
                uint value32 = (uint)-(int)value;
                *(int*)chars = '-' + ('0' << 16);
                *(char*)(chars + sizeof(char) * 2) = 'x';
                *(char*)(chars + sizeof(char) * 3) = (char)((value32 >> 4) + '0');
                *(char*)(chars + sizeof(char) * 4) = (char)AutoCSer.Extensions.NumberExtension.ToHex(value32 & 15);
            }
            else
            {
                byte* chars = GetBeforeMove(4 * sizeof(char));
                uint value32 = (uint)(int)value;
                *(int*)chars = '0' + ('x' << 16);
                *(char*)(chars + sizeof(char) * 2) = (char)((value32 >> 4) + '0');
                *(char*)(chars + sizeof(char) * 3) = (char)AutoCSer.Extensions.NumberExtension.ToHex(value32 & 15);
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteJsonHex(ushort value)
        {
            byte* chars = GetBeforeMove(6 * sizeof(char));
            *(int*)chars = '0' + ('x' << 16);
            AutoCSer.Extensions.NumberExtension.ToHex4(value, (char*)(chars + sizeof(int)));
        }
        /// <summary>
        /// 预增数据流字符长度并写入负号
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteNegative(int size)
        {
            PrepCharSize(size);
            Data.Write('-');
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        internal void WriteJsonHex(uint value)
        {
            char* chars = GetPrepCharSizeCurrent(10);
            *(int*)chars = '0' + ('x' << 16);
            char* next = AutoCSer.Extensions.NumberExtension.GetToHex(value >> 16, chars + 2);
            AutoCSer.Extensions.NumberExtension.ToHex4(value & 0xffff, next);
            Data.CurrentIndex += ((int)(next - chars) + 4) * sizeof(char);
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value"></param>
        internal void WriteJsonString(long value)
        {
            PrepCharSize(24 + 2);
            Data.Write('"');
            AutoCSer.Extensions.NumberExtension.UnsafeToString(value, this);
            Data.Write('"');
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value"></param>
        internal void WriteJsonHex(ulong value)
        {
            char* chars = GetPrepCharSizeCurrent(18), next;
            uint value32 = (uint)(value >> 32);
            *(int*)chars = '0' + ('x' << 16);
            if (value32 >= 0x10000)
            {
                next = AutoCSer.Extensions.NumberExtension.GetToHex(value32 >> 16, chars + 2);
                AutoCSer.Extensions.NumberExtension.ToHex4(value32 & 0xffff, next);
                next += 4;
            }
            else next = AutoCSer.Extensions.NumberExtension.GetToHex(value32, chars + 2);
            AutoCSer.Extensions.NumberExtension.ToHex4((value32 = (uint)value) >> 16, next);
            AutoCSer.Extensions.NumberExtension.ToHex4(value32 & 0xffff, next + 4);
            Data.CurrentIndex += ((int)(next - chars) + 8) * sizeof(char);
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value"></param>
        internal void WriteJsonString(ulong value)
        {
            PrepCharSize(22 + 2);
            Data.Write('"');
            AutoCSer.Extensions.NumberExtension.UnsafeToString(value, this);
            Data.Write('"');
        }
        /// <summary>
        /// 输出 double 值
        /// </summary>
        /// <param name="value"></param>
        internal void WriteJson(float value)
        {
            if (!float.IsNaN(value) && !float.IsInfinity(value))
            {
                int size = JsonSerializer.CustomConfig.Write(this, value);
                if (size > 0) Data.CurrentIndex += size << 1;
            }
            else WriteJsonNaN();
        }
        /// <summary>
        /// 输出 double 值
        /// </summary>
        /// <param name="value"></param>
        internal void WriteJsonInfinity(float value)
        {
            if (!float.IsNaN(value))
            {
                if (!float.IsInfinity(value))
                {
                    int size = JsonSerializer.CustomConfig.Write(this, value);
                    if (size > 0) Data.CurrentIndex += size << 1;
                }
                else if (float.IsPositiveInfinity(value)) WritePositiveInfinity();
                else WriteNegativeInfinity();
            }
            else WriteJsonNaN();
        }
        /// <summary>
        /// 输出 double 值
        /// </summary>
        /// <param name="value"></param>
        internal void WriteJson(double value)
        {
            if (!double.IsNaN(value) && !double.IsInfinity(value))
            {
                if (value <= 1.7976931348623150E+308)
                {
                    if (value >= -1.7976931348623150E+308)
                    {
                        int size = JsonSerializer.CustomConfig.Write(this, value);
                        if (size > 0) Data.CurrentIndex += size << 1;
                    }
                    else writeDoubleMinValue(value);
                }
                else writeDoubleMaxValue(value);
            }
            else WriteJsonNaN();
        }
        /// <summary>
        /// 输出 double 值
        /// </summary>
        /// <param name="value"></param>
        internal void WriteJsonInfinity(double value)
        {
            if (!double.IsNaN(value))
            {
                if (!double.IsInfinity(value))
                {
                    if (value <= 1.7976931348623150E+308)
                    {
                        if (value >= -1.7976931348623150E+308)
                        {
                            int size = JsonSerializer.CustomConfig.Write(this, value);
                            if (size > 0) Data.CurrentIndex += size << 1;
                        }
                        else writeDoubleMinValue(value);
                    }
                    else writeDoubleMaxValue(value);
                }
                else if (double.IsPositiveInfinity(value)) WritePositiveInfinity();
                else WriteNegativeInfinity();
            }
            else WriteJsonNaN();
        }
        /// <summary>
        /// 输出非数字值
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteJsonNaN()
        {
            *(long*)GetPrepCharSizeCurrent(4) = 'N' + ('a' << 16) + ((long)'N' << 32);
            Data.CurrentIndex += 3 * sizeof(char);
        }
        /// <summary>
        /// 输出正无穷
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WritePositiveInfinity()
        {
            writeInfinity((byte*)GetBeforeMove(8 * sizeof(char)));
        }
        /// <summary>
        /// 输出负无穷
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteNegativeInfinity()
        {
            byte* write = GetBeforeMove(9 * sizeof(char));
            *(char*)write = '-';
            writeInfinity(write + sizeof(char));
        }
        /// <summary>
        /// 输出无穷
        /// </summary>
        /// <param name="write"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void writeInfinity(byte* write)
        {//Infinity
            *(long*)write = 'I' + ('n' << 16) + ((long)'f' << 32) + ((long)'i' << 48);
            *(long*)(write + sizeof(long)) = 'n' + ('i' << 16) + ((long)'t' << 32) + ((long)'y' << 48);
        }
        /// <summary>
        /// 输出 double 最大值
        /// </summary>
        /// <param name="value"></param>
        private void writeDoubleMaxValue(double value)
        {
            byte* write = (byte*)GetPrepCharSizeCurrent(24);
            //1.79 7693 1348 6231 57E+ 308
            *(long*)write = '1' + ('.' << 16) + ((long)'7' << 32) + ((long)'9' << 48);
            *(long*)(write + sizeof(long)) = '7' + ('6' << 16) + ((long)'9' << 32) + ((long)'3' << 48);
            *(long*)(write + sizeof(long) * 2) = '1' + ('3' << 16) + ((long)'4' << 32) + ((long)'8' << 48);
            *(long*)(write + sizeof(long) * 3) = '6' + ('2' << 16) + ((long)'3' << 32) + ((long)'1' << 48);

            if (value >= 1.7976931348623157E+308) *(long*)(write + sizeof(long) * 4) = '5' + ('7' << 16) + ((long)'E' << 32) + ((long)'+' << 48);
            else if (value >= 1.7976931348623154E+308)
            {
                if (value >= 1.7976931348623155E+308) *(long*)(write + sizeof(long) * 4) = '5' + ('6' << 16) + ((long)'E' << 32) + ((long)'+' << 48);
                else *(long*)(write + sizeof(long) * 4) = '5' + ('4' << 16) + ((long)'E' << 32) + ((long)'+' << 48);
            }
            else *(long*)(write + sizeof(long) * 4) = '5' + ('2' << 16) + ((long)'E' << 32) + ((long)'+' << 48);
            *(long*)(write + sizeof(long) * 5) = '3' + ('0' << 16) + ((long)'8' << 32);
            Data.CurrentIndex += 23 * sizeof(char);
        }
        /// <summary>
        /// 输出 double 最大值
        /// </summary>
        /// <param name="value"></param>
        private void writeDoubleMinValue(double value)
        {
            byte* write = (byte*)GetBeforeMove(24 * sizeof(char));
            //-1.7 9769 3134 8623 157E +308
            *(long*)write = '-' + ('1' << 16) + ((long)'.' << 32) + ((long)'7' << 48);
            *(long*)(write + sizeof(long)) = '9' + ('7' << 16) + ((long)'6' << 32) + ((long)'9' << 48);
            *(long*)(write + sizeof(long) * 2) = '3' + ('1' << 16) + ((long)'3' << 32) + ((long)'4' << 48);
            *(long*)(write + sizeof(long) * 3) = '8' + ('6' << 16) + ((long)'2' << 32) + ((long)'3' << 48);

            if (value <= -1.7976931348623157E+308) *(long*)(write + sizeof(long) * 4) = '1' + ('5' << 16) + ((long)'7' << 32) + ((long)'E' << 48);
            else if (value <= -1.7976931348623154E+308)
            {
                if (value <= -1.7976931348623155E+308) *(long*)(write + sizeof(long) * 4) = '1' + ('5' << 16) + ((long)'6' << 32) + ((long)'E' << 48);
                else *(long*)(write + sizeof(long) * 4) = '1' + ('5' << 16) + ((long)'4' << 32) + ((long)'E' << 48);
            }
            else *(long*)(write + sizeof(long) * 4) = '1' + ('5' << 16) + ((long)'2' << 32) + ((long)'E' << 48);
            *(long*)(write + sizeof(long) * 5) = '+' + ('3' << 16) + ((long)'0' << 32) + ((long)'8' << 48);
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="nullChar">空字符替换</param>
        internal void WriteJson(char value, char nullChar = ' ')
        {
            if (((AutoCSer.JsonDeSerializer.DeSerializeBits.Byte[(byte)value] & AutoCSer.JsonDeSerializer.EscapeBit) | (value >> 8)) == 0)
            {
                byte* data = GetBeforeMove(4 * sizeof(char));
                *(char*)data = '"';
                switch ((value ^ (value >> 3)) & 7)
                {
                    case ('\b' ^ ('\b' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('b' << 16); break;
                    case ('\t' ^ ('\t' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('t' << 16); break;
                    case ('\n' ^ ('\n' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('n' << 16); break;
                    case ('\f' ^ ('\f' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('f' << 16); break;
                    case ('\r' ^ ('\r' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('r' << 16); break;
                    case ('\\' ^ ('\\' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('\\' << 16); break;
                    case ('\"' ^ ('\"' >> 3)) & 7: *(int*)(data + sizeof(char)) = '\\' + ('"' << 16); break;
                }
                *(char*)(data + sizeof(char) * 3) = '"';
            }
            else
            {
                byte* data = GetBeforeMove(3 * sizeof(char));
                *(char*)data = '"';
                *(char*)(data + sizeof(char)) = value == 0 ? nullChar : value;
                *(char*)(data + sizeof(char) * 2) = '"';
            }
        }
        /// <summary>
        /// 时间转字符串
        /// </summary>
        /// <param name="time">时间</param>
        internal void WriteJsonString(DateTime time)
        {
            switch (time.Kind)
            {
                case DateTimeKind.Utc:
                    char* utcFixed = GetPrepCharSizeCurrent(19 + 8 + 3);
                    *utcFixed = '"';
                    int utcSize = AutoCSer.Date.ToString(time, utcFixed + 1);
                    *(int*)(utcFixed + (utcSize + 1)) = 'Z' + ('"' << 16);
                    Data.CurrentIndex += (utcSize + 3) << 1;
                    return;
                case DateTimeKind.Local:
                    char* localFixed = GetPrepCharSizeCurrent(20 + 8 + 8);
                    *localFixed = '"';
                    int localSize = AutoCSer.Date.ToString(time, localFixed + 1);
                    *(long*)(localFixed + (localSize + 1)) = Date.ZoneHourString;
                    *(long*)(localFixed + (localSize + 5)) = Date.ZoneMinuteString;
                    Data.CurrentIndex += (localSize + 6 + 2) << 1;
                    return;
                default:
                    char* timeFixed = GetPrepCharSizeCurrent(19 + 8 + 2);
                    *timeFixed = '"';
                    int size = AutoCSer.Date.ToString(time, timeFixed + 1);
                    *(timeFixed + (size + 1)) = '"';
                    Data.CurrentIndex += (size + 2) << 1;
                    return;
            }
        }
        /// <summary>
        /// 时间转字符串
        /// </summary>
        /// <param name="time">时间</param>
        internal void WriteJsonSqlString(DateTime time)
        {
            byte* chars = GetBeforeMove((AutoCSer.Date.MillisecondStringSize + 2) * sizeof(char));
            *(char*)chars = '"';
            AutoCSer.Date.ToMillisecondString(time, (char*)(chars + sizeof(char)));
            *(char*)(chars + (AutoCSer.Date.MillisecondStringSize + 1) * sizeof(char)) = '"';
        }
        /// <summary>
        /// 写入 new Date(
        /// </summary>
        internal void WriteJsonNewDate()
        {
            byte* data = (byte*)GetPrepCharSizeCurrent(9 + 19 + 1);
            *(long*)data = 'n' + ('e' << 16) + ((long)'w' << 32) + ((long)' ' << 48);
            *(long*)(data + sizeof(long)) = 'D' + ('a' << 16) + ((long)'t' << 32) + ((long)'e' << 48);
            *(char*)(data + sizeof(long) * 2) = '(';
            Data.CurrentIndex += 9 * sizeof(char);
        }
        /// <summary>
        /// 时间转字符串 第三方格式开始 "/Date(
        /// </summary>
        internal void WriteJsonOtherDate()
        {
            byte* data = (byte*)GetPrepCharSizeCurrent(7 + 19 + 4);
            *(long*)data = '"' + ('/' << 16) + ((long)'D' << 32) + ((long)'a' << 48);
            *(long*)(data + sizeof(long)) = 't' + ('e' << 16) + ((long)'(' << 32);
            Data.CurrentIndex += 7 * sizeof(char);
        }
        /// <summary>
        /// 时间转字符串 第三方格式结束 )/"
        /// </summary>
        internal void WriteJsonOtherDateEnd()
        {
            *(long*)Data.Current = ')' + ('/' << 16) + ((long)'"' << 32);
            Data.CurrentIndex += 3 * sizeof(char);
        }
        /// <summary>
        /// Guid转换成字符串
        /// </summary>
        /// <param name="value">Guid</param>
        public void WriteJson(ref System.Guid value)
        {
            byte* data = GetBeforeMove(38 * sizeof(char));
            *(char*)data = '"';
            new GuidCreator { Value = value }.ToString((char*)(data + sizeof(char)));
            *(char*)(data + sizeof(char) * 37) = '"';
        }
        /// <summary>
        /// 写入空字符串
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteJsonEmptyString()
        {
            *(int*)GetBeforeMove(2 * sizeof(char)) = '"' + ('"' << 16);
        }
        /// <summary>
        /// 写入 JSON 字符串
        /// </summary>
        /// <param name="value">不能为 null</param>
        /// <param name="nullChar"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteJson(string value, char nullChar = ' ')
        {
            if (value.Length != 0) WriteJsonNotEmpty(value, nullChar);
            else WriteJsonEmptyString();
        }
        /// <summary>
        /// 写入 JSON 字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nullChar"></param>
        private void WriteJsonNotEmpty(string value, char nullChar)
        {
            Write('"');
            fixed (char* valueFixed = value) WriteJson(valueFixed, value.Length, nullChar);
            Write('"');
        }
        /// <summary>
        /// 写入 JSON 字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nullChar"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteJson(ref SubString value, char nullChar = ' ')
        {
            if (value.Length != 0)
            {
                Write('"');
                fixed (char* valueFixed = value.GetFixedBuffer()) WriteJson(valueFixed + value.Start, value.Length, nullChar);
                Write('"');
            }
            else WriteJsonEmptyString();
        }
        /// <summary>
        /// 写入 JSON 字符串
        /// </summary>
        /// <param name="stringStart">起始位置</param>
        /// <param name="stringLength">字符串长度，必须大于0</param>
        /// <param name="nullChar">空字符替换</param>
        internal void WriteJson(char* stringStart, int stringLength, char nullChar)
        {
#if DEBUG
            if (stringStart == null) throw new Exception("stringStart == null");
            if (stringLength <= 0) throw new Exception(stringLength.toString() + " <= 0");
#endif
            char* start = stringStart, end = stringStart + stringLength;
            byte* bits = AutoCSer.JsonDeSerializer.DeSerializeBits.Byte;
            int length = 0;
            do
            {
                if (((bits[*(byte*)start] & AutoCSer.JsonDeSerializer.EscapeBit) | *(((byte*)start) + 1)) == 0) ++length;
            }
            while (++start != end);
            if (length == 0)
            {
                char* write = (char*)GetBeforeMove(stringLength * sizeof(char));
                if (nullChar == 0) new Span<char>(stringStart, stringLength).CopyTo(new Span<char>(write, stringLength));
                else
                {
                    start = stringStart;
                    do
                    {
                        *write++ = *start == 0 ? nullChar : *start;
                    }
                    while (++start != end);
                }
            }
            else
            {
                length += stringLength;
                char* write = (char*)GetBeforeMove(length * sizeof(char));
                start = stringStart;
                do
                {
                    if (((bits[*(byte*)start] & AutoCSer.JsonDeSerializer.EscapeBit) | *(((byte*)start) + 1)) == 0)
                    {
                        switch ((*start ^ (*start >> 3)) & 7)
                        {
                            case ('\b' ^ ('\b' >> 3)) & 7: *(int*)write = '\\' + ('b' << 16); break;
                            case ('\t' ^ ('\t' >> 3)) & 7: *(int*)write = '\\' + ('t' << 16); break;
                            case ('\n' ^ ('\n' >> 3)) & 7: *(int*)write = '\\' + ('n' << 16); break;
                            case ('\f' ^ ('\f' >> 3)) & 7: *(int*)write = '\\' + ('f' << 16); break;
                            case ('\r' ^ ('\r' >> 3)) & 7: *(int*)write = '\\' + ('r' << 16); break;
                            case ('\\' ^ ('\\' >> 3)) & 7: *(int*)write = '\\' + ('\\' << 16); break;
                            case ('\"' ^ ('\"' >> 3)) & 7: *(int*)write = '\\' + ('"' << 16); break;
                        }
                        write += 2;
                    }
                    else *write++ = *start == 0 ? nullChar : *start;
                }
                while (++start != end);
            }
        }
        /// <summary>
        /// 写入 JSON 名称
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="nullChar"></param>
        internal void WriteJsonName(string value, char nullChar)
        {
            if (string.IsNullOrEmpty(value))
            {
                *(long*)GetPrepCharSizeCurrent(4 * sizeof(char)) = '"' + ('"' << 16) + ((long)':' << 32);
                Data.CurrentIndex += 3 * sizeof(char);
            }
            else
            {
                WriteJsonNotEmpty(value, nullChar);
                Write(':');
            }
        }
        /// <summary>
        /// 写入 JSON Key
        /// </summary>
        /// <param name="size"></param>
        internal void WriteJsonKeyValueKey(int size)
        {
            byte* data = (byte*)GetPrepCharSizeCurrent(size);
            *(long*)data = '{' + ('"' << 16) + ((long)'K' << 32) + ((long)'e' << 48);
            *(long*)(data + sizeof(long)) = 'y' + ('"' << 16) + ((long)':' << 32);
            Data.CurrentIndex += 7 * sizeof(char);
        }
        /// <summary>
        /// 写入 JSON Value
        /// </summary>
        internal void WriteJsonKeyValueValue()
        {
            byte* data = GetBeforeMove(9 * sizeof(char));
            *(long*)data = ',' + ('"' << 16) + ((long)'V' << 32) + ((long)'a' << 48);
            *(long*)(data + sizeof(long)) = 'l' + ('u' << 16) + ((long)'e' << 32) + ((long)'"' << 48);
            *(char*)(data + sizeof(long) * 2) = ':';
        }
        /// <summary>
        /// 输出 JSON 字符串，不处理转义符（带双引号）
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteQuote(string value)
        {
            PrepCharSize(value.Length + 2);
            Data.WriteQuote(value);
        }
        /// <summary>
                 /// 写 JSON 名称
                 /// </summary>
                 /// <param name="name"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteJsonCustomNameFirst(string name)
        {
            PrepCharSize(name.Length + (5 + 1));
            Data.WriteJsonCustomNameFirst(name);
        }
        /// <summary>
        /// 写 JSON 名称
        /// </summary>
        /// <param name="name"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteJsonCustomNameNext(string name)
        {
            PrepCharSize(name.Length + (5 + 1));
            Data.WriteJsonCustomNameNext(name);
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <param name="value">字符串</param>
        internal void WriteJsonDictionaryKey(string value)
        {
            if (string.IsNullOrEmpty(value)) *(int*)GetBeforeMove(2 * sizeof(char)) = '"' + ('"' << 16);
            else
            {
                int stringLength = value.Length;
                fixed (char* valueFixed = value)
                {
                    char* start = valueFixed, end = valueFixed + stringLength;
                    byte* bits = AutoCSer.JsonDeSerializer.DeSerializeBits.Byte;
                    int length = 0;
                    do
                    {
                        if (((bits[*(byte*)start] & AutoCSer.JsonDeSerializer.EscapeBit) | *(((byte*)start) + 1)) == 0) ++length;
                    }
                    while (++start != end);
                    if (length == 0)
                    {
                        char* write = GetPrepCharSizeCurrent(stringLength + 1 + 3);
                        *write = '"';
                        AutoCSer.Extensions.StringExtension.SimpleCopyNotNull64(valueFixed, ++write, stringLength);
                        *(write + stringLength) = '"';
                        Data.CurrentIndex += (stringLength + 2) << 1;
                    }
                    else
                    {
                        char* write = (char*)GetBeforeMove((length + stringLength + 2) * sizeof(char));
                        *write++ = '"';
                        start = valueFixed;
                        do
                        {
                            if (((bits[*(byte*)start] & AutoCSer.JsonDeSerializer.EscapeBit) | *(((byte*)start) + 1)) == 0)
                            {
                                switch ((*start ^ (*start >> 3)) & 7)
                                {
                                    case ('\b' ^ ('\b' >> 3)) & 7: *(int*)write = '\\' + ('b' << 16); break;
                                    case ('\t' ^ ('\t' >> 3)) & 7: *(int*)write = '\\' + ('t' << 16); break;
                                    case ('\n' ^ ('\n' >> 3)) & 7: *(int*)write = '\\' + ('n' << 16); break;
                                    case ('\f' ^ ('\f' >> 3)) & 7: *(int*)write = '\\' + ('f' << 16); break;
                                    case ('\r' ^ ('\r' >> 3)) & 7: *(int*)write = '\\' + ('r' << 16); break;
                                    case ('\\' ^ ('\\' >> 3)) & 7: *(int*)write = '\\' + ('\\' << 16); break;
                                    case ('\"' ^ ('\"' >> 3)) & 7: *(int*)write = '\\' + ('"' << 16); break;
                                }
                                write += 2;
                            }
                            else *write++ = *start;
                        }
                        while (++start != end);
                        *write = '"';
                    }
                }
            }
        }

        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteWebViewJson(bool value)
        {
            Write(value ? '1' : '0');
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteWebViewJson(byte value)
        {
            WriteJsonHex(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteWebViewJson(sbyte value)
        {
            WriteJsonHex(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteWebViewJson(short value)
        {
            if (value >= 0) WriteJsonHex((ushort)value);
            else
            {
                WriteNegative(7);
                WriteJsonHex((ushort)-value);
            }
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteWebViewJson(ushort value)
        {
            WriteJsonHex(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteWebViewJson(int value)
        {
            if (value >= 0) WriteJsonHex((uint)value);
            else
            {
                WriteNegative(11);
                WriteJsonHex((uint)-value);
            }
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteWebViewJson(uint value)
        {
            WriteJsonHex(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        public void WriteWebViewJson(long value)
        {
            if (value >= 0) WriteWebViewJson((ulong)value);
            else if ((ulong)(value + JsonSerializer.MaxInteger) <= (ulong)(JsonSerializer.MaxInteger << 1))
            {
                WriteNegative(19);
                WriteWebViewJson((ulong)-value);
            }
            else WriteJsonString(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        public void WriteWebViewJson(ulong value)
        {
            if (value <= JsonSerializer.MaxInteger)
            {
                if (value <= uint.MaxValue) WriteJsonHex((uint)value);
                else WriteJsonHex(value);
            }
            else WriteJsonString(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteWebViewJson(float value)
        {
           WriteJson(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteWebViewJson(double value)
        {
            WriteJson(value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteWebViewJson(decimal value)
        {
            SimpleWrite(value.ToString());
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteWebViewJson(char value)
        {
            WriteJson(value, ' ');
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteWebViewJson(DateTime value)
        {
            WriteJsonNewDate();
            WriteWebViewJson((long)((value.Kind == DateTimeKind.Utc ? value.Ticks + Date.LocalTimeTicks : value.Ticks) - AutoCSer.JsonDeSerializer.JavascriptLocalMinTimeTicks) / TimeSpan.TicksPerMillisecond);
            Data.Write(')');
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteWebViewJson(Guid value)
        {
            WriteJson(ref value);
        }
        /// <summary>
        /// WebView 写 JSON 数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteWebViewJson(string value)
        {
            WriteJson(value);
        }
        /// <summary>
        /// 写入 JSON 字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WriteWebViewJson(SubString value)
        {
            WriteJson(ref value);
        }
        /// <summary>
        /// 对象转换成JSON字符串
        /// </summary>
        /// <param name="value">#!转换URL</param>
        public void WriteWebViewJson(AutoCSer.WebView.HashUrl value)
        {
            Write('"');
            if (!string.IsNullOrEmpty(value.Path))
            {
                fixed (char* valueFixed = value.Path) WriteJson(valueFixed, value.Path.Length, ' ');
            }
            if (!string.IsNullOrEmpty(value.Query))
            {
                *(int*)GetBeforeMove(2 * sizeof(char)) = '#' + ('!' << 16);
                fixed (char* valueFixed = value.Query) WriteJson(valueFixed, value.Query.Length, ' ');
            }
            Write('"');
        }
        /// <summary>
        /// 模拟javascript解码函数unescape
        /// </summary>
        /// <param name="value">原字符串,长度必须大于 0</param>
        public void JavascriptUnescape(SubArray<byte> value)
        {
            fixed (byte* valueFixed = value.GetFixedBuffer())
            {
                PrepCharSize(value.Length + 2);
                byte* start = valueFixed + value.Start;
                Data.JavascriptUnescape(start, start + value.Length);
            }
        }
    }
}
