using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 提交数据类型
    /// </summary>
    public enum PostType : byte
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// JSON数据
        /// </summary>
        Json,
        /// <summary>
        /// 表单
        /// </summary>
        Form,
        /// <summary>
        /// 表单数据boundary
        /// </summary>
        FormData,
        /// <summary>
        /// XML数据
        /// </summary>
        Xml,
        /// <summary>
        /// 未知数据流
        /// </summary>
        Data,
    }
}
