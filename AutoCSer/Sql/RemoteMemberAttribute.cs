using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 远程成员配置
    /// </summary>
    public sealed partial class RemoteMemberAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        /// <summary>
        /// 名称类型
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// 直接连接
            /// </summary>
            Concat,
            /// <summary>
            /// 下划线连接
            /// </summary>
            Join,
            /// <summary>
            /// 仅名称
            /// </summary>
            OnlyName
        }
        /// <summary>
        /// 成员名称
        /// </summary>
        public string MemberName;
        /// <summary>
        /// 名称类型（用于成员缓存调用链）
        /// </summary>
        public Type NameType;
    }
}
