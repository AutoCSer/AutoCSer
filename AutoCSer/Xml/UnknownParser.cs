using System;
using System.Reflection;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 类型解析器
    /// </summary>
    internal unsafe static partial class TypeParser<valueType>
    {
        /// <summary>
        /// 未知名称解析
        /// </summary>
        private sealed class UnknownParser
        {
            /// <summary>
            /// 函数委托
            /// </summary>
            private readonly AutoCSer.Reflection.ActionRef13<valueType, Parser, Pointer.Size, bool> method;
            /// <summary>
            /// 值类型自定义反序列化
            /// </summary>
            /// <param name="methodInfo">自定义函数</param>
            internal UnknownParser(MethodInfo methodInfo)
            {
                method = (AutoCSer.Reflection.ActionRef13<valueType, Parser, Pointer.Size, bool>)typeof(AutoCSer.Reflection.InvokeMethodRef13<,,,>).MakeGenericType(typeof(valueType), typeof(Parser), typeof(Pointer.Size), typeof(bool)).GetMethod("get", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { methodInfo });
            }
            /// <summary>
            /// 未知名称解析委托
            /// </summary>
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            /// <param name="name">节点名称</param>
            /// <returns></returns>
            public bool Parse(Parser parser, ref valueType value, ref Pointer.Size name)
            {
                return method(ref value, parser, ref name);
            }
        }
    }
}
