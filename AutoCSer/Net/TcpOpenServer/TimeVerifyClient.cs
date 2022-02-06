using System;
using AutoCSer.Extensions;

namespace AutoCSer.Net.TcpOpenServer
{
    /// <summary>
    /// 时间验证服务客户端
    /// </summary>
    public static class TimeVerifyClient
    {
        /// <summary>
        /// 时间验证服务客户端委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="userID">用户ID</param>
        /// <param name="randomPrefix">随机前缀</param>
        /// <param name="md5Data">MD5 数据</param>
        /// <param name="ticks">验证时钟周期</param>
        /// <returns></returns>
        public delegate TcpServer.ReturnValue<bool> Verifier(ClientSocketSender sender, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks);
        /// <summary>
        /// 时间验证客户端验证
        /// </summary>
        /// <param name="verify">时间验证服务客户端委托</param>
        /// <param name="sender"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public unsafe static bool Verify(Verifier verify, ClientSocketSender sender, AutoCSer.Net.TcpOpenServer.Client client)
        {
            string verifyString = client.Attribute.VerifyString;
            if (verifyString == null)
            {
                long ticks = 0;
                return verify(sender, null, 0, null, ref ticks).Value;
            }
            return Verify(verify, sender, client, null, verifyString);
        }
        /// <summary>
        /// 时间验证客户端验证
        /// </summary>
        /// <param name="verify">时间验证服务客户端委托</param>
        /// <param name="sender"></param>
        /// <param name="client"></param>
        /// <param name="userID">用户ID</param>
        /// <param name="verifyString">验证字符串</param>
        /// <returns></returns>
        public unsafe static bool Verify(Verifier verify, ClientSocketSender sender, AutoCSer.Net.TcpOpenServer.Client client, string userID, string verifyString)
        {
            TcpServer.ServerBaseAttribute attribute = client.Attribute;
            ulong markData = 0;
            if (attribute.IsMarkData) markData = attribute.VerifyHashCode;
            long ticks = AutoCSer.Net.TcpInternalServer.TimeVerifyClient.GetTicks();
            TcpServer.ClientSocketBase socket = sender.ClientSocket;
            bool isError = false;
            do
            {
                ulong randomPrefix = Random.Default.SecureNextULongNotZero();
                while (randomPrefix == markData) randomPrefix = Random.Default.SecureNextULongNotZero();
                socket.ReceiveMarkData = attribute.IsMarkData ? markData ^ randomPrefix : 0UL;
                sender.SendMarkData = 0;
                long lastTicks = ticks;
                TcpServer.ReturnValue<bool> isVerify = verify(sender, userID, randomPrefix, TcpServer.TimeVerifyServer.Md5(verifyString, randomPrefix, ticks), ref ticks);
                if (isVerify.Value)
                {
                    sender.SendMarkData = socket.ReceiveMarkData;
                    if (isError) socket.Log.Debug(sender.ClientSocket.IpAddress.ToString() + ":" + sender.ClientSocket.Port.toString() + " TCP客户端验证时间重试成功");
                    return true;
                }
                if (isVerify.Type != TcpServer.ReturnType.Success || ticks <= lastTicks)
                {
                    socket.Log.Error(sender.ClientSocket.IpAddress.ToString() + ":" + sender.ClientSocket.Port.toString() + " TCP客户端验证失败 [" + isVerify.Type.ToString() + "] " + ticks.toString() + " <= " + lastTicks.toString());
                    return false;
                }
                socket.Log.Error(sender.ClientSocket.IpAddress.ToString() + ":" + sender.ClientSocket.Port.toString() + " TCP客户端验证时间失败重试 " + ticks.toString() + " - " + lastTicks.toString());
                isError = true;
            }
            while (true);
        }
    }
}
