using System;
using AutoCSer.CacheServer.DataStructure;
using AutoCSer.Extension;

namespace AutoCSer.Example.CacheServer
{
    /// <summary>
    /// 数组测试（数据节点）
    /// </summary>
    internal class ValueArray
    {
        /// <summary>
        /// 数组测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.Client client)
        {
            #region 创建名称为 ValueArray 的数组缓存
            ValueArray<int> array = client.GetOrCreateDataStructure<ValueArray<int>>("ValueArray").Value;
            if (array == null)
            {
                return false;
            }
            #endregion
            return TestCase(array);
        }
        /// <summary>
        /// 数组测试
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        internal static bool TestCase(ValueArray<int> array)
        {

            #region array[1] = 9;
            AutoCSer.CacheServer.ReturnValue<bool> isSet = array.Set(1, 9);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 获取索引为 1 的数据
            AutoCSer.CacheServer.ReturnValue<int> value = array.Get(1);
            if (value.Value != 9)
            {
                return false;
            }
            #endregion

            #region 获取索引为 1 的数字更新
            AutoCSer.CacheServer.OperationUpdater.Number<int> number1 = array.GetNumberUpdater(1);
            #endregion

            #region 索引为 1 的数据 +2
            value = number1 + 2;
            if (value.Value != 11)
            {
                return false;
            }
            #endregion

            #region 索引为 1 的数据 -5
            value = number1 - 5;
            if (value.Value != 6)
            {
                return false;
            }
            #endregion

            #region 获取索引为 1 的数据
            value = array.Get(1);
            if (value.Value != 6)
            {
                return false;
            }
            #endregion

            #region 如果索引为 1 的数据小于 10 则 +5
            value = number1.Add(5, number1 < 10);
            if (value.Value != 11)
            {
                return false;
            }
            #endregion

            #region 如果索引为 1 的数据小于 10 则 +5
            value = number1.Add(5, number1 < 10);
            if (value.Value != 11)
            {
                return false;
            }
            #endregion

            #region 获取索引为 1 的数据
            value = array.Get(1);
            if (value.Value != 11)
            {
                return false;
            }
            #endregion

            #region 获取数组已使用长度
            AutoCSer.CacheServer.ReturnValue<int> count = array.Count;
            if (count.Value != 2)
            {
                return false;
            }
            #endregion

            #region 清除所有数据
            AutoCSer.CacheServer.ReturnValue<bool> isClear = array.Clear();
            if (!isClear.Value)
            {
                return false;
            }
            count = array.Count;
            if (count.Type != AutoCSer.CacheServer.ReturnType.Success || count.Value != 0)
            {
                return false;
            }
            #endregion

            return true;
        }
        /// <summary>
        /// 数组测试（短路径）
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.ShortPath.Array<int> array)
        {

            #region array[1] = 9;
            AutoCSer.CacheServer.ReturnValue<bool> isSet = array.Set(1, 9);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 获取索引为 1 的数据
            AutoCSer.CacheServer.ReturnValue<int> value = array.Get(1);
            if (value.Value != 9)
            {
                return false;
            }
            #endregion

            #region 获取数组已使用长度
            AutoCSer.CacheServer.ReturnValue<int> count = array.Count;
            if (count.Value != 2)
            {
                return false;
            }
            #endregion

            #region 清除所有数据
            AutoCSer.CacheServer.ReturnValue<bool> isClear = array.Clear();
            if (!isClear.Value)
            {
                return false;
            }
            count = array.Count;
            if (count.Type != AutoCSer.CacheServer.ReturnType.Success || count.Value != 0)
            {
                return false;
            }
            #endregion

            return true;
        }
    }
}
