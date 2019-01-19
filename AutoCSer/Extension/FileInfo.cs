using System;
using System.IO;
using System.Diagnostics;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 文件相关操作
    /// </summary>
    public static partial class FileInfoExtension
    {
        /// <summary>
        /// 在文件当前目录启动进程
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="arguments">执行参数</param>
        /// <returns>是否成功</returns>
        private static Process startProcessDirectory(this FileInfo file, string arguments = null)
        {
            if (file != null && file.Exists)
            {
                ProcessStartInfo info = new ProcessStartInfo(file.FullName, arguments);
                info.UseShellExecute = true;
                info.WorkingDirectory = file.DirectoryName;
                return Process.Start(info);
            }
            return null;
        }
        /// <summary>
        /// 在文件当前目录启动进程
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="arguments">执行参数</param>
        /// <returns>是否成功</returns>
        public static bool StartProcessDirectory(this FileInfo file, string arguments = null)
        {
            return startProcessDirectory(file, arguments) != null;
        }
        /// <summary>
        /// 在文件当前目录启动进程并等待结束
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="arguments">执行参数</param>
        /// <returns>是否成功</returns>
        public static bool WaitProcessDirectory(this FileInfo file, string arguments = null)
        {
            using (Process process = startProcessDirectory(file, arguments))
            {
                if (process != null)
                {
                    process.WaitForExit();
                    return true;
                }
            }
            return false;
        }
    }
}
