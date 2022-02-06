using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoCSer.Extensions;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 客户端部署信息
    /// </summary>
    public partial struct ClientDeploy
    {
        /// <summary>
        /// 启动部署
        /// </summary>
        /// <returns>部署结果</returns>
        internal async Task<DeployResult> DeployAsync()
        {
#if NoAutoCSer
            throw new Exception();
#else
            if (TcpClient.IsClient)
            {
                //需要加锁
                AutoCSer.Net.TcpServer.ReturnValue<int> indexReuslt = await TcpClient.TcpInternalClient.createAwaiter();
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
                        tasks[taskIndex].FileIndexs = new LeftArray<KeyValue<string, int>>(0);
                        switch (Tasks[taskIndex].Type)
                        {
                            case TaskType.AssemblyFile:
                                appendSource(fileSources, (ClientTask.WebFile)Tasks[taskIndex], ref tasks[taskIndex]);
                                break;
                            case TaskType.File:
                                ClientTask.File file = (ClientTask.File)Tasks[taskIndex];
                                DirectoryInfo clientDirectory = new DirectoryInfo(file.ClientPath);
                                Directory directory = Directory.Create(clientDirectory, client.Config.FileLastWriteTime, file.SearchPatterns);
                                getFileDifferentReuslt = await TcpClient.TcpInternalClient.getFileDifferentAwaiter(directory, file.ServerPath);
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
                                getFileDifferentReuslt = await TcpClient.TcpInternalClient.getFileDifferentAwaiter(webDirectory, webFile.ServerPath);
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
                        AutoCSer.Net.TcpServer.ReturnValue<bool> result = await TcpClient.TcpInternalClient.setFileSourceAwaiter(indexReuslt.Value, fileSources.getArray(value => value.Value.Data));
                        if (!result.Value) return new DeployResult { Index = -1, State = DeployState.SetFileSourceError, ReturnType = result.Type };
                    }
                    for (int taskIndex = 0; taskIndex != Tasks.Length; ++taskIndex)
                    {
                        AutoCSer.Net.TcpServer.ReturnValue<int> result;
                        switch (Tasks[taskIndex].Type)
                        {
                            case TaskType.Run:
                                result = await TcpClient.TcpInternalClient.addRunAwaiter(indexReuslt.Value, (ClientTask.Run)Tasks[taskIndex]);
                                if (result.Type == AutoCSer.Net.TcpServer.ReturnType.Success) tasks[taskIndex].TaskIndex = result.Value;
                                break;
                            case TaskType.WebFile:
                            case TaskType.File:
                                ClientTask.WebFile webFile = (ClientTask.WebFile)Tasks[taskIndex];
                                webFile.Directory = tasks[taskIndex].Directory;
                                result = await TcpClient.TcpInternalClient.addFilesAwaiter(indexReuslt.Value, webFile);
                                break;
                            case TaskType.AssemblyFile:
                                ClientTask.AssemblyFile assemblyFile = (ClientTask.AssemblyFile)Tasks[taskIndex];
                                assemblyFile.FileIndexs = tasks[taskIndex].FileIndexs.ToArray();
                                result = await TcpClient.TcpInternalClient.addAssemblyFilesAwaiter(indexReuslt.Value, assemblyFile);
                                break;
                            case TaskType.WaitRunSwitch:
                                result = await TcpClient.TcpInternalClient.addWaitRunSwitchAwaiter(indexReuslt.Value, tasks[((ClientTask.WaitRunSwitch)Tasks[taskIndex]).TaskIndex].TaskIndex);
                                break;
                            case TaskType.UpdateSwitchFile:
                                result = await TcpClient.TcpInternalClient.addUpdateSwitchFileAwaiter(indexReuslt.Value, (ClientTask.UpdateSwitchFile)Tasks[taskIndex]);
                                break;
                            case TaskType.Custom:
                                result = await TcpClient.TcpInternalClient.addCustomAwaiter(indexReuslt.Value, (ClientTask.Custom)Tasks[taskIndex]);
                                break;
                            default: return new DeployResult { Index = -1, State = DeployState.UnknownTaskType, ReturnType = AutoCSer.Net.TcpServer.ReturnType.Unknown };
                        }
                        if (result.Type != AutoCSer.Net.TcpServer.ReturnType.Success || result.Value == -1)
                        {
                            return new DeployResult { Index = -1, State = getErrorState(Tasks[taskIndex].Type), ReturnType = result.Type };
                        }
                    }
                    AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Deploy.DeployResultData> startResult = await TcpClient.TcpInternalClient.startAwaiter(indexReuslt.Value, DateTime.MinValue);
                    if (startResult.Type == AutoCSer.Net.TcpServer.ReturnType.Success)
                    {
                        return new DeployResult { Index = indexReuslt.Value, State = startResult.Value.State, Data = startResult.Value.Data, ReturnType = AutoCSer.Net.TcpServer.ReturnType.Success };
                    }
                    return new DeployResult { Index = -1, State = DeployState.StartError, ReturnType = startResult.Type };
                }
                finally
                {
                    await TcpClient.TcpInternalClient.cancelAwaiter(indexReuslt.Value);// && TcpClient.TcpInternalClient.clear(identity).Type != AutoCSer.Net.TcpServer.ReturnType.Success
                }
            }
#endif
            return new DeployResult { Index = -1, State = DeployState.NoClient };
        }
    }
}
