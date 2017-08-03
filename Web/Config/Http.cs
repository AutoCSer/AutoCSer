using System;

namespace AutoCSer.Web.Config
{
    /// <summary>
    /// AutoCSer.Web.HttpServer 项目配置
    /// </summary>
    public static class Http
    {
        /// <summary>
        /// 缓存文件目录
        /// </summary>
        public static readonly string CachePath = (Pub.IsLocal ? @"C:\AutoCSer\Web\HttpServer\bin\Release\" : @"C:\AutoCSer\HttpServer\bin\Release\").ToLower();
    }
}
