using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 类型扩展
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// 获取枚举基础类型
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>枚举基础类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Type GetEnumUnderlyingType(this Type type)
        {
            return System.Enum.GetUnderlyingType(type);
        }
    }
}
