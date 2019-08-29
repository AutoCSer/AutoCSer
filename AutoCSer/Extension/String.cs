using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 字符串相关操作
    /// </summary>
    internal unsafe static partial class StringExtension
    {
        /// <summary>
        /// 复制字符串
        /// </summary>
        /// <param name="source">原字符串,不能为null</param>
        /// <param name="destination">目标字符串地址,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void CopyNotNull(string source, void* destination)
        {
            fixed (char* sourceFixed = source) Memory.CopyNotNull(sourceFixed, destination, source.Length << 1);
        }
        /// <summary>
        /// 复制字符串
        /// </summary>
        /// <param name="source">原字符串,不能为null</param>
        /// <param name="destination">目标字符串地址,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void SimpleCopyNotNull(string source, void* destination)
        {
            fixed (char* sourceFixed = source) SimpleCopyNotNull(sourceFixed, (char*)destination, source.Length);
        }
        /// <summary>
        /// 复制字符数组
        /// </summary>
        /// <param name="source">原串起始地址,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="count">字符数量,大于等于0</param>
        internal static void SimpleCopyNotNull(char* source, char* destination, int count)
        {
            for (char* end = source + (count & (int.MaxValue - (sizeof(uint) - 1))); source != end; source += sizeof(uint), destination += sizeof(uint))
            {
                *(ulong*)destination = *(ulong*)source;
            }
            if ((((count &= (sizeof(uint) - 1))) & sizeof(ushort)) != 0)
            {
                *(uint*)destination = *(uint*)source;
                source += sizeof(ushort);
                destination += sizeof(ushort);
            }
            if ((count & 1) != 0) *(ushort*)destination = *(ushort*)source;
        }
        /// <summary>
        /// 复制字符数组(不足8字节按8字节算)
        /// </summary>
        /// <param name="source">原串起始地址,不能为null</param>
        /// <param name="destination">目标串起始地址,不能为null</param>
        /// <param name="count">字符数量,大于等于0</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void SimpleCopyNotNull64(char* source, char* destination, int count)
        {
            if (count > 0) Memory.SimpleCopyNotNull64((byte*)source, (byte*)destination, count << 1);
        }
        /// <summary>
        /// 获取Ascii字符串原始字节流
        /// </summary>
        /// <param name="value">字符串,不能为null</param>
        /// <param name="length">字符串长度</param>
        /// <param name="write">写入位置,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void WriteBytesNotNull(char* value, int length, byte* write)
        {
            for (char* start = value, end = value + length; start != end; ++start) *write++ = *(byte*)start;
        }

        /// <summary>
        /// 申请字符串空间
        /// </summary>
        internal static readonly Func<int, string> FastAllocateString;
        static StringExtension()
        {
            MethodInfo method = typeof(string).GetMethod("FastAllocateString", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(int) }, null);
            if (method == null) FastAllocateString = (count) => new string((char)0, count);
            else FastAllocateString = (Func<int, string>)Delegate.CreateDelegate(typeof(Func<int, string>), method);
        }
    }
}
