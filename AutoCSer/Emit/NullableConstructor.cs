using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Emit
{
    /// <summary>
    /// 可空类型构造函数
    /// </summary>
    internal static class NullableConstructor
    {
        /// <summary>
        /// 可空类型构造函数
        /// </summary>
        internal static readonly Dictionary<Type, ConstructorInfo> Constructors;

        static NullableConstructor()
        {
            Constructors = DictionaryCreator.CreateOnly<Type, ConstructorInfo>();
            Constructors.Add(typeof(bool), typeof(bool?).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(bool) }, null));
            Constructors.Add(typeof(byte), typeof(byte?).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(byte) }, null));
            Constructors.Add(typeof(char), typeof(char?).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(char) }, null));
            Constructors.Add(typeof(DateTime), typeof(DateTime?).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(DateTime) }, null));
            Constructors.Add(typeof(decimal), typeof(decimal?).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(decimal) }, null));
            Constructors.Add(typeof(double), typeof(double?).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(double) }, null));
            Constructors.Add(typeof(float), typeof(float?).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(float) }, null));
            Constructors.Add(typeof(Guid), typeof(Guid?).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(Guid) }, null));
            Constructors.Add(typeof(short), typeof(short?).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(short) }, null));
            Constructors.Add(typeof(int), typeof(int?).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(int) }, null));
            Constructors.Add(typeof(long), typeof(long?).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(long) }, null));
        }
    }
}
