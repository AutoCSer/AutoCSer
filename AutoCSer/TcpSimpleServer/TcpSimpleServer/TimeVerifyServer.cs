﻿using System;
using System.Diagnostics;
using AutoCSer.Extensions;
using System.Security.Cryptography;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// 时间验证服务
    /// </summary>
    /// <typeparam name="serverType"></typeparam>
    /// <typeparam name="serverSocketType"></typeparam>
    public abstract unsafe class TimeVerifyServer<serverType, serverSocketType>
#if !NOJIT
        : ISetTcpServer<serverType>
#endif
        where serverType : Server<serverType, serverSocketType>
        where serverSocketType : ServerSocket<serverType, serverSocketType>
    {
        /// <summary>
        /// TCP 服务端
        /// </summary>
        protected serverType server;
        /// <summary>
        /// TCP 服务端
        /// </summary>
        internal serverType TcpServer
        {
            get { return server; }
        }
        /// <summary>
        /// 设置TCP服务端
        /// </summary>
        /// <param name="server">TCP服务端</param>
        public virtual void SetTcpServer(serverType server)
        {
            this.server = server;
        }
        /// <summary>
        /// MD5 加密
        /// </summary>
        private MD5CryptoServiceProvider md5;
        /// <summary>
        /// 验证时间戳
        /// </summary>
        private TcpServer.TimeVerifyTick timeVerifyTick = new TcpServer.TimeVerifyTick(AutoCSer.Threading.SecondTimer.UtcNow.Ticks - 1);
        /// <summary>
        /// 时间验证函数
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="randomPrefix">随机前缀</param>
        /// <param name="md5Data">MD5 数据</param>
        /// <param name="ticks">验证时钟周期</param>
        /// <returns>是否验证成功</returns>
        [Method(IsVerifyMethod = true, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, CommandIdentity = AutoCSer.Net.TcpServer.TimeVerifyServer.CommandIdentity)]
        protected virtual bool verify(serverSocketType socket, ulong randomPrefix, byte[] md5Data, ref long ticks)
        {
            if (server.CheckVerifyString()) return true;
            if (md5Data != null && md5Data.Length == 16)
            {
                if (!timeVerifyTick.Check(ref ticks, ref socket.TimeVerifyTicks)) return false;
                TcpServer.ServerBaseAttribute attribute = server.Attribute;
                if (md5 == null) md5 = new MD5CryptoServiceProvider();
                if (AutoCSer.Net.TcpServer.TimeVerifyServer.IsMd5(AutoCSer.Net.TcpServer.TimeVerifyServer.Md5(md5, attribute.VerifyString, randomPrefix, ticks), md5Data) == 0)
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
