using System;
using System.IO;
using AutoCSer.Extension;

namespace AutoCSer.Deploy.AssemblyEnvironment
{
    /// <summary>
    /// 程序集环境检测
    /// </summary>
    public sealed class Checker
    {
        /// <summary>
        /// 程序集环境检测服务
        /// </summary>
        public readonly CheckServer CheckServer = new CheckServer();
        /// <summary>
        /// 程序集环境检测程序文件
        /// </summary>
        private readonly FileInfo checkFile;
        /// <summary>
        /// 程序集环境检测
        /// </summary>
        /// <param name="checkFileName"></param>
        public Checker(string checkFileName)
        {
            checkFile = new FileInfo(checkFileName);
            if (!checkFile.Exists) throw new FileNotFoundException(checkFileName);
        }
        /// <summary>
        /// 添加程序集环境检测任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public CheckResult Check(CheckTask task)
        {
            int taskId = CheckServer.Check(task);
            checkFile.StartProcessDirectory(AutoCSer.Pub.StartTime.Ticks.toString() + " " + taskId.toString());
            task.WaitHandle.Wait();
            return task.Result;
        }
    }
}
