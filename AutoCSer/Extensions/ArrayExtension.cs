using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public static unsafe partial class ArrayExtension
    {
        /// <summary>
        /// 复制数组数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="subArray"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void CopyTo<T>(this T[] array, SubArray<T> subArray)
        {
            Array.Copy(array, 0, subArray.Array, subArray.Start, array.Length);
        }
        /// <summary>
        /// 复制数组数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="subArray"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void CopyTo<T>(this SubArray<T> array, SubArray<T> subArray)
        {
            array.CopyTo(ref subArray);
        }

        /// <summary>
        /// 数组是否为空或者长度为0
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <returns>数组是否为空或者长度为0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static bool isEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }
        /// <summary>
        /// 获取数组长度
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <returns>null为0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int length<T>(this T[] array)
        {
            return array != null ? array.Length : 0;
        }
        /// <summary>
        /// 空值转0长度数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <returns>非空数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T[] notNull<T>(this T[] array)
        {
            return array != null ? array : EmptyArray<T>.Array;
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <typeparam name="T">目标数组类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <returns>目标数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T[] toArray<T>(this Array array)
        {
            return array != null ? array as T[] : EmptyArray<T>.Array;
        }
        /// <summary>
        /// 根据索引位置获取数组元素值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="array">值集合</param>
        /// <param name="index">索引位置</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>数组元素值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T get<T>(this T[] array, int index, T nullValue)
        {
            return array != null && (uint)index < (uint)array.Length ? array[index] : nullValue;
        }
        /// <summary>
        /// 移动数据块
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">待处理数组</param>
        /// <param name="index">原始数据位置</param>
        /// <param name="writeIndex">目标数据位置</param>
        /// <param name="count">移动数据数量</param>
        internal static void MoveNotNull<T>(T[] array, int index, int writeIndex, int count)
        {
            int endIndex = index + count;
            if (index < writeIndex && endIndex > writeIndex)
            {
                for (int writeEndIndex = writeIndex + count; endIndex != index; array[--writeEndIndex] = array[--endIndex]) ;
            }
            else array.AsSpan(index, count).CopyTo(array.AsSpan(writeIndex, count));
        }
        /// <summary>
        /// 复制数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">待复制数组</param>
        /// <returns>复制后的新数组</returns>
        public static T[] copy<T>(this T[] array)
        {
            if (array.isEmpty()) return EmptyArray<T>.Array;
            T[] newArray = new T[array.Length];
            array.AsSpan().CopyTo(newArray.AsSpan());
            return newArray;
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <returns>连接后的数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T[] getArray<T>(this T[][] array)
        {
            if (array.isEmpty()) return EmptyArray<T>.Array;
            if (array.Length != 1) return getConcatArray(array);
            return array[0].copy();
        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <typeparam name="VT">目标数组类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数组</returns>
        public static VT[] getArray<T, VT>(this T[] array, Func<T, VT> getValue)
        {
            if (array.Length != 0)
            {
                VT[] newValues = new VT[array.Length];
                int index = 0;
                foreach (T value in array) newValues[index++] = getValue(value);
                return newValues;
            }
            return EmptyArray<VT>.Array;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="value">添加的数据</param>
        /// <returns>添加数据的数组</returns>
        public static T[] getAdd<T>(this T[] array, T value)
        {
            if (array != null)
            {
                T[] newArray = new T[array.Length + 1];
                Array.Copy(array, 0, newArray, 0, array.Length);
                newArray[array.Length] = value;
                return newArray;
            }
            return new T[] { value };
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <returns>连接后的数组</returns>
        private static T[] getConcatArray<T>(T[][] array)
        {
            int length = 0;
            foreach (T[] value in array)
            {
                if (value != null) length += value.Length;
            }
            if (length != 0)
            {
                T[] newValues = new T[length];
                length = 0;
                foreach (T[] value in array)
                {
                    if (value != null)
                    {
                        value.CopyTo(newValues, length);
                        length += value.Length;
                    }
                }
                return newValues;
            }
            return EmptyArray<T>.Array;
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <param name="addArray">数组集合</param>
        /// <returns>连接后的数组</returns>
        public static T[] concat<T>(this T[] array, T[] addArray)
        {
            if (addArray.Length == 0) return array;
            if (array.Length == 0) return addArray;
            T[] newArray = new T[array.Length + addArray.Length];
            array.CopyTo(newArray.AsSpan());
            addArray.CopyTo(newArray.AsSpan(array.Length, addArray.Length));
            return newArray;
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <returns>连接后的数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T[] concat<T>(params T[][] array)
        {
            return array.getArray();
        }
        /// <summary>
        /// 复制数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">待复制数组</param>
        /// <param name="newLength">新数组长度</param>
        /// <returns>复制后的新数组</returns>
        public static T[] copyNew<T>(this T[] array, int newLength)
        {
            T[] newArray = new T[newLength];
            array.AsSpan().CopyTo(newArray.AsSpan());
            return newArray;
        }
        /// <summary>
        /// 复制数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">待复制数组</param>
        /// <param name="newLength">新数组长度</param>
        /// <param name="copySize">复制数据数量</param>
        /// <returns>复制后的新数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T[] copyNew<T>(this T[] array, int newLength, int copySize)
        {
            T[] newArray = new T[newLength];
            array.AsSpan(0, copySize).CopyTo(newArray.AsSpan(0, copySize));
            return newArray;
        }
        /// <summary>
        /// 获取第一个匹配值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>第一个匹配值,失败为default(valueType)</returns>
        public static T firstOrDefault<T>(this T[] array, Func<T, bool> isValue)
        {
            if (array != null)
            {
                foreach (T value in array)
                {
                    if (isValue(value)) return value;
                }
            }
            return default(T);
        }
        /// <summary>
        /// 判断是否存在匹配值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>是否存在匹配值</returns>
        public static bool any<T>(this T[] array, Func<T, bool> isValue)
        {
            if (array != null)
            {
                foreach (T value in array)
                {
                    if (isValue(value)) return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取匹配集合
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配集合</returns>
        public static LeftArray<T> getFind<T>(this T[] array, Func<T, bool> isValue)
        {
            if (array != null)
            {
                int length = array.Length;
                if (length != 0)
                {
                    T[] newValues = new T[array.Length < sizeof(int) ? sizeof(int) : length];
                    length = 0;
                    foreach (T value in array)
                    {
                        if (isValue(value)) newValues[length++] = value;
                    }
                    return new LeftArray<T> { Array = newValues, Length = length };
                }
            }
            return new LeftArray<T>(0);
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数组</returns>
        public unsafe static T[] getFindArray<T>(this T[] array, Func<T, bool> isValue)
        {
            int length = array.length();
            if (length == 0) return EmptyArray<T>.Array;
            AutoCSer.Memory.UnmanagedPool pool = AutoCSer.Memory.UnmanagedPool.GetPool(length = ((length + 63) >> 6) << 3);
            AutoCSer.Memory.Pointer buffer = pool.GetMinSize(length);
            try
            {
                AutoCSer.Memory.Common.Clear(buffer.ULong, length >> 3);
                return getFindArray(array, isValue, new MemoryMap(buffer.Data));
            }
            finally { pool.Push(ref buffer); }
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="map">匹配结果位图</param>
        /// <returns>匹配数组</returns>
        private static T[] getFindArray<T>(T[] array, Func<T, bool> isValue, MemoryMap map)
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
            if (length == 0) return EmptyArray<T>.Array;
            T[] newValues = new T[length];
            for (int index = array.Length; length != 0;)
            {
                if (map.Get(--index) != 0) newValues[--length] = array[index];
            }
            return newValues;
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <typeparam name="VT">目标数组类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>匹配数组</returns>
        public unsafe static VT[] getFindArray<T, VT>(this T[] array, Func<T, bool> isValue, Func<T, VT> getValue)
        {
            int length = array.length();
            if (length == 0) return EmptyArray<VT>.Array;
            AutoCSer.Memory.UnmanagedPool pool = AutoCSer.Memory.UnmanagedPool.GetPool(length = ((length + 63) >> 6) << 3);
            AutoCSer.Memory.Pointer buffer = pool.GetMinSize(length);
            try
            {
                AutoCSer.Memory.Common.Clear(buffer.ULong, length >> 3);
                return getFindArray(array, isValue, getValue, new MemoryMap(buffer.Data));
            }
            finally { pool.Push(ref buffer); }
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <typeparam name="VT">目标数组类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="getValue">数据获取器</param>
        /// <param name="map">匹配结果位图</param>
        /// <returns>匹配数组</returns>
        private static VT[] getFindArray<T, VT>(T[] array, Func<T, bool> isValue, Func<T, VT> getValue, MemoryMap map)
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
                VT[] newValues = new VT[length];
                for (int index = array.Length; length != 0;)
                {
                    if (map.Get(--index) != 0) newValues[--length] = getValue(array[index]);
                }
                return newValues;
            }
            return EmptyArray<VT>.Array;
        }
        /// <summary>
        /// 转换HASH
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <typeparam name="VT">目标数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>HASH</returns>
        public static HashSet<VT> getHash<T, VT>(this T[] array, Func<T, VT> getValue)
            where VT : IEquatable<VT>
        {
            if (array != null)
            {
                HashSet<VT> hash = HashSetCreator<VT>.Create();
                foreach (T value in array) hash.Add(getValue(value));
                return hash;
            }
            return null;
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="comparer">比较器</param>
        /// <returns>排序后的数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T[] sort<T>(this T[] array, Func<T, T, int> comparer)
        {
            AutoCSer.Algorithm.QuickSort.Sort(array, comparer);
            return array;
        }
        /// <summary>
        /// 分组计数
        /// </summary>
        /// <typeparam name="VT">数据类型</typeparam>
        /// <typeparam name="KT">分组键类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getKey">键值获取器</param>
        /// <returns>分组计数</returns>
        public static Dictionary<KT, int> groupCount<VT, KT>(this VT[] array, Func<VT, KT> getKey)
            where KT : IEquatable<KT>
        {
            if (array != null)
            {
                Dictionary<KT, int> dictionary = DictionaryCreator<KT>.Create<int>(array.Length);
                int count;
                foreach (VT value in array)
                {
                    KT key = getKey(value);
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
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数据集合</param>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接字符</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string joinString<T>(this T[] array, char join, Func<T, string> toString)
        {
            if (array.Length == 0) return string.Empty;
            return JoinString(array.getArray(toString), join);
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="array">字符串集合，长度必须大于 0</param>
        /// <param name="join">字符连接</param>
        /// <returns>连接后的字符串</returns>
        internal static string JoinString(string[] array, char join)
        {
            int length = 0;
            foreach (string nextString in array) length += nextString.Length;
            string value = StringExtension.FastAllocateString(length + array.Length - 1);
            fixed (char* valueFixed = value)
            {
                char* write = valueFixed;
                foreach (string nextString in array)
                {
                    if (write != valueFixed) *write++ = join;
                    int size = nextString.Length;
                    if (size != 0)
                    {
                        nextString.AsSpan().CopyTo(new Span<char>(write, size));
                        write += size;
                    }
                }
            }
            return value;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">数据集合</param>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string joinString<T>(this T[] array, string join, Func<T, string> toString)
        {
            return string.Join(join, array.getArray(toString));
        }
    }
}
