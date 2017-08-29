using System;
using AutoCSer.Extension;

namespace AutoCSer.Net.TcpOpenSimpleServer
{
    /// <summary>
    /// 时间验证服务客户端
    /// </summary>
    public static class TimeVerifyClient
    {
        /// <summary>
        /// 时间验证服务客户端委托
        /// </summary>
        /// <param name="randomPrefix">随机前缀</param>
        /// <param name="md5Data">MD5 数据</param>
        /// <param name="ticks">验证时钟周期</param>
        /// <returns></returns>
        public delegate TcpServer.ReturnValue<bool> Verifier(ulong randomPrefix, byte[] md5Data, ref long ticks);
        /// <summary>
        /// 时间验证客户端验证
        /// </summary>
        /// <param name="verify">时间验证服务客户端委托</param>
        /// <param name="client"></param>
        /// <returns></returns>
        public unsafe static bool Verify(Verifier verify, AutoCSer.Net.TcpOpenSimpleServer.Client client)
        {
            long ticks;
            ServerAttribute attribute = client.Attribute;
            string verifyString = attribute.VerifyString;
            if (verifyString == null)
            {
                ticks = 0;
                return verify(0, null, ref ticks).Value;
            }
            ulong markData = 0;
            if (attribute.IsMarkData) markData = attribute.VerifyHashCode;
            ticks = Date.NowTime.SetUtc().Ticks;
            do
            {
                ulong randomPrefix = Random.Default.SecureNextULongNotZero();
                while (randomPrefix == markData) randomPrefix = Random.Default.SecureNextULongNotZero();
                client.ReceiveMarkData = attribute.IsMarkData ? markData ^ randomPrefix : 0UL;
                client.SendMarkData = 0;
                long lastTicks = ticks;
                TcpServer.ReturnValue<bool> isVerify = verify(randomPrefix, TcpServer.TimeVerifyServer.Md5(verifyString, randomPrefix, ticks), ref ticks);
                if (isVerify.Value)
                {
                    client.SendMarkData = client.ReceiveMarkData;
                    return true;
                }
                if (isVerify.Type != TcpServer.ReturnType.Success || ticks <= lastTicks)
                {
                    client.Log.add(AutoCSer.Log.LogType.Error, "TCP客户端验证失败 [" + isVerify.Type.ToString() + "] " + ticks.toString() + " <= " + lastTicks.toString());
                    return false;
                }
                client.Log.add(AutoCSer.Log.LogType.Error, "TCP客户端验证时间失败重试 " + ticks.toString() + " - " + lastTicks.toString());
            }
            while (true);
        }
    }
}
