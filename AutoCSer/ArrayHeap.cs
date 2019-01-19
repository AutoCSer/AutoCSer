using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 数组模拟最小堆
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal unsafe class ArrayHeap<keyType, valueType> : IDisposable
        where keyType : IComparable<keyType>
    {
        /// <summary>
        /// 默认数组长度
        /// </summary>
        private const int defaultArrayLength = 256;
        /// <summary>
        /// 数据数组
        /// </summary>
        internal KeyValue<keyType, valueType>[] Array;
        /// <summary>
        /// 最小堆索引
        /// </summary>
        internal Pointer.Size Heap;
        /// <summary>
        /// 是否固定内存申请
        /// </summary>
        private bool isStaticUnmanaged;
        /// <summary>
        /// 数据数量
        /// </summary>
        internal int Count
        {
            get { return *Heap.Int; }
        }
        /// <summary>
        /// 数组模拟最小堆
        /// </summary>
        /// <param name="isStaticUnmanaged">是否固定内存申请</param>
        internal ArrayHeap(bool isStaticUnmanaged = false)
        {
            Array = new KeyValue<keyType, valueType>[defaultArrayLength];
            Heap = Unmanaged.Get(defaultArrayLength * sizeof(int), false, this.isStaticUnmanaged = isStaticUnmanaged);
            reset(Heap.Int);
        }
        /// <summary>
        ///  析构释放资源
        /// </summary>
        ~ArrayHeap()
        {
            Unmanaged.Free(ref Heap, isStaticUnmanaged);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Unmanaged.Free(ref Heap, isStaticUnmanaged);
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        internal void Clear()
        {
            if (Array.Length == defaultArrayLength)
            {
                int* heapFixed = Heap.Int;
                if (*heapFixed != 0)
                {
                    reset(heapFixed);
                    System.Array.Clear(Array, 0, Array.Length);
                }
            }
            else
            {
                KeyValue<keyType, valueType>[] newArray = new KeyValue<keyType, valueType>[defaultArrayLength];
                Pointer.Size newHeap = Unmanaged.Get(defaultArrayLength * sizeof(int), false, isStaticUnmanaged), oldHeap = Heap;
                reset(newHeap.Int);
                Array = newArray;
                Heap = newHeap;
                Unmanaged.Free(ref oldHeap, isStaticUnmanaged);
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">数据值</param>
        internal unsafe void Push(keyType key, ref valueType value)
        {
            int* heapFixed = Heap.Int;
            if (*heapFixed == 0) Array[heapFixed[1]].Set(key, value);
            else
            {
                int heapIndex = *heapFixed + 1;
                if (heapIndex == Array.Length)
                {
                    create();
                    heapFixed = Heap.Int;
                }
                int valueIndex = heapFixed[heapIndex];
                Array[valueIndex].Set(key, value);
                heapFixed[getPushIndex(key, heapIndex)] = valueIndex;
            }
            ++*heapFixed;
        }
        /// <summary>
        /// 重建数据
        /// </summary>
        protected void create()
        {
            int heapIndex = Array.Length, newCount = heapIndex << 1, newHeapSize = newCount * sizeof(int);
            KeyValue<keyType, valueType>[] newArray = new KeyValue<keyType, valueType>[newCount];
            Pointer.Size newHeap = Unmanaged.Get(newHeapSize, false, isStaticUnmanaged), oldHeap = Heap;
            int* newHeapFixed = newHeap.Int;
            Array.CopyTo(newArray, 0);
            AutoCSer.Memory.CopyNotNull(Heap.Byte, newHeapFixed, newHeapSize >> 1);
            do
            {
                --newCount;
                newHeapFixed[newCount] = newCount;
            }
            while (newCount != heapIndex);
            Array = newArray;
            Heap = newHeap;
            Unmanaged.Free(ref oldHeap, isStaticUnmanaged);
        }
        /// <summary>
        /// 获取添加数据位置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="heapIndex"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int getPushIndex(keyType key, int heapIndex)
        {
            int* heapFixed = Heap.Int;
        START:
            int parentValueIndex = heapFixed[heapIndex >> 1];
            if (key.CompareTo(Array[parentValueIndex].Key) < 0)
            {
                heapFixed[heapIndex] = parentValueIndex;
                if ((heapIndex >>= 1) != 1) goto START;
                return 1;
            }
            return heapIndex;
        }
        /// <summary>
        /// 删除堆顶数据
        /// </summary>
        internal void RemoveTop()
        {
            int* heapFixed = Heap.Int;
            int heapIndex = 1, lastHeapIndex = *heapFixed, lastValueIndex = heapFixed[lastHeapIndex];
            Array[heapFixed[lastHeapIndex] = heapFixed[1]].Null();
            for (int maxHeapIndex = (lastHeapIndex + 1) >> 1; heapIndex < maxHeapIndex; )
            {
                int left = heapIndex << 1, right = left + 1;
                if (right != lastHeapIndex)
                {
                    if (Array[heapFixed[left]].Key.CompareTo(Array[heapFixed[right]].Key) < 0)
                    {
                        heapFixed[heapIndex] = heapFixed[left];
                        heapIndex = left;
                    }
                    else
                    {
                        heapFixed[heapIndex] = heapFixed[right];
                        heapIndex = right;
                    }
                }
                else
                {
                    heapFixed[heapIndex] = heapFixed[left];
                    heapIndex = left;
                    break;
                }
            }
            heapFixed[heapIndex == 1 ? 1 : getPushIndex(Array[lastValueIndex].Key, heapIndex)] = lastValueIndex;
            --*heapFixed;
        }

        /// <summary>
        /// 初始化索引
        /// </summary>
        /// <param name="heapFixed"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void reset(int* heapFixed)
        {
            for (int index = defaultArrayLength; index != 0; heapFixed[index] = index) --index;
        }
    }
}
