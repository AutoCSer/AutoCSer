using System;
using System.Reflection;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Emit
{
    /// <summary>
    /// 公共类型
    /// </summary>
    internal static partial class Pub
    {
        /// <summary>
        /// 带长度的指针的引用类型
        /// </summary>
        internal static readonly Type PointerSizeRefType = typeof(Pointer.Size).MakeByRefType();

#if !NOJIT
        /// <summary>
        /// 创建构造函数委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parameterType">参数类型</param>
        /// <returns>构造函数委托</returns>
        internal static Delegate CreateConstructor(Type type, Type parameterType)
        {
            DynamicMethod dynamicMethod = new DynamicMethod("Constructor", type, new Type[] { parameterType }, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Newobj, type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { parameterType }, null));
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(Func<,>).MakeGenericType(parameterType, type));
        }
#endif
    }
}
