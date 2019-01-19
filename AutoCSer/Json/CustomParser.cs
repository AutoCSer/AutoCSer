using System;
using System.Reflection;

namespace AutoCSer.Json
{
    /// <summary>
    /// 类型解析器
    /// </summary>
    internal unsafe static partial class TypeParser<valueType>
    {
        /// <summary>
        /// 值类型自定义反序列化
        /// </summary>
        private sealed class CustomParser
        {
            /// <summary>
            /// 函数委托
            /// </summary>
            private readonly AutoCSer.Reflection.ActionRef1<valueType, Parser> method;
            /// <summary>
            /// 值类型自定义反序列化
            /// </summary>
            /// <param name="methodInfo">自定义函数</param>
            internal CustomParser(MethodInfo methodInfo)
            {
                method = (AutoCSer.Reflection.ActionRef1<valueType, Parser>)typeof(AutoCSer.Reflection.InvokeMethodRef1<,>).MakeGenericType(typeof(valueType), typeof(Parser)).GetMethod("get", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { methodInfo });
            }
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name="parser"></param>
            /// <param name="value"></param>
            public void Parse(Parser parser, ref valueType value)
            {
                method(ref value, parser);
            }
        }
    }
}
