using System;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Reflection;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.WebView
{
    /// <summary>
    /// WEB视图成员清理
    /// </summary>
    internal static class ClearMember
    {
        /// <summary>
        /// WEB视图成员清理动态函数
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct ClearMemberDynamicMethod
        {
            /// <summary>
            /// 动态函数
            /// </summary>
            private DynamicMethod dynamicMethod;
            /// <summary>
            /// 
            /// </summary>
            private ILGenerator generator;
            /// <summary>
            /// 数据类型
            /// </summary>
            private Type type;
            /// <summary>
            /// 动态函数
            /// </summary>
            /// <param name="type"></param>
            public ClearMemberDynamicMethod(Type type)
            {
                dynamicMethod = new DynamicMethod("WebView.ClearMember", null, new Type[] { type }, this.type = type, true);
                generator = dynamicMethod.GetILGenerator();
            }
            /// <summary>
            /// 添加字段
            /// </summary>
            /// <param name="field">字段信息</param>
            public void Push(FieldIndex field)
            {
                generator.Emit(OpCodes.Ldarg_0);
                if (field.Member.FieldType.IsValueType)
                {
                    generator.Emit(OpCodes.Ldflda, field.Member);
                    generator.Emit(OpCodes.Initobj, field.Member.FieldType);
                }
                else
                {
                    generator.Emit(OpCodes.Ldnull);
                    generator.Emit(OpCodes.Stfld, field.Member);
                }
            }
            /// <summary>
            /// 基类调用
            /// </summary>
            /// <param name="type"></param>
            public void Base(Type type)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Call, AutoCSer.WebView.Metadata.GenericType.Get(type).WebViewClearMemberMethod);
            }
            /// <summary>
            /// 创建成员转换委托
            /// </summary>
            /// <returns>成员转换委托</returns>
            public Delegate Create<delegateType>()
            {
                generator.Emit(OpCodes.Ret);
                return dynamicMethod.CreateDelegate(typeof(delegateType));
            }
        }
        ///// <summary>
        ///// WEB视图成员清理函数信息
        ///// </summary>
        //private static readonly MethodInfo clearMethod = typeof(ClearMember).GetMethod("clear", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// WEB视图成员清理
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void clear<valueType>(valueType value)
        {
            ClearMember<valueType>.Cleaner(value);
        }
    }
    /// <summary>
    /// WEB视图成员清理
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    internal static class ClearMember<valueType>
    {
        /// <summary>
        /// 成员清理
        /// </summary>
        public static readonly Action<valueType> Cleaner;
        /// <summary>
        /// 忽略成员清理
        /// </summary>
        /// <param name="value"></param>
        private static void ignore(valueType value) { }

        static ClearMember()
        {
            Type type = typeof(valueType);
            if (type.IsClass)
            {
                LeftArray<FieldIndex> fields = MemberIndexGroup<valueType>.GetFields(MemberFilters.InstanceField)
                    .getFind(value => value.Member.DeclaringType == type && value.GetSetupAttribute<ClearMemberAttribute>(true) != null);
                Type baseType = type.BaseType;
                if (baseType == typeof(object)) baseType = null;
                else
                {
                    ClearMemberAttribute attribute = baseType.customAttribute<ClearMemberAttribute>();
                    if (attribute != null && !attribute.IsSetup) baseType = null;
                }
                if (fields.Length != 0 || baseType != null)
                {
                    ClearMember.ClearMemberDynamicMethod dynamicMethod = new ClearMember.ClearMemberDynamicMethod(type);
                    foreach (FieldIndex field in fields) dynamicMethod.Push(field);
                    if (baseType != null) dynamicMethod.Base(baseType);
                    Cleaner = (Action<valueType>)dynamicMethod.Create<Action<valueType>>();
                    return;
                }
            }
            Cleaner = ignore;
        }
    }
}
