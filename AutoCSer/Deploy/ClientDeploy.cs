using System;
using System.Collections.Generic;
using System.IO;
using AutoCSer.Extension;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 客户端部署信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ClientDeploy
    {
        /// <summary>
        /// 部署名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 部署服务，null 表示默认服务
        /// </summary>
        public string ServerName;
        /// <summary>
        /// 任务信息
        /// </summary>
        public ClientTask[] Tasks;
        /// <summary>
        /// 部署服务客户端
        /// </summary>
        internal TcpClient TcpClient;
        /// <summary>
        /// 启动部署
        /// </summary>
        /// <returns>部署结果</returns>
        internal DeployResult Deploy()
        {
#if NoAutoCSer
            throw new Exception();
#else
            if (TcpClient.IsClient)
            {
                AutoCSer.Net.IndexIdentity identity = TcpClient.TcpInternalClient.create(TcpClient.ClientId);
                byte isStart = 0;
                try
                {
                    Client client = TcpClient.Client;
                    Dictionary<HashString, FileSource> fileSources = DictionaryCreator.CreateHashString<FileSource>();
                    ClientTaskInfo[] tasks = new ClientTaskInfo[Tasks.Length];
                    for (int taskIndex = 0; taskIndex != Tasks.Length; ++taskIndex)
                    {
                        switch (Tasks[taskIndex].Type)
                        {
                            case TaskType.Run:
                            case TaskType.AssemblyFile: appendSource(fileSources, ref Tasks[taskIndex], ref tasks[taskIndex]); break;
                            case TaskType.File:
                                //appendFileSource(fileSources, ref Tasks[taskIndex], ref tasks[taskIndex]);
                                DirectoryInfo clientDirectory = new DirectoryInfo(Tasks[taskIndex].ClientPath);
                                Directory directory = Directory.Create(clientDirectory, client.Config.FileLastWriteTime, Tasks[taskIndex].FileSearchPatterns);
                                tasks[taskIndex].Directory = TcpClient.TcpInternalClient.getFileDifferent(directory, Tasks[taskIndex].ServerPath);
                                tasks[taskIndex].Directory.Load(clientDirectory);
                                break;
                            case TaskType.WebFile:
                                DirectoryInfo webClientDirectory = new DirectoryInfo(Tasks[taskIndex].ClientPath);
                                Directory webDirectory = Directory.CreateWeb(webClientDirectory, client.Config.FileLastWriteTime);
                                tasks[taskIndex].Directory = TcpClient.TcpInternalClient.getFileDifferent(webDirectory, Tasks[taskIndex].ServerPath);
                                tasks[taskIndex].Directory.Load(webClientDirectory);
                                break;
                        }
                    }

                    if (fileSources.Count != 0 && !TcpClient.TcpInternalClient.setFileSource(identity, fileSources.getArray(value => value.Value.Data)))
                    {
                        return new DeployResult { Index = -1, State = DeployState.SetFileSourceError };
                    }
                    for (int taskIndex = 0; taskIndex != Tasks.Length; ++taskIndex)
                    {
                        switch (Tasks[taskIndex].Type)
                        {
                            case TaskType.Run:
                                if ((tasks[taskIndex].TaskIndex = TcpClient.TcpInternalClient.addRun(identity, tasks[taskIndex].FileIndexs.ToArray(), Tasks[taskIndex].ServerPath, Tasks[taskIndex].RunSleep)) == -1)
                                {
                                    return new DeployResult { Index = -1, State = DeployState.AddRunError };
                                }
                                break;
                            case TaskType.WebFile:
                            case TaskType.File:
                                if (TcpClient.TcpInternalClient.addFiles(identity, tasks[taskIndex].Directory, Tasks[taskIndex].ServerPath, Tasks[taskIndex].Type) == -1)
                                {
                                    return new DeployResult { Index = -1, State = DeployState.AddFileError };
                                }
                                break;
                            case TaskType.AssemblyFile:
                                if (TcpClient.TcpInternalClient.addAssemblyFiles(identity, tasks[taskIndex].FileIndexs.ToArray(), Tasks[taskIndex].ServerPath) == -1)
                                {
                                    return new DeployResult { Index = -1, State = DeployState.AddAssemblyFileError };
                                }
                                break;
                            case TaskType.WaitRunSwitch:
                                if (TcpClient.TcpInternalClient.addWaitRunSwitch(identity, tasks[Tasks[taskIndex].TaskIndex].TaskIndex) == -1)
                                {
                                    return new DeployResult { Index = -1, State = DeployState.AddWaitRunSwitchError };
                                }
                                break;
                        }
                    }
                    if (TcpClient.TcpInternalClient.start(identity, DateTime.MinValue))
                    {
                        isStart = 1;
                        return new DeployResult { Index = identity.Index, State = DeployState.Success };
                    }
                    return new DeployResult { Index = -1, State = DeployState.StartError };
                }
                finally
                {
                    if (isStart == 0) TcpClient.TcpInternalClient.clear(identity);// && TcpClient.TcpInternalClient.clear(identity).Type != AutoCSer.Net.TcpServer.ReturnType.Success
                }
            }
#endif
            return new DeployResult { Index = -1, State = DeployState.NoClient };
        }
        /// <summary>
        /// 添加文件数据源
        /// </summary>
        /// <param name="fileSources"></param>
        /// <param name="task"></param>
        /// <param name="serverTask"></param>
        private void appendSource(Dictionary<HashString, FileSource> fileSources, ref ClientTask task, ref ClientTaskInfo serverTask)
        {
            Client client = TcpClient.Client;
            DirectoryInfo directory = new DirectoryInfo(task.ClientPath);
            string directoryName = directory.fullName();
            if (task.RunFileName != null) appendSource(fileSources, directoryName, task.RunFileName, ref serverTask);
            foreach (FileInfo file in directory.GetFiles("*.exe"))
            {
                if (file.LastWriteTimeUtc > client.Config.FileLastWriteTime && file.Name != task.RunFileName && !client.IgnoreFileNames.Contains(file.Name)
                    && !file.Name.EndsWith(".vshost.exe", StringComparison.Ordinal))
                {
                    appendSource(fileSources, directoryName, file.Name, ref serverTask);
                }
            }
            foreach (FileInfo file in directory.GetFiles("*.dll"))
            {
                if (file.LastWriteTimeUtc > client.Config.FileLastWriteTime && !client.IgnoreFileNames.Contains(file.Name))
                {
                    appendSource(fileSources, directoryName, file.Name, ref serverTask);
                }
            }
            foreach (FileInfo file in directory.GetFiles("*.pdb"))
            {
                if (file.LastWriteTimeUtc > client.Config.FileLastWriteTime && !client.IgnoreFileNames.Contains(file.Name))
                {
                    appendSource(fileSources, directoryName, file.Name, ref serverTask);
                }
            }
        }
        /// <summary>
        /// 添加文件数据源
        /// </summary>
        /// <param name="fileSources"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="serverTask"></param>
        private void appendSource(Dictionary<HashString, FileSource> fileSources, string path, string fileName, ref ClientTaskInfo serverTask)
        {
            FileSource fileSource;
            if (!fileSources.TryGetValue(fileName, out fileSource))
            {
                foreach (string runFilePath in TcpClient.Client.Config.RunFilePaths.notNull())
                {
                    string runFileName = runFilePath + fileName;
                    if (File.Exists(runFileName))
                    {
                        fileSources.Add(fileName, fileSource = new FileSource { Data = File.ReadAllBytes(runFileName), Index = fileSources.Count });
                        break;
                    }
                }
                if (fileSource.Data == null) fileSources.Add(fileName, fileSource = new FileSource { Data = File.ReadAllBytes(path + fileName), Index = fileSources.Count });
            }
            serverTask.FileIndexs.Add(new KeyValue<string, int>(fileName, fileSource.Index));
        }
        ///// <summary>
        ///// 添加文件数据源
        ///// </summary>
        ///// <param name="fileSources"></param>
        ///// <param name="task"></param>
        ///// <param name="serverTask"></param>
        //private void appendFileSource(Dictionary<HashString, FileSource> fileSources, ref ClientTask task, ref ClientTaskInfo serverTask)
        //{
        //    Client client = TcpClient.Client;
        //    DirectoryInfo directory = new DirectoryInfo(task.ClientPath);
        //    string directoryName = directory.fullName();
        //    foreach (FileInfo file in directory.GetFiles())
        //    {
        //        if (file.LastWriteTimeUtc > client.Config.FileLastWriteTime && !client.IgnoreFileNames.Contains(file.Name))
        //        {
        //            appendSource(fileSources, directoryName, file.Name, ref serverTask);
        //        }
        //    }
        //}
    }
}
