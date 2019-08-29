using System;
using AutoCSer.Threading;

namespace AutoCSer.Web.SearchServer.Queue
{
    /// <summary>
    /// 更新数据
    /// </summary>
    internal sealed class Update : TaskQueueNode
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
        /// 旧文本
        /// </summary>
        private readonly string oldText;
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="text"></param>
        /// <param name="oldText"></param>
        internal Update(DataKey key, string text, string oldText)
        {
            this.key = key;
            this.text = text;
            this.oldText = oldText;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        public override void RunTask()
        {
            Searcher.DefaultThreadParameter.Update(ref key, text, oldText);

        }
    }

}
