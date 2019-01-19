using System;

namespace AutoCSer.Deploy.ClientTask
{
    /// <summary>
    /// 自定义任务处理 任务信息
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class Custom : Task
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public override TaskType Type { get { return TaskType.Custom; } }
        /// <summary>
        /// 自定义调用名称
        /// </summary>
        public string CallName;
        /// <summary>
        /// 自定义参数数据
        /// </summary>
        public byte[] CustomData;
    }
}
