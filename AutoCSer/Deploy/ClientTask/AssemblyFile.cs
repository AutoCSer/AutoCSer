using AutoCSer.Extensions;
using System;
using System.IO;

namespace AutoCSer.Deploy.ClientTask
{
    /// <summary>
    /// 写文件 exe/dll/pdb 任务信息
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class AssemblyFile : WebFile
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public override TaskType Type { get { return TaskType.AssemblyFile; } }

        /// <summary>
        /// 文件集合
        /// </summary>
        internal KeyValue<string, int>[] FileIndexs;
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        internal override DeployResultData Call(Timer timer)
        {
            DirectoryInfo serverDirectory = new DirectoryInfo(ServerPath);
            if (!serverDirectory.Exists) serverDirectory.Create();
            string serverDirectoryName = serverDirectory.fullName(), bakDirectoryName = timer.CreateBakDirectory().fullName();
            foreach (KeyValue<string, int> fileIndex in FileIndexs)
            {
                byte[] data = timer.DeployInfo.Files[fileIndex.Value];
                string fileName = serverDirectoryName + fileIndex.Key;
                FileInfo file = new FileInfo(fileName);
                if (file.Exists) System.IO.File.Move(fileName, bakDirectoryName + fileIndex.Key);
                using (FileStream fileStream = file.Create()) fileStream.Write(data, 0, data.Length);
            }
            return DeployState.Success;
        }
    }
}
