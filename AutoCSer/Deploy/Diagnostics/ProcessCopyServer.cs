using System;
using AutoCSer.Net;
using System.IO;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

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
        /// 保存文件锁
        /// </summary>
        private int saveCacheLock;
        /// <summary>
        /// 守护进程集合
        /// </summary>
        private readonly Dictionary<int, ProcessCopyer> guards = DictionaryCreator.CreateInt<ProcessCopyer>();
        /// <summary>
        /// 设置TCP服务端
        /// </summary>
        /// <param name="tcpServer">TCP服务端</param>
        public override void SetTcpServer(AutoCSer.Net.TcpInternalServer.Server tcpServer)
        {
            base.SetTcpServer(tcpServer);
            cacheFile = ServerName + (ServerName == tcpServer.ServerAttribute.ServerName ? null : ("_" + tcpServer.ServerAttribute.ServerName)) + ".cache";
            if (File.Exists(cacheFile))
            {
                try
                {
                    foreach (ProcessCopyer copyer in AutoCSer.BinarySerialize.DeSerializer.DeSerialize<ProcessCopyer[]>(File.ReadAllBytes(cacheFile)))
                    {
                        if (copyer.Guard(this)) guards.Add(copyer.ProcessId, copyer);
                        else saveCacheLock = 1;
                    }
                }
                catch (Exception error)
                {
                    saveCacheLock = 1;
                    tcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
                }
                if (saveCacheLock != 0) server.CallQueueLink.Add(new ProcessCopySaveCache(this));
            }
        }
        /// <summary>
        /// 保存进程守护信息集合到缓存文件
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void trySaveCache()
        {
            if (Interlocked.CompareExchange(ref saveCacheLock, 1, 0) == 0) server.CallQueueLink.Add(new ProcessCopySaveCache(this));
        }
        /// <summary>
        /// 保存进程守护信息集合到缓存文件
        /// </summary>
        internal void SaveCache()
        {
            try
            {
                if (guards.Count == 0) File.Delete(cacheFile);
                else File.WriteAllBytes(cacheFile, AutoCSer.BinarySerialize.Serializer.Serialize(guards.Values.getArray()));
            }
            catch (Exception error)
            {
                server.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            finally
            {
                Interlocked.Exchange(ref saveCacheLock, 0);
            }
        }
        /// <summary>
        /// 删除守护进程
        /// </summary>
        /// <param name="copyer">文件信息</param>
        internal void Remove(ProcessCopyer copyer)
        {
            ProcessCopyer cache;
            if (guards.TryGetValue(copyer.ProcessId, out cache) && cache == copyer)
            {
                guards.Remove(copyer.ProcessId);
                cache.RemoveGuard();
                if (Interlocked.CompareExchange(ref saveCacheLock, 1, 0) == 0) SaveCache();
            }
        }
        /// <summary>
        /// 守护进程
        /// </summary>
        /// <param name="copyer">文件信息</param>
        /// <returns>是否成功添加守护</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private bool guard(ProcessCopyer copyer)
        {
            if (copyer.CheckName())
            {
                ProcessCopyer cache;
                if (guards.TryGetValue(copyer.ProcessId, out cache))
                {
                    if (copyer.Guard(this))
                    {
                        guards[copyer.ProcessId] = copyer;
                        cache.RemoveGuard();
                        trySaveCache();
                        return true;
                    }
                }
                else if (copyer.Guard(this))
                {
                    guards.Add(copyer.ProcessId, copyer);
                    trySaveCache();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 删除守护进程
        /// </summary>
        /// <param name="copyer">文件信息</param>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private void remove(ProcessCopyer copyer)
        {
            if (copyer.CheckName())
            {
                ProcessCopyer cache;
                if (guards.TryGetValue(copyer.ProcessId, out cache))
                {
                    if (cache.ProcessName == copyer.ProcessName)
                    {
                        guards.Remove(copyer.ProcessId);
                        trySaveCache();
                    }
                    cache.RemoveGuard();
                }
            }
        }
        /// <summary>
        /// 复制重启进程
        /// </summary>
        /// <param name="copyer">文件复制器</param>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.QueueLink, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private void copy(ProcessCopyer copyer)
        {
            if (copyer.CheckName())
            {
                if (guards.Remove(copyer.ProcessId)) trySaveCache();
                AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(copyer.Copy);
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
