using System;
using System.Diagnostics;
using AutoCSer.Extensions;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using AutoCSer.Log;

namespace AutoCSer.Diagnostics
{
    /// <summary>
    /// 进程文件复制
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class ProcessCopyer : IDisposable
    {
        /// <summary>
        /// 进程标识
        /// </summary>
        internal int ProcessId;
        /// <summary>
        /// 进程名称
        /// </summary>
        internal string ProcessName;
        /// <summary>
        /// 目标路径
        /// </summary>
        internal string Path;
        /// <summary>
        /// 复制文件源路径
        /// </summary>
        internal string CopyPath;
        /// <summary>
        /// 进程文件名
        /// </summary>
        internal string Process;
        /// <summary>
        /// 进程启动参数
        /// </summary>
        internal string Arguments;
        /// <summary>
        /// 进程信息
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        private Process process;
        /// <summary>
        /// 进程复制重启服务
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal ProcessCopyServer Server;
        /// <summary>
        /// 守护事件
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        private EventHandler guardEvent;
        /// <summary>
        /// 日志处理
        /// </summary>
        private ILog log
        {
            get { return Server == null ? AutoCSer.LogHelper.Default : Server.TcpServer.Log; }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Server != null)
            {
                Server.Remove(this);
                Server = null;
                //new ProcessCopyerServerCall(this, ProcessCopyerServerCall.CallType.Remove).CheckCall(Net.TcpServer.ServerTaskType.QueueLink, Server.TcpServer);
            }
        }
        /// <summary>
        /// 守护进程
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        internal bool Guard(ProcessCopyServer server)
        {
            Server = server;
            try
            {
                if ((process = System.Diagnostics.Process.GetProcessById(ProcessId)) != null && process.ProcessName == ProcessName)
                {
                    if (guardEvent == null) guardEvent = guard;
                    process.EnableRaisingEvents = true;
                    process.Exited += guardEvent;
                    server.TcpServer.Log.Info("添加守护进程 " + Process, LogLevel.Info | LogLevel.AutoCSer);
                    return true;
                }
            }
            catch (Exception error)
            {
                server.TcpServer.Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
            }
            return false;
        }
        /// <summary>
        /// 进程退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void guard(object sender, EventArgs e)
        {
            Server.TcpServer.CallQueueLink.Add(new ProcessCopyerServerCall(this, ProcessCopyerServerCall.CallType.Start));
        }
        /// <summary>
        /// 守护启动进程
        /// </summary>
        internal void Start()
        {
            try
            {
                ProcessCopyServer server = Server;
                if (server != null)
                {
                    Server = null;
                    server.Remove(this);
                }
                if (System.IO.File.Exists(Process)) start();
                else log.Error("没有找到文件 " + Process, LogLevel.Error | LogLevel.AutoCSer);
            }
            catch (Exception error)
            {
                log.Exception(error, "进程启动失败 " + Process, LogLevel.Exception  | LogLevel.AutoCSer);
            }
        }
        /// <summary>
        /// 启动进程
        /// </summary>
        private void start()
        {
            ProcessStartInfo info = new ProcessStartInfo(Process, Arguments);
            info.UseShellExecute = true;
            info.WorkingDirectory = Path;
            System.Diagnostics.Process.Start(info);
            log.Info("进程启动成功 " + Process, LogLevel.Info | LogLevel.AutoCSer);
        }
        /// <summary>
        /// 删除进程退出事件
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RemoveGuard()
        {
            if (process != null)
            {
                using (process)
                {
                    if (guardEvent != null)
                    {
                        process.Exited -= guardEvent;
                        guardEvent = null;
                    }
                }
                process = null;
            }
        }
        /// <summary>
        /// 验证进程名称
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CheckName()
        {
            Process process = System.Diagnostics.Process.GetProcessById(ProcessId);
            if (process != null)
            {
                using (process) return process.ProcessName == ProcessName;
            }
            return false;
        }
        /// <summary>
        /// 进程文件复制
        /// </summary>
        /// <returns></returns>
        private bool copy()
        {
            DirectoryInfo directory = new DirectoryInfo(Path);
            if (!directory.Exists) directory.Create();
            DirectoryInfo copyDirectory = new DirectoryInfo(CopyPath);
            Path = directory.fullName();
            if (copyDirectory.Exists)
            {
                foreach (FileInfo file in copyDirectory.GetFiles()) file.CopyTo(Path + file.Name, true);
                return true;
            }
            return System.IO.File.Exists(Process);
        }
        /// <summary>
        /// 开始复制文件
        /// </summary>
        internal void Copy()
        {
            log.Info("启动文件复制 " + Process, LogLevel.Info | LogLevel.AutoCSer);
            DateTime timeout = AutoCSer.Threading.SecondTimer.Now.AddMinutes(Config.CopyTimeoutMinutes);
            for (int milliseconds = 1 << 4; true;)
            {
                Thread.Sleep(milliseconds);
                try
                {
                    if (copy())
                    {
                        start();
                        return;
                    }
                }
                catch (Exception error)
                {
                    if (milliseconds == 1 << 4) log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                }
                if (AutoCSer.Threading.SecondTimer.Now >= timeout)
                {
                    log.Error("文件复制超时 " + Process, LogLevel.Error | LogLevel.AutoCSer);
                    return;
                }
                if (milliseconds != 4 << 10) milliseconds <<= 1;
            }
        }

        /// <summary>
        /// 进程复制配置
        /// </summary>
        internal static readonly ProcessCopyConfig Config = (ProcessCopyConfig)AutoCSer.Configuration.Common.Get(typeof(ProcessCopyConfig)) ?? new ProcessCopyConfig();
        /// <summary>
        /// 默认文件复制器
        /// </summary>
        private static ProcessCopyer defaultCopyer;
        /// <summary>
        /// 默认文件复制器
        /// </summary>
        internal static ProcessCopyer Default
        {
            get
            {
                if (defaultCopyer == null)
                {
                    string command = Environment.CommandLine;
                    int index = command.IndexOf(' ') + 1;
                    using (Process process = System.Diagnostics.Process.GetCurrentProcess())
                    {
                        FileInfo file = new FileInfo(process.MainModule.FileName);
                        defaultCopyer = new ProcessCopyer
                        {
                            ProcessId = process.Id,
                            ProcessName = process.ProcessName,
                            Process = file.FullName,
                            Path = file.Directory.FullName,
                            CopyPath = Config.WatcherPath,
                            Arguments = index == 0 || index == command.Length ? null : command.Substring(index)
                        };
                    }
                }
                return defaultCopyer;
            }
        }
    }
}
