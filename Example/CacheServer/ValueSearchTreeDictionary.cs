using System;
using AutoCSer.CacheServer.DataStructure;
using AutoCSer.CacheServer.DataStructure.Value;
using AutoCSer.Extension;

namespace AutoCSer.Example.CacheServer
{
    /// <summary>
    /// 搜索树字典测试（数据节点）
    /// </summary>
    internal class ValueSearchTreeDictionary
    {
        /// <summary>
        /// 搜索树字典测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.Client client)
        {
            #region 创建名称为 ValueSearchTreeDictionary 的搜索树字典缓存
            ValueSearchTreeDictionary<int, int> dictionary = client.GetOrCreateDataStructure<ValueSearchTreeDictionary<int, int>>("ValueSearchTreeDictionary").Value;
            if (dictionary == null)
            {
                return false;
            }
            #endregion

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

