using System;
using System.Runtime.CompilerServices;
using System.Reflection;
using AutoCSer.Memory;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Extensions
{
    /// <summary>
    /// MSIL生成
    /// </summary>
    internal static partial class EmitGeneratorExpand
    {
        /// <summary>
        /// 写入字符
        /// </summary>
        /// <param name="generator"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void charStreamWriteChar(this ILGenerator generator)
        {
            generator.Emit(OpCodes.Callvirt, AutoCSer.Extensions.EmitGenerator.CharStreamWriteCharMethod);
        }
        /// <summary>
        /// 内存字符流写入字符串方法信息
        /// </summary>
        private static readonly MethodInfo charStreamWriteStringMethod = ((Action<CharStream, string>)CharStream.Write).Method;
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
