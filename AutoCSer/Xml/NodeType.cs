using System;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 节点类型
    /// </summary>
    public enum NodeType : byte
    {
        /// <summary>
        /// 字符串
        /// </summary>
        String,
        /// <summary>
        /// 未解码字符串
        /// </summary>
        EncodeString,
        /// <summary>
        /// 未解码可修改字符串
        /// </summary>
        TempString,
        /// <summary>
        /// 子节点
        /// </summary>
        Node,
        /// <summary>
        /// 字符串解析失败
        /// </summary>
        ErrorString,
    }
}
