using System;
using AutoCSer.Extension;
using System.Diagnostics;

namespace AutoCSer.Net.TcpStaticServer
{
    /// <summary>
    /// 时间验证
    /// </summary>
    /// <typeparam name="verifyType">验证类型</typeparam>
    public abstract class TimeVerify<verifyType>
        where verifyType : TimeVerify<verifyType>
    {
        /// <summary>
        /// 验证时间戳
        /// </summary>
        private static TcpServer.TimeVerifyTick timeVerifyTick = new TcpServer.TimeVerifyTick(Date.NowTime.UtcNow.Ticks - 1);
        /// <summary>
        /// 时间验证函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="userID">用户ID</param>
        /// <param name="randomPrefix">随机前缀</param>
        /// <param name="md5Data">MD5 数据</param>
        /// <param name="ticks">验证时钟周期</param>
        /// <returns>是否验证成功</returns>
        [Method(IsVerifyMethod = true, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected static bool verify(TcpInternalServer.ServerSocketSender sender, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks)
        {
            TcpServer.ServerBase server = sender.Server;
            return server.CheckVerifyString() || verify(sender, randomPrefix, server.Attribute.VerifyString, md5Data, ref ticks);
        }
        /// <summary>
        /// 时间验证函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="randomPrefix">随机前缀</param>
        /// <param name="verifyString">验证字符串</param>
        /// <param name="md5Data">MD5 数据</param>
        /// <param name="ticks">验证时钟周期</param>
        /// <returns>是否验证成功</returns>
        protected static bool verify(TcpInternalServer.ServerSocketSender sender, ulong randomPrefix, string verifyString, byte[] md5Data, ref long ticks)
        {
            if (md5Data != null && md5Data.Length == 16)
            {
                if (!timeVerifyTick.Check(ref ticks, ref sender.TimeVerifyTicks)) return false;
                if (TcpServer.TimeVerifyServer.IsMd5(TcpServer.TimeVerifyServer.Md5(verifyString, randomPrefix, ticks), md5Data) == 0)
                {
                    timeVerifyTick.Set(ticks);
                    if (!sender.Server.Attribute.IsMarkData || sender.SetMarkData(sender.Server.ServerAttribute.VerifyHashCode ^ randomPrefix)) return true;
                }
            }
            ticks = 0;
            return false;
        }

    }
}
