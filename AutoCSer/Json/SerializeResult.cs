using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// 序列化结果
    /// </summary>
    public struct SerializeResult
    {
        /// <summary>
        /// JSON 字符串
        /// </summary>
        public string Json;
        /// <summary>
        /// 警告提示状态
        /// </summary>
        public SerializeWarning Warning;
        /// <summary>
        /// JSON 字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator string(SerializeResult value) { return value.Json; }
    }
}
