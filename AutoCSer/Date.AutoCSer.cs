using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

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
        private static Pointer weekData;
        /// <summary>
        /// 月份
        /// </summary>
        private static Pointer monthData;
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
            int value = date.Day, value10 = (value * (int)Number.Div10_16Mul) >> Number.Div10_16Shift;
            *(int*)(data + sizeof(int)) = (' ' + (value10 << 8) + ((value - value10 * 10) << 16) + (' ' << 24)) | 0x303000;
            value = date.Year;
            *(int*)(data + sizeof(int) * 2) = monthData.Int[date.Month - 1];
            value10 = (value * (int)Number.Div10_16Mul) >> Number.Div10_16Shift;
            int value100 = (value10 * (int)Number.Div10_16Mul) >> Number.Div10_16Shift;
            int value1000 = (value100 * (int)Number.Div10_16Mul) >> Number.Div10_16Shift;
            *(int*)(data + sizeof(int) * 3) = (value1000 + ((value100 - value1000 * 10) << 8) + ((value10 - value100 * 10) << 16) + ((value - value10 * 10) << 24)) | 0x30303030;

            value100 = (int)(date.Ticks % TimeSpan.TicksPerDay / (1000 * 10000));
            value1000 = (int)(((ulong)value100 * Div60_32Mul) >> Div60_32Shift);
            value100 -= value1000 * 60;
            value = (value1000 * (int)Div60_16Mul) >> Div60_16Shift;
            value1000 -= value * 60;

            value10 = (value * (int)Number.Div10_16Mul) >> Number.Div10_16Shift;
            *(int*)(data + sizeof(int) * 4) = (' ' + (value10 << 8) + ((value - value10 * 10) << 16) + (':' << 24)) | 0x303000;
            value10 = (value1000 * (int)Number.Div10_16Mul) >> Number.Div10_16Shift;
            value = (value100 * (int)Number.Div10_16Mul) >> Number.Div10_16Shift;
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
            fixed (byte* dataFixed = dataArray.Array)
            {
                byte* data = dataFixed + dataArray.Start;
                if (((*(int*)data ^ weekData.Int[(int)date.DayOfWeek]) | (*(data + sizeof(int) * 7) ^ (byte)'T')) != 0) return 1;
                int value = date.Day, value10 = (value * (int)Number.Div10_16Mul) >> Number.Div10_16Shift;
                if (*(int*)(data + sizeof(int)) != ((' ' + (value10 << 8) + ((value - value10 * 10) << 16) + (' ' << 24)) | 0x303000)) return 1;
                value = date.Year;
                if (*(int*)(data + sizeof(int) * 2) != monthData.Int[date.Month - 1]) return 1;
                value10 = (value * (int)Number.Div10_16Mul) >> Number.Div10_16Shift;
                int value100 = (value10 * (int)Number.Div10_16Mul) >> Number.Div10_16Shift;
                int value1000 = (value100 * (int)Number.Div10_16Mul) >> Number.Div10_16Shift;
                if (*(int*)(data + sizeof(int) * 3) != ((value1000 + ((value100 - value1000 * 10) << 8) + ((value10 - value100 * 10) << 16) + ((value - value10 * 10) << 24)) | 0x30303030)) return 1;


                value100 = (int)(date.Ticks % TimeSpan.TicksPerDay / (1000 * 10000));
                value1000 = (int)(((ulong)value100 * Div60_32Mul) >> Div60_32Shift);
                value100 -= value1000 * 60;
                value = (value1000 * (int)Div60_16Mul) >> Div60_16Shift;
                value1000 -= value * 60;

                value10 = (value * (int)Number.Div10_16Mul) >> Number.Div10_16Shift;
                if (*(int*)(data + sizeof(int) * 4) != ((' ' + (value10 << 8) + ((value - value10 * 10) << 16) + (':' << 24)) | 0x303000)) return 1;
                value10 = (value1000 * (int)Number.Div10_16Mul) >> Number.Div10_16Shift;
                value = (value100 * (int)Number.Div10_16Mul) >> Number.Div10_16Shift;
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
            string timeString = AutoCSer.Extension.StringExtension.FastAllocateString(10);
            fixed (char* timeFixed = timeString) toString(time, timeFixed, split);
            return timeString;
        }
    }
}
