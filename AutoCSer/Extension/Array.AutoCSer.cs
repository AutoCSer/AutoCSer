using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public static partial class ArrayExtension
    {
        /// <summary>
        /// 复制数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待复制数组</param>
        /// <param name="newLength">新数组长度</param>
        /// <param name="copySize">复制数据数量</param>
        /// <returns>复制后的新数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] copyNew<valueType>(this valueType[] array, int newLength, int copySize)
        {
            valueType[] newArray = new valueType[newLength];
            System.Array.Copy(array, 0, newArray, 0, copySize);
            return newArray;
        }
        /// <summary>
        /// 获取第一个匹配值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>第一个匹配值,失败为default(valueType)</returns>
        public static valueType firstOrDefault<valueType>(this valueType[] array, Func<valueType, bool> isValue)
        {
            if (array != null)
            {
                foreach (valueType value in array)
                {
                    if (isValue(value)) return value;
                }
            }
            return default(valueType);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <typeparam name="valueType">目标数组类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <returns>目标数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] toArray<valueType>(this Array array)
        {
            return array != null ? array as valueType[] : NullValue<valueType>.Array;
        }
        /// <summary>
        /// 根据索引位置获取数组元素值
        /// </summary>
        /// <typeparam name="valueType">值类型</typeparam>
        /// <param name="array">值集合</param>
        /// <param name="index">索引位置</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>数组元素值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType get<valueType>(this valueType[] array, int index, valueType nullValue)
        {
            return array != null && (uint)index < (uint)array.Length ? array[index] : nullValue;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="value">添加的数据</param>
        /// <returns>添加数据的数组</returns>
        public static valueType[] getAdd<valueType>(this valueType[] array, valueType value)
        {
            if (array != null)
            {
                valueType[] newArray = new valueType[array.Length + 1];
                Array.Copy(array, 0, newArray, 0, array.Length);
                newArray[array.Length] = value;
                return newArray;
            }
            return new valueType[] { value };
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数组类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>匹配数组</returns>
        public unsafe static arrayType[] getFindArray<valueType, arrayType>
            (this valueType[] array, Func<valueType, bool> isValue, Func<valueType, arrayType> getValue)
        {
            int length = array.length();
            if (length == 0) return NullValue<arrayType>.Array;
            UnmanagedPool pool = AutoCSer.UnmanagedPool.GetDefaultPool(length = ((length + 63) >> 6) << 3);
            Pointer.Size buffer = pool.GetSize64(length);
            try
            {
                Memory.ClearUnsafe(buffer.ULong, length >> 3);
                return getFindArray(array, isValue, getValue, new MemoryMap(buffer.Data));
            }
            finally { pool.PushOnly(ref buffer); }
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数组类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="getValue">数据获取器</param>
        /// <param name="map">匹配结果位图</param>
        /// <returns>匹配数组</returns>
        private static arrayType[] getFindArray<valueType, arrayType>
            (valueType[] array, Func<valueType, bool> isValue, Func<valueType, arrayType> getValue, MemoryMap map)
        {
            int length = 0;
            for (int index = 0; index != array.Length; ++index)
            {
                if (isValue(array[index]))
                {
                    ++length;
                    map.Set(index);
                }
            }
            if (length != 0)
            {
                arrayType[] newValues = new arrayType[length];
                for (int index = array.Length; length != 0; )
                {
                    if (map.Get(--index) != 0) newValues[--length] = getValue(array[index]);
                }
                return newValues;
            }
            return NullValue<arrayType>.Array;
        }

        /// <summary>
        /// 取子集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="index">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>子集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] getSub<valueType>(this valueType[] array, int index, int count)
        {
            if (index < 0) throw new IndexOutOfRangeException("index[" + index.toString() + "] < 0");
            if (index + count > array.Length) throw new IndexOutOfRangeException("index[" + index.toString() + "] + count[" + count.toString() + "] > array.Length[" + array.Length.toString() + "]");
            return new SubArray<valueType> { Array = array, Start = index, Length = count }.GetArray();
        }
        /// <summary>
        /// 判断是否存在匹配值
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>是否存在匹配值</returns>
        public static bool any<valueType>(this valueType[] array, Func<valueType, bool> isValue)
        {
            if (array != null)
            {
                foreach (valueType value in array)
                {
                    if (isValue(value)) return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取匹配集合
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配集合</returns>
        public static LeftArray<valueType> getFind<valueType>(this valueType[] array, Func<valueType, bool> isValue)
        {
            if (array != null)
            {
                int length = array.Length;
                if (length != 0)
                {
                    valueType[] newValues = new valueType[array.Length < sizeof(int) ? sizeof(int) : length];
                    length = 0;
                    foreach (valueType value in array)
                    {
                        if (isValue(value)) newValues[length++] = value;
                    }
                    return new LeftArray<valueType> { Array = newValues, Length = length };
                }
            }
            return default(LeftArray<valueType>);
        }
        /// <summary>
        /// 转换HASH
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="hashType">目标数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>HASH</returns>
        public static HashSet<hashType> getHash<valueType, hashType>(this valueType[] array, Func<valueType, hashType> getValue)
        {
            if (array != null)
            {
                HashSet<hashType> hash = HashSetCreator<hashType>.Create();
                foreach (valueType value in array) hash.Add(getValue(value));
                return hash;
            }
            return null;
        }
        /// <summary>
        /// 分组计数
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="keyType">分组键类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getKey">键值获取器</param>
        /// <returns>分组计数</returns>
        public static Dictionary<keyType, int> groupCount<valueType, keyType>(this valueType[] array, Func<valueType, keyType> getKey)
            where keyType : IEquatable<keyType>
        {
            if (array != null)
            {
                Dictionary<keyType, int> dictionary = DictionaryCreator<keyType>.Create<int>(array.Length);
                int count;
                foreach (valueType value in array)
                {
                    keyType key = getKey(value);
                    if (dictionary.TryGetValue(key, out count)) dictionary[key] = count + 1;
                    else dictionary.Add(key, 1);
                }
                return dictionary;
            }
            return null;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据集合</param>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string joinString<valueType>(this valueType[] array, string join, Func<valueType, string> toString)
        {
            return string.Join(join, array.getArray(toString));
        }
    }
}
