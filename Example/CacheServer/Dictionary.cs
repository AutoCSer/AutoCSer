using System;
using AutoCSer.CacheServer.DataStructure;
using AutoCSer.CacheServer.DataStructure.Value;

namespace AutoCSer.Example.CacheServer
{
    /// <summary>
    /// 字典测试（嵌套节点）
    /// </summary>
    internal class Dictionary
    {
        /// <summary>
        /// 字典测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.Client client)
        {
            #region 创建名称为 Dictionary 的字典缓存
            Dictionary<int, Link<int>> dictionary = client.GetOrCreateDataStructure<Dictionary<int, Link<int>>>("Dictionary").Value;
            if (dictionary == null)
            {
                return false;
            }
            #endregion

            #region 创建或者获取关键字为 1 的子节点
            Link<int> link = dictionary.GetOrCreate(1).Value;
            if (link == null)
            {
                return false;
            }
            #endregion

            #region 判断是否存在关键字为 1 的子节点
            AutoCSer.CacheServer.ReturnValue<bool> isNode = dictionary.ContainsKey(1);
            if (!isNode.Value)
            {
                return false;
            }
            #endregion

            if (!Link.TestCase(link))
            {
                return false;
            }

            #region 创建短路径
            AutoCSer.CacheServer.ShortPath.Link<int> shortPathLink = link.CreateShortPath().Value;
            if (shortPathLink == null)
            {
                return false;
            }
            #endregion

            if (!Link.TestCase(shortPathLink))
            {
                return false;
            }

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

