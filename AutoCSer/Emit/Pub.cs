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
        internal static readonly Type PointerSizeRefType = typeof(AutoCSer.Memory.Pointer).MakeByRefType();

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

        ///// <summary>
        ///// 名称赋值数据信息集合
        ///// </summary>
        //private static readonly AutoCSer.Threading.LockDictionary<string, Pointer> nameAssignmentPools = new AutoCSer.Threading.LockDictionary<string, Pointer>();
        ///// <summary>
        ///// 获取名称赋值数据
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //internal static unsafe char* GetNameAssignmentPool(string name)
        //{
        //    Pointer pointer;
        //    if (nameAssignmentPools.TryGetValue(name, out pointer)) return pointer.Char;
        //    char* value = NamePool.Get(name, 0, 1);
        //    *(value + name.Length) = '=';
        //    nameAssignmentPools.Set(name, new Pointer { Data = value });
        //    return value;
        //}
    }
}
