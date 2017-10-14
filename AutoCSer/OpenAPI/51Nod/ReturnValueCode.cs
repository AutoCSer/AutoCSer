using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// 返回值
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct ReturnValueCode
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int Code;
        /// <summary>
        /// 数据是否有效
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        public ReturnCode ReturnCode
        {
            get { return (ReturnCode)Code; }
            set { Code = (int)value; }
        }
    }
    /// <summary>
    /// API请求返回值
    /// </summary>
    public sealed partial class ReturnValue<valueType>
    {
        /// <summary>
        /// 返回值
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        public struct ReturnValueCode
        {
            /// <summary>
            /// 返回码
            /// </summary>
            public int Code;
            /// <summary>
            /// 数据是否有效
            /// </summary>
            [AutoCSer.Metadata.Ignore]
            public ReturnCode ReturnCode
            {
                get { return (ReturnCode)Code; }
                set { Code = (int)value; }
            }
            /// <summary>
            /// 返回值
            /// </summary>
            public valueType Value;
        }
    }
}
