using System;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 序列化结果
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SerializeResult
    {
        /// <summary>
        /// XML 字符串
        /// </summary>
        public string Xml;
        /// <summary>
        /// 警告提示状态
        /// </summary>
        public SerializeWarning Warning;
        /// <summary>
        /// Xml 字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator string(SerializeResult value) { return value.Xml; }
    }
}
