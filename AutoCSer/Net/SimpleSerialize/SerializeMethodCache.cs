using System;
using System.Reflection;

namespace AutoCSer.Net.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    internal static partial class SerializeMethodCache
    {
        /// <summary>
        /// 枚举转换函数信息
        /// </summary>
        private static readonly MethodInfo enumByteMethod = typeof(Serializer).GetMethod("enumByte", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举转换函数信息
        /// </summary>
        private static readonly MethodInfo enumSByteMethod = typeof(Serializer).GetMethod("enumSByte", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举转换函数信息
        /// </summary>
        private static readonly MethodInfo enumShortMethod = typeof(Serializer).GetMethod("enumShort", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举转换函数信息
        /// </summary>
        private static readonly MethodInfo enumUShortMethod = typeof(Serializer).GetMethod("enumUShort", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举转换函数信息
        /// </summary>
        private static readonly MethodInfo enumIntMethod = typeof(Serializer).GetMethod("enumInt", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举转换函数信息
        /// </summary>
        private static readonly MethodInfo enumUIntMethod = typeof(Serializer).GetMethod("enumUInt", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举转换函数信息
        /// </summary>
        private static readonly MethodInfo enumLongMethod = typeof(Serializer).GetMethod("enumLong", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举转换函数信息
        /// </summary>
        private static readonly MethodInfo enumULongMethod = typeof(Serializer).GetMethod("enumULong", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 未知类型序列化调用函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> methods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        /// <summary>
        /// 未知类型枚举序列化委托调用函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>未知类型序列化委托调用函数信息</returns>
        internal static MethodInfo Get(Type type)
        {
            MethodInfo method;
            if (methods.TryGetValue(type, out method)) return method;
            Type enumType = System.Enum.GetUnderlyingType(type);
            if (enumType == typeof(uint)) method = enumUIntMethod;
            else if (enumType == typeof(byte)) method = enumByteMethod;
            else if (enumType == typeof(ulong)) method = enumULongMethod;
            else if (enumType == typeof(ushort)) method = enumUShortMethod;
            else if (enumType == typeof(long)) method = enumLongMethod;
            else if (enumType == typeof(short)) method = enumShortMethod;
            else if (enumType == typeof(sbyte)) method = enumSByteMethod;
            else method = enumIntMethod;
            method = method.MakeGenericMethod(type);
            methods.Set(type, method);
            return method;
        }
    }
}
