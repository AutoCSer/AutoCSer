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
    /// <typeparam name="valueType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct LeftArray<valueType> : IList<valueType>
    {
        /// <summary>
        /// 默认数组长度
        /// </summary>
        private const int defalutArraySize = sizeof(int);
        /// <summary>
        /// 原数组
        /// </summary>
        internal valueType[] Array;
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
        /// 设置或获取值
        /// </summary>
        /// <param name="index">位置</param>
        /// <returns>数据值</returns>
        public valueType this[int index]
        {
            get
            {
                if ((uint)index < (uint)Length) return Array[index];
                throw new IndexOutOfRangeException("index[" + index.toString() + "] >= Length[" + Length.toString() + "]");
            }
            set
            {
                if ((uint)index < (uint)Length)
                {
                    Array[index] = value;
                    return;
                }
                throw new IndexOutOfRangeException("index[" + index.toString() + "] >= Length[" + Length.toString() + "]");
            }
        }
        /// <summary>
        /// 只读
        /// </summary>
        public bool IsReadOnly { get { return false; } }
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="size">容器大小</param>
        public LeftArray(int size)
        {
            Array = size > 0 ? new valueType[size] : null;
            Length = 0;
        }
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="value">数组</param>
        public LeftArray(valueType[] value)
        {
            Array = value;
            Length = value == null ? 0 : value.Length;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        IEnumerator<valueType> IEnumerable<valueType>.GetEnumerator()
        {
            if (Length != 0) return new Enumerator<valueType>.Array(this);
            return Enumerator<valueType>.Empty;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (Length != 0) return new Enumerator<valueType>.Array(this);
            return Enumerator<valueType>.Empty;
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
        /// 置空并释放数组
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType[] GetNull()
        {
            valueType[] array = Array;
            SetNull();
            return array;
        }
        /// <summary>
        /// 置空并释放数组
        /// </summary>
        /// <param name="array"></param>
        /// <param name="length"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void GetNull(ref valueType[] array, ref int length)
        {
            array = Array;
            length = Length;
            SetNull();
        }
        /// <summary>
        /// 数组互换
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Exchange(ref LeftArray<valueType> value)
        {
            LeftArray<valueType> temp = value;
            value = this;
            this = temp;
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="value">数组,不能为null</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(valueType[] value)
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
        internal void Set(valueType[] value, int length)
        {
            Array = value;
            Length = length;
        }
        /// <summary>
        /// 设置数据容器长度
        /// </summary>
        /// <param name="count">数据长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void setLength(int count)
        {
            valueType[] newArray = DynamicArray<valueType>.GetNewArray(count);
            System.Array.Copy(Array, 0, newArray, 0, Length);
            Array = newArray;
        }
        /// <summary>
        /// 增加数据长度
        /// </summary>
        /// <param name="length">数据长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void addToLength(int length)
        {
            if (Array == null) Array = new valueType[length < defalutArraySize ? defalutArraySize : length];
            else if (length > Array.Length) setLength(length);
        }
        /// <summary>
        /// 预增长度
        /// </summary>
        /// <param name="length"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void PrepLength(int length)
        {
            if (Array == null) Array = new valueType[length < defalutArraySize ? defalutArraySize : length];
            else if ((length += this.Length) > Array.Length) setLength(Math.Max(length, Array.Length << 1));
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Clear()
        {
            if (Array != null)
            {
                if (DynamicArray<valueType>.IsClearArray) System.Array.Clear(Array, 0, Array.Length);
                Length = 0;
            }
        }
        /// <summary>
        /// 清除当前长度有效数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void ClearOnlyLength()
        {
            if (Array != null)
            {
                System.Array.Clear(Array, 0, Length);
                Length = 0;
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        public void Add(valueType value)
        {
            if (Array == null)
            {
                Array = new valueType[defalutArraySize];
                Array[0] = value;
                Length = 1;
            }
            else
            {
                if (Length == Array.Length)
                {
                    if (Length == 0) Array = new valueType[defalutArraySize];
                    else setLength(Length << 1);
                }
                Array[Length++] = value;
            }
        }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
        public void Add(ICollection<valueType> values)
        {
            int count = values.count();
            if (count != 0)
            {
                addToLength(Length + count);
                foreach (valueType value in values) Array[Length++] = value;
            }
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据</param>
        public void Insert(int index, valueType value)
        {
            if ((uint)index > (uint)Length) throw new IndexOutOfRangeException("index[" + index.toString() + "] > Length[" + Length.toString() + "]");
            if (index == Length)
            {
                Add(value);
                return;
            }
            if (Length == Array.Length)
            {
                valueType[] values = DynamicArray<valueType>.GetNewArray(Length << 1);
                System.Array.Copy(Array, 0, values, 0, index);
                values[index] = value;
                System.Array.Copy(Array, index, values, index + 1, Length++ - index);
                Array = values;
            }
            else
            {
                AutoCSer.Extension.ArrayExtension.MoveNotNull(Array, index, index + 1, Length - index);
                Array[index] = value;
                ++Length;
            }
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>是否存在数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Contains(valueType value)
        {
            return IndexOf(value) != -1;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int IndexOf(valueType value)
        {
            return Length == 0 ? -1 : System.Array.IndexOf(Array, value, 0, Length);
        }
        /// <summary>
        /// 获取获取数组中的匹配位置
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>数组中的匹配位置,失败为-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int indexOf(Func<valueType, bool> isValue)
        {
            int index = 0;
            foreach (valueType value in Array)
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
        public int IndexOf(Func<valueType, bool> isValue)
        {
            return Length == 0 ? -1 : indexOf(isValue);
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否存在移除数据</returns>
        public bool Remove(valueType value)
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
            if ((uint)index >= (uint)Length) throw new IndexOutOfRangeException("index[" + index.toString() + "] >= Length[" + Length.toString() + "]");
            AutoCSer.Extension.ArrayExtension.MoveNotNull(Array, index + 1, index, --Length - index);
            Array[Length] = default(valueType);
        }
        /// <summary>
        /// 最后一个数据移动到被删除数据位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool removeAtToEnd(int index)
        {
            if (index >= 0)
            {
                if (index != --Length) Array[index] = Array[Length];
                Array[Length] = default(valueType);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 移除数据，然后将最后一个数据移动到被删除数据位置
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>是否存在移除数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool RemoveToEnd(Func<valueType, bool> isValue)
        {
            return removeAtToEnd(IndexOf(isValue));
        }
        /// <summary>
        /// 弹出最后一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType UnsafePop()
        {
            valueType value = Array[--Length];
            Array[Length] = default(valueType);
            return value;
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="values">目标数据</param>
        /// <param name="index">目标位置</param>
        public void CopyTo(valueType[] values, int index)
        {
            if (index < 0) throw new IndexOutOfRangeException("index[" + index.toString() + "]");
            if (Length + index > values.Length) throw new IndexOutOfRangeException("Length + index[" + (Length + index).toString() + "] > values.Length[" + values.Length.toString() + "]");
            if (Length != 0) System.Array.Copy(Array, 0, values, index, Length);
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType[] ToArray()
        {
            if (Length == 0) return NullValue<valueType>.Array;
            return Length == Array.Length ? Array : getArray();
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private valueType[] getArray()
        {
            valueType[] newArray = new valueType[Length];
            System.Array.Copy(Array, 0, newArray, 0, Length);
            return newArray;
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType[] GetArray()
        {
            return Length != 0 ? getArray() : NullValue<valueType>.Array;
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="comparer">比较器</param>
        /// <returns>排序后的数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public LeftArray<valueType> Sort(Func<valueType, valueType, int> comparer)
        {
            AutoCSer.Algorithm.QuickSort.Sort(Array, comparer, 0, Length);
            return this;
        }
    }
}
