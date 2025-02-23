﻿using System;
using System.Threading;
using AutoCSer.Extensions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;

namespace AutoCSer.Web.SearchServer
{
    /// <summary>
    /// 搜索服务
    /// </summary>
    [AutoCSer.Net.TcpStaticServer.Server(Name = Server.ServerName, Host = "127.0.0.1", Port = 8992, IsServer = true)]
    public partial class Server : AutoCSer.Net.TcpStaticServer.TimeVerify<Server>
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = "SearchServer";
        /// <summary>
        /// 关键字搜索
        /// </summary>
        /// <param name="key">字搜索</param>
        /// <param name="onSearch">搜索回调</param>
        [AutoCSer.Net.TcpStaticServer.SerializeBoxMethod(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        internal static void Search(string key, AutoCSer.Net.TcpServer.ServerCallback<SearchItem[]> onSearch)
        {
            Searcher.SearchTaskQueue.CheckAdd(new Queue.Search(key, onSearch));
        }

        /// <summary>
        /// 触发 HTML 初始化加载
        /// </summary>
        private static readonly Html html0 = Html.Cache[0];
        /// <summary>
        /// 当前服务实例
        /// </summary>
        private static AutoCSer.Web.SearchServer.TcpStaticServer.SearchServer searchServer;
        /// <summary>
        /// 创建搜索服务
        /// </summary>
        internal static void CreateSearchServer()
        {
            AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute = AutoCSer.Web.Config.Pub.GetTcpStaticRegisterAttribute(typeof(AutoCSer.Web.SearchServer.Server), false);
            if (new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Name == AutoCSer.Deploy.Server.DefaultSwitchDirectoryName) serverAttribute.Port += 10000;
            do
            {
                try
                {
                    searchServer = new AutoCSer.Web.SearchServer.TcpStaticServer.SearchServer(AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpInternalServer.ServerAttribute>.MemberwiseClone(serverAttribute));
                    if (searchServer.IsListen)
                    {
                        Console.WriteLine("Search 服务启动成功 " + serverAttribute.Host + ":" + searchServer.Port.toString());
                        return;
                    }
                    Console.WriteLine("Search 服务启动失败 " + serverAttribute.Host + ":" + searchServer.Port.toString());
                }
                catch (Exception error)
                {
                    Console.WriteLine(error.ToString());
                }
                Thread.Sleep(1000);
            }
            while (true);
        }
    }
}
