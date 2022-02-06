using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// 序列化线程静态变量
    /// </summary>
    internal sealed class ThreadStaticSerializer
    {
        /// <summary>
        /// JSON 序列化
        /// </summary>
        [ThreadStatic]
        internal readonly JsonSerializer Serializer = new JsonSerializer(true);

        /// <summary>
        /// 线程静态变量
        /// </summary>
        [ThreadStatic]
        private static ThreadStaticSerializer value;
        /// <summary>
        /// 创建线程静态变量访问锁
        /// </summary>
        private static AutoCSer.Threading.SpinLock createLock;
        /// <summary>
        /// 默认线程静态变量
        /// </summary>
        /// <returns></returns>
        internal static ThreadStaticSerializer Get()
        {
            return value ?? get();
        }
        /// <summary>
        /// 默认线程静态变量
        /// </summary>
        /// <returns></returns>
        private static ThreadStaticSerializer get()
        {
            createLock.EnterSleep();
            try
            {
                if (value == null) value = new ThreadStaticSerializer();
            }
            finally { createLock.Exit(); }
            return value;
        }
    }
}
