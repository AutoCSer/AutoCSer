using System;

namespace AutoCSer.Net.TcpStaticServer
{
    /// <summary>
    /// 远程调用链目标成员配置
    /// </summary>
    public partial class RemoteMemberAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
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
        /// <summary>
        /// 是否取消生成
        /// </summary>
        public bool IsCancel;
        /// <summary>
        /// 默认为 true 表示生成客户端远程成员，false 主要用于缓存推送的自定义处理
        /// </summary>
        public bool IsClientRemoteMember = true;
        /// <summary>
        /// 是否生成函数
        /// </summary>
        internal virtual bool IsMethod
        {
            get { return false; }
        }
    }
}
