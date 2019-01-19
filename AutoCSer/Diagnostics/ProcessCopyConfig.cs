using System;

namespace AutoCSer.Diagnostics
{
    /// <summary>
    /// 进程复制配置
    /// </summary>
    public class ProcessCopyConfig
    {
        /// <summary>
        /// 文件更新重启检测时间(单位:秒)
        /// </summary>
        public int CheckTimeoutSeconds = 5;
        /// <summary>
        /// 文件更新重启复制超时时间(单位:分)
        /// </summary>
        public int CopyTimeoutMinutes = 10;
        /// <summary>
        /// 文件监视路径
        /// </summary>
        public string WatcherPath;
    }
}
