using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.HttpDomainServer
{
    /// <summary>
    /// 文件缓存关键字
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct FileCacheKey : IEquatable<FileCacheKey>
    {
        /// <summary>
        /// 字节数组
        /// </summary>
        private SubArray<byte> path;
        /// <summary>
        /// 路径标识
        /// </summary>
        private int pathIdentity;
        /// <summary>
        /// HASH 值
        /// </summary>
        internal int HashCode;
        /// <summary>
        /// 文件缓存关键字
        /// </summary>
        /// <param name="pathIdentity"></param>
        /// <param name="path"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        internal FileCacheKey(int pathIdentity, byte[] path, int startIndex, int length)
        {
            this.path = new SubArray<byte> { Array = path, Start = startIndex, Length = length };
            this.pathIdentity = HashCode = pathIdentity;
            if (length != 0)
            {
                fixed (byte* dataFixed = path) HashCode ^= Memory.GetHashCode(dataFixed + startIndex, length) ^ Random.Hash;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public unsafe bool Equals(FileCacheKey other)
        {
            if (path.Array == null) return other.path.Array == null;
            if (other.path.Array != null && ((HashCode ^ other.HashCode) | (pathIdentity ^ other.pathIdentity) | (path.Length ^ other.path.Length)) == 0)
            {
                if (path.Array == other.path.Array && path.Start == other.path.Start) return true;
                fixed (byte* dataFixed = path.Array, otherDataFixed = other.path.Array)
                {
                    return Memory.EqualNotNull(dataFixed + path.Start, otherDataFixed + other.path.Start, path.Length);
                }
            }
            return false;
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
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            return Equals((FileCacheKey)other);
        }
        /// <summary>
        /// 复制文件缓存关键字
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CopyPath()
        {
            if (path.Length != 0)
            {
                byte[] data = path.GetArray();
                path.Set(data, 0, data.Length);
            }
        }
    }
}
