using System;
using System.Reflection;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Extension
{
    /// <summary>
    /// MSIL生成
    /// </summary>
    internal static class EmitGenerator_Sql
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
            generator.Emit(OpCodes.Callvirt, AutoCSer.Extension.EmitGenerator.CharStreamWriteCharMethod);
        }
        /// <summary>
        /// 内存字符流写入字符串方法信息
        /// </summary>
        private static readonly MethodInfo charStreamSimpleWriteNotNullMethod = typeof(CharStream).GetMethod("SimpleWriteNotNull", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(string) }, null);
        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="generator"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void charStreamSimpleWriteNotNull(this ILGenerator generator)
        {
            generator.Emit(OpCodes.Call, charStreamSimpleWriteNotNullMethod);
        }
        /// <summary>
        /// string.Empty
        /// </summary>
        private static readonly FieldInfo emptyStringField = typeof(string).GetField("Empty", BindingFlags.Static | BindingFlags.Public);
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
            generator.Emit(OpCodes.Ldsfld, emptyStringField);
            generator.MarkLabel(notNullLabel);
        }
        /// <summary>
        /// 获取当前时间函数信息
        /// </summary>
        public static readonly System.Reflection.MethodInfo TableGetNowTimeMethod = typeof(AutoCSer.Sql.Table).GetMethod("GetNowTime", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// SQL名称关键字处理函数信息
        /// </summary>
        public static readonly System.Reflection.MethodInfo ConstantConverterConvertNameToSqlStreamMethod = ((Action<CharStream, string>)AutoCSer.Sql.ConstantConverter.Default.ConvertNameToSqlStream).Method;
    }
}
