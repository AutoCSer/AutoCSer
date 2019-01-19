using System;

namespace AutoCSer.Deploy.ClientTask
{
    /// <summary>
    /// 写文件 exe/dll/pdb 并运行程序 任务信息
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class Run : WebFile
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public override TaskType Type { get { return TaskType.Run; } }
        /// <summary>
        /// 运行文件名称
        /// </summary>
        public string FileName;
        /// <summary>
        /// 运行前休眠
        /// </summary>
        public int Sleep;
        /// <summary>
        /// 是否等待运行程序结束
        /// </summary>
        public bool IsWait;
    }
}
