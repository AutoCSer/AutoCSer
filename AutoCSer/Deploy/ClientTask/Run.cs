using AutoCSer.Extensions;
using System;
using System.IO;
using System.Threading;

namespace AutoCSer.Deploy.ClientTask
{
    /// <summary>
    /// 写文件 exe/dll/pdb 并运行程序 任务信息
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class Run : Task
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public override TaskType Type { get { return TaskType.Run; } }
        /// <summary>
        /// 服务器端目录
        /// </summary>
        public string ServerPath;
        /// <summary>
        /// 运行文件名称
        /// </summary>
        public string FileName;
        /// <summary>
        /// 运行前休眠
        /// </summary>
        public int Sleep;
        /// <summary>
        /// 是否等待运行程序结束
        /// </summary>
        public bool IsWait;

        /// <summary>
        /// 写文件并运行程序
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        internal override DeployResultData Call(Timer timer)
        {
            FileInfo runFileInfo = new FileInfo(Path.Combine(ServerPath, FileName));
            if (runFileInfo.Exists)
            {
                Thread.Sleep(Sleep);
                if (IsWait) runFileInfo.WaitProcessDirectory();
                else runFileInfo.StartProcessDirectory();
                return DeployState.Success;
            }
            return DeployState.NotFoundFile;
        }
    }
}
