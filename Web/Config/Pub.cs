using System;
using System.IO;
using AutoCSer.Net.TcpInternalServer;
using AutoCSer.Extension;
using System.Threading;

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
        /// <summary>
        /// 远程控制明文密码
        /// </summary>
        public const ulong RemoteControlClearPassword = 0UL;
        /// <summary>
        /// 远程控制密码
        /// </summary>
        public const ulong RemoteControlPassword = 0UL;
        #endregion
        /// <summary>
        /// 服务器监听 IP 地址
        /// </summary>
        public static readonly string ServerListenIp = IsLocal ? "127.0.0.1" : "172.19.51.248";
        /// <summary>
        /// AutoCSer 根目录
        /// </summary>
        public static readonly string AutoCSerPath;
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
        ///// <summary>
        ///// 获取默认 TCP 内部服务配置
        ///// </summary>
        ///// <param name="serverType"></param>
        ///// <returns></returns>
        //public static AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute GetVerifyTcpServerSimpleAttribute(Type serverType)
        //{
        //    AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute attribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute>(serverType, false);
        //    attribute.VerifyString = TcpVerifyString;
        //    return attribute;
        //}
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
        /// 获取默认 TCP 内部静态服务配置
        /// </summary>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static ServerAttribute GetVerifyTcpStaticServerAttribute(Type serverType)
        {
            ServerAttribute attribute = new ServerAttribute();
            AutoCSer.MemberCopy.Copyer<ServerBaseAttribute>.Copy(attribute, AutoCSer.Metadata.TypeAttribute.GetAttribute<AutoCSer.Net.TcpStaticServer.ServerAttribute>(serverType, false));
            attribute.VerifyString = TcpVerifyString;
            return attribute;
        }
        /// <summary>
        /// 获取默认 TCP 内部服务配置
        /// </summary>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static ServerAttribute GetTcpStaticRegisterAttribute(Type serverType)
        {
            ServerAttribute attribute = GetVerifyTcpStaticServerAttribute(serverType);
            attribute.TcpRegister = TcpRegister;
            attribute.Port = 0;
            return attribute;
        }
        /// <summary>
        /// 控制台命令处理
        /// </summary>
        /// <param name="exitEvent"></param>
        /// <param name="onUnknownCommand"></param>
        public static void ConsoleCommand(EventWaitHandle exitEvent = null, Action<string> onUnknownCommand = null)
        {
            do
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\nclear cache / quit");//threads / 
                string command = Console.ReadLine();
                switch (command)
                {
                    case "quit": if (exitEvent != null) exitEvent.Set(); return;
                    case "clear cache": AutoCSer.Pub.ClearCache(); break;
                    //case "threads": AutoCSer.Deploy.Server.CheckThreadLog(); break;
                    default: if (onUnknownCommand != null) onUnknownCommand(command); break;
                }
            }
            while (true);
        }
        static Pub()
        {
            DirectoryInfo directory = new DirectoryInfo(AutoCSer.PubPath.ApplicationPath), lastDirectory = null;
            while (string.Compare(directory.Name, "AutoCSer", StringComparison.OrdinalIgnoreCase) != 0)
            {
                lastDirectory = directory;
                if ((directory = directory.Parent) == null)
                {
                    AutoCSerPath = (Pub.IsLocal ? @"C:\AutoCSer\Web\" : @"C:\AutoCSer\").ToLower();
                    break;
                }
            }
            if (directory != null)
            {
                if (lastDirectory != null && string.Compare(lastDirectory.Name, "Web", StringComparison.OrdinalIgnoreCase) == 0) directory = lastDirectory;
                AutoCSerPath = directory.fullName();
            }
        }
    }
}
