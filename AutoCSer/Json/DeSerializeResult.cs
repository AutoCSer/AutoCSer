using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 解析状态结果
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct DeSerializeResult
    {
        /// <summary>
        /// JSON 字符串
        /// </summary>
        public SubString Json;
        /// <summary>
        /// 成员位图
        /// </summary>
        internal AutoCSer.Metadata.MemberMap MemberMap;
        /// <summary>
        /// 当前解析位置
        /// </summary>
        public int Index;
        /// <summary>
        /// 解析状态
        /// </summary>
        public DeSerializeState State;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator bool(DeSerializeResult value) { return value.State == DeSerializeState.Success; }
    }
}
