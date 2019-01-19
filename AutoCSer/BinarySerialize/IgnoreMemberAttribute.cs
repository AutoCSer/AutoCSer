using System;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据序列化成员忽略配置
    /// </summary>
    public sealed class IgnoreMemberAttribute : SerializeMemberAttribute
    {
        /// <summary>
        /// 禁止当前安装
        /// </summary>
        internal override bool GetIsIgnoreCurrent
        {
            get { return true; }
        }
    }
}
