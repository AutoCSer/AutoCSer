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
    public partial struct ClientDeploy
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
        public ClientTask.Task[] Tasks;
        /// <summary>
        /// 部署服务客户端
        /// </summary>
        internal TcpClient TcpClient;
        /// <summary>
        /// 启动部署
        /// </summary>
        /// <param name="startTime">部署任务启动时间</param>
        /// <returns>部署结果</returns>
        internal DeployResult Deploy(DateTime startTime = default(DateTime))
        {
#if NoAutoCSer
            throw new Exception();
#else
            if (TcpClient.IsClient)
            {
                AutoCSer.Net.TcpServer.ReturnValue<int> indexReuslt = TcpClient.TcpInternalClient.create();
                if (indexReuslt.Type != AutoCSer.Net.TcpServer.ReturnType.Success)
                {
                    return new DeployResult { Index = -1, State = DeployState.CreateError, ReturnType = indexReuslt.Type };
                }
                try
                {
                    Client client = TcpClient.Client;
                    Dictionary<HashString, FileSource> fileSources = DictionaryCreator.CreateHashString<FileSource>();
                    ClientTaskInfo[] tasks = new ClientTaskInfo[Tasks.Length];
                    AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Deploy.Directory> getFileDifferentReuslt;
                    for (int taskIndex = 0; taskIndex != Tasks.Length; ++taskIndex)
                    {
                        switch (Tasks[taskIndex].Type)
                        {
                            case TaskType.AssemblyFile:
                                appendSource(fileSources, (ClientTask.WebFile)Tasks[taskIndex], ref tasks[taskIndex]);
                                break;
                            case TaskType.File:
                                ClientTask.File file = (ClientTask.File)Tasks[taskIndex];
                                DirectoryInfo clientDirectory = new DirectoryInfo(file.ClientPath);
                                Directory directory = Directory.Create(clientDirectory, client.Config.FileLastWriteTime, file.SearchPatterns);
                                getFileDifferentReuslt = TcpClient.TcpInternalClient.getFileDifferent(directory, file.ServerPath);
                                if (getFileDifferentReuslt.Type != AutoCSer.Net.TcpServer.ReturnType.Success)
                                {
                                    return new DeployResult { Index = -1, State = DeployState.GetFileDifferentError, ReturnType = getFileDifferentReuslt.Type };
                                }
                                tasks[taskIndex].Directory = getFileDifferentReuslt.Value;
                                tasks[taskIndex].Directory.Load(clientDirectory);
                                break;
                            case TaskType.WebFile:
                                ClientTask.WebFile webFile = (ClientTask.WebFile)Tasks[taskIndex];
                                DirectoryInfo webClientDirectory = new DirectoryInfo(webFile.ClientPath);
                                Directory webDirectory = Directory.CreateWeb(webClientDirectory, client.Config.FileLastWriteTime);
                                getFileDifferentReuslt = TcpClient.TcpInternalClient.getFileDifferent(webDirectory, webFile.ServerPath);
                                if (getFileDifferentReuslt.Type != AutoCSer.Net.TcpServer.ReturnType.Success)
                                {
                                    return new DeployResult { Index = -1, State = DeployState.GetFileDifferentError, ReturnType = getFileDifferentReuslt.Type };
                                }
                                tasks[taskIndex].Directory = getFileDifferentReuslt.Value;
                                tasks[taskIndex].Directory.Load(webClientDirectory);
                                break;
                        }
                    }

                    if (fileSources.Count != 0)
                    {
                        AutoCSer.Net.TcpServer.ReturnValue<bool> result = TcpClient.TcpInternalClient.setFileSource(fileSources.getArray(value => value.Value.Data));
                        if (!result.Value) return new DeployResult { Index = -1, State = DeployState.SetFileSourceError, ReturnType = result.Type };
                    }
                    for (int taskIndex = 0; taskIndex != Tasks.Length; ++taskIndex)
                    {
                        AutoCSer.Net.TcpServer.ReturnValue<int> result;
                        switch (Tasks[taskIndex].Type)
                        {
                            case TaskType.Run:
                                result = TcpClient.TcpInternalClient.addRun( (ClientTask.Run)Tasks[taskIndex]);
                                if (result.Type == AutoCSer.Net.TcpServer.ReturnType.Success) tasks[taskIndex].TaskIndex = result.Value;
                                break;
                            case TaskType.WebFile:
                            case TaskType.File:
                                result = TcpClient.TcpInternalClient.addFiles(tasks[taskIndex].Directory, (ClientTask.WebFile)Tasks[taskIndex], Tasks[taskIndex].Type);
                                break;
                            case TaskType.AssemblyFile:
                                result = TcpClient.TcpInternalClient.addAssemblyFiles(tasks[taskIndex].FileIndexs.ToArray(), (ClientTask.AssemblyFile)Tasks[taskIndex]);
                                break;
                            case TaskType.WaitRunSwitch:
                                result = TcpClient.TcpInternalClient.addWaitRunSwitch(tasks[((ClientTask.WaitRunSwitch)Tasks[taskIndex]).TaskIndex].TaskIndex);
                                break;
                            case TaskType.UpdateSwitchFile:
                                result = TcpClient.TcpInternalClient.addUpdateSwitchFile((ClientTask.UpdateSwitchFile)Tasks[taskIndex]);
                                break;
                            case TaskType.Custom:
                                result = TcpClient.TcpInternalClient.addCustom((ClientTask.Custom)Tasks[taskIndex]);
                                break;
                            default: return new DeployResult { Index = -1, State = DeployState.UnknownTaskType, ReturnType = AutoCSer.Net.TcpServer.ReturnType.Unknown };
                        }
                        if (result.Type != AutoCSer.Net.TcpServer.ReturnType.Success || result.Value == -1)
                        {
                            return new DeployResult { Index = -1, State = getErrorState(Tasks[taskIndex].Type), ReturnType = result.Type };
                        }
                    }
                    AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Deploy.DeployState> startResult = TcpClient.TcpInternalClient.start(startTime);
                    if (startResult.Type == AutoCSer.Net.TcpServer.ReturnType.Success)
                    {
                        return new DeployResult { Index = indexReuslt.Value, State = startResult.Value, ReturnType = AutoCSer.Net.TcpServer.ReturnType.Success };
                    }
                    return new DeployResult { Index = -1, State = DeployState.StartError, ReturnType = startResult.Type };
                }
                finally
                {
                    TcpClient.TcpInternalClient.cancel();// && TcpClient.TcpInternalClient.clear(identity).Type != AutoCSer.Net.TcpServer.ReturnType.Success
                }
            }
#endif
            return new DeployResult { Index = -1, State = DeployState.NoClient };
        }
        /// <summary>
        /// 获取错误状态
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static DeployState getErrorState(TaskType type)
        {
            switch (type)
            {
                case TaskType.Run: return DeployState.AddRunError;
                case TaskType.WebFile:
                case TaskType.File: return DeployState.AddFileError;
                case TaskType.AssemblyFile: return DeployState.AddAssemblyFileError;
                case TaskType.WaitRunSwitch: return DeployState.AddWaitRunSwitchError;
                case TaskType.UpdateSwitchFile: return DeployState.AddUpdateSwitchFileError;
                case TaskType.Custom: return DeployState.AddCustomError;
                default: return DeployState.UnknownTaskType;
            }
        }
        /// <summary>
        /// 判断部署是否需要清理操作
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private static bool checkIsClear(DeployState state)
        {
            switch (state)
            {
                case DeployState.Success:
                case DeployState.Canceled:
                case DeployState.Exception:
                    return true;
            }
            return false;
        }
        ///// <summary>
        ///// 添加文件数据源
        ///// </summary>
        ///// <param name="fileSources"></param>
        ///// <param name="run"></param>
        ///// <param name="serverTask"></param>
        //private void appendSource(Dictionary<HashString, FileSource> fileSources, ClientTask.Run run, ref ClientTaskInfo serverTask)
        //{
        //    appendSource(fileSources, run, ref serverTask, run.FileName);
        //}
        /// <summary>
        /// 添加文件数据源
        /// </summary>
        /// <param name="fileSources"></param>
        /// <param name="webFile"></param>
        /// <param name="serverTask"></param>
        /// <param name="runFileName"></param>
        private void appendSource(Dictionary<HashString, FileSource> fileSources, ClientTask.WebFile webFile, ref ClientTaskInfo serverTask, string runFileName = null)
        {
            Client client = TcpClient.Client;
            if (webFile.ClientPath != null)
            {
                DirectoryInfo directory = new DirectoryInfo(webFile.ClientPath);
                string directoryName = directory.fullName();
                if (runFileName != null) appendSource(fileSources, directoryName, runFileName, ref serverTask);
                foreach (FileInfo file in directory.GetFiles("*.exe"))
                {
                    if (file.LastWriteTimeUtc > client.Config.FileLastWriteTime && file.Name != runFileName && !client.IgnoreFileNames.Contains(file.Name)
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
