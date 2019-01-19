using System;
using System.IO;

namespace AutoCSer.IO
{
    /// <summary>
    /// 新建文件监视计数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct CreateFlieTimeoutCounter
    {
        /// <summary>
        /// 文件监视器
        /// </summary>
        public FileSystemWatcher Watcher;
        /// <summary>
        /// 监视计数
        /// </summary>
        public int Count;
        /// <summary>
        /// 文件监视器初始化
        /// </summary>
        /// <param name="path">监视路径</param>
        /// <param name="onCreated">新建文件处理</param>
        public void Create(string path, FileSystemEventHandler onCreated)
        {
            Watcher = new FileSystemWatcher(path);
            Watcher.IncludeSubdirectories = false;
            Watcher.EnableRaisingEvents = true;
            Watcher.Created += onCreated;
            Count = 1;
        }
    }
}
