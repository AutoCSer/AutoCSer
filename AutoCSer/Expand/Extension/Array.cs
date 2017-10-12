using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数组扩展
    /// </summary>
    public static partial class Array_Expand
    {
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="array">字符串集合</param>
        /// <param name="join">字符连接</param>
        /// <param name="nullString">null 值替换字符串</param>
        /// <returns>连接后的字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string joinString<valueType>(this valueType[] array, char join, string nullString = null)
        {
            return AutoCSer.NumberToCharStream.Join<valueType>.JoinString(array, join, nullString);
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="array">字符串集合</param>
        /// <param name="join">字符连接</param>
        /// <param name="nullString">null 值替换字符串</param>
        /// <returns>连接后的字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string joinString<valueType>(this SubArray<valueType> array, char join, string nullString = null)
        {
            return AutoCSer.NumberToCharStream.Join<valueType>.JoinString(ref array, join, nullString);
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="array">字符串集合</param>
        /// <param name="join">字符连接</param>
        /// <param name="nullString">null 值替换字符串</param>
        /// <returns>连接后的字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string joinString<valueType>(this LeftArray<valueType> array, char join, string nullString = null)
        {
            return AutoCSer.NumberToCharStream.Join<valueType>.JoinString(ref array, join, nullString);
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="array">字符串集合</param>
        /// <param name="stream">字符流</param>
        /// <param name="join">字符连接</param>
        /// <param name="nullString">null 值替换字符串</param>
        /// <returns>连接后的字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void joinString(this int[] array, CharStream stream, char join, string nullString = null)
        {
            if (nullString == null) nullString = string.Empty;
            if (array.length() == 0)
            {
                if (nullString.Length != 0) stream.WriteNotNull(nullString);
            }
            else AutoCSer.NumberToCharStream.Join<int>.NumberJoinChar(stream, array, 0, array.Length, join, nullString);
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="array">字符串集合</param>
        /// <param name="stream">字符流</param>
        /// <param name="join">字符连接</param>
        /// <param name="nullString">null 值替换字符串</param>
        /// <returns>连接后的字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void joinString(this LeftArray<int> array, CharStream stream, char join, string nullString = null)
        {
            if (nullString == null) nullString = string.Empty;
            if (array.Length == 0)
            {
                if (nullString.Length != 0) stream.WriteNotNull(nullString);
            }
            else AutoCSer.NumberToCharStream.Join<int>.NumberJoinChar(stream, array.Array, 0, array.Length, join, nullString);
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="array">字符串集合</param>
        /// <param name="stream">字符流</param>
        /// <param name="join">字符连接</param>
        /// <param name="nullString">null 值替换字符串</param>
        /// <returns>连接后的字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void joinString(this SubArray<int> array, CharStream stream, char join, string nullString = null)
        {
            if (nullString == null) nullString = string.Empty;
            if (array.Length == 0)
            {
                if (nullString.Length != 0) stream.WriteNotNull(nullString);
            }
            else AutoCSer.NumberToCharStream.Join<int>.NumberJoinChar(stream, array.Array, array.Start, array.Length, join, nullString);
        }
    }
}
