using System;
using AutoCSer.Extension;

namespace AutoCSer.StateSearcher
{
    /// <summary>
    /// 状态数据创建器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct CharBuilder
    {
        /// <summary>
        /// 名称集合
        /// </summary>
        private KeyValue<string, int>[] names;
        /// <summary>
        /// 状态数据
        /// </summary>
        public Pointer.Size Data;
        /// <summary>
        /// 状态集合
        /// </summary>
        private byte* state;
        /// <summary>
        /// ASCII字符查找表
        /// </summary>
        private byte* charsAscii;
        /// <summary>
        /// 特殊字符串查找表
        /// </summary>
        private byte* charStart;
        /// <summary>
        /// 特殊字符串查找表结束位置
        /// </summary>
        private byte* charEnd;
        /// <summary>
        /// 前缀集合
        /// </summary>
        private byte* prefix;
        /// <summary>
        /// 状态矩阵
        /// </summary>
        private byte* table;
        /// <summary>
        /// 状态数量
        /// </summary>
        private int stateCount;
        /// <summary>
        /// 矩阵状态数量
        /// </summary>
        private int tableCount;
        /// <summary>
        /// 前缀数量
        /// </summary>
        private int prefixSize;
        /// <summary>
        /// 查询矩阵单位尺寸类型
        /// </summary>
        private int tableType;
        /// <summary>
        /// 状态字符集合
        /// </summary>
        private LeftArray<char> chars;
        /// <summary>
        /// 状态数据创建器
        /// </summary>
        /// <param name="names">名称集合</param>
        /// <param name="isStaticUnmanaged">是否固定内存申请</param>
        public CharBuilder(KeyValue<string, int>[] names, bool isStaticUnmanaged)
        {
            this.names = names;
            prefixSize = tableCount = stateCount = tableType = 0;
            state = charsAscii = charStart = charEnd = prefix = table = null;
            chars = default(LeftArray<char>);
            if (names.Length > 1)
            {
                Data = new Pointer.Size();
                count(0, names.Length, 0);
                int charCount, asciiCount;
                System.Array.Sort(chars.Array, 0, chars.Length);
                fixed (char* charFixed = chars.Array)
                {
                    char* start = charFixed + 1, end = charFixed + chars.Length, write = start;
                    char value = *charFixed;
                    if (*(end - 1) < 128)
                    {
                        while (start != end)
                        {
                            if (*start != value) *write++ = value = *start;
                            ++start;
                        }
                        asciiCount = (int)(write - charFixed);
                        charCount = 0;
                    }
                    else
                    {
                        while (value < 128)
                        {
                            while (*start == value) ++start;
                            *write++ = value = *start++;
                        }
                        asciiCount = (int)(write - charFixed) - 1;
                        while (start != end)
                        {
                            if (*start != value) *write++ = value = *start;
                            ++start;
                        }
                        charCount = (int)(write - charFixed) - asciiCount;
                    }
                    chars.Length = asciiCount + charCount;
                    int size = (1 + (stateCount += tableCount) * 3) * sizeof(int) + (128 + 2 + charCount + prefixSize) * sizeof(ushort);
                    if (stateCount < 256) size += tableCount * (chars.Length + 1);
                    else if (stateCount < 65536)
                    {
                        size += tableCount * (chars.Length + 1) * sizeof(ushort);
                        tableType = 1;
                    }
                    else
                    {
                        size += tableCount * (chars.Length + 1) * sizeof(int);
                        tableType = 2;
                    }
                    Data = Unmanaged.Get(size, true, isStaticUnmanaged);
                    *Data.Int = stateCount;//状态数量[int]
                    state = Data.Byte + sizeof(int);//状态集合[stateCount*(前缀位置[int]+状态位置[int]+名称索引[int])]
                    charsAscii = state + (stateCount * 3) * sizeof(int);//ascii字符查找表[128*ushort]
                    charStart = charsAscii + 128 * sizeof(ushort);
                    *(ushort*)charStart = (ushort)(asciiCount + 1);//特殊字符起始值[ushort]
                    *(ushort*)(charStart + sizeof(ushort)) = (ushort)charCount;//特殊字符数量[ushort]
                    charStart += sizeof(ushort) * 2;
                    ushort charIndex = 0;
                    for (start = charFixed, end = charFixed + asciiCount; start != end; ++start)
                    {
                        *(ushort*)(charsAscii + (*start << 1)) = ++charIndex;
                    }
                    charEnd = charStart;
                    if (charCount != 0)
                    {//特殊字符二分查找表[charCount*char]
                        AutoCSer.Memory.CopyNotNull((byte*)start, charStart, charCount << 1);
                        charEnd += charCount << 1;
                    }
                    prefix = charStart + charCount * sizeof(ushort);//前缀集合
                    table = prefix + prefixSize * sizeof(ushort);//状态矩阵[tableCount*(chars.Count+1)*[byte/ushort/int]]
                }
                stateCount = 0;
                create(0, names.Length, 0);
            }
            else
            {
                if (names.Length == 0) Data = new Pointer.Size();
                else if (names[0].Key.Length <= 128)
                {
                    Data = Unmanaged.Get(sizeof(int) + sizeof(int) * 3 + 128 * sizeof(ushort) + 2 * sizeof(ushort), false, isStaticUnmanaged);
                    *Data.Int = 1;//状态数量
                    state = Data.Byte + sizeof(int);
                    *(int*)state = sizeof(int) * 3;//前缀位置
                    *(int*)(state + sizeof(int)) = 0;//状态位置
                    *(int*)(state + sizeof(int) * 2) = names[0].Value;//名称索引
                    prefix = Data.Byte + sizeof(int) * 4;
                    AutoCSer.Extension.StringExtension.SimpleCopyNotNull(names[0].Key, prefix);
                    *(char*)(prefix + (names[0].Key.Length << 1)) = (char)0;
                    *(int*)(Data.Byte + sizeof(int) * 4 + 128 * sizeof(ushort)) = 0;
                }
                else
                {
                    Data = Unmanaged.Get(sizeof(int) + sizeof(int) * 3 + 128 * sizeof(ushort) + 2 * sizeof(ushort) + names[0].Key.Length * sizeof(char) + sizeof(char), true, isStaticUnmanaged);
                    *Data.Int = 1;//状态数量
                    state = Data.Byte + sizeof(int);
                    *(int*)state = sizeof(int) * 3 + 128 * sizeof(ushort) + 2 * sizeof(ushort);//前缀位置
                    *(int*)(state + sizeof(int)) = 0;//状态位置
                    *(int*)(state + sizeof(int) * 2) = names[0].Value;//名称索引
                    AutoCSer.Extension.StringExtension.SimpleCopyNotNull(names[0].Key, state + *(int*)state);
                }
            }
        }
        /// <summary>
        /// 计算状态数量
        /// </summary>
        /// <param name="start">起始名称位置</param>
        /// <param name="end">结束名称位置</param>
        /// <param name="current"></param>
        private void count(int start, int end, int current)
        {
            ++tableCount;
            int index = start, prefixSize = 0;
            char value = (char)0;
            while (names[start].Key.Length != current)
            {
                value = names[start].Key[current];
                while (++index != end && names[index].Key[current] == value) ;
                if (index != end) break;
                ++prefixSize;
                index = start;
                ++current;
            }
            if (prefixSize != 0) this.prefixSize += prefixSize + 1;
            do
            {
                int count = index - start;
                if (count == 0) index = ++start;
                else
                {
                    if (value == 0) throw new IndexOutOfRangeException();
                    chars.Add(value);
                    if (count == 1)
                    {
                        ++stateCount;
                        prefixSize = names[start].Key.Length - current - 1;
                        if (prefixSize != 0) this.prefixSize += prefixSize + 1;
                    }
                    else this.count(start, index, current + 1);
                }
                if (index == end) break;
                value = names[start = index].Key[current];
                while (++index != end && names[index].Key[current] == value) ;
            }
            while (true);
        }
        /// <summary>
        /// 创建状态数据
        /// </summary>
        /// <param name="start">起始名称位置</param>
        /// <param name="end">结束名称位置</param>
        /// <param name="current"></param>
        private void create(int start, int end, int current)
        {
            byte* prefix = this.prefix, table = this.table;
            *(int*)(state + sizeof(int)) = (int)(table - state);
            int index = start;
            char value = (char)0;
            if (names[start].Key.Length == current) *(int*)(state + sizeof(int) * 2) = names[start].Value;
            else
            {
                do
                {
                    value = names[index].Key[current];
                    while (++index != end && names[index].Key[current] == value) ;
                    if (index != end)
                    {
                        *(int*)(state + sizeof(int) * 2) = -1;
                        break;
                    }
                    *(char*)this.prefix = value;
                    this.prefix += 2;
                    if (names[index = start].Key.Length == ++current)
                    {
                        *(int*)(state + sizeof(int) * 2) = names[index].Value;
                        break;
                    }
                }
                while (true);
            }
            if (prefix == this.prefix) *(int*)state = (int)(charsAscii - state);
            else
            {
                *(char*)this.prefix = (char)0;
                *(int*)state = (int)(prefix - state);
                this.prefix += 2;
            }
            state += sizeof(int) * 3;
            ++stateCount;
            if (tableType == 0) this.table += chars.Length + 1;
            else if (tableType == 1) this.table += (chars.Length + 1) * sizeof(ushort);
            else this.table += (chars.Length + 1) * sizeof(int);
            do
            {
                int count = index - start;
                if (count == 0) index = ++start;
                else
                {
                    int charIndex;
                    if (value < 128) charIndex = (int)*(ushort*)(charsAscii + (value << 1));
                    else
                    {
                        char* charStart = (char*)this.charStart, charEnd = (char*)this.charEnd, charCurrent = charStart + ((int)(charEnd - charStart) >> 1);
                        while (*charCurrent != value)
                        {
                            if (value < *charCurrent) charEnd = charCurrent;
                            else charStart = charCurrent + 1;
                            charCurrent = charStart + ((int)(charEnd - charStart) >> 1);
                        }
                        charIndex = (int)*(ushort*)(this.charStart - sizeof(int)) + (int)(charCurrent - (char*)this.charStart);
                    }
                    if (tableType == 0) *(table + charIndex) = (byte)stateCount;
                    else if (tableType == 1) *(ushort*)(table + charIndex * sizeof(ushort)) = (ushort)stateCount;
                    else *(int*)(table + charIndex * sizeof(int)) = stateCount * 3 * sizeof(int);
                    if (count == 1)
                    {
                        int prefixSize = names[start].Key.Length - current - 1;
                        if (prefixSize == 0) *(int*)state = (int)(charsAscii - state);
                        else
                        {
                            *(int*)state = (int)(this.prefix - state);
                            fixed (char* charFixed = names[start].Key)
                            {
                                AutoCSer.Memory.SimpleCopyNotNull((byte*)(charFixed + current + 1), this.prefix, prefixSize <<= 1);
                                *(char*)(this.prefix += prefixSize) = (char)0;
                                this.prefix += sizeof(char);
                            }
                        }
                        *(int*)(state + sizeof(int) * 2) = names[start].Value;
                        ++stateCount;
                        state += sizeof(int) * 3;
                    }
                    else create(start, index, current + 1);
                }
                if (index == end) break;
                value = names[start = index].Key[current];
                while (++index != end && names[index].Key[current] == value) ;
            }
            while (true);
        }

        /// <summary>
        /// 字符串比较大小
        /// </summary>
        internal static readonly Func<KeyValue<string, int>, KeyValue<string, int>, int> StringCompare = compare;
        /// <summary>
        /// 字符串比较大小
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int compare(KeyValue<string, int> left, KeyValue<string, int> right)
        {
            return string.CompareOrdinal(left.Key, right.Key);
        }
        /// <summary>
        /// 状态检测
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        internal static bool Check(KeyValue<string, int>[] values)
        {
            if (values.Length > 1)
            {
                string value = null;
                for (int index = values.Length; index != 0; )
                {
                    string newValue = values[--index].Key;
                    if (newValue == value) return false;
                    value = newValue;
                }
            }
            return true;
        }
        /// <summary>
        /// 创建名称查找数据
        /// </summary>
        /// <param name="names">名称集合</param>
        /// <param name="isStaticUnmanaged">是否固定内存申请</param>
        /// <returns>名称查找数据</returns>
        public static Pointer.Size Create(string[] names, bool isStaticUnmanaged)
        {
            int index = 0;
            KeyValue<string, int>[] strings = new KeyValue<string, int>[names.Length];
            foreach (string name in names)
            {
                strings[index].Set(name, index);
                ++index;
            }
            strings = strings.sort(StringCompare);
            return new CharBuilder(Check(strings) ? strings : NullValue<KeyValue<string, int>>.Array, isStaticUnmanaged).Data;
        }
    }
}
