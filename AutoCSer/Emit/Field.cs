using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Emit
{
    /// <summary>
    /// 字段操作
    /// </summary>
    internal partial class Field
    {
#if NOJIT
        /// <summary>
        /// 字段信息
        /// </summary>
        private FieldInfo field;
        /// <summary>
        /// 获取字段
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="fieldType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private fieldType get<valueType, fieldType>(valueType value)
        {
            return (fieldType)field.GetValue(value);
        }
        /// <summary>
        /// 设置字段
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="fieldType"></typeparam>
        /// <param name="value"></param>
        /// <param name="fieldValue"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void set<valueType, fieldType>(valueType value, fieldType fieldValue)
        {
            field.SetValue(value, fieldValue);
        }
        /// <summary>
        /// 创建设置字段委托
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="fieldType"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Action<valueType, fieldType> UnsafeSetField<valueType, fieldType>(string fieldName) where valueType : class
        {
            FieldInfo field = typeof(valueType).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field == null) return null;
            return new Field { field = field }.set<valueType, fieldType>;
        }
        /// <summary>
        /// 创建设置字段委托
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="fieldType"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        internal static Action<valueType, fieldType> UnsafeSetField<valueType, fieldType>(FieldInfo field) where valueType : class
        {
            return new Field { field = field }.set<valueType, fieldType>;
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
            return new Field { field = field }.get<valueType, fieldType>;
        }
#else
        /// <summary>
        /// 创建设置字段委托
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="fieldType"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Action<valueType, fieldType> UnsafeSetField<valueType, fieldType>(string fieldName) where valueType : class
        {
            FieldInfo field = typeof(valueType).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return field == null ? null : UnsafeSetField<valueType, fieldType>(field);
        }
        /// <summary>
        /// 创建设置字段委托
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="fieldType"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        internal static Action<valueType, fieldType> UnsafeSetField<valueType, fieldType>(FieldInfo field) where valueType : class
        {
            DynamicMethod dynamicMethod = new DynamicMethod("set_" + field.Name, null, new Type[] { typeof(valueType), typeof(fieldType) }, typeof(valueType), true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Stfld, field);
            generator.Emit(OpCodes.Ret);
            return (Action<valueType, fieldType>)dynamicMethod.CreateDelegate(typeof(Action<valueType, fieldType>));
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
#endif
    }
}
