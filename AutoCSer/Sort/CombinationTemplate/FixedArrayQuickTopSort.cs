using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;double,Double;float,Float;DateTime,DateTime
Desc,CompareTo;,CompareFrom*/

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 快速排序
    /// </summary>
    internal static unsafe partial class FixedArrayQuickTopSort
    {
        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        public static LeftArray<ulong> GetTopDesc(ulong[] values, int count)
        {
            if (values == null || count <= 0) return new LeftArray<ulong>(0);
            if (count < values.Length)
            {
                if (count <= values.Length >> 1) return getTopDesc(values, count);
                values = getRemoveTopDesc(values, count);
            }
            else
            {
                ulong[] newValues = new ulong[values.Length];
                Array.Copy(values, 0, newValues, 0, values.Length);
                values = newValues;
            }
            return new LeftArray<ulong> { Array = values, Length = values.Length };
        }
        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray<ulong> TopDesc(ulong[] values, int count)
        {
            if (values == null || count <= 0) return new LeftArray<ulong>(0);
            if (count < values.Length)
            {
                if (count <= values.Length >> 1) return getTopDesc(values, count);
                values = getRemoveTopDesc(values, count);
            }
            return new LeftArray<ulong> { Array = values, Length = values.Length };
        }
        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        private static LeftArray<ulong> getTopDesc(ulong[] values, int count)
        {
            uint sqrtMod;
            int length = Math.Min(Math.Max(count << 2, count + (int)AutoCSer.Extensions.NumberExtension.sqrt((uint)values.Length, out sqrtMod)), values.Length);
            ulong[] newValues = new ulong[length];
            fixed (ulong* newValueFixed = newValues, valueFixed = values)
            {
                ulong* readIndex = valueFixed + values.Length - length;
                Buffer.BlockCopy(values, (int)((byte*)readIndex - (byte*)valueFixed), newValues, 0, length * sizeof(ulong));
                //unsafer.memory.Copy(readIndex, newValueFixed, length * sizeof(ulong));
                ulong* writeStat = newValueFixed + count, writeEnd = newValueFixed + --length, writeIndex = writeStat;
                FixedArrayQuickRangeSort.ULongRangeSorterDesc sort = new FixedArrayQuickRangeSort.ULongRangeSorterDesc { SkipCount = writeStat - 1, GetEndIndex = writeStat - 1 };
                sort.Sort(newValueFixed, writeEnd);
                for (ulong maxValue = *sort.SkipCount; readIndex != valueFixed; )
                {
                    if ((*--readIndex).CompareTo(maxValue) > 0)
                    {
                        *writeIndex = *readIndex;
                        if (writeIndex == writeEnd)
                        {
                            sort.Sort(newValueFixed, writeEnd);
                            writeIndex = writeStat;
                            maxValue = *sort.SkipCount;
                        }
                        else ++writeIndex;
                    }
                }
                if (writeIndex != writeStat) sort.Sort(newValueFixed, --writeIndex);
            }
            return new LeftArray<ulong> { Array = newValues, Length = count };
        }
        /// <summary>
        /// 排序去除Top N
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        private static ulong[] getRemoveTopDesc(ulong[] values, int count)
        {
            ulong[] newValues = new ulong[count];
            count = values.Length - count;
            uint sqrtMod;
            int length = Math.Min(Math.Max(count << 2, count + (int)AutoCSer.Extensions.NumberExtension.sqrt((uint)values.Length, out sqrtMod)), values.Length);
            ulong[] removeValues = new ulong[length];
            fixed (ulong* newValueFixed = newValues, removeFixed = removeValues, valueFixed = values)
            {
                int copyCount = length - count, copyLength = copyCount * sizeof(ulong);
                
                ulong* readIndex = valueFixed + values.Length - length, removeStart = removeFixed + copyCount;
                Buffer.BlockCopy(values, (int)((byte*)readIndex - (byte*)valueFixed), removeValues, 0, length * sizeof(ulong));
                //unsafer.memory.Copy(readIndex, removeFixed, length * sizeof(ulong));
                ulong* removeEnd = removeFixed + --length, removeIndex = removeStart, writeIndex = newValueFixed + copyCount;
                FixedArrayQuickRangeSort.ULongRangeSorterDesc sort = new FixedArrayQuickRangeSort.ULongRangeSorterDesc { SkipCount = removeStart, GetEndIndex = removeStart };
                sort.Sort(removeFixed, removeEnd);
                Buffer.BlockCopy(removeValues, 0, newValues, 0, copyLength);
                //unsafer.memory.Copy(removeFixed, newValueFixed, copyLength);
                for (ulong maxValue = *removeStart; readIndex != valueFixed; )
                {
                    if ((*--readIndex).CompareTo(maxValue) >= 0) *writeIndex++ = *readIndex;
                    else
                    {
                        *--removeIndex = *readIndex;
                        if (removeIndex == removeFixed)
                        {
                            sort.Sort(removeFixed, removeEnd);
                            removeIndex = removeStart;
                            maxValue = *removeStart;
                            Buffer.BlockCopy(removeValues, 0, newValues, (int)((byte*)writeIndex - (byte*)newValueFixed), copyLength);
                            //unsafer.memory.Copy(removeFixed, writeIndex, copyLength);
                            writeIndex += copyCount;
                        }
                    }
                }
                if (removeIndex != removeStart)
                {
                    sort.Sort(removeIndex, removeEnd);
                    Buffer.BlockCopy(removeValues, (int)((byte*)removeIndex - (byte*)removeFixed), newValues, (int)((byte*)writeIndex - (byte*)newValueFixed), (int)((byte*)removeStart - (byte*)removeIndex));
                    //unsafer.memory.Copy(removeIndex, writeIndex, (int)(removeStart - removeIndex) * sizeof(ulong));
                }
            }
            return newValues;
        }
        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] GetTopDesc<valueType>(valueType[] values, Func<valueType, ulong> getKey, int count)
        {
            if (values == null || count <= 0) return EmptyArray<valueType>.Array;
            if (count >= values.Length) return AutoCSer.Extensions.ArrayExtension.copy(values);
            if (count <= values.Length >> 1) return getTopDesc(values, getKey, count);
            return getRemoveTopDesc(values, getKey, count);
        }
        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType[] TopDesc<valueType>(valueType[] values, Func<valueType, ulong> getKey, int count)
        {
            if (values == null || count <= 0) return EmptyArray<valueType>.Array;
            if (count >= values.Length) return values;
            if (count <= values.Length >> 1) return getTopDesc(values, getKey, count);
            return getRemoveTopDesc(values, getKey, count);
        }
        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        private static valueType[] getTopDesc<valueType>(valueType[] values, Func<valueType, ulong> getKey, int count)
        {
            uint sqrtMod;
            int length = Math.Min(Math.Max(count << 2, count + (int)AutoCSer.Extensions.NumberExtension.sqrt((uint)values.Length, out sqrtMod)), values.Length), size = length * sizeof(ULongSortIndex);
            AutoCSer.Memory.UnmanagedPool pool = AutoCSer.Memory.UnmanagedPool.GetPool(size);
            AutoCSer.Memory.Pointer data = pool.GetMinSize(size);
            try
            {
                return getTopDesc(values, getKey, count, length, (ULongSortIndex*)data.Data);
            }
            finally { pool.Push(ref data); }
        }
        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="count">排序数据数量</param>
        /// <param name="length">排序缓存区尺寸</param>
        /// <param name="indexFixed">索引位置</param>
        /// <returns>排序后的数据</returns>
        private static valueType[] getTopDesc<valueType>(valueType[] values, Func<valueType, ulong> getKey, int count, int length, ULongSortIndex* indexFixed)
        {
            ULongSortIndex* writeEnd = indexFixed;
            int index = 0;
            while (index != length) (*writeEnd++).Set(getKey(values[index]), index++);
            ULongSortIndex* writeStat = indexFixed + count, writeIndex = writeStat;
            FixedArrayQuickRangeSort.ULongRangeIndexSorterDesc sort = new FixedArrayQuickRangeSort.ULongRangeIndexSorterDesc { SkipCount = writeStat - 1, GetEndIndex = writeStat - 1 };
            sort.Sort(indexFixed, --writeEnd);
            for (ulong maxValue = (*sort.SkipCount).Value; index != values.Length; ++index)
            {
                ulong value = getKey(values[index]);
                if (value.CompareTo(maxValue) > 0)
                {
                    (*writeIndex).Set(value, index);
                    if (writeIndex == writeEnd)
                    {
                        sort.Sort(indexFixed, writeEnd);
                        writeIndex = writeStat;
                        maxValue = (*sort.SkipCount).Value;
                    }
                    else ++writeIndex;
                }
            }
            if (writeIndex != writeStat) sort.Sort(indexFixed, --writeIndex);
            valueType[] newValues = new valueType[count];
            for (writeIndex = indexFixed, index = 0; index != count; ++index) newValues[index] = values[(*writeIndex++).Index];
            return newValues;
        }
        /// <summary>
        /// 排序去除Top N
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        private static valueType[] getRemoveTopDesc<valueType>(valueType[] values, Func<valueType, ulong> getKey, int count)
        {
            valueType[] newValues = new valueType[count];
            count = values.Length - count;
            uint sqrtMod;
            int length = Math.Min(Math.Max(count << 2, count + (int)AutoCSer.Extensions.NumberExtension.sqrt((uint)values.Length, out sqrtMod)), values.Length), size = length * sizeof(ULongSortIndex);
            AutoCSer.Memory.UnmanagedPool pool = AutoCSer.Memory.UnmanagedPool.GetPool(size);
            AutoCSer.Memory.Pointer data = pool.GetMinSize(size);
            try
            {
                removeTopDesc(values, getKey, count, newValues, length, (ULongSortIndex*)data.Data);
            }
            finally { pool.Push(ref data); }
            return newValues;
        }
        /// <summary>
        /// 排序去除Top N
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="count">排序数据数量</param>
        /// <param name="newValues">目标数据数组</param>
        /// <param name="length">排序缓存区尺寸</param>
        /// <param name="removeFixed">索引位置</param>
        private static void removeTopDesc<valueType>(valueType[] values, Func<valueType, ulong> getKey, int count, valueType[] newValues, int length, ULongSortIndex* removeFixed)
        {
            int index = 0, writeIndex = 0;
            ULongSortIndex* removeEnd = removeFixed;
            while (index != length) (*removeEnd++).Set(getKey(values[index]), index++);
            ULongSortIndex* removeStart = removeFixed + (count = length - count), removeIndex = removeFixed;
            FixedArrayQuickRangeSort.ULongRangeIndexSorterDesc sort = new FixedArrayQuickRangeSort.ULongRangeIndexSorterDesc { SkipCount = removeStart, GetEndIndex = removeStart };
            sort.Sort(removeFixed, --removeEnd);
            while (writeIndex != count) newValues[writeIndex++] = values[(*removeIndex++).Index];
            for (ulong maxValue = (*removeStart).Value; index != values.Length; ++index)
            {
                ulong value = getKey(values[index]);
                if (value.CompareTo(maxValue) >= 0) newValues[writeIndex++] = values[index];
                else
                {
                    (*--removeIndex).Set(value, index);
                    if (removeIndex == removeFixed)
                    {
                        sort.Sort(removeFixed, removeEnd);
                        for (removeIndex = removeFixed; removeIndex != removeStart; newValues[writeIndex++] = values[(*removeIndex++).Index]) ;
                        maxValue = (*removeStart).Value;
                    }
                }
            }
            if (removeIndex != removeStart)
            {
                sort.Sort(removeIndex, removeEnd);
                while (removeIndex != removeStart) newValues[writeIndex++] = values[(*removeIndex++).Index];
            }
        }
    }
}
