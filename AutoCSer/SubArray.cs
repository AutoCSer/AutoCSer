using System;
using System.Collections.Generic;
using System.Collections;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 数组子串
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct SubArray<valueType> : IList<valueType>
    {
        /// <summary>
        /// 原数组
        /// </summary>
        internal valueType[] Array;
        /// <summary>
        /// 原数组中的起始位置
        /// </summary>
        internal int Start;
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
        /// 数据结束位置
        /// </summary>
        public int EndIndex
        {
            get
            {
                return Start + Length;
            }
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
                if ((uint)index < (uint)Length) return Array[Start + index];
                throw new IndexOutOfRangeException("index[" + index.toString() + "] >= Length[" + Length.toString() + "]");
            }
            set
            {
                if ((uint)index < (uint)Length)
                {
                    Array[Start + index] = value;
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
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        IEnumerator<valueType> IEnumerable<valueType>.GetEnumerator()
        {
            if (Length != 0) return new Enumerator<valueType>.Array(Array, Start, EndIndex);
            return Enumerator<valueType>.Empty;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (Length != 0) return new Enumerator<valueType>.Array(Array, Start, EndIndex);
            return Enumerator<valueType>.Empty;
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
                Empty();
            }
        }
        /// <summary>
        /// 长度设为0（注意：对于引用类型没有置 0 可能导致内存泄露）
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Empty()
        {
            Start = Length = 0;
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="startIndex">起始位置,必须合法</param>
        /// <param name="length">长度,必须合法</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(int startIndex, int length)
        {
            Start = startIndex;
            Length = length;
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="value">数组,不能为null</param>
        /// <param name="startIndex">起始位置,必须合法</param>
        /// <param name="length">长度,必须合法</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(valueType[] value, int startIndex, int length)
        {
            Array = value;
            Start = startIndex;
            Length = length;
        }
        /// <summary>
        /// 修改起始位置
        /// </summary>
        /// <param name="count"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void MoveStart(int count)
        {
            Start += count;
            Length -= count;
        }
        /// <summary>
        /// 设置数据容器长度
        /// </summary>
        /// <param name="count">数据长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void setLength(int count)
        {
            valueType[] newArray = DynamicArray<valueType>.GetNewArray(count);
            System.Array.Copy(Array, Start, newArray, Start, Length);
            Array = newArray;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        public void Add(valueType value)
        {
            if (Array == null)
            {
                Array = new valueType[sizeof(int)];
                Array[0] = value;
                Length = 1;
            }
            else
            {
                int index = Start + Length;
                if (index == Array.Length)
                {
                    if (index == 0) Array = new valueType[sizeof(int)];
                    else setLength(index << 1);
                }
                Array[index] = value;
                ++Length;
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
            if (Start + Length == Array.Length)
            {
                valueType[] values = DynamicArray<valueType>.GetNewArray(Array.Length << 1);
                System.Array.Copy(Array, Start, values, Start, index);
                values[Start + index] = value;
                System.Array.Copy(Array, Start + index, values, Start + index + 1, Length++ - index);
                Array = values;
            }
            else
            {
                AutoCSer.Extension.ArrayExtension.MoveNotNull(Array, Start + index, Start + index + 1, Length - index);
                Array[Start + index] = value;
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
            if (Length != 0)
            {
                int index = System.Array.IndexOf(Array, value, Start, Length);
                if (index >= 0) return index - Start;
            }
            return -1;
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
            AutoCSer.Extension.ArrayExtension.MoveNotNull(Array, Start + index + 1, Start + index, --Length - index);
            Array[Start + Length] = default(valueType);
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
            if (Length != 0) System.Array.Copy(Array, Start, values, index, Length);
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
            System.Array.Copy(Array, Start, newArray, 0, Length);
            return newArray;
        }
    }
}
