using System;
using AutoCSer.Extension;

namespace AutoCSer.StateSearcher
{
    /// <summary>
    /// 状态数据创建器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct ByteBuilder
    {
        /// <summary>
        /// 状态集合
        /// </summary>
        private KeyValue<byte[], int>[] values;
        /// <summary>
        /// 状态集合
        /// </summary>
        private byte* state;
        /// <summary>
        /// ASCII字符查找表
        /// </summary>
        private byte* bytes;
        /// <summary>
        /// 前缀集合
        /// </summary>
        private byte* prefix;
        /// <summary>
        /// 空前缀
        /// </summary>
        private byte* nullPrefix;
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
        /// 状态数据
        /// </summary>
        public Pointer.Size Data;
        /// <summary>
        /// 状态字符集合
        /// </summary>
        private MemoryMap chars;
        /// <summary>
        /// 状态数据创建器
        /// </summary>
        /// <param name="values">状态集合</param>
        /// <param name="isStaticUnmanaged">是否固定内存申请</param>
        private ByteBuilder(KeyValue<byte[], int>[] values, bool isStaticUnmanaged)
        {
            this.values = values;
            prefixSize = tableCount = stateCount = tableType = charCount = 0;
            state = bytes = nullPrefix = prefix = table = null;
            if (values.Length > 1)
            {
                Data = new Pointer.Size();
                byte* chars = stackalloc byte[256 >> 3];
                this.chars = new MemoryMap((ulong*)chars, (256 >> 3) >> 3);
                count(0, values.Length, 0);
                charCount = (*(ulong*)chars).bitCount() + (*(ulong*)(chars + sizeof(ulong))).bitCount() + (*(ulong*)(chars + sizeof(ulong) * 2)).bitCount() + (*(ulong*)(chars + sizeof(ulong) * 3)).bitCount();
                int size = (1 + (stateCount += tableCount) * 3) * sizeof(int) + ((prefixSize + (256 + 4 + 3)) & (int.MaxValue - 3));
                if (stateCount < 256) size += tableCount * charCount;
                else if (stateCount < 65536)
                {
                    size += tableCount * charCount * sizeof(ushort);
                    tableType = 1;
                }
                else
                {
                    size += tableCount * charCount * sizeof(int);
                    tableType = 2;
                }
                Data = Unmanaged.Get(size, true, isStaticUnmanaged);
                *Data.Int = stateCount;//状态数量[int]
                state = Data.Byte + sizeof(int);//状态集合[stateCount*(前缀位置[int]+状态位置[int]+名称索引[int])]
                bytes = state + (stateCount * 3) * sizeof(int);//字节查找表[256*byte]
                byte charIndex = 0;
                for (int index = 0; index != 256; ++index)
                {
                    if (this.chars.Get(index) != 0) *(bytes + index) = charIndex++;
                }
                nullPrefix = bytes + 256;//空前缀
                table = nullPrefix + ((prefixSize + (4 + 3)) & (int.MaxValue - 3));//状态矩阵[tableCount*charCount*[byte/ushort/int]]
                *(ushort*)nullPrefix = (ushort)charCount;//字符数量
                prefix = nullPrefix + sizeof(int) + sizeof(ushort);//前缀集合
                nullPrefix += sizeof(int);
                stateCount = 0;
                create(0, values.Length, 0);
            }
            else
            {
                chars = new MemoryMap();
                byte[] value = values[0].Key;
                fixed (byte* valueFixed = value)
                {
                    if (values[0].Key.Length <= 254)
                    {
                        Data = Unmanaged.Get(sizeof(int) + sizeof(int) * 3 + 256 + 2, false, isStaticUnmanaged);
                        *Data.Int = 1;//状态数量
                        state = Data.Byte + sizeof(int);
                        *(int*)state = sizeof(int) * 3 + sizeof(ushort);//前缀位置
                        *(int*)(state + sizeof(int)) = 0;//状态位置
                        *(int*)(state + sizeof(int) * 2) = values[0].Value;//名称索引
                        prefix = Data.Byte + sizeof(int) * 4;
                        *(ushort*)prefix = (ushort)value.Length;
                        AutoCSer.Memory.SimpleCopyNotNull(valueFixed, prefix + sizeof(ushort), value.Length);
                        *(ushort*)(prefix + 256) = 0;
                    }
                    else
                    {
                        Data = Unmanaged.Get(sizeof(int) + sizeof(int) * 3 + 256 + 4 + 2 + value.Length, true, isStaticUnmanaged);
                        *Data.Int = 1;//状态数量
                        state = Data.Byte + sizeof(int);
                        *(int*)state = sizeof(int) * 3 + 256 + 4 + sizeof(ushort);//前缀位置
                        *(int*)(state + sizeof(int)) = 0;//状态位置
                        *(int*)(state + sizeof(int) * 2) = values[0].Value;//名称索引
                        prefix = Data.Byte + sizeof(int) * 4 + 256 + 4;
                        *(ushort*)prefix = (ushort)value.Length;
                        AutoCSer.Memory.SimpleCopyNotNull(valueFixed, prefix + sizeof(ushort), value.Length);
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
            byte value = 0;
            while (values[start].Key.Length != current)
            {
                value = values[start].Key[current];
                while (++index != end && values[index].Key[current] == value) ;
                if (index != end) break;
                ++prefixSize;
                index = start;
                ++current;
            }
            if (prefixSize != 0) this.prefixSize += (prefixSize + 3) & (int.MaxValue - 1);
            do
            {
                int count = index - start;
                if (count == 0) index = ++start;
                else
                {
                    chars.Set(value);
                    if (count == 1)
                    {
                        ++stateCount;
                        prefixSize = values[start].Key.Length - current - 1;
                        if (prefixSize != 0) this.prefixSize += (prefixSize + 3) & (int.MaxValue - 1);
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
            byte value = 0;
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
            int prefixSize = (int)(this.prefix - prefix);
            if (prefixSize == 0) *(int*)state = (int)(nullPrefix - state);
            else
            {
                *(ushort*)(prefix - sizeof(ushort)) = (ushort)prefixSize;
                *(int*)state = (int)(prefix - state);
                this.prefix += sizeof(ushort) + (prefixSize & 1);
            }
            state += sizeof(int) * 3;
            ++stateCount;
            if (tableType == 0) this.table += charCount;
            else if (tableType == 1) this.table += charCount * sizeof(ushort);
            else this.table += charCount * sizeof(int);
            do
            {
                int count = index - start;
                if (count == 0) index = ++start;
                else
                {
                    int charIndex = (int)*(bytes + value);
                    if (tableType == 0) *(table + charIndex) = (byte)stateCount;
                    else if (tableType == 1) *(ushort*)(table + charIndex * sizeof(ushort)) = (ushort)stateCount;
                    else *(int*)(table + charIndex * sizeof(int)) = stateCount * 3 * sizeof(int);
                    if (count == 1)
                    {
                        prefixSize = values[start].Key.Length - current - 1;
                        if (prefixSize == 0) *(int*)state = (int)(nullPrefix - state);
                        else
                        {
                            *(int*)state = (int)(this.prefix - state);
                            *(ushort*)(this.prefix - sizeof(ushort)) = (ushort)prefixSize;
                            fixed (byte* charFixed = values[start].Key)
                            {
                                AutoCSer.Memory.SimpleCopyNotNull(charFixed + current + 1, this.prefix, prefixSize);
                                this.prefix += (prefixSize + 3) & (int.MaxValue - 1);
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
        internal static Pointer.Size Create(byte[][] states, bool isStaticUnmanaged)
        {
            if (states.Length == 0) return default(Pointer.Size);
            int index = 0;
            KeyValue<byte[], int>[] strings = new KeyValue<byte[], int>[states.Length];
            foreach (byte[] name in states)
            {
                if (name.Length >= 65536) throw new IndexOutOfRangeException("name.Length[" + name.Length.toString() + "] >= 65536");
                strings[index].Set(name, index);
                ++index;
            }
            strings = strings.sort(compareHanlde);
            return new ByteBuilder(check(strings) ? strings : NullValue<KeyValue<byte[], int>>.Array, isStaticUnmanaged).Data;
        }
        /// <summary>
        /// 状态检测
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private static bool check(KeyValue<byte[], int>[] values)
        {
            if (values.Length > 1)
            {
                byte[] value = null;
                for (int index = values.Length; index != 0; )
                {
                    byte[] newValue = values[--index].Key;
                    if (newValue.equal(value)) return false;
                    value = newValue;
                }
            }
            return true;
        }
        /// <summary>
        /// 字节数组比较大小
        /// </summary>
        private static Func<KeyValue<byte[], int>, KeyValue<byte[], int>, int> compareHanlde = compare;
        /// <summary>
        /// 字节数组比较大小
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int compare(KeyValue<byte[], int> left, KeyValue<byte[], int> right)
        {
            int length = Math.Min(left.Key.Length, right.Key.Length);
            fixed (byte* leftFixed = left.Key, rightFixed = right.Key)
            {
                for (byte* start = leftFixed, end = leftFixed + length, read = rightFixed; start != end; ++start, ++read)
                {
                    if (*start != *read) return *start - *read;
                }
            }
            return left.Key.Length - right.Key.Length;
        }
    }
}
