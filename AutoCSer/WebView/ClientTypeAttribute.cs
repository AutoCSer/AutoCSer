using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.WebView
{
    /// <summary>
    /// 客户端视图绑定类型
    /// </summary>
    public class ClientTypeAttribute : Attribute
    {
        /// <summary>
        /// 默认绑定成员名称
        /// </summary>
        public const string DefaultMemberName = "Id";
        /// <summary>
        /// 默认客户端视图绑定类型
        /// </summary>
        internal static readonly ClientTypeAttribute Null = new ClientTypeAttribute();
        /// <summary>
        /// 客户端视图绑定类型名称（PrefixName 为 null 时有效）
        /// </summary>
        public string Name;
        /// <summary>
        /// 客户端视图绑定类型前缀
        /// </summary>
        public string PrefixName;
        /// <summary>
        /// 绑定成员名称,默认为Id
        /// </summary>
        public string MemberName = DefaultMemberName;
        /// <summary>
        /// 客户端视图绑定类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal string GetClientName(Type type)
        {
            if (PrefixName == null) return Name;
            return PrefixName + type.Name;
        }
    }
}
