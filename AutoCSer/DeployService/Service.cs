using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.IO;
using AutoCSer.Extension;

namespace AutoCSer.DeployService
{
    /// <summary>
    /// 发布启动服务
    /// </summary>
    public partial class Service : ServiceBase
    {
        /// <summary>
        /// 发布启动服务配置文件
        /// </summary>
        protected virtual string jsonConfigFileName { get { return null; } }

        /// <summary>
        /// 发布启动服务
        /// </summary>
        public Service()
        {
            InitializeComponent();
            Config config = Config.Get(jsonConfigFileName);
            this.ServiceName = config.ServiceName;
        }

        /// <summary>
        /// 调用批处理命令
        /// </summary>
        /// <param name="BatFileName"></param>
        private static void CallBat(string BatFileName)
        {
            if (!string.IsNullOrEmpty(BatFileName))
            {
                FileInfo BatFile = new FileInfo(BatFileName);
                if (BatFile.Exists) Process.Start(BatFile.FullName);
                else AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, "没有找到批处理文件 " + BatFile.FullName);
            }
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            Config config = Config.Get(jsonConfigFileName);
            CallBat(config.StartBatFileName);
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        protected override void OnStop()
        {
            Config config = Config.Get(jsonConfigFileName);
            CallBat(config.StopBatFileName);
        }
    }
}
