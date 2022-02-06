using AutoCSer.Extensions;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 默认构造函数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class DefaultConstructor<T>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        internal static readonly Func<T> Constructor;
        /// <summary>
        /// 未初始化对象，用于Clone
        /// </summary>
        private static readonly object uninitializedObject;
        /// <summary>
        /// 获取未初始化对象，用于Clone
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static T cloneUninitializedObject()
        {
            return (T)AutoCSer.MemberCopy.Common.CallMemberwiseClone(uninitializedObject);
        }
        /// <summary>
        /// 是否存在默认构造函数
        /// </summary>
        internal static readonly DefaultConstructorType Type;
        /// <summary>
        /// 默认空值
        /// </summary>
        /// <returns>默认空值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static T Default()
        {
            return default(T);
        }

        static DefaultConstructor()
        {
            Type type = typeof(T);
            if (type.IsValueType) Type = DefaultConstructorType.Default;
            else if (!type.IsArray && type != typeof(string) && !type.IsInterface && !type.IsAbstract)
            {
                ConstructorInfo constructor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, EmptyArray<Type>.Array, null);
                if (constructor != null)
                {
                    DynamicMethod dynamicMethod = new DynamicMethod("DefaultConstructor", type, EmptyArray<Type>.Array, type, true);
                    ILGenerator generator = dynamicMethod.GetILGenerator();
                    generator.Emit(OpCodes.Newobj, constructor);
                    generator.Emit(OpCodes.Ret);
                    Constructor = (Func<T>)dynamicMethod.CreateDelegate(typeof(Func<T>));
                    Type = DefaultConstructorType.Constructor;
                    uninitializedObject = Common.EmptyObject;
                    return;
                }
                uninitializedObject = (T)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
                if (uninitializedObject != null)
                {
                    Constructor = cloneUninitializedObject;
                    Type = DefaultConstructorType.UninitializedObjectClone;
                    return;
                }
            }
            uninitializedObject = Common.EmptyObject;
            Constructor = AutoCSer.Common.GetDefault<T>;
        }
    }
}
