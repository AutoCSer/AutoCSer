using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数组扩展操作
    /// </summary>
    public static partial class ArrayExtension
    {
        /// <summary>
        /// 数组是否为空或者长度为0
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <returns>数组是否为空或者长度为0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static bool isEmpty<valueType>(this valueType[] array)
        {
            return array == null || array.Length == 0;
        }
        /// <summary>
        /// 获取数组长度
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <returns>null为0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static int length<valueType>(this valueType[] array)
        {
            return array != null ? array.Length : 0;
        }
        /// <summary>
        /// 空值转0长度数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <returns>非空数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] notNull<valueType>(this valueType[] array)
        {
            return array != null ? array : NullValue<valueType>.Array;
        }
        /// <summary>
        /// 移动数据块
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待处理数组</param>
        /// <param name="index">原始数据位置</param>
        /// <param name="writeIndex">目标数据位置</param>
        /// <param name="count">移动数据数量</param>
        internal static void MoveNotNull<valueType>(valueType[] array, int index, int writeIndex, int count)
        {
            int endIndex = index + count;
            if (index < writeIndex && endIndex > writeIndex)
            {
                for (int writeEndIndex = writeIndex + count; endIndex != index; array[--writeEndIndex] = array[--endIndex]) ;
            }
            else System.Array.Copy(array, index, array, writeIndex, count);
        }
        /// <summary>
        /// 复制数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待复制数组</param>
        /// <returns>复制后的新数组</returns>
        public static valueType[] copy<valueType>(this valueType[] array)
        {
            if (array.isEmpty()) return NullValue<valueType>.Array;
            valueType[] newArray = new valueType[array.Length];
            System.Array.Copy(array, 0, newArray, 0, array.Length);
            return newArray;
        }
        /// <summary>
        /// 复制数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待复制数组</param>
        /// <param name="newLength">新数组长度</param>
        /// <returns>复制后的新数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] copyNew<valueType>(this valueType[] array, int newLength)
        {
            valueType[] newArray = new valueType[newLength];
            System.Array.Copy(array, 0, newArray, 0, array.Length);
            return newArray;
        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="arrayType">目标数组类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="getValue">数据获取器</param>
        /// <returns>目标数组</returns>
        public static arrayType[] getArray<valueType, arrayType>(this valueType[] array, Func<valueType, arrayType> getValue)
        {
            if (array.isEmpty()) return NullValue<arrayType>.Array;
            arrayType[] newValues = new arrayType[array.Length];
            int index = 0;
            foreach (valueType value in array) newValues[index++] = getValue(value);
            return newValues;
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <param name="addValues">数组集合</param>
        /// <returns>连接后的数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] concat<valueType>(this valueType[] array, valueType[] addValues)
        {
            return getArray(new valueType[][] { array, addValues });
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <returns>连接后的数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] getArray<valueType>(this valueType[][] array)
        {
            if (array.isEmpty()) return NullValue<valueType>.Array;
            if (array.Length != 1) return getConcatArray(array);
            return array[0].copy();
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <returns>连接后的数组</returns>
        private static valueType[] getConcatArray<valueType>(valueType[][] array)
        {
            int length = 0;
            foreach (valueType[] value in array)
            {
                if (value != null) length += value.Length;
            }
            if (length != 0)
            {
                valueType[] newValues = new valueType[length];
                length = 0;
                foreach (valueType[] value in array)
                {
                    if (value != null)
                    {
                        value.CopyTo(newValues, length);
                        length += value.Length;
                    }
                }
                return newValues;
            }
            return NullValue<valueType>.Array;
        }
        /// <summary>
        /// 连接数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组集合</param>
        /// <returns>连接后的数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] concat<valueType>(params valueType[][] array)
        {
            return array.getArray();
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">待排序数组</param>
        /// <param name="comparer">比较器</param>
        /// <returns>排序后的数组</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] sort<valueType>(this valueType[] array, Func<valueType, valueType, int> comparer)
        {
            Algorithm.QuickSort.Sort(array, comparer);
            return array.notNull();
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数据集合</param>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接字符</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string joinString<valueType>(this valueType[] array, char join, Func<valueType, string> toString)
        {
            return JoinString(array.getArray(toString), join);
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="array">字符串集合</param>
        /// <param name="join">字符连接</param>
        /// <returns>连接后的字符串</returns>
        internal unsafe static string JoinString(string[] array, char join)
        {
            if (array.Length == 0) return string.Empty;
            int length = 0;
            foreach (string nextString in array)
            {
                if (nextString != null) length += nextString.Length;
            }
            string value = AutoCSer.Extension.StringExtension.FastAllocateString(length + array.Length - 1);
            fixed (char* valueFixed = value)
            {
                char* write = valueFixed;
                foreach (string nextString in array)
                {
                    if (write != valueFixed) *write++ = join;
                    if (nextString != null)
                    {
                        StringExtension.CopyNotNull(nextString, write);
                        write += nextString.Length;
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>匹配数组</returns>
        public unsafe static valueType[] getFindArray<valueType>(this valueType[] array, Func<valueType, bool> isValue)
        {
            int length = array.length();
            if (length == 0) return NullValue<valueType>.Array;
            UnmanagedPool pool = AutoCSer.UnmanagedPool.GetDefaultPool(length = ((length + 63) >> 6) << 3);
            Pointer.Size buffer = pool.GetSize64(length);
            try
            {
                Memory.ClearUnsafe(buffer.ULong, length >> 3);
                return getFindArray(array, isValue, new MemoryMap(buffer.Data));
            }
            finally { pool.PushOnly(ref buffer); }
        }
        /// <summary>
        /// 获取匹配数组
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="array">数组数据</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="map">匹配结果位图</param>
        /// <returns>匹配数组</returns>
        private static valueType[] getFindArray<valueType>(valueType[] array, Func<valueType, bool> isValue, MemoryMap map)
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
            if (length == 0) return NullValue<valueType>.Array;
            valueType[] newValues = new valueType[length];
            for (int index = array.Length; length != 0;)
            {
                if (map.Get(--index) != 0) newValues[--length] = array[index];
            }
            return newValues;
        }
    }
}
