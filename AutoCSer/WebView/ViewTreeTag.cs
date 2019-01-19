using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// HTML模板树节点标识
    /// </summary>
    internal sealed class ViewTreeTag : IEquatable<ViewTreeTag>
    {
        /// <summary>
        /// 树节点标识类型
        /// </summary>
        internal ViewTreeTagType Type;
        /// <summary>
        /// 标识command
        /// </summary>
        internal SubString Command;
        /// <summary>
        /// 内容
        /// </summary>
        internal SubString Content;
        /// <summary>
        /// 是否已经回合
        /// </summary>
        internal bool IsRound;
        /// <summary>
        /// 判断树节点标识是否相等
        /// </summary>
        /// <param name="other">树节点标识</param>
        /// <returns>是否相等</returns>
        public bool Equals(ViewTreeTag other)
        {
            return other != null && other.Type == Type && other.Command.Equals(ref Command) && other.Content.Equals(ref Content);
        }
    }
}
