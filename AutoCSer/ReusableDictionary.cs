using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 可重用字典静态数据
    /// </summary>
    public static partial class ReusableDictionary
    {
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="isClear">是否需要清除数据</param>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReusableDictionary<int, valueType> CreateInt<valueType>(int capacity = 0, bool isClear = true)
        {
#if __IOS__
            return new ReusableDictionary<int, valueType>(capacity, isClear, EqualityComparer.Int);
#else
            return new ReusableDictionary<int, valueType>(capacity, isClear);
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
        internal static readonly int[] CapacityPrimes = { 3, 7, 17, 37, 89, 197, 431, 919, 1931, 4049, 8419, 17519, 36353, 75431, 156437, 324449, 672827, 1395263, 2893249, 5999471 };
        /// <summary>
        /// 获取质数索引
        /// </summary>
        /// <param name="capacity"></param>
        /// <returns></returns>
        internal static byte GetPrimeIndex(int capacity)
        {
            int start = 0, length = CapacityPrimes.Length, average;
            do
            {
                if (capacity > CapacityPrimes[average = start + ((length - start) >> 1)]) start = average + 1;
                else length = average;
            }
            while (start != length);
            return (byte)start;
        }
        /// <summary>
        /// 小质数集合
        /// </summary>
        private static readonly ushort[] primes;
        /// <summary>
        /// 判断是否质数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool IsPrime(int value)
        {
            foreach (ushort prime in primes)
            {
                if ((value % (int)prime) == 0) return false;
            }
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
            LeftArray<ushort> primes = new LeftArray<ushort>(4791);
            for (ushort mod = 3; mod != 3 * 3; mod += 2) primes.Add(mod);
            for (ushort max = (ushort)(int)Math.Sqrt(int.MaxValue), mod = 11; mod <= max; mod += 2)
            {
                if (isPrime(mod)) primes.Add(mod);
            }
            ReusableDictionary.primes = primes.ToArray();
        }
    }
    /// <summary>
    /// 创建可重用字典
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public static class ReusableDictionary<keyType> where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType = typeof(keyType).IsValueType;
        /// <summary>
        /// 创建字典
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="isClear">是否需要清除数据</param>
        /// <returns>字典</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReusableDictionary<keyType, valueType> Create<valueType>(int capacity = 0, bool isClear = true)
        {
#if __IOS__
            if (isValueType) return new ReusableDictionary<keyType, valueType>(capacity, isClear, EqualityComparer.comparer<keyType>.Default);
#endif
            return new ReusableDictionary<keyType, valueType>(capacity, isClear);
        }
    }
    /// <summary>
    /// 可重用字典
    /// </summary>
    /// <typeparam name="keyType"></typeparam>
    /// <typeparam name="valueType"></typeparam>
    public sealed partial class ReusableDictionary<keyType, valueType> where keyType : IEquatable<keyType>
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
            internal keyType Key;
            /// <summary>
            /// 数据
            /// </summary>
            internal valueType Value;

            /// <summary>
            /// 清除数据
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Clear()
            {
                Key = default(keyType);
                Value = default(valueType);
            }
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="node"></param>
            /// <param name="value"></param>
            /// <param name="source"></param>
            internal void Set(ref SearchNode node, valueType value, uint source)
            {
                HashCode = node.HashCode;
                Next = int.MaxValue;
                Key = node.Key;
                Value = value;
                Source = source;
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
            internal keyType Key;
            /// <summary>
            /// 查找节点
            /// </summary>
            /// <param name="key"></param>
            internal SearchNode(ref keyType key)
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
            internal SearchNode(ref Node node, out valueType value)
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
        public valueType this[keyType key]
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
                if (primeIndex < ReusableDictionary.CapacityPrimes.Length) capacity = ReusableDictionary.CapacityPrimes[primeIndex];
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
        private ReusableDictionary(ReusableDictionary<keyType, valueType> dictionary)
        {
            primeIndex = (byte)(dictionary.primeIndex + 1);
            if (primeIndex < ReusableDictionary.CapacityPrimes.Length) capacity = ReusableDictionary.CapacityPrimes[primeIndex];
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
            valueType value;
            for (int index = 0; index != nodes.Length; ++index)
            {
                SearchNode node = new SearchNode(ref nodes[index], out value);
                set(ref node, value);
            }
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
                while (count != 0) nodes[--count].Key = default(keyType);
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
                while (count != 0) nodes[--count].Value = default(valueType);
            }
            else count = 0;
        }
        /// <summary>
        /// 获取关键字匹配位置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int indexOf(ref keyType key)
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
        public bool TryGetValue(keyType key, out valueType value)
        {
            return TryGetValue(ref key, out value);
        }
        /// <summary>
        /// 尝试获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">目标数据</param>
        /// <returns>是否获取成功</returns>
        public bool TryGetValue(ref keyType key, out valueType value)
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
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 重组数据
        /// </summary>
        private void resize()
        {
            ReusableDictionary<keyType, valueType> dictionary = new ReusableDictionary<keyType, valueType>(this);
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
        private bool set(ref SearchNode node, valueType value)
        {
        START:
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
                                    return true;
                                }
                                resize();
                                goto START;
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
                    return true;
                }
                resize();
                goto START;
            }
            nodes[0].Set(ref node, value, (uint)hashIndex);
            nodes[hashIndex].LinkIndex = 0;
            count = 1;
            return true;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否新增数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Set(keyType key, valueType value)
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
        public bool Set(ref keyType key, valueType value)
        {
            SearchNode node = new SearchNode(ref key);
            return set(ref node, value);
        }
    }
}
