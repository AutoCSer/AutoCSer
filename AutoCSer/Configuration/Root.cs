using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.Configuration
{
    /// <summary>
    /// 根配置接口
    /// </summary>
    public interface IRoot
    {
        /// <summary>
        /// 主配置类型集合
        /// </summary>
        IEnumerable<Type> MainTypes { get; }
        /// <summary>
        /// 公共配置类型集合
        /// </summary>
        IEnumerable<Type> PublicTypes { get; }
        /// <summary>
        /// 缓存类型加载异常
        /// </summary>
        /// <param name="exceptionTypes">缓存类型加载异常</param>
        void OnLoadException(ref LeftArray<KeyValue<Type, Exception>> exceptionTypes);
    }
    /// <summary>
    /// 根配置
    /// </summary>
    public class Root : IRoot
    {
        /// <summary>
        /// 主配置类型集合
        /// </summary>
        public virtual IEnumerable<Type> MainTypes { get { return EmptyArray<Type>.Array; } }
        /// <summary>
        /// 公共配置类型集合
        /// </summary>
        public virtual IEnumerable<Type> PublicTypes { get { return EmptyArray<Type>.Array; } }
        /// <summary>
        /// 缓存类型加载异常
        /// </summary>
        /// <param name="exceptionTypes">缓存类型加载异常</param>
        public virtual void OnLoadException(ref LeftArray<KeyValue<Type, Exception>> exceptionTypes)
        {
            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(new ListArray<KeyValue<Type, Exception>>(ref exceptionTypes), AutoCSer.Threading.ThreadTaskType.ConfigLoadException);
        }
        /// <summary>
        /// 配置缓存加载失败异常处理
        /// </summary>
        /// <param name="exceptionTypes"></param>
        public static void ConfigLoadException(ListArray<KeyValue<Type, Exception>> exceptionTypes)
        {
            foreach (KeyValue<Type, Exception> exception in exceptionTypes)
            {
                AutoCSer.LogHelper.Exception(exception.Value, exception.Key.fullName(), LogLevel.Exception | LogLevel.AutoCSer);
            }
        }
        /// <summary>
        /// 默认空配置
        /// </summary>
        internal static readonly Root Null = new Root();
    }
}
