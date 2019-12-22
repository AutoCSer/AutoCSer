using System;

namespace AutoCSer.Deploy.ClientTask
{
    /// <summary>
    /// 发布切换更新
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    public sealed class UpdateSwitchFile : Task
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public override TaskType Type { get { return TaskType.UpdateSwitchFile; } }
        /// <summary>
        /// 服务器端目录
        /// </summary>
        public string ServerPath;
        /// <summary>
        /// 运行文件名称
        /// </summary>
        public string FileName;
        /// <summary>
        /// 切换服务相对目录名称
        /// </summary>
        public string SwitchDirectoryName;
        /// <summary>
        /// 更新服务相对目录名称
        /// </summary>
        public string UpdateDirectoryName;
    }
}
