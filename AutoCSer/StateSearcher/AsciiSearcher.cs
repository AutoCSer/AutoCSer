using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.StateSearcher
{
    /// <summary>
    /// ASCII 字节搜索器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe partial struct AsciiSearcher
    {
        /// <summary>
        /// 状态集合
        /// </summary>
        internal byte* State;
        /// <summary>
        /// ASCII字符查找表
        /// </summary>
        private readonly byte* charsAscii;
        /// <summary>
        /// 查询矩阵单位尺寸类型
        /// </summary>
        private readonly byte tableType;
        /// <summary>
        /// ASCII字节搜索器
        /// </summary>
        /// <param name="data">数据起始位置</param>
        internal AsciiSearcher(Pointer data)
        {
            if (data.Data == null)
            {
                State = charsAscii = null;
                tableType = 0;
            }
            else
            {
                int stateCount = *data.Int;
                State = data.Byte + sizeof(int);
                charsAscii = State + stateCount * 3 * sizeof(int);
                if (stateCount < 256) tableType = 0;
                else if (stateCount < 65536) tableType = 1;
                else tableType = 2;
            }
        }
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="start">匹配起始位置</param>
        /// <returns>状态索引,失败返回-1</returns>
        internal int UnsafeSearch(ref char* start)
        {
            byte* currentState = State;
            do
            {
                for (byte* prefix = currentState + *(int*)currentState; *prefix != 0; ++prefix, ++start)
                {
                    if (*start != *prefix) return -1;
                }
                if (*(int*)(currentState + sizeof(int)) == 0) return *(int*)(currentState + sizeof(int) * 2);
                if (*start >= 128) return -1;
                int index = (int)*(charsAscii + *start);
                byte* table = currentState + *(int*)(currentState + sizeof(int));
                if (tableType == 0)
                {
                    if ((index = *(table + index)) == 0) return -1;
                    currentState = State + index * 3 * sizeof(int);
                }
                else if (tableType == 1)
                {
                    if ((index = (int)*(ushort*)(table + index * sizeof(ushort))) == 0) return -1;
                    currentState = State + index * 3 * sizeof(int);
                }
                else
                {
                    if ((index = *(int*)(table + index * sizeof(int))) == 0) return -1;
                    currentState = State + index;
                }
                ++start;
            }
            while (true);
        }
    }
}
