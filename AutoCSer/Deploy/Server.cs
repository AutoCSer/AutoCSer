using System;
using AutoCSer.Net;
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Reflection;
using AutoCSer.Extensions;
using System.Diagnostics;
using AutoCSer.Net.TcpInternalServer;
using System.Text.RegularExpressions;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 部署服务
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Name = Server.ServerName, Host = "127.0.0.1", Port = (int)ServerPort.Deploy, MinCompressSize = 1024, CommandIdentityEnmuType = typeof(Server.Command))]
    public partial class Server : AutoCSer.Net.TcpInternalServer.TimeVerifyServer
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = "Deploy";
        /// <summary>
        /// 部署服务命令
        /// </summary>
        public enum Command
        {
            /// <summary>
            /// 时间验证函数
            /// </summary>
            verify = 1,
            /// <summary>
            /// 写文件
            /// </summary>
            addAssemblyFiles,
            /// <summary>
            /// 添加自定义任务
            /// </summary>
            addCustom,
            /// <summary>
            /// 添加web任务(css/js/html)
            /// </summary>
            addFiles,
            /// <summary>
            /// 写文件并运行程序
            /// </summary>
            addRun,
            /// <summary>
            /// 等待运行程序切换结束
            /// </summary>
            addWaitRunSwitch,
            /// <summary>
            /// 发布切换更新
            /// </summary>
            addUpdateSwitchFile,
            /// <summary>
            /// 创建部署
            /// </summary>
            create,
            /// <summary>
            /// 取消部署信息
            /// </summary>
            cancel,
            /// <summary>
            /// 启动部署
            /// </summary>
            start,
            /// <summary>
            /// 自定义服务端推送
            /// </summary>
            customPush,
            /// <summary>
            /// 部署任务状态轮询
            /// </summary>
            getLog,
            /// <summary>
            /// 比较文件最后修改时间
            /// </summary>
            getFileDifferent,
            /// <summary>
            /// 设置文件数据源
            /// </summary>
            setFileSource,
        }

        /// <summary>
        /// 切换服务
        /// </summary>
        /// <param name="SwitchFile"></param>
        public void Switch(FileInfo SwitchFile)
        {
            server.StopListen();
            try
            {
                beforeSwitch();
            }
            finally { SwitchFile.StartProcessDirectory(); }
        }
        /// <summary>
        /// 切换服务前的调用
        /// </summary>
        protected virtual void beforeSwitch() { }
        /// <summary>
        /// 时间验证函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="userID">用户ID</param>
        /// <param name="randomPrefix">随机前缀</param>
        /// <param name="md5Data">MD5 数据</param>
        /// <param name="ticks">验证时钟周期</param>
        /// <returns>是否验证成功</returns>
        [AutoCSer.Net.TcpServer.Method(IsVerifyMethod = true, ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected override bool verify(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, string userID, ulong randomPrefix, byte[] md5Data, ref long ticks)
        {
            if (base.verify(sender, userID, randomPrefix, md5Data, ref ticks))
            {
                sender.ClientObject = createClientObject();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 创建客户端对象
        /// </summary>
        /// <returns></returns>
        protected virtual ClientObject createClientObject()
        {
            return new ClientObject();
        }
        /// <summary>
        /// 部署任务状态轮询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="onLog">部署任务状态更新回调</param>
        [AutoCSer.Net.TcpServer.KeepCallbackMethod(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected virtual void getLog(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, AutoCSer.Net.TcpServer.ServerCallback<Log> onLog)
        {
            ((ClientObject)sender.ClientObject).OnLog = onLog;
        }
        /// <summary>
        /// 发布编号
        /// </summary>
        private int deployIndex;
        /// <summary>
        /// 创建部署
        /// </summary>
        /// <param name="sender"></param>
        /// <returns>发布编号</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected virtual int create(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender)
        {
            return new DeployInfo((ClientObject)sender.ClientObject, ++deployIndex).Index;
        }
        /// <summary>
        /// 取消部署信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deployIndex">发布编号</param>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected virtual void cancel(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, int deployIndex)
        {
            ((ClientObject)sender.ClientObject).Deploys.Remove(deployIndex);
        }
        /// <summary>
        /// 启动部署
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deployIndex">发布编号</param>
        /// <param name="time">启动时间</param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.ThreadPool, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected virtual DeployResultData start(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, int deployIndex, DateTime time)
        {
            DeployInfo deployInfo = ((ClientObject)sender.ClientObject).GetDeploy(deployIndex);
            if (deployInfo == null) return DeployState.StartError;
            return deployInfo.Start(time, new Timer { Server = this });
        }
        /// <summary>
        /// 设置文件数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deployIndex">发布编号</param>
        /// <param name="files">文件数据源</param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected virtual bool setFileSource(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, int deployIndex, byte[][] files)
        {
            DeployInfo deployInfo = ((ClientObject)sender.ClientObject).GetDeploy(deployIndex);
            if (deployInfo != null)
            {
                deployInfo.Files = files;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 比较文件最后修改时间
        /// </summary>
        /// <param name="directory">目录信息</param>
        /// <param name="serverPath">服务器端路径</param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.ThreadPool, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected virtual Directory getFileDifferent(Directory directory, string serverPath)
        {
            directory.Different(new DirectoryInfo(serverPath));
            return directory;
        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deployIndex">发布编号</param>
        /// <param name="webFile">写文件任务信息</param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected virtual int addFiles(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, int deployIndex, ClientTask.WebFile webFile)
        {
            DeployInfo deployInfo = ((ClientObject)sender.ClientObject).GetDeploy(deployIndex);
            if (deployInfo != null) return deployInfo.AddTask(webFile);
            return -1;
        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deployIndex">发布编号</param>
        /// <param name="assemblyFile">写文件 exe/dll/pdb 任务信息</param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected virtual int addAssemblyFiles(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, int deployIndex, ClientTask.AssemblyFile assemblyFile)
        {
            DeployInfo deployInfo = ((ClientObject)sender.ClientObject).GetDeploy(deployIndex);
            if (deployInfo != null) return deployInfo.AddTask(assemblyFile);
            return -1;
        }
        /// <summary>
        /// 写文件并运行程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deployIndex">发布编号</param>
        /// <param name="run">写文件并运行程序 任务信息</param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected virtual int addRun(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, int deployIndex, ClientTask.Run run)
        {
            DeployInfo deployInfo = ((ClientObject)sender.ClientObject).GetDeploy(deployIndex);
            if (deployInfo != null) return deployInfo.AddTask(run);
            return -1;
        }
        /// <summary>
        /// 等待运行程序切换结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deployIndex">发布编号</param>
        /// <param name="taskIndex">任务索引位置</param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected virtual int addWaitRunSwitch(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, int deployIndex, int taskIndex)
        {
            DeployInfo deployInfo = ((ClientObject)sender.ClientObject).GetDeploy(deployIndex);
            if (deployInfo != null)
            {
                return deployInfo.AddTask(new ClientTask.WaitRunSwitch { TaskIndex = taskIndex });
            }
            return -1;
        }
        /// <summary>
        /// 发布切换更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deployIndex">发布编号</param>
        /// <param name="updateSwitchFile">发布切换更新</param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected virtual int addUpdateSwitchFile(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, int deployIndex, ClientTask.UpdateSwitchFile updateSwitchFile)
        {
            DeployInfo deployInfo = ((ClientObject)sender.ClientObject).GetDeploy(deployIndex);
            if (deployInfo != null) return deployInfo.AddTask(updateSwitchFile);
            return -1;
        }
        /// <summary>
        /// 添加自定义任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deployIndex">发布编号</param>
        /// <param name="custom">自定义任务处理 任务信息</param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected virtual int addCustom(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, int deployIndex, ClientTask.Custom custom)
        {
            DeployInfo deployInfo = ((ClientObject)sender.ClientObject).GetDeploy(deployIndex);
            if (deployInfo != null)
            {
                custom.Sender = sender;
                return deployInfo.AddTask(custom);
            }
            return -1;
        }
        /// <summary>
        /// 自定义服务端推送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="onPush">推送委托</param>
        [AutoCSer.Net.TcpServer.KeepCallbackMethod(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.QueueLink, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        protected virtual void customPush(AutoCSer.Net.TcpInternalServer.ServerSocketSender sender, AutoCSer.Net.TcpServer.ServerCallback<byte[]> onPush)
        {
            ((ClientObject)sender.ClientObject).OnCustomPush = onPush;
        }
        /// <summary>
        /// 自定义任务处理
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public virtual DeployResultData CallCustomTask(ClientTask.Custom task)
        {
            return DeployState.CustomError;
        }

        /// <summary>
        /// 默认切换服务相对目录名称
        /// </summary>
        public const string DefaultSwitchDirectoryName = "Switch";
        /// <summary>
        /// 默认更新服务相对目录名称
        /// </summary>
        public const string DefaultUpdateDirectoryName = "Update";
        ///// <summary>
        ///// 发布服务更新以后的后续处理
        ///// </summary>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //public void OnDeployServerUpdated()
        //{
        //    OnDeployServerUpdated(null);
        //}
        /// <summary>
        /// 发布服务更新以后的后续处理
        /// </summary>
        /// <param name="deployServerFileName">发布服务文件名称</param>
        /// <param name="switchDirectoryName">切换服务相对目录名称</param>
        /// <param name="updateDirectoryName">更新服务相对目录名称</param>
        public void OnDeployServerUpdated(string deployServerFileName = null, string switchDirectoryName = DefaultSwitchDirectoryName, string updateDirectoryName = DefaultUpdateDirectoryName)
        {
            if (switchDirectoryName == null) switchDirectoryName = DefaultSwitchDirectoryName;
            if (updateDirectoryName == null) updateDirectoryName = DefaultUpdateDirectoryName;
            DirectoryInfo currentDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory ?? Environment.CurrentDirectory), switchDirectory, updateDirectory;
            if (currentDirectory.Name == switchDirectoryName)
            {
                switchDirectory = currentDirectory.Parent;
                updateDirectory = new DirectoryInfo(Path.Combine(switchDirectory.FullName, updateDirectoryName));
            }
            else
            {
                switchDirectory = new DirectoryInfo(Path.Combine(currentDirectory.FullName, switchDirectoryName));
                if (!switchDirectory.Exists) switchDirectory.Create();
                updateDirectory = new DirectoryInfo(Path.Combine(currentDirectory.FullName, updateDirectoryName));
            }
            if (deployServerFileName == null) deployServerFileName = new FileInfo(deployServerFileName = Assembly.GetEntryAssembly().Location).Name;
            FileInfo updateFile = new FileInfo(Path.Combine(updateDirectory.FullName, deployServerFileName)), switchFile = new FileInfo(Path.Combine(switchDirectory.FullName, deployServerFileName));
            if (updateFile.LastWriteTimeUtc > switchFile.LastWriteTimeUtc)
            {
                foreach (FileInfo file in updateDirectory.GetFiles())
                {
                    FileInfo removeFile = new FileInfo(Path.Combine(switchDirectory.FullName, file.Name));
                    if (removeFile.Exists) removeFile.Delete();
                    file.MoveTo(removeFile.FullName);
                }
                Switch(new FileInfo(switchFile.FullName));
            }
        }
        /// <summary>
        /// 初始化时获取切换服务文件
        /// </summary>
        /// <param name="deployServerFileName">发布服务文件名称</param>
        /// <param name="switchDirectoryName">切换服务相对目录名称</param>
        /// <returns>切换服务文件</returns>
        public static FileInfo GetSwitchFile(string deployServerFileName = null, string switchDirectoryName = DefaultSwitchDirectoryName)
        {
            DirectoryInfo CurrentDirectory = new DirectoryInfo(AutoCSer.Config.ApplicationPath), SwitchDirectory;
            if (CurrentDirectory.Name == switchDirectoryName)
            {
                SwitchDirectory = CurrentDirectory.Parent;
            }
            else
            {
                SwitchDirectory = new DirectoryInfo(Path.Combine(CurrentDirectory.FullName, switchDirectoryName));
            }
            if (SwitchDirectory.Exists)
            {
                if (deployServerFileName == null) deployServerFileName = new FileInfo(deployServerFileName = Assembly.GetEntryAssembly().Location).Name;
                FileInfo SwitchFile = new FileInfo(Path.Combine(SwitchDirectory.FullName, deployServerFileName));
                if (SwitchFile.Exists)
                {
                    FileInfo CurrentFile = new FileInfo(Path.Combine(CurrentDirectory.FullName, deployServerFileName));
                    if (SwitchFile.LastWriteTimeUtc > CurrentFile.LastWriteTimeUtc) return SwitchFile;
                }
            }
            return null;
        }
        /// <summary>
        /// 发布切换更新，返回当前检测文件
        /// </summary>
        /// <param name="deployPath">发布目标路径</param>
        /// <param name="checkFileName">检测文件名称</param>
        /// <param name="switchDirectoryName">切换服务相对目录名称</param>
        /// <param name="updateDirectoryName">更新服务相对目录名称</param>
        /// <returns>当前检测文件</returns>
        public static FileInfo UpdateSwitchFile(string deployPath, string checkFileName, string switchDirectoryName = DefaultSwitchDirectoryName, string updateDirectoryName = DefaultUpdateDirectoryName)
        {
            string otherFileName;
            return UpdateSwitchFile(deployPath, checkFileName, switchDirectoryName, updateDirectoryName, out otherFileName);
        }
        /// <summary>
        /// 发布切换更新，返回当前检测文件
        /// </summary>
        /// <param name="deployPath">发布目标路径</param>
        /// <param name="checkFileName">检测文件名称</param>
        /// <param name="switchDirectoryName">切换服务相对目录名称</param>
        /// <param name="updateDirectoryName">更新服务相对目录名称</param>
        /// <param name="otherFileName">另外一个待运行文件名称</param>
        /// <returns>当前检测文件</returns>
        internal static FileInfo UpdateSwitchFile(string deployPath, string checkFileName, string switchDirectoryName, string updateDirectoryName, out string otherFileName)
        {
            DirectoryInfo directory = new DirectoryInfo(deployPath), moveToDirectory = directory;
            DirectoryInfo switchDirectory = new DirectoryInfo(Path.Combine(directory.FullName, switchDirectoryName ?? DefaultSwitchDirectoryName)), otherDirectory = switchDirectory;
            FileInfo fileInfo = new FileInfo(Path.Combine(directory.FullName, checkFileName));
            if (fileInfo.Exists)
            {
                if (switchDirectory.Exists)
                {
                    FileInfo switchFileInfo = new FileInfo(Path.Combine(switchDirectory.FullName, fileInfo.Name));
                    if (!switchFileInfo.Exists || switchFileInfo.LastWriteTimeUtc < fileInfo.LastWriteTimeUtc)
                    {
                        moveToDirectory = switchDirectory;
                        otherDirectory = directory;
                    }
                }
                else
                {
                    moveToDirectory = switchDirectory;
                    otherDirectory = directory;
                }
            }
            DirectoryInfo updateDirectory = new DirectoryInfo(Path.Combine(directory.FullName, updateDirectoryName ?? DefaultUpdateDirectoryName));
            copyUpdateSwitchFile(updateDirectory, moveToDirectory.FullName);

            otherFileName = Path.Combine(otherDirectory.FullName, fileInfo.Name);
            return new FileInfo(Path.Combine(moveToDirectory.FullName, fileInfo.Name));
        }
        /// <summary>
        /// 复制更新文件
        /// </summary>
        /// <param name="updateDirectory"></param>
        /// <param name="moveToDirectoryName"></param>
        private static void copyUpdateSwitchFile(DirectoryInfo updateDirectory, string moveToDirectoryName)
        {
            if (!System.IO.Directory.Exists(moveToDirectoryName)) System.IO.Directory.CreateDirectory(moveToDirectoryName);
            foreach (DirectoryInfo nextUpdateDirectory in updateDirectory.GetDirectories())
            {
                copyUpdateSwitchFile(nextUpdateDirectory, Path.Combine(moveToDirectoryName, nextUpdateDirectory.Name));
            }
            foreach (FileInfo file in updateDirectory.GetFiles())
            {
                FileInfo removeFile = new FileInfo(Path.Combine(moveToDirectoryName, file.Name));
                if (removeFile.Exists) removeFile.Delete();
                file.MoveTo(removeFile.FullName);
            }
        }
#if !DotNetStandard
        /// <summary>
        /// 检测日志输出
        /// </summary>
        /// <param name="isCheckRun">是否检测运行状态，false 表示输出所有线程信息</param>
        public static void CheckThreadLog(bool isCheckRun = true)
        {
            LeftArray<AutoCSer.Threading.Thread> threads = AutoCSer.Threading.Thread.GetThreads(isCheckRun).getLeftArray();
            AutoCSer.LogHelper.Debug("线程数量 " + threads.Length.toString());
            int currentId = Thread.CurrentThread.ManagedThreadId;
            foreach (AutoCSer.Threading.Thread threadInfo in threads)
            {
                Thread thread = threadInfo.Handle;
                if (thread.ManagedThreadId != currentId)
                {
                    StackTrace stack = null;
                    Exception exception = null;
                    System.Threading.ThreadState threadState = default(System.Threading.ThreadState);
                    bool isSuspend = false;
                    try
                    {
                        threadState = thread.ThreadState;
#pragma warning disable 618
                        if ((threadState & (System.Threading.ThreadState.StopRequested | System.Threading.ThreadState.SuspendRequested | System.Threading.ThreadState.Stopped | System.Threading.ThreadState.AbortRequested | System.Threading.ThreadState.Aborted)) == 0)
                        {
                            if ((threadState & (System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.WaitSleepJoin | System.Threading.ThreadState.Suspended)) == 0)
                            {
                                thread.Suspend();
                                isSuspend = true;
                            }
                            stack = new StackTrace(thread, true);
                        }
#pragma warning restore 618
                        //if (stack.FrameCount == AutoCSer.Threading.Thread.DefaultFrameCount) stack = null;
                    }
                    catch (ThreadStateException)
                    {
                        if ((threadState & System.Threading.ThreadState.WaitSleepJoin) == 0)
                        {
                            AutoCSer.LogHelper.Debug(threadState.ToString());
                        }
                    }
                    catch (Exception error)
                    {
                        exception = error;
                    }
                    finally
                    {
#pragma warning disable 618
                        if (isSuspend) thread.Resume();
#pragma warning restore 618
                    }
                    if (exception != null)
                    {
                        try
                        {
                            AutoCSer.LogHelper.Exception(exception);
                        }
                        catch { }
                    }
                    if (stack != null)
                    {
                        try
                        {
                            AutoCSer.LogHelper.Debug(stack.ToString(),  LogLevel.Debug | LogLevel.AutoCSer);
                        }
                        catch { }
                    }
                }
            }
        }
#endif
        /// <summary>
        /// 获取申请进程排他锁名称
        /// </summary>
        /// <returns></returns>
        private static string GetProcessEventWaitHandleName()
        {
            Assembly Assembly = Assembly.GetEntryAssembly();
            if (Assembly == null) throw new ArgumentNullException("Name is null");
            return Assembly.FullName;
        }
        /// <summary>
        /// 尝试申请进程排他锁
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static EventWaitHandle TryCreateProcessEventWaitHandle(string Name = null)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, Name ?? GetProcessEventWaitHandleName(), out createdProcessWait);
            return createdProcessWait ? processWait : null;
        }
        /// <summary>
        /// 设置申请进程排他锁
        /// </summary>
        /// <param name="Name"></param>
        public static void SetProcessEventWaitHandle(string Name = null)
        {
            using (EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, Name ?? GetProcessEventWaitHandleName()))
            {
                processWait.Set();
            }
        }
        /// <summary>
        /// 获取备份文件夹名称
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static string GetBakDirectoryName(int index)
        {
            return AutoCSer.Threading.SecondTimer.Now.ToString("yyyyMMddHHmmss_" + index.toString());
        }
        /// <summary>
        /// 备份文件夹名称正则表达式
        /// </summary>
        private static readonly Regex BakDirectoryNameRegex = new Regex(@"^\d{14}_\d+$", RegexOptions.Compiled);
        /// <summary>
        /// 备份文件夹根目录
        /// </summary>
        public static readonly DirectoryInfo BootBakDirectory = new DirectoryInfo("A").Parent;
        /// <summary>
        /// 清理备份文件夹
        /// </summary>
        /// <param name="DeleteTime"></param>
        public static void DeleteBakDirectory(DateTime DeleteTime)
        {
            foreach (DirectoryInfo Directory in BootBakDirectory.GetDirectories())
            {
                if (Directory.CreationTimeUtc < DeleteTime && BakDirectoryNameRegex.IsMatch(Directory.Name)) Directory.Delete(true);
            }
        }
    }
}
