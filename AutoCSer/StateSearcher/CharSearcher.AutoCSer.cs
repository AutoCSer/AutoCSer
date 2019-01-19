using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.StateSearcher
{
    /// <summary>
    /// 字符搜索器
    /// </summary>
    internal unsafe partial struct CharSearcher
    {
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="value"></param>
        /// <returns>状态索引,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe int SearchLower(string value)
        {
            if (state == null || value == null) return -1;
            fixed (char* valueFixed = value) return SearchLower(valueFixed, valueFixed + value.Length);
        }
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="start">匹配起始位置</param>
        /// <param name="end">匹配结束位置</param>
        /// <returns>状态索引,失败返回-1</returns>
        internal int SearchLower(char* start, char* end)
        {
            if (state == null) return -1;
            byte* currentState = state;
            do
            {
                char* prefix = (char*)(currentState + *(int*)currentState);
                if (*prefix != 0)
                {
                    if (start == end) return -1;
                    if (*start != *prefix)
                    {
                        if ((uint)(*start - 'A') >= 26 || (*start | 0x20) != *prefix) return -1;
                    }
                    while (*++prefix != 0)
                    {
                        if (++start == end) return -1;
                        if (*start != *prefix)
                        {
                            if ((uint)(*start - 'A') >= 26 || (*start | 0x20) != *prefix) return -1;
                        }
                    }
                    ++start;
                }
                if (start == end) return *(int*)(currentState + sizeof(int) * 2);
                if (*(int*)(currentState + sizeof(int)) == 0) return -1;
                int index = *start < 128 ? (int)*(ushort*)(charsAscii + (((uint)(*start - 'A') < 26 ? (*start | 0x20) : *start) << 1)) : getCharIndex(*start);
                byte* table = currentState + *(int*)(currentState + sizeof(int));
                if (tableType == 0)
                {
                    if ((index = *(table + index)) == 0) return -1;
                    currentState = state + index * 3 * sizeof(int);
                }
                else if (tableType == 1)
                {
                    if ((index = (int)*(ushort*)(table + index * sizeof(ushort))) == 0) return -1;
                    currentState = state + index * 3 * sizeof(int);
                }
                else
                {
                    if ((index = *(int*)(table + index * sizeof(int))) == 0) return -1;
                    currentState = state + index;
                }
                ++start;
            }
            while (true);
        }
    }
}
