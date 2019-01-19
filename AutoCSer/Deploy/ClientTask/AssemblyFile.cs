using System;

namespace AutoCSer.Deploy.ClientTask
{
    /// <summary>
    /// 写文件 exe/dll/pdb 任务信息
    /// </summary>
    public sealed class AssemblyFile : WebFile
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public override TaskType Type { get { return TaskType.AssemblyFile; } }
    }
}
