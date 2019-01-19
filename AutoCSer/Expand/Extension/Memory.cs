using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 内存或字节数组处理
    /// </summary>
    internal unsafe static class Memory_Expand
    {
        /// <summary>
        /// 获取整数
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static short GetShortBigEndian(byte* start)
        {
            short value;
            (&value)[0] = start[1];
            (&value)[1] = start[0];
            return value;
        }
        /// <summary>
        /// 获取整数
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static int GetIntBigEndian(byte* start)
        {
            int value;
            (&value)[0] = start[3];
            (&value)[1] = start[2];
            (&value)[2] = start[1];
            (&value)[3] = start[0];
            return value;
        }
        /// <summary>
        /// 获取整数
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static long GetLongBigEndian(byte* start)
        {
            long value;
            (&value)[0] = start[7];
            (&value)[1] = start[6];
            (&value)[2] = start[5];
            (&value)[3] = start[4];
            (&value)[4] = start[3];
            (&value)[5] = start[2];
            (&value)[6] = start[1];
            (&value)[7] = start[0];
            return value;
        }
        /// <summary>
        /// 获取整数
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ushort GetUShortBigEndian(byte* start)
        {
            return (ushort)(((uint)start[0] << 8) + (uint)start[1]);
        }
        /// <summary>
        /// 获取整数
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static uint GetUIntBigEndian(byte* start)
        {
            return ((uint)start[0] << 24) + ((uint)start[1] << 16) + ((uint)start[2] << 8) + (uint)start[3];
        }
        /// <summary>
        /// 获取整数
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ulong GetULongBigEndian(byte* start)
        {
            return ((ulong)start[0] << 56) + ((ulong)start[1] << 48) + ((ulong)start[2] << 40) + ((ulong)start[3] << 32)
                + ((ulong)start[4] << 24) + ((ulong)start[5] << 16) + ((ulong)start[6] << 8) + (ulong)start[7];
        }
    }
}
