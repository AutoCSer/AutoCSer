using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Json
{
    /// <summary>
    /// 名称状态查找器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct StateSearcher
    {
        /// <summary>
        /// 状态集合
        /// </summary>
        internal readonly byte* State;
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
        /// 名称查找器
        /// </summary>
        /// <param name="data">数据起始位置</param>
        internal StateSearcher(Pointer data)
        {
            if (data.Data == null)
            {
                State = charsAscii = charStart = charEnd = null;
                charIndex = 0;
                tableType = 0;
            }
            else
            {
                int stateCount = *data.Int;
                State = data.Byte + sizeof(int);
                charsAscii = State + stateCount * 3 * sizeof(int);
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
        /// 获取名称索引
        /// </summary>
        /// <param name="parser">JSON 解析器</param>
        /// <param name="isQuote">名称是否带引号</param>
        /// <returns>名称索引,失败返回-1</returns>
        internal int SearchName(Parser parser, out bool isQuote)
        {
            char value = parser.GetFirstName();
            if (State == null)
            {
                isQuote = parser.Quote != 0;
                return -1;
            }
            if (parser.Quote != 0)
            {
                isQuote = true;
                return searchString(parser, value);
            }
            isQuote = false;
            if (parser.ParseState != ParseState.Success) return -1;
            byte* currentState = State;
            do
            {
                char* prefix = (char*)(currentState + *(int*)currentState);
                if (*prefix != 0)
                {
                    if (value != *prefix) return -1;
                    while (*++prefix != 0)
                    {
                        if (parser.GetNextName() != *prefix) return -1;
                    }
                    value = parser.GetNextName();
                }
                if (value == 0) return parser.ParseState == ParseState.Success ? *(int*)(currentState + sizeof(int) * 2) : -1;
                if (*(int*)(currentState + sizeof(int)) == 0) return -1;
                int index = value < 128 ? (int)*(ushort*)(charsAscii + (value << 1)) : getCharIndex(value);
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
                value = parser.GetNextName();
            }
            while (true);
        }
        /// <summary>
        /// 根据字符串查找目标索引
        /// </summary>
        /// <param name="parser">JSON 解析器</param>
        /// <param name="value">第一个字符</param>
        /// <returns>目标索引,null返回-1</returns>
        internal int searchString(Parser parser, char value)
        {
            byte* currentState = State;
            do
            {
                char* prefix = (char*)(currentState + *(int*)currentState);
                if (*prefix != 0)
                {
                    if (value != *prefix) return -1;
                    while (*++prefix != 0)
                    {
                        if (parser.NextStringChar() != *prefix) return -1;
                    }
                    value = parser.NextStringChar();
                }
                if (value == 0) return parser.ParseState == ParseState.Success ? *(int*)(currentState + sizeof(int) * 2) : -1;
                if (*(int*)(currentState + sizeof(int)) == 0) return -1;
                int index = value < 128 ? (int)*(ushort*)(charsAscii + (value << 1)) : getCharIndex(value);
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
                value = parser.NextStringChar();
            }
            while (true);
        }
        /// <summary>
        /// 获取特殊字符索引值
        /// </summary>
        /// <param name="value">特殊字符</param>
        /// <returns>索引值,匹配失败返回0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int getCharIndex(char value)
        {
            char* current = AutoCSer.StateSearcher.CharSearcher.GetCharIndex((char*)charStart, (char*)charEnd, value);
            return current == null ? 0 : (charIndex + (int)(current - (char*)charStart));
        }
        /// <summary>
        /// 根据字符串查找目标索引
        /// </summary>
        /// <param name="parser">JSON 解析器</param>
        /// <returns>目标索引,null返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int SearchString(Parser parser)
        {
            char value = parser.SearchQuote();
            return parser.ParseState == ParseState.Success && State != null ? searchString(parser, value) : -1;
        }
        /// <summary>
        /// 根据枚举字符串查找目标索引
        /// </summary>
        /// <param name="parser">JSON 解析器</param>
        /// <returns>目标索引,null返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int SearchFlagEnum(Parser parser)
        {
            return flagEnum(parser, parser.SearchEnumQuote());
        }
        /// <summary>
        /// 根据枚举字符串查找目标索引
        /// </summary>
        /// <param name="parser">JSON 解析器</param>
        /// <param name="value">当前字符</param>
        /// <returns>目标索引,null返回-1</returns>
        private int flagEnum(Parser parser, char value)
        {
            byte* currentState = State;
            do
            {
                char* prefix = (char*)(currentState + *(int*)currentState);
                if (*prefix != 0)
                {
                    if (value != *prefix) return -1;
                    while (*++prefix != 0)
                    {
                        if (parser.NextEnumChar() != *prefix) return -1;
                    }
                    value = parser.NextEnumChar();
                }
                if (value == 0 || value == ',') return parser.ParseState == ParseState.Success ? *(int*)(currentState + sizeof(int) * 2) : -1;
                if (*(int*)(currentState + sizeof(int)) == 0) return -1;
                int index = value < 128 ? (int)*(ushort*)(charsAscii + (value << 1)) : getCharIndex(value);
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
                value = parser.NextEnumChar();
            }
            while (true);
        }
        /// <summary>
        /// 根据枚举字符串查找目标索引
        /// </summary>
        /// <param name="parser">JSON 解析器</param>
        /// <returns>目标索引,null返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int NextFlagEnum(Parser parser)
        {
            return flagEnum(parser, parser.SearchNextEnum());
        }

        /// <summary>
        /// 成员名称查找数据
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, Pointer> memberSearchers = new AutoCSer.Threading.LockDictionary<Type, Pointer>();
        /// <summary>
        /// 成员名称查找数据创建锁
        /// </summary>
        private static readonly object memberSearcherLock = new object();
        /// <summary>
        /// 获取成员名称查找数据
        /// </summary>
        /// <param name="type">定义类型</param>
        /// <param name="names">成员名称集合</param>
        /// <returns>成员名称查找数据</returns>
        internal static Pointer GetMemberSearcher(Type type, string[] names)
        {
            if (type.IsGenericType) type = type.GetGenericTypeDefinition();
            Pointer data;
            if (memberSearchers.TryGetValue(type, out data)) return data;
            Monitor.Enter(memberSearcherLock);
            if (memberSearchers.TryGetValue(type, out data))
            {
                Monitor.Exit(memberSearcherLock);
                return data;
            }
            try
            {
                memberSearchers.Set(type, data = AutoCSer.StateSearcher.CharBuilder.Create(names, true).Pointer);
            }
            finally { Monitor.Exit(memberSearcherLock); }
            return data;
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            memberSearchers.Clear();
        }

        static StateSearcher()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
