using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 唯一静态哈希
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class UniqueHashSet<valueType> where valueType : struct, IEquatable<valueType>
    {
        /// <summary>
        /// 哈希数据数组
        /// </summary>
        private valueType[] array;
        /// <summary>
        /// 唯一静态哈希
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="size">哈希容器尺寸</param>
        public unsafe UniqueHashSet(valueType[] values, int size)
        {
            if (size < values.Length) throw new IndexOutOfRangeException(size.toString() + " < " + values.Length.toString());
            if (size == 0) throw new IndexOutOfRangeException();
            array = new valueType[size];
            int count64 = (size + 63) >> 6;
            byte* isValue = stackalloc byte[count64 << 3];
            MemoryMap map = new MemoryMap((ulong*)isValue, count64);
            foreach (valueType value in values)
            {
                int index = value.GetHashCode();
                if ((uint)index >= size) throw new IndexOutOfRangeException(index.toString() + " >= " + size.toString());
                if (map.Get(index) != 0) throw new IndexOutOfRangeException(index.toString() + " is exists");
                map.Set(index);
                array[index] = value;
            }
        }
        /// <summary>
        /// 判断是否存在某值
        /// </summary>
        /// <param name="value">待匹配值</param>
        /// <returns>是否存在某值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Contains(valueType value)
        {
            int index = value.GetHashCode();
            return (uint)index < array.Length && value.Equals(array[index]);
        }
    }
}
