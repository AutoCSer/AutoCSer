using System;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using AutoCSer.Net.TcpInternalServer;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 内部注册服务
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Name = Server.ServerName, Host = "127.0.0.1", Port = (int)ServerPort.TcpRegister, CommandIdentityEnmuType = typeof(command))]
    public partial class Server :  TimeVerifyServer
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = "TcpRegister";
        /// <summary>
        /// 命令
        /// </summary>
        private enum command
        {
            /// <summary>
            /// 验证
            /// </summary>
            verify = 1,
            /// <summary>
            /// TCP 服务端轮询
            /// </summary>
            getLog,
            /// <summary>
            /// 添加TCP 服务注册信息
            /// </summary>
            appendLog,
        }

        /// <summary>
        /// 客户端集合
        /// </summary>
        private LeftArray<ClientInfo> clients;
        /// <summary>
        /// TCP 服务信息集合
        /// </summary>
        private readonly Dictionary<HashString, ServerSet> serverSets = DictionaryCreator.CreateHashString<ServerSet>();
        /// <summary>
        /// 时间验证函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="userID">用户ID</param>
        /// <param name="randomPrefix">随机前缀</param>
        /// <param name="md5Data">MD5 数据</param>
        /// <param name="ticks">验证时钟周期</param>
        /// <returns>是否验证成功</returns>
        [TcpServer.Method(IsVerifyMethod = true, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected override bool verify(ServerSocketSender sender, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks)
        {
            if (base.verify(sender, userID, randomPrefix, md5Data, ref ticks))
            {
                ClientInfo client = new ClientInfo();
                clients.Add(client);
                sender.ClientObject = client;
                return true;
            }
            return false;
        }
        /// <summary>
        /// TCP 服务端轮询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="onLog">TCP 服务注册通知委托</param>
        [TcpServer.KeepCallbackMethod(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.QueueLink, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.TcpQueue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private void getLog(ServerSocketSender sender, AutoCSer.Net.TcpServer.ServerCallback<ServerLog> onLog)
        {
            ClientInfo client = new UnionType { Value = sender.ClientObject }.ClientInfo;
            foreach (ServerSet serverSet in serverSets.Values)
            {
                if (!serverSet.OnLog(onLog))
                {
                    client.IsRemove = true;
                    return;
                }
            }
            client.OnLog = onLog;
        }
        /// <summary>
        /// 设置只读模式
        /// </summary>
        /// <param name="sender"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void SetReadCommand(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender)
        {
            sender.SetCommand((int)command.getLog);
        }
        /// <summary>
        /// 注册 TCP 服务信息
        /// </summary>
        /// <param name="server">TCP 服务信息</param>
        /// <returns>注册状态</returns>
        [TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private bool appendLog(ServerLog server)
        {
            if (server != null && server.HostToIpAddress())
            {
                ServerSet serverSet;
                bool isLog = true;
                switch (server.LogType)
                {
                    case LogType.RegisterServer:
                        HashString serverName = server.Name;
                        if (serverSets.TryGetValue(serverName, out serverSet))
                        {
                            bool isMainChanged;
                            isLog = serverSet.Add(server, out isMainChanged);
                        }
                        else serverSets.Add(serverName, serverSet = new ServerSet(server));
                        break;
                    case LogType.RemoveServer:
                        if (serverSets.TryGetValue(server.Name, out serverSet)) serverSet.Remove(server);
                        break;
                    default:
                        AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, "未知的 TCP 内部注册服务更新日志类型 " + server.toJson());
                        return false;
                }
                if (isLog)
                {
                    onLog(server);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 日志回调处理
        /// </summary>
        /// <param name="log"></param>
        private void onLog(ServerLog log)
        {
            ClientInfo[] clientArray = clients.Array;
            for (int index = clients.Length; index != 0;)
            {
                ClientInfo client = clientArray[--index];
                if (client.OnLog != null && (client.IsRemove || !client.OnLog.Callback(log)))
                {
                    clientArray[index] = clientArray[--clients.Length];
                    clientArray[clients.Length] = null;
                }
            }
        }
    }
}
