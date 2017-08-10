using System;
using System.Threading;

namespace AutoCSer.TestCase.SqlTableWeb
{
    /// <summary>
    /// web视图服务
    /// </summary>
    public partial class WebServer
    {
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="registerServer"></param>
        /// <param name="domains"></param>
        /// <param name="onStop"></param>
        /// <returns></returns>
        public override bool Start(Net.HttpRegister.Server registerServer, Net.HttpRegister.Domain[] domains, Action onStop)
        {
            if (base.Start(registerServer, domains, onStop))
            {
                AutoCSer.Threading.ThreadPool.TinyBackground.Start(loadSqlClientCache);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否已经加载数据库客户端缓存
        /// </summary>
        private volatile int isSqlClientCache;
        /// <summary>
        /// 数据库客户端缓存初始化
        /// </summary>
        private void loadSqlClientCache()
        {
            if (Interlocked.CompareExchange(ref isSqlClientCache, 1, 0) == 0)
            {
                SqlTableCacheServer.ClientCache.Class = SqlTableCacheServer.ClientCache.Class.CreateNull(typeof(SqlTableCacheServer.TcpCall.Class));
                SqlTableCacheServer.ClientCache.Student = SqlTableCacheServer.ClientCache.Student.CreateNull(typeof(SqlTableCacheServer.TcpCall.Student));
            }
        }
    }
}
