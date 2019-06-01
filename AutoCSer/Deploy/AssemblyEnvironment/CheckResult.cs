using System;

namespace AutoCSer.Deploy.AssemblyEnvironment
{
    /// <summary>
    /// 程序集环境检测结果
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class CheckResult
    {
        /// <summary>
        /// 时钟周期标识
        /// </summary>
        internal long Tick;
        /// <summary>
        /// 任务编号
        /// </summary>
        internal int TaskId;
        /// <summary>
        /// 是否存在检测结果
        /// </summary>
        public bool IsResult;
        /// <summary>
        /// 程序集环境检测结果
        /// </summary>
        public string Result;
    }
}
