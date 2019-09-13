using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// 线程静态变量
    /// </summary>
    internal sealed class ThreadStatic
    {
        /// <summary>
        /// JSON 序列化
        /// </summary>
        [ThreadStatic]
        internal readonly Serializer Serializer = new Serializer(true);
        /// <summary>
        /// JSON 解析
        /// </summary>
        [ThreadStatic]
        internal readonly Parser Parser = new Parser();

        /// <summary>
        /// 线程静态变量
        /// </summary>
        [ThreadStatic]
        private static ThreadStatic value;
        /// <summary>
        /// 默认线程静态变量
        /// </summary>
        /// <returns></returns>
        internal static ThreadStatic Get()
        {
            return value ?? (value = new ThreadStatic());
        }
    }
}
