using System;
using System.Collections.Generic;

namespace fastCSharp
{
    /// <summary>
    /// 创建 HashSet
    /// </summary>
    public static class HashSetCreator
    {
        /// <summary>
        /// 创建HASH表
        /// </summary>
        /// <returns>HASH表</returns>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        public static HashSet<int> CreateInt()
        {
#if __IOS__
            return new HashSet<int>(EqualityComparer.Int);
#else
            return new HashSet<int>();
#endif
        }
    }
}
