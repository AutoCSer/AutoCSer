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
        /// 成员位图函数信息
        /// </summary>
        private sealed class MemberMap : AutoCSer.Metadata.MemberMap
        {
            private MemberMap() : base(null) { }
            /// <summary>
            /// 判断成员位图是否匹配成员索引
            /// </summary>
            internal static readonly MethodInfo IsMemberMethod;
            /// <summary>
            /// 设置成员索引
            /// </summary>
            internal static readonly MethodInfo SetMemberMethod;
            static MemberMap()
            {
                AutoCSer.Metadata.MemberMap memberMap = new MemberMap();
                IsMemberMethod = ((Func<int, bool>)memberMap.IsMember).Method;
                SetMemberMethod = ((Action<int>)memberMap.SetMember).Method;
            }
        }
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
        ///// <summary>
        ///// 判断成员位图是否匹配成员索引
        ///// </summary>
        //private static readonly MethodInfo memberMapIsMemberMethod = typeof(AutoCSer.Metadata.MemberMap).GetMethod("IsMember", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(int) }, null);
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
            //generator.call(memberMapIsMemberMethod);
            generator.call(MemberMap.IsMemberMethod);
        }
        ///// <summary>
        ///// 设置成员索引
        ///// </summary>
        //private static readonly MethodInfo memberMapSetMemberMethod = typeof(AutoCSer.Metadata.MemberMap).GetMethod("SetMember", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(int) }, null);
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
            //generator.call(memberMapSetMemberMethod);
            generator.call(MemberMap.SetMemberMethod);
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
        /// 整数转换成指针
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static unsafe char* toPointer(long value)
        {
            return (char*)value;
        }
        /// <summary>
        /// 整数转换成指针
        /// </summary>
        private static readonly MethodInfo toPointerMethod = typeof(EmitGenerator).GetMethod("toPointer", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(long) }, null);
        /// <summary>
        /// 内存字符流写入字符串方法信息
        /// </summary>
        private static readonly MethodInfo charStreamSimpleWriteNotNullCharsMethod = typeof(CharStream).GetMethod("SimpleWriteNotNull", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(char*), typeof(int) }, null);
        /// <summary>
        /// 写字符串
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="value">字符串起始位置</param>
        /// <param name="count">写入字符数</param>
        public static unsafe void charStreamSimpleWriteNotNull(this ILGenerator generator, OpCode target, char* value, int count)
        {
            generator.Emit(target);
            generator.Emit(OpCodes.Ldc_I8, (long)value);
            generator.Emit(OpCodes.Call, toPointerMethod);
            //if ((ulong)value > uint.MaxValue) generator.Emit(OpCodes.Ldc_I8, (ulong)value);
            //else generator.Emit(OpCodes.Ldc_I4, (uint)value);
            //generator.Emit(OpCodes.Conv_U);
            generator.int32(count);
            generator.Emit(OpCodes.Call, charStreamSimpleWriteNotNullCharsMethod);
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
    }
}
