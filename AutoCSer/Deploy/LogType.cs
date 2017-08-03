using System;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 部署任务状态更新日志类型
    /// </summary>
    public enum LogType : byte
    {
        /// <summary>
        /// 部署错误结束
        /// </summary>
        Error,
        /// <summary>
        /// 创建备份目录
        /// </summary>
        CreateBakDirectory,
        /// <summary>
        /// 创建备份目录完毕
        /// </summary>
        OnCreateBakDirectory,
        /// <summary>
        /// 启动任务
        /// </summary>
        Run,
        /// <summary>
        /// 任务执行完毕
        /// </summary>
        OnRun,
        /// <summary>
        /// 部署完毕
        /// </summary>
        Completed,
    }
}
