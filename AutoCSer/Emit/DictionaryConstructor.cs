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
    /// <typeparam name="dictionaryType">集合类型</typeparam>
    /// <typeparam name="keyType">枚举值类型</typeparam>
    /// <typeparam name="valueType">枚举值类型</typeparam>
    internal static class DictionaryConstructor<dictionaryType, keyType, valueType>
    {
#if NOJIT
        /// <summary>
        /// 构造函数
        /// </summary>
        private static readonly ConstructorInfo constructor = typeof(dictionaryType).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(IDictionary<,>).MakeGenericType(typeof(keyType), typeof(valueType)) }, null);
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static dictionaryType Constructor(IDictionary<keyType, valueType> value)
        {
            return (dictionaryType)constructor.Invoke(new object[] { value });
        }
#else
        /// <summary>
        /// 构造函数
        /// </summary>
        public static readonly Func<IDictionary<keyType, valueType>, dictionaryType> Constructor = (Func<IDictionary<keyType, valueType>, dictionaryType>)Pub.CreateConstructor(typeof(dictionaryType), typeof(IDictionary<,>).MakeGenericType(typeof(keyType), typeof(valueType)));
#endif
    }
}
