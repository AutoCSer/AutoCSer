using System;
using AutoCSer.Net;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 部署任务状态更新日志
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct Log
    {
        /// <summary>
        /// 部署信息索引标识
        /// </summary>
        public IndexIdentity Identity;
        /// <summary>
        /// 当前任务标识
        /// </summary>
        public int TaskIndex;
        /// <summary>
        /// 任务类型
        /// </summary>
        public TaskType TaskType;
        /// <summary>
        /// 部署任务状态更新日志类型
        /// </summary>
        public LogType Type;
        /// <summary>
        /// 设置当前任务信息
        /// </summary>
        /// <param name="taskIndex"></param>
        /// <param name="taskType"></param>
        internal void Set(int taskIndex, TaskType taskType)
        {
            TaskIndex = taskIndex;
            TaskType = taskType;
            Type = LogType.Run;
        }
    }
}
