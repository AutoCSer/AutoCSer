using System;

namespace AutoCSer.Example.CacheServer
{
    /// <summary>
    /// 位图测试（数据节点）
    /// </summary>
    internal class Bitmap
    {
        /// <summary>
        /// 位图测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.Client client)
        {
            #region 创建名称为 Bitmap 的位图缓存
            AutoCSer.CacheServer.DataStructure.Bitmap bitmap = client.GetOrCreateDataStructure<AutoCSer.CacheServer.DataStructure.Bitmap>("Bitmap").Value;
            if (bitmap == null)
            {
                return false;
            }
            #endregion

            #region 设置第 3 个位，返回设置前的位状态 false
            AutoCSer.CacheServer.ReturnValue<bool> bit = bitmap.Set(3);
            if (bit.Type != AutoCSer.CacheServer.ReturnType.Success || bit.Value)
            {
                return false;
            }
            #endregion

            #region 第 5 个位取反，返回取反后的结果 true
            bit = bitmap.SetNegate(5);
            if (!bit.Value)
            {
                return false;
            }
            #endregion

            #region 获取第 3 个位，结果为 true
            bit = bitmap.Get(3);
            if (!bit.Value)
            {
                return false;
            }
            #endregion

            #region 清除第 5 个位，返回清除前的位状态 true
            bit = bitmap.Clear(5);
            if (!bit.Value)
            {
                return false;
            }
            #endregion

            #region 第 3 个位取反，返回取反后的结果 false
            bit = bitmap.SetNegate(3);
            if (bit.Type != AutoCSer.CacheServer.ReturnType.Success || bit.Value)
            {
                return false;
            }
            #endregion

            #region 获取第 5 个位，结果为 false
            bit = bitmap.Get(5);
            if (bit.Type != AutoCSer.CacheServer.ReturnType.Success || bit.Value)
            {
                return false;
            }
            #endregion

            return true;
        }
    }
}
