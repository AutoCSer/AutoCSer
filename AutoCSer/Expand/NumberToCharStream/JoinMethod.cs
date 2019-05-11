using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoCSer.NumberToCharStream
{
    /// <summary>
    /// 数字转换成字符串
    /// </summary>
    internal static class JoinMethod
    {
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="array">字符串集合</param>
        /// <param name="join">字符连接</param>
        /// <param name="nullString"></param>
        /// <returns>连接后的字符串</returns>
        private unsafe static string joinNullString(string[] array, char join, string nullString)
        {
            int length = 0;
            foreach (string nextString in array) length += (nextString ?? nullString).Length;
            string value = AutoCSer.Extension.StringExtension.FastAllocateString(length + array.Length - 1);
            fixed (char* valueFixed = value)
            {
                char* write = valueFixed;
                foreach (string nextString in array)
                {
                    if (write != valueFixed) *write++ = join;
                    if (nextString == null)
                    {
                        AutoCSer.Extension.StringExtension.CopyNotNull(nullString, write);
                        write += nullString.Length;
                    }
                    else
                    {
                        AutoCSer.Extension.StringExtension.CopyNotNull(nextString, write);
                        write += nextString.Length;
                    }
                }
            }
            return value;
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="array"></param>
        /// <param name="join"></param>
        /// <param name="nullString"></param>
        /// <returns></returns>
        internal static string structJoinChar<valueType>(valueType[] array, char join, string nullString) where valueType : struct
        {
            if (array.Length == 1)
            {
                foreach (valueType value in array) return value.ToString();
            }
            string[] stringArray = new string[array.Length];
            int index = 0;
            foreach (valueType value in array) stringArray[index++] = value.ToString();
            return joinNullString(stringArray, join, nullString);
        }
        ///// <summary>
        ///// 连接字符串集合函数信息
        ///// </summary>
        //public static readonly MethodInfo StructJoinCharMethod = typeof(JoinMethod).GetMethod("structJoinChar", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="subArray"></param>
        /// <param name="join"></param>
        /// <param name="nullString"></param>
        /// <returns></returns>
        internal static string structSubArrayJoinChar<valueType>(SubArray<valueType> subArray, char join, string nullString) where valueType : struct
        {
            valueType[] array = subArray.Array;
            if (subArray.Length == 1) return array[subArray.Start].ToString();
            string[] stringArray = new string[subArray.Length];
            int index = 0, startIndex = subArray.Start, endIndex = startIndex + subArray.Length;
            do
            {
                stringArray[index++] = array[startIndex++].ToString();
            }
            while (startIndex != endIndex);
            return joinNullString(stringArray, join, nullString);
        }
        ///// <summary>
        ///// 连接字符串集合函数信息
        ///// </summary>
        //public static readonly MethodInfo StructSubArrayJoinCharMethod = typeof(JoinMethod).GetMethod("structSubArrayJoinChar", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="array"></param>
        /// <param name="join"></param>
        /// <param name="nullString"></param>
        /// <returns></returns>
        internal static string nullableJoinChar<valueType>(Nullable<valueType>[] array, char join, string nullString) where valueType : struct
        {
            if (array.Length == 1)
            {
                foreach (Nullable<valueType> value in array) return value.HasValue ? value.Value.ToString() : nullString;
            }
            string[] stringArray = new string[array.Length];
            int index = 0;
            foreach (Nullable<valueType> value in array) stringArray[index++] = value.HasValue ? value.Value.ToString() : nullString;
            return joinNullString(stringArray, join, nullString);
        }
        ///// <summary>
        ///// 连接字符串集合函数信息
        ///// </summary>
        //public static readonly MethodInfo NullableJoinCharMethod = typeof(JoinMethod).GetMethod("nullableJoinChar", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="subArray"></param>
        /// <param name="join"></param>
        /// <param name="nullString"></param>
        /// <returns></returns>
        internal static string nullableSubArrayJoinChar<valueType>(SubArray<Nullable<valueType>> subArray, char join, string nullString) where valueType : struct
        {
            Nullable<valueType>[] array = subArray.Array;
            if (subArray.Length == 1)
            {
                Nullable<valueType> value = array[subArray.Start];
                return value.HasValue ? value.Value.ToString() : nullString;
            }
            string[] stringArray = new string[subArray.Length];
            int index = 0, startIndex = subArray.Start, endIndex = startIndex + subArray.Length;
            do
            {
                Nullable<valueType> value = array[startIndex++];
                stringArray[index++] = value.HasValue ? value.Value.ToString() : nullString;
            }
            while (startIndex != endIndex);
            return joinNullString(stringArray, join, nullString);
        }
        ///// <summary>
        ///// 连接字符串集合函数信息
        ///// </summary>
        //public static readonly MethodInfo NullableSubArrayJoinCharMethod = typeof(JoinMethod).GetMethod("nullableSubArrayJoinChar", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="array"></param>
        /// <param name="join"></param>
        /// <param name="nullString"></param>
        /// <returns></returns>
        internal static string classJoinChar<valueType>(valueType[] array, char join, string nullString) where valueType : class
        {
            if (array.Length == 1)
            {
                foreach (valueType value in array) return value == null ? nullString : value.ToString();
            }
            string[] stringArray = new string[array.Length];
            int index = 0;
            foreach (valueType value in array) stringArray[index++] = value == null ? nullString : value.ToString();
            return joinNullString(stringArray, join, nullString);
        }
        ///// <summary>
        ///// 连接字符串集合函数信息
        ///// </summary>
        //public static readonly MethodInfo ClassJoinCharMethod = typeof(JoinMethod).GetMethod("classJoinChar", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="subArray"></param>
        /// <param name="join"></param>
        /// <param name="nullString"></param>
        /// <returns></returns>
        internal static string classSubArrayJoinChar<valueType>(SubArray<valueType> subArray, char join, string nullString) where valueType : class
        {
            valueType[] array = subArray.Array;
            if (subArray.Length == 1)
            {
                valueType value = array[subArray.Start];
                return value == null ? nullString : value.ToString();
            }
            string[] stringArray = new string[subArray.Length];
            int index = 0, startIndex = subArray.StartIndex, endIndex = startIndex + subArray.Length;
            do
            {
                valueType value = array[startIndex++];
                stringArray[index++] = value == null ? nullString : value.ToString();
            }
            while (startIndex != endIndex);
            return joinNullString(stringArray, join, nullString);
        }
        ///// <summary>
        ///// 连接字符串集合函数信息
        ///// </summary>
        //public static readonly MethodInfo ClassSubArrayJoinCharMethod = typeof(JoinMethod).GetMethod("classSubArrayJoinChar", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="array"></param>
        /// <param name="join"></param>
        /// <param name="nullString"></param>
        /// <returns></returns>
        internal static string stringJoinChar(string[] array, char join, string nullString)
        {
            if (array.Length == 1)
            {
                foreach (string value in array) return value == null ? nullString : value.ToString();
            }
            return joinNullString(array, join, nullString);
        }
        ///// <summary>
        ///// 连接字符串集合函数信息
        ///// </summary>
        //public static readonly MethodInfo StringJoinCharMethod = typeof(JoinMethod).GetMethod("stringJoinChar", BindingFlags.Static | BindingFlags.NonPublic);
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="subArray"></param>
        /// <param name="join"></param>
        /// <param name="nullString"></param>
        /// <returns></returns>
        internal unsafe static string stringSubArrayJoinChar(SubArray<string> subArray, char join, string nullString)
        {
            string[] array = subArray.Array;
            if (subArray.Length == 1)
            {
                string value = array[subArray.StartIndex];
                return value == null ? nullString : value.ToString();
            }
            int startIndex = subArray.Start, length = 0, endIndex = startIndex + subArray.Length;
            if (nullString.Length == 0)
            {
                do
                {
                    string nextString = array[startIndex++];
                    if (nextString != null) length += nextString.Length;
                }
                while (startIndex != endIndex);
                string value = AutoCSer.Extension.StringExtension.FastAllocateString(length + subArray.Length - 1);
                fixed (char* valueFixed = value)
                {
                    char* write = valueFixed;
                    startIndex = subArray.Start;
                    do
                    {
                        string nextString = array[startIndex++];
                        if (write != valueFixed) *write++ = join;
                        if (nextString != null)
                        {
                            AutoCSer.Extension.StringExtension.CopyNotNull(nextString, write);
                            write += nextString.Length;
                        }
                    }
                    while (startIndex != endIndex);
                }
                return value;
            }
            else
            {
                do
                {
                    length += (array[startIndex++] ?? nullString).Length;
                }
                while (startIndex != endIndex);
                string value = AutoCSer.Extension.StringExtension.FastAllocateString(length + subArray.Length - 1);
                fixed (char* valueFixed = value)
                {
                    char* write = valueFixed;
                    startIndex = subArray.Start;
                    do
                    {
                        string nextString = array[startIndex++] ?? nullString;
                        if (write != valueFixed) *write++ = join;
                        AutoCSer.Extension.StringExtension.CopyNotNull(nextString, write);
                        write += nextString.Length;
                    }
                    while (startIndex != endIndex);
                }
                return value;
            }
        }
        ///// <summary>
        ///// 连接字符串集合函数信息
        ///// </summary>
        //public static readonly MethodInfo StringSubArrayJoinCharMethod = typeof(JoinMethod).GetMethod("stringSubArrayJoinChar", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// 数值转换调用函数信息集合
        /// </summary>
        private static readonly Dictionary<Type, MethodInfo> toStringMethods;
        /// <summary>
        /// 获取数值转换委托调用函数信息
        /// </summary>
        /// <param name="type">数值类型</param>
        /// <returns>数值转换委托调用函数信息</returns>
        public static MethodInfo GetToStringMethod(Type type)
        {
            MethodInfo method;
            return toStringMethods.TryGetValue(type, out method) ? method : null;
        }
        static JoinMethod()
        {
            toStringMethods = DictionaryCreator.CreateOnly<Type, MethodInfo>();
            foreach (MethodInfo method in typeof(AutoCSer.Extension.Number).GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (method.Name == "ToString")
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length == 2 && parameters[1].ParameterType == typeof(CharStream))
                    {
                        toStringMethods.Add(parameters[0].ParameterType, method);
                    }
                }
            }
        }
    }
}
