using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoCSer.Extension;
using System.Threading;

namespace AutoCSer.Net.HttpDomainServer
{
    /// <summary>
    /// 网站资源版本号文件监视
    /// </summary>
    internal sealed class VersionFileWatcher : IDisposable
    {
        /// <summary>
        /// 保存当前版本的文件名称
        /// </summary>
        private const string onCreatedVersionFileName = "%"+ AutoCSer.Net.Http.Header.VersionFileName;
        /// <summary>
        /// 文件监视器
        /// </summary>
        private FileSystemWatcher watcher;
        /// <summary>
        /// 监视文件名称
        /// </summary>
        private readonly string versionFileName;
        /// <summary>
        /// WEB 视图服务集合
        /// </summary>
        private readonly HashSet<ViewServer> servers = HashSetCreator.CreateOnly<ViewServer>();
        /// <summary>
        /// WEB 视图服务集合访问锁
        /// </summary>
        private readonly object serverLock = new object();
        /// <summary>
        /// 当前版本号
        /// </summary>
        private string version;
        /// <summary>
        /// 网站资源版本号文件监视
        /// </summary>
        /// <param name="server">WEB 视图服务</param>
        private VersionFileWatcher(ViewServer server)
        {
            FileInfo file = new FileInfo(versionFileName = server.WorkPath + AutoCSer.Net.Http.Header.VersionFileName);
            if (file.Exists) server.StaticFileVersion = version = System.IO.File.ReadAllText(file.FullName, System.Text.Encoding.ASCII);
            watcher = new FileSystemWatcher(server.WorkPath, "*" + AutoCSer.Net.Http.Header.VersionFileName);//"*.html"
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;
            watcher.Created += onCreated;
            servers.Add(server);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Created -= onCreated;
                watcher.Dispose();
                watcher = null;
            }
        }
        /// <summary>
        /// 新建文件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                AutoCSer.Threading.TimerTask.Default.Add(getVersion, Date.NowTime.Set().AddSeconds(2), Threading.TimerTaskThreadType.Queue);
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
        }
        /// <summary>
        /// 获取网站资源版本号
        /// </summary>
        private void getVersion()
        {
            FileInfo file = new FileInfo(versionFileName);
            if (file.Exists)
            {
                version = System.IO.File.ReadAllText(file.FullName, System.Text.Encoding.ASCII);
                Monitor.Enter(serverLock);
                foreach (ViewServer server in servers) server.StaticFileVersion = version;
                Monitor.Exit(serverLock);
            }
        }
        /// <summary>
        /// 添加 WEB 视图服务
        /// </summary>
        /// <param name="server"></param>
        private void add(ViewServer server)
        {
            Monitor.Enter(serverLock);
            try
            {
                servers.Add(server);
            }
            finally { Monitor.Exit(serverLock); }
            if (version != null) server.StaticFileVersion = version;
        }
        /// <summary>
        /// 删除监视 WEB 视图服务
        /// </summary>
        /// <param name="server"></param>
        /// <returns>是否需要删除监视</returns>
        private bool remove(ViewServer server)
        {
            Monitor.Enter(serverLock);
            try
            {
                return servers.Remove(server) && servers.Count == 0;
            }
            finally { Monitor.Exit(serverLock); }
        }

        /// <summary>
        /// 网站资源版本号文件监视集合
        /// </summary>
        private static readonly Dictionary<HashString, VersionFileWatcher> watchers = DictionaryCreator.CreateHashString<VersionFileWatcher>();
        /// <summary>
        /// 网站资源版本号文件监视集合访问锁
        /// </summary>
        private static readonly object watcherLock = new object();
        /// <summary>
        /// 添加监视 WEB 视图服务
        /// </summary>
        /// <param name="server"></param>
        internal static void Add(ViewServer server)
        {
            HashString path = server.WorkPath;
            VersionFileWatcher watcher;
            Monitor.Enter(watcherLock);
            try
            {
                if (watchers.TryGetValue(path, out watcher)) watcher.add(server);
                else watchers.Add(path, new VersionFileWatcher(server));
            }
            finally { Monitor.Exit(watcherLock); }
        }
        /// <summary>
        /// 删除监视 WEB 视图服务
        /// </summary>
        /// <param name="server"></param>
        internal static void Remove(ViewServer server)
        {
            HashString path = server.WorkPath;
            VersionFileWatcher watcher, removeWatcher = null;
            Monitor.Enter(watcherLock);
            try
            {
                if (watchers.TryGetValue(path, out watcher) && watcher.remove(server))
                {
                    watchers.Remove(path);
                    removeWatcher = watcher;
                }
            }
            finally { Monitor.Exit(watcherLock); }
            if (removeWatcher != null) removeWatcher.Dispose();
        }
    }
}
