using System;
using AutoCSer.IO;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.HttpRegister
{
    /// <summary>
    /// 域名服务信息
    /// </summary>
    internal sealed class DomainServer : IDisposable
    {
        /// <summary>
        /// 域名注册信息
        /// </summary>
        internal Cache Register;
        /// <summary>
        /// HTTP 注册管理服务
        /// </summary>
        internal Server RegisterServer;
        /// <summary>
        /// 域名服务
        /// </summary>
        internal HttpDomainServer.Server HttpDomainServer;
        /// <summary>
        /// 文件监视路径
        /// </summary>
        internal string FileWatcherPath;
        /// <summary>
        /// 域名信息集合
        /// </summary>
        internal KeyValue<Domain, int>[] Domains;
        /// <summary>
        /// 有效域名数量
        /// </summary>
        internal int DomainCount;
        /// <summary>
        /// 是否已经启动
        /// </summary>
        internal bool IsStart;
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            RemoveFileWatcher();
            if (HttpDomainServer != null)
            {
                HttpDomainServer.Dispose();
                HttpDomainServer = null;
            }
        }
        /// <summary>
        /// 删除文件监视路径
        /// </summary>
        internal void RemoveFileWatcher()
        {
            CreateFlieTimeoutWatcher fileWatcher = RegisterServer.FileWatcher;
            if (fileWatcher != null)
            {
                string path = Interlocked.Exchange(ref FileWatcherPath, null);
                if (path != null) fileWatcher.Remove(path);
            }
        }
        /// <summary>
        /// 域名状态检测
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        internal int CheckDomain(ref RegisterState state)
        {
            int index = 0;
            foreach (Domain domain in Register.Domains)
            {
                if ((state = RegisterServer.CheckDomain(ref Register.Domains[index], this)) != RegisterState.Success) return index;
                ++index;
            }
            return index;
        }
        /// <summary>
        /// 获取域名注册信息
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Cache GetSaveCache()
        {
            return Register.Get(Domains.getFindArray(value => value.Value == 0, value => value.Key));
        }
        /// <summary>
        /// 停止监听
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void StopListen()
        {
            if (HttpDomainServer != null) HttpDomainServer.StopListen();
        }
    }
}
