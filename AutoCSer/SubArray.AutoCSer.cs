﻿using System;
using System.Collections.Generic;
using System.Collections;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;

namespace AutoCSer
{
    /// <summary>
    /// 数组子串
    /// </summary>
    public partial struct SubArray<T>
    {
        /// <summary>
        /// 公共数组空子串
        /// </summary>
        internal static SubArray<T> Null;

        ///// <summary>
        ///// 数组子串
        ///// </summary>
        ///// <param name="size">容器大小</param>
        //public SubArray(int size)
        //{
        //    Array = size > 0 ? new valueType[size] : null;
        //    StartIndex = Length = 0;
        //}
        /// <summary>
        /// 原数组
        /// </summary>
        public T[] BufferArray
        {
            get { return Array; }
        }
        /// <summary>
        /// 原数组中的起始位置
        /// </summary>
        public int StartIndex
        {
            get { return Start; }
        }
        ///// <summary>
        ///// 数组空闲字节大小
        ///// </summary>
        //internal int FreeSize
        //{
        //    get { return Array.Length - Length; }
        //}
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="array">数组</param>
        public SubArray(T[] array)
        {
            Array = array;
            Start = 0;
            Length = array == null ? 0 : array.Length;
        }
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="array">数组</param>
        internal SubArray(ref LeftArray<T> array)
        {
            Array = array.Array;
            Start = 0;
            Length = array.Length;
        }
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="array">数组</param>
        internal SubArray(ListArray<T> array) : this(ref array.Array) { }
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        public SubArray(T[] array, int startIndex, int length)
        {
            FormatRange range = new FormatRange(array.length(), startIndex, length);
            Array = array;
            Start = range.SkipCount;
            Length = range.GetCount;
        }
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="array"></param>
        internal SubArray(int startIndex, int length, T[] array)
        {
            Array = array;
            Start = startIndex;
            Length = length;
        }
        /// <summary>
        /// 置空并释放数组
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetNull()
        {
            Array = null;
            Start = Length = 0;
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetFull()
        {
            Start = 0;
            Length = Array == null ? 0 : Array.Length;
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="array"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(T[] array)
        {
            if (array == null) SetNull();
            else Set(array, 0, array.Length);
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public T[] GetArray()
        {
            return Length == 0 ? EmptyArray<T>.Array : getArray();
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <typeparam name="arrayType">数组类型</typeparam>
        /// <param name="getValue">数据获取委托</param>
        /// <returns>数组</returns>
        public arrayType[] GetArray<arrayType>(Func<T, arrayType> getValue)
        {
            if (Length == 0) return EmptyArray<arrayType>.Array;
            arrayType[] newArray = new arrayType[Length];
            int count = 0, index = Start;
            do
            {
                newArray[count] = getValue(Array[index++]);
            }
            while (++count != Length);
            return newArray;
        }
        /// <summary>
        /// 获取子串
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public SubArray<T> GetSub(int startIndex, int length)
        {
            FormatRange range = new FormatRange(Length, startIndex, length);
            if (range.GetCount != length) throw new IndexOutOfRangeException("startIndex[" + startIndex.toString() + "] + length[" + length.toString() + "] > Length[" + Length.toString() + "]");
            SubArray<T> value = new SubArray<T>();
            value.Set(Array, Start + startIndex, length);
            return value;
        }
    }
}
