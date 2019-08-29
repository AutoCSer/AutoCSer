using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;

namespace AutoCSer.Web.DeployService
{
    /// <summary>
    /// 发布启动服务安装
    /// </summary>
    [RunInstaller(true)]
    public partial class DeployServiceInstaller : AutoCSer.DeployService.ServiceInstaller
    {
        /// <summary>
        /// JSON 配置文件名称
        /// </summary>
        protected override string jsonConfigFileName { get { return DeployService.JsonConfigFileName; } }
    }
}
