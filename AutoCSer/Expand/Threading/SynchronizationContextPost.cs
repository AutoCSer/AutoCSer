using System;
using System.Threading;

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
        private readonly SynchronizationContext context;
        /// <summary>
        /// UI 线程上下文 POST 访问锁
        /// </summary>
        private readonly object postLock = new object();
        /// <summary>
        /// UI 线程上下文 POST 调用
        /// </summary>
        /// <param name="context">UI 线程上下文</param>
        public SynchronizationContextPost(SynchronizationContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">UI 线程上下文</param>
        /// <returns>UI 线程上下文 POST 调用</returns>
        public static implicit operator SynchronizationContextPost(SynchronizationContext value) { return new SynchronizationContextPost(value); }
        /// <summary>
        /// POST 调用
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="value"></param>
        internal void ContextPost(SendOrPostCallback callback, object value)
        {
            Monitor.Enter(postLock);
            try
            {
                context.Post(callback, value);
            }
            finally { Monitor.Exit(postLock); }
        }
        /// <summary>
        /// POST 调用
        /// </summary>
        /// <param name="action">回调委托</param>
        public void Post(Action action)
        {
            if (action != null) new PostAction(this, action).Post();
        }
        /// <summary>
        /// 获取 POST 调用委托
        /// </summary>
        /// <param name="action">回调委托</param>
        /// <returns>回调委托包装</returns>
        public Action GetPost(Action action)
        {
            if (action != null) return new PostAction(this, action).Post;
            return null;
        }
        /// <summary>
        /// POST 调用信息
        /// </summary>
        private sealed class PostAction
        {
            /// <summary>
            /// UI 线程上下文 POST 调用
            /// </summary>
            private readonly SynchronizationContextPost context;
            /// <summary>
            /// 回调委托
            /// </summary>
            private readonly Action action;
            /// <summary>
            /// POST 调用信息
            /// </summary>
            /// <param name="context">UI 线程上下文 POST 调用</param>
            /// <param name="action">回调委托</param>
            internal PostAction(SynchronizationContextPost context, Action action)
            {
                this.context = context;
                this.action = action;
            }
            /// <summary>
            /// POST 调用
            /// </summary>
            internal void Post()
            {
                context.ContextPost(OnPost, this);
            }
            /// <summary>
            /// 执行回调委托
            /// </summary>
            /// <param name="value"></param>
            internal void OnPost(object value)
            {
                try
                {
                    action();
                }
                catch (Exception error)
                {
                    AutoCSer.LogHelper.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                }
            }
        }
        /// <summary>
        /// POST 调用
        /// </summary>
        /// <typeparam name="valueType">调用参数类型</typeparam>
        /// <param name="action">回调委托</param>
        /// <param name="value">调用参数</param>
        public void Post<valueType>(Action<valueType> action, valueType value)
        {
            if (action != null) new PostAction<valueType>(this, action).Post(value);
        }
        /// <summary>
        /// 获取 POST 调用委托
        /// </summary>
        /// <typeparam name="valueType">调用参数类型</typeparam>
        /// <param name="action">回调委托</param>
        /// <returns>回调委托包装</returns>
        public Action<valueType> GetPost<valueType>(Action<valueType> action)
        {
            if (action != null) return new PostAction<valueType>(this, action).Post;
            return null;
        }
        /// <summary>
        /// POST 调用信息
        /// </summary>
        /// <typeparam name="valueType">调用参数类型</typeparam>
        private sealed class PostAction<valueType>
        {
            /// <summary>
            /// UI 线程上下文 POST 调用
            /// </summary>
            private readonly SynchronizationContextPost context;
            /// <summary>
            /// 回调委托
            /// </summary>
            private readonly Action<valueType> action;
            /// <summary>
            /// POST 调用信息
            /// </summary>
            /// <param name="context">UI 线程上下文 POST 调用</param>
            /// <param name="action">回调委托</param>
            internal PostAction(SynchronizationContextPost context, Action<valueType> action)
            {
                this.context = context;
                this.action = action;
            }
            /// <summary>
            /// POST 调用
            /// </summary>
            /// <param name="value"></param>
            internal void Post(valueType value)
            {
                context.ContextPost(OnPost, value);
            }
            /// <summary>
            /// 执行回调委托
            /// </summary>
            /// <param name="value"></param>
            internal void OnPost(object value)
            {
                try
                {
                    action((valueType)value);
                }
                catch (Exception error)
                {
                    AutoCSer.LogHelper.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                }
            }
        }
    }
}
