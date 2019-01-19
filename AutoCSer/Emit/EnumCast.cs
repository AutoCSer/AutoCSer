using System;
#if NOJIT
using System.Runtime.CompilerServices;
#else
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Emit
{
    /// <summary>
    /// 枚举值解析
    /// </summary>
    /// <typeparam name="valueType">枚举类型</typeparam>
    /// <typeparam name="intType">枚举值数字类型</typeparam>
    internal static class EnumCast<valueType, intType>
    {
#if NOJIT
        /// <summary>
        /// 枚举转数字委托
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static intType ToInt(valueType value)
        {
            return (intType)(object)value;
        }
        /// <summary>
        /// 枚举转数字委托
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType FromInt(intType value)
        {
            return (valueType)(object)value;
        }
#else
        /// <summary>
        /// 枚举转数字委托
        /// </summary>
        public static readonly Func<valueType, intType> ToInt;
        /// <summary>
        /// 数字转枚举委托
        /// </summary>
        public static readonly Func<intType, valueType> FromInt;

        static EnumCast()
        {
            DynamicMethod toIntDynamicMethod = new DynamicMethod("To" + typeof(intType).FullName, typeof(intType), new Type[] { typeof(valueType) }, typeof(valueType), true);
            ILGenerator generator = toIntDynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ret);
            ToInt = (Func<valueType, intType>)toIntDynamicMethod.CreateDelegate(typeof(Func<valueType, intType>));

            DynamicMethod fromIntDynamicMethod = new DynamicMethod("From" + typeof(intType).FullName, typeof(valueType), new Type[] { typeof(intType) }, typeof(valueType), true);
            generator = fromIntDynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ret);
            FromInt = (Func<intType, valueType>)fromIntDynamicMethod.CreateDelegate(typeof(Func<intType, valueType>));
        }
#endif
        }
    }
