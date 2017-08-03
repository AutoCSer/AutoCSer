using System;
/*Type:ulong,ULongRangeSorter,ULongSortIndex,ULongRangeIndexSorter;long,LongRangeSorter,LongSortIndex,LongRangeIndexSorter;uint,UIntRangeSorter,UIntSortIndex,UIntRangeIndexSorter;int,IntRangeSorter,IntSortIndex,IntRangeIndexSorter;double,DoubleRangeSorter,DoubleSortIndex,DoubleRangeIndexSorter;float,FloatRangeSorter,FloatSortIndex,FloatRangeIndexSorter;DateTime,DateTimeRangeSorter,DateTimeSortIndex,DateTimeRangeIndexSorter*/
/*Compare:,<,<=;Desc,>,>=*/

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
        public static LeftArray</*Type[0]*/ulong/*Type[0]*/> GetTop/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] values, int count)
        {
            if (values == null || count <= 0) return default(LeftArray</*Type[0]*/ulong/*Type[0]*/>);
            if (count < values.Length)
            {
                if (count <= values.Length >> 1) return getTop/*Compare[0]*//*Compare[0]*/(values, count);
                values = getRemoveTop/*Compare[0]*//*Compare[0]*/(values, count);
            }
            else
            {
                /*Type[0]*/
                ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[values.Length];
                Array.Copy(values, 0, newValues, 0, values.Length);
                values = newValues;
            }
            return new LeftArray</*Type[0]*/ulong/*Type[0]*/> { Array = values, Length = values.Length };
        }
        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray</*Type[0]*/ulong/*Type[0]*/> Top/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] values, int count)
        {
            if (values == null || count <= 0) return default(LeftArray</*Type[0]*/ulong/*Type[0]*/>);
            if (count < values.Length)
            {
                if (count <= values.Length >> 1) return getTop/*Compare[0]*//*Compare[0]*/(values, count);
                values = getRemoveTop/*Compare[0]*//*Compare[0]*/(values, count);
            }
            return new LeftArray</*Type[0]*/ulong/*Type[0]*/> { Array = values, Length = values.Length };
        }
        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        private static LeftArray</*Type[0]*/ulong/*Type[0]*/> getTop/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] values, int count)
        {
            uint sqrtMod;
            int length = Math.Min(Math.Max(count << 2, count + (int)AutoCSer.Extension.Number.sqrt((uint)values.Length, out sqrtMod)), values.Length);
            /*Type[0]*/
            ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[length];
            fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues, valueFixed = values)
            {
                /*Type[0]*/
                ulong/*Type[0]*/* readIndex = valueFixed + values.Length - length;
                Buffer.BlockCopy(values, (int)((byte*)readIndex - (byte*)valueFixed), newValues, 0, length * sizeof(/*Type[0]*/ulong/*Type[0]*/));
                //unsafer.memory.Copy(readIndex, newValueFixed, length * sizeof(/*Type[0]*/ulong/*Type[0]*/));
                /*Type[0]*/
                ulong/*Type[0]*/* writeStat = newValueFixed + count, writeEnd = newValueFixed + --length, writeIndex = writeStat;
                FixedArrayQuickRangeSort./*Type[1]*/ULongRangeSorter/*Type[1]*//*Compare[0]*//*Compare[0]*/ sort = new FixedArrayQuickRangeSort./*Type[1]*/ULongRangeSorter/*Type[1]*//*Compare[0]*//*Compare[0]*/ { SkipCount = writeStat - 1, GetEndIndex = writeStat - 1 };
                sort.Sort(newValueFixed, writeEnd);
                for (/*Type[0]*/ulong/*Type[0]*/ maxValue = *sort.SkipCount; readIndex != valueFixed; )
                {
                    if (*--readIndex /*Compare[1]*/</*Compare[1]*/ maxValue)
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
            return new LeftArray</*Type[0]*/ulong/*Type[0]*/> { Array = newValues, Length = count };
        }
        /// <summary>
        /// 排序去除Top N
        /// </summary>
        /// <param name="values">待排序数组</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        private static /*Type[0]*/ulong/*Type[0]*/[] getRemoveTop/*Compare[0]*//*Compare[0]*/(/*Type[0]*/ulong/*Type[0]*/[] values, int count)
        {
            /*Type[0]*/
            ulong/*Type[0]*/[] newValues = new /*Type[0]*/ulong/*Type[0]*/[count];
            count = values.Length - count;
            uint sqrtMod;
            int length = Math.Min(Math.Max(count << 2, count + (int)AutoCSer.Extension.Number.sqrt((uint)values.Length, out sqrtMod)), values.Length);
            /*Type[0]*/
            ulong/*Type[0]*/[] removeValues = new /*Type[0]*/ulong/*Type[0]*/[length];
            fixed (/*Type[0]*/ulong/*Type[0]*/* newValueFixed = newValues, removeFixed = removeValues, valueFixed = values)
            {
                int copyCount = length - count, copyLength = copyCount * sizeof(/*Type[0]*/ulong/*Type[0]*/);
                /*Type[0]*/
                ulong/*Type[0]*/* readIndex = valueFixed + values.Length - length, removeStart = removeFixed + copyCount;
                Buffer.BlockCopy(values, (int)((byte*)readIndex - (byte*)valueFixed), removeValues, 0, length * sizeof(/*Type[0]*/ulong/*Type[0]*/));
                //unsafer.memory.Copy(readIndex, removeFixed, length * sizeof(/*Type[0]*/ulong/*Type[0]*/));
                /*Type[0]*/
                ulong/*Type[0]*/* removeEnd = removeFixed + --length, removeIndex = removeStart, writeIndex = newValueFixed + copyCount;
                FixedArrayQuickRangeSort./*Type[1]*/ULongRangeSorter/*Type[1]*//*Compare[0]*//*Compare[0]*/ sort = new FixedArrayQuickRangeSort./*Type[1]*/ULongRangeSorter/*Type[1]*//*Compare[0]*//*Compare[0]*/ { SkipCount = removeStart, GetEndIndex = removeStart };
                sort.Sort(removeFixed, removeEnd);
                Buffer.BlockCopy(removeValues, 0, newValues, 0, copyLength);
                //unsafer.memory.Copy(removeFixed, newValueFixed, copyLength);
                for (/*Type[0]*/ulong/*Type[0]*/ maxValue = *removeStart; readIndex != valueFixed; )
                {
                    if (*--readIndex /*Compare[2]*/<=/*Compare[2]*/ maxValue) *writeIndex++ = *readIndex;
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
                    //unsafer.memory.Copy(removeIndex, writeIndex, (int)(removeStart - removeIndex) * sizeof(/*Type[0]*/ulong/*Type[0]*/));
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
        public static valueType[] GetTop/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] values, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int count)
        {
            if (values == null || count <= 0) return NullValue<valueType>.Array;
            if (count >= values.Length) return AutoCSer.Extension.ArrayExtension.copy(values);
            if (count <= values.Length >> 1) return getTop/*Compare[0]*//*Compare[0]*/(values, getKey, count);
            return getRemoveTop/*Compare[0]*//*Compare[0]*/(values, getKey, count);
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
        public static valueType[] Top/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] values, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int count)
        {
            if (values == null || count <= 0) return NullValue<valueType>.Array;
            if (count >= values.Length) return values;
            if (count <= values.Length >> 1) return getTop/*Compare[0]*//*Compare[0]*/(values, getKey, count);
            return getRemoveTop/*Compare[0]*//*Compare[0]*/(values, getKey, count);
        }
        /// <summary>
        /// 排序取Top N
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="values">待排序数组</param>
        /// <param name="getKey">排序键值获取器</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的数据</returns>
        private static valueType[] getTop/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] values, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int count)
        {
            uint sqrtMod;
            int length = Math.Min(Math.Max(count << 2, count + (int)AutoCSer.Extension.Number.sqrt((uint)values.Length, out sqrtMod)), values.Length), size = length * sizeof(/*Type[2]*/ULongSortIndex/*Type[2]*/);
            UnmanagedPool pool = AutoCSer.UnmanagedPool.GetDefaultPool(size);
            Pointer.Size data = pool.GetSize(size);
            try
            {
                return getTop/*Compare[0]*//*Compare[0]*/(values, getKey, count, length, (/*Type[2]*/ULongSortIndex/*Type[2]*/*)data.Data);
            }
            finally { pool.PushOnly(ref data); }
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
        private static valueType[] getTop/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] values, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int count, int length, /*Type[2]*/ULongSortIndex/*Type[2]*/* indexFixed)
        {
            /*Type[2]*/
            ULongSortIndex/*Type[2]*/* writeEnd = indexFixed;
            int index = 0;
            while (index != length) (*writeEnd++).Set(getKey(values[index]), index++);
            /*Type[2]*/
            ULongSortIndex/*Type[2]*/* writeStat = indexFixed + count, writeIndex = writeStat;
            FixedArrayQuickRangeSort./*Type[3]*/ULongRangeIndexSorter/*Type[3]*//*Compare[0]*//*Compare[0]*/ sort = new FixedArrayQuickRangeSort./*Type[3]*/ULongRangeIndexSorter/*Type[3]*//*Compare[0]*//*Compare[0]*/ { SkipCount = writeStat - 1, GetEndIndex = writeStat - 1 };
            sort.Sort(indexFixed, --writeEnd);
            for (/*Type[0]*/ulong/*Type[0]*/ maxValue = (*sort.SkipCount).Value; index != values.Length; ++index)
            {
                /*Type[0]*/
                ulong/*Type[0]*/ value = getKey(values[index]);
                if (value /*Compare[1]*/</*Compare[1]*/ maxValue)
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
        private static valueType[] getRemoveTop/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] values, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int count)
        {
            valueType[] newValues = new valueType[count];
            count = values.Length - count;
            uint sqrtMod;
            int length = Math.Min(Math.Max(count << 2, count + (int)AutoCSer.Extension.Number.sqrt((uint)values.Length, out sqrtMod)), values.Length), size = length * sizeof(/*Type[2]*/ULongSortIndex/*Type[2]*/);
            UnmanagedPool pool = AutoCSer.UnmanagedPool.GetDefaultPool(size);
            Pointer.Size data = pool.GetSize(size);
            try
            {
                removeTop/*Compare[0]*//*Compare[0]*/(values, getKey, count, newValues, length, (/*Type[2]*/ULongSortIndex/*Type[2]*/*)data.Data);
            }
            finally { pool.PushOnly(ref data); }
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
        private static void removeTop/*Compare[0]*//*Compare[0]*/<valueType>(valueType[] values, Func<valueType, /*Type[0]*/ulong/*Type[0]*/> getKey, int count, valueType[] newValues, int length, /*Type[2]*/ULongSortIndex/*Type[2]*/* removeFixed)
        {
            int index = 0, writeIndex = 0;
            /*Type[2]*/
            ULongSortIndex/*Type[2]*/* removeEnd = removeFixed;
            while (index != length) (*removeEnd++).Set(getKey(values[index]), index++);
            /*Type[2]*/
            ULongSortIndex/*Type[2]*/* removeStart = removeFixed + (count = length - count), removeIndex = removeFixed;
            FixedArrayQuickRangeSort./*Type[3]*/ULongRangeIndexSorter/*Type[3]*//*Compare[0]*//*Compare[0]*/ sort = new FixedArrayQuickRangeSort./*Type[3]*/ULongRangeIndexSorter/*Type[3]*//*Compare[0]*//*Compare[0]*/ { SkipCount = removeStart, GetEndIndex = removeStart };
            sort.Sort(removeFixed, --removeEnd);
            while (writeIndex != count) newValues[writeIndex++] = values[(*removeIndex++).Index];
            for (/*Type[0]*/ulong/*Type[0]*/ maxValue = (*removeStart).Value; index != values.Length; ++index)
            {
                /*Type[0]*/
                ulong/*Type[0]*/ value = getKey(values[index]);
                if (value /*Compare[2]*/<=/*Compare[2]*/ maxValue) newValues[writeIndex++] = values[index];
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
