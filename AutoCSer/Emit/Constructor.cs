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
    /// 默认构造函数
    /// </summary>
    public sealed class ConstructorAttribute : Attribute
    {
#if NOJIT
        /// <summary>
        /// 值类型默认构造函数
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<Type, Func<object>> defaultGetters = new AutoCSer.Threading.LockLastDictionary<Type, Func<object>>();
        /// <summary>
        /// 获取值类型默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                Func<object> value;
                if (!defaultGetters.TryGetValue(type, out value))
                {
                    defaultGetters.Set(type, value = (Func<object>)Delegate.CreateDelegate(typeof(Func<object>), typeof(Constructor<>).MakeGenericType(type).GetMethod("getDefault", BindingFlags.Static | BindingFlags.NonPublic)));
                }
                return value();
            }
            return null;
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            defaultGetters.Clear();
        }
        static ConstructorAttribute()
        {
            AutoCSer.Pub.ClearCaches += clearCache;
        }
#endif
    }
    /// <summary>
    /// 默认构造函数
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public static class Constructor<valueType>
    {
#if NOJIT
        /// <summary>
        /// 默认构造函数信息
        /// </summary>
        private static readonly ConstructorInfo constructorInfo;
        /// <summary>
        /// 构造函数调用
        /// </summary>
        /// <returns></returns>
        private static valueType invoke()
        {
            return (valueType)constructorInfo.Invoke(null);
        }
        /// <summary>
        /// 对象浅复制函数信息
        /// </summary>
        private static readonly MethodInfo cloneMethod;
        /// <summary>
        /// 对象浅复制
        /// </summary>
        /// <returns></returns>
        private static valueType clone()
        {
            return (valueType)cloneMethod.Invoke(uninitializedObject, null);
        }
        /// <summary>
        /// 默认空值
        /// </summary>
        /// <returns>默认空值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static object getDefault()
        {
            return default(valueType);
        }
#endif
        /// <summary>
        /// 未初始化对象，用于Clone
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly valueType uninitializedObject;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public static readonly Func<valueType> New;
        /// <summary>
        /// 默认空值
        /// </summary>
        /// <returns>默认空值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType Default()
        {
            return default(valueType);
        }

        static Constructor()
        {
            Type type = typeof(valueType);
            if (type.IsValueType || type.IsArray || type == typeof(string))
            {
                New = Default;
                return;
            }
            if (type.IsDefined(typeof(ConstructorAttribute), false))
            {
                foreach (AutoCSer.Metadata.AttributeMethod methodInfo in AutoCSer.Metadata.AttributeMethod.GetStatic(type))
                {
                    if (methodInfo.Method.ReflectedType == type && methodInfo.Method.GetParameters().Length == 0 && methodInfo.GetAttribute<ConstructorAttribute>() != null)
                    {
                        New = (Func<valueType>)Delegate.CreateDelegate(typeof(Func<valueType>), methodInfo.Method);
                        return;
                    }
                }
            }
            if (!type.IsInterface && !type.IsAbstract)
            {
#if NOJIT
                constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, NullValue<Type>.Array, null);
#else
                ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, NullValue<Type>.Array, null);
#endif
                if (constructorInfo == null)
                {
                    uninitializedObject = (valueType)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
                    if (uninitializedObject != null)
                    {
#if NOJIT
                        //cloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
                        cloneMethod = AutoCSer.MemberCopy.Copyer.MemberwiseCloneMethod;
                        New = clone;
#else
                        DynamicMethod dynamicMethod = new DynamicMethod("UninitializedObjectClone", type, NullValue<Type>.Array, type, true);
                        ILGenerator generator = dynamicMethod.GetILGenerator();
                        generator.Emit(OpCodes.Ldsfld, typeof(Constructor<valueType>).GetField("uninitializedObject", BindingFlags.Static | BindingFlags.NonPublic));
                        //generator.Emit(OpCodes.Callvirt, typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic));
                        generator.Emit(OpCodes.Callvirt, AutoCSer.MemberCopy.Copyer.MemberwiseCloneMethod);
                        generator.Emit(OpCodes.Ret);
                        New = (Func<valueType>)dynamicMethod.CreateDelegate(typeof(Func<valueType>));
#endif
                    }
                }
                else
                {
#if NOJIT
                    New = invoke;
#else
                    DynamicMethod dynamicMethod = new DynamicMethod("Constructor", type, NullValue<Type>.Array, type, true);
                    ILGenerator generator = dynamicMethod.GetILGenerator();
                    generator.Emit(OpCodes.Newobj, constructorInfo);
                    generator.Emit(OpCodes.Ret);
                    New = (Func<valueType>)dynamicMethod.CreateDelegate(typeof(Func<valueType>));
#endif
                }
            }
        }
    }
}
