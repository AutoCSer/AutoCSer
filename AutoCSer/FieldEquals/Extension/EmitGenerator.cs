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
        /// 判断成员位图是否匹配成员索引
        /// </summary>
        private static readonly MethodInfo memberMapIsMemberMethod = typeof(AutoCSer.Metadata.MemberMap).GetMethod("IsMember", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(int) }, null);
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
            generator.Emit(OpCodes.Callvirt, memberMapIsMemberMethod);
        }
    }
}
