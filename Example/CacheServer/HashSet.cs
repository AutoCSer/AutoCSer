using System;
using AutoCSer.CacheServer.DataStructure;
using AutoCSer.CacheServer.DataStructure.Value;

namespace AutoCSer.Example.CacheServer
{
    /// <summary>
    /// 哈希表测试（数据节点）
    /// </summary>
    internal class HashSet
    {
        /// <summary>
        /// 哈希表测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.Client client)
        {
            #region 创建名称为 HashSet 的哈希表缓存
            HashSet<int> hashSet = client.GetOrCreateDataStructure<HashSet<int>>("HashSet").Value;
            if (hashSet == null)
            {
                return false;
            }
            #endregion

            #region 添加值为 9 的数据
            AutoCSer.CacheServer.ReturnValue<bool> isAdd = hashSet.Add(9);
            if (!isAdd.Value)
            {
                return false;
            }
            #endregion

            #region 判断是否存在值为 9 的数据
            AutoCSer.CacheServer.ReturnValue<bool> isValue = hashSet.Contains(9);
            if (!isValue.Value)
            {
                return false;
            }
            #endregion

            #region 获取哈希表数据数量
            AutoCSer.CacheServer.ReturnValue<int> count = hashSet.Count;
            if (count.Value != 1)
            {
                return false;
            }
            #endregion

            #region 删除值为 9 的数据
            AutoCSer.CacheServer.ReturnValue<bool> isRemove = hashSet.Remove(9);
            if (!isRemove.Value)
            {
                return false;
            }
            count = hashSet.Count;
            if (count.Type != AutoCSer.CacheServer.ReturnType.Success || count.Value != 0)
            {
                return false;
            }
            #endregion

            return true;
        }
    }
}
