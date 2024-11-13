using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 内存或字节数组处理
    /// </summary>
    internal unsafe static class MemoryExtensionExpand
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
            byte* data = (byte*)&value;
            data[0] = start[1];
            data[1] = start[0];
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
            byte* data = (byte*)&value;
            data[0] = start[3];
            data[1] = start[2];
            data[2] = start[1];
            data[3] = start[0];
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
            byte* data = (byte*)&value;
            data[0] = start[7];
            data[1] = start[6];
            data[2] = start[5];
            data[3] = start[4];
            data[4] = start[3];
            data[5] = start[2];
            data[6] = start[1];
            data[7] = start[0];
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
            uint value;
            byte* data = (byte*)&value;
            data[0] = start[3];
            data[1] = start[2];
            data[2] = start[1];
            data[3] = start[0];
            return value;
        }
        /// <summary>
        /// 获取整数
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ulong GetULongBigEndian(byte* start)
        {
            ulong value;
            byte* data = (byte*)&value;
            data[0] = start[7];
            data[1] = start[6];
            data[2] = start[5];
            data[3] = start[4];
            data[4] = start[3];
            data[5] = start[2];
            data[6] = start[1];
            data[7] = start[0];
            return value;
        }
    }
}
