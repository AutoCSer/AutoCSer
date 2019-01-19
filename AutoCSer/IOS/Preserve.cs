using System;

namespace AutoCSer.IOS
{
    /// <summary>
    /// 编译连接
    /// </summary>
    public sealed class Preserve : Attribute
    {
        /// <summary>
        /// 是否编译连接所有成员，作用于类型
        /// </summary>
        public bool AllMembers;
        /// <summary>
        /// 当类型被引用时才编译连接，作用于成员
        /// </summary>
        public bool Conditional;
    }
}
