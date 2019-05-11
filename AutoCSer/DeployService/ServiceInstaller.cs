using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;


namespace AutoCSer.DeployService
{
    /// <summary>
    /// 发布启动服务安装
    /// </summary>
    [RunInstaller(true)]
    public partial class ServiceInstaller : System.Configuration.Install.Installer
    {
        /// <summary>
        /// 发布启动服务配置文件
        /// </summary>
        protected virtual string jsonConfigFileName { get { return null; } }

        /// <summary>
        /// 发布启动服务安装
        /// </summary>
        public ServiceInstaller()
        {
            InitializeComponent();

            Config config = Config.Get(jsonConfigFileName);
            this.DeployServiceInstaller.Description = config.ServiceDescription;

            this.DeployServiceProcessInstaller.Password = config.InstallPassword;
            this.DeployServiceProcessInstaller.Username = config.InstallUsername;

            this.DeployServiceInstaller.ServiceName = config.ServiceName;
        }
    }
}
