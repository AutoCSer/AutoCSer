using System;

namespace AutoCSer.Net.RemoteExpression
{
    /// <summary>
    /// 远程表达式成员配置
    /// </summary>
    public sealed class MemberAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        /// <summary>
        /// 自定义成员编号，用于表达式节点名称冲突问题，默认为 0 表示不编号
        /// </summary>
        public int MemberIdentity;
        /// <summary>
        /// 默认为 true 表示允许返回数据
        /// </summary>
        public bool IsReturn = true;
        /// <summary>
        /// 默认为 false 表示不生成远程表达式泛型实例化代码，否则表示如果该成员类型为泛型类型时实例化泛型远程表达式
        /// </summary>
        public bool IsGenericTypeInstantiation = false;
    }
}
