using System;
using System.Collections.Generic;
using AutoCSer.Extensions;
using System.Collections;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;

namespace AutoCSer
{
    /// <summary>
    /// 数组子串
    /// </summary>
    public partial struct LeftArray<T>
    {
        /// <summary>
        /// 原数组是否为 null
        /// </summary>
        public bool IsNull
        {
            get { return Array == null; }
        }
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="values">数据集合</param>
        public LeftArray(ICollection<T> values)
        {
            Length = Reserve = 0;
            if (values == null) Array = EmptyArray<T>.Array;
            else
            {
                int count = values.Count;
                if (count == 0) Array = EmptyArray<T>.Array;
                else
                {
                    Array = new T[count];
                    foreach (T value in values)
                    {
                        if (--count >= 0) Array[Length++] = value;
                        else Add(value);
                    }
                }
            }
        }
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="values">数据集合</param>
        public LeftArray(IEnumerable<T> values)
        {
            Array = EmptyArray<T>.Array;
            Length = Reserve = 0;
            if (values != null)
            {
                foreach (T value in values) Add(value);
            }
        }
        /// <summary>
        /// 长度设为0（注意：对于引用类型没有置 0 可能导致内存泄露）
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void ResetCount()
        {
            Length = 0;
        }
        ///// <summary>
        ///// 返回非 null 数组  
        ///// </summary>
        ///// <returns></returns>
        //internal LeftArray<T> NotNull()
        //{
        //    if (Array == null) return new LeftArray<T>(EmptyArray<T>.Array);
        //    return this;
        //}
        /// <summary>
        /// 获取最后一个值
        /// </summary>
        internal T UnsafeLast
        {
            get { return Array[Length - 1]; }
        }
        /// <summary>
        /// 获取最后一个值
        /// </summary>
        /// <returns>最后一个值,失败为default(valueType)</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public T LastOrDefault()
        {
            return Length != 0 ? Array[Length - 1] : default(T);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(ListArray<T> values)
        {
            if (values != null) this.Add(ref values.Array);
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="collectionValueType">集合数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="getValue">获取数据委托</param>
        public void Add<collectionValueType>(ICollection<collectionValueType> values, Func<collectionValueType, T> getValue)
        {
            int count = values.count();
            if (count != 0)
            {
                addToLength(Length + count);
                foreach (collectionValueType value in values)
                {
                    Array[Length] = getValue(value);
                    ++Length;
                }
            }
        }
        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal T UnsafePopOnly()
        {
            return Array[--Length];
        }
        /// <summary>
        /// 逆转列表
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Reverse()
        {
            if (Length > 1) System.Array.Reverse(Array, 0, Length);
        }
        /// <summary>
        /// 获取第一个匹配值
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值,失败为 default(valueType)</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public T FirstOrDefault(Func<T, bool> isValue)
        {
            if (Length != 0)
            {
                int index = indexOf(isValue);
                if (index != -1) return Array[index];
                return index != -1 ? Array[index] : default(T);
            }
            return default(T);
        }
        /// <summary>
        /// 获取匹配值集合
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值集合</returns>
        public unsafe T[] GetFindArray(Func<T, bool> isValue)
        {
            if (Length == 0) return EmptyArray<T>.Array;
            int length = ((Length + 63) >> 6) << 3;
            UnmanagedPool pool = UnmanagedPool.GetPool(length);
            AutoCSer.Memory.Pointer buffer = pool.GetMinSize(length);
            try
            {
                AutoCSer.Memory.Common.Clear(buffer.ULong, length >> 3);
                return getFindArray(isValue, new MemoryMap(buffer.Data));
            }
            finally { pool.Push(ref buffer); }
        }
        /// <summary>
        /// 获取匹配值集合
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="map">匹配结果位图</param>
        /// <returns>匹配值集合</returns>
        private T[] getFindArray(Func<T, bool> isValue, MemoryMap map)
        {
            int count = 0, index = 0;
            foreach (T value in Array)
            {
                if (isValue(value))
                {
                    ++count;
                    map.Set(index);
                }
                if (++index == Length) break;
            }
            if (count == 0) return EmptyArray<T>.Array;
            T[] values = new T[count];
            for (index = Length; count != 0; values[--count] = Array[index])
            {
                while (map.Get(--index) == 0) ;
            }
            return values;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public string JoinString(string join, Func<T, string> toString)
        {
            return string.Join(join, GetArray(toString));
        }
        /// <summary>
        /// 设置数据长度并清除其它数据
        /// </summary>
        /// <param name="length"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void setLengthClear(int length)
        {
            if (DynamicArray<T>.IsClearArray) System.Array.Clear(Array, length, Length - length);
            Length = length;
        }
        /// <summary>
        /// 最后一个数据移动到被删除数据位置
        /// </summary>
        /// <param name="index"></param>
        internal void RemoveAtToEnd(int index)
        {
            if ((uint)index < (uint)Length) TryRemoveAtToEnd(index);
            else throw new IndexOutOfRangeException("index[" + index.toString() + "] >= Length[" + Length.toString() + "]");
        }
        /// <summary>
        /// 移除所有后端匹配值
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        private void removeEnd(Func<T, bool> isValue)
        {
#if DEBUG
            if (Length == 0) throw new Exception("Length == 0");
#endif
            int index = Length;
            do
            {
                if (isValue(Array[index - 1])) --index;
                else break;
            }
            while (index != 0);
            setLengthClear(index);
        }
        /// <summary>
        /// 移除匹配值
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        internal void Remove(Func<T, bool> isValue)
        {
            if (Length != 0)
            {
                removeEnd(isValue);
                if (Length != 0)
                {
                    int index = indexOf(isValue);
                    if (index != -1)
                    {
                        for (int read = index; ++read != Length; )
                        {
                            if (!isValue(Array[read])) Array[index++] = Array[read];
                        }
                        setLengthClear(index);
                    }
                }
            }
        }
        /// <summary>
        /// 移除所有后端匹配值
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        private void removeEndNot(Func<T, bool> isValue)
        {
#if DEBUG
            if (Length == 0) throw new Exception("Length == 0");
#endif
            int index = Length;
            do
            {
                if (!isValue(Array[index - 1])) --index;
                else break;
            }
            while (index != 0);
            setLengthClear(index);
        }
        /// <summary>
        /// 获取获取数组中的匹配位置
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>数组中的匹配位置,失败为-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int indexOfNot(Func<T, bool> isValue)
        {
#if DEBUG
            if (Length == 0) throw new Exception("Length == 0");
#endif
            int index = 0;
            foreach (T value in Array)
            {
                if (!isValue(value)) return index;
                if (++index == Length) return -1;
            }
            return -1;
        }
        /// <summary>
        /// 移除匹配值
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        internal void RemoveNot(Func<T, bool> isValue)
        {
            if (Length != 0)
            {
                removeEndNot(isValue);
                if (Length != 0)
                {
                    int index = indexOfNot(isValue);
                    if (index != -1)
                    {
                        for (int read = index; ++read != Length; )
                        {
                            if (isValue(Array[read])) Array[index++] = Array[read];
                        }
                        setLengthClear(index);
                    }
                }
            }
        }
        /// <summary>
        /// 移除第一个匹配数据，然后将最后一个数据移动到被删除数据位置
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否存在移除数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool RemoveToEnd(T value)
        {
            return TryRemoveAtToEnd(IndexOf(value));
        }
        /// <summary>
        /// 移除数据范围
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="count">移除数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RemoveRangeOnly(int index, int count)
        {
#if DEBUG
            if (index < 0) throw new Exception(index.toString() + " < 0");
            if (count <= 0) throw new Exception(count.toString() + " <= 0");
            if (index + count > Length) throw new Exception(index.toString() + " + " + count.toString() + " > " + Length.toString());
#endif
            AutoCSer.Extensions.ArrayExtension.MoveNotNull(Array, index + count, index, (Length -= count) - index);
            //if (DynamicArray<valueType>.IsClearArray) System.Array.Clear(Array, Length, count);
        }
    }
}
