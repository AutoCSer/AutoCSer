﻿using System;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace AutoCSer
{
    /// <summary>
    /// 日期相关操作
    /// </summary>
    public unsafe static partial class Date
    {
        /// <summary>
        /// 星期
        /// </summary>
        private static AutoCSer.Memory.Pointer weekData;
        /// <summary>
        /// 月份
        /// </summary>
        private static AutoCSer.Memory.Pointer monthData;
        /// <summary>
        /// 时间转字节流长度
        /// </summary>
        internal const int ToByteLength = 29;
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static DateTime toUniversalTime(this DateTime date)
        {
            return date.Kind == DateTimeKind.Utc ? date : new DateTime(date.Ticks - LocalTimeTicks, DateTimeKind.Utc);
        }
        /// <summary>
        /// 时间转字节流
        /// </summary>
        /// <param name="date">时间</param>
        /// <param name="data">写入数据起始位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static void ToBytes(DateTime date, byte* data)
        {
            UniversalToBytes(date.toUniversalTime(), data);
        }
        /// <summary>
        /// 时间转字节流
        /// </summary>
        /// <param name="date">时间</param>
        /// <param name="data">写入数据起始位置</param>
        internal unsafe static void UniversalToBytes(DateTime date, byte* data)
        {
            *(int*)data = weekData.Int[(int)date.DayOfWeek];
            int value = date.Day, value10 = (value * (int)NumberExtension.Div10_16Mul) >> NumberExtension.Div10_16Shift;
            *(int*)(data + sizeof(int)) = (' ' + (value10 << 8) + ((value - value10 * 10) << 16) + (' ' << 24)) | 0x303000;
            value = date.Year;
            *(int*)(data + sizeof(int) * 2) = monthData.Int[date.Month - 1];
            value10 = (value * (int)NumberExtension.Div10_16Mul) >> NumberExtension.Div10_16Shift;
            int value100 = (value10 * (int)NumberExtension.Div10_16Mul) >> NumberExtension.Div10_16Shift;
            int value1000 = (value100 * (int)NumberExtension.Div10_16Mul) >> NumberExtension.Div10_16Shift;
            *(int*)(data + sizeof(int) * 3) = (value1000 + ((value100 - value1000 * 10) << 8) + ((value10 - value100 * 10) << 16) + ((value - value10 * 10) << 24)) | 0x30303030;

            value100 = (int)(date.Ticks % TimeSpan.TicksPerDay / TimeSpan.TicksPerSecond);
            value1000 = (int)(((ulong)value100 * Div60_32Mul) >> Div60_32Shift);
            value100 -= value1000 * 60;
            value = (value1000 * (int)Div60_16Mul) >> Div60_16Shift;
            value1000 -= value * 60;

            value10 = (value * (int)NumberExtension.Div10_16Mul) >> NumberExtension.Div10_16Shift;
            *(int*)(data + sizeof(int) * 4) = (' ' + (value10 << 8) + ((value - value10 * 10) << 16) + (':' << 24)) | 0x303000;
            value10 = (value1000 * (int)NumberExtension.Div10_16Mul) >> NumberExtension.Div10_16Shift;
            value = (value100 * (int)NumberExtension.Div10_16Mul) >> NumberExtension.Div10_16Shift;
            *(int*)(data + sizeof(int) * 5) = (value10 + ((value1000 - value10 * 10) << 8) + (':' << 16) + (value << 24)) | 0x30003030;
            *(int*)(data + sizeof(int) * 6) = ((value100 - value * 10) + '0') + (' ' << 8) + ('G' << 16) + ('M' << 24);
            *(data + sizeof(int) * 7) = (byte)'T';
        }
        /// <summary>
        /// 时间转字节流
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe static byte[] ToBytes(this DateTime date)
        {
            byte[] data = new byte[ToByteLength];
            fixed (byte* dataFixed = data) ToBytes(date, dataFixed);
            return data;
        }
        /// <summary>
        /// 时间转字节流
        /// </summary>
        /// <param name="date">时间</param>
        /// <returns>字节流</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static byte[] UniversalNewBytes(this DateTime date)
        {
            byte[] data = new byte[ToByteLength];
            fixed (byte* fixedData = data) UniversalToBytes(date, fixedData);
            return data;
        }
        /// <summary>
        /// 判断时间是否相等
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dataArray"></param>
        /// <returns></returns>
        internal unsafe static int UniversalByteEquals(DateTime date, SubArray<byte> dataArray)
        {
            fixed (byte* dataFixed = dataArray.GetFixedBuffer())
            {
                byte* data = dataFixed + dataArray.Start;
                if (((*(int*)data ^ weekData.Int[(int)date.DayOfWeek]) | (*(data + sizeof(int) * 7) ^ (byte)'T')) != 0) return 1;
                int value = date.Day, value10 = (value * (int)NumberExtension.Div10_16Mul) >> NumberExtension.Div10_16Shift;
                if (*(int*)(data + sizeof(int)) != ((' ' + (value10 << 8) + ((value - value10 * 10) << 16) + (' ' << 24)) | 0x303000)) return 1;
                value = date.Year;
                if (*(int*)(data + sizeof(int) * 2) != monthData.Int[date.Month - 1]) return 1;
                value10 = (value * (int)NumberExtension.Div10_16Mul) >> NumberExtension.Div10_16Shift;
                int value100 = (value10 * (int)NumberExtension.Div10_16Mul) >> NumberExtension.Div10_16Shift;
                int value1000 = (value100 * (int)NumberExtension.Div10_16Mul) >> NumberExtension.Div10_16Shift;
                if (*(int*)(data + sizeof(int) * 3) != ((value1000 + ((value100 - value1000 * 10) << 8) + ((value10 - value100 * 10) << 16) + ((value - value10 * 10) << 24)) | 0x30303030)) return 1;


                value100 = (int)(date.Ticks % TimeSpan.TicksPerDay / TimeSpan.TicksPerSecond);
                value1000 = (int)(((ulong)value100 * Div60_32Mul) >> Div60_32Shift);
                value100 -= value1000 * 60;
                value = (value1000 * (int)Div60_16Mul) >> Div60_16Shift;
                value1000 -= value * 60;

                value10 = (value * (int)NumberExtension.Div10_16Mul) >> NumberExtension.Div10_16Shift;
                if (*(int*)(data + sizeof(int) * 4) != ((' ' + (value10 << 8) + ((value - value10 * 10) << 16) + (':' << 24)) | 0x303000)) return 1;
                value10 = (value1000 * (int)NumberExtension.Div10_16Mul) >> NumberExtension.Div10_16Shift;
                value = (value100 * (int)NumberExtension.Div10_16Mul) >> NumberExtension.Div10_16Shift;
                return (*(int*)(data + sizeof(int) * 5) ^ ((value10 + ((value1000 - value10 * 10) << 8) + (':' << 16) + (value << 24)) | 0x30003030))
                    | (*(int*)(data + sizeof(int) * 6) ^ ((value100 - value * 10) + '0') + (' ' << 8) + ('G' << 16) + ('M' << 24));
            }
        }
        /// <summary>
        /// 时间转换成日期字符串(yyyy/MM/dd)
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static string toDateString(this DateTime time, char split = '/')
        {
            string timeString = AutoCSer.Extensions.StringExtension.FastAllocateString(10);
            fixed (char* timeFixed = timeString) toString(time, timeFixed, split);
            return timeString;
        }

        /// <summary>
        /// 每毫秒时间戳
        /// </summary>
        internal static readonly long TimestampPerMillisecond = Stopwatch.IsHighResolution ? Stopwatch.Frequency / 1000 : TimeSpan.TicksPerMillisecond;
        /// <summary>
        /// 每秒 毫秒时间戳误差
        /// </summary>
        internal static readonly long MillisecondTimestampDifferencePerSecond = Stopwatch.IsHighResolution ? Stopwatch.Frequency - Stopwatch.Frequency / 1000 * 1000 : 0;
        /// <summary>
        /// 获取初始化时间差
        /// </summary>
        /// <returns></returns>
        internal static long TimestampDifference
        {
            get
            {
                if (Stopwatch.IsHighResolution) return Stopwatch.GetTimestamp() - StartTimestamp;
                return DateTime.UtcNow.Ticks - StartTimestamp;
            }
        }
        ///// <summary>
        ///// 获取初始化时间差
        ///// </summary>
        //internal static long TimestampDifferenceTicks
        //{
        //    get
        //    {
        //        if (Stopwatch.IsHighResolution)
        //        {
        //            return (Stopwatch.GetTimestamp() - startTimestamp) * TimeSpan.TicksPerSecond / Stopwatch.Frequency;
        //        }
        //        return Stopwatch.GetTimestamp() - startTimestamp;
        //    }
        //}
        /// <summary>
        /// 时钟周期转时间戳乘数
        /// </summary>
        private static readonly double ticksToTimestamp = Stopwatch.IsHighResolution ? (double)Stopwatch.Frequency / TimeSpan.TicksPerSecond : 1;
        /// <summary>
        /// 时钟周期转时间戳
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static long GetTimestampByTicks(long ticks)
        {
            return Stopwatch.IsHighResolution ? (long)(ticks * ticksToTimestamp) : ticks;
        }
        /// <summary>
        /// 时间戳转毫秒数乘数
        /// </summary>
        private static readonly double timestampToMilliseconds = Stopwatch.IsHighResolution ? 1000 / (double)Stopwatch.Frequency : (1 / (double)TimeSpan.TicksPerMillisecond);
        /// <summary>
        /// 时间戳转毫秒数
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static long GetMillisecondsByTimestamp(long timestamp)
        {
            if (Stopwatch.IsHighResolution) return (long)(timestamp * timestampToMilliseconds);
            return timestamp / TimeSpan.TicksPerMillisecond;
        }
        /// <summary>
        /// 毫秒数转时间戳乘数
        /// </summary>
        private static readonly double millisecondsToTimestamp = Stopwatch.IsHighResolution ? (double)Stopwatch.Frequency / 1000 : TimeSpan.TicksPerMillisecond;
        /// <summary>
        /// 毫秒数转时间戳
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static long GetTimestampByMilliseconds(long milliseconds)
        {
            if (Stopwatch.IsHighResolution) return (long)(milliseconds * millisecondsToTimestamp);
            return milliseconds * TimeSpan.TicksPerMillisecond;
        }
        /// <summary>
        /// 获取时钟周期差值（不处理溢出）
        /// </summary>
        /// <param name="startTimestamp"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static TimeSpan GetTimestampTimeSpan(long startTimestamp)
        {
            return new TimeSpan(GetTicksByTimestamp(Stopwatch.GetTimestamp() - startTimestamp));
        }
        /// <summary>
        /// 时间戳转时钟周期
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static long GetTicksByTimestamp(long timestamp)
        {
            return Stopwatch.IsHighResolution ? timestamp * TimeSpan.TicksPerSecond / Stopwatch.Frequency : timestamp;
        }
    }
}
