using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// API回调返回值
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    [StructLayout(LayoutKind.Auto)]
    public struct Callback<valueType>
    {
        /// <summary>
        /// 回调类型
        /// </summary>
        public CallbackType Type;
        /// <summary>
        /// 回调数据
        /// </summary>
        public valueType Value;
    }
}
