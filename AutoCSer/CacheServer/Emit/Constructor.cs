using System;
using System.Reflection;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Emit
{
    /// <summary>
    /// 构造函数
    /// </summary>
    internal static partial class Constructor
    {
#if !NOJIT
        /// <summary>
        /// 创建构造函数委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parameterTypes">参数类型</param>
        /// <returns>构造函数委托</returns>
        internal static Delegate CreateDataStructure(Type type, Type[] parameterTypes)
        {
            DynamicMethod dynamicMethod = new DynamicMethod("DataStructureConstructor", type, parameterTypes, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Newobj, type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameterTypes, null));
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(Func<,>).MakeGenericType(parameterTypes[0], type));
        }
        /// <summary>
        /// 创建构造函数委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parameterTypes">参数类型</param>
        /// <returns>构造函数委托</returns>
        internal static Delegate CreateCache(Type type, Type[] parameterTypes)
        {
            DynamicMethod dynamicMethod = new DynamicMethod("CacheConstructor", type, parameterTypes, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Newobj, type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameterTypes, null));
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(AutoCSer.CacheServer.Cache.Constructor<>).MakeGenericType(type));
        }
#endif
    }
}
