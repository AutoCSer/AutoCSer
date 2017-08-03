using System;
using System.IO;
using AutoCSer.Extension;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 代码生成项目配置
    /// </summary>
    [AutoCSer.Config.Type]
    internal static class Config
    {
        /// <summary>
        /// 日志配置
        /// </summary>
        [AutoCSer.Config.Member]
        public static AutoCSer.Log.Config LogConfig
        {
            get
            {
                string logPath = PubPath.ApplicationPath;
                DirectoryInfo directory = new DirectoryInfo(logPath);
                if (string.CompareOrdinal(directory.Parent.Name.ToLower(), "packet") == 0) logPath = directory.Parent.fullName();
                return new Log.Config { Type = AutoCSer.Log.LogType.All, FilePath = logPath };
            }
        }
    }
}
