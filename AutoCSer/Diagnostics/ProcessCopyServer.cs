using System;
using AutoCSer.Net;
using System.IO;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace AutoCSer.Diagnostics
{
    /// <summary>
    /// 进程复制重启服务
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Name = ProcessCopyServer.ServerName, Host = "127.0.0.1", Port = (int)ServerPort.ProcessCopy)]
    public partial class ProcessCopyServer : AutoCSer.Net.TcpInternalServer.TimeVerifyServer
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = "ProcessCopy";
        /// <summary>
        /// 进程守护缓存文件名
        /// </summary>
        private string cacheFile;
        /// <summary>
        /// 守护进程集合
        /// </summary>
        private readonly Dictionary<int, ProcessCopyer> guards = DictionaryCreator.CreateInt<ProcessCopyer>();
        /// <summary>
        /// 守护进程集合访问锁
        /// </summary>
        private readonly object guardLock = new object();
        /// <summary>
        /// 设置TCP服务端
        /// </summary>
        /// <param name="tcpServer">TCP服务端</param>
        public override void SetTcpServer(AutoCSer.Net.TcpInternalServer.Server tcpServer)
        {
            base.SetTcpServer(tcpServer);
            bool isError = false;
            cacheFile = ServerName + (ServerName == tcpServer.Attribute.ServerName ? null : ("_" + tcpServer.Attribute.ServerName)) + ".cache";
            if (File.Exists(cacheFile))
            {
                try
                {
                    foreach (ProcessCopyer copyer in AutoCSer.BinarySerialize.DeSerializer.DeSerialize<ProcessCopyer[]>(File.ReadAllBytes(cacheFile)))
                    {
                        if (copyer.Guard(this)) guards.Add(copyer.ProcessId, copyer);
                        else isError = true;
                    }
                }
                catch (Exception error)
                {
                    isError = true;
                    tcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
                }
                if (isError) saveCache();
            }
        }
        /// <summary>
        /// 保存进程守护信息集合到缓存文件
        /// </summary>
        private void saveCache()
        {
            Monitor.Enter(guardLock);
            try
            {
                ProcessCopyer[] cache = guards.Values.getArray();
                try
                {
                    if (cache.Length == 0) File.Delete(cacheFile);
                    else File.WriteAllBytes(cacheFile, AutoCSer.BinarySerialize.Serializer.Serialize(cache));
                }
                catch (Exception error)
                {
                    server.Log.Add(AutoCSer.Log.LogType.Error, error);
                }
            }
            finally { Monitor.Exit(guardLock); }
        }
        /// <summary>
        /// 删除守护进程
        /// </summary>
        /// <param name="copyer">文件信息</param>
        internal void Remove(ProcessCopyer copyer)
        {
            ProcessCopyer cache;
            Monitor.Enter(guardLock);
            try
            {
                if (guards.TryGetValue(copyer.ProcessId, out cache) && cache == copyer)
                {
                    guards.Remove(copyer.ProcessId);
                    saveCache();
                }
            }
            finally { Monitor.Exit(guardLock); }
            if (cache == copyer) cache.RemoveGuard();
        }
        /// <summary>
        /// 守护进程
        /// </summary>
        /// <param name="copyer">文件信息</param>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private void guard(ProcessCopyer copyer)
        {
            if (copyer.CheckName())
            {
                ProcessCopyer cache;
                Monitor.Enter(guardLock);
                try
                {
                    if (guards.TryGetValue(copyer.ProcessId, out cache))
                    {
                        if (copyer.Guard(this))
                        {
                            guards[copyer.ProcessId] = copyer;
                            cache.RemoveGuard();
                            saveCache();
                        }
                    }
                    else if (copyer.Guard(this))
                    {
                        guards.Add(copyer.ProcessId, copyer);
                        saveCache();
                    }
                }
                finally { Monitor.Exit(guardLock); }
            }
        }
        /// <summary>
        /// 删除守护进程
        /// </summary>
        /// <param name="copyer">文件信息</param>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private void remove(ProcessCopyer copyer)
        {
            if (copyer.CheckName())
            {
                ProcessCopyer cache;
                Monitor.Enter(guardLock);
                try
                {
                    if (guards.TryGetValue(copyer.ProcessId, out cache) && cache.ProcessName == copyer.ProcessName)
                    {
                        guards.Remove(copyer.ProcessId);
                        saveCache();
                    }
                }
                finally { Monitor.Exit(guardLock); }
                if (cache != null) cache.RemoveGuard();
            }
        }
        /// <summary>
        /// 复制重启进程
        /// </summary>
        /// <param name="copyer">文件复制器</param>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private void copy(ProcessCopyer copyer)
        {
            if (copyer.CheckName())
            {
                bool isGuard;
                Monitor.Enter(guardLock);
                try
                {
                    if (isGuard = guards.Remove(copyer.ProcessId)) saveCache();
                }
                finally { Monitor.Exit(guardLock); }
                AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(copyer, AutoCSer.Threading.Thread.CallType.ProcessCopyer);
            }
        }

        /// <summary>
        /// 文件监视器过滤
        /// </summary>
        /// <param name="e"></param>
        /// <returns>是否继续检测</returns>
        internal static unsafe bool FileWatcherFilter(FileSystemEventArgs e)
        {
            string name = e.FullPath;
            if (name.Length > 4)
            {
                fixed (char* nameFixed = name)
                {
                    char* end = nameFixed + name.Length;
                    int code = *(end - 4) | (*(end - 3) << 8) | (*(end - 2) << 16) | (*(end - 1) << 24) | 0x202020;
                    if (code == ('.' | ('d' << 8) | ('l' << 16) | ('l' << 24))
                        || code == ('.' | ('p' << 8) | ('d' << 16) | ('b' << 24))
                        || code == ('.' | ('e' << 8) | ('x' << 16) | ('e' << 24)))
                    {
                        return true;
                    }
                    if ((code | 0x20000000) == ('n' | ('f' << 8) | ('i' << 16) | ('g' << 24)) && name.Length > 7)
                    {
                        end -= 7;
                        if ((*end | (*(end + 1) << 8) | (*(end + 2) << 16) | 0x2020) == ('.' | ('c' << 8) | ('o' << 16))) return true;
                    }
                }
            }
            return false;
        }
    }
}
