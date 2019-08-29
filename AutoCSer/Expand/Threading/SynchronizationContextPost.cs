using System;
using AutoCSer.Extension;

namespace AutoCSer.Threading
{
    /// <summary>
    /// UI 线程上下文 POST 调用
    /// </summary>
    public sealed class SynchronizationContextPost
    {
        /// <summary>
        /// UI 线程上下文
        /// </summary>
        private readonly System.Threading.SynchronizationContext context;
        /// <summary>
        /// 回调委托
        /// </summary>
        private readonly Action action;
        /// <summary>
        /// UI 线程上下文 POST 调用
        /// </summary>
        /// <param name="context">UI 线程上下文</param>
        /// <param name="action">回调委托</param>
        public SynchronizationContextPost(System.Threading.SynchronizationContext context, Action action)
        {
            this.context = context;
            this.action = action;
        }
        /// <summary>
        /// POST 调用
        /// </summary>
        public void Post()
        {
            context.Post(onPost, null);
        }
        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="Value"></param>
        private void onPost(object Value)
        {
            try
            {
                action();
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
            }
        }
    }
    /// <summary>
    /// UI 线程上下文 POST 调用
    /// </summary>
    /// <typeparam name="valueType">调用参数类型</typeparam>
    public sealed class SynchronizationContextPost<valueType>
    {
        /// <summary>
        /// UI 线程上下文
        /// </summary>
        private readonly System.Threading.SynchronizationContext context;
        /// <summary>
        /// 回调委托
        /// </summary>
        private readonly Action<valueType> action;
        /// <summary>
        /// UI 线程上下文 POST 调用
        /// </summary>
        /// <param name="context">UI 线程上下文</param>
        /// <param name="action">回调委托</param>
        public SynchronizationContextPost(System.Threading.SynchronizationContext context, Action<valueType> action)
        {
            this.context = context;
            this.action = action;
        }
        /// <summary>
        /// POST 调用
        /// </summary>
        /// <param name="Value"></param>
        public void Post(valueType Value)
        {
            context.Post(onPost, Value);
        }
        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="Value"></param>
        private void onPost(object Value)
        {
            try
            {
                action((valueType)Value);
            }
            catch (Exception error)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Error, error);
            }
        }
    }
}
