using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.TcpServer.FileSynchronous
{
    /// <summary>
    /// 服务端文件
    /// </summary>
    public abstract class ServerFile
    {
        /// <summary>
        /// 文件编号
        /// </summary>
        private static long identity;

        /// <summary>
        /// 文件同步服务端
        /// </summary>
        protected readonly Server server;
        /// <summary>
        /// 文件路径
        /// </summary>
        private readonly string path;
        /// <summary>
        /// 列表文件数据
        /// </summary>
        protected readonly ListFileItem listFileItem;
        /// <summary>
        /// 文件编号
        /// </summary>
        internal readonly long Identity;
        /// <summary>
        /// 文件信息
        /// </summary>
        protected readonly FileInfo fileInfo;
        /// <summary>
        /// 是否上传，否则为下载
        /// </summary>
        internal readonly bool IsUpload;
        /// <summary>
        /// 文件流
        /// </summary>
        protected FileStream fileStream;
        /// <summary>
        /// 超时检测秒数
        /// </summary>
        protected long checkTimeoutSeconds;
        /// <summary>
        /// 服务端文件
        /// </summary>
        /// <param name="server">文件同步服务端</param>
        /// <param name="path">文件路径</param>
        /// <param name="listFileItem">列表文件数据</param>
        /// <param name="fileInfo">文件信息</param>
        internal ServerFile(Server server, string path, ref ListFileItem listFileItem, FileInfo fileInfo)
        {
            this.server = server;
            this.path = path;
            this.listFileItem = listFileItem;
            this.fileInfo = fileInfo ?? new FileInfo(Path.Combine(Path.Combine(server.Path, path), listFileItem.Name));
            IsUpload = fileInfo == null;
            Identity = Interlocked.Increment(ref identity);
            checkTimeoutSeconds = AutoCSer.Date.NowTime.CurrentSeconds;
        }
        /// <summary>
        /// 检测是否超时
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CheckTimeout(int timeoutSeconds)
        {
            return checkTimeoutSeconds + timeoutSeconds < AutoCSer.Date.NowTime.CurrentSeconds;
        }
    }
}
