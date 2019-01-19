using System;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据序列化成员 JSON 扩展配置
    /// </summary>
    public sealed class JsonMemberAttribute : SerializeMemberAttribute
    {
        /// <summary>
        /// 是否采用 JSON 混合序列化
        /// </summary>
        internal override bool GetIsJson
        {
            get { return true; }
        }
    }
}
