using System;
using AutoCSer.CacheServer.DataStructure;
using AutoCSer.Extension;

namespace AutoCSer.Example.CacheServer
{
    /// <summary>
    /// 最小堆测试（数据节点）
    /// </summary>
    internal class Heap
    {
        /// <summary>
        /// 最小堆测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.Client client)
        {
            #region 创建名称为 Heap 的最小堆缓存
            Heap<int, int> heap = client.GetOrCreateDataStructure<Heap<int, int>>("Heap").Value;
            if (heap == null)
            {
                return false;
            }
            #endregion

            #region 添加关键字为 6 值为 9 的数据
            AutoCSer.CacheServer.ReturnValue<bool> isSet = heap.Push(6, 9);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            isSet = heap.Push(2, 8);
            if (!isSet.Value)
            {
                return false;
            }

            isSet = heap.Push(4, 7);
            if (!isSet.Value)
            {
                return false;
            }

            #region 获取数据数量
            AutoCSer.CacheServer.ReturnValue<int> count = heap.Count;
            if (count.Value != 3)
            {
                return false;
            }
            #endregion

            #region 获取堆顶关键字
            AutoCSer.CacheServer.ReturnValue<int> key = heap.GetTopKey();
            if (key.Value != 2)
            {
                return false;
            }
            #endregion

            #region 获取堆顶数据
            AutoCSer.CacheServer.ReturnValue<int> value = heap.GetTopValue();
            if (value.Value != 8)
            {
                return false;
            }
            #endregion

            #region 删除堆顶数据
            isSet = heap.PopTop();
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 删除堆顶数据并返回关键字
            key = heap.GetPopTopKey();
            if (key.Value != 4)
            {
                return false;
            }
            #endregion

            #region 删除堆顶数据并返回数据
            value = heap.GetPopTopValue();
            if (value.Value != 9)
            {
                return false;
            }
            #endregion

            count = heap.Count;
            if (count.Type != AutoCSer.CacheServer.ReturnType.Success || count.Value != 0)
            {
                return false;
            }

            return true;
        }
    }
}

