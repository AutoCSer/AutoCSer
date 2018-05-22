using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 加载数据
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct LoadData
    {
        /// <summary>
        /// 数据起始位置
        /// </summary>
        internal byte* DataFixed;
        /// <summary>
        /// 当前加载位置
        /// </summary>
        internal byte* Start;
        /// <summary>
        /// 数据结束位置
        /// </summary>
        private byte* end;
        /// <summary>
        /// 是否存在下一个数据
        /// </summary>
        internal bool IsNext
        {
            get { return Start != end; }
        }
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal Buffer Buffer;
        /// <summary>
        /// 设置加载数据
        /// </summary>
        /// <param name="data">加载数据</param>
        /// <param name="index">当前加载位置</param>
        /// <param name="size">数据字节长度</param>
        /// <param name="dataFixed">数据起始位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(byte[] data, int index, int size, byte* dataFixed)
        {
            Buffer.Array.Set(data, 0, 0);
            Start = dataFixed + index;
            DataFixed = dataFixed;
            end = Start + size;
        }
        /// <summary>
        /// 设置加载数据
        /// </summary>
        /// <param name="data">加载数据</param>
        /// <param name="dataFixed">数据起始位置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ref SubArray<byte> data, byte* dataFixed)
        {
            Buffer.Array = data;
            Start = dataFixed + data.Start;
            DataFixed = dataFixed;
            end = Start + data.Count;
        }
        /// <summary>
        /// 移动到下一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal byte* MoveNext()
        {
            byte* end = Start + *(int*)Start;
            if (end <= this.end)
            {
                Buffer.Array.Set((int)(Start - DataFixed), (int)(end - Start));
                Start = end;
                return end;
            }
            return null;
        }
    }
}
