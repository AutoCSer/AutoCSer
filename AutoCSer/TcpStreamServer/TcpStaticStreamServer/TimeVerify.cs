using System;

namespace AutoCSer.Net.TcpStaticStreamServer
{
    /// <summary>
    /// 时间验证
    /// </summary>
    /// <typeparam name="verifyType">验证类型</typeparam>
    public abstract class TimeVerify<verifyType>
        where verifyType : TimeVerify<verifyType>
    {
        /// <summary>
        /// 最后一次验证时间
        /// </summary>
        private static long lastVerifyTicks = Date.NowTime.UtcNow.Ticks - 1;
        /// <summary>
        /// 最后一次验证时间访问锁
        /// </summary>
        private static int lastVerifyTickLock;
        /// <summary>
        /// 时间验证函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="randomPrefix">随机前缀</param>
        /// <param name="md5Data">MD5 数据</param>
        /// <param name="ticks">验证时钟周期</param>
        /// <returns>是否验证成功</returns>
        [Method(IsVerifyMethod = true, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected static bool verify(TcpInternalStreamServer.ServerSocketSender sender, ulong randomPrefix, byte[] md5Data, ref long ticks)
        {
            TcpServer.ServerBase<TcpInternalStreamServer.ServerAttribute> server = sender.Server;
            TcpServer.ServerBaseAttribute attribute = server.Attribute;
            if (TcpServer.TimeVerifyServer.CheckVerifyString(server, attribute)) return true;
            if (md5Data != null && md5Data.Length == 16)
            {
                if (ticks <= lastVerifyTicks && ticks != sender.TimeVerifyTicks)
                {
                    if (sender.TimeVerifyTicks == 0)
                    {
                        while (System.Threading.Interlocked.CompareExchange(ref lastVerifyTickLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TimeVerifyServerSetTicks);
                        sender.TimeVerifyTicks = ++lastVerifyTicks;
                        System.Threading.Interlocked.Exchange(ref lastVerifyTickLock, 0);
                    }
                    ticks = sender.TimeVerifyTicks;
                    return false;
                }
                if (TcpServer.TimeVerifyServer.IsMd5(TcpServer.TimeVerifyServer.Md5(attribute.VerifyString, randomPrefix, ticks), md5Data) == 0)
                {
                    if (ticks > lastVerifyTicks)
                    {
                        while (System.Threading.Interlocked.CompareExchange(ref lastVerifyTickLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.TimeVerifyServerSetTicks);
                        if (ticks > lastVerifyTicks) lastVerifyTicks = ticks;
                        System.Threading.Interlocked.Exchange(ref lastVerifyTickLock, 0);
                    }
                    if (!attribute.IsMarkData || sender.SetMarkData(attribute.VerifyHashCode ^ randomPrefix)) return true;
                }
            }
            ticks = 0;
            return false;
        }
    }
}
