using System;

namespace AutoCSer.Xml
{
    /// <summary>
    /// XML 解析结果
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ParseResult
    {
        /// <summary>
        /// 成员位图
        /// </summary>
        public AutoCSer.Metadata.MemberMap MemberMap;
        /// <summary>
        /// XML 字符串
        /// </summary>
        public SubString Xml;
        /// <summary>
        /// 当前解析位置
        /// </summary>
        public int Index;
        /// <summary>
        /// 解析状态
        /// </summary>
        public ParseState State;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator bool(ParseResult value) { return value.State == ParseState.Success; }
    }
}
