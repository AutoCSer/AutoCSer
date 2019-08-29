using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.StateSearcher
{
    /// <summary>
    /// ASCII字节搜索器
    /// </summary>
    internal unsafe partial struct AsciiSearcher
    {
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="value">匹配状态</param>
        /// <returns>状态索引,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int Search(string value)
        {
            if (value != null && value.Length != 0)
            {
                fixed (char* valueFixed = value) return Search(valueFixed, valueFixed + value.Length);
            }
            return -1;
        }
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="start">匹配起始位置</param>
        /// <param name="end">匹配结束位置</param>
        /// <returns>状态索引,失败返回-1</returns>
        internal int Search(char* start, char* end)
        {
            if (State == null || start >= end) return -1;
            byte* currentState = State;
            do
            {
                for (byte* prefix = currentState + *(int*)currentState; *prefix != 0; ++prefix, ++start)
                {
                    if (start == end || *start != *prefix) return -1;
                }
                if (start == end) return *(int*)(currentState + sizeof(int) * 2);
                if (*(int*)(currentState + sizeof(int)) == 0 || *start >= 128) return -1;
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
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="value">匹配状态</param>
        /// <returns>状态索引,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int SearchLower(string value)
        {
            if (value != null && value.Length != 0)
            {
                fixed (char* valueFixed = value) return SearchLower(valueFixed, valueFixed + value.Length);
            }
            return -1;
        }
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="value">匹配状态</param>
        /// <returns>状态索引,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int SearchLower(ref SubString value)
        {
            if (value.Length != 0)
            {
                fixed (char* valueFixed = value.String)
                {
                    char* start = valueFixed + value.Start;
                    return SearchLower(start, start + value.Length);
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="start">匹配起始位置</param>
        /// <param name="end">匹配结束位置</param>
        /// <returns>状态索引,失败返回-1</returns>
        internal int SearchLower(char* start, char* end)
        {
            if (State == null || start >= end) return -1;
            byte* currentState = State;
            do
            {
                for (byte* prefix = currentState + *(int*)currentState; *prefix != 0; ++prefix, ++start)
                {
                    if (start == end) return -1;
                    if (*start != *prefix)
                    {
                        if ((uint)(*start - 'A') >= 26 || (*start | 0x20) != *prefix) return -1;
                    }
                }
                if (start == end) return *(int*)(currentState + sizeof(int) * 2);
                if (*(int*)(currentState + sizeof(int)) == 0 || *start >= 128) return -1;
                int index = (int)*(charsAscii + ((uint)(*start - 'A') < 26 ? (*start | 0x20) : *start));
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
        internal int Search(SubArray<byte> data)
        {
            return Search(ref data);
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
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="data">匹配状态</param>
        /// <returns>状态索引,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int SearchNotEmpty(byte[] data)
        {
            fixed (byte* dataFixed = data) return Search(dataFixed, dataFixed + data.Length);
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
                for (byte* prefix = currentState + *(int*)currentState; *prefix != 0; ++prefix, ++start)
                {
                    if (start == end || *start != *prefix) return -1;
                }
                if (start == end) return *(int*)(currentState + sizeof(int) * 2);
                if (*(int*)(currentState + sizeof(int)) == 0 || *start >= 128) return -1;
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
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="data">匹配状态</param>
        /// <returns>状态索引,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int SearchLower(ref SubArray<byte> data)
        {
            if (data.Length != 0)
            {
                fixed (byte* dataFixed = data.Array)
                {
                    byte* start = dataFixed + data.Start;
                    return SearchLower(start, start + data.Length);
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取状态索引
        /// </summary>
        /// <param name="start">匹配起始位置</param>
        /// <param name="end">匹配结束位置</param>
        /// <returns>状态索引,失败返回-1</returns>
        internal int SearchLower(byte* start, byte* end)
        {
            if (State == null || start >= end) return -1;
            byte* currentState = State;
            do
            {
                for (byte* prefix = currentState + *(int*)currentState; *prefix != 0; ++prefix, ++start)
                {
                    if (start == end) return -1;
                    if (*start != *prefix)
                    {
                        if ((uint)(*start - 'A') >= 26 || (*start | 0x20) != *prefix) return -1;
                    }
                }
                if (start == end) return *(int*)(currentState + sizeof(int) * 2);
                if (*(int*)(currentState + sizeof(int)) == 0 || *start >= 128) return -1;
                int index = (int)*(charsAscii + ((uint)(*start - 'A') < 26 ? (*start | 0x20) : *start));
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
    /// <summary>
    /// ASCII字节状态搜索
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class AsciiSearcher<valueType> : IDisposable
    {
        /// <summary>
        /// ASCII 字节搜索器
        /// </summary>
        internal AsciiSearcher Searcher;
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
        internal AsciiSearcher(string[] states, valueType[] values, bool isStaticUnmanaged = false)
        {
            this.Array = values;
            data = AsciiBuilder.Create(states, this.isStaticUnmanaged = isStaticUnmanaged);
            Searcher = new AsciiSearcher(data.Pointer);
        }
        ///// <summary>
        ///// ASCII字节状态搜索
        ///// </summary>
        ///// <param name="states">状态集合</param>
        ///// <param name="values">状态数据集合</param>
        ///// <param name="isStaticUnmanaged">是否固定内存申请</param>
        //public AsciiSearcher(byte[][] states, valueType[] values, bool isStaticUnmanaged = false)
        //{
        //    this.values = values;
        //    data = AsciiByteBuilder.Create(states, this.isStaticUnmanaged = isStaticUnmanaged);
        //}
        /// <summary>
        /// 获取状态数据
        /// </summary>
        /// <param name="state">查询状态</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>状态数据,失败返回默认空值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType Get(string state, valueType nullValue = default(valueType))
        {
            int index = Searcher.Search(state);
            return index >= 0 ? Array[index] : nullValue;
        }
        ///// <summary>
        ///// 判断是否存在状态数据
        ///// </summary>
        ///// <param name="state">查询状态</param>
        ///// <returns>是否存在状态数据</returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal int IndexOf(string state)
        //{
        //    return Searcher.Search(state);
        //}
        ///// <summary>
        ///// 判断是否存在状态数据
        ///// </summary>
        ///// <param name="state">查询状态</param>
        ///// <returns>是否存在状态数据</returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal int IndexOfLower(string state)
        //{
        //    return Searcher.SearchLower(state);
        //}
        /// <summary>
        /// 获取状态数据
        /// </summary>
        /// <param name="state">查询状态</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>状态数据,失败返回默认空值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType Get(ref SubArray<byte> state, valueType nullValue = default(valueType))
        {
            int index = Searcher.Search(ref state);
            return index >= 0 ? Array[index] : nullValue;
        }
        /// <summary>
        /// 获取状态数据
        /// </summary>
        /// <param name="state">查询状态</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>状态数据,失败返回默认空值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType GetLower(ref SubArray<byte> state, valueType nullValue = default(valueType))
        {
            int index = Searcher.SearchLower(ref state);
            return index >= 0 ? Array[index] : nullValue;
        }
        /// <summary>
        /// 获取状态数据
        /// </summary>
        /// <param name="state">查询状态</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>状态数据,失败返回默认空值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType Get(SubArray<byte> state, valueType nullValue = default(valueType))
        {
            return Get(ref state, nullValue);
        }
        /// <summary>
        /// 获取状态数据
        /// </summary>
        /// <param name="state">查询状态</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>状态数据,失败返回默认空值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType GetLower(SubArray<byte> state, valueType nullValue = default(valueType))
        {
            return GetLower(ref state, nullValue);
        }
        /// <summary>
        /// 获取状态数据
        /// </summary>
        /// <param name="state">查询状态</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>状态数据,失败返回默认空值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType GetNotEmpty(byte[] state, valueType nullValue = default(valueType))
        {
            int index = Searcher.SearchNotEmpty(state);
            return index >= 0 ? Array[index] : nullValue;
        }
        /// <summary>
        ///  析构释放资源
        /// </summary>
        ~AsciiSearcher()
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
