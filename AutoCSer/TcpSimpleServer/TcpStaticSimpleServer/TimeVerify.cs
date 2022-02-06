using System;
using System.Diagnostics;
using AutoCSer.Extensions;
using System.Security.Cryptography;

namespace AutoCSer.Net.TcpStaticSimpleServer
{
    /// <summary>
    /// 时间验证
    /// </summary>
    /// <typeparam name="verifyType">验证类型</typeparam>
    public abstract class TimeVerify<verifyType>
        where verifyType : TimeVerify<verifyType>
    {
        /// <summary>
        /// MD5 加密
        /// </summary>
        private static MD5CryptoServiceProvider md5;
        /// <summary>
        /// 验证时间戳
        /// </summary>
        private static TcpServer.TimeVerifyTick timeVerifyTick = new TcpServer.TimeVerifyTick(AutoCSer.Threading.SecondTimer.UtcNow.Ticks - 1);
        /// <summary>
        /// 时间验证函数
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="randomPrefix">随机前缀</param>
        /// <param name="md5Data">MD5 数据</param>
        /// <param name="ticks">验证时钟周期</param>
        /// <returns>是否验证成功</returns>
        [Method(IsVerifyMethod = true, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected static bool verify(TcpInternalSimpleServer.ServerSocket socket, ulong randomPrefix, byte[] md5Data, ref long ticks)
        {
            TcpServer.ServerBase server = socket.Server;
            if (server.CheckVerifyString()) return true;
            if (md5Data != null && md5Data.Length == 16)
            {
                if (!timeVerifyTick.Check(ref ticks, ref socket.TimeVerifyTicks)) return false;
                TcpServer.ServerBaseAttribute attribute = server.Attribute;
                if (md5 == null) md5 = new MD5CryptoServiceProvider();
                if (TcpServer.TimeVerifyServer.IsMd5(TcpServer.TimeVerifyServer.Md5(md5, attribute.VerifyString, randomPrefix, ticks), md5Data) == 0)
                {
                    timeVerifyTick.Set(ticks);
                    if (attribute.IsMarkData) socket.MarkData = server.ServerAttribute.VerifyHashCode ^ randomPrefix;
                    return true;
                }
            }
            ticks = 0;
            return false;
        }
    }
}
