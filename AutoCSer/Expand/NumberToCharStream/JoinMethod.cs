using AutoCSer.Memory;
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
            string value = AutoCSer.Extensions.StringExtension.FastAllocateString(length + array.Length - 1);
            fixed (char* valueFixed = value)
            {
                char* write = valueFixed;
                foreach (string nextString in array)
                {
                    if (write != valueFixed) *write++ = join;
                    if (nextString == null)
                    {
                        AutoCSer.Extensions.StringExtension.CopyNotNull(nullString, write);
                        write += nullString.Length;
                    }
                    else
                    {
                        AutoCSer.Extensions.StringExtension.CopyNotNull(nextString, write);
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
                string value = AutoCSer.Extensions.StringExtension.FastAllocateString(length + subArray.Length - 1);
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
                            AutoCSer.Extensions.StringExtension.CopyNotNull(nextString, write);
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
                string value = AutoCSer.Extensions.StringExtension.FastAllocateString(length + subArray.Length - 1);
                fixed (char* valueFixed = value)
                {
                    char* write = valueFixed;
                    startIndex = subArray.Start;
                    do
                    {
                        string nextString = array[startIndex++] ?? nullString;
                        if (write != valueFixed) *write++ = join;
                        AutoCSer.Extensions.StringExtension.CopyNotNull(nextString, write);
                        write += nextString.Length;
                    }
                    while (startIndex != endIndex);
                }
                return value;
            }
        }

        /// <summary>
        /// 数值转换调用函数信息集合
        /// </summary>
        private static readonly Dictionary<HashType, Delegate> toStringDelegates;
        /// <summary>
        /// 获取数值转换委托调用函数信息
        /// </summary>
        /// <param name="type">数值类型</param>
        /// <returns>数值转换委托调用函数信息</returns>
        public static Delegate GetToStringMethod(Type type)
        {
            Delegate method;
            return toStringDelegates.TryGetValue(type, out method) ? method : null;
        }
        static JoinMethod()
        {
            toStringDelegates = DictionaryCreator<HashType>.Create<Delegate>();
            toStringDelegates.Add(typeof(int), (Action<int, CharStream>)AutoCSer.Extensions.NumberExtension.ToString);
            toStringDelegates.Add(typeof(long), (Action<long, CharStream>)AutoCSer.Extensions.NumberExtension.ToString);
            toStringDelegates.Add(typeof(byte), (Action<byte, CharStream>)AutoCSer.Extensions.NumberExtension.ToString);
            toStringDelegates.Add(typeof(uint), (Action<uint, CharStream>)AutoCSer.Extensions.NumberExtension.ToString);
            toStringDelegates.Add(typeof(ulong), (Action<ulong, CharStream>)AutoCSer.Extensions.NumberExtension.ToString);
            toStringDelegates.Add(typeof(sbyte), (Action<sbyte, CharStream>)AutoCSer.Extensions.NumberExtension.ToString);
            toStringDelegates.Add(typeof(short), (Action<short, CharStream>)AutoCSer.Extensions.NumberExtension.ToString);
            toStringDelegates.Add(typeof(ushort), (Action<ushort, CharStream>)AutoCSer.Extensions.NumberExtension.ToString);
        }
    }
}
