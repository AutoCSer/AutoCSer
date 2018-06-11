using System;
using AutoCSer.CacheServer.DataStructure;
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

            #region 获取关键字为 1 的数字更新
            AutoCSer.CacheServer.OperationUpdater.Number<int> number1 = dictionary.GetNumberUpdater(1);
            #endregion

            #region 关键字为 1 的数据 +2
            value = number1 + 2;
            if (value.Value != 11)
            {
                return false;
            }
            #endregion

            #region 关键字为 1 的数据 -5
            value = number1 - 5;
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

            #region dictionary[3] = 8;
            isSet = dictionary.Set(3, 8);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 获取逆序分页数据
            AutoCSer.CacheServer.ReturnValue<int[]> page = dictionary.GetPageDesc(1, 10);
            if (page.Value == null || page.Value.Length != 2 || page.Value[0] != 8 || page.Value[1] != 6)
            {
                return false;
            }
            #endregion

            #region 获取字典数据数量
            AutoCSer.CacheServer.ReturnValue<int> count = dictionary.Count;
            if (count.Value != 2)
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
            #endregion

            isRemove = dictionary.Remove(3);
            if (!isRemove.Value)
            {
                return false;
            }
            count = dictionary.Count;
            if (count.Type != AutoCSer.CacheServer.ReturnType.Success || count.Value != 0)
            {
                return false;
            }

            return true;
        }
    }
}

