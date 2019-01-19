using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.HttpRegister
{
    /// <summary>
    /// 字节数组搜索器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct DomainSearcher
    {
        /// <summary>
        /// 状态集合
        /// </summary>
        internal byte* State;
        /// <summary>
        /// 字节查找表
        /// </summary>
        private readonly byte* bytes;
        /// <summary>
        /// 查询矩阵单位尺寸类型
        /// </summary>
        private readonly byte tableType;
        /// <summary>
        /// ASCII字节搜索器
        /// </summary>
        /// <param name="data">数据起始位置</param>
        public DomainSearcher(ref Pointer.Size data)
        {
            int stateCount = *data.Int;
            State = data.Byte + sizeof(int);
            bytes = State + stateCount * 3 * sizeof(int);
            if (stateCount < 256) tableType = 0;
            else if (stateCount < 65536) tableType = 1;
            else tableType = 2;
        }
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="end">匹配起始位置</param>
        /// <param name="start">匹配结束位置</param>
        /// <returns>状态索引,失败返回-1</returns>
        internal int Search(byte* start, byte* end)
        {
            int dotIndex = -1, value = 0;
            byte* currentState = State;
            do
            {
                byte* prefix = currentState + *(int*)currentState;
                int prefixSize = *(ushort*)(prefix - sizeof(ushort));
                if (prefixSize != 0)
                {
                    for (byte* endPrefix = prefix + prefixSize; prefix != endPrefix; ++prefix)
                    {
                        if (end == start) return dotIndex;
                        if ((uint)((value = *--end) - 'A') < 26) value |= 0x20;
                        if (value != *prefix) return dotIndex;
                    }
                }
                if (end == start) return *(int*)(currentState + sizeof(int) * 2);
                if (value == '.' && (value = *(int*)(currentState + sizeof(int) * 2)) >= 0) dotIndex = value;
                if (*(int*)(currentState + sizeof(int)) == 0) return dotIndex;
                if ((uint)((value = *--end) - 'A') < 26) value |= 0x20;
                int index = (int)*(bytes + value);
                byte* table = currentState + *(int*)(currentState + sizeof(int));
                if (tableType == 0)
                {
                    if ((index = *(table + index)) == 0) return dotIndex;
                    currentState = State + index * 3 * sizeof(int);
                }
                else if (tableType == 1)
                {
                    if ((index = (int)*(ushort*)(table + index * sizeof(ushort))) == 0) return dotIndex;
                    currentState = State + index * 3 * sizeof(int);
                }
                else
                {
                    if ((index = *(int*)(table + index * sizeof(int))) == 0) return dotIndex;
                    currentState = State + index;
                }
            }
            while (true);
        }
        ///// <summary>
        ///// 获取状态索引
        ///// </summary>
        ///// <param name="data">匹配状态</param>
        ///// <returns>状态索引,失败返回-1</returns>
        //[MethodImpl((MethodImplOptions)AutoCSer.pub.MethodImplOptionsAggressiveInlining)]
        //public int Search(ref subArray<byte> data)
        //{
        //    fixed (byte* dataFixed = data.array)
        //    {
        //        byte* start = dataFixed + data.StartIndex;
        //        return Search(start, start + data.Count);
        //    }
        //}
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="data">匹配状态</param>
        /// <returns>状态索引,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int Search(byte[] data)
        {
            fixed (byte* dataFixed = data) return Search(dataFixed, dataFixed + data.Length);
        }
    }
}
