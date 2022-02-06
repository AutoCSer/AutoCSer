using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 公共配置参数
    /// </summary>
    public class Common
    {
        /// <summary>
        /// LGD
        /// </summary>
        internal const int PuzzleValue = 0x10035113;
#if ASSERTABLE
        /// <summary>
        /// 断言是否可用
        /// </summary>
        public const bool Assertable = true;
#else
        /// <summary>
        /// 断言是否可用
        /// </summary>
        public const bool Assertable = false;
#endif
        /// <summary>
        /// 公共配置参数
        /// </summary>
        public static readonly AutoCSer.Config Config;

        /// <summary>
        /// CPU 逻辑处理器数量（线程数量）
        /// </summary>
        public static readonly int ProcessorCount = Math.Max(Environment.ProcessorCount, 1);

        /// <summary>
        /// 获取默认值，消除警告
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static T GetDefault<T>()
        {
            return default(T);
        }
        /// <summary>
        /// 空对象
        /// </summary>
        internal static readonly object EmptyObject = new object();
        /// <summary>
        /// 空委托
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void emptyAction() { }
        /// <summary>
        /// 空委托
        /// </summary>
        public static readonly Action EmptyAction = emptyAction;

        static Common()
        {
            Config = (AutoCSer.Config)AutoCSer.Configuration.Common.Get(typeof(AutoCSer.Config)) ?? new AutoCSer.Config();
            Config.Format();
        }
    }
}
