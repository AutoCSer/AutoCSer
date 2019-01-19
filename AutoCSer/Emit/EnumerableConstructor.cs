using System;
using System.Collections.Generic;
#if NOJIT
using System.Reflection;
#else
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Emit
{ 
    /// <summary>
    /// 集合构造函数
    /// </summary>
    /// <typeparam name="valueType">集合类型</typeparam>
    /// <typeparam name="argumentType">枚举值类型</typeparam>
    internal static class EnumerableConstructor<valueType, argumentType>
    {
#if NOJIT
        /// <summary>
        /// 构造函数
        /// </summary>
        private static readonly ConstructorInfo constructor = typeof(valueType).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(IEnumerable<>).MakeGenericType(typeof(argumentType)) }, null);
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static valueType Constructor(IEnumerable<argumentType> value)
        {
            return (valueType)constructor.Invoke(new object[] { value });
        }
#else
        /// <summary>
        /// 构造函数
        /// </summary>
        public static readonly Func<IEnumerable<argumentType>, valueType> Constructor = (Func<IEnumerable<argumentType>, valueType>)Pub.CreateConstructor(typeof(valueType), typeof(IEnumerable<>).MakeGenericType(typeof(argumentType)));
#endif
    }

}
