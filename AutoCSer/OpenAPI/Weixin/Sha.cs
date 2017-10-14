using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// SHA
    /// </summary>
    internal static class SHA
    {
        /// <summary>
        /// SHA1哈希加密
        /// </summary>
        private static readonly SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
        /// <summary>
        /// SHA1哈希加密访问锁
        /// </summary>
        private static readonly object sha1Lock = new object();
        /// <summary>
        /// SHA1哈希加密
        /// </summary>
        /// <param name="buffer">数据</param>
        /// <returns>SHA1哈希</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static byte[] Sha1(byte[] buffer)
        {
            return Sha1(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// SHA1哈希加密
        /// </summary>
        /// <param name="buffer">数据</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">数据长度</param>
        /// <returns>SHA1哈希</returns>
        internal static byte[] Sha1(byte[] buffer, int startIndex, int length)
        {
            Monitor.Enter(sha1Lock);
            try
            {
                buffer = sha1.ComputeHash(buffer, startIndex, length);
            }
            finally { Monitor.Exit(sha1Lock); }
            return buffer;
        }
    }
}
