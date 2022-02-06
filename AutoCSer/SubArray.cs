using System;
using System.Collections.Generic;
using System.Collections;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 数组子串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct SubArray<T> : IList<T>
    {
        /// <summary>
        /// 原数组
        /// </summary>
        internal T[] Array;
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
        public T this[int index]
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
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (Length != 0) return new Enumerator<T>.Array(Array, Start, EndIndex);
            return Enumerator<T>.Empty;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (Length != 0) return new Enumerator<T>.Array(Array, Start, EndIndex);
            return Enumerator<T>.Empty;
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            if (Length != 0)
            {
                if (DynamicArray<T>.IsClearArray) System.Array.Clear(Array, Start, Length);
                Length = 0;
            }
            Start = 0;
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
        /// 数组引用比较
        /// </summary>
        /// <param name="other"></param>
        /// <returns>是否相等</returns>
        internal unsafe NullableBool ReferenceEqual(ref SubArray<T> other)
        {
            if (Length == 0)
            {
                return other.Length == 0 && !((Array == null) ^ (other.Array == null)) ? NullableBool.True : NullableBool.False;
            }
            if (other.Length == Length)
            {
                if (Object.ReferenceEquals(Array, other.Array) && Start == other.Start) return NullableBool.True;
                return NullableBool.Null;
            }
            return NullableBool.False;
        }
        /// <summary>
        /// 设置数据容器长度
        /// </summary>
        /// <param name="count">数据长度</param>
        private void setLength(int count)
        {
            T[] newArray = DynamicArray<T>.GetNewArray(count);
            Array.AsSpan(Start, Length).CopyTo(newArray.AsSpan(Start, Length));
            Array = newArray;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        public void Add(T value)
        {
            if (Array.Length == 0)
            {
                Array = new T[sizeof(int)];
                Array[0] = value;
                Length = 1;
            }
            else
            {
                int index = Start + Length;
                if (index == Array.Length)
                {
                    if (index == 0) Array = new T[sizeof(int)];
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
        public void Insert(int index, T value)
        {
            if ((uint)index <= (uint)Length)
            {
                int copySize = Length - index;
                if (copySize != 0)
                {
                    if (Start + Length == Array.Length)
                    {
                        T[] newArray = DynamicArray<T>.GetNewArray(Array.Length << 1);
                        if (index != 0) Array.AsSpan(Start, index).CopyTo(newArray.AsSpan(Start, index));
                        newArray[index += Start] = value;
                        Array.AsSpan(index, copySize).CopyTo(newArray.AsSpan(index + 1, copySize));
                        Array = newArray;
                    }
                    else
                    {
                        index += Start;
                        Array.AsSpan(index, copySize).CopyTo(Array.AsSpan(index + 1, copySize));
                        Array[index] = value;
                        ++Length;
                    }
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
            if ((uint)index < (uint)Length)
            {
                int copySize = --Length - index;
                index += Start;
                Array.AsSpan(index + 1, copySize).CopyTo(Array.AsSpan(index, copySize));
                Array[Start + Length] = AutoCSer.Common.GetDefault<T>();
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
                    if (Length != 0) Array.AsSpan(Start, Length).CopyTo(values.AsSpan(index, Length));
                    return;
                }
                throw new IndexOutOfRangeException("Length + index[" + (Length + index).toString() + "] > values.Length[" + values.Length.toString() + "]");
            }
            throw new IndexOutOfRangeException("index[" + index.toString() + "]");
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
        internal void Set(T[] value, int startIndex, int length)
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
        /// 复制数组数据
        /// </summary>
        /// <param name="array"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CopyTo(ref SubArray<T> array)
        {
            System.Array.Copy(Array, Start, array.Array, array.Start, Length);
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private T[] getArray()
        {
            T[] newArray = new T[Length];
            System.Array.Copy(Array, Start, newArray, 0, Length);
            return newArray;
        }
        /// <summary>
        /// 获取 fixed 缓冲区，DEBUG 模式对数据范围进行检测
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal T[] GetFixedBuffer()
        {
#if DEBUG
            DebugCheckFixed();
#endif
            return Array;
        }
#if DEBUG
        /// <summary>
        /// fixed 之前检查数据
        /// </summary>
        internal void DebugCheckFixed()
        {
            if (Start < 0) throw new Exception(Start.toString() + " < 0");
            if (Length < 0) throw new Exception(Length.toString() + " < 0");
            if (Start != 0 && Length != 0 && Start + Length > Array.Length) throw new Exception(Start.toString() + " + " + Length.toString() + " > " + Array.Length.toString());
        }
#endif
    }
}
