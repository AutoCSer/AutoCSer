using AutoCSer.Extensions;
using System;

namespace AutoCSer.Deploy.ClientTask
{
    /// <summary>
    /// 发布切换更新
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class UpdateSwitchFile : Task
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public override TaskType Type { get { return TaskType.UpdateSwitchFile; } }
        /// <summary>
        /// 服务器端目录
        /// </summary>
        public string ServerPath;
        /// <summary>
        /// 运行文件名称
        /// </summary>
        public string FileName;
        /// <summary>
        /// 切换服务相对目录名称
        /// </summary>
        public string SwitchDirectoryName;
        /// <summary>
        /// 更新服务相对目录名称
        /// </summary>
        public string UpdateDirectoryName;

        /// <summary>
        /// 另外一个待运行文件名称
        /// </summary>
        [AutoCSer.BinarySerialize.IgnoreMember]
        internal string WaitFile;
        /// <summary>
        /// 发布切换更新
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        internal override DeployResultData Call(Timer timer)
        {
            if (ServerPath == null && FileName == string.Empty)
            {
                AutoCSer.Threading.ThreadPool.TinyBackground.Start(() => timer.Server.OnDeployServerUpdated(null, SwitchDirectoryName, UpdateDirectoryName));
            }
            else Server.UpdateSwitchFile(ServerPath, FileName, SwitchDirectoryName, UpdateDirectoryName, out WaitFile).StartProcessDirectory();
            return DeployState.Success;
        }
    }
}
