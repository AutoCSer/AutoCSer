using System;

namespace AutoCSer.Json
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
        /// 缺少循环引用设置函数名称
        /// </summary>
        LessSetLoop,
        /// <summary>
        /// 缺少循环引用获取函数名称
        /// </summary>
        LessGetLoop,
        /// <summary>
        /// 成员位图类型不匹配
        /// </summary>
        MemberMap,
    }
}
