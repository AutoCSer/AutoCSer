using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 枚举器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class Enumerator<T>
    {
        /// <summary>
        /// 空枚举器
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct EmptyEnumerator : IEnumerator<T>
        {
            /// <summary>
            /// 当前数据元素
            /// </summary>
            T IEnumerator<T>.Current
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
            /// <summary>
            /// 当前数据元素
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
            /// <summary>
            /// 转到下一个数据元素
            /// </summary>
            /// <returns>是否存在下一个数据元素</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public bool MoveNext()
            {
                return false;
            }
            /// <summary>
            /// 重置枚举器状态
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Reset() { }
            /// <summary>
            /// 释放枚举器
            /// </summary>
            public void Dispose() { }
        }
        /// <summary>
        /// 空枚举实例
        /// </summary>
        internal static readonly IEnumerator<T> Empty = new EmptyEnumerator();

        /// <summary>
        /// 数组枚举器
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct Array : IEnumerator<T>
        {
            /// <summary>
            /// 被枚举数组
            /// </summary>
            private T[] array;
            /// <summary>
            /// 当前位置
            /// </summary>
            private int currentIndex;
            /// <summary>
            /// 结束位置
            /// </summary>
            private int endIndex;
            /// <summary>
            /// 起始位置
            /// </summary>
            private int startIndex;
            /// <summary>
            /// 数组枚举器
            /// </summary>
            /// <param name="value">数组子串</param>
            public Array(LeftArray<T> value)
            {
                array = value.Array;
                startIndex = 0;
                endIndex = value.Length;
                currentIndex = startIndex - 1;
            }
            /// <summary>
            /// 数组枚举器
            /// </summary>
            /// <param name="array">数组</param>
            /// <param name="startIndex">起始位置</param>
            /// <param name="endIndex">结束位置</param>
            public Array(T[] array, int startIndex, int endIndex)
            {
                this.array = array;
                this.startIndex = startIndex;
                this.endIndex = endIndex;
                currentIndex = startIndex - 1;
            }
            /// <summary>
            /// 当前数据元素
            /// </summary>
            T IEnumerator<T>.Current
            {
                get { return array[currentIndex]; }
            }
            /// <summary>
            /// 当前数据元素
            /// </summary>
            object IEnumerator.Current
            {
                get { return array[currentIndex]; }
            }
            /// <summary>
            /// 转到下一个数据元素
            /// </summary>
            /// <returns>是否存在下一个数据元素</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public bool MoveNext()
            {
                if (++currentIndex != endIndex) return true;
                --currentIndex;
                return false;
            }
            /// <summary>
            /// 重置枚举器状态
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Reset()
            {
                currentIndex = startIndex - 1;
            }
            /// <summary>
            /// 释放枚举器
            /// </summary>
            public void Dispose() { }
        }
    }
}
