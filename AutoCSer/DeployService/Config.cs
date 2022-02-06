using System;
using System.IO;
using System.Text;
using AutoCSer.Extensions;

namespace AutoCSer.DeployService
{
    /// <summary>
    /// 发布启动服务配置
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 默认服务名称
        /// </summary>
        internal const string DefaultServiceName = "AutoCSer.DeployService";
        /// <summary>
        /// 默认服务描述
        /// </summary>
        internal const string DefaultServiceDescription = "AutoCSer 发布工具启动服务";

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName = DefaultServiceName;
        /// <summary>
        /// 服务描述
        /// </summary>
        public string ServiceDescription = DefaultServiceDescription;
        /// <summary>
        /// 安装服务用户名称
        /// </summary>
        public string InstallUsername = Environment.MachineName + @"\" + Environment.UserName;
        /// <summary>
        /// 安装服务用户密码
        /// </summary>
        public string InstallPassword;

        /// <summary>
        /// 启动调用批处理文件名称
        /// </summary>
        public string StartBatFileName = DefaultServiceName + ".bat";
        /// <summary>
        /// 停止调用批处理文件名称
        /// </summary>
        public string StopBatFileName;

        /// <summary>
        /// 获取发布启动服务配置
        /// </summary>
        /// <param name="JsonFileName"></param>
        /// <returns></returns>
        public static Config Get(string JsonFileName = null)
        {
            if (!string.IsNullOrEmpty(JsonFileName))
            {
                FileInfo JsonFile = new FileInfo(JsonFileName);
                if (JsonFile.Exists)
                {
                    Config Config = AutoCSer.JsonDeSerializer.DeSerialize<Config>(File.ReadAllText(JsonFile.FullName, Encoding.UTF8));
                    if (Config != null) return Config;
                    AutoCSer.LogHelper.Error("JSON 配置文件 " + JsonFile.FullName + " 解析失败", LogLevel.Error | LogLevel.AutoCSer);
                }
                else AutoCSer.LogHelper.Error("没有找到 JSON 配置文件 " + JsonFile.FullName, LogLevel.Error | LogLevel.AutoCSer);
            }
            return Default;
        }
        /// <summary>
        /// 默认发布启动服务配置
        /// </summary>
        private static readonly Config Default = new Config();
    }
}
