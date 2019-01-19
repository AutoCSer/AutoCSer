using System;
using System.Diagnostics;
using AutoCSer.Extension;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using AutoCSer.Log;

namespace AutoCSer.Diagnostics
{
    /// <summary>
    /// 进程文件复制
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
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
        private ProcessCopyServer server;
        /// <summary>
        /// 日志处理
        /// </summary>
        private ILog log
        {
            get { return server == null ? Log.Pub.Log : server.TcpServer.Log; }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (server != null)
            {
                server.Remove(this);
                server = null;
            }
        }
        /// <summary>
        /// 守护进程
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        internal bool Guard(ProcessCopyServer server)
        {
            this.server = server;
            try
            {
                if ((process = System.Diagnostics.Process.GetProcessById(ProcessId)) != null && process.ProcessName == ProcessName)
                {
                    process.EnableRaisingEvents = true;
                    process.Exited += guard;
                    server.TcpServer.Log.Add(AutoCSer.Log.LogType.Info, "添加守护进程 " + Process);
                    return true;
                }
            }
            catch (Exception error)
            {
                server.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
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
            try
            {
                Dispose();
                if (System.IO.File.Exists(Process)) start();
                else log.Add(AutoCSer.Log.LogType.Error, "没有找到文件 " + Process);
            }
            catch (Exception error)
            {
                log.Add(AutoCSer.Log.LogType.Error, error, "进程启动失败 " + Process);
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
            log.Add(AutoCSer.Log.LogType.Info, "进程启动成功 " + Process);
        }
        /// <summary>
        /// 删除进程退出事件
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RemoveGuard()
        {
            if (process != null)
            {
                using (process) process.Exited -= guard;
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
            log.Add(AutoCSer.Log.LogType.Info, "启动文件复制 " + Process);
            DateTime timeout = Date.NowTime.Now.AddMinutes(Config.CopyTimeoutMinutes);
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
                    if (milliseconds == 1 << 4) log.Add(AutoCSer.Log.LogType.Error, error);
                }
                if (Date.NowTime.Now >= timeout)
                {
                    log.Add(AutoCSer.Log.LogType.Error, "文件复制超时 " + Process);
                    return;
                }
                if (milliseconds != 4 << 10) milliseconds <<= 1;
            }
        }

        /// <summary>
        /// 进程复制配置
        /// </summary>
        internal static readonly ProcessCopyConfig Config = ConfigLoader.GetUnion(typeof(ProcessCopyConfig)).ProcessCopyConfig ?? new ProcessCopyConfig();
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
