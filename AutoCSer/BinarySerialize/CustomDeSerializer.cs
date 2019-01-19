using System;
using System.Reflection;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    internal unsafe partial class TypeDeSerializer<valueType>
    {
        /// <summary>
        /// 值类型自定义反序列化
        /// </summary>
        private sealed class CustomDeSerializer
        {
            /// <summary>
            /// 函数委托
            /// </summary>
            private readonly AutoCSer.Reflection.ActionRef1<valueType, DeSerializer> method;
            /// <summary>
            /// 值类型自定义反序列化
            /// </summary>
            /// <param name="methodInfo">自定义函数</param>
            internal CustomDeSerializer(MethodInfo methodInfo)
            {
                method = (AutoCSer.Reflection.ActionRef1<valueType, DeSerializer>)typeof(AutoCSer.Reflection.InvokeMethodRef1<,>).MakeGenericType(typeof(valueType), typeof(DeSerializer)).GetMethod("get", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { methodInfo });
            }
            /// <summary>
            /// 反序列化
            /// </summary>
            /// <param name="deSerializer"></param>
            /// <param name="value"></param>
            public void DeSerialize(DeSerializer deSerializer, ref valueType value)
            {
                method(ref value, deSerializer);
            }
        }
    }
}
