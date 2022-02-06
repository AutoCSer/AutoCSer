using System;
using System.Collections.Generic;
using AutoCSer.Extensions;
using System.Collections;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 数组子串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct LeftArray<T> : IList<T>
    {
        /// <summary>
        /// 原数组
        /// </summary>
        internal T[] Array;
        /// <summary>
        /// 长度
        /// </summary>
        internal int Length;
        /// <summary>
        /// 长度
        /// </summary>
        public int Count
        {
            get { return Length; }
        }
        /// <summary>
        /// 保留字段
        /// </summary>
        internal int Reserve;
        /// <summary>
        /// 只读
        /// </summary>
        public bool IsReadOnly { get { return false; } }
        /// <summary>
        /// 空闲元素数量
        /// </summary>
        public int FreeCount
        {
            get { return Array.Length - Length; }
        }
        /// <summary>
        /// 设置或获取值
        /// </summary>
        /// <param name="index">位置</param>
        /// <returns>数据值</returns>
        public T this[int index]
        {
            get
            {
                if ((uint)index < (uint)Length) return Array[index];
                throw new IndexOutOfRangeException("index[" + index.toString() + "] >= Length[" + Length.toString() + "]");
            }
            set
            {
                if ((uint)index < (uint)Length) Array[index] = value;
                else throw new IndexOutOfRangeException("index[" + index.toString() + "] >= Length[" + Length.toString() + "]");
            }
        }
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="capacity">容器大小</param>
        public LeftArray(int capacity)
        {
            Array = capacity > 0 ? new T[capacity] : EmptyArray<T>.Array;
            Length = Reserve = 0;
        }
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="array">数组</param>
        public LeftArray(T[] array) : this(array.Length, array) { }
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="length">初始化数据长度</param>
        /// <param name="array">原数组</param>
        internal LeftArray(int length, T[] array)
        {
            Array = array;
            Length = length;
            Reserve = 0;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (Length != 0) return new Enumerator<T>.Array(this);
            return Enumerator<T>.Empty;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (Length != 0) return new Enumerator<T>.Array(this);
            return Enumerator<T>.Empty;
        }
        /// <summary>
        /// 设置数据容器长度
        /// </summary>
        /// <param name="capacity">数据长度</param>
        private void setCapacity(int capacity)
        {
            T[] newArray = DynamicArray<T>.GetNewArray(capacity);
            Array.CopyTo(newArray.AsSpan());
            Array = newArray;
        }
        /// <summary>
        /// 增加数据长度
        /// </summary>
        /// <param name="length">数据长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void addToLength(int length)
        {
            if (length > Array.Length) setCapacity(length);
        }
        /// <summary>
        /// 预增长度
        /// </summary>
        /// <param name="length"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void PrepLength(int length)
        {
            if ((length += Length) > Array.Length)
            {
                long newSize = (long)Array.Length << 1;
                if (newSize <= int.MaxValue) setCapacity(Math.Max(length, (int)newSize));
                else setCapacity(int.MaxValue);
            }
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            if (Array.Length != 0)
            {
                if (DynamicArray<T>.IsClearArray) System.Array.Clear(Array, 0, Array.Length);
                Length = 0;
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeAdd(T value)
        {
            Array[Length++] = value;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        public void Add(T value)
        {
            if (Array.Length == 0)
            {
                Array = new T[DynamicArray.DefalutArrayCapacity];
                Array[0] = value;
                Length = 1;
            }
            else
            {
                if (Length == Array.Length) setCapacity(Length << 1);
                Array[Length++] = value;
            }
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        public void Add(ICollection<T> values)
        {
            int count = values.count();
            if (count != 0)
            {
                addToLength(Length + count);
                foreach (T value in values) Array[Length++] = value;
            }
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="array">数据集合</param>
        public void Add(T[] array)
        {
            int count = array.Length;
            if (count != 0)
            {
                addToLength(Length + count);
                array.AsSpan().CopyTo(Array.AsSpan(Length, count));
                Length += count;
            }
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="array">数据集合</param>
        public void Add(ref LeftArray<T> array)
        {
            if (array.Length != 0)
            {
                addToLength(Length + array.Length);
                array.Array.AsSpan(0, array.Length).CopyTo(Array.AsSpan(Length, array.Length));
                Length += array.Length;
            }
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据</param>
        public void Insert(int index, T value)
        {
            if ((uint)index <= (uint)Length)
            {
                if (index != Length)
                {
                    int size = Length - index;
                    if (Length == Array.Length)
                    {
                        T[] values = DynamicArray<T>.GetNewArray(Length << 1);
                        Array.AsSpan(0, index).CopyTo(values.AsSpan());
                        Array.AsSpan(index, size).CopyTo(values.AsSpan(index + 1, size));
                        Array = values;
                    }
                    else Array.AsSpan(index, size).CopyTo(Array.AsSpan(index + 1, size));
                    Array[index] = value;
                    ++Length;
                }
                else Add(value);
                return;
            }
            throw new IndexOutOfRangeException("index[" + index.toString() + "] > Length[" + Length.toString() + "]");
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>是否存在数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Contains(T value)
        {
            return IndexOf(value) != -1;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int IndexOf(T value)
        {
            return Length == 0 ? -1 : System.Array.IndexOf(Array, value, 0, Length);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否存在移除数据</returns>
        public bool Remove(T value)
        {
            int index = IndexOf(value);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="index">数据位置</param>
        /// <returns>被移除数据</returns>
        public void RemoveAt(int index)
        {
            int size = Length - index;
            if (size > 0)
            {
                --size;
                Array.AsSpan(index + 1, size).CopyTo(Array.AsSpan(index, size));
                --Length;
                Array[Length] = AutoCSer.Common.GetDefault<T>();
                return;
            }
            throw new IndexOutOfRangeException("index[" + index.toString() + "] >= Length[" + Length.toString() + "]");
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="values">目标数据</param>
        /// <param name="index">目标位置</param>
        public void CopyTo(T[] values, int index)
        {
            if (index >= 0)
            {
                if (Length + index <= values.Length)
                {
                    if (Length != 0) AsSpan().CopyTo(values.AsSpan(index, Length));
                    return;
                }
                throw new IndexOutOfRangeException("Length + index[" + (Length + index).toString() + "] > values.Length[" + values.Length.toString() + "]");
            }
            throw new IndexOutOfRangeException("index[" + index.toString() + "]");
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public T[] ToArray()
        {
            if (Length == 0) return EmptyArray<T>.Array;
            return Length == Array.Length ? Array : getArray();
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        private T[] getArray()
        {
            T[] newArray = new T[Length];
            Array.AsSpan(0, Length).CopyTo(newArray.AsSpan());
            return newArray;
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public T[] GetArray()
        {
            return Length != 0 ? getArray() : EmptyArray<T>.Array;
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <typeparam name="VT">数组类型</typeparam>
        /// <param name="getValue">数据获取委托</param>
        /// <returns>数组</returns>
        public VT[] GetArray<VT>(Func<T, VT> getValue)
        {
            if (Length == 0) return EmptyArray<VT>.Array;
            VT[] newArray = new VT[Length];
            int index = 0;
            do
            {
                newArray[index] = getValue(Array[index]);
            }
            while (++index != Length);
            return newArray;
        }

        /// <summary>
        /// 置空并释放数组
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetNull()
        {
            Array = null;
            Length = 0;
        }
        /// <summary>
        /// 设置为非空数组
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void NotNull()
        {
            if (Array == null) Array = EmptyArray<T>.Array;
        }
        /// <summary>
        /// 置空并释放数组
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetEmpty()
        {
            Array = EmptyArray<T>.Array;
            Length = 0;
        }
        /// <summary>
        /// 置空并释放数组
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal T[] GetArraySetEmpty()
        {
            T[] array = Array;
            SetEmpty();
            return array;
        }
        /// <summary>
        /// 置空并释放数组
        /// </summary>
        /// <param name="array"></param>
        /// <param name="length"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void GetArraySetEmpty(ref T[] array, ref int length)
        {
            array = Array;
            length = Length;
            SetEmpty();
        }
        /// <summary>
        /// 数组互换
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Exchange(ref LeftArray<T> value)
        {
            LeftArray<T> temp = value;
            value = this;
            this = temp;
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="value">数组,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(T[] value)
        {
            Array = value;
            Length = value.Length;
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="value">数组,不能为null</param>
        /// <param name="length">长度,必须合法</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(T[] value, int length)
        {
            Array = value;
            Length = length;
        }
        /// <summary>
        /// 清除当前长度有效数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void ClearOnlyLength()
        {
            if (Array.Length != 0)
            {
                if (DynamicArray<T>.IsClearArray) System.Array.Clear(Array, 0, Length);
                Length = 0;
            }
        }
        /// <summary>
        /// 获取获取数组中的匹配位置
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>数组中的匹配位置,失败为-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int indexOf(Func<T, bool> isValue)
        {
            int index = 0;
            foreach (T value in Array)
            {
                if (isValue(value)) return index;
                if (++index == Length) return -1;
            }
            return -1;
        }
        /// <summary>
        /// 获取获取数组中的匹配位置
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>数组中的匹配位置,失败为-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int IndexOf(Func<T, bool> isValue)
        {
            return Length == 0 ? -1 : indexOf(isValue);
        }
        /// <summary>
        /// 最后一个数据移动到被删除数据位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal bool TryRemoveAtToEnd(int index)
        {
            if (index >= 0)
            {
                if (index != --Length) Array[index] = Array[Length];
                Array[Length] = default(T);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 移除第一个匹配数据，然后将最后一个数据移动到被删除数据位置
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>是否存在移除数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool RemoveToEnd(Func<T, bool> isValue)
        {
            return TryRemoveAtToEnd(IndexOf(isValue));
        }
        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal T UnsafePop()
        {
            T value = Array[--Length];
            Array[Length] = default(T);
            return value;
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="comparer">比较器</param>
        /// <returns>排序后的数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public LeftArray<T> Sort(Func<T, T, int> comparer)
        {
            AutoCSer.Algorithm.QuickSort.Sort(Array, comparer, 0, Length);
            return this;
        }
        /// <summary>
        /// 获取 fixed 缓冲区，DEBUG 模式对数据范围进行检测
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal T[] GetFixedBuffer()
        {
#if DEBUG
            if (Length < 0) throw new Exception(Length.toString() + " < 0");
            if (Length > Array.Length) throw new Exception(Length.toString() + " > " + Array.Length.toString());
#endif
            return Array;
        }
    }
}
