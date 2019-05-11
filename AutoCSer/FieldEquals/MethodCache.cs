using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using AutoCSer.Metadata;
using AutoCSer.Extension;

namespace AutoCSer.FieldEquals
{
    /// <summary>
    /// 对象对比函数信息
    /// </summary>
    internal static partial class MethodCache
    {
        /// <summary>
        /// 获取字段成员集合
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="memberFilter"></param>
        /// <returns>字段成员集合</returns>
        internal static KeyValue<FieldInfo, int>[] GetFieldIndexs<valueType>(MemberFilters memberFilter)
        {
            return MemberIndexGroup<valueType>.GetFields(memberFilter)
                .getArray(value => new KeyValue<FieldInfo, int>(value.Member, value.MemberIndex));
        }

        /// <summary>
        /// 浮点数比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static bool floatEquals(float left, float right)
        {
            if (float.IsNaN(left)) return float.IsNaN(right);
            return left == right || (!float.IsNaN(right) && float.Parse(left.ToString()) == right);
        }
        ///// <summary>
        ///// 浮点数比较函数信息
        ///// </summary>
        //public static readonly MethodInfo FloatMethod = typeof(MethodCache).GetMethod("floatEquals", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 浮点数比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static bool doubleEquals(double left, double right)
        {
            if (double.IsNaN(left)) return double.IsNaN(right);
            return left == right || (!double.IsNaN(right) && double.Parse(left.ToString(), System.Globalization.CultureInfo.InvariantCulture) == right);
        }
        ///// <summary>
        ///// 浮点数比较函数信息
        ///// </summary>
        //public static readonly MethodInfo DoubleMethod = typeof(MethodCache).GetMethod("doubleEquals", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 结构体数据比较
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static bool structIEquatable<valueType>(valueType left, valueType right) where valueType : struct, IEquatable<valueType>
        {
            return left.Equals(right);
        }
        /// <summary>
        /// 结构体数据比较函数信息
        /// </summary>
        internal static readonly MethodInfo StructIEquatableMethod = typeof(MethodCache).GetMethod("structIEquatable", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 引用对象比较
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static bool classIEquatable<valueType>(valueType left, valueType right) where valueType : class, IEquatable<valueType>
        {
            if (Object.ReferenceEquals(left, right)) return true;
            return left != null && right != null && left.Equals(right);
        }
        /// <summary>
        /// 引用对象比较函数信息
        /// </summary>
        internal static readonly MethodInfo ClassIEquatableMethod = typeof(MethodCache).GetMethod("classIEquatable", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 数组比较
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="leftArray"></param>
        /// <param name="rightArray"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static bool array<valueType>(valueType[] leftArray, valueType[] rightArray)
        {
            if (Object.ReferenceEquals(leftArray, rightArray)) return true;
            if (leftArray != null && rightArray != null && leftArray.Length == rightArray.Length)
            {
                int index = 0;
                foreach (valueType left in leftArray)
                {
                    if (!Comparor<valueType>.Equals(left, rightArray[index++])) return false;
                }
                return true;
            }
            return false;
        }
        ///// <summary>
        ///// 数组比较函数信息
        ///// </summary>
        //public static readonly MethodInfo ArrayMethod = typeof(MethodCache).GetMethod("array", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 可空数据比较
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static bool nullable<valueType>(Nullable<valueType> left, Nullable<valueType> right) where valueType : struct
        {
            if (left.HasValue) return right.HasValue && Comparor<valueType>.Equals(left.Value, right.Value);
            return !right.HasValue;
        }
        /// <summary>
        /// 可空数据比较函数信息
        /// </summary>
        internal static readonly MethodInfo NullableMethod = typeof(MethodCache).GetMethod("nullable", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 数组比较
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="leftArray"></param>
        /// <param name="rightArray"></param>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static bool leftArray<valueType>(LeftArray<valueType> leftArray, LeftArray<valueType> rightArray)
        {
            if (leftArray.Length == rightArray.Length)
            {
                for (int index = leftArray.Length; index != 0; )
                {
                    --index;
                    if (!Comparor<valueType>.Equals(leftArray[index], rightArray[index])) return false;
                }
                return true;
            }
            return false;
        }
        ///// <summary>
        ///// 数组比较函数信息
        ///// </summary>
        //public static readonly MethodInfo LeftArrayMethod = typeof(MethodCache).GetMethod("leftArray", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 数组比较
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="leftArray"></param>
        /// <param name="rightArray"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal static bool listArray<valueType>(ListArray<valueType> leftArray, ListArray<valueType> rightArray)
        {
            if (Object.ReferenceEquals(leftArray, rightArray)) return true;
            if (leftArray != null && rightArray != null && leftArray.Length == rightArray.Length)
            {
                for (int index = leftArray.Length; index != 0; )
                {
                    --index;
                    if (!Comparor<valueType>.Equals(leftArray[index], rightArray[index])) return false;
                }
                return true;
            }
            return false;
        }
        ///// <summary>
        ///// 数组比较函数信息
        ///// </summary>
        //public static readonly MethodInfo ListArrayMethod = typeof(MethodCache).GetMethod("listArray", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 数组比较
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="argumentType"></typeparam>
        /// <param name="leftArray"></param>
        /// <param name="rightArray"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static bool collection<valueType, argumentType>(valueType leftArray, valueType rightArray) where valueType : IEnumerable<argumentType>, ICollection
        {
            if (Object.ReferenceEquals(leftArray, rightArray)) return true;
            if (leftArray != null && rightArray != null && leftArray.Count == rightArray.Count)
            {
                IEnumerator<argumentType> right = rightArray.GetEnumerator();
                foreach (argumentType left in leftArray)
                {
                    if (!right.MoveNext() || !Comparor<argumentType>.Equals(left, right.Current)) return false;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 数组比较函数信息
        /// </summary>
        internal static readonly MethodInfo CollectionMethod = typeof(MethodCache).GetMethod("collection", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 字典比较
        /// </summary>
        /// <typeparam name="dictionaryType"></typeparam>
        /// <typeparam name="keyType"></typeparam>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="leftArray"></param>
        /// <param name="rightArray"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static bool dictionary<dictionaryType, keyType, valueType>(dictionaryType leftArray, dictionaryType rightArray) where dictionaryType : IDictionary<keyType, valueType>
        {
            if (Object.ReferenceEquals(leftArray, rightArray)) return true;
            if (leftArray != null && rightArray != null && leftArray.Count == rightArray.Count)
            {
                foreach (KeyValuePair<keyType, valueType> left in leftArray)
                {
                    valueType right;
                    if (!rightArray.TryGetValue(left.Key, out right) || !Comparor<valueType>.Equals(left.Value, right))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 字典比较函数信息
        /// </summary>
        internal static readonly MethodInfo DictionaryMethod = typeof(MethodCache).GetMethod("dictionary", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// 类型比较字段与委托调用集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, KeyValue<FieldInfo, MethodInfo>> equalsFieldInvokes = new AutoCSer.Threading.LockDictionary<Type, KeyValue<FieldInfo, MethodInfo>>();
        /// <summary>
        /// 获取类型比较字段与委托调用
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static KeyValue<FieldInfo, MethodInfo> GetEqualsFieldInvoke(Type type)
        {
            KeyValue<FieldInfo, MethodInfo> fieldInvoke;
            if (equalsFieldInvokes.TryGetValue(type, out fieldInvoke)) return fieldInvoke;
            fieldInvoke.Key = typeof(Comparor<>).MakeGenericType(type).GetField("Equals", BindingFlags.Static | BindingFlags.Public);
            fieldInvoke.Value = typeof(Func<,,>).MakeGenericType(type, type, typeof(bool)).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { type, type }, null);
            equalsFieldInvokes.Set(type, fieldInvoke);
            return fieldInvoke;
        }

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            equalsFieldInvokes.Clear();
        }
        static MethodCache()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
