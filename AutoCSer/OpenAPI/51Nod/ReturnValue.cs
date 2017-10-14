using System;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// API请求返回值
    /// </summary>
    public sealed class ReturnValue : IReturn
    {
        /// <summary>
        /// 返回值
        /// </summary>
        public ReturnValueCode Return;
        /// <summary>
        /// 数据是否有效
        /// </summary>
        public bool IsReturn
        {
            get { return Return.Code == (int)ReturnCode.Success; }
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message
        {
            get { return ((ReturnCode)Return.Code).ToString(); }
        }
    }
    /// <summary>
    /// API请求返回值
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public sealed partial class ReturnValue<valueType> : IReturn
    {
        /// <summary>
        /// 返回值
        /// </summary>
        public ReturnValueCode Return;
        /// <summary>
        /// 数据是否有效
        /// </summary>
        public bool IsReturn
        {
            get { return Return.Code == (int)ReturnCode.Success; }
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message
        {
            get { return ((ReturnCode)Return.Code).ToString(); }
        }
        /// <summary>
        /// 返回值
        /// </summary>
        public valueType Value
        {
            get { return Return.Value; }
        }
    }
}
