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
        internal SubArray<byte> Path;
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
            Path = new SubArray<byte>(startIndex, length, path);
            this.pathIdentity = HashCode = pathIdentity;
            if (length != 0)
            {
                fixed (byte* dataFixed = path) HashCode ^= AutoCSer.Memory.Common.GetHashCode(dataFixed + startIndex, length) ^ Random.Hash;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public unsafe bool Equals(FileCacheKey other)
        {
            if (Path.Array == null) return other.Path.Array == null;
            if (other.Path.Array != null && ((HashCode ^ other.HashCode) | (pathIdentity ^ other.pathIdentity) | (Path.Length ^ other.Path.Length)) == 0)
            {
                if (Path.Array == other.Path.Array && Path.Start == other.Path.Start) return true;
                fixed (byte* dataFixed = Path.GetFixedBuffer(), otherDataFixed = other.Path.GetFixedBuffer())
                {
                    return AutoCSer.Memory.Common.EqualNotNull(dataFixed + Path.Start, otherDataFixed + other.Path.Start, Path.Length);
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
            if (Path.Length != 0)
            {
                byte[] data = Path.GetArray();
                Path.Set(data, 0, data.Length);
            }
        }
    }
}
