using System;
using System.IO;
using AutoCSer.Extension;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 文件备份
    /// </summary>
    public sealed class FileBackup : IDisposable
    {
        /// <summary>
        /// 默认缓冲区大小
        /// </summary>
        private const int bufferSize = 4 << 10;
        /// <summary>
        /// 文件名称
        /// </summary>
        private readonly string fileName;
        /// <summary>
        /// 缓存主服务客户端
        /// </summary>
        private readonly MasterServer.TcpInternalClient masterClient;
        /// <summary>
        /// 日志处理
        /// </summary>
        private readonly AutoCSer.Log.ILog log;
        /// <summary>
        /// 读取数据回调委托
        /// </summary>
        private readonly Action<AutoCSer.Net.TcpServer.ReturnValue<ReadFileParameter>> onReadHandle;
        /// <summary>
        /// 失败重试间隔时钟周期
        /// </summary>
        private readonly long tryTicks;
        /// <summary>
        /// 文件版本号
        /// </summary>
        private ulong version;
        /// <summary>
        /// 当前写入位置
        /// </summary>
        private long index;
        /// <summary>
        /// 文件流
        /// </summary>
        private FileStream fileStream;
        /// <summary>
        /// 读取数据保持回调
        /// </summary>
        private AutoCSer.Net.TcpServer.KeepCallback readKeepCallback;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private bool isDisposed;
        /// <summary>
        /// 文件备份
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="trySeconds">失败重试间隔秒数</param>
        /// <param name="masterClient">缓存主服务客户端</param>
        /// <param name="log">日志处理</param>
        public FileBackup(string fileName, int trySeconds = 4, MasterServer.TcpInternalClient masterClient = null, AutoCSer.Log.ILog log = null)
        {
            this.fileName = fileName;
            tryTicks = Math.Max(trySeconds, 1) * TimeSpan.TicksPerSecond;
            this.masterClient = masterClient ?? new MasterServer.TcpInternalClient();
            this.log = log ?? AutoCSer.Log.Pub.Log;
            onReadHandle = onRead;
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(getVersion);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            if (fileStream != null)
            {
                fileStream.Dispose();
                fileStream = null;
            }
        }
        /// <summary>
        /// 获取版本号
        /// </summary>
        private void getVersion()
        {
            bool isTry = true;
            if (!isDisposed)
            {
                try
                {
                    if ((version = masterClient.GetFileVersion().Value) != 0)
                    {
                        FileInfo file = new FileInfo(fileName + "." + version.toString());
                        if (file.Exists)
                        {
                            fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Write, FileShare.Read, bufferSize, FileOptions.None);
                            index = fileStream.Length;
                        }
                        else
                        {
                            fileStream = new FileStream(file.FullName, FileMode.CreateNew, FileAccess.Write, FileShare.Read, bufferSize, FileOptions.None);
                            index = 0;
                        }
                        if ((readKeepCallback = masterClient.ReadFile(version, index, onReadHandle)) != null)
                        {
                            isTry = false;
                            return;
                        }
                    }
                }
                catch (Exception error)
                {
                    log.Add(Log.LogType.Error, error);
                }
            }
            if (isTry)
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                    fileStream = null;
                }
                if (!isDisposed) AutoCSer.Threading.TimerTask.Default.Add(getVersion, Date.NowTime.Set().AddTicks(tryTicks));
            }
        }
        /// <summary>
        /// 读取数据回调
        /// </summary>
        /// <param name="parameter"></param>
        private void onRead(AutoCSer.Net.TcpServer.ReturnValue<ReadFileParameter> parameter)
        {
            bool isTry = true, isClose = false;
            if (parameter.Type == Net.TcpServer.ReturnType.Success)
            {
                SubArray<byte> data = parameter.Value.Data;
                if (data.Length != 0)
                {
                    try
                    {
                        fileStream.Write(data.Array, data.Start, data.Length);
                        isTry = false;
                        return;
                    }
                    catch (Exception error)
                    {
                        log.Add(Log.LogType.Error, error);
                    }
                }
                isClose = true;
            }
            if (isTry)
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                    fileStream = null;
                }
                if (readKeepCallback != null)
                {
                    if (!isClose) readKeepCallback.Cancel();
                    readKeepCallback = null;
                }
                if (!isDisposed) AutoCSer.Threading.TimerTask.Default.Add(getVersion, Date.NowTime.Set().AddTicks(tryTicks));
            }
        }
    }
}
