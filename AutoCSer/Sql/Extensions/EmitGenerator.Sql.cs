using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Extensions
{
    /// <summary>
    /// MSIL生成
    /// </summary>
    internal static class EmitGeneratorSql
    {
        /// <summary>
        /// 写入字符
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void charStreamWriteChar(this ILGenerator generator, OpCode target, char value)
        {
            generator.Emit(target);
            generator.int32(value);
            generator.Emit(OpCodes.Callvirt, AutoCSer.Extensions.EmitGenerator.CharStreamWriteCharMethod);
        }
        /// <summary>
        /// 内存字符流写入字符串方法信息
        /// </summary>
        private static readonly MethodInfo charStreamSimpleWriteMethod = ((Action<CharStream, string>)CharStream.SimpleWrite).Method;
        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="generator"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void charStreamSimpleWrite(this ILGenerator generator)
        {
            generator.Emit(OpCodes.Call, charStreamSimpleWriteMethod);
        }
        /// <summary>
        /// 获取 string.Empty
        /// </summary>
        /// <returns></returns>
        private static string getStringEmpty()
        {
            return string.Empty;
        }
        /// <summary>
        /// 获取 string.Empty
        /// </summary>
        private static readonly MethodInfo getEmptyStringMethod = ((Func<string>)getStringEmpty).Method;
        /// <summary>
        /// 字符串 null 转 string.Empty
        /// </summary>
        /// <param name="generator"></param>
        public static void nullStringEmpty(this ILGenerator generator)
        {
            Label notNullLabel = generator.DefineLabel();
            generator.Emit(OpCodes.Dup);
            generator.Emit(OpCodes.Brtrue_S, notNullLabel);
            generator.Emit(OpCodes.Pop);
            generator.call(getEmptyStringMethod);
            generator.MarkLabel(notNullLabel);
        }
        /// <summary>
        /// 获取当前时间函数信息
        /// </summary>
        public static readonly System.Reflection.MethodInfo TableGetNowTimeMethod = ((Func<AutoCSer.Sql.Table, DateTime, int, DateTime>)AutoCSer.Sql.Table.GetNowTime).Method;
        /// <summary>
        /// SQL名称关键字处理函数信息
        /// </summary>
        public static readonly System.Reflection.MethodInfo ConstantConverterConvertNameToSqlStreamMethod = ((Action<CharStream, string>)AutoCSer.Sql.ConstantConverter.Default.ConvertNameToSqlStream).Method;
    }
}
