using System;

namespace AutoCSer
{
    /// <summary>
    /// 屏蔽方法实现
    /// </summary>
    public sealed class MethodImplAttribute : Attribute
    {
        /// <summary>
        /// 屏蔽方法实现
        /// </summary>
        /// <param name="methodImplOptions"></param>
        public MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions methodImplOptions) { }
	}
    /// <summary>
    /// 屏蔽方法实现
    /// </summary>
    public static class MethodImpl
    {
        /// <summary>
        /// 内联方法定义
        /// </summary>
        public const System.Runtime.CompilerServices.MethodImplOptions AggressiveInlining = (System.Runtime.CompilerServices.MethodImplOptions)0;
    }
}
