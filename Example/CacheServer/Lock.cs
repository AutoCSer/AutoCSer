using System;

namespace AutoCSer.Example.CacheServer
{
    /// <summary>
    /// 锁测试（数据节点）
    /// </summary>
    internal class Lock
    {
        /// <summary>
        /// 锁测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static bool TestCase(AutoCSer.CacheServer.Client client)
        {
            #region 创建名称为 Lock 的锁缓存
            AutoCSer.CacheServer.DataStructure.Lock Lock = client.GetOrCreateDataStructure<AutoCSer.CacheServer.DataStructure.Lock>("Lock").Value;
            if (Lock == null)
            {
                return false;
            }
            #endregion

            #region 同步等待申请锁，锁超时为 5 秒
            AutoCSer.CacheServer.ReturnValue<AutoCSer.CacheServer.Lock.Manager> manager = Lock.GetEnter(5 * 1000);
            if (manager.Value == null)
            {
                return false;
            }
            #endregion

            using (manager.Value)
            {
                #region 尝试申请锁
                AutoCSer.CacheServer.ReturnValue<AutoCSer.CacheServer.Lock.Manager> tryManager = Lock.GetTryEnter(5 * 1000);
                if (tryManager.Type != AutoCSer.CacheServer.ReturnType.Locked)
                {
                    return false;
                }
                #endregion
            }

            manager = Lock.GetTryEnter(5 * 1000);
            if (manager.Value == null)
            {
                return false;
            }
            #region 释放锁，使用 using 也可以
            manager.Value.Exit();
            #endregion

            return true;
        }
    }
}
