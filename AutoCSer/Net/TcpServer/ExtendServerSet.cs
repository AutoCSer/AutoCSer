using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 扩展服务集合
    /// </summary>
    internal sealed class ExtendServerSet
    {
        /// <summary>
        /// 服务关键字
        /// </summary>
        private struct serverKey : IEquatable<serverKey>
        {
            /// <summary>
            /// 接口类型
            /// </summary>
            public Type InterfaceType;
            /// <summary>
            /// 接口名称
            /// </summary>
            public string Name;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(serverKey other)
            {
                return InterfaceType == other.InterfaceType && Name == other.Name;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                int value = InterfaceType.GetHashCode();
                if (Name != null) value ^= Name.GetHashCode();
                return value;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return Equals((serverKey)obj);
            }
        }
        /// <summary>
        /// 扩展服务集合
        /// </summary>
        private LeftArray<ExtendServer> serverArray = new LeftArray<ExtendServer>(0);
        /// <summary>
        /// TCP 服务基类
        /// </summary>
        private readonly ServerBase server;
        /// <summary>
        /// 扩展服务集合
        /// </summary>
        private readonly Dictionary<serverKey, ExtendServer> servers;
        /// <summary>
        /// 创建扩展服务访问锁
        /// </summary>
        private readonly object createServerLock;
        /// <summary>
        /// 会话索引最大值
        /// </summary>
        private readonly int commandIndexAnd;
        /// <summary>
        /// 命令二进制位数
        /// </summary>
        private readonly byte commandBits;
        /// <summary>
        /// 扩展服务集合
        /// </summary>
        /// <param name="server"></param>
        /// <param name="commandBits">命令二进制位数</param>
        internal ExtendServerSet(ServerBase server, byte commandBits)
        {
            if (commandBits == 0) commandIndexAnd = (int)Server.CommandIndexAnd;
            else
            {
                this.server = server;
                commandIndexAnd = (1 << commandBits) - 1;
                this.commandBits = commandBits;
                createServerLock = new object();
                servers = DictionaryCreator<serverKey>.Create<ExtendServer>();
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        internal void Free()
        {
            foreach (ExtendServer server in serverArray) server.Free();
        }
        /// <summary>
        /// 检查最大命令是否在有效范围内
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CheckMaxCommand(int command)
        {
            return command <= commandIndexAnd;
        }
        /// <summary>
        /// 判断命令是否有效
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal bool IsCommand(int index)
        {
            int serverIndex = (index >> commandBits) - 1;
            if ((uint)serverIndex < (uint)serverArray.Length) return serverArray.Array[serverIndex].IsCommand(index & commandIndexAnd);
            if (server != null && server.Log.IsAnyLevel(LogLevel.Info)) server.Log.Info(server.ServerAttribute.ServerName + " 缺少命令处理委托 [" + index.toString() + "]", LogLevel.Info | LogLevel.AutoCSer);
            return false;
        }
        /// <summary>
        /// 添加扩展服务
        /// </summary>
        /// <param name="interfaceType">服务接口类型</param>
        /// <param name="name">服务绑定名称</param>
        /// <returns>是否添加成功</returns>
        internal bool Append(Type interfaceType, string name)
        {
            if (servers != null)
            {
                serverKey key = new serverKey { InterfaceType = interfaceType, Name = name };
                Monitor.Enter(createServerLock);
                try
                {
                    if (!servers.ContainsKey(key))
                    {
                        ExtendServer extendServer = server.CreateExtendServer(interfaceType);
                        servers.Add(key, extendServer);
                        serverArray.Add(extendServer);
                        return true;
                    }
                }
                finally { Monitor.Exit(createServerLock); }
            }
            return false;
        }

        /// <summary>
        /// 默认空扩展服务集合
        /// </summary>
        internal static readonly ExtendServerSet Null = new ExtendServerSet(null, 0);
    }
}
