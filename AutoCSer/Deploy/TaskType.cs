using System;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 任务类型
    /// </summary>
    public enum TaskType : byte
    {
        /// <summary>
        /// 写文件 exe/dll/pdb 并运行程序
        /// </summary>
        Run,
        /// <summary>
        /// css/js/html
        /// </summary>
        WebFile,
        /// <summary>
        /// 写文件 exe/dll/pdb
        /// </summary>
        AssemblyFile,
        /// <summary>
        /// 所有文件
        /// </summary>
        File,
        /// <summary>
        /// 等待运行程序切换结束
        /// </summary>
        WaitRunSwitch,
        
        /// <summary>
        /// 自定义任务处理
        /// </summary>
        Custom = 0xff,
    }
}
