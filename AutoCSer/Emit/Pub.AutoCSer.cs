using System;
using System.Reflection;
using System.Runtime.CompilerServices;
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

        /// <summary>
        /// 可空类型是否为空判断函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> nullableHasValues = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        /// <summary>
        /// 获取可空类型是否为空判断函数信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns>可空类型是否为空判断函数信息</returns>
        internal static MethodInfo GetNullableHasValue(Type type)
        {
            MethodInfo method;
            if (nullableHasValues.TryGetValue(type, out method)) return method;
            method = type.GetProperty("HasValue", BindingFlags.Instance | BindingFlags.Public).GetGetMethod();
            nullableHasValues.Set(type, method);
            return method;
        }
        /// <summary>
        /// 可空类型获取数据函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> nullableValues = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        /// <summary>
        /// 获取可空类型获取数据函数信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns>可空类型获取数据函数信息</returns>
        internal static MethodInfo GetNullableValue(Type type)
        {
            MethodInfo method;
            if (nullableValues.TryGetValue(type, out method)) return method;
            method = type.GetProperty("Value", BindingFlags.Instance | BindingFlags.Public).GetGetMethod();
            nullableValues.Set(type, method);
            return method;
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void clearCache(int count)
        {
            nullableHasValues.Clear();
            nullableValues.Clear();
        }
        static Pub()
        {
            AutoCSer.Pub.ClearCaches += clearCache;
        }
    }
}
