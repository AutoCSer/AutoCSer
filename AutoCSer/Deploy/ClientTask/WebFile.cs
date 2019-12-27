using System;
using System.IO;

namespace AutoCSer.Deploy.ClientTask
{
    /// <summary>
    /// 写 css/js/html 任务信息
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    public class WebFile : Task
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public override TaskType Type { get { return TaskType.WebFile; } }
        /// <summary>
        /// 服务器端目录
        /// </summary>
        public string ServerPath;
        /// <summary>
        /// 客户端目录
        /// </summary>
        [AutoCSer.BinarySerialize.IgnoreMember]
        public string ClientPath;

        /// <summary>
        /// Web部署目录信息
        /// </summary>
        internal Directory Directory;
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        internal override DeployState Call(Timer timer)
        {
            if (Directory.Name != null)
            {
                DirectoryInfo serverDirectory = new DirectoryInfo(ServerPath);
                if (!serverDirectory.Exists) serverDirectory.Create();
                Directory.Deploy(serverDirectory, timer.CreateBakDirectory());
            }
            return DeployState.Success;
        }
    }
}
