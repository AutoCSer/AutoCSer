using System;

namespace fastCSharp
{
    /// <summary>
    /// 字符子串
    /// </summary>
    public unsafe partial struct SubString
    {
        /// <summary>
        /// 设置数据长度
        /// </summary>
        /// <param name="startIndex">起始位置,必须合法</param>
        /// <param name="length">长度,必须合法</param>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        internal void Set(int startIndex, int length)
        {
            StartIndex = startIndex;
            Length = length;
        }
    }
}
