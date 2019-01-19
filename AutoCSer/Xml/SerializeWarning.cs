using System;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 警告提示状态
    /// </summary>
    public enum SerializeWarning : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        None,
        /// <summary>
        /// 成员位图类型不匹配
        /// </summary>
        MemberMap,
    }
}
