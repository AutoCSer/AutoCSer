using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 二叉树集合
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public sealed class Set<keyType> where keyType : IComparable<keyType>
    {
        /// <summary>
        /// 二叉树节点
        /// </summary>
        private sealed class Node : Node<Node, keyType>
        {
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
            /// 添加数据
            /// </summary>
            /// <param name="key"></param>
            /// <returns>是否添加了数据</returns>
            internal bool Add(ref keyType key)
            {
                int cmp = key.CompareTo(Key);
                if (cmp == 0) return false;
                if (cmp < 0)
                {
                    if (Left == null)
                    {
                        Left = new Node { Key = key, Count = 1 };
                        ++Count;
                        return true;
                    }
                    if (Left.Add(ref key))
                    {
                        checkLeft();
                        return true;
                    }
                    return false;
                }
                if (Right == null)
                {
                    Right = new Node { Key = key, Count = 1 };
                    ++Count;
                    return true;
                }
                if (Right.Add(ref key))
                {
                    checkRight();
                    return true;
                }
                return false;
            }
            /// <summary>
            /// 交换节点数据
            /// </summary>
            /// <param name="key"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            private void changeKey(ref keyType key)
            {
                keyType tempKey = key;
                key = Key;
                Key = tempKey;
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
                                leftRight.changeKey(ref Key);
                                Right = leftRight;
                            }
                        }
                        else if (Left.Right != null)
                        {
                            Node left = Left;
                            Left = left.rightToLeft(Right);
                            left.changeKey(ref Key);
                            Right = left;
                        }
                    }
                }
                else
                {
                    checkLeftRight();
                    Right.changeKey(ref Key);
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
                                rightLeft.changeKey(ref Key);
                                Left = rightLeft;
                            }
                        }
                        else if (Right.Left != null)
                        {
                            Node right = Right;
                            Right = right.leftToRight(Left);
                            right.changeKey(ref Key);
                            Left = right;
                        }
                    }
                }
                else
                {
                    checkRightLeft();
                    Left.changeKey(ref Key);
                }
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
        }
        /// <summary>
        /// 根节点
        /// </summary>
        private Node boot;
        /// <summary>
        /// 节点数据
        /// </summary>
        public int Count
        {
            get { return boot != null ? boot.Count : 0; }
        }
        /// <summary>
        /// 获取树高度，需要 O(n)
        /// </summary>
        public int Height
        {
            get
            {
                return boot == null ? 0 : boot.Height;
            }
        }
        /// <summary>
        /// 二叉树集合
        /// </summary>
        public Set() { }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Clear()
        {
            boot = null;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否添加了数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Add(keyType key)
        {
            return Add(ref key);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否添加了数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Add(ref keyType key)
        {
            if (boot == null)
            {
                boot = new Node { Key = key, Count = 1 };
                return true;
            }
            return boot.Add(ref key);
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
            if (boot != null)
            {
                Node node = boot.Remove(ref key);
                if (node != null)
                {
                    if (node == boot) boot = node.Remove();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断是否包含关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否包含关键字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Contains(keyType key)
        {
            return boot != null && boot.Get(ref key) != null;
        }
        /// <summary>
        /// 判断是否包含关键字
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>是否包含关键字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Contains(ref keyType key)
        {
            return boot != null && boot.Get(ref key) != null;
        }
        /// <summary>
        /// 根据关键字获取一个匹配节点位置
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>一个匹配节点位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int IndexOf(keyType key)
        {
            return boot != null ? boot.IndexOf(ref key) : -1;
        }
        /// <summary>
        /// 根据关键字获取一个匹配节点位置
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>一个匹配节点位置,失败返回-1</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int IndexOf(ref keyType key)
        {
            return boot != null ? boot.IndexOf(ref key) : -1;
        }
        /// <summary>
        /// 根据关键字比它小的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int CountLess(ref keyType key)
        {
            return boot != null ? boot.CountLess(ref key) : 0;
        }
        /// <summary>
        /// 根据关键字比它大的节点数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>节点数量</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int CountThan(ref keyType key)
        {
            return boot != null ? boot.CountThan(ref key) : 0;
        }
        /// <summary>
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">节点位置</param>
        /// <returns>数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public keyType At(int index)
        {
            if (boot != null) return boot.At(index).Key;
            throw new IndexOutOfRangeException();
        }
    }
}
