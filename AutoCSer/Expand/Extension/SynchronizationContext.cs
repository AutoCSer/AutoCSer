using System;

namespace AutoCSer.Extension
{
    /// <summary>
    /// UI 线程上下文 POST 调用
    /// </summary>
    public static class SynchronizationContext
    {
        /// <summary>
        /// UI 线程上下文 POST 调用
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="context">UI 线程上下文</param>
        /// <param name="action">回调委托</param>
        /// <param name="value">调用参数</param>
        public static void Post<valueType>(this System.Threading.SynchronizationContext context, Action<valueType> action, valueType value)
        {
            new AutoCSer.Threading.SynchronizationContextPost<valueType>(context, action).Post(value);
        }
        /// <summary>
        /// UI 线程上下文 POST 调用
        /// </summary>
        /// <param name="context">UI 线程上下文</param>
        /// <param name="action">回调委托</param>
        public static void Post(this System.Threading.SynchronizationContext context, Action action)
        {
            new AutoCSer.Threading.SynchronizationContextPost(context, action).Post();
        }
        /// <summary>
        /// 获取 UI 线程上下文 POST 调用委托
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="context">UI 线程上下文</param>
        /// <param name="action">回调委托</param>
        /// <returns>回调委托包装</returns>
        public static Action<valueType> GetPost<valueType>(this System.Threading.SynchronizationContext context, Action<valueType> action)
        {
            return new AutoCSer.Threading.SynchronizationContextPost<valueType>(context, action).Post;
        }
        /// <summary>
        /// 获取 UI 线程上下文 POST 调用委托
        /// </summary>
        /// <param name="context">UI 线程上下文</param>
        /// <param name="action">回调委托</param>
        /// <returns>回调委托包装</returns>
        public static Action GetPost(this System.Threading.SynchronizationContext context, Action action)
        {
            return new AutoCSer.Threading.SynchronizationContextPost(context, action).Post;
        }
    }
}
