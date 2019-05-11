using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.NumberToCharStream
{
    /// <summary>
    /// 数字转换成字符串
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public static class Join<valueType>
    {
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        internal static readonly Action<CharStream, valueType[], int, int, char, string> NumberJoinChar;
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        private static readonly Func<valueType[], char, string, string> otherJoinChar;
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        private static readonly Func<SubArray<valueType>, char, string, string> subArrayJoinChar;
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="array"></param>
        /// <param name="join"></param>
        /// <param name="nullString">null 值替换字符串</param>
        /// <returns></returns>
        public unsafe static string JoinString(valueType[] array, char join, string nullString = null)
        {
            if (nullString == null) nullString = string.Empty;
            if (array.length() == 0) return nullString;
            if (NumberJoinChar == null) return otherJoinChar(array, join, nullString);
            byte* buffer = UnmanagedPool.Default.Get();
            try
            {
                using (CharStream stream = new CharStream((char*)buffer, UnmanagedPool.DefaultSize >> 1))
                {
                    NumberJoinChar(stream, array, 0, array.Length, join, nullString);
                    return new string(stream.Char, 0, stream.Length);
                }
            }
            finally { UnmanagedPool.Default.Push(buffer); }
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="array"></param>
        /// <param name="join"></param>
        /// <param name="nullString">null 值替换字符串</param>
        /// <returns></returns>
        public unsafe static string JoinString(ref SubArray<valueType> array, char join, string nullString = null)
        {
            if (nullString == null) nullString = string.Empty;
            if (array.Length == 0) return nullString;
            if (NumberJoinChar == null) return subArrayJoinChar(array, join, nullString);
            byte* buffer = UnmanagedPool.Default.Get();
            try
            {
                using (CharStream stream = new CharStream((char*)buffer, UnmanagedPool.DefaultSize >> 1))
                {
                    NumberJoinChar(stream, array.Array, array.Start, array.Length, join, nullString);
                    return new string(stream.Char, 0, stream.Length);
                }
            }
            finally { UnmanagedPool.Default.Push(buffer); }
        }
        /// <summary>
        /// 连接字符串集合
        /// </summary>
        /// <param name="array"></param>
        /// <param name="join"></param>
        /// <param name="nullString">null 值替换字符串</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public unsafe static string JoinString(ref LeftArray<valueType> array, char join, string nullString = null)
        {
            SubArray<valueType> subArray = new SubArray<valueType>(0, array.Length, array.Array);
            return JoinString(ref subArray, join, nullString);
        }
        static Join()
        {
            Type type = typeof(valueType);
            if (type.IsValueType)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Type[] parameterTypes = type.GetGenericArguments();
                    MethodInfo method = JoinMethod.GetToStringMethod(parameterTypes[0]);
                    if (method == null)
                    {
                        //otherJoinChar = (Func<valueType[], char, string, string>)Delegate.CreateDelegate(typeof(Func<valueType[], char, string, string>), JoinMethod.NullableJoinCharMethod.MakeGenericMethod(parameterTypes));
                        otherJoinChar = (Func<valueType[], char, string, string>)AutoCSer.Expand.Metadata.StructGenericType.Get(parameterTypes[0]).NumberToCharStreamNullableJoinCharDelegate;
                        //subArrayJoinChar = (Func<SubArray<valueType>, char, string, string>)Delegate.CreateDelegate(typeof(Func<SubArray<valueType>, char, string, string>), JoinMethod.NullableSubArrayJoinCharMethod.MakeGenericMethod(parameterTypes));
                        subArrayJoinChar = (Func<SubArray<valueType>, char, string, string>)AutoCSer.Expand.Metadata.StructGenericType.Get(parameterTypes[0]).NumberToCharStreamNullableSubArrayJoinCharDelegate; 
                    }
                    else
                    {
#if NOJIT
                        NumberJoinChar = new NullableJoiner(method).Join;
#else
                        JoinDynamicMethod dynamicMethod = new JoinDynamicMethod(type, typeof(valueType[]));
                        dynamicMethod.JoinCharNull(method, type);
                        NumberJoinChar = (Action<CharStream, valueType[], int, int, char, string>)dynamicMethod.Create<Action<CharStream, valueType[], int, int, char, string>>();
#endif
                    }
                }
                else
                {
                    MethodInfo method = JoinMethod.GetToStringMethod(type);
                    if (method == null)
                    {
                        //otherJoinChar = (Func<valueType[], char, string, string>)Delegate.CreateDelegate(typeof(Func<valueType[], char, string, string>), JoinMethod.StructJoinCharMethod.MakeGenericMethod(type));
                        otherJoinChar = (Func<valueType[], char, string, string>)AutoCSer.Expand.Metadata.StructGenericType.Get(type).NumberToCharStreamStructJoinCharDelegate;
                        //subArrayJoinChar = (Func<SubArray<valueType>, char, string, string>)Delegate.CreateDelegate(typeof(Func<SubArray<valueType>, char, string, string>), JoinMethod.StructSubArrayJoinCharMethod.MakeGenericMethod(type));
                        subArrayJoinChar = (Func<SubArray<valueType>, char, string, string>)AutoCSer.Expand.Metadata.StructGenericType.Get(type).NumberToCharStreamStructSubArrayJoinCharDelegate;
                    }
                    else
                    {
#if NOJIT
                        NumberJoinChar = new Joiner(method).Join;
#else
                        JoinDynamicMethod dynamicMethod = new JoinDynamicMethod(type, typeof(valueType[]));
                        dynamicMethod.JoinChar(method, type);
                        NumberJoinChar = (Action<CharStream, valueType[], int, int, char, string>)dynamicMethod.Create<Action<CharStream, valueType[], int, int, char, string>>();
#endif
                    }
                }
            }
            else
            {
                //MethodInfo method, subArrayMethod;
                if (type == typeof(string))
                {
                    //method = JoinMethod.StringJoinCharMethod;
                    //subArrayMethod = JoinMethod.StringSubArrayJoinCharMethod;
                    otherJoinChar = (Func<valueType[], char, string, string>)(object)(Func<string[], char, string, string>)JoinMethod.stringJoinChar;
                    subArrayJoinChar = (Func<SubArray<valueType>, char, string, string>)(object)(Func<SubArray<string>, char, string, string>)JoinMethod.stringSubArrayJoinChar;
                }
                else
                {
                    //method = JoinMethod.ClassJoinCharMethod.MakeGenericMethod(type);
                    //subArrayMethod = JoinMethod.ClassSubArrayJoinCharMethod.MakeGenericMethod(type);
                    otherJoinChar = (Func<valueType[], char, string, string>)AutoCSer.Expand.Metadata.ClassGenericType.Get(type).NumberToCharStreamClassJoinCharDelegate;
                    subArrayJoinChar = (Func<SubArray<valueType>, char, string, string>)AutoCSer.Expand.Metadata.ClassGenericType.Get(type).NumberToCharStreamClassSubArrayJoinCharDelegate;
                }
                //otherJoinChar = (Func<valueType[], char, string, string>)Delegate.CreateDelegate(typeof(Func<valueType[], char, string, string>), method);
                //subArrayJoinChar = (Func<SubArray<valueType>, char, string, string>)Delegate.CreateDelegate(typeof(Func<SubArray<valueType>, char, string, string>), subArrayMethod);
            }
        }
#if NOJIT
        /// <summary>
        /// 可空类型数字转换成字符串
        /// </summary>
        private sealed class NullableJoiner
        {
            /// <summary>
            /// 数字转换成字符串
            /// </summary>
            private MethodInfo method;
            /// <summary>
            /// 可空类型判断是否存在值
            /// </summary>
            private MethodInfo hasValueMethod;
            /// <summary>
            /// 获取可空类型数据
            /// </summary>
            private MethodInfo valueMethod;
            /// <summary>
            /// 可空类型数字转换成字符串
            /// </summary>
            /// <param name="method"></param>
            public NullableJoiner(MethodInfo method)
            {
                this.method = method;
                hasValueMethod = AutoCSer.Emit.Pub.GetNullableHasValue(typeof(valueType));
                valueMethod = AutoCSer.Emit.Pub.GetNullableValue(typeof(valueType));
            }
            /// <summary>
            /// 数字转换成字符串
            /// </summary>
            /// <param name="charStream"></param>
            /// <param name="values"></param>
            /// <param name="startIndex"></param>
            /// <param name="endIndex"></param>
            /// <param name="joinChar"></param>
            /// <param name="nullString"></param>
            public void Join(CharStream charStream, valueType[] values, int startIndex, int endIndex, char joinChar, string nullString)
            {
                if (startIndex != endIndex)
                {
                    object[] parameters = null;
                    do
                    {
                        object value = values[startIndex];
                        if ((bool)hasValueMethod.Invoke(value, null))
                        {
                            if (parameters == null) parameters = new object[] { null, charStream };
                            parameters[0] = valueMethod.Invoke(value, null);
                            method.Invoke(null, parameters);
                        }
                        else if (nullString.Length != 0) charStream.WriteNotNull(nullString);
                        if (++startIndex == endIndex) return;
                        charStream.Write(joinChar);
                    }
                    while (true);
                }
            }
        }
        /// <summary>
        /// 数字转换成字符串
        /// </summary>
        private sealed class Joiner
        {
            /// <summary>
            /// 数字转换成字符串
            /// </summary>
            private MethodInfo method;
            /// <summary>
            /// 可空类型数字转换成字符串
            /// </summary>
            /// <param name="method"></param>
            public Joiner(MethodInfo method)
            {
                this.method = method;
            }
            /// <summary>
            /// 数字转换成字符串
            /// </summary>
            /// <param name="charStream"></param>
            /// <param name="values"></param>
            /// <param name="startIndex"></param>
            /// <param name="endIndex"></param>
            /// <param name="joinChar"></param>
            /// <param name="nullString"></param>
            public void Join(CharStream charStream, valueType[] values, int startIndex, int endIndex, char joinChar, string nullString)
            {
                if (startIndex != endIndex)
                {
                    object[] parameters = new object[] { null, charStream };
                    do
                    {
                        parameters[0] = values[startIndex];
                        method.Invoke(null, parameters);
                        if (++startIndex == endIndex) return;
                        charStream.Write(joinChar);
                    }
                    while (true);
                }
            }
        }
#endif
    }
}
