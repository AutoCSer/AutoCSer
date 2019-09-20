using System;

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
        /// Nuget 路径
        /// </summary>
        public const string NugetPath = AutoCSerPath + @"Nuget\";

        /// <summary>
        /// 服务器 IP 地址
        /// </summary>
        public static readonly string ServerIp = Pub.IsLocal ? "127.0.0.1" : "139.196.98.185";
        /// <summary>
        /// 服务端路径
        /// </summary>
        public static readonly string ServerPath = Pub.IsLocal ? AutoCSerPath + @"Web\DeployServer\bin\Release\Local\" : AutoCSerPath;
    }
}
