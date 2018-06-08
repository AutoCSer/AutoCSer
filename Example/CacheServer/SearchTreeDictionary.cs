using System;
using AutoCSer.CacheServer.DataStructure;
using AutoCSer.CacheServer.DataStructure.Value;

namespace AutoCSer.Example.CacheServer
{
    /// <summary>
    /// 搜索树字典测试（嵌套节点）
    /// </summary>
    internal class SearchTreeDictionary
    {
        /// <summary>
        /// 搜索树字典测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.Client client)
        {
            #region 创建名称为 SearchTreeDictionary 的搜索树字典缓存
            SearchTreeDictionary<int, ValueArray<int>> dictionary = client.GetOrCreateDataStructure<SearchTreeDictionary<int, ValueArray<int>>>("SearchTreeDictionary").Value;
            if (dictionary == null)
            {
                return false;
            }
            #endregion

            #region 创建或者获取关键字为 1 的子节点
            ValueArray<int> array = dictionary.GetOrCreate(1).Value;
            if (array == null)
            {
                return false;
            }
            #endregion

            if (!ValueArray.TestCase(array))
            {
                return false;
            }

            #region 创建短路径
            AutoCSer.CacheServer.ShortPath.Array<int> shortPathArray = array.CreateShortPath().Value;
            if (shortPathArray == null)
            {
                return false;
            }
            #endregion

            if (!ValueArray.TestCase(shortPathArray))
            {
                return false;
            }

            #region 判断关键字 1 是否存在
            AutoCSer.CacheServer.ReturnValue<bool> isKey = dictionary.ContainsKey(1);
            if (!isKey.Value)
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

