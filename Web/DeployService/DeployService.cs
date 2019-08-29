using System;

namespace AutoCSer.Web.DeployService
{
    /// <summary>
    /// 发布启动服务
    /// </summary>
    internal partial class DeployService : AutoCSer.DeployService.Service
    {
        /// <summary>
        /// JSON 配置文件名称
        /// </summary>
        public const string JsonConfigFileName = @"C:\AutoCSer\DeployService\bin\Release\AutoCSer.DeployService.config.json";
        /// <summary>
        /// JSON 配置文件名称
        /// </summary>
        protected override string jsonConfigFileName { get { return JsonConfigFileName; } }
    }
}
