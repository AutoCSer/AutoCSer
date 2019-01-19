using System;

namespace AutoCSer.Deploy.ClientTask
{
    /// <summary>
    /// 写文件 任务信息
    /// </summary>
    public sealed class File : WebFile
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public override TaskType Type { get { return TaskType.File; } }
        /// <summary>
        /// 文件搜索匹配集合
        /// </summary>
        [AutoCSer.BinarySerialize.IgnoreMember]
        public string[] SearchPatterns;
    }
}
