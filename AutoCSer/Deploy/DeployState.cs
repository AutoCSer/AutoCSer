using System;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 部署状态
    /// </summary>
    public enum DeployState : byte
    {
        /// <summary>
        /// 部署客户端不可用
        /// </summary>
        NoClient,
        /// <summary>
        /// 定时部署启动成功或者同步部署执行成功
        /// </summary>
        Success,
        /// <summary>
        /// 部署启动失败
        /// </summary>
        StartError,
        /// <summary>
        /// 设置文件数据源失败
        /// </summary>
        SetFileSourceError,
        /// <summary>
        /// 添加运行 Run 任务失败
        /// </summary>
        AddRunError,
        /// <summary>
        /// 添加文件 WebFile /File 任务失败
        /// </summary>
        AddFileError,
        /// <summary>
        /// 添加程序集文件 AssemblyFile 任务失败
        /// </summary>
        AddAssemblyFileError,
        /// <summary>
        /// 添加等待运行 WaitRunSwitch 任务失败
        /// </summary>
        AddWaitRunSwitchError,

        /// <summary>
        /// 添加自定义任务失败
        /// </summary>
        AddCustomError,
        /// <summary>
        /// 部署索引标识错误
        /// </summary>
        IdentityError,
        /// <summary>
        /// 部署任务已经被取消
        /// </summary>
        Canceled,
        /// <summary>
        /// 执行异常
        /// </summary>
        Exception,
        /// <summary>
        /// 未知任务类型
        /// </summary>
        UnknownTaskType,
    }
}
