using System;
using AutoCSer.Threading;

namespace AutoCSer.Web.SearchServer.Queue
{
    /// <summary>
    /// 删除数据
    /// </summary>
    internal sealed class Delete : TaskQueueNode
    {
        /// <summary>
        /// 关键字
        /// </summary>
        private DataKey key;
        /// <summary>
        /// 文本
        /// </summary>
        private readonly string text;
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="text"></param>
        internal Delete(DataKey key, string text)
        {
            this.key = key;
            this.text = text;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        public override void RunTask()
        {
            Searcher.DefaultThreadParameter.Remove(ref key, text);
        }
    }
}
