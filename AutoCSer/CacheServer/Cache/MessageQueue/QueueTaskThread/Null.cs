using System;

namespace AutoCSer.CacheServer.Cache.MessageQueue.QueueTaskThread
{
    /// <summary>
    /// 空操作任务
    /// </summary>
    internal sealed class Null : Node
    {
        /// <summary>
        /// 空操作任务
        /// </summary>
        /// <param name="writer"></param>
        internal Null(FileWriter writer) : base(writer.Node) { }
        /// <summary>
        /// 空操作任务
        /// </summary>
        /// <returns></returns>
        internal override Node RunTask()
        {
            throw new NotImplementedException();
        }
    }
}
