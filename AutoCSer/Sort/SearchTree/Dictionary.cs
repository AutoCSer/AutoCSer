using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 二叉树字典
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed partial class Dictionary<keyType, valueType> where keyType : IComparable<keyType>
    {
        /// <summary>
        /// 二叉树节点
        /// </summary>
        internal sealed class Node : Node<Node, keyType>
        {
            /// <summary>
            /// 节点数据
            /// </summary>
            internal valueType Value;
            /// <summary>
            /// 数据
            /// </summary>
            internal KeyValue<keyType, valueType> KeyValue
            {
                get { return new KeyValue<keyType, valueType>(Key, Value); }
            }
            /// <summary>
            /// 数据集合
            /// </summary>
            internal IEnumerable<KeyValue<keyType, valueType>> KeyValues
            {
                get
                {
                    if (Left != null)
                    {
                        foreach (KeyValue<keyType, valueType> value in Left.KeyValues) yield return value;
                    }
                    yield return new KeyValue<keyType, valueType>(Key, Value);
                    if (Right != null)
                    {
                        foreach (KeyValue<keyType, valueType> value in Right.KeyValues) yield return value;
                    }
                }
            }

            /// <summary>
            /// 根据关键字获取二叉树节点
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>匹配节点</returns>
            internal Node Get(ref keyType key)
            {
                int cmp = key.CompareTo(Key);
                if (cmp == 0) return this;
                return cmp < 0 ? (Left != null ? Left.Get(ref key) : null) : (Right != null ? Right.Get(ref key) : null);
            }
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal Node At(int index)
            {
                if ((uint)index < Count) return at(index);
                throw new IndexOutOfRangeException();
            }
            /// <summary>
            /// 根据节点位置获取数据
            /// </summary>
            /// <param name="index">节点位置</param>
            /// <returns>数据</returns>
            internal Node at(int index)
            {
                if (Left != null)
                {
                    if (index < Left.Count) return Left.at(index);
                    if ((index -= Left.Count) == 0) return this;
                }
                else if (index == 0) return this;
                return Right.at(index - 1);
            }

            /// <summary>
            /// 交换节点数据
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            private void changeKeyValue(ref keyType key, ref valueType value)
            {
                keyType tempKey = key;
                valueType tempValue = value;
                key = Key;
                value = Value;
                Key = tempKey;
                Value = tempValue;
            }
            /// <summary>
            /// 检测左节点数量
            /// </summary>
            private void checkLeft()
            {
                ++Count;
                if (Right != null)
                {
                    if ((Left.Count >> 1) > Right.Count && Left.isLeftAndRight)
                    {
                        if (Left.leftRightDifferenceCount <= 0)
                        {
                            Node leftRight = Left.Right;
                            if (leftRight.isLeftAndRight)
                            {
                                Left.Right = leftRight.rightToLeft(Right);
                                Left.removeCount1(leftRight.Left);
                                leftRight.changeKeyValue(ref Key, ref Value);
                                Right = leftRight;
                            }
                        }
                        else if (Left.Right != null)
                        {
                            Node left = Left;
                            Left = left.rightToLeft(Right);
                            left.changeKeyValue(ref Key, ref Value);
                            Right = left;
                        }
                    }
                }
                else
                {
                    checkLeftRight();
                    Right.changeKeyValue(ref Key, ref Value);
                }
            }
            /// <summary>
            /// 检测右节点数量
            /// </summary>
            private void checkRight()
            {
                ++Count;
                if (Left != null)
                {
                    if ((Right.Count >> 1) > Left.Count && Right.isLeftAndRight)
                    {
                        if (Right.leftRightDifferenceCount >= 0)
                        {
                            Node rightLeft = Right.Left;
                            if (rightLeft.isLeftAndRight)
                            {
                                Right.Left = rightLeft.leftToRight(Left);
                                Right.removeCount1(rightLeft.Right);
                                rightLeft.changeKeyValue(ref Key, ref Value);
                                Left = rightLeft;
                            }
                        }
                        else if (Right.Left != null)
                        {
                            Node right = Right;
                            Right = right.leftToRight(Left);
                            right.changeKeyValue(ref Key, ref Value);
                            Left = right;
                        }
                    }
                }
                else
                {
                    checkRightLeft();
                    Left.changeKeyValue(ref Key, ref Value);
                }
            }
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="keyValue"></param>
            /// <returns>是否添加了数据</returns>
            internal bool TryAdd(ref KeyValue<keyType, valueType> keyValue)
            {
                int cmp = keyValue.Key.CompareTo(Key);
                if (cmp == 0) return false;
                if (cmp < 0)
                {
                    if (Left == null)
                    {
                        Left = new Node { Key = keyValue.Key, Value = keyValue.Value, Count = 1 };
                        ++Count;
                        return true;
                    }
                    if (Left.TryAdd(ref keyValue))
                    {
                        checkLeft();
                        return true;
                    }
                    return false;
                }
                if (Right == null)
                {
                    Right = new Node { Key = keyValue.Key, Value = keyValue.Value, Count = 1 };
                    ++Count;
                    return true;
                }
                if (Right.TryAdd(ref keyValue))
                {
                    checkRight();
                    return true;
                }
                return false;
            }
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="keyValue">数据</param>
            /// <returns>是否添加了数据</returns>
            internal bool Set(ref KeyValue<keyType, valueType> keyValue)
            {
                int cmp = keyValue.Key.CompareTo(Key);
                if (cmp == 0)
                {
                    Value = keyValue.Value;
                    return false;
                }
                if (cmp < 0)
                {
                    if (Left == null)
                    {
                        Left = new Node { Key = keyValue.Key, Value = keyValue.Value, Count = 1 };
                        ++Count;
                        return true;
                    }
                    if (Left.Set(ref keyValue))
                    {
                        checkLeft();
                        return true;
                    }
                    return false;
                }
                if (Right == null)
                {
                    Right = new Node { Key = keyValue.Key, Value = keyValue.Value, Count = 1 };
                    ++Count;
                    return true;
                }
                if (Right.Set(ref keyValue))
                {
                    checkRight();
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 删除数据
            /// </summary>
            /// <param name="key">关键字</param>
            /// <returns>被删除节点</returns>
            internal Node Remove(ref keyType key)
            {
                int cmp = key.CompareTo(Key);
                if (cmp == 0) return this;
                if (cmp < 0)
                {
                    if (Left != null)
                    {
                        Node node = Left.Remove(ref key);
                        if (node != null)
                        {
                            --Count;
                            if (node == Left) Left = node.Remove();
                            return node;
                        }
                    }
                }
                else if (Right != null)
                {
                    Node node = Right.Remove(ref key);
                    if (node != null)
                    {
                        --Count;
                        if (node == Right) Right = node.Remove();
                        return node;
                    }
                }
                return null;
            }
            /// <summary>
            /// 删除当前节点
            /// </summary>
            /// <returns>用户替换当前节点的节点</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal Node Remove()
            {
                if (Right != null)
                {
                    Node node = Right.removeMin();
                    if (node == Right) node.set(Left, Count - 1);
                    else node.set(Left, Right, Count - 1);
                    return node;
                }
                if (Left != null)
                {
                    Left.Count = Count - 1;
                    return Left;
                }
                return null;
            }
            /// <summary>
            /// 删除最小节点
            /// </summary>
            /// <returns></returns>
            private Node removeMin()
            {
                if (Left != null)
                {
                    --Count;
                    Node node = Left.removeMin();
                    if (node == Left) Left = node.Right;
                    return node;
                }
                return this;
            }

            /// <summary>
            /// 获取数组
            /// </summary>
            /// <param name="array"></param>
            internal void GetArraySkip(ref PageArray<valueType> array)
            {
                if (Left != null)
                {
                    int count = Left.Count;
                    if (count > array.SkipCount)
                    {
                        Left.GetArraySkip(ref array);
                        if (!array.IsArray && !array.Add(Value)) Right.getArray(ref array);
                        return;
                    }
                    array.SkipCount -= count;
                }
                if (array.SkipCount == 0)
                {
                    if (!array.Add(Value)) Right.getArray(ref array);
                    return;
                }
                --array.SkipCount;
                Right.GetArraySkip(ref array);
            }
            /// <summary>
            /// 获取数组
            /// </summary>
            /// <param name="array"></param>
            private void getArray(ref PageArray<valueType> array)
            {
                if (Left != null)
                {
                    Left.getArray(ref array);
                    if (array.IsArray) return;
                }
                if (!array.Add(Value)) Right.getArray(ref array);
            }

            /// <summary>
            /// 获取数组
            /// </summary>
            /// <typeparam name="arrayType"></typeparam>
            /// <param name="array"></param>
            internal void GetArraySkip<arrayType>(ref PageArray<valueType, arrayType> array)
            {
                if (Left != null)
                {
                    int count = Left.Count;
                    if (count > array.SkipCount)
                    {
                        Left.GetArraySkip(ref array);
                        if (!array.IsArray && !array.Add(Value)) Right.getArray(ref array);
                        return;
                    }
                    array.SkipCount -= count;
                }
                if (array.SkipCount == 0)
                {
                    if (!array.Add(Value)) Right.getArray(ref array);
                    return;
                }
                --array.SkipCount;
                Right.GetArraySkip(ref array);
            }
            /// <summary>
            /// 获取数组
            /// </summary>
            /// <typeparam name="arrayType"></typeparam>
            /// <param name="array"></param>
            private void getArray<arrayType>(ref PageArray<valueType, arrayType> array)
            {
                if (Left != null)
                {
                    Left.getArray(ref array);
                    if (array.IsArray) return;
                }
                if (!array.Add(Value)) Right.getArray(ref array);
            }

            /// <summary>
            /// 获取数组
            /// </summary>
            /// <param name="array"></param>
            internal void GetDescArraySkip(ref PageArray<valueType> array)
            {
                if (Left != null)
                {
                    int count = Left.Count;
                    if (count > array.SkipCount)
                    {
                        Left.GetDescArraySkip(ref array);
                        if (array.Index != 0 && array.AddDesc(Value) != 0) Right.getDescArray(ref array);
                        return;
                    }
                    array.SkipCount -= count;
                }
                if (array.SkipCount == 0)
                {
                    if (array.AddDesc(Value) != 0) Right.getDescArray(ref array);
                    return;
                }
                --array.SkipCount;
                Right.GetDescArraySkip(ref array);
            }
            /// <summary>
            /// 获取数组
            /// </summary>
            /// <param name="array"></param>
            private void getDescArray(ref PageArray<valueType> array)
            {
                if (Left != null)
                {
                    Left.getDescArray(ref array);
                    if (array.Index == 0) return;
                }
                if (array.AddDesc(Value) != 0) Right.getDescArray(ref array);
            }
            /// <summary>
            /// 查找数据
            /// </summary>
            /// <param name="array"></param>
            internal void GetFind(ref FindArray<valueType> array)
            {
                if (Left != null) Left.GetFind(ref array);
                array.Add(Value);
                if (Right != null) Right.GetFind(ref array);
            }
        }
        /// <summary>
        /// 根节点
        /// </summary>
        internal Node Boot;
        /// <summary>
        /// 节点数据
        /// </summary>
        public int Count
        {
            get { return Boot != null ? Boot.Count : 0; }
        }
        /// <summary>
        /// 获取树高度，需要 O(n)
        /// </summary>
        public int Height
        {
            get
            {
                return Boot == null ? 0 : Boot.Height;
            }
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        internal IEnumerable<KeyValue<keyType, valueType>> KeyValues
        {
            get
            {
                return Count != 0 ? Boot.KeyValues : NullValue<KeyValue<keyType, valueType>>.Array;
            }
        }
        /// <summary>
        /// 二叉树更新版本
        /// </summary>
        public int Version { get; private set; }
        /// <summary>
        /// 更新版本重置事件
        /// </summary>
        internal event Action OnResetVersion;
        /// <summary>
        /// 根据关键字获取或者设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据,获取失败KeyNotFoundException</returns>
        public valueType this[keyType key]
        {
            get
            {
                if (Boot != null)
                {
                    Node node = Boot.Get(ref key);
                    if (node != null) return node.Value;
                }
                throw new KeyNotFoundException(key.ToString());
            }
            set { Set(ref key, value); }
        }
        /// <summary>
        /// 二叉树字典
        /// </summary>
        public Dictionary() { Version = 1; }
        /// <summary>
        /// 更新二叉树版本
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void nextVersion()
        {
            if (++Version == 0)
            {
                Version = 1;
                if (OnResetVersion != null) OnResetVersion();
            }
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Clear()
        {
            Boot = null;
            nextVersion();
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>是否添加了关键字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Set(keyType key, valueType value)
        {
            return Set(ref key, value);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>是否添加了关键字</returns>
        public bool Set(ref keyType key, valueType value)
        {
            if (Boot == null)
            {
                Boot = new Node { Key = key, Value = value, Count = 1 };
                nextVersion();
                return true;
            }
            KeyValue<keyType, valueType> keyValue = new KeyValue<keyType, valueType>(ref key, value);
            if (Boot.Set(ref keyValue))
            {
                nextVersion();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>是否添加了数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool TryAdd(keyType key, valueType value)
        {
            return TryAdd(ref key, value);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">数据</param>
        /// <returns>是否添加了数据</returns>
        public bool TryAdd(ref keyType key, valueType value)
        {
            if (Boot == null)
            {
                Boot = new Node { Key = key, Value = value, Count = 1 };
                nextVersion();
                return true;
            }
            KeyValue<keyType, valueType> keyValue = new KeyValue<keyType, valueType>(ref key, value);
            if (Boot.TryAdd(ref keyValue))
            {
                nextVersion();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否存在关键字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Remove(keyType key)
        {
            return Remove(ref key);
        }
        /// <summary>
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(ref keyType key)
        {
            if (Boot != null)
            {
                Node node = Boot.Remove(ref key);
                if (node != null)
                {
                    if (node == Boot) Boot = node.Remove();
                    nextVersion();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">被删除数据</param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(ref keyType key, out valueType value)
        {
            if (Boot != null)
            {
                Node node = Boot.Remove(ref key);
                if (node != null)
                {
                    if (node == Boot) Boot = node.Remove();
                    value = node.Value;
                    nextVersion();
                    return true;
                }
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 判断是否包含关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否包含关键字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool ContainsKey(keyType key)
        {
            return Boot != null && Boot.Get(ref key) != null;
        }
        /// <summary>
        /// 判断是否包含关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否包含关键字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool ContainsKey(ref keyType key)
        {
            return Boot != null && Boot.Get(ref key) != null;
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">目标数据</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool TryGetValue(keyType key, out valueType value)
        {
            return TryGetValue(ref key, out value);
        }
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">目标数据</param>
        /// <returns>是否成功</returns>
        public bool TryGetValue(ref keyType key, out valueType value)
        {
            if (Boot != null)
            {
                Node node = Boot.Get(ref key);
                if (node != null)
                {
                    value = node.Value;
                    return true;
                }
            }
            value = default(valueType);
            return false;
        }
        /// <summary>
        /// 根据关键字获取一个匹配节点位置
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>一个匹配节点位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int IndexOf(keyType key)
        {
            return Boot != null ? Boot.IndexOf(ref key) : -1;
        }
        /// <summary>
        /// 根据关键字获取一个匹配节点位置
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>一个匹配节点位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int IndexOf(ref keyType key)
        {
            return Boot != null ? Boot.IndexOf(ref key) : -1;
        }
        /// <summary>
        /// 根据关键字比它小的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int CountLess(ref keyType key)
        {
            return Boot != null ? Boot.CountLess(ref key) : 0;
        }
        /// <summary>
        /// 根据关键字比它大的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int CountThan(ref keyType key)
        {
            return Boot != null ? Boot.CountThan(ref key) : 0;
        }
        /// <summary>
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">节点位置</param>
        /// <returns>数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public KeyValue<keyType,valueType> At(int index)
        {
            if (Boot != null) return Boot.At(index).KeyValue;
            throw new IndexOutOfRangeException();
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="page">分页号,从 1 开始</param>
        /// <returns>分页数据</returns>
        internal valueType[] GetPage(int pageSize, int page)
        {
            if (Boot != null)
            {
                int count = Boot.Count, skipCount = pageSize * (page - 1);
                if (skipCount < count)
                {
                    PageArray<valueType> array = new PageArray<valueType> { Array = new valueType[Math.Min(count - skipCount, pageSize)], SkipCount = skipCount };
                    Boot.GetArraySkip(ref array);
                    return array.Array;
                }
            }
            return NullValue<valueType>.Array;
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="page">分页号,从 1 开始</param>
        /// <param name="getValue">获取数据委托</param>
        /// <returns>分页数据</returns>
        internal arrayType[] GetPage<arrayType>(int pageSize, int page, Func<valueType, arrayType> getValue)
        {
            if (Boot != null)
            {
                int count = Boot.Count, skipCount = pageSize * (page - 1);
                if (skipCount < count)
                {
                    PageArray<valueType, arrayType> array = new PageArray<valueType, arrayType> { Array = new arrayType[Math.Min(count - skipCount, pageSize)], SkipCount = skipCount, GetValue = getValue };
                    Boot.GetArraySkip(ref array);
                    return array.Array;
                }
            }
            return NullValue<arrayType>.Array;
        }
        /// <summary>
        /// 创建二叉树分页缓存
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="arrayPageCount">数组缓存数量</param>
        /// <param name="fifoPageCount">先进先出缓存数量</param>
        /// <returns>二叉树分页缓存</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public DictionaryPageCache<keyType, valueType> CreatePageCache(int pageSize = 10, int arrayPageCount = 16, int fifoPageCount = 16)
        {
            if (pageSize <= 0) throw new IndexOutOfRangeException();
            return new DictionaryPageCache<keyType, valueType>(this, pageSize, Math.Max(arrayPageCount, 1), Math.Max(fifoPageCount, 0));
        }
        /// <summary>
        /// 获取逆序分页数据
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="page">分页号,从 1 开始</param>
        /// <returns>分页数据</returns>
        internal valueType[] GetPageDesc(int pageSize, int page)
        {
            if (Boot != null)
            {
                int count = Boot.Count, skipCount = pageSize * (page - 1);
                if (skipCount < count)
                {
                    pageSize = Math.Min(count - skipCount, pageSize);
                    PageArray<valueType> array = new PageArray<valueType> { Array = new valueType[pageSize], SkipCount = count - (skipCount + pageSize), Index = pageSize };
                    Boot.GetDescArraySkip(ref array);
                    return array.Array;
                }
            }
            return NullValue<valueType>.Array;
        }
        /// <summary>
        /// 创建二叉树逆序分页缓存
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="arrayPageCount">数组缓存数量</param>
        /// <param name="fifoPageCount">先进先出缓存数量</param>
        /// <returns>二叉树分页缓存</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public DictionaryPageDescCache<keyType, valueType> CreatePageDescCache(int pageSize = 10, int arrayPageCount = 16, int fifoPageCount = 16)
        {
            if (pageSize <= 0) throw new IndexOutOfRangeException();
            return new DictionaryPageDescCache<keyType, valueType>(this, pageSize, Math.Max(arrayPageCount, 1), Math.Max(fifoPageCount, 0));
        }
        /// <summary>
        /// 获取范围数据集合
        /// </summary>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>数据集合</returns>
        internal valueType[] GetRange(int skipCount, int getCount)
        {
            if (Boot != null && skipCount < Boot.Count)
            {
                PageArray<valueType> array = new PageArray<valueType> { Array = new valueType[Math.Min(Boot.Count - skipCount, getCount)], SkipCount = skipCount };
                Boot.GetArraySkip(ref array);
                return array.Array;
            }
            return NullValue<valueType>.Array;
        }
        /// <summary>
        /// 获取逆序范围数据集合
        /// </summary>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        /// <returns>数据集合</returns>
        internal valueType[] GetRangeDesc(int skipCount, int getCount)
        {
            if (Boot != null && skipCount < Boot.Count)
            {
                getCount = Math.Min(Boot.Count - skipCount, getCount);
                PageArray<valueType> array = new PageArray<valueType> { Array = new valueType[getCount], SkipCount = Boot.Count - (skipCount + getCount), Index = getCount };
                Boot.GetDescArraySkip(ref array);
                return array.Array;
            }
            return NullValue<valueType>.Array;
        }
        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="isValue">数据匹配委托</param>
        /// <returns></returns>
        internal LeftArray<valueType> GetFind(Func<valueType, bool> isValue)
        {
            if (Boot != null)
            {
                FindArray<valueType> array = new FindArray<valueType> { IsValue = isValue };
                Boot.GetFind(ref array);
                return array.Array;
            }
            return default(LeftArray<valueType>);
        }
    }
}
