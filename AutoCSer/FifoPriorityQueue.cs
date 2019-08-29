using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 先进先出优先队列
    /// </summary>
    /// <typeparam name="keyType">键值类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed class FifoPriorityQueue<keyType, valueType> where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 数据节点
        /// </summary>
        internal sealed class Node
        {
            /// <summary>
            /// 前一个节点
            /// </summary>
            public Node Previous;
            /// <summary>
            /// 后一个节点
            /// </summary>
            public Node Next;
            /// <summary>
            /// 键值
            /// </summary>
            public keyType Key;
            /// <summary>
            /// 数据
            /// </summary>
            public valueType Value;
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        private ReusableDictionary<keyType, Node> dictionary;
        /// <summary>
        /// 获取所有数据
        /// </summary>
        public IEnumerable<KeyValue<keyType, valueType>> KeyValues
        {
            get
            {
                foreach (KeyValue<keyType, Node> KeyValue in dictionary.KeyValues) yield return new KeyValue<keyType, valueType>(KeyValue.Key, KeyValue.Value.Value);
            }
        }
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count
        {
            get { return dictionary.Count; }
        }
        /// <summary>
        /// 头节点
        /// </summary>
        private Node header;
        /// <summary>
        /// 尾节点
        /// </summary>
        private Node end;
        /// <summary>
        /// 数据对象
        /// </summary>
        /// <param name="key">查询键值</param>
        /// <returns>数据对象</returns>
        public valueType this[keyType key]
        {
            get
            {
                Node node = getNode(ref key);
                return node != null ? node.Value : default(valueType);
            }
            set { Set(ref key, value); }
        }
        /// <summary>
        /// 先进先出优先队列
        /// </summary>
        /// <param name="dictionaryCapacity">字典初始化容器尺寸</param>
        /// <param name="isClear">是否需要清除数据</param>
        public FifoPriorityQueue(int dictionaryCapacity = 0, bool isClear = true)
        {
            dictionary = ReusableDictionary<keyType>.Create<Node>(dictionaryCapacity, isClear);
        }
        /// <summary>
        /// 长度设为0（注意：对于引用类型没有置 0 可能导致内存泄露）
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Empty()
        {
            dictionary.Empty();
            header = end = null;
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Clear()
        {
            dictionary.Clear();
            header = end = null;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="nullValue">失败空值</param>
        /// <returns>数据对象</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType Get(ref keyType key, valueType nullValue)
        {
            Node node = getNode(ref key);
            return node != null ? node.Value : nullValue;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">目标数据对象</param>
        /// <returns>是否获取成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool TryGetValue(ref keyType key, out valueType value)
        {
            Node node = getNode(ref key);
            if (node != null)
            {
                value = node.Value;
                return true;
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>数据对象</returns>
        private Node getNode(ref keyType key)
        {
            Node node;
            if (dictionary.TryGetValue(ref key, out node))
            {
                if (node != end)
                {
                    Node previous = node.Previous;
                    if (previous == null) (header = node.Next).Previous = null;
                    else (previous.Next = node.Next).Previous = previous;
                    end.Next = node;
                    node.Previous = end;
                    node.Next = null;
                    end = node;
                }
            }
            return node;
        }
        /// <summary>
        /// 获取数据(不调整位置)
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value"></param>
        /// <returns>数据对象</returns>
        public bool TryGetOnly(keyType key, out valueType value)
        {
            Node node;
            if (dictionary.TryGetValue(ref key, out node))
            {
                value = node.Value;
                return true;
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">数据对象</param>
        /// <returns>被替换的数据对象,没有返回default(valueType)</returns>
        public valueType Set(ref keyType key, valueType value)
        {
            Node node = getNode(ref key);
            if (node != null)
            {
                valueType oldValue = node.Value;
                node.Value = value;
                return oldValue;
            }
            else
            {
                UnsafeAdd(ref key, value);
                return default(valueType);
            }
        }
        /// <summary>
        /// 设置数据(不调整位置)
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetOnly(keyType key, valueType value)
        {
            Node node;
            if (dictionary.TryGetValue(ref key, out node)) node.Value = value;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeAdd(ref keyType key, valueType value)
        {
            Node node = new Node { Value = value, Key = key, Previous = end };
            dictionary.Set(ref key, node);
            if (end == null) header = end = node;
            else
            {
                end.Next = node;
                end = node;
            }
        }
        /// <summary>
        /// 弹出一个节点
        /// </summary>
        /// <returns></returns>
        internal Node UnsafePopNode()
        {
            Node node = header;
            if ((header = header.Next) == null) end = null;
            else header.Previous = null;
            dictionary.Remove(ref node.Key);
            return node;
        }
        /// <summary>
        /// 弹出一个值
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Pop()
        {
            if (header != null) UnsafePopNode();
        }
        /// <summary>
        /// 弹出一个值
        /// </summary>
        /// <returns>值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal valueType UnsafePopValue()
        {
            return UnsafePopNode().Value;
        }
        /// <summary>
        /// 删除一个数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">被删除数据对象</param>
        /// <returns>是否删除了数据对象</returns>
        public bool Remove(ref keyType key, out valueType value)
        {
            Node node;
            if (dictionary.Remove(ref key, out node))
            {
                if (node.Previous == null)
                {
                    header = node.Next;
                    if (header == null) end = null;
                    else header.Previous = null;
                }
                else if (node.Next == null) (end = node.Previous).Next = null;
                else
                {
                    node.Previous.Next = node.Next;
                    node.Next.Previous = node.Previous;
                }
                value = node.Value;
                return true;
            }
            value = default(valueType);
            return false;
        }
    }
}
