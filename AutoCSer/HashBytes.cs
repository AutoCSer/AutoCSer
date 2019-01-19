using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 用于 HASH 的字节数组
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct HashBytes : IEquatable<HashBytes>//, IEquatable<subArray<byte>>, IEquatable<byte[]>
    {
        /// <summary>
        /// 字节数组
        /// </summary>
        internal SubArray<byte> SubArray;
        /// <summary>
        /// HASH 值
        /// </summary>
        internal int HashCode;
        /// <summary>
        /// 字节数组 HASH
        /// </summary>
        /// <param name="data">字节数组</param>
        public unsafe HashBytes(ref SubArray<byte> data)
        {
            this.SubArray = data;
            if (data.Length == 0) HashCode = 0;
            else
            {
                fixed (byte* dataFixed = data.Array) HashCode = Memory.GetHashCode(dataFixed + data.StartIndex, data.Length) ^ Random.Hash;
            }
        }
        /// <summary>
        /// HASH字节数组隐式转换
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>HASH字节数组</returns>
        public static implicit operator HashBytes(SubArray<byte> data) { return new HashBytes(ref data); }
        /// <summary>
        /// HASH字节数组隐式转换
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>HASH字节数组</returns>
        public static implicit operator HashBytes(byte[] data)
        {
            SubArray<byte> subArray = new SubArray<byte> { Array = data, Start = 0, Length = data.Length };
            return new HashBytes(ref subArray);
        }
        /// <summary>
        /// 比较字节数组是否相等
        /// </summary>
        /// <param name="other">用于HASH的字节数组</param>
        /// <returns>是否相等</returns>
        public unsafe bool Equals(HashBytes other)
        {
            return HashCode == other.HashCode && SubArray.equal(ref other.SubArray);
        }
        /// <summary>
        /// 获取 HASH 值
        /// </summary>
        /// <returns>HASH 值</returns>
        public override int GetHashCode()
        {
            return HashCode;
        }
        /// <summary>
        /// 比较字节数组是否相等
        /// </summary>
        /// <param name="other">字节数组HASH</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object other)
        {
            return Equals((HashBytes)other);
        }
        /// <summary>
        /// 复制 HASH 的字节数组
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CopyData()
        {
            SubArray = new SubArray<byte>(SubArray.GetArray());
        }
    }
}
