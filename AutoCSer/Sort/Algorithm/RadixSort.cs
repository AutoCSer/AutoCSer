using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 基数排序
    /// </summary>
    internal static class RadixSort
    {
        /// <summary>
        /// 基数排序数据量
        /// </summary>
        internal const int SortSize = 1 << 9;
        /// <summary>
        /// 64位基数排序数据量
        /// </summary>
        internal const int SortSize64 = 4 << 9;
        /// <summary>
        /// 计数缓冲区字节大小
        /// </summary>
        private const int countBufferSize = 256 * 4 * sizeof(int);
        /// <summary>
        /// 计数缓冲区
        /// </summary>
        private static readonly UnmanagedPool countBuffer = UnmanagedPool.GetOrCreate(countBufferSize);
        /// <summary>
        /// 计数缓冲区字节大小
        /// </summary>
        private const int countBufferSize64 = 256 * 8 * sizeof(int);
        /// <summary>
        /// 计数缓冲区
        /// </summary>
        private static readonly UnmanagedPool countBuffer64 = UnmanagedPool.GetOrCreate(countBufferSize64);
        /// <summary>
        /// 获取非托管内存池
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static UnmanagedPool GetUnmanagedPool(int size)
        {
            return size <= UnmanagedPool.DefaultSize ? (size <= UnmanagedPool.TinySize ? UnmanagedPool.Tiny : UnmanagedPool.Default) : countBuffer64;
        }

        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="newArrayFixed">目标数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        private static unsafe void sort(uint* arrayFixed, uint* newArrayFixed, uint* swapFixed, int length)
        {
            byte* countFixed = countBuffer.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
            for (uint* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                byte value = *((byte*)start + 3);
                ++count24[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            *count24 = 0;
            index = *(count24 + 1);
            for (int* start = count24 + 2, end = count24 + 256; start != end; *start++ = index) index += *start;
            *--count0 = 0;
            for (uint* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++] = *start;
            }
            *--count8 = 0;
            for (uint* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count8[*((byte*)start + 1)]++] = *start;
            }
            *--count16 = 0;
            for (uint* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++] = *start;
            }
            for (uint* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count24[*((byte*)start + 3)]++] = *start;
            }
            countBuffer.Push(countFixed);
        }
        /// <summary>
        /// 索引数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        internal static unsafe void Sort(UIntSortIndex* arrayFixed, UIntSortIndex* swapFixed, int length)
        {
            byte* countFixed = countBuffer.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
            for (UIntSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                byte value = *((byte*)start + 3);
                ++count24[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            *count24 = 0;
            index = *(count24 + 1);
            for (int* start = count24 + 2, end = count24 + 256; start != end; *start++ = index) index += *start;
            *--count0 = 0;
            for (UIntSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++].Set((*start).Value, (*start).Index);
            }
            *--count8 = 0;
            for (UIntSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count8[*((byte*)start + 1)]++].Set((*start).Value, (*start).Index);
            }
            *--count16 = 0;
            for (UIntSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++].Set((*start).Value, (*start).Index);
            }
            for (UIntSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count24[*((byte*)start + 3)]++].Set((*start).Value, (*start).Index);
            }
            countBuffer.Push(countFixed);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        internal static unsafe void Sort(uint[] array, int startIndex, int count)
        {
            int size = count * sizeof(uint);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize(size);
            try
            {
                fixed (uint* arrayFixed = array)
                {
                    uint* start = arrayFixed + startIndex;
                    sort(start, start, swap.UInt, count);
                }
            }
            finally { pool.PushOnly(ref swap); }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的新数组</returns>
        internal static unsafe uint[] GetSort(uint[] array, int startIndex, int count)
        {
            uint[] newArray = new uint[count];
            int size = count * sizeof(uint);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize(size);
            try
            {
                fixed (uint* newArrayFixed = newArray, arrayFixed = array) sort(arrayFixed + startIndex, newArrayFixed, swap.UInt, count);
            }
            finally { pool.PushOnly(ref swap); }
            return newArray;
        }

        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="newArrayFixed">目标数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        private static unsafe void sortDesc(uint* arrayFixed, uint* newArrayFixed, uint* swapFixed, int length)
        {
            byte* countFixed = countBuffer.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
            for (uint* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                byte value = *((byte*)start + 3);
                ++count24[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            *count24 = length;
            index = 0;
            for (int* start = count24 + 1, end = count24 + 256; start != end; *start++ = length - index) index += *start;
            *--count0 = 0;
            for (uint* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++] = *start;
            }
            *--count8 = 0;
            for (uint* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count8[*((byte*)start + 1)]++] = *start;
            }
            *--count16 = 0;
            for (uint* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++] = *start;
            }
            for (uint* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[--count24[*((byte*)start + 3)]] = *start;
            }
            countBuffer.Push(countFixed);
        }
        /// <summary>
        /// 索引数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        internal static unsafe void SortDesc(UIntSortIndex* arrayFixed, UIntSortIndex* swapFixed, int length)
        {
            byte* countFixed = countBuffer.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
            for (UIntSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                byte value = *((byte*)start + 3);
                ++count24[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            *count24 = length;
            index = 0;
            for (int* start = count24 + 1, end = count24 + 256; start != end; *start++ = length - index) index += *start;
            *--count0 = 0;
            for (UIntSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++].Set((*start).Value, (*start).Index);
            }
            *--count8 = 0;
            for (UIntSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count8[*((byte*)start + 1)]++].Set((*start).Value, (*start).Index);
            }
            *--count16 = 0;
            for (UIntSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++].Set((*start).Value, (*start).Index);
            }
            for (UIntSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[--count24[*((byte*)start + 3)]].Set((*start).Value, (*start).Index);
            }
            countBuffer.Push(countFixed);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        internal static unsafe void SortDesc(uint[] array, int startIndex, int count)
        {
            int size = count * sizeof(uint);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize(size);
            try
            {
                fixed (uint* arrayFixed = array)
                {
                    uint* start = arrayFixed + startIndex;
                    sortDesc(start, start, swap.UInt, count);
                }
            }
            finally { pool.PushOnly(ref swap); }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的新数组</returns>
        internal static unsafe uint[] GetSortDesc(uint[] array, int startIndex, int count)
        {
            uint[] newArray = new uint[count];
            int size = count * sizeof(uint);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize(size);
            try
            {
                fixed (uint* newArrayFixed = newArray, arrayFixed = array) sortDesc(arrayFixed + startIndex, newArrayFixed, swap.UInt, count);
            }
            finally { pool.PushOnly(ref swap); }
            return newArray;
        }
        
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void Sort(IntSortIndex* arrayFixed, IntSortIndex* swapFixed, int length)
        {
            Sort(arrayFixed, (UIntSortIndex*)swapFixed, length);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="newArrayFixed">目标数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        private static unsafe void sort(int* arrayFixed, int* newArrayFixed, uint* swapFixed, int length)
        {
            byte* countFixed = countBuffer.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
            for (int* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                byte value = *((byte*)start + 3);
                value ^= 0x80;
                ++count24[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            *count24 = 0;
            index = *(count24 + 1);
            for (int* start = count24 + 2, end = count24 + 256; start != end; *start++ = index) index += *start;
            *--count0 = 0;
            for (int* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++] = (uint)*start ^ 0x80000000U;
            }
            *--count8 = 0;
            for (uint* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count8[*((byte*)start + 1)]++] = (int)*start;
            }
            *--count16 = 0;
            for (int* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++] = (uint)*start;
            }
            for (uint* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count24[*((byte*)start + 3)]++] = (int)(*start ^ 0x80000000U);
            }
            countBuffer.Push(countFixed);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        internal static unsafe void Sort(IntSortIndex* arrayFixed, UIntSortIndex* swapFixed, int length)
        {
            byte* countFixed = countBuffer.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
            for (IntSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                byte value = *((byte*)start + 3);
                value ^= 0x80;
                ++count24[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            *count24 = 0;
            index = *(count24 + 1);
            for (int* start = count24 + 2, end = count24 + 256; start != end; *start++ = index) index += *start;
            *--count0 = 0;
            for (IntSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++].Set((uint)(*start).Value ^ 0x80000000U, (*start).Index);
            }
            *--count8 = 0;
            for (UIntSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count8[*((byte*)start + 1)]++].Set((int)(*start).Value, (*start).Index);
            }
            *--count16 = 0;
            for (IntSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++].Set((uint)(*start).Value, (*start).Index);
            }
            for (UIntSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count24[*((byte*)start + 3)]++].Set((int)((*start).Value ^ 0x80000000U), (*start).Index);
            }
            countBuffer.Push(countFixed);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void SortDesc(IntSortIndex* arrayFixed, IntSortIndex* swapFixed, int length)
        {
            SortDesc(arrayFixed, (UIntSortIndex*)swapFixed, length);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        public static unsafe void Sort(int[] array, int startIndex, int count)
        {
            int size = count * sizeof(uint);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize(size);
            try
            {
                fixed (int* arrayFixed = array)
                {
                    int* start = arrayFixed + startIndex;
                    sort(start, start, swap.UInt, count);
                }
            }
            finally { pool.PushOnly(ref swap); }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的新数组</returns>
        public static unsafe int[] GetSort(int[] array, int startIndex, int count)
        {
            int[] newArray = new int[count];
            int size = count * sizeof(uint);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize(size);
            try
            {
                fixed (int* newArrayFixed = newArray, arrayFixed = array) sort(arrayFixed + startIndex, newArrayFixed, swap.UInt, count);
            }
            finally { pool.PushOnly(ref swap); }
            return newArray;
        }

        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="newArrayFixed">目标数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        private static unsafe void sortDesc(int* arrayFixed, int* newArrayFixed, uint* swapFixed, int length)
        {
            byte* countFixed = countBuffer.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
            for (int* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                byte value = *((byte*)start + 3);
                value ^= 0x80;
                ++count24[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            *count24 = length;
            index = 0;
            for (int* start = count24 + 1, end = count24 + 256; start != end; *start++ = length - index) index += *start;
            *--count0 = 0;
            for (int* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++] = (uint)*start ^ 0x80000000U;
            }
            *--count8 = 0;
            for (uint* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count8[*((byte*)start + 1)]++] = (int)*start;
            }
            *--count16 = 0;
            for (int* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++] = (uint)*start;
            }
            for (uint* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[--count24[*((byte*)start + 3)]] = (int)(*start ^ 0x80000000U);
            }
            countBuffer.Push(countFixed);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        internal static unsafe void SortDesc(IntSortIndex* arrayFixed, UIntSortIndex* swapFixed, int length)
        {
            byte* countFixed = countBuffer.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + (256 * 3 - 1);
            for (IntSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                byte value = *((byte*)start + 3);
                value ^= 0x80;
                ++count24[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            *count24 = length;
            index = 0;
            for (int* start = count24 + 1, end = count24 + 256; start != end; *start++ = length - index) index += *start;
            *--count0 = 0;
            for (IntSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++].Set((uint)(*start).Value ^ 0x80000000U, (*start).Index);
            }
            *--count8 = 0;
            for (UIntSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count8[*((byte*)start + 1)]++].Set((int)(*start).Value, (*start).Index);
            }
            *--count16 = 0;
            for (IntSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++].Set((uint)(*start).Value, (*start).Index);
            }
            for (UIntSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[--count24[*((byte*)start + 3)]].Set((int)((*start).Value ^ 0x80000000U), (*start).Index);
            }
            countBuffer.Push(countFixed);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        public static unsafe void SortDesc(int[] array, int startIndex, int count)
        {
            int size = count * sizeof(uint);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize(size);
            try
            {
                fixed (int* arrayFixed = array)
                {
                    int* start = arrayFixed + startIndex;
                    sortDesc(start, start, swap.UInt, count);
                }
            }
            finally { pool.PushOnly(ref swap); }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的新数组</returns>
        public static unsafe int[] GetSortDesc(int[] array, int startIndex, int count)
        {
            int[] newArray = new int[count];
            int size = count * sizeof(uint);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize(size);
            try
            {
                fixed (int* newArrayFixed = newArray, arrayFixed = array) sortDesc(arrayFixed + startIndex, newArrayFixed, swap.UInt, count);
            }
            finally { pool.PushOnly(ref swap); }
            return newArray;
        }

        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="newArrayFixed">目标数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        private static unsafe void sort(ulong* arrayFixed, ulong* newArrayFixed, ulong* swapFixed, int length)
        {
            byte* countFixed = countBuffer64.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize64 >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
            int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
            for (ulong* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                ++count24[*((byte*)start + 3)];
                ++count32[*((byte*)start + 4)];
                ++count40[*((byte*)start + 5)];
                ++count48[*((byte*)start + 6)];
                byte value = *((byte*)start + 7);
                ++count56[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            index = *count24;
            for (int* start = count24 + 1, end = count24 + 255; start != end; *start++ = index) index += *start;
            index = *count32;
            for (int* start = count32 + 1, end = count32 + 255; start != end; *start++ = index) index += *start;
            index = *count40;
            for (int* start = count40 + 1, end = count40 + 255; start != end; *start++ = index) index += *start;
            index = *count48;
            for (int* start = count48 + 1, end = count48 + 255; start != end; *start++ = index) index += *start;
            *count56 = 0;
            index = *(count56 + 1);
            for (int* start = count56 + 2, end = count56 + 256; start != end; *start++ = index) index += *start;
            *--count0 = 0;
            for (ulong* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++] = *start;
            }
            *--count8 = 0;
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count8[*((byte*)start + 1)]++] = *start;
            }
            *--count16 = 0;
            for (ulong* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++] = *start;
            }
            *--count24 = 0;
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count24[*((byte*)start + 3)]++] = *start;
            }
            *--count32 = 0;
            for (ulong* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count32[*((byte*)start + 4)]++] = *start;
            }
            *--count40 = 0;
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count40[*((byte*)start + 5)]++] = *start;
            }
            *--count48 = 0;
            for (ulong* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count48[*((byte*)start + 6)]++] = *start;
            }
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count56[*((byte*)start + 7)]++] = *start;
            }
            countBuffer64.Push(countFixed);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        internal static unsafe void Sort(ULongSortIndex* arrayFixed, ULongSortIndex* swapFixed, int length)
        {
            byte* countFixed = countBuffer64.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize64 >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
            int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
            for (ULongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                ++count24[*((byte*)start + 3)];
                ++count32[*((byte*)start + 4)];
                ++count40[*((byte*)start + 5)];
                ++count48[*((byte*)start + 6)];
                byte value = *((byte*)start + 7);
                ++count56[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            index = *count24;
            for (int* start = count24 + 1, end = count24 + 255; start != end; *start++ = index) index += *start;
            index = *count32;
            for (int* start = count32 + 1, end = count32 + 255; start != end; *start++ = index) index += *start;
            index = *count40;
            for (int* start = count40 + 1, end = count40 + 255; start != end; *start++ = index) index += *start;
            index = *count48;
            for (int* start = count48 + 1, end = count48 + 255; start != end; *start++ = index) index += *start;
            *count56 = 0;
            index = *(count56 + 1);
            for (int* start = count56 + 2, end = count56 + 256; start != end; *start++ = index) index += *start;
            *--count0 = 0;
            for (ULongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++].Set((*start).Value, (*start).Index);
            }
            *--count8 = 0;
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count8[*((byte*)start + 1)]++].Set((*start).Value, (*start).Index);
            }
            *--count16 = 0;
            for (ULongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++].Set((*start).Value, (*start).Index);
            }
            *--count24 = 0;
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count24[*((byte*)start + 3)]++].Set((*start).Value, (*start).Index);
            }
            *--count32 = 0;
            for (ULongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count32[*((byte*)start + 4)]++].Set((*start).Value, (*start).Index);
            }
            *--count40 = 0;
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count40[*((byte*)start + 5)]++].Set((*start).Value, (*start).Index);
            }
            *--count48 = 0;
            for (ULongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count48[*((byte*)start + 6)]++].Set((*start).Value, (*start).Index);
            }
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count56[*((byte*)start + 7)]++].Set((*start).Value, (*start).Index);
            }
            countBuffer64.Push(countFixed);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        internal static unsafe void Sort(ulong[] array, int startIndex, int count)
        {
            int size = count * sizeof(ulong);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize64(size);
            try
            {
                fixed (ulong* arrayFixed = array)
                {
                    ulong* start = arrayFixed + startIndex;
                    sort(start, start, swap.ULong, count);
                }
            }
            finally { pool.PushOnly(ref swap); }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的新数组</returns>
        internal static unsafe ulong[] GetSort(ulong[] array, int startIndex, int count)
        {
            ulong[] newArray = new ulong[count];
            int size = count * sizeof(ulong);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize64(size);
            try
            {
                fixed (ulong* newArrayFixed = newArray, arrayFixed = array) sort(arrayFixed + startIndex, newArrayFixed, swap.ULong, count);
            }
            finally { pool.PushOnly(ref swap); }
            return newArray;
        }

        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="newArrayFixed">目标数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        private static unsafe void sortDesc(ulong* arrayFixed, ulong* newArrayFixed, ulong* swapFixed, int length)
        {
            byte* countFixed = countBuffer64.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize64 >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
            int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
            for (ulong* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                ++count24[*((byte*)start + 3)];
                ++count32[*((byte*)start + 4)];
                ++count40[*((byte*)start + 5)];
                ++count48[*((byte*)start + 6)];
                byte value = *((byte*)start + 7);
                ++count56[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            index = *count24;
            for (int* start = count24 + 1, end = count24 + 255; start != end; *start++ = index) index += *start;
            index = *count32;
            for (int* start = count32 + 1, end = count32 + 255; start != end; *start++ = index) index += *start;
            index = *count40;
            for (int* start = count40 + 1, end = count40 + 255; start != end; *start++ = index) index += *start;
            index = *count48;
            for (int* start = count48 + 1, end = count48 + 255; start != end; *start++ = index) index += *start;
            *count56 = length;
            index = 0;
            for (int* start = count56 + 1, end = count56 + 256; start != end; *start++ = length - index) index += *start;
            *--count0 = 0;
            for (ulong* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++] = *start;
            }
            *--count8 = 0;
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count8[*((byte*)start + 1)]++] = *start;
            }
            *--count16 = 0;
            for (ulong* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++] = *start;
            }
            *--count24 = 0;
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count24[*((byte*)start + 3)]++] = *start;
            }
            *--count32 = 0;
            for (ulong* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count32[*((byte*)start + 4)]++] = *start;
            }
            *--count40 = 0;
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count40[*((byte*)start + 5)]++] = *start;
            }
            *--count48 = 0;
            for (ulong* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count48[*((byte*)start + 6)]++] = *start;
            }
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[--count56[*((byte*)start + 7)]] = *start;
            }
            countBuffer64.Push(countFixed);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        internal static unsafe void SortDesc(ULongSortIndex* arrayFixed, ULongSortIndex* swapFixed, int length)
        {
            byte* countFixed = countBuffer64.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize64 >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
            int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
            for (ULongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                ++count24[*((byte*)start + 3)];
                ++count32[*((byte*)start + 4)];
                ++count40[*((byte*)start + 5)];
                ++count48[*((byte*)start + 6)];
                byte value = *((byte*)start + 7);
                ++count56[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            index = *count24;
            for (int* start = count24 + 1, end = count24 + 255; start != end; *start++ = index) index += *start;
            index = *count32;
            for (int* start = count32 + 1, end = count32 + 255; start != end; *start++ = index) index += *start;
            index = *count40;
            for (int* start = count40 + 1, end = count40 + 255; start != end; *start++ = index) index += *start;
            index = *count48;
            for (int* start = count48 + 1, end = count48 + 255; start != end; *start++ = index) index += *start;
            *count56 = length;
            index = 0;
            for (int* start = count56 + 1, end = count56 + 256; start != end; *start++ = length - index) index += *start;
            *--count0 = 0;
            for (ULongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++].Set((*start).Value, (*start).Index);
            }
            *--count8 = 0;
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count8[*((byte*)start + 1)]++].Set((*start).Value, (*start).Index);
            }
            *--count16 = 0;
            for (ULongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++].Set((*start).Value, (*start).Index);
            }
            *--count24 = 0;
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count24[*((byte*)start + 3)]++].Set((*start).Value, (*start).Index);
            }
            *--count32 = 0;
            for (ULongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count32[*((byte*)start + 4)]++].Set((*start).Value, (*start).Index);
            }
            *--count40 = 0;
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count40[*((byte*)start + 5)]++].Set((*start).Value, (*start).Index);
            }
            *--count48 = 0;
            for (ULongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count48[*((byte*)start + 6)]++].Set((*start).Value, (*start).Index);
            }
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[--count56[*((byte*)start + 7)]].Set((*start).Value, (*start).Index);
            }
            countBuffer64.Push(countFixed);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        internal static unsafe void SortDesc(ulong[] array, int startIndex, int count)
        {
            int size = count * sizeof(ulong);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize64(size);
            try
            {
                fixed (ulong* arrayFixed = array)
                {
                    ulong* start = arrayFixed + startIndex;
                    sortDesc(start, start, swap.ULong, count);
                }
            }
            finally { pool.PushOnly(ref swap); }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的新数组</returns>
        internal static unsafe ulong[] GetSortDesc(ulong[] array, int startIndex, int count)
        {
            ulong[] newArray = new ulong[count];
            int size = count * sizeof(ulong);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize64(size);
            try
            {
                fixed (ulong* newArrayFixed = newArray, arrayFixed = array) sortDesc(arrayFixed + startIndex, newArrayFixed, swap.ULong, count);
            }
            finally { pool.PushOnly(ref swap); }
            return newArray;
        }
        
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void Sort(LongSortIndex* arrayFixed, LongSortIndex* swapFixed, int length)
        {
            Sort(arrayFixed, (ULongSortIndex*)swapFixed, length);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="newArrayFixed">目标数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        private static unsafe void sort(long* arrayFixed, long* newArrayFixed, ulong* swapFixed, int length)
        {
            byte* countFixed = countBuffer64.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize64 >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
            int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
            for (long* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                ++count24[*((byte*)start + 3)];
                ++count32[*((byte*)start + 4)];
                ++count40[*((byte*)start + 5)];
                ++count48[*((byte*)start + 6)];
                byte value = *((byte*)start + 7);
                value ^= 0x80;
                ++count56[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            index = *count24;
            for (int* start = count24 + 1, end = count24 + 255; start != end; *start++ = index) index += *start;
            index = *count32;
            for (int* start = count32 + 1, end = count32 + 255; start != end; *start++ = index) index += *start;
            index = *count40;
            for (int* start = count40 + 1, end = count40 + 255; start != end; *start++ = index) index += *start;
            index = *count48;
            for (int* start = count48 + 1, end = count48 + 255; start != end; *start++ = index) index += *start;
            *count56 = 0;
            index = *(count56 + 1);
            for (int* start = count56 + 2, end = count56 + 256; start != end; *start++ = index) index += *start;
            *--count0 = 0;
            for (long* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++] = (ulong)*start ^ 0x8000000000000000UL;
                //byte* low = (byte*)(swapFixed + count0[*(byte*)start]++);
                //*(uint*)low = *(uint*)start;
                //*((uint*)(low + sizeof(uint))) = *((uint*)start + 1) ^ 0x80000000U;
            }
            *--count8 = 0;
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count8[*((byte*)start + 1)]++] = (long)*start;
            }
            *--count16 = 0;
            for (long* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++] = (ulong)*start;
            }
            *--count24 = 0;
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count24[*((byte*)start + 3)]++] = (long)*start;
            }
            *--count32 = 0;
            for (long* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count32[*((byte*)start + 4)]++] = (ulong)*start;
            }
            *--count40 = 0;
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count40[*((byte*)start + 5)]++] = (long)*start;
            }
            *--count48 = 0;
            for (long* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count48[*((byte*)start + 6)]++] = (ulong)*start;
            }
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count56[*((byte*)start + 7)]++] = (long)(*start ^ 0x8000000000000000UL);
                //byte* low = (byte*)(newArrayFixed + count56[*((byte*)start + 7)]++);
                //*(uint*)low = *(uint*)start;
                //*((uint*)(low + sizeof(uint))) = *((uint*)start + 1) ^ 0x80000000U;
            }
            countBuffer64.Push(countFixed);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        internal static unsafe void Sort(LongSortIndex* arrayFixed, ULongSortIndex* swapFixed, int length)
        {
            byte* countFixed = countBuffer64.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize64 >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
            int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
            for (LongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                ++count24[*((byte*)start + 3)];
                ++count32[*((byte*)start + 4)];
                ++count40[*((byte*)start + 5)];
                ++count48[*((byte*)start + 6)];
                byte value = *((byte*)start + 7);
                value ^= 0x80;
                ++count56[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            index = *count24;
            for (int* start = count24 + 1, end = count24 + 255; start != end; *start++ = index) index += *start;
            index = *count32;
            for (int* start = count32 + 1, end = count32 + 255; start != end; *start++ = index) index += *start;
            index = *count40;
            for (int* start = count40 + 1, end = count40 + 255; start != end; *start++ = index) index += *start;
            index = *count48;
            for (int* start = count48 + 1, end = count48 + 255; start != end; *start++ = index) index += *start;
            *count56 = 0;
            index = *(count56 + 1);
            for (int* start = count56 + 2, end = count56 + 256; start != end; *start++ = index) index += *start;
            *--count0 = 0;
            for (LongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++].Set((ulong)(*start).Value ^ 0x8000000000000000UL, (*start).Index);
                //byte* low = (byte*)(swapFixed + count0[*(byte*)start]++);
                //*(uint*)low = *(uint*)start;
                //*((uint*)(low + sizeof(uint))) = *((uint*)start + 1) ^ 0x80000000U;
                //(*((ulongSortIndex*)low)).Index = (*start).Index;
            }
            *--count8 = 0;
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count8[*((byte*)start + 1)]++].Set((long)(*start).Value, (*start).Index);
            }
            *--count16 = 0;
            for (LongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++].Set((ulong)(*start).Value, (*start).Index);
            }
            *--count24 = 0;
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count24[*((byte*)start + 3)]++].Set((long)(*start).Value, (*start).Index);
            }
            *--count32 = 0;
            for (LongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count32[*((byte*)start + 4)]++].Set((ulong)(*start).Value, (*start).Index);
            }
            *--count40 = 0;
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count40[*((byte*)start + 5)]++].Set((long)(*start).Value, (*start).Index);
            }
            *--count48 = 0;
            for (LongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count48[*((byte*)start + 6)]++].Set((ulong)(*start).Value, (*start).Index);
            }
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count56[*((byte*)start + 7)]++].Set((long)((*start).Value ^ 0x8000000000000000UL), (*start).Index);
                //byte* low = (byte*)(arrayFixed + count56[*((byte*)start + 7)]++);
                //*(uint*)low = *(uint*)start;
                //*((uint*)(low + sizeof(uint))) = *((uint*)start + 1) ^ 0x80000000U;
                //(*((longSortIndex*)low)).Index = (*start).Index;
            }
            countBuffer64.Push(countFixed);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void SortDesc(LongSortIndex* arrayFixed, LongSortIndex* swapFixed, int length)
        {
            SortDesc(arrayFixed, (ULongSortIndex*)swapFixed, length);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        internal static unsafe void Sort(long[] array, int startIndex, int count)
        {
            int size = count * sizeof(ulong);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize64(size);
            try
            {
                fixed (long* arrayFixed = array)
                {
                    long* start = arrayFixed + startIndex;
                    sort(start, start, swap.ULong, count);
                }
            }
            finally { pool.PushOnly(ref swap); }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的新数组</returns>
        internal static unsafe long[] GetSort(long[] array, int startIndex, int count)
        {
            long[] newArray = new long[count];
            int size = count * sizeof(ulong);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize64(size);
            try
            {
                fixed (long* newArrayFixed = newArray, arrayFixed = array) sort(arrayFixed + startIndex, newArrayFixed, swap.ULong, count);
            }
            finally { pool.PushOnly(ref swap); }
            return newArray;
        }

        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="newArrayFixed">目标数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        private static unsafe void sortDesc(long* arrayFixed, long* newArrayFixed, ulong* swapFixed, int length)
        {
            byte* countFixed = countBuffer64.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize64 >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
            int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
            for (long* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                ++count24[*((byte*)start + 3)];
                ++count32[*((byte*)start + 4)];
                ++count40[*((byte*)start + 5)];
                ++count48[*((byte*)start + 6)];
                byte value = *((byte*)start + 7);
                value ^= 0x80;
                ++count56[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            index = *count24;
            for (int* start = count24 + 1, end = count24 + 255; start != end; *start++ = index) index += *start;
            index = *count32;
            for (int* start = count32 + 1, end = count32 + 255; start != end; *start++ = index) index += *start;
            index = *count40;
            for (int* start = count40 + 1, end = count40 + 255; start != end; *start++ = index) index += *start;
            index = *count48;
            for (int* start = count48 + 1, end = count48 + 255; start != end; *start++ = index) index += *start;
            *count56 = length;
            index = 0;
            for (int* start = count56 + 1, end = count56 + 256; start != end; *start++ = length - index) index += *start;
            *--count0 = 0;
            for (long* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++] = (ulong)*start ^ 0x8000000000000000UL;
                //byte* low = (byte*)(swapFixed + count0[*(byte*)start]++);
                //*(uint*)low = *(uint*)start;
                //*((uint*)(low + sizeof(uint))) = *((uint*)start + 1) ^ 0x80000000U;
            }
            *--count8 = 0;
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count8[*((byte*)start + 1)]++] = (long)*start;
            }
            *--count16 = 0;
            for (long* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++] = (ulong)*start;
            }
            *--count24 = 0;
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count24[*((byte*)start + 3)]++] = (long)*start;
            }
            *--count32 = 0;
            for (long* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count32[*((byte*)start + 4)]++] = (ulong)*start;
            }
            *--count40 = 0;
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[count40[*((byte*)start + 5)]++] = (long)*start;
            }
            *--count48 = 0;
            for (long* start = newArrayFixed, end = newArrayFixed + length; start != end; ++start)
            {
                swapFixed[count48[*((byte*)start + 6)]++] = (ulong)*start;
            }
            for (ulong* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                newArrayFixed[--count56[*((byte*)start + 7)]] = (long)(*start ^ 0x8000000000000000UL);
                //byte* low = (byte*)(newArrayFixed + --count56[*((byte*)start + 7)]);
                //*(uint*)low = *(uint*)start;
                //*((uint*)(low + sizeof(uint))) = *((uint*)start + 1) ^ 0x80000000U;
            }
            countBuffer64.Push(countFixed);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        internal static unsafe void SortDesc(LongSortIndex* arrayFixed, ULongSortIndex* swapFixed, int length)
        {
            byte* countFixed = countBuffer64.Get();
            Memory.ClearUnsafe((ulong*)countFixed, countBufferSize64 >> 3);
            int* count0 = (int*)countFixed + 1, count8 = count0 + 256, count16 = count0 + 256 * 2, count24 = count0 + 256 * 3;
            int* count32 = count0 + 256 * 4, count40 = count0 + 256 * 5, count48 = count0 + 256 * 6, count56 = count0 + (256 * 7 - 1);
            for (LongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                ++count0[*(byte*)start];
                ++count8[*((byte*)start + 1)];
                ++count16[*((byte*)start + 2)];
                ++count24[*((byte*)start + 3)];
                ++count32[*((byte*)start + 4)];
                ++count40[*((byte*)start + 5)];
                ++count48[*((byte*)start + 6)];
                byte value = *((byte*)start + 7);
                value ^= 0x80;
                ++count56[++value];
            }
            int index = *count0;
            for (int* start = count0 + 1, end = count0 + 255; start != end; *start++ = index) index += *start;
            index = *count8;
            for (int* start = count8 + 1, end = count8 + 255; start != end; *start++ = index) index += *start;
            index = *count16;
            for (int* start = count16 + 1, end = count16 + 255; start != end; *start++ = index) index += *start;
            index = *count24;
            for (int* start = count24 + 1, end = count24 + 255; start != end; *start++ = index) index += *start;
            index = *count32;
            for (int* start = count32 + 1, end = count32 + 255; start != end; *start++ = index) index += *start;
            index = *count40;
            for (int* start = count40 + 1, end = count40 + 255; start != end; *start++ = index) index += *start;
            index = *count48;
            for (int* start = count48 + 1, end = count48 + 255; start != end; *start++ = index) index += *start;
            *count56 = length;
            index = 0;
            for (int* start = count56 + 1, end = count56 + 256; start != end; *start++ = length - index) index += *start;
            *--count0 = 0;
            for (LongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count0[*(byte*)start]++].Set((ulong)(*start).Value ^ 0x8000000000000000UL, (*start).Index);
                //byte* low = (byte*)(swapFixed + count0[*(byte*)start]++);
                //*(uint*)low = *(uint*)start;
                //*((uint*)(low + sizeof(uint))) = *((uint*)start + 1) ^ 0x80000000U;
                //(*((ulongSortIndex*)low)).Index = (*start).Index;
            }
            *--count8 = 0;
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count8[*((byte*)start + 1)]++].Set((long)(*start).Value, (*start).Index);
            }
            *--count16 = 0;
            for (LongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count16[*((byte*)start + 2)]++].Set((ulong)(*start).Value, (*start).Index);
            }
            *--count24 = 0;
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count24[*((byte*)start + 3)]++].Set((long)(*start).Value, (*start).Index);
            }
            *--count32 = 0;
            for (LongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count32[*((byte*)start + 4)]++].Set((ulong)(*start).Value, (*start).Index);
            }
            *--count40 = 0;
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[count40[*((byte*)start + 5)]++].Set((long)(*start).Value, (*start).Index);
            }
            *--count48 = 0;
            for (LongSortIndex* start = arrayFixed, end = arrayFixed + length; start != end; ++start)
            {
                swapFixed[count48[*((byte*)start + 6)]++].Set((ulong)(*start).Value, (*start).Index);
            }
            for (ULongSortIndex* start = swapFixed, end = swapFixed + length; start != end; ++start)
            {
                arrayFixed[--count56[*((byte*)start + 7)]].Set((long)((*start).Value ^ 0x8000000000000000UL), (*start).Index);
                //byte* low = (byte*)(arrayFixed + --count56[*((byte*)start + 7)]);
                //*(uint*)low = *(uint*)start;
                //*((uint*)(low + sizeof(uint))) = *((uint*)start + 1) ^ 0x80000000U;
                //(*((longSortIndex*)low)).Index = (*start).Index;
            }
            countBuffer64.Push(countFixed);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        internal static unsafe void SortDesc(long[] array, int startIndex, int count)
        {
            int size = count * sizeof(ulong);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize64(size);
            try
            {
                fixed (long* arrayFixed = array)
                {
                    long* start = arrayFixed + startIndex;
                    sortDesc(start, start, swap.ULong, count);
                }
            }
            finally { pool.PushOnly(ref swap); }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的新数组</returns>
        internal static unsafe long[] GetSortDesc(long[] array, int startIndex, int count)
        {
            long[] newArray = new long[count];
            int size = count * sizeof(ulong);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize64(size);
            try
            {
                fixed (long* newArrayFixed = newArray, arrayFixed = array) sortDesc(arrayFixed + startIndex, newArrayFixed, swap.ULong, count);
            }
            finally { pool.PushOnly(ref swap); }
            return newArray;
        }
        
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void Sort(DateTimeSortIndex* arrayFixed, DateTimeSortIndex* swapFixed, int length)
        {
            Sort((LongSortIndex*)arrayFixed, (ULongSortIndex*)swapFixed, length);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void Sort(DateTime[] array, int startIndex, int count)
        {
            Sort(new UnionType { Value = array }.LongArray, startIndex, count);
            //int size = count * sizeof(ulong);
            //UnmanagedPool pool = GetUnmanagedPool(size);
            //Pointer.Size swap = pool.GetSize64(size);
            //try
            //{
            //    fixed (DateTime* arrayFixed = array)
            //    {
            //        long* start = (long*)arrayFixed + startIndex;
            //        sort(start, start, swap.ULong, count);
            //    }
            //}
            //finally { pool.PushOnly(ref swap); }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的新数组</returns>
        internal static unsafe DateTime[] GetSort(DateTime[] array, int startIndex, int count)
        {
            DateTime[] newArray = new DateTime[count];
            int size = count * sizeof(ulong);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize64(size);
            try
            {
                fixed (DateTime* newArrayFixed = newArray, arrayFixed = array) sort((long*)arrayFixed + startIndex, (long*)newArrayFixed, swap.ULong, count);
            }
            finally { pool.PushOnly(ref swap); }
            return newArray;
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="arrayFixed">数组起始位置</param>
        /// <param name="swapFixed">临时数组起始位置</param>
        /// <param name="length">数组数据长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void SortDesc(DateTimeSortIndex* arrayFixed, DateTimeSortIndex* swapFixed, int length)
        {
            SortDesc((LongSortIndex*)arrayFixed, (ULongSortIndex*)swapFixed, length);
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe void SortDesc(DateTime[] array, int startIndex, int count)
        {
            SortDesc(new UnionType { Value = array }.LongArray, startIndex, count);
            //int size = count * sizeof(ulong);
            //UnmanagedPool pool = GetUnmanagedPool(size);
            //Pointer.Size swap = pool.GetSize64(size);
            //try
            //{
            //    fixed (DateTime* arrayFixed = array)
            //    {
            //        long* start = (long*)arrayFixed + startIndex;
            //        sortDesc(start, start, swap.ULong, count);
            //    }
            //}
            //finally { pool.PushOnly(ref swap); }
        }
        /// <summary>
        /// 数组排序
        /// </summary>
        /// <param name="array">待排序数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">排序数据数量</param>
        /// <returns>排序后的新数组</returns>
        internal static unsafe DateTime[] GetSortDesc(DateTime[] array, int startIndex, int count)
        {
            DateTime[] newArray = new DateTime[count];
            int size = count * sizeof(ulong);
            UnmanagedPool pool = GetUnmanagedPool(size);
            Pointer.Size swap = pool.GetSize64(size);
            try
            {
                fixed (DateTime* newArrayFixed = newArray, arrayFixed = array)
                {
                    sortDesc((long*)arrayFixed + startIndex, (long*)newArrayFixed, swap.ULong, count);
                }
            }
            finally { pool.PushOnly(ref swap); }
            return newArray;
        }
    }
}
