using System;
using System.Threading;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using AutoCSer.Memory;

namespace AutoCSer
{
    /// <summary>
    /// 日期相关操作
    /// </summary>
    public unsafe static partial class Date
    {
        /// <summary>
        /// 默认基础时间值 1900/1/1
        /// </summary>
        public static readonly DateTime BaseTime = new DateTime(1900, 1, 1);
        /// <summary>
        /// 初始化时间 Utc
        /// </summary>
        public static readonly DateTime StartTime;
        /// <summary>
        /// 初始化时钟周期
        /// </summary>
        internal static readonly long StartTimestamp;

        ///// <summary>
        ///// 每毫秒计时周期数
        ///// </summary>
        //public static readonly long MillisecondTicks = new TimeSpan(0, 0, 0, 0, 1).Ticks;
        ///// <summary>
        ///// 每秒计时周期数
        ///// </summary>
        //public static readonly long SecondTicks = TimeSpan.TicksPerMillisecond * 1000;
        ///// <summary>
        ///// 每分钟计时周期数
        ///// </summary>
        //public static readonly long MinutesTicks = TimeSpan.TicksPerSecond * 60;
        ///// <summary>
        ///// 一天的计时周期数
        ///// </summary>
        //public static readonly long DayTiks = 24L * 60L * 60L * TimeSpan.TicksPerSecond;
        /// <summary>
        /// 本地时钟周期
        /// </summary>
        public static readonly long LocalTimeTicks;
        /// <summary>
        /// 时区小时字符串 +HH:
        /// </summary>
        internal static readonly long ZoneHourString;
        /// <summary>
        /// 时区f分钟字符串 mm"
        /// </summary>
        internal static readonly long ZoneMinuteString;

        /// <summary>
        /// 时间转换字符串字节长度
        /// </summary>
        public const int MillisecondStringSize = 23;
        /// <summary>
        /// 默认日期分隔符
        /// </summary>
        internal const char DateSplitChar = '/';
        /// <summary>
        /// 时间转换成字符串(精确到毫秒)
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="charStream">字符流</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void ToMillisecondString(DateTime time, CharStream charStream)
        {
            ToMillisecondString(time, charStream.CurrentChar);
            charStream.Data.CurrentIndex += MillisecondStringSize * sizeof(char);
        }
        /// <summary>
        /// 时间转换成字符串(精确到毫秒)
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="chars">时间字符串</param>
        internal unsafe static void ToMillisecondString(DateTime time, char* chars)
        {
            long dayTiks = time.Ticks % TimeSpan.TicksPerDay;
            toString(time, chars, DateSplitChar);
            long seconds = dayTiks / TimeSpan.TicksPerSecond;
            *(chars + 19) = '.';
            *(chars + 10) = ' ';
            toString((int)seconds, chars + 11);
            int data0 = (int)(((ulong)(dayTiks - seconds * TimeSpan.TicksPerSecond) * AutoCSer.Extensions.NumberExtension.Div10000Mul) >> AutoCSer.Extensions.NumberExtension.Div10000Shift);
            int data1 = (data0 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            int data2 = (data1 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(chars + 22) = (char)(data0 - data1 * 10 + '0');
            *(int*)(chars + 20) = (data2 + ((data1 - data2 * 10) << 16)) + 0x300030;
        }
        /// <summary>
        /// 时间转换成日期字符串(yyyy/MM/dd)
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="chars">时间字符串</param>
        /// <param name="split">分隔符</param>
        private unsafe static void toString(DateTime time, char* chars, char split)
        {
            int data0 = time.Year, data1 = (data0 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(chars + 4) = split;
            int data2 = (data1 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(chars + 7) = split;
            int data3 = (data2 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(int*)(chars + 2) = ((data1 - data2 * 10) + ((data0 - data1 * 10) << 16)) + 0x300030;
            *(int*)chars = (data3 + ((data2 - data3 * 10) << 16)) + 0x300030;
            data0 = time.Month;
            data2 = time.Day;
            data1 = (data0 + 6) >> 4;
            data3 = (data2 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(chars + 5) = (char)(data1 + '0');
            *(chars + 6) = (char)((data0 - data1 * 10) + '0');
            *(int*)(chars + 8) = (data3 + ((data2 - data3 * 10) << 16)) + 0x300030;
        }
        /// <summary>
        /// 32位除以60转乘法的乘数
        /// </summary>
        public const ulong Div60_32Mul = ((1L << Div60_32Shift) + 59) / 60;
        /// <summary>
        /// 32位除以60转乘法的位移
        /// </summary>
        public const int Div60_32Shift = 37;
        /// <summary>
        /// 16位除以60转乘法的乘数
        /// </summary>
        public const uint Div60_16Mul = ((1U << Div60_16Shift) + 59) / 60;
        /// <summary>
        /// 16位除以60转乘法的位移
        /// </summary>
        public const int Div60_16Shift = 21;
        /// <summary>
        /// 时间转换成字符串(HH:mm:ss)
        /// </summary>
        /// <param name="second">当天的计时秒数</param>
        /// <param name="chars">时间字符串</param>
        private unsafe static void toString(int second, char* chars)
        {
            int minute = (int)(((ulong)second * Div60_32Mul) >> Div60_32Shift);
            int hour = (minute * (int)Div60_16Mul) >> Div60_16Shift;
            second -= minute * 60;
            int high = (hour * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            minute -= hour * 60;
            *chars = (char)(high + '0');
            *(chars + 1) = (char)((hour - high * 10) + '0');
            *(chars + 2) = ':';
            high = (minute * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(int*)(chars + 3) = (high + ((minute - high * 10) << 16)) + 0x300030;
            *(chars + 5) = ':';
            high = (second * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(chars + 6) = (char)(high + '0');
            *(chars + 7) = (char)((second - high * 10) + '0');
        }
        /// <summary>
        /// 时间转换成字符串(yyyy/MM/dd HH:mm:ss)
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="dateSplit">日期分隔符</param>
        /// <returns>时间字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static string toString(this DateTime time, char dateSplit = DateSplitChar)
        {
            string timeString = AutoCSer.Extensions.StringExtension.FastAllocateString(19);
            fixed (char* timeFixed = timeString)
            {
                toString(time, timeFixed, dateSplit);
                *(timeFixed + 10) = ' ';
                toString((int)((time.Ticks % TimeSpan.TicksPerDay) / TimeSpan.TicksPerSecond), timeFixed + 11);
            }
            return timeString;
        }
        /// <summary>
        /// 时间转换成字符串
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="charStream">字符流</param>
        /// <param name="dateSplit">日期分隔符</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void ToString(this DateTime time, CharStream charStream, char dateSplit = DateSplitChar)
        {
            char* timeFixed = (char*)charStream.GetBeforeMove(19 * sizeof(char));
            toString(time, timeFixed, dateSplit);
            *(timeFixed + 10) = ' ';
            toString((int)((time.Ticks % TimeSpan.TicksPerDay) / TimeSpan.TicksPerSecond), timeFixed + 11);
        }
        /// <summary>
        /// 时间转换成字符串 yyyy-MM-ddTHH:mm:ss.XXXXXXX
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeFixed"></param>
        /// <returns>字符串长度</returns>
        internal static int ToString(this DateTime time, char* timeFixed)
        {
            toString(time, timeFixed, '-');
            *(timeFixed + 10) = 'T';
            long ticks = time.Ticks % TimeSpan.TicksPerDay, seconds = ticks / TimeSpan.TicksPerSecond;
            toString((int)seconds, timeFixed + 11);
            ticks -= seconds * TimeSpan.TicksPerSecond;
            if (ticks == 0) return 19;
            int low = (int)(uint)ticks, high = (int)(((uint)low * NumberExtension.Div10000Mul) >> NumberExtension.Div10000Shift);
            int data1 = (high * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            int data2 = (data1 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(long*)(timeFixed + 19) = '.' + (data2 << 16) + ((long)(data1 - data2 * 10) << 32) + ((long)(high - data1 * 10) << 48) + 0x30003000300000L;
            if ((low -= high * 10000) == 0) return 23;
            high = (low * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            data1 = (high * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            data2 = (data1 * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            *(long*)(timeFixed + 23) = data2 + ((data1 - data2 * 10) << 16) + ((long)(high - data1 * 10) << 32) + ((long)(low - high * 10) << 48) + 0x30003000300030L;
            return 27;
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static DateTime localToUniversalTime(this DateTime date)
        {
            return new DateTime(date.Ticks - LocalTimeTicks, DateTimeKind.Utc);
        }

        static Date()
        {
            if (Stopwatch.IsHighResolution)
            {
                StartTime = DateTime.UtcNow;
                StartTimestamp = Stopwatch.GetTimestamp();
            }
            else
            {
                StartTime = DateTime.UtcNow;
                StartTimestamp = StartTime.Ticks;
            }
#if DOTNET2
            LocalTimeTicks = StartTime.ToLocalTime().Ticks - StartTime.Ticks;
#else
            LocalTimeTicks = TimeZoneInfo.Local.BaseUtcOffset.Ticks;
#endif

            long zoneChar0, localTimeTicks;
            if (LocalTimeTicks >= 0)
            {
                zoneChar0 = '+' + ((long)':' << 48);
                localTimeTicks = LocalTimeTicks;
            }
            else
            {
                zoneChar0 = '-' + ((long)':' << 48);
                localTimeTicks = -LocalTimeTicks;
            }
            long minute = (int)(LocalTimeTicks / TimeSpan.TicksPerMinute);
            int hour = (int)(((ulong)minute * Div60_32Mul) >> Div60_32Shift), high = (hour * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            ZoneHourString = zoneChar0 + ((high + '0') << 16) + ((long)((hour - high * 10) + '0') << 32);
            minute -= hour * 60;
            high = ((int)minute * (int)AutoCSer.Extensions.NumberExtension.Div10_16Mul) >> AutoCSer.Extensions.NumberExtension.Div10_16Shift;
            ZoneMinuteString = (high + '0') + ((((int)minute - high * 10) + '0') << 16) + ((long)'"' << 32);

#if !Serialize
            weekData = Unmanaged.GetStaticPointer(8 * sizeof(int) + 12 * sizeof(int), false);
            monthData = new Pointer { Data = weekData.Byte + 8 * sizeof(int) };
            int* write = weekData.Int;
            *write = 'S' + ('u' << 8) + ('n' << 16) + (',' << 24);
            *++write = 'M' + ('o' << 8) + ('n' << 16) + (',' << 24);
            *++write = 'T' + ('u' << 8) + ('e' << 16) + (',' << 24);
            *++write = 'W' + ('e' << 8) + ('d' << 16) + (',' << 24);
            *++write = 'T' + ('h' << 8) + ('u' << 16) + (',' << 24);
            *++write = 'F' + ('r' << 8) + ('i' << 16) + (',' << 24);
            *++write = 'S' + ('a' << 8) + ('t' << 16) + (',' << 24);
            write = monthData.Int;
            *write = 'J' + ('a' << 8) + ('n' << 16) + (' ' << 24);
            *++write = 'F' + ('e' << 8) + ('b' << 16) + (' ' << 24);
            *++write = 'M' + ('a' << 8) + ('r' << 16) + (' ' << 24);
            *++write = 'A' + ('p' << 8) + ('r' << 16) + (' ' << 24);
            *++write = 'M' + ('a' << 8) + ('y' << 16) + (' ' << 24);
            *++write = 'J' + ('u' << 8) + ('n' << 16) + (' ' << 24);
            *++write = 'J' + ('u' << 8) + ('l' << 16) + (' ' << 24);
            *++write = 'A' + ('u' << 8) + ('g' << 16) + (' ' << 24);
            *++write = 'S' + ('e' << 8) + ('p' << 16) + (' ' << 24);
            *++write = 'O' + ('c' << 8) + ('t' << 16) + (' ' << 24);
            *++write = 'N' + ('o' << 8) + ('v' << 16) + (' ' << 24);
            *++write = 'D' + ('e' << 8) + ('c' << 16) + (' ' << 24);
#endif
        }
    }
}
