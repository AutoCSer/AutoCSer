using System;
using System.Reflection;
using fastCSharp.Extension;
#if NOJIT
#else
using System.Reflection.Emit;
#endif

namespace fastCSharp.Emit
{
    /// <summary>
    /// 公共类型
    /// </summary>
    internal static partial class Pub
    {
        /// <summary>
        /// 创建获取字段委托
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="fieldType"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        internal static Func<valueType, fieldType> GetField<valueType, fieldType>(string fieldName)
        {
            FieldInfo field = typeof(valueType).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field == null) throw new FieldAccessException(typeof(valueType).fullName() + " 未找到字段成员 " + fieldName);
            return UnsafeGetField<valueType, fieldType>(field);
        }
        /// <summary>
        /// 创建获取字段委托
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="fieldType"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        internal static Func<valueType, fieldType> UnsafeGetField<valueType, fieldType>(FieldInfo field)
        {
            DynamicMethod dynamicMethod = new DynamicMethod("get_" + field.Name, typeof(fieldType), new Type[] { typeof(valueType) }, typeof(valueType), true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            if (typeof(valueType).IsValueType) generator.Emit(OpCodes.Ldarga_S, 0);
            else generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, field);
            generator.Emit(OpCodes.Ret);
            return (Func<valueType, fieldType>)dynamicMethod.CreateDelegate(typeof(Func<valueType, fieldType>));
        }
    }
}
