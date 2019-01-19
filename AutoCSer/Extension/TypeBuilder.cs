using System;
using System.Reflection;
using System.Runtime.CompilerServices;
#if NOJIT
#else
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Extension
{
    /// <summary>
    /// Emit 类型创建器扩展
    /// </summary>
    internal unsafe static partial class TypeBuilderExtension
    {
        /// <summary>
        /// 创建类型
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Type CreateType(this TypeBuilder typeBuilder)
        {
            return typeBuilder.CreateTypeInfo().AsType();
        }
    }
}
