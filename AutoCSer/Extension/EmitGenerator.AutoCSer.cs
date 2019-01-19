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
    internal static partial class EmitGenerator
    {
        /// <summary>
        /// 加载参数
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="index"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void ldarg(this ILGenerator generator, int index)
        {
            switch (index)
            {
                case 0: generator.Emit(OpCodes.Ldarg_0); return;
                case 1: generator.Emit(OpCodes.Ldarg_1); return;
                case 2: generator.Emit(OpCodes.Ldarg_2); return;
                case 3: generator.Emit(OpCodes.Ldarg_3); return;
            }
            generator.Emit((uint)index <= sbyte.MaxValue ? OpCodes.Ldarg_S : OpCodes.Ldarg, index);
        }
        /// <summary>
        /// 参数保存到值类型局部变量字段
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="parameter"></param>
        /// <param name="index"></param>
        /// <param name="valueLocalBuilder"></param>
        /// <param name="field"></param>
        public static void parameterToStructField(this ILGenerator generator, ParameterInfo parameter, int index, LocalBuilder valueLocalBuilder, FieldInfo field)
        {
            generator.Emit(OpCodes.Ldloca_S, valueLocalBuilder);
            generator.ldarg(index);
            if (parameter.ParameterType.IsByRef)
            {
                Type parameterType = parameter.elementType();
                if (parameterType.IsValueType)
                {
                    if (parameterType.IsEnum) parameterType = System.Enum.GetUnderlyingType(parameterType);
                    if (parameterType == typeof(int)) generator.Emit(OpCodes.Ldind_I4);
                    else if (parameterType == typeof(uint)) generator.Emit(OpCodes.Ldind_U4);
                    else if (parameterType == typeof(long) || parameterType == typeof(ulong)) generator.Emit(OpCodes.Ldind_I8);
                    else if (parameterType == typeof(byte)) generator.Emit(OpCodes.Ldind_U1);
                    else if (parameterType == typeof(sbyte)) generator.Emit(OpCodes.Ldind_I1);
                    else if (parameterType == typeof(short)) generator.Emit(OpCodes.Ldind_I2);
                    else if (parameterType == typeof(char) || parameterType == typeof(ushort)) generator.Emit(OpCodes.Ldind_U2);
                    else if (parameterType == typeof(float)) generator.Emit(OpCodes.Ldind_R4);
                    else if (parameterType == typeof(double)) generator.Emit(OpCodes.Ldind_R8);
                    else generator.Emit(OpCodes.Ldobj, parameterType);
                }
                else generator.Emit(OpCodes.Ldind_Ref);
            }
            generator.Emit(OpCodes.Stfld, field);
        }
        /// <summary>
        /// 输出参数从返回值类型局部变量读取字段
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="parameter"></param>
        /// <param name="index"></param>
        /// <param name="returnValueBuilder"></param>
        /// <param name="field"></param>
        public static void outParameterFromValueField(this ILGenerator generator, ParameterInfo parameter, int index, LocalBuilder returnValueBuilder, FieldInfo field)
        {
            generator.ldarg(index);
            generator.Emit(OpCodes.Ldloca_S, returnValueBuilder);
            generator.Emit(OpCodes.Ldfld, field);
            Type parameterType = parameter.elementType();
            if (parameterType.IsValueType)
            {
                if (parameterType.IsEnum) parameterType = System.Enum.GetUnderlyingType(parameterType);
                if (parameterType == typeof(int) || parameterType == typeof(uint)) generator.Emit(OpCodes.Stind_I4);
                else if (parameterType == typeof(long) || parameterType == typeof(ulong)) generator.Emit(OpCodes.Stind_I8);
                else if (parameterType == typeof(byte) || parameterType == typeof(sbyte)) generator.Emit(OpCodes.Stind_I1);
                else if (parameterType == typeof(char) || parameterType == typeof(short) || parameterType == typeof(ushort)) generator.Emit(OpCodes.Stind_I2);
                else if (parameterType == typeof(float)) generator.Emit(OpCodes.Stind_R4);
                else if (parameterType == typeof(double)) generator.Emit(OpCodes.Stind_R8);
                else generator.Emit(OpCodes.Stobj, parameterType);
            }
            else generator.Emit(OpCodes.Stind_Ref);
        }
        /// <summary>
        /// 输出参数设置默认值
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="parameter"></param>
        /// <param name="index"></param>
        /// <param name="isInitobj"></param>
        public static void outParameterDefault(this ILGenerator generator, ParameterInfo parameter, int index, bool isInitobj)
        {
            Type parameterType = parameter.elementType();
            if (isInitobj || parameterType.isInitobj())
            {
                generator.ldarg(index);
                if (parameterType.IsValueType)
                {
                    if (parameterType.IsEnum) parameterType = System.Enum.GetUnderlyingType(parameterType);
                    if (parameterType == typeof(int) || parameterType == typeof(uint))
                    {
                        generator.Emit(OpCodes.Ldc_I4_0);
                        generator.Emit(OpCodes.Stind_I4);
                    }
                    else if (parameterType == typeof(long) || parameterType == typeof(ulong))
                    {
                        generator.Emit(OpCodes.Ldc_I4_0);
                        generator.Emit(OpCodes.Conv_I8);
                        generator.Emit(OpCodes.Stind_I8);
                    }
                    else if (parameterType == typeof(byte) || parameterType == typeof(sbyte))
                    {
                        generator.Emit(OpCodes.Ldc_I4_0);
                        generator.Emit(OpCodes.Stind_I1);
                    }
                    else if (parameterType == typeof(char) || parameterType == typeof(short) || parameterType == typeof(ushort))
                    {
                        generator.Emit(OpCodes.Ldc_I4_0);
                        generator.Emit(OpCodes.Stind_I2);
                    }
                    else if (parameterType == typeof(float))
                    {
                        generator.Emit(OpCodes.Ldc_R4, 0.0);
                        generator.Emit(OpCodes.Stind_R4);
                    }
                    else if (parameterType == typeof(double))
                    {
                        generator.Emit(OpCodes.Ldc_R8, 0.0);
                        generator.Emit(OpCodes.Stind_R8);
                    }
                    else generator.Emit(OpCodes.Initobj, parameterType);
                }
                else
                {
                    generator.Emit(OpCodes.Ldnull);
                    generator.Emit(OpCodes.Stind_Ref);
                }
            }
        }
        /// <summary>
        /// 字符串异常构造函数
        /// </summary>
        internal static readonly ConstructorInfo StringExceptionConstructor = typeof(Exception).GetConstructor(new Type[] { typeof(string) });
        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="message"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void throwString(this ILGenerator generator, string message)
        {
            if (message == null) generator.Emit(OpCodes.Ldnull);
            else generator.Emit(OpCodes.Ldstr, message);
            generator.Emit(OpCodes.Newobj, StringExceptionConstructor);
            generator.Emit(OpCodes.Throw);
        }
        /// <summary>
        /// 非托管内存数据流预增数据流长度函数
        /// </summary>
        internal static readonly MethodInfo UnmanagedStreamPrepLengthMethod;// = typeof(UnmanagedStream).GetMethod("PrepLength", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(int) }, null);
        /// <summary>
        /// 非托管内存数据流当前数据长度
        /// </summary>
        internal static readonly FieldInfo UnmanagedStreamBaseByteSizeField = typeof(UnmanagedStreamBase).GetField("ByteSize", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        /// <summary>
        /// 内存字符流写入字符方法信息
        /// </summary>
        internal static readonly MethodInfo CharStreamWriteCharMethod;// = typeof(CharStream).GetMethod("Write", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(char) }, null);
        unsafe static EmitGenerator()
        {
            CharStream charStream = new CharStream((char*)AutoCSer.Pub.PuzzleValue, 0);
            UnmanagedStreamPrepLengthMethod = ((Action<int>)charStream.PrepLength).Method;
            CharStreamWriteCharMethod = ((Action<char>)charStream.Write).Method;
        }
    }
}
