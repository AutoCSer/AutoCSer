using System;
using AutoCSer.CacheServer.DataStructure;
using AutoCSer.CacheServer.DataStructure.Value;

namespace AutoCSer.Example.CacheServer
{
    /// <summary>
    /// 数组测试（嵌套节点）
    /// </summary>
    internal class Array
    {
        /// <summary>
        /// 数组测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.Client client)
        {
            #region 创建名称为 Array 的数组缓存
            Array<ValueDictionary<int, int>> array = client.GetOrCreateDataStructure<Array<ValueDictionary<int, int>>>("Array").Value;
            if (array == null)
            {
                return false;
            }
            #endregion

            #region 创建或者获取子节点
            ValueDictionary<int, int> dictionary = array.GetOrCreate(1).Value;
            if (dictionary == null)
            {
                return false;
            }
            #endregion

            #region 判断第一个节点是否可用
            AutoCSer.CacheServer.ReturnValue<bool> isNode = array.IsNode(1);
            if (!isNode.Value)
            {
                return false;
            }
            #endregion

            if (!ValueDictionary.TestCase(dictionary))
            {
                return false;
            }

            #region 创建短路径
            AutoCSer.CacheServer.ShortPath.Dictionary<int, int> shortPathDictionary = dictionary.CreateShortPath().Value;
            if (shortPathDictionary == null)
            {
                return false;
            }
            #endregion

            if (!ValueDictionary.TestCase(shortPathDictionary))
            {
                return false;
            }

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
