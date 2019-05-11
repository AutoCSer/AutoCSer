using System;
using AutoCSer.Metadata;
using System.Reflection;
using System.Collections.Generic;

namespace AutoCSer.FieldEquals
{
    /// <summary>
    /// 对象对比
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public static class Comparor<valueType>
    {
        /// <summary>
        /// 对象对比委托
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public static new readonly Func<valueType, valueType, bool> Equals;
        /// <summary>
        /// 对象对比委托
        /// </summary>
        public static readonly Func<valueType, valueType, MemberMap, bool> MemberMapEquals;

        /// <summary>
        /// 枚举值比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static bool enumByte(valueType left, valueType right)
        {
            return Emit.EnumCast<valueType, byte>.ToInt(left) == Emit.EnumCast<valueType, byte>.ToInt(right);
        }
        /// <summary>
        /// 枚举值比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static bool enumSByte(valueType left, valueType right)
        {
            return Emit.EnumCast<valueType, sbyte>.ToInt(left) == Emit.EnumCast<valueType, sbyte>.ToInt(right);
        }
        /// <summary>
        /// 枚举值比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static bool enumUShort(valueType left, valueType right)
        {
            return Emit.EnumCast<valueType, ushort>.ToInt(left) == Emit.EnumCast<valueType, ushort>.ToInt(right);
        }
        /// <summary>
        /// 枚举值比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static bool enumShort(valueType left, valueType right)
        {
            return Emit.EnumCast<valueType, short>.ToInt(left) == Emit.EnumCast<valueType, short>.ToInt(right);
        }
        /// <summary>
        /// 枚举值比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static bool enumUInt(valueType left, valueType right)
        {
            return Emit.EnumCast<valueType, uint>.ToInt(left) == Emit.EnumCast<valueType, uint>.ToInt(right);
        }
        /// <summary>
        /// 枚举值比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static bool enumInt(valueType left, valueType right)
        {
            return Emit.EnumCast<valueType, int>.ToInt(left) == Emit.EnumCast<valueType, int>.ToInt(right);
        }
        /// <summary>
        /// 枚举值比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static bool enumULong(valueType left, valueType right)
        {
            return Emit.EnumCast<valueType, ulong>.ToInt(left) == Emit.EnumCast<valueType, ulong>.ToInt(right);
        }
        /// <summary>
        /// 枚举值比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static bool enumLong(valueType left, valueType right)
        {
            return Emit.EnumCast<valueType, long>.ToInt(left) == Emit.EnumCast<valueType, long>.ToInt(right);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static bool unknown(valueType left, valueType right)
        {
            return false;
        }

        static Comparor()
        {
            Type type = typeof(valueType);
            if (typeof(IEquatable<valueType>).IsAssignableFrom(type))
            {
                if (type == typeof(float))
                {
                    //Equals = (Func<valueType, valueType, bool>)Delegate.CreateDelegate(typeof(Func<valueType, valueType, bool>), MethodCache.FloatMethod);
                    Equals = (Func<valueType, valueType, bool>)(object)(Func<float, float, bool>)MethodCache.floatEquals;
                    return;
                }
                if (type == typeof(double))
                {
                    //Equals = (Func<valueType, valueType, bool>)Delegate.CreateDelegate(typeof(Func<valueType, valueType, bool>), MethodCache.DoubleMethod);
                    Equals = (Func<valueType, valueType, bool>)(object)(Func<double, double, bool>)MethodCache.doubleEquals;
                    return;
                }
                Equals = (Func<valueType, valueType, bool>)Delegate.CreateDelegate(typeof(Func<valueType, valueType, bool>), (type.IsValueType ? MethodCache.StructIEquatableMethod : MethodCache.ClassIEquatableMethod).MakeGenericMethod(type));
                return;
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    //Equals = (Func<valueType, valueType, bool>)Delegate.CreateDelegate(typeof(Func<valueType, valueType, bool>), MethodCache.ArrayMethod.MakeGenericMethod(type.GetElementType()));
                    Equals = (Func<valueType, valueType, bool>)AutoCSer.FieldEquals.Metadata.GenericType.Get(type.GetElementType()).ArrayDelegate;
                }
                else Equals = unknown;
                return;
            }
            if (type.IsEnum)
            {
                Type enumType = System.Enum.GetUnderlyingType(type);
                if (enumType == typeof(uint)) Equals = enumUInt;
                else if (enumType == typeof(byte)) Equals = enumByte;
                else if (enumType == typeof(ulong)) Equals = enumULong;
                else if (enumType == typeof(ushort)) Equals = enumUShort;
                else if (enumType == typeof(long)) Equals = enumLong;
                else if (enumType == typeof(short)) Equals = enumShort;
                else if (enumType == typeof(sbyte)) Equals = enumSByte;
                else Equals = enumInt;
                return;
            }
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Nullable<>))
                {
                    Equals = (Func<valueType, valueType, bool>)Delegate.CreateDelegate(typeof(Func<valueType, valueType, bool>), MethodCache.NullableMethod.MakeGenericMethod(type.GetGenericArguments()));
                    return;
                }
                if (genericType == typeof(LeftArray<>))
                {
                    //Equals = (Func<valueType, valueType, bool>)Delegate.CreateDelegate(typeof(Func<valueType, valueType, bool>), MethodCache.LeftArrayMethod.MakeGenericMethod(type.GetGenericArguments()));
                    Equals = (Func<valueType, valueType, bool>)AutoCSer.FieldEquals.Metadata.GenericType.Get(type.GetGenericArguments()[0]).LeftArrayDelegate;
                    return;
                }
                if (genericType == typeof(ListArray<>))
                {
                    //Equals = (Func<valueType, valueType, bool>)Delegate.CreateDelegate(typeof(Func<valueType, valueType, bool>), MethodCache.ListArrayMethod.MakeGenericMethod(type.GetGenericArguments()));
                    Equals = (Func<valueType, valueType, bool>)AutoCSer.FieldEquals.Metadata.GenericType.Get(type.GetGenericArguments()[0]).ListArrayDelegate;
                    return;
                }
                if (genericType == typeof(List<>) || genericType == typeof(Queue<>) || genericType == typeof(Stack<>) || genericType == typeof(LinkedList<>)
#if !DOTNET2
                    || genericType == typeof(HashSet<>) || genericType == typeof(SortedSet<>)
#endif
                )
                {
                    Equals = (Func<valueType, valueType, bool>)Delegate.CreateDelegate(typeof(Func<valueType, valueType, bool>), MethodCache.CollectionMethod.MakeGenericMethod(type, type.GetGenericArguments()[0]));
                    return;
                }
                if (genericType == typeof(Dictionary<,>) || genericType == typeof(SortedDictionary<,>) || genericType == typeof(SortedList<,>))
                {
                    Type[] parameterTypes = type.GetGenericArguments();
                    Equals = (Func<valueType, valueType, bool>)Delegate.CreateDelegate(typeof(Func<valueType, valueType, bool>), MethodCache.DictionaryMethod.MakeGenericMethod(type, parameterTypes[0], parameterTypes[1]));
                    return;
                }
            }
            if (type.IsPointer || type.IsInterface)
            {
                Equals = unknown;
                return;
            }
#if NOJIT
            Equals = new memberEquals(type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)).GetEquals();
            MemberMapEquals = new memberMapEquals().GetEquals();
#else
            MemberDynamicMethod dynamicMethod = new MemberDynamicMethod(type, false);
            foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                dynamicMethod.Push(field);
            }
            dynamicMethod.Base();
            Equals = (Func<valueType, valueType, bool>)dynamicMethod.Create<Func<valueType, valueType, bool>>();

            dynamicMethod = new MemberDynamicMethod(type, true);
            foreach (KeyValue<FieldInfo, int> field in MethodCache.GetFieldIndexs<valueType>(MemberFilters.InstanceField))
            {
                dynamicMethod.Push(field.Key, field.Value);
            }
            MemberMapEquals = (Func<valueType, valueType, MemberMap, bool>)dynamicMethod.Create<Func<valueType, valueType, MemberMap, bool>>();
#endif
        }
    }
}
