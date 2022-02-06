using System;
using System.Reflection;
using System.Threading;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 反射公共配置
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// 创建委托数量
        /// </summary>
        private static int createDelegateCount;
        /// <summary>
        /// 创建委托数量
        /// </summary>
        public static int CreateDelegateCount { get { return createDelegateCount; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Delegate CreateDelegate(Type type, MethodInfo method)
        {
            Delegate value = Delegate.CreateDelegate(type, method);
            Interlocked.Increment(ref createDelegateCount);
            return value;
        }
    }
}
