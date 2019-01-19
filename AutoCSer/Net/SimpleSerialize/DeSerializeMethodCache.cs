using System;
using System.Reflection;

namespace AutoCSer.Net.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    internal static partial class DeSerializeMethodCache
    {
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumByteMethod = typeof(DeSerializer).GetMethod("enumByte", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumSByteMethod = typeof(DeSerializer).GetMethod("enumSByte", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumShortMethod = typeof(DeSerializer).GetMethod("enumShort", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumUShortMethod = typeof(DeSerializer).GetMethod("enumUShort", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumIntMethod = typeof(DeSerializer).GetMethod("EnumInt", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumUIntMethod = typeof(DeSerializer).GetMethod("EnumUInt", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumLongMethod = typeof(DeSerializer).GetMethod("EnumLong", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 枚举反序列化函数信息
        /// </summary>
        private static readonly MethodInfo enumULongMethod = typeof(DeSerializer).GetMethod("EnumULong", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 未知类型反序列化调用函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> methods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        /// <summary>
        /// 未知类型枚举反序列化委托调用函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>未知类型反序列化委托调用函数信息</returns>
        public static MethodInfo Get(Type type)
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
