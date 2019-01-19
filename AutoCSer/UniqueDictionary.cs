using System;
using AutoCSer.Extension;

namespace AutoCSer
{
    /// <summary>
    /// 唯一静态哈希字典
    /// </summary>
    /// <typeparam name="keyType">键值类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal sealed class UniqueDictionary<keyType, valueType> where keyType : struct, IEquatable<keyType>
    {
        /// <summary>
        /// 哈希数据数组
        /// </summary>
        private KeyValue<keyType, valueType>[] array;
        /// <summary>
        /// 唯一静态哈希字典
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="size">哈希容器尺寸</param>
        internal UniqueDictionary(KeyValue<keyType, valueType>[] values, int size)
        {
            if (size < values.Length) throw new IndexOutOfRangeException(size.toString() + " < " + values.Length.toString());
            if (size == 0) throw new IndexOutOfRangeException();
            array = new KeyValue<keyType, valueType>[size];
            if (values.Length != 0) fromArray(values, values.Length, size);
        }
        /// <summary>
        /// 唯一静态哈希字典
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="count">数据数量</param>
        /// <param name="size">哈希容器尺寸</param>
        private unsafe void fromArray(KeyValue<keyType, valueType>[] values, int count, int size)
        {
            int count64 = (size + 63) >> 6;
            byte* isValue = stackalloc byte[count64 << 3];
            MemoryMap map = new MemoryMap((ulong*)isValue, count64);
            do
            {
                KeyValue<keyType, valueType> value = values[--count];
                int index = value.Key.GetHashCode();
                if ((uint)index >= size) throw new IndexOutOfRangeException(index.toString() + " >= " + size.toString());
                if (map.Get(index) != 0) throw new IndexOutOfRangeException(index.toString() + " is exists");
                map.Set(index);
                array[index] = value;
            }
            while (count != 0);
        }
        /// <summary>
        /// 获取匹配数据
        /// </summary>
        /// <param name="key">哈希键值</param>
        /// <param name="value">目标数据</param>
        public void Get(keyType key, ref valueType value)
        {
            int index = key.GetHashCode();
            if ((uint)index < array.Length)
            {
                KeyValue<keyType, valueType> keyValue = array[index];
                if (key.Equals(keyValue.Key))
                {
                    value = keyValue.Value;
                    return;
                }
            }
        }
    }
}
