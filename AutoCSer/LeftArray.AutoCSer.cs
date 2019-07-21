using System;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Collections;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 数组子串
    /// </summary>
    public partial struct LeftArray<valueType>
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
        public LeftArray(ICollection<valueType> values)
        {
            Length = 0;
            if (values == null) Array = null;
            else
            {
                int count = values.Count;
                if (count == 0) Array = NullValue<valueType>.Array;
                else
                {
                    Array = new valueType[count];
                    foreach (valueType value in values)
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
        public LeftArray(IEnumerable<valueType> values)
        {
            Array = null;
            Length = 0;
            if (values != null)
            {
                foreach (valueType value in values) Add(value);
            }
        }
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="array"></param>
        internal LeftArray(ListArray<valueType> array)
        {
            if (array == null)
            {
                Array = null;
                Length = 0;
            }
            else
            {
                Array = array.Array;
                Length = array.Length;
            }
        }
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="value">数组</param>
        internal LeftArray(int length, valueType[] value)
        {
            Array = value;
            Length = length;
        }
        /// <summary>
        /// 长度设为0（注意：对于引用类型没有置 0 可能导致内存泄露）
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Empty()
        {
            Length = 0;
        }
        /// <summary>
        /// 返回非 null 数组  
        /// </summary>
        /// <returns></returns>
        internal LeftArray<valueType> NotNull()
        {
            if (Array == null) return new LeftArray<valueType>(NullValue<valueType>.Array);
            return this;
        }
        /// <summary>
        /// 获取最后一个值
        /// </summary>
        internal valueType UnsafeLast
        {
            get { return Array[Length - 1]; }
        }
        /// <summary>
        /// 获取最后一个值
        /// </summary>
        /// <returns>最后一个值,失败为default(valueType)</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType LastOrDefault()
        {
            return Length != 0 ? Array[Length - 1] : default(valueType);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeAdd(valueType value)
        {
            Array[Length++] = value;
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="array">数据集合</param>
        public void Add(valueType[] array)
        {
            int count = array.length();
            if (count != 0)
            {
                addToLength(Length + count);
                System.Array.Copy(array, 0, Array, Length, count);
                Length += count;
            }
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        public void Add(ref LeftArray<valueType> values)
        {
            if (values.Length != 0)
            {
                addToLength(Length + values.Length);
                System.Array.Copy(values.Array, 0, Array, Length, values.Length);
                Length += values.Length;
            }
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        public void Add(ListArray<valueType> values)
        {
            if (values != null && values.Length != 0)
            {
                addToLength(Length + values.Length);
                System.Array.Copy(values.Array, 0, Array, Length, values.Length);
                Length += values.Length;
            }
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <typeparam name="collectionValueType">集合数据类型</typeparam>
        /// <param name="values">数据集合</param>
        /// <param name="getValue">获取数据委托</param>
        public void Add<collectionValueType>(ICollection<collectionValueType> values, Func<collectionValueType, valueType> getValue)
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
        internal valueType UnsafePopOnly()
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
        /// 转换数组
        /// </summary>
        /// <typeparam name="arrayType">数组类型</typeparam>
        /// <param name="getValue">数据获取委托</param>
        /// <returns>数组</returns>
        public arrayType[] GetArray<arrayType>(Func<valueType, arrayType> getValue)
        {
            if (Length == 0) return NullValue<arrayType>.Array;
            arrayType[] newArray = new arrayType[Length];
            int index = 0;
            do
            {
                newArray[index] = getValue(Array[index]);
            }
            while (++index != Length);
            return newArray;
        }
        /// <summary>
        /// 获取第一个匹配值
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值,失败为 default(valueType)</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType FirstOrDefault(Func<valueType, bool> isValue)
        {
            int index = indexOf(isValue);
            return index != -1 ? Array[index] : default(valueType);
        }
        /// <summary>
        /// 获取匹配值集合
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配值集合</returns>
        public unsafe valueType[] GetFindArray(Func<valueType, bool> isValue)
        {
            if (Length == 0) return NullValue<valueType>.Array;
            int length = ((Length + 63) >> 6) << 3;
            UnmanagedPool pool = AutoCSer.UnmanagedPool.GetDefaultPool(length);
            Pointer.Size buffer = pool.GetSize64(length);
            try
            {
                Memory.ClearUnsafe(buffer.ULong, length >> 3);
                return getFindArray(isValue, new MemoryMap(buffer.Data));
            }
            finally { pool.PushOnly(ref buffer); }
        }
        /// <summary>
        /// 获取匹配值集合
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="map">匹配结果位图</param>
        /// <returns>匹配值集合</returns>
        private valueType[] getFindArray(Func<valueType, bool> isValue, MemoryMap map)
        {
            int count = 0, index = 0;
            foreach (valueType value in Array)
            {
                if (isValue(value))
                {
                    ++count;
                    map.Set(index);
                }
                if (++index == Length) break;
            }
            if (count == 0) return NullValue<valueType>.Array;
            valueType[] values = new valueType[count];
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
        public string JoinString(string join, Func<valueType, string> toString)
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
            if (DynamicArray<valueType>.IsClearArray) System.Array.Clear(Array, length, Length - length);
            Length = length;
        }
        /// <summary>
        /// 移除所有后端匹配值
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        private void removeEnd(Func<valueType, bool> isValue)
        {
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
        internal void Remove(Func<valueType, bool> isValue)
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
        private void removeEndNot(Func<valueType, bool> isValue)
        {
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
        private int indexOfNot(Func<valueType, bool> isValue)
        {
            int index = 0;
            foreach (valueType value in Array)
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
        internal void RemoveNot(Func<valueType, bool> isValue)
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
        /// 移除数据，然后将最后一个数据移动到被删除数据位置
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否存在移除数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool RemoveToEnd(valueType value)
        {
            return removeAtToEnd(IndexOf(value));
        }
        /// <summary>
        /// 移除数据范围
        /// </summary>
        /// <param name="index">起始位置</param>
        /// <param name="count">移除数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RemoveRangeOnly(int index, int count)
        {
            AutoCSer.Extension.ArrayExtension.MoveNotNull(Array, index + count, index, (Length -= count) - index);
            //if (DynamicArray<valueType>.IsClearArray) System.Array.Clear(Array, Length, count);
        }
    }
}
