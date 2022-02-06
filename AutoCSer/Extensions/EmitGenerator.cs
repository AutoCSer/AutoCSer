using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// MSIL生成
    /// </summary>
    internal static partial class EmitGenerator
    {
        /// <summary>
        /// 加载Int32数据
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="value"></param>
        public static void int32(this ILGenerator generator, int value)
        {
            switch (value)
            {
                case 0: generator.Emit(OpCodes.Ldc_I4_0); return;
                case 1: generator.Emit(OpCodes.Ldc_I4_1); return;
                case 2: generator.Emit(OpCodes.Ldc_I4_2); return;
                case 3: generator.Emit(OpCodes.Ldc_I4_3); return;
                case 4: generator.Emit(OpCodes.Ldc_I4_4); return;
                case 5: generator.Emit(OpCodes.Ldc_I4_5); return;
                case 6: generator.Emit(OpCodes.Ldc_I4_6); return;
                case 7: generator.Emit(OpCodes.Ldc_I4_7); return;
                case 8: generator.Emit(OpCodes.Ldc_I4_8); return;
            }
            if (value == -1) generator.Emit(OpCodes.Ldc_I4_M1);
            else generator.Emit((uint)value <= sbyte.MaxValue ? OpCodes.Ldc_I4_S : OpCodes.Ldc_I4, value);
        }

        /// <summary>
        /// 加载参数
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="index"></param>
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
        /// 函数调用
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="method"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void call(this ILGenerator generator, MethodInfo method)
        {
            generator.Emit(method.IsFinal || !method.IsVirtual ? OpCodes.Call : OpCodes.Callvirt, method);
        }
        /// <summary>
        /// 对象初始化
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type"></param>
        /// <param name="local"></param>
        public static void initobjShort(this ILGenerator generator, Type type, LocalBuilder local)
        {
            if (type.isInitobj())
            {
                if (type.IsValueType)
                {
                    generator.Emit(OpCodes.Ldloca_S, local);
                    generator.Emit(OpCodes.Initobj, type);
                }
                else
                {
                    generator.Emit(OpCodes.Ldnull);
                    generator.Emit(OpCodes.Stloc_S, local);
                }
            }
        }
        /// <summary>
        /// 对象初始化
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type"></param>
        /// <param name="local"></param>
        public static void initobj(this ILGenerator generator, Type type, LocalBuilder local)
        {
            if (type.isInitobj())
            {
                if (type.IsValueType)
                {
                    generator.Emit(OpCodes.Ldloca, local);
                    generator.Emit(OpCodes.Initobj, type);
                }
                else
                {
                    generator.Emit(OpCodes.Ldnull);
                    generator.Emit(OpCodes.Stloc, local);
                }
            }
        }

        /// <summary>
        /// 判断成员位图是否匹配成员索引
        /// </summary>
        private static readonly MethodInfo memberMapIsMemberMethod = ((Func<MemberMap, int, bool>)MemberMap.IsMember).Method;
        /// <summary>
        /// 判断成员位图是否匹配成员索引
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void memberMapIsMember(this ILGenerator generator, OpCode target, int value)
        {
            generator.Emit(target);
            generator.int32(value);
            generator.call(memberMapIsMemberMethod);
        }
        /// <summary>
        /// 设置成员索引
        /// </summary>
        internal static readonly MethodInfo memberMapSetMemberMethod = ((Action<MemberMap, int>)MemberMap.SetMember).Method;
        /// <summary>
        /// 设置成员索引
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void memberMapSetMember(this ILGenerator generator, OpCode target, int value)
        {
            generator.Emit(target);
            generator.int32(value);
            generator.call(memberMapSetMemberMethod);
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
        /// 创建异常
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Exception newException(string message)
        {
            return new Exception(message);
        }
        /// <summary>
        /// 字符串异常构造函数
        /// </summary>
        internal static readonly MethodInfo NewExceptionStringMethodInfo = ((Func<string, Exception>)newException).Method;
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
            generator.call(NewExceptionStringMethodInfo);
            generator.Emit(OpCodes.Throw);
        }

        /// <summary>
        /// 非托管内存数据流预增数据流长度方法信息
        /// </summary>
        internal static readonly MethodInfo UnmanagedStreamBasePrepSizeMethod = ((Action<UnmanagedStreamBase, int>)UnmanagedStreamBase.PrepSize).Method;
        /// <summary>
        /// 预增数据流长度
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="size"></param>
        public static void unmanagedStreamBasePrepSize(this ILGenerator generator, OpCode target, int size)
        {
            generator.Emit(target);
            generator.int32(size);
            generator.Emit(OpCodes.Call, UnmanagedStreamBasePrepSizeMethod);
        }
        /// <summary>
        /// 非托管内存数据流写入 64 字节数据方法信息
        /// </summary>
        private static readonly MethodInfo unmanagedStreamBaseUnsafeWriteULong4Method = ((Action<UnmanagedStreamBase, ulong, ulong, ulong, ulong>)UnmanagedStreamBase.UnsafeWrite).Method;
        /// <summary>
        /// 写入 64 字节数据
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        public static void unmanagedStreamBaseUnsafeWrite(this ILGenerator generator, OpCode target, ulong value0, ulong value1, ulong value2, ulong value3)
        {
            generator.Emit(target);
            generator.Emit(OpCodes.Ldc_I8, (long)value0);
            generator.Emit(OpCodes.Ldc_I8, (long)value1);
            generator.Emit(OpCodes.Ldc_I8, (long)value2);
            generator.Emit(OpCodes.Ldc_I8, (long)value3);
            generator.Emit(OpCodes.Call, unmanagedStreamBaseUnsafeWriteULong4Method);
        }
        /// <summary>
        /// 非托管内存数据流写入数据方法信息
        /// </summary>
        private static readonly MethodInfo unmanagedStreamBaseUnsafeWriteULong4SizeMethod = ((Action<UnmanagedStreamBase, ulong, ulong, ulong, ulong, int>)UnmanagedStreamBase.UnsafeWrite).Method;
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="size"></param>
        public static void unmanagedStreamBaseUnsafeWrite(this ILGenerator generator, OpCode target, ulong value0, ulong value1, ulong value2, ulong value3, int size)
        {
            generator.Emit(target);
            generator.Emit(OpCodes.Ldc_I8, (long)value0);
            generator.Emit(OpCodes.Ldc_I8, (long)value1);
            generator.Emit(OpCodes.Ldc_I8, (long)value2);
            generator.Emit(OpCodes.Ldc_I8, (long)value3);
            generator.int32(size);
            generator.Emit(OpCodes.Call, unmanagedStreamBaseUnsafeWriteULong4SizeMethod);
        }
        /// <summary>
        /// 非托管内存数据流写入数据方法信息
        /// </summary>
        private static readonly MethodInfo unmanagedStreamBaseUnsafeWriteULong3SizeMethod = ((Action<UnmanagedStreamBase, ulong, ulong, ulong, int>)UnmanagedStreamBase.UnsafeWrite).Method;
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="size"></param>
        public static void unmanagedStreamBaseUnsafeWrite(this ILGenerator generator, OpCode target, ulong value0, ulong value1, ulong value2, int size)
        {
            generator.Emit(target);
            generator.Emit(OpCodes.Ldc_I8, (long)value0);
            generator.Emit(OpCodes.Ldc_I8, (long)value1);
            generator.Emit(OpCodes.Ldc_I8, (long)value2);
            generator.int32(size);
            generator.Emit(OpCodes.Call, unmanagedStreamBaseUnsafeWriteULong3SizeMethod);
        }
        /// <summary>
        /// 非托管内存数据流写入数据方法信息
        /// </summary>
        private static readonly MethodInfo unmanagedStreamBaseUnsafeWriteULong2SizeMethod = ((Action<UnmanagedStreamBase, ulong, ulong, int>)UnmanagedStreamBase.UnsafeWrite).Method;
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="value0"></param>
        /// <param name="value1"></param>
        /// <param name="size"></param>
        public static void unmanagedStreamBaseUnsafeWrite(this ILGenerator generator, OpCode target, ulong value0, ulong value1, int size)
        {
            generator.Emit(target);
            generator.Emit(OpCodes.Ldc_I8, (long)value0);
            generator.Emit(OpCodes.Ldc_I8, (long)value1);
            generator.int32(size);
            generator.Emit(OpCodes.Call, unmanagedStreamBaseUnsafeWriteULong2SizeMethod);
        }
        /// <summary>
        /// 非托管内存数据流写入数据方法信息
        /// </summary>
        private static readonly MethodInfo unmanagedStreamBaseUnsafeWriteULongSizeMethod = ((Action<UnmanagedStreamBase, ulong, int>)UnmanagedStreamBase.UnsafeWrite).Method;
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        public static void unmanagedStreamBaseUnsafeWrite(this ILGenerator generator, OpCode target, ulong value, int size)
        {
            generator.Emit(target);
            generator.Emit(OpCodes.Ldc_I8, (long)value);
            generator.int32(size);
            generator.Emit(OpCodes.Call, unmanagedStreamBaseUnsafeWriteULongSizeMethod);
        }

        /// <summary>
        /// 内存字符流写入字符方法信息
        /// </summary>
        internal static readonly MethodInfo CharStreamWriteCharMethod = ((Action<CharStream, char>)CharStream.Write).Method;
        /// <summary>
        /// 内存字符流移动当前位置方法信息
        /// </summary>
        internal static readonly MethodInfo UnmanagedStreamBaseUnsafeMoveSizeMethod = ((Action<UnmanagedStreamBase, int>)UnmanagedStreamBase.UnsafeMoveSize).Method;
    }
}
