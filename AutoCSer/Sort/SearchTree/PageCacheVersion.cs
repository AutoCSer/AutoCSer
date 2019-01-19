using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 分页缓存版本
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct PageCacheVersion<valueType>
    {
        /// <summary>
        /// 分页数据
        /// </summary>
        internal valueType[] Array;
        /// <summary>
        /// 更新版本
        /// </summary>
        internal int Version;
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="version">更新版本</param>
        /// <returns>分页数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType[] Get(int version)
        {
            return Version == version ? Array : null;
        }
        /// <summary>
        /// 设置分页数据
        /// </summary>
        /// <param name="array">分页数据</param>
        /// <param name="version">更新版本</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(valueType[] array, int version)
        {
            Array = array;
            Version = version;
        }
    }
}
