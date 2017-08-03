using System;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 磁盘块索引位置
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct MemberIndex : IEquatable<MemberIndex>
    {
        /// <summary>
        /// 磁盘块索引位置
        /// </summary>
        internal ulong Index;
        /// <summary>
        /// 数据长度
        /// </summary>
        internal int Size;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(MemberIndex other)
        {
            return Index == other.Index && Size == other.Size;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)(uint)Index ^ (int)(uint)(Index >> 32) ^ Size;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((MemberIndex)obj);
        }
    }
}
