using System;
using AutoCSer.CacheServer.DataStructure;
using AutoCSer.Extension;

namespace AutoCSer.Example.CacheServer
{
    /// <summary>
    /// 字典测试（数据节点）
    /// </summary>
    internal class ValueDictionary
    {
        /// <summary>
        /// 字典测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.Client client)
        {
            #region 创建名称为 ValueDictionary 的字典缓存
            ValueDictionary<int, int> dictionary = client.GetOrCreateDataStructure<ValueDictionary<int, int>>("ValueDictionary").Value;
            if (dictionary == null)
            {
                return false;
            }
            #endregion

            return TestCase(dictionary);
        }
        /// <summary>
        /// 字典测试
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        internal static bool TestCase(ValueDictionary<int, int> dictionary)
        {
            #region dictionary[1] = 9;
            AutoCSer.CacheServer.ReturnValue<bool> isSet = dictionary.Set(1, 9);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 判断关键字 1 是否存在
            AutoCSer.CacheServer.ReturnValue<bool> isKey = dictionary.ContainsKey(1);
            if (!isKey.Value)
            {
                return false;
            }
            #endregion

            #region 获取关键字为 1 的数据
            AutoCSer.CacheServer.ReturnValue<int> value = dictionary.Get(1);
            if (value.Value != 9)
            {
                return false;
            }
            #endregion

            #region 获取关键字为 1 的数字更新
            AutoCSer.CacheServer.OperationUpdater.Integer<int> integer = dictionary.GetIntegerUpdater(1);
            #endregion

            #region 关键字为 1 的数据 ^2
            value = integer ^ 2;
            if (value.Value != 11)
            {
                return false;
            }
            #endregion

            #region 关键字为 1 的数据 &7
            value = integer & 7;
            if (value.Value != 3)
            {
                return false;
            }
            #endregion

            #region 关键字为 1 的数据 +3
            value = integer.Number + 3;
            if (value.Value != 6)
            {
                return false;
            }
            #endregion

            #region 获取关键字为 1 的数据
            value = dictionary.Get(1);
            if (value.Value != 6)
            {
                return false;
            }
            #endregion

            #region 获取字典数据数量
            AutoCSer.CacheServer.ReturnValue<int> count = dictionary.Count;
            if (count.Value != 1)
            {
                return false;
            }
            #endregion

            #region 删除关键字为 1 的数据
            AutoCSer.CacheServer.ReturnValue<bool> isRemove = dictionary.Remove(1);
            if (!isRemove.Value)
            {
                return false;
            }
            count = dictionary.Count;
            if (count.Type != AutoCSer.CacheServer.ReturnType.Success || count.Value != 0)
            {
                return false;
            }
            #endregion

            return true;
        }
        /// <summary>
        /// 字典测试（短路径）
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.ShortPath.Dictionary<int, int> dictionary)
        {
            #region dictionary[1] = 9;
            AutoCSer.CacheServer.ReturnValue<bool> isSet = dictionary.Set(1, 9);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 判断关键字 1 是否存在
            AutoCSer.CacheServer.ReturnValue<bool> isKey = dictionary.ContainsKey(1);
            if (!isKey.Value)
            {
                return false;
            }
            #endregion

            #region 获取关键字为 1 的数据
            AutoCSer.CacheServer.ReturnValue<int> value = dictionary.Get(1);
            if (value.Value != 9)
            {
                return false;
            }
            #endregion

            #region 获取字典数据数量
            AutoCSer.CacheServer.ReturnValue<int> count = dictionary.Count;
            if (count.Value != 1)
            {
                return false;
            }
            #endregion

            #region 删除关键字为 1 的数据
            AutoCSer.CacheServer.ReturnValue<bool> isRemove = dictionary.Remove(1);
            if (!isRemove.Value)
            {
                return false;
            }
            count = dictionary.Count;
            if (count.Type != AutoCSer.CacheServer.ReturnType.Success || count.Value != 0)
            {
                return false;
            }
            #endregion

            return true;
        }
    }
}

