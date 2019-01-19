using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 名称状态查找器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct HeaderQueryParseStateSearcher
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
        /// 查询矩阵单位尺寸类型
        /// </summary>
        private readonly byte tableType;
        /// <summary>
        /// 名称查找器
        /// </summary>
        /// <param name="data">数据起始位置</param>
        internal HeaderQueryParseStateSearcher(Pointer data)
        {
            if (data.Data == null)
            {
                state = charsAscii = charStart = null;//charEnd = 
                //charIndex = 0;
                tableType = 0;
            }
            else
            {
                int stateCount = *data.Int;
                state = data.Byte + sizeof(int);
                charsAscii = state + stateCount * 3 * sizeof(int);
                charStart = charsAscii + 128 * sizeof(ushort);
                //charIndex = *(ushort*)charStart;
                charStart += sizeof(ushort) * 2;
                //charEnd = charStart + *(ushort*)(charStart - sizeof(ushort)) * sizeof(ushort);
                if (stateCount < 256) tableType = 0;
                else if (stateCount < 65536) tableType = 1;
                else tableType = 2;
            }
        }
        /// <summary>
        /// 获取名称索引
        /// </summary>
        /// <param name="parser">查询解析器</param>
        /// <returns>名称索引,失败返回-1</returns>
        internal int SearchName(HeaderQueryParser parser)
        {
            if (state == null) return -1;
            byte value = parser.GetName();
            if (value == 0) return *(int*)(state + sizeof(int) * 2);
            byte* currentState = state;
            do
            {
                char* prefix = (char*)(currentState + *(int*)currentState);
                if (*prefix != 0)
                {
                    if (value != *prefix) return -1;
                    while (*++prefix != 0)
                    {
                        if (parser.GetName() != *prefix) return -1;
                    }
                    value = parser.GetName();
                }
                if (value == 0) return *(int*)(currentState + sizeof(int) * 2);
                if (*(int*)(currentState + sizeof(int)) == 0 || value >= 128) return -1;
                int index = (int)*(ushort*)(charsAscii + (value << 1));
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
                value = parser.GetName();
            }
            while (true);
        }
    }
}