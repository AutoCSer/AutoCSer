using System;
using AutoCSer.Net.TcpInternalServer;

namespace AutoCSer.Web.Config
{
    /// <summary>
    /// 公共配置
    /// </summary>
    public static class Pub
    {
        #region AutoCSer.Web.ExamplePack.exe
        /// <summary>
        /// 是否本地模式
        /// </summary>
        public static readonly bool IsLocal = true;
        /// <summary>
        /// TCP 服务默认验证字符串
        /// </summary>
        public const string TcpVerifyString = "XXX";
        #endregion
        /// <summary>
        /// 服务器监听 IP 地址
        /// </summary>
        public static readonly string ServerListenIp = IsLocal ? "127.0.0.1" : "172.19.51.248";
        /// <summary>
        /// TCP 注册服务名称
        /// </summary>
        public const string TcpRegister = "TcpRegister";
        /// <summary>
        /// TCP 注册服务名称
        /// </summary>
        public const string TcpRegisterReader = TcpRegister + "Reader";

        /// <summary>
        /// 获取默认 TCP 内部服务配置
        /// </summary>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static ServerAttribute GetVerifyTcpServerAttribute(Type serverType)
        {
            ServerAttribute attribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<ServerAttribute>(serverType, false);
            attribute.VerifyString = TcpVerifyString;
            return attribute;
        }
        /// <summary>
        /// 获取默认 TCP 内部服务配置
        /// </summary>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static ServerAttribute GetTcpRegisterAttribute(Type serverType)
        {
            ServerAttribute attribute = GetVerifyTcpServerAttribute(serverType);
            attribute.TcpRegister = TcpRegister;
            attribute.Port = 0;
            return attribute;
        }
        /// <summary>
        /// 控制台命令处理
        /// </summary>
        public static void ConsoleCommand()
        {
            do
            {
                Console.WriteLine("clear cache / quit");
                switch (Console.ReadLine())
                {
                    case "quit": return;
                    case "clear cache": AutoCSer.Pub.ClearCache(); break;
                }
            }
            while (true);
        }
    }
}
