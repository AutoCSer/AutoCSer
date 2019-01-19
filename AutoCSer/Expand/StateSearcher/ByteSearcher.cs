using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace AutoCSer.StateSearcher
{
    /// <summary>
    /// 字节数组搜索器
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct ByteSearcher
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
        internal ByteSearcher(Pointer data)
        {
            if (data.Data == null)
            {
                State = bytes = null;
                tableType = 0;
            }
            else
            {
                int stateCount = *data.Int;
                State = data.Byte + sizeof(int);
                bytes = State + stateCount * 3 * sizeof(int);
                if (stateCount < 256) tableType = 0;
                else if (stateCount < 65536) tableType = 1;
                else tableType = 2;
            }
        }
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="start">匹配起始位置</param>
        /// <param name="end">匹配结束位置</param>
        /// <returns>状态索引,失败返回-1</returns>
        internal int Search(byte* start, byte* end)
        {
            if (State == null || start >= end) return -1;
            byte* currentState = State;
            do
            {
                byte* prefix = currentState + *(int*)currentState;
                int prefixSize = *(ushort*)(prefix - sizeof(ushort));
                if (prefixSize != 0)
                {
                    for (byte* endPrefix = prefix + prefixSize; prefix != endPrefix; ++prefix, ++start)
                    {
                        if (start == end || *start != *prefix) return -1;
                    }
                }
                if (start == end) return *(int*)(currentState + sizeof(int) * 2);
                if (*(int*)(currentState + sizeof(int)) == 0) return -1;
                int index = (int)*(bytes + *start);
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
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="data">匹配状态</param>
        /// <returns>状态索引,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int Search(ref SubArray<byte> data)
        {
            if (data.Length != 0)
            {
                fixed (byte* dataFixed = data.Array)
                {
                    byte* start = dataFixed + data.Start;
                    return Search(start, start + data.Length);
                }
            }
            return -1;
        }
    }
    /// <summary>
    /// 字节数组状态搜索
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class ByteSearcher<valueType> : IDisposable
    {
        /// <summary>
        /// ASCII 字节搜索器
        /// </summary>
        internal ByteSearcher Searcher;
        /// <summary>
        /// 状态数据集合
        /// </summary>
        internal readonly valueType[] Array;
        /// <summary>
        /// 状态搜索数据
        /// </summary>
        private Pointer.Size data;
        /// <summary>
        /// 是否固定内存申请
        /// </summary>
        private bool isStaticUnmanaged;
        /// <summary>
        /// ASCII字节状态搜索
        /// </summary>
        /// <param name="states">状态集合</param>
        /// <param name="values">状态数据集合</param>
        /// <param name="isStaticUnmanaged">是否固定内存申请</param>
        internal ByteSearcher(byte[][] states, valueType[] values, bool isStaticUnmanaged = false)
        {
            this.Array = values;
            data = ByteBuilder.Create(states, this.isStaticUnmanaged = isStaticUnmanaged);
            Searcher = new ByteSearcher(data.Pointer);
        }
        /// <summary>
        /// 获取状态数据
        /// </summary>
        /// <param name="state">查询状态</param>
        /// <param name="length">状态长度</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>状态数据,失败返回默认空值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe valueType Get(void* state, int length, valueType nullValue = default(valueType))
        {
            int index = Searcher.Search((byte*)state, (byte*)state + length);
            return index >= 0 ? Array[index] : nullValue;
        }
        /// <summary>
        ///  析构释放资源
        /// </summary>
        ~ByteSearcher()
        {
            if (isStaticUnmanaged) Unmanaged.Free(ref data, isStaticUnmanaged);
            else Unmanaged.Free(ref data);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public unsafe void Dispose()
        {
            Searcher.State = null;
            if (isStaticUnmanaged) Unmanaged.Free(ref data, isStaticUnmanaged);
            else Unmanaged.Free(ref data);
        }
    }
}
