using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// 反序列化线程静态变量
    /// </summary>
    internal sealed class ThreadStaticDeSerializer
    {
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        [ThreadStatic]
        internal readonly JsonDeSerializer DeSerializer = new JsonDeSerializer();

        /// <summary>
        /// 线程静态变量
        /// </summary>
        [ThreadStatic]
        private static ThreadStaticDeSerializer value;
        /// <summary>
        /// 创建线程静态变量访问锁
        /// </summary>
        private static AutoCSer.Threading.SpinLock createLock;
        /// <summary>
        /// 默认线程静态变量
        /// </summary>
        /// <returns></returns>
        internal static ThreadStaticDeSerializer Get()
        {
            return value ?? get();
        }
        /// <summary>
        /// 默认线程静态变量
        /// </summary>
        /// <returns></returns>
        private static ThreadStaticDeSerializer get()
        {
            createLock.EnterSleep();
            try
            {
                if (value == null) value = new ThreadStaticDeSerializer();
            }
            finally { createLock.Exit(); }
            return value;
        }
    }
}
