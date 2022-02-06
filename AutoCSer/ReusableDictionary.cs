using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;
using AutoCSer.Extensions;

namespace AutoCSer
{
    /// <summary>
    /// 可重用字典静态数据（主要用于非引用类型缓冲区，避免 new / Clear 开销）
    /// </summary>
    public static unsafe partial class ReusableDictionary
    {
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="isClear">是否需要清除数据</param>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReusableDictionary<int, T> CreateInt<T>(int capacity = 0, bool isClear = true)
        {
#if __IOS__
            return new ReusableDictionary<int, T>(capacity, isClear, AutoCSer.IOS.IntComparer.Default);
#else
            return new ReusableDictionary<int, T>(capacity, isClear);
#endif
        }

        /// <summary>
        /// 查找节点状态
        /// </summary>
        internal enum SearchState : byte
        {
            /// <summary>
            /// 最后一个节点
            /// </summary>
            End,
            /// <summary>
            /// 相等
            /// </summary>
            Equal,
            /// <summary>
            /// 下一个节点
            /// </summary>
            Next,
        }


        /// <summary>
        /// 容器大小质数集合
        /// </summary>
        private static AutoCSer.Memory.Pointer capacityPrimes;
        /// <summary>
        /// 根据质数索引获取质数，失败返回 0
        /// </summary>
        /// <param name="primeIndex"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static int GetPrime(int primeIndex)
        {
            if (primeIndex < capacityPrimes.CurrentIndex) return capacityPrimes.Int[primeIndex];
            return 0;
        }
        /// <summary>
        /// 获取质数索引
        /// </summary>
        /// <param name="capacity"></param>
        /// <returns></returns>
        internal static byte GetPrimeIndex(int capacity)
        {
            int start = 0, length = capacityPrimes.CurrentIndex, average;
            do
            {
                if (capacity > capacityPrimes.Int[average = start + ((length - start) >> 1)]) start = average + 1;
                else length = average;
            }
            while (start != length);
            return (byte)start;
        }
        /// <summary>
        /// 小质数集合起始位置
        /// </summary>
        private static AutoCSer.Memory.Pointer primes;
        /// <summary>
        /// 小质数集合结束位置
        /// </summary>
        private static AutoCSer.Memory.Pointer endPrimes;
        /// <summary>
        /// 判断是否质数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool IsPrime(int value)
        {
            ushort* prime = primes.UShort, endPrime = endPrimes.UShort;
            do
            {
                if ((value % (int)*prime) == 0) return false;
            }
            while (++prime != endPrime);
            return true;
        }
        /// <summary>
        /// 判断是否质数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool isPrime(int value)
        {
            for (int max = (int)Math.Sqrt(value), mod = 3; mod <= max; mod += 2)
            {
                if ((value % mod) == 0) return false;
            }
            return true;
        }
        static ReusableDictionary()
        {
            int[] capacityPrimesArray = new int[] { 3, 7, 17, 37, 89, 197, 431, 919, 1931, 4049, 8419, 17519, 36353, 75431, 156437, 324449, 672827, 1395263, 2893249, 5999471 };
            capacityPrimes = Unmanaged.GetStaticPointer(capacityPrimesArray.Length * sizeof(int) + 4792 * sizeof(ushort), false);
            capacityPrimes.CurrentIndex = capacityPrimesArray.Length;
            capacityPrimesArray.AsSpan().CopyTo(new Span<int>(capacityPrimes.Data, capacityPrimes.ByteSize));

            primes = new AutoCSer.Memory.Pointer(capacityPrimes.Int + capacityPrimesArray.Length, 0);
            ushort* endPrime = primes.UShort;
            for (ushort mod = 3; mod != 3 * 3; mod += 2) *endPrime++ = mod;
            for (ushort max = (ushort)(int)Math.Sqrt(int.MaxValue), mod = 11; mod <= max; mod += 2)
            {
                if (isPrime(mod)) *endPrime++ = mod;
            }
            endPrimes = new AutoCSer.Memory.Pointer(endPrime, 0);
        }
    }
    /// <summary>
    /// 创建可重用字典（主要用于非引用类型缓冲区，避免 new / Clear 开销）
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    public static class ReusableDictionary<KT> where KT : IEquatable<KT>
    {
#if __IOS__
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType = typeof(KT).IsValueType;
#endif
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="VT">数据类型</typeparam>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="isClear">是否需要清除数据</param>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReusableDictionary<KT, VT> Create<VT>(int capacity = 0, bool isClear = true)
        {
#if __IOS__
            if (isValueType) return new ReusableDictionary<KT, VT>(capacity, isClear, AutoCSer.IOS.EqualityComparer<KT>.Default);
#endif
            return new ReusableDictionary<KT, VT>(capacity, isClear);
        }
    }
    /// <summary>
    /// 可重用字典（主要用于非引用类型缓冲区，避免 new / Clear 开销）
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
    public sealed partial class ReusableDictionary<KT, VT> where KT : IEquatable<KT>
    {
        /// <summary>
        /// 节点数据
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private partial struct Node
        {
            /// <summary>
            /// 哈希索引
            /// </summary>
            internal int LinkIndex;
            /// <summary>
            /// 节点来源，最高位为 0 表示首节点，否则表示后续节点
            /// </summary>
            internal uint Source;
            /// <summary>
            /// 节点来源，用于调试
            /// </summary>
            private uint SourceIndex { get { return Source & (uint)int.MaxValue; } }

            /// <summary>
            /// 关键字哈希值
            /// </summary>
            internal int HashCode;
            /// <summary>
            /// 下一个数据索引位置，int.MaxValue 表示最后一个
            /// </summary>
            internal int Next;
            /// <summary>
            /// 关键字
            /// </summary>
            internal KT Key;
            /// <summary>
            /// 数据
            /// </summary>
            internal VT Value;

            /// <summary>
            /// 清除数据
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Clear()
            {
                Key = AutoCSer.Common.GetDefault<KT>();
                Value = AutoCSer.Common.GetDefault<VT>();
            }
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="node"></param>
            /// <param name="value"></param>
            /// <param name="source"></param>
            internal void Set(ref SearchNode node, VT value, uint source)
            {
                HashCode = node.HashCode;
                Key = node.Key;
                Value = value;
                Source = source;
                Next = int.MaxValue;
            }
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="node"></param>
            /// <param name="value"></param>
            /// <param name="source"></param>
            /// <param name="next"></param>
            internal void Set(ref SearchNode node, VT value, uint source, int next)
            {
                HashCode = node.HashCode;
                Key = node.Key;
                Value = value;
                Source = source;
                Next = next;
            }
            /// <summary>
            /// 判断关键字是否相等
            /// </summary>
            /// <param name="node">查找节点</param>
            /// <param name="source">节点来源</param>
            /// <returns></returns>
            internal bool Search(ref SearchNode node, uint source)
            {
                if (HashCode == node.HashCode)
                {
                    if (Source == source)
                    {
                        if (this.Key.Equals(node.Key)) return true;
                        node.Index = Next;
                    }
                    else node.Index = int.MaxValue;
                }
                else node.Index = Source == source ? Next : int.MaxValue;
                return false;
            }
            /// <summary>
            /// 判断关键字是否相等
            /// </summary>
            /// <param name="node">查找节点</param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal bool Search(ref SearchNode node)
            {
                if (HashCode == node.HashCode && this.Key.Equals(node.Key)) return true;
                node.Index = Next;
                return false;
            }
            /// <summary>
            /// 搜索节点
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            internal ReusableDictionary.SearchState SearchState(ref SearchNode node)
            {
                if (HashCode == node.HashCode && this.Key.Equals(node.Key)) return ReusableDictionary.SearchState.Equal;
                if (Next == int.MaxValue) return ReusableDictionary.SearchState.End;
                node.Index = Next;
                return ReusableDictionary.SearchState.Next;
            }
        }
        /// <summary>
        /// 查找节点
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct SearchNode
        {
            /// <summary>
            /// 关键字哈希值
            /// </summary>
            internal int HashCode;
            /// <summary>
            /// 查找索引
            /// </summary>
            internal int Index;
            /// <summary>
            /// 关键字
            /// </summary>
            internal KT Key;
            /// <summary>
            /// 查找节点
            /// </summary>
            /// <param name="key"></param>
            internal SearchNode(ref KT key)
            {
                HashCode = key.GetHashCode() & int.MaxValue;
                Key = key;
                Index = int.MaxValue;
            }
            /// <summary>
            /// 查找节点
            /// </summary>
            /// <param name="node"></param>
            /// <param name="value"></param>
            internal SearchNode(ref Node node, out VT value)
            {
                HashCode = node.HashCode;
                Key = node.Key;
                Index = int.MaxValue;
                value = node.Value;
            }
        }
#if __IOS__
        /// <summary>
        /// 比较器
        /// </summary>
        private readonly IEqualityComparer<keyType> comparer;
#endif
        /// <summary>
        /// 是否需要清除数据
        /// </summary>
        private readonly bool isClear;
        /// <summary>
        /// 质数索引未知
        /// </summary>
        private byte primeIndex;
        /// <summary>
        /// 容器大小
        /// </summary>
        private int capacity;
        /// <summary>
        /// 有效数据数量
        /// </summary>
        private int count;
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count
        {
            get { return count; }
        }
        /// <summary>
        /// 节点集合
        /// </summary>
        private Node[] nodes;
        /// <summary>
        /// 获取或者设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public VT this[KT key]
        {
            get
            {
                int index = indexOf(ref key);
                if (index >= 0) return nodes[index].Value;
                throw new IndexOutOfRangeException();
            }
            set { Set(ref key, value); }
        }
        /// <summary>
        /// 可重用字典
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="isClear">是否需要清除数据</param>
#if __IOS__
        /// <param name="comparer">比较器</param>
#endif
        public ReusableDictionary(int capacity = 0, bool isClear = true
#if __IOS__
            , IEqualityComparer<keyType> comparer
#endif
)
        {
            if (capacity <= 3)
            {
                primeIndex = 0;
                capacity = 3;
            }
            else
            {
                primeIndex = ReusableDictionary.GetPrimeIndex(capacity);
                int prime = ReusableDictionary.GetPrime(primeIndex);
                if (prime != 0) capacity = prime;
                else
                {
                    for (capacity |= 1; capacity != int.MaxValue; capacity += 2)
                    {
                        if (ReusableDictionary.IsPrime(capacity)) break;
                    }
                }
            }
#if __IOS__
            this.comparer = comparer;
#endif
            this.isClear = isClear;
            this.capacity = capacity;
            nodes = new Node[capacity];
        }
        /// <summary>
        /// 可重用字典重组 
        /// </summary>
        /// <param name="dictionary"></param>
        private ReusableDictionary(ReusableDictionary<KT, VT> dictionary)
        {
            primeIndex = (byte)(dictionary.primeIndex + 1);
            int prime = ReusableDictionary.GetPrime(primeIndex);
            if (prime != 0) capacity = prime;
            else
            {
                capacity = dictionary.capacity;
                if (capacity == int.MaxValue) throw new IndexOutOfRangeException();
                capacity = capacity < int.MaxValue >> 1 ? (capacity << 1) + 1 : int.MaxValue;
                while (capacity != int.MaxValue)
                {
                    if (ReusableDictionary.IsPrime(capacity)) break;
                    capacity += 2;
                }
            }
#if __IOS__
            this.comparer = dictionary.comparer;
#endif
            isClear = dictionary.isClear;
            this.nodes = new Node[capacity];

            Node[] nodes = dictionary.nodes;
            for (int index = 0; index != nodes.Length; ++index)
            {
                VT value;
                SearchNode node = new SearchNode(ref nodes[index], out value);
                add(ref node, value);
            }
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private void add(ref SearchNode node, VT value)
        {
            int hashIndex = node.HashCode % capacity, linkIndex = nodes[hashIndex].LinkIndex;
            if (linkIndex < count && nodes[linkIndex].Source == (uint)hashIndex)
            {
                nodes[linkIndex].Source = (uint)count | 0x80000000U;
                nodes[count].Set(ref node, value, (uint)hashIndex, linkIndex);
            }
            else nodes[count].Set(ref node, value, (uint)hashIndex);
            nodes[hashIndex].LinkIndex = count++;
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clear()
        {
            if (isClear)
            {
                while (count != 0) nodes[--count].Clear();
            }
            else count = 0;
        }
        /// <summary>
        /// 清除关键字 
        /// </summary>
        internal void ClearKey()
        {
            if (isClear)
            {
                while (count != 0) nodes[--count].Key = AutoCSer.Common.GetDefault<KT>();
            }
            else count = 0;
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        internal void ClearValue()
        {
            if (isClear)
            {
                while (count != 0) nodes[--count].Value = AutoCSer.Common.GetDefault<VT>();
            }
            else count = 0;
        }
        /// <summary>
        /// 获取关键字匹配位置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int indexOf(ref KT key)
        {
            SearchNode node = new SearchNode(ref key);
            int hashIndex = node.HashCode % capacity, nodeIndex = nodes[hashIndex].LinkIndex;
            if (nodeIndex < count)
            {
                if (nodes[nodeIndex].Search(ref node, (uint)hashIndex)) return nodeIndex;
                while (node.Index < count)
                {
                    if (nodes[node.Index].Search(ref node)) return node.Index;
                }
            }
            return -1;
        }
        /// <summary>
        /// 尝试获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">目标数据</param>
        /// <returns>是否获取成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool TryGetValue(KT key, out VT value)
        {
            return TryGetValue(ref key, out value);
        }
        /// <summary>
        /// 尝试获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">目标数据</param>
        /// <returns>是否获取成功</returns>
        public bool TryGetValue(ref KT key, out VT value)
        {
            if (count != 0)
            {
                int index = indexOf(ref key);
                if (index >= 0)
                {
                    value = nodes[index].Value;
                    return true;
                }
            }
            value = AutoCSer.Common.GetDefault<VT>();
            return false;
        }
        /// <summary>
        /// 重组数据
        /// </summary>
        private void resize()
        {
            ReusableDictionary<KT, VT> dictionary = new ReusableDictionary<KT, VT>(this);
            nodes = dictionary.nodes;
            count = dictionary.count;
            primeIndex = dictionary.primeIndex;
            capacity = dictionary.capacity;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="value"></param>
        /// <returns>是否新增数据</returns>
        private bool set(ref SearchNode node, VT value)
        {
            int hashIndex = node.HashCode % capacity;
            if (count != 0)
            {
                node.Index = nodes[hashIndex].LinkIndex;
                if (node.Index < count && nodes[node.Index].Source == (uint)hashIndex)
                {
                    do
                    {
                        int nodeIndex = node.Index;
                        switch (nodes[nodeIndex].SearchState(ref node))
                        {
                            case ReusableDictionary.SearchState.End:
                                if (count != capacity)
                                {
                                    nodes[count].Set(ref node, value, (uint)nodeIndex | 0x80000000U);
                                    nodes[nodeIndex].Next = count++;
                                }
                                else
                                {
                                    resize();
                                    add(ref node, value);
                                }
                                return true;
                            case ReusableDictionary.SearchState.Equal: nodes[nodeIndex].Value = value; return false;
                            default: break;
                        }
                    }
                    while (true);
                }
                if (count != capacity)
                {
                    nodes[count].Set(ref node, value, (uint)hashIndex);
                    nodes[hashIndex].LinkIndex = count++;
                }
                else
                {
                    resize();
                    add(ref node, value);
                }
            }
            else
            {
                nodes[0].Set(ref node, value, (uint)hashIndex);
                nodes[hashIndex].LinkIndex = 0;
                count = 1;
            }
            return true;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否新增数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Set(KT key, VT value)
        {
            SearchNode node = new SearchNode(ref key);
            return set(ref node, value);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否新增数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Set(ref KT key, VT value)
        {
            SearchNode node = new SearchNode(ref key);
            return set(ref node, value);
        }
    }
}
