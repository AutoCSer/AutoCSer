using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 成员方法相关操作
    /// </summary>
    public static class MethodInfoExtension
    {
        /// <summary>
        /// 成员方法全名
        /// </summary>
        /// <param name="method">成员方法</param>
        /// <returns>成员方法全名</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string fullName(this MethodInfo method)
        {
            return method != null ? method.DeclaringType.fullName() + "." + method.Name : null;
        }
    }
}
