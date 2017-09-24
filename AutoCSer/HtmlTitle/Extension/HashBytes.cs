using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// HASH 的字节数组扩展
    /// </summary>
    public static class HashBytes_HtmlTitle
    {
        /// <summary>
        /// 复制 HASH 字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns>HASH 字节数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static HashBytes Copy(this HashBytes value)
        {
            return new HashBytes { SubArray = new SubArray<byte>(value.SubArray.GetArray()), HashCode = value.HashCode };
        }
    }
}
