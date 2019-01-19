using System;

namespace AutoCSer.Deploy.ClientTask
{
    /// <summary>
    /// 等待运行程序切换结束 任务信息
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class WaitRunSwitch : Task
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public override TaskType Type { get { return TaskType.WaitRunSwitch; } }
        /// <summary>
        /// 任务信息索引位置
        /// </summary>
        public int TaskIndex;
    }
}
