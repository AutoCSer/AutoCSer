using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

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
        public static ReusableDictionary<HashString, valueType> CreateHashString<valueType>(int capacity = 0, bool isClear = true)
        {
#if __IOS__
            return new ReusableDictionary<HashString, valueType>(capacity, isClear, EqualityComparer.HashString);
#else
            return new ReusableDictionary<HashString, valueType>(capacity, isClear);
#endif
        }
    }
    /// <summary>
    /// 可重用字典
    /// </summary>
    public sealed partial class ReusableDictionary<keyType, valueType>
    {
        /// <summary>
        /// 节点数据
        /// </summary>
        private partial struct Node
        {
            /// <summary>
            /// 获取键值对
            /// </summary>
            internal KeyValue<keyType, valueType> KeyValue
            {
                get { return new KeyValue<keyType, valueType>(Key, Value); }
            }
            /// <summary>
            /// 删除节点时获取下一个数据索引位置
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal int RemoveGetNext(out valueType value)
            {
                value = Value;
                return Next;
            }
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal uint RemoveSet(ref Node node)
            {
                Source = node.Source;
                HashCode = node.HashCode;
                Next = node.Next;
                Key = node.Key;
                Value = node.Value;
                return Source;
            }
        }
        /// <summary>
        /// 键值对集合
        /// </summary>
        public IEnumerable<KeyValue<keyType, valueType>> KeyValues
        {
            get
            {
                if (count != 0)
                {
                    int index = count;
                    foreach (Node node in nodes)
                    {
                        yield return node.KeyValue;
                        if (--index == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 关键字集合
        /// </summary>
        public IEnumerable<keyType> Keys
        {
            get
            {
                if (count != 0)
                {
                    int index = count;
                    foreach (Node node in nodes)
                    {
                        yield return node.Key;
                        if (--index == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<valueType> Values
        {
            get
            {
                if (count != 0)
                {
                    int index = count;
                    foreach (Node node in nodes)
                    {
                        yield return node.Value;
                        if (--index == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 长度设为0（注意：对于引用类型没有置 0 可能导致内存泄露）
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Empty()
        {
            count = 0;
        }
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool ContainsKey(ref keyType key)
        {
            return count != 0 && indexOf(ref key) >= 0;
        }
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool ContainsKey(keyType key)
        {
            return count != 0 && indexOf(ref key) >= 0;
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Remove(keyType key)
        {
            valueType value;
            return Remove(ref key, out value);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Remove(ref keyType key)
        {
            valueType value;
            return Remove(ref key, out value);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">被删除数据</param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Remove(keyType key, out valueType value)
        {
            return Remove(ref key, out value);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">被删除数据</param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(ref keyType key, out valueType value)
        {
            if (count != 0)
            {
                SearchNode node = new SearchNode(ref key);
                int hashIndex = node.HashCode % capacity, nodeIndex = nodes[hashIndex].LinkIndex;
                if (nodeIndex < count)
                {
                    if (nodes[nodeIndex].Search(ref node, (uint)hashIndex))
                    {
                        int index = nodes[nodeIndex].RemoveGetNext(out value);
                        if (index != int.MaxValue) nodes[index].Source = (uint)hashIndex;
                        nodes[hashIndex].LinkIndex = index;
                        remove(nodeIndex);
                        return true;
                    }
                    for (int nextNodeIndex = node.Index; nextNodeIndex < count; nodeIndex = nextNodeIndex, nextNodeIndex = node.Index)
                    {
                        if (nodes[nextNodeIndex].Search(ref node))
                        {
                            hashIndex = nodes[nextNodeIndex].RemoveGetNext(out value);
                            if (hashIndex != int.MaxValue) nodes[hashIndex].Source = (uint)nodeIndex | 0x80000000U;
                            nodes[nodeIndex].Next = hashIndex;
                            remove(nextNodeIndex);
                            return true;
                        }
                    }
                }
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="nodeIndex"></param>
        private void remove(int nodeIndex)
        {
            int lastIndex = count - 1;
            if (nodeIndex != lastIndex)
            {
                uint source = nodes[nodeIndex].RemoveSet(ref nodes[lastIndex]);
                if ((source & 0x80000000U) == 0) nodes[(int)source].LinkIndex = nodeIndex;
                else nodes[(int)(source & (uint)int.MaxValue)].Next = nodeIndex;
            }
            if (isClear) nodes[lastIndex].Clear();
            count = lastIndex;
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="values"></param>
        internal void CopyTo(ref LeftArray<KeyValue<keyType, valueType>> values)
        {
            if (count != 0)
            {
                values.PrepLength(count);
                KeyValue<keyType, valueType>[] array = values.Array;
                int arrayIndex = values.Length, endIndex = arrayIndex + count;
                foreach (Node node in nodes)
                {
                    array[arrayIndex].Set(node.Key, node.Value);
                    if (++arrayIndex == endIndex) break;
                }
                values.Length = endIndex;
            }
        }
    }
}
