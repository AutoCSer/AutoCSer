﻿using System;

namespace AutoCSer.Web.Config
{
    /// <summary>
    /// AutoCSer.Web.DeployClient 项目配置
    /// </summary>
    public static class Deploy
    {
        /// <summary>
        /// AutoCSer 解决方案路径
        /// </summary>
        public const string AutoCSerPath = @"C:\AutoCSer\";
        /// <summary>
        /// AutoCSer 项目路径
        /// </summary>
        public const string AutoCSerProjectPath = AutoCSerPath + @"AutoCSer\";
        /// <summary>
        /// AutoCSer2 解决方案路径
        /// </summary>
        public const string AutoCSer2Path = @"C:\AutoCSer2\";
        /// <summary>
        /// AutoCSer2 项目路径
        /// </summary>
        public const string AutoCSer2ProjectPath = AutoCSer2Path + @"AutoCSer\";
        /// <summary>
        /// 个人目录
        /// </summary>
        public const string ShowjimPath = @"C:\Showjim\";
        /// <summary>
        /// Nuget 路径
        /// </summary>
        public const string NugetPath = ShowjimPath + @"Nuget\";

        /// <summary>
        /// 服务器 IP 地址
        /// </summary>
        public static readonly string ServerIp = Pub.IsLocal ? "127.0.0.1" : "127.0.0.1";
        /// <summary>
        /// 服务端路径
        /// </summary>
        public static readonly string ServerPath = Pub.IsLocal ? AutoCSerPath + @"Web\DeployServer\bin\Release\Local\" : AutoCSerPath;
    }
}
