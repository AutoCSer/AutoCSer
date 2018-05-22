using System;
using AutoCSer.CacheServer.DataStructure;
using AutoCSer.CacheServer.DataStructure.Value;

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
        internal static bool TestCase(AutoCSer.CacheServer.MasterClient client)
        {
            #region 创建名称为 ValueArray 的数组缓存
            ValueArray<Value<int>> array = client.GetOrCreateDataStructure<ValueArray<Value<int>>>("ValueArray").Value;
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
        internal static bool TestCase(ValueArray<Value<int>> array)
        {

            #region array[1] = 9;
            AutoCSer.CacheServer.ReturnValue<bool> isSet = array.Set(1, 9);
            if (!isSet.Value)
            {
                return false;
            }
            #endregion

            #region 获取索引为 1 的数据
            AutoCSer.CacheServer.ReturnValueNode<Value<int>> value = array.Get(1);
            if (value.Get().Value != 9)
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
