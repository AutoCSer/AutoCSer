using System;
using AutoCSer.CacheServer.DataStructure;
using AutoCSer.CacheServer.DataStructure.Value;
using AutoCSer.Extension;

namespace AutoCSer.Example.CacheServer
{
    /// <summary>
    /// 链表测试（数据节点）
    /// </summary>
    internal class Link
    {
        /// <summary>
        /// 链表测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.Client client)
        {
            #region 创建名称为 Link 的链表缓存
            Link<int> link = client.GetOrCreateDataStructure<Link<int>>("Link").Value;
            if (link == null)
            {
                return false;
            }
            #endregion
            return TestCase(link);
        }
        /// <summary>
        /// 链表测试
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        internal static bool TestCase(Link<int> link)
        {
            #region 链表尾部追加数据 3，链表结果为 3
            AutoCSer.CacheServer.ReturnValue<bool> isSet = link.Append(3);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 在第 1 个元素之前插入数据 1，链表结果为 1 -> 3
            isSet = link.InsertBefore(0, 1);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 在倒数第 1 个元素之前插入数据 1，链表结果为 1 -> 2 -> 3
            isSet = link.InsertBefore(-1, 2);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 在倒数第 1 个元素之后插入数据 5，链表结果为 1 -> 2 -> 3 -> 5
            isSet = link.InsertAfter(-1, 5);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 在第 3 个元素之后插入数据 4，链表结果为 1 -> 2 -> 3 -> 4 -> 5
            isSet = link.InsertAfter(2, 4);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 获取链表长度
            AutoCSer.CacheServer.ReturnValue<int> count = link.Count;
            if (count.Value != 5)
            {
                return false;
            }
            #endregion

            #region 获取第 3 个数据
            AutoCSer.CacheServer.ReturnValue<int> value = link.Get(2);
            if (value.Value != 3)
            {
                return false;
            }
            #endregion

            #region 获取倒数第 2 个数据
            value = link.Get(-2);
            if (value.Value != 4)
            {
                return false;
            }
            #endregion

            #region 弹出第一个数据，链表结果为 2 -> 3 -> 4 -> 5
            value = link.Dequeue();
            if (value.Value != 1)
            {
                return false;
            }
            #endregion

            #region 弹出最后一个数据，链表结果为 2 -> 3 -> 4
            value = link.StackPop();
            if (value.Value != 5)
            {
                return false;
            }
            #endregion

            #region 获取并删除第 2 个数据，链表结果为 2 -> 4
            value = link.GetRemove(1);
            if (value.Value != 3)
            {
                return false;
            }
            #endregion

            #region 删除倒数第 2 个数据，链表结果为 4
            AutoCSer.CacheServer.ReturnValue<bool> isRemove = link.Remove(-2);
            if (!isRemove.Value)
            {
                return false;
            }
            #endregion

            count = link.Count;
            if (count.Value != 1)
            {
                return false;
            }
            isRemove = link.Clear();
            if (!isRemove.Value)
            {
                return false;
            }
            count = link.Count;
            if (count.Type != AutoCSer.CacheServer.ReturnType.Success || count.Value != 0)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// 链表测试（短路径）
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.ShortPath.Link<int> link)
        {
            #region 链表尾部追加数据 3，链表结果为 3
            AutoCSer.CacheServer.ReturnValue<bool> isSet = link.Append(3);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 在第 1 个元素之前插入数据 1，链表结果为 1 -> 3
            isSet = link.InsertBefore(0, 1);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 在倒数第 1 个元素之前插入数据 1，链表结果为 1 -> 2 -> 3
            isSet = link.InsertBefore(-1, 2);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 在倒数第 1 个元素之后插入数据 5，链表结果为 1 -> 2 -> 3 -> 5
            isSet = link.InsertAfter(-1, 5);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 在第 3 个元素之后插入数据 4，链表结果为 1 -> 2 -> 3 -> 4 -> 5
            isSet = link.InsertAfter(2, 4);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 获取链表长度
            AutoCSer.CacheServer.ReturnValue<int> count = link.Count;
            if (count.Value != 5)
            {
                return false;
            }
            #endregion

            #region 获取第 3 个数据
            AutoCSer.CacheServer.ReturnValue<int> value = link.Get(2);
            if (value.Value != 3)
            {
                return false;
            }
            #endregion

            #region 获取倒数第 2 个数据
            value = link.Get(-2);
            if (value.Value != 4)
            {
                return false;
            }
            #endregion

            #region 弹出第一个数据，链表结果为 2 -> 3 -> 4 -> 5
            value = link.Dequeue();
            if (value.Value != 1)
            {
                return false;
            }
            #endregion

            #region 弹出最后一个数据，链表结果为 2 -> 3 -> 4
            value = link.StackPop();
            if (value.Value != 5)
            {
                return false;
            }
            #endregion

            #region 获取并删除第 2 个数据，链表结果为 2 -> 4
            value = link.GetRemove(1);
            if (value.Value != 3)
            {
                return false;
            }
            #endregion

            #region 删除倒数第 2 个数据，链表结果为 4
            AutoCSer.CacheServer.ReturnValue<bool> isRemove = link.Remove(-2);
            if (!isRemove.Value)
            {
                return false;
            }
            #endregion

            count = link.Count;
            if (count.Value != 1)
            {
                return false;
            }
            isRemove = link.Clear();
            if (!isRemove.Value)
            {
                return false;
            }
            count = link.Count;
            if (count.Type != AutoCSer.CacheServer.ReturnType.Success || count.Value != 0)
            {
                return false;
            }

            return true;
        }
    }
}

