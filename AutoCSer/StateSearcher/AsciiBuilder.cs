using System;
using AutoCSer.Extension;

namespace AutoCSer.StateSearcher
{
    /// <summary>
    /// 状态数据创建器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct AsciiBuilder
    {
        /// <summary>
        /// 状态集合
        /// </summary>
        private KeyValue<string, int>[] values;
        /// <summary>
        /// 状态数据
        /// </summary>
        public Pointer.Size Data;
        /// <summary>
        /// 状态字符集合
        /// </summary>
        private MemoryMap chars;
        /// <summary>
        /// 状态集合
        /// </summary>
        private byte* state;
        /// <summary>
        /// ASCII字符查找表
        /// </summary>
        private byte* charsAscii;
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
        /// 状态字符数量
        /// </summary>
        private int charCount;
        /// <summary>
        /// 状态数据创建器
        /// </summary>
        /// <param name="values">状态集合</param>
        /// <param name="isStaticUnmanaged">是否固定内存申请</param>
        public AsciiBuilder(KeyValue<string, int>[] values, bool isStaticUnmanaged)
        {
            this.values = values;
            prefixSize = tableCount = stateCount = tableType = charCount = 0;
            state = charsAscii = prefix = table = null;
            if (values.Length > 1)
            {
                byte* chars = stackalloc byte[128 >> 3];
                //Memory.ClearUnsafe((ulong*)chars, 128 >> (3 + 3));
                this.chars = new MemoryMap((ulong*)chars, (128 >> 3) >> 3);
                Data = new Pointer.Size();
                count(0, values.Length, 0);
                charCount = (*(ulong*)chars).bitCount() + (*(ulong*)(chars + sizeof(ulong))).bitCount();
                int size = (1 + (stateCount += tableCount) * 3) * sizeof(int) + 128 + 4 + (prefixSize & (int.MaxValue - 3));
                if (stateCount < 256) size += tableCount * (charCount + 1);
                else if (stateCount < 65536)
                {
                    size += tableCount * (charCount + 1) * sizeof(ushort);
                    tableType = 1;
                }
                else
                {
                    size += tableCount * (charCount + 1) * sizeof(int);
                    tableType = 2;
                }
                Data = Unmanaged.Get(size, true, isStaticUnmanaged);
                *Data.Int = stateCount;//状态数量[int]
                state = Data.Byte + sizeof(int);//状态集合[stateCount*(前缀位置[int]+状态位置[int]+名称索引[int])]
                charsAscii = state + (stateCount * 3) * sizeof(int);//ascii字符查找表[128*byte]
                byte charIndex = 0;
                for (byte index = 1; index != 128; ++index)
                {
                    if (this.chars.Get(index) != 0) *(charsAscii + index) = ++charIndex;
                }
                prefix = charsAscii + 128;//前缀集合
                table = prefix + ((prefixSize & (int.MaxValue - 3)) + 4);//状态矩阵[tableCount*(charCount+1)*[byte/ushort/int]]
                *prefix++ = (byte)charCount;//字符数量
                stateCount = 0;
                create(0, values.Length, 0);
            }
            else
            {
                chars = new MemoryMap();
                string value = values[0].Key;
                fixed (char* valueFixed = value)
                {
                    if (values[0].Key.Length <= 128)
                    {
                        Data = Unmanaged.Get(sizeof(int) + sizeof(int) * 3 + 128 + 1, false, isStaticUnmanaged);
                        *Data.Int = 1;//状态数量
                        state = Data.Byte + sizeof(int);
                        *(int*)state = sizeof(int) * 3;//前缀位置
                        *(int*)(state + sizeof(int)) = 0;//状态位置
                        *(int*)(state + sizeof(int) * 2) = values[0].Value;//名称索引
                        prefix = Data.Byte + sizeof(int) * 4;
                        AutoCSer.Extension.StringExtension.WriteBytesNotNull(valueFixed, value.Length, prefix);
                        *(prefix + value.Length) = *(prefix + 128) = 0;
                    }
                    else
                    {
                        Data = Unmanaged.Get(sizeof(int) + sizeof(int) * 3 + 128 + 1 + value.Length + 1, true, isStaticUnmanaged);
                        *Data.Int = 1;//状态数量
                        state = Data.Byte + sizeof(int);
                        *(int*)state = sizeof(int) * 3 + 128 + 1;//前缀位置
                        *(int*)(state + sizeof(int)) = 0;//状态位置
                        *(int*)(state + sizeof(int) * 2) = values[0].Value;//名称索引
                        AutoCSer.Extension.StringExtension.WriteBytesNotNull(valueFixed, value.Length, Data.Byte + sizeof(int) * 3 + 128 + 1);
                    }
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
            while (values[start].Key.Length != current)
            {
                value = values[start].Key[current];
                while (++index != end && values[index].Key[current] == value) ;
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
                    if (value >= 128) throw new IndexOutOfRangeException("value[" + ((ushort)value).toString() + "] >= 128");
                    if (value == 0) throw new IndexOutOfRangeException();
                    chars.Set(value);
                    if (count == 1)
                    {
                        ++stateCount;
                        prefixSize = values[start].Key.Length - current - 1;
                        if (prefixSize != 0) this.prefixSize += prefixSize + 1;
                    }
                    else this.count(start, index, current + 1);
                }
                if (index == end) break;
                value = values[start = index].Key[current];
                while (++index != end && values[index].Key[current] == value) ;
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
            if (values[start].Key.Length == current) *(int*)(state + sizeof(int) * 2) = values[start].Value;
            else
            {
                do
                {
                    value = values[index].Key[current];
                    while (++index != end && values[index].Key[current] == value) ;
                    if (index != end)
                    {
                        *(int*)(state + sizeof(int) * 2) = -1;
                        break;
                    }
                    *this.prefix++ = (byte)value;
                    if (values[index = start].Key.Length == ++current)
                    {
                        *(int*)(state + sizeof(int) * 2) = values[index].Value;
                        break;
                    }
                }
                while (true);
            }
            if (prefix == this.prefix) *(int*)state = (int)(charsAscii - state);
            else
            {
                *this.prefix++ = 0;
                *(int*)state = (int)(prefix - state);
            }
            state += sizeof(int) * 3;
            ++stateCount;
            if (tableType == 0) this.table += charCount + 1;
            else if (tableType == 1) this.table += (charCount + 1) * sizeof(ushort);
            else this.table += (charCount + 1) * sizeof(int);
            do
            {
                int count = index - start;
                if (count == 0) index = ++start;
                else
                {
                    int charIndex = (int)*(charsAscii + value);
                    if (tableType == 0) *(table + charIndex) = (byte)stateCount;
                    else if (tableType == 1) *(ushort*)(table + charIndex * sizeof(ushort)) = (ushort)stateCount;
                    else *(int*)(table + charIndex * sizeof(int)) = stateCount * 3 * sizeof(int);
                    if (count == 1)
                    {
                        int prefixSize = values[start].Key.Length - current - 1;
                        if (prefixSize == 0) *(int*)state = (int)(charsAscii - state);
                        else
                        {
                            *(int*)state = (int)(this.prefix - state);
                            fixed (char* charFixed = values[start].Key)
                            {
                                AutoCSer.Extension.StringExtension.WriteBytesNotNull(charFixed + current + 1, prefixSize, this.prefix);
                                *(this.prefix += prefixSize) = 0;
                                ++this.prefix;
                            }
                        }
                        *(int*)(state + sizeof(int) * 2) = values[start].Value;
                        ++stateCount;
                        state += sizeof(int) * 3;
                    }
                    else create(start, index, current + 1);
                }
                if (index == end) break;
                value = values[start = index].Key[current];
                while (++index != end && values[index].Key[current] == value) ;
            }
            while (true);
        }

        /// <summary>
        /// 创建状态查找数据
        /// </summary>
        /// <param name="states">状态集合</param>
        /// <param name="isStaticUnmanaged">是否固定内存申请</param>
        /// <returns>状态查找数据</returns>
        internal static Pointer.Size Create(string[] states, bool isStaticUnmanaged)
        {
            if (states.Length == 0) return default(Pointer.Size);
            int index = 0;
            KeyValue<string, int>[] strings = new KeyValue<string, int>[states.Length];
            foreach (string name in states)
            {
                strings[index].Set(name, index);
                ++index;
            }
            strings = strings.sort(CharBuilder.StringCompare);
            return new AsciiBuilder(CharBuilder.Check(strings) ? strings : NullValue<KeyValue<string, int>>.Array, isStaticUnmanaged).Data;
        }
    }
}
