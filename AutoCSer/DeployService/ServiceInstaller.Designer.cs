namespace AutoCSer.DeployService
{
    partial class ServiceInstaller
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.DeployServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.DeployServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            this.DeployServiceInstaller.Description = Config.DefaultServiceDescription;
            this.DeployServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;

            this.DeployServiceProcessInstaller.Password = null;
            this.DeployServiceProcessInstaller.Username = null;

            this.DeployServiceInstaller.ServiceName = Config.DefaultServiceName;

            this.Installers.AddRange(new System.Configuration.Install.Installer[] { this.DeployServiceProcessInstaller, this.DeployServiceInstaller });
        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller DeployServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller DeployServiceInstaller;
    }
}