using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoCSer.Extension;

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
                AutoCSer.Net.IndexIdentity identity = await TcpClient.TcpInternalClient.createAwaiter(TcpClient.ClientId);
                bool isClear = true;
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
                                appendSource(fileSources, (ClientTask.Run)Tasks[taskIndex], ref tasks[taskIndex]);
                                break;
                            case TaskType.AssemblyFile:
                                appendSource(fileSources, (ClientTask.WebFile)Tasks[taskIndex], ref tasks[taskIndex]);
                                break;
                            case TaskType.File:
                                ClientTask.File file = (ClientTask.File)Tasks[taskIndex];
                                DirectoryInfo clientDirectory = new DirectoryInfo(file.ClientPath);
                                Directory directory = Directory.Create(clientDirectory, client.Config.FileLastWriteTime, file.SearchPatterns);
                                tasks[taskIndex].Directory = await TcpClient.TcpInternalClient.getFileDifferentAwaiter(directory, file.ServerPath);
                                tasks[taskIndex].Directory.Load(clientDirectory);
                                break;
                            case TaskType.WebFile:
                                ClientTask.WebFile webFile = (ClientTask.WebFile)Tasks[taskIndex];
                                DirectoryInfo webClientDirectory = new DirectoryInfo(webFile.ClientPath);
                                Directory webDirectory = Directory.CreateWeb(webClientDirectory, client.Config.FileLastWriteTime);
                                tasks[taskIndex].Directory = await TcpClient.TcpInternalClient.getFileDifferentAwaiter(webDirectory, webFile.ServerPath);
                                tasks[taskIndex].Directory.Load(webClientDirectory);
                                break;
                        }
                    }

                    if (fileSources.Count != 0 && !(await TcpClient.TcpInternalClient.setFileSourceAwaiter(identity, fileSources.getArray(value => value.Value.Data))))
                    {
                        return new DeployResult { Index = -1, State = DeployState.SetFileSourceError };
                    }
                    for (int taskIndex = 0; taskIndex != Tasks.Length; ++taskIndex)
                    {
                        switch (Tasks[taskIndex].Type)
                        {
                            case TaskType.Run:
                                if ((tasks[taskIndex].TaskIndex = await TcpClient.TcpInternalClient.addRunAwaiter(identity, tasks[taskIndex].FileIndexs.ToArray(), (ClientTask.Run)Tasks[taskIndex])) == -1)
                                {
                                    return new DeployResult { Index = -1, State = DeployState.AddRunError };
                                }
                                break;
                            case TaskType.WebFile:
                            case TaskType.File:
                                if (await TcpClient.TcpInternalClient.addFilesAwaiter(identity, tasks[taskIndex].Directory, (ClientTask.WebFile)Tasks[taskIndex], Tasks[taskIndex].Type) == -1)
                                {
                                    return new DeployResult { Index = -1, State = DeployState.AddFileError };
                                }
                                break;
                            case TaskType.AssemblyFile:
                                if (await TcpClient.TcpInternalClient.addAssemblyFilesAwaiter(identity, tasks[taskIndex].FileIndexs.ToArray(), (ClientTask.AssemblyFile)Tasks[taskIndex]) == -1)
                                {
                                    return new DeployResult { Index = -1, State = DeployState.AddAssemblyFileError };
                                }
                                break;
                            case TaskType.WaitRunSwitch:
                                if (await TcpClient.TcpInternalClient.addWaitRunSwitchAwaiter(identity, tasks[((ClientTask.WaitRunSwitch)Tasks[taskIndex]).TaskIndex].TaskIndex) == -1)
                                {
                                    return new DeployResult { Index = -1, State = DeployState.AddWaitRunSwitchError };
                                }
                                break;
                            case TaskType.Custom:
                                if (await TcpClient.TcpInternalClient.addCustomAwaiter(identity, (ClientTask.Custom)Tasks[taskIndex]) == -1)
                                {
                                    return new DeployResult { Index = -1, State = DeployState.AddCustomError };
                                }
                                break;
                        }
                    }
                    AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Deploy.DeployState> result = await TcpClient.TcpInternalClient.startAwaiter(identity, DateTime.MinValue);
                    if (result.Type == AutoCSer.Net.TcpServer.ReturnType.Success)
                    {
                        isClear = checkIsClear(result.Value);
                        return new DeployResult { Index = identity.Index, State = result.Value };
                    }
                    return new DeployResult { Index = -1, State = DeployState.StartError };
                }
                finally
                {
                    if (isClear) await TcpClient.TcpInternalClient.clearAwaiter(identity);// && TcpClient.TcpInternalClient.clear(identity).Type != AutoCSer.Net.TcpServer.ReturnType.Success
                }
            }
#endif
            return new DeployResult { Index = -1, State = DeployState.NoClient };
        }
    }
}
