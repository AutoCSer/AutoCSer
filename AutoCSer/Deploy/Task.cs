using System;
using System.IO;
using AutoCSer.Extension;
using System.Threading;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 任务信息
    /// </summary>
    internal sealed class Task
    {
        /// <summary>
        /// 服务器端目录
        /// </summary>
        internal DirectoryInfo ServerDirectory;
        /// <summary>
        /// 文件集合
        /// </summary>
        internal KeyValue<string, int>[] FileIndexs;
        /// <summary>
        /// 任务信息索引位置
        /// </summary>
        internal int TaskIndex;
        /// <summary>
        /// 运行文件名称 / 自定义调用名称
        /// </summary>
        internal string RunFileName;
        /// <summary>
        /// 运行前休眠
        /// </summary>
        internal int RunSleep;
        /// <summary>
        /// 目录信息
        /// </summary>
        internal Directory Directory;
        /// <summary>
        /// 任务类型
        /// </summary>
        internal TaskType Type;
        /// <summary>
        /// 是否执行other目录文件
        /// </summary>
        internal bool IsRunOther;
        /// <summary>
        /// 是否等待运行程序结束
        /// </summary>
        internal bool IsWaitRun;
        /// <summary>
        /// 自定义参数数据
        /// </summary>
        internal byte[] CustomData;
        /// <summary>
        /// 运行任务
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        internal DeployState Run(Timer timer)
        {
            switch (Type)
            {
                case TaskType.Run: run(timer); break;
                case TaskType.WebFile:
                case TaskType.File:
                    if (Directory.Name != null)
                    {
                        if (!ServerDirectory.Exists) ServerDirectory.Create();
                        Directory.Deploy(ServerDirectory, timer.CreateBakDirectory());
                    }
                    break;
                case TaskType.AssemblyFile: assemblyFile(timer); break;
                case TaskType.WaitRunSwitch: wait(timer); break;
                case TaskType.Custom: return timer.Server.CustomTask.Call(timer.Server, this);
                default: return DeployState.UnknownTaskType;
            }
            return DeployState.Success;
        }
        /// <summary>
        /// 判断文件是否可写
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool canWrite(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            if (fileInfo.Exists)
            {
                try
                {
                    using (FileStream fileStream = fileInfo.OpenWrite()) return true;
                }
                catch { }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 写文件并运行程序
        /// </summary>
        /// <param name="timer"></param>
        private void run(Timer timer)
        {
            if (!ServerDirectory.Exists) ServerDirectory.Create();
            string serverDirectoryName = ServerDirectory.fullName(), runFileName = serverDirectoryName + (RunFileName ?? FileIndexs[0].Key);
            DirectoryInfo otherServerDirectory = new DirectoryInfo(serverDirectoryName + Server.DefaultSwitchDirectoryName);
            if (otherServerDirectory.Exists && !canWrite(runFileName))
            {
                ServerDirectory = otherServerDirectory;
                runFileName = otherServerDirectory.fullName() + (RunFileName ?? FileIndexs[0].Key);
                IsRunOther = true;
            }
            assemblyFile(timer);
            Thread.Sleep(RunSleep);
            if (IsWaitRun) new FileInfo(runFileName).WaitProcessDirectory();
            else new FileInfo(runFileName).StartProcessDirectory();
        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="timer"></param>
        private void assemblyFile(Timer timer)
        {
            if (!ServerDirectory.Exists) ServerDirectory.Create();
            string serverDirectoryName = ServerDirectory.fullName(), bakDirectoryName = timer.CreateBakDirectory().fullName();
            foreach (KeyValue<string, int> fileIndex in FileIndexs)
            {
                byte[] data = timer.DeployInfo.Files[fileIndex.Value];
                string fileName = serverDirectoryName + fileIndex.Key;
                FileInfo file = new FileInfo(fileName);
                if (file.Exists) File.Move(fileName, bakDirectoryName + fileIndex.Key);
                using (FileStream fileStream = file.Create()) fileStream.Write(data, 0, data.Length);
            }
        }
        /// <summary>
        /// 等待运行程序切换结束的文件
        /// </summary>
        private string waitFile
        {
            get
            {
                if (IsRunOther) return ServerDirectory.Parent.fullName() + FileIndexs[0].Key;
                return ServerDirectory.fullName() + Server.DefaultSwitchDirectoryName + AutoCSer.Extension.DirectoryExtension.Separator + FileIndexs[0].Key;
            }
        }
        /// <summary>
        /// 等待运行程序切换结束
        /// </summary>
        /// <param name="timer"></param>
        private void wait(Timer timer)
        {
            FileInfo file = new FileInfo(timer.DeployInfo.Tasks.Array[TaskIndex].waitFile);
            if (file.Exists)
            {
                do
                {
                    try
                    {
                        using (FileStream fileStream = file.OpenWrite()) return;
                    }
                    catch { }
                    Thread.Sleep(1);
                }
                while (true);
            }
        }
    }
}
