using System;
using System.Runtime.CompilerServices;
using System.Reflection;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Extension
{
    /// <summary>
    /// MSIL生成
    /// </summary>
    internal static partial class EmitGenerator_Expand
    {
        /// <summary>
        /// 写入字符
        /// </summary>
        /// <param name="generator"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void charStreamWriteChar(this ILGenerator generator)
        {
            generator.Emit(OpCodes.Callvirt, AutoCSer.Extension.EmitGenerator.CharStreamWriteCharMethod);
        }
        /// <summary>
        /// 内存字符流写入字符串方法信息
        /// </summary>
        private static readonly MethodInfo charStreamWriteStringMethod = typeof(CharStream).GetMethod("Write", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(string) }, null);
        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="generator"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void charStreamWriteString(this ILGenerator generator)
        {
            generator.Emit(OpCodes.Callvirt, charStreamWriteStringMethod);
        }
    }
}
