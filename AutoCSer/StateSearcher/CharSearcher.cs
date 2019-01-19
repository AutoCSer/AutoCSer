using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.StateSearcher
{
    /// <summary>
    /// 字符搜索器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe partial struct CharSearcher
    {
        /// <summary>
        /// 状态集合
        /// </summary>
        private readonly byte* state;
        /// <summary>
        /// ASCII字符查找表
        /// </summary>
        private readonly byte* charsAscii;
        /// <summary>
        /// 特殊字符串查找表
        /// </summary>
        private readonly byte* charStart;
        /// <summary>
        /// 特殊字符串查找表结束位置
        /// </summary>
        private readonly byte* charEnd;
        /// <summary>
        /// 特殊字符起始值
        /// </summary>
        private readonly int charIndex;
        /// <summary>
        /// 查询矩阵单位尺寸类型
        /// </summary>
        private readonly byte tableType;
        /// <summary>
        /// 字符搜索器
        /// </summary>
        /// <param name="data">数据起始位置</param>
        public CharSearcher(Pointer data)
        {
            if (data.Data == null)
            {
                state = charsAscii = charStart = charEnd = null;
                charIndex = 0;
                tableType = 0;
            }
            else
            {
                int stateCount = *data.Int;
                state = data.Byte + sizeof(int);
                charsAscii = state + stateCount * 3 * sizeof(int);
                charStart = charsAscii + 128 * sizeof(ushort);
                charIndex = *(ushort*)charStart;
                charStart += sizeof(ushort) * 2;
                charEnd = charStart + *(ushort*)(charStart - sizeof(ushort)) * sizeof(ushort);
                if (stateCount < 256) tableType = 0;
                else if (stateCount < 65536) tableType = 1;
                else tableType = 2;
            }
        }
        /// <summary>
        /// 获取特殊字符索引值
        /// </summary>
        /// <param name="value">特殊字符</param>
        /// <returns>索引值,匹配失败返回0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int getCharIndex(char value)
        {
            char* current = GetCharIndex((char*)charStart, (char*)charEnd, value);
            return current == null ? 0 : (charIndex + (int)(current - (char*)charStart));
        }
        /// <summary>
        /// 获取特殊字符索引值
        /// </summary>
        /// <param name="charStart">特殊字符串查找表</param>
        /// <param name="charEnd">特殊字符串查找表结束位置</param>
        /// <param name="value">特殊字符</param>
        /// <returns>特殊字符位置,匹配失败返回null</returns>
        internal static char* GetCharIndex(char* charStart, char* charEnd, char value)
        {
            char* current = charStart + ((int)(charEnd - charStart) >> 1);
            while (*current != value)
            {
                if (value < *current)
                {
                    if (current == charStart) return null;
                    charEnd = current;
                    current = charStart + ((int)(charEnd - charStart) >> 1);
                }
                else
                {
                    if ((charStart = current + 1) == charEnd) return null;
                    current = charStart + ((int)(charEnd - charStart) >> 1);
                }
            }
            return current;
        }
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="value"></param>
        /// <returns>状态索引,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe int Search(string value)
        {
            if (state == null || value == null) return -1;
            fixed (char* valueFixed = value) return UnsafeSearch(valueFixed, valueFixed + value.Length);
        }
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="start">匹配起始位置</param>
        /// <param name="end">匹配结束位置</param>
        /// <returns>状态索引,失败返回-1</returns>
        public int UnsafeSearch(char* start, char* end)
        {
            if (state == null) return -1;
            byte* currentState = state;
            do
            {
                char* prefix = (char*)(currentState + *(int*)currentState);
                if (*prefix != 0)
                {
                    if (start == end || *start != *prefix) return -1;
                    while (*++prefix != 0)
                    {
                        if (++start == end || *start != *prefix) return -1;
                    }
                    ++start;
                }
                if (start == end) return *(int*)(currentState + sizeof(int) * 2);
                if (*(int*)(currentState + sizeof(int)) == 0) return -1;
                int index = *start < 128 ? (int)*(ushort*)(charsAscii + (*start << 1)) : getCharIndex(*start);
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
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="start">匹配起始位置</param>
        /// <param name="length">匹配长度</param>
        /// <returns>状态索引,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int UnsafeSearch(char* start, int length)
        {
            return UnsafeSearch(start, start + length);
        }
    }
}
