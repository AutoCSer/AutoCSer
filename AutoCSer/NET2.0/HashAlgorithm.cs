using System;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// HASH 算法
    /// </summary>
    internal static class HashAlgorithm
    {
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Dispose(this System.Security.Cryptography.HashAlgorithm hashAlgorithm)
        {
            ((IDisposable)hashAlgorithm).Dispose();
        }
    }
}

