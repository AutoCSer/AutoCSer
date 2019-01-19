using System;

namespace AutoCSer.Deploy.ClientTask
{
    /// <summary>
    /// 写 css/js/html 任务信息
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    public class WebFile : Task
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public override TaskType Type { get { return TaskType.WebFile; } }
        /// <summary>
        /// 服务器端目录
        /// </summary>
        public string ServerPath;
        /// <summary>
        /// 客户端目录
        /// </summary>
        [AutoCSer.BinarySerialize.IgnoreMember]
        public string ClientPath;
    }
}
