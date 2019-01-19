using System;
using System.Reflection;

namespace AutoCSer.Json
{
    /// <summary>
    /// 类型序列化
    /// </summary>
    internal unsafe static partial class TypeSerializer<valueType>
    {
        /// <summary>
        /// 值类型自定义序列化
        /// </summary>
        private sealed class CustomSerializer
        {
            /// <summary>
            /// 函数委托
            /// </summary>
            private readonly AutoCSer.Reflection.ActionRef1<valueType, Serializer> method;
            /// <summary>
            /// 值类型自定义反序列化
            /// </summary>
            /// <param name="methodInfo">自定义函数</param>
            internal CustomSerializer(MethodInfo methodInfo)
            {
                method = (AutoCSer.Reflection.ActionRef1<valueType, Serializer>)typeof(AutoCSer.Reflection.InvokeMethodRef1<,>).MakeGenericType(typeof(valueType), typeof(Serializer)).GetMethod("get", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { methodInfo });
            }
            /// <summary>
            /// 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            public void Serialize(Serializer serializer, valueType value)
            {
                method(ref value, serializer);
            }
        }
    }
}
